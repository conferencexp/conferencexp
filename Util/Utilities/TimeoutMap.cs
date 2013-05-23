using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;


namespace edu.washington.cs.cct.cxp.utilities
{
    /// <summary>
    /// A hash-table-like data structure that associates a time-out with each item.
    /// Note: the mapping not automatically removed upon a timeout.
    /// 
    /// The current implementation is not very scalable, because it does a linear scan of each
    /// entry for each clock tick.
    /// 
    /// </summary>
    /// 

    public delegate void TimeoutMapCallback<K,V> (TimeoutMap<K,V> map, K entry);

    public class TimeoutMap<K, V> : IDisposable
    {

        private readonly IDictionary<K, MapValue<V>> dictionary = new Dictionary<K, MapValue<V>>();

        public static readonly long TIMER_INTERVAL = 500; // ms

        private static readonly System.Timers.Timer timer;

        static TimeoutMap()
        {
            timer = new Timer();
            timer.Enabled = true;
            timer.Interval = TIMER_INTERVAL;
        }


        public  event TimeoutMapCallback<K,V> TimeoutEvent;

        public TimeoutMap()
        {
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
        }

       
        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            DateTime now = DateTime.Now;
            ICollection<K> expired = new List<K>();
            lock (this)
            {
                foreach (KeyValuePair<K, MapValue<V>> entry in dictionary)
                {
                    if (now.CompareTo(entry.Value.Timeout) > 0)
                    {
                        expired.Add(entry.Key);
                    }
                }
            }

            // issue the callbacks; this can't be done with the lock, for fear of deadlock
            foreach (K key in expired)
            {
                TimeoutEvent(this,key);
            }
        }

        public V Put(K key, V value,long timeoutDurationInMilleseconds)
        {
            lock (this)
            {
                MapValue<V> mapValue = new MapValue<V>(value, DateTime.Now.AddMilliseconds(timeoutDurationInMilleseconds));
                MapValue<V> oldValue = (dictionary[key] = mapValue);
                if (oldValue == null)
                    return default(V);
                else return oldValue.Value;
            }
        }

        public V Get(K key)
        {
            lock (this)
            {
                MapValue<V> value = null;
                dictionary.TryGetValue(key, out value);
                if (value == null)
                    return default(V);
                else return value.Value;
            }
        }

        public bool Remove(K key)
        {
            lock (this)
            {
                return dictionary.Remove(key);
            }
        }

        public bool ContainsKey(K key)
        {
            lock (this)
            {
                return dictionary.ContainsKey(key);
            }
        }


        internal class MapValue<U>
        {
            U value;

            public U Value
            {
                get { return this.value; }
                set { this.value = value; }
            }
            DateTime timeout;

            public DateTime Timeout
            {
                get { return timeout; }
                set { timeout = value; }
            }

            public MapValue(U value, DateTime expiration)
            {
                this.timeout = expiration;
                this.value = value;
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            timer.Elapsed -= timer_Elapsed;
        }

        #endregion
    }


 
}

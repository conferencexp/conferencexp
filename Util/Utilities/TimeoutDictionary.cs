using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

namespace edu.washington.cs.cct.cxp.utilities
{
    /// <summary>
    /// A hash table (dictionary) data structure, which includes a timeout mechanism for expiring old state.
    /// This is implemented by keeping two internal dictionaries: one for "old" state and one for "new" state.
    /// Periodically, the new state becomes old state, and the old old state is discarded.
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="V"></typeparam>
    public class TimeoutDictionary<K,V> : IDictionary<K,V>
    {
        private readonly Timer timer;

        private IDictionary<K, V> oldState = new Dictionary<K, V>();
        private IDictionary<K, V> newState = new Dictionary<K, V>();

        /// <summary>
        /// The timeout specifies the minimum amount of time that a mapping will reside in the
        /// dictionary.  In practice, the item will persist between [T,2T].
        /// </summary>
        /// <param name="timeoutInMilleseconds"></param>
        public TimeoutDictionary(long timeoutInMilleseconds)
        {
            timer = new Timer();
            timer.Enabled = true;
            timer.Interval = timeoutInMilleseconds;

            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
        }

        /// <summary>
        /// Discard old state; demote new state to "old state".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            lock (this)
            {
                oldState = newState;
                newState = new Dictionary<K, V>();
            }
        }

        #region IDictionary<K,V> Members

        public void Add(K key, V value)
        {
            newState[key] = value;
            oldState.Remove(key);
        }

        public bool ContainsKey(K key)
        {
            return newState.ContainsKey(key) || oldState.ContainsKey(key);
        }

        public ICollection<K> Keys
        {
            get
            {
                lock (this)
                {
                    // we maintain the invariant that key appears in at most one of the dictionaries.
                    // so, we don't need to check for duplicates here.
                    List<K> list = new List<K>(newState.Keys);
                    list.AddRange(oldState.Keys);
      
                    return list;
                }
            }
        }

        public bool Remove(K key)
        {
            return newState.Remove(key) || oldState.Remove(key);
        }

        public bool TryGetValue(K key, out V value)
        {
            bool contains = newState.TryGetValue(key, out value);
            if (!contains)
                contains = oldState.TryGetValue(key, out value);
            return contains;
        }

        public ICollection<V> Values
        {
            get 
            {
                lock (this)
                {
                    List<V> list = new List<V>(newState.Values);

                    // ugly: manually check for duplicates; C# does not have a Set data structure...
                    foreach (V v in oldState.Values)
                    {
                        if (! list.Contains(v))
                            list.Add(v);
                    }
                        

                    return list;
                }
            }
        }

        public V this[K key]
        {
            get
            {
                if (newState.ContainsKey(key))
                    return newState[key];
                else return oldState[key];
            }
            set
            {
                // maintain a single copy of each mapping
                newState[key] = value;
                oldState.Remove(key);
            }
        }

        #endregion

        #region ICollection<KeyValuePair<K,V>> Members

        public void Add(KeyValuePair<K, V> item)
        {
            newState.Add(item);
        }

        public void Clear()
        {
            lock (this)
            {
                oldState.Clear();
                newState.Clear();
            }
        }

        public bool Contains(KeyValuePair<K, V> item)
        {
            return newState.Contains(item) || oldState.Contains(item);
        }

        public void CopyTo(KeyValuePair<K, V>[] array, int arrayIndex)
        {
            lock (this)
            {
                newState.CopyTo(array, arrayIndex);
                oldState.CopyTo(array, arrayIndex + newState.Count);
            }
        }

        public int Count
        {
            get 
            {
                lock (this)
                {
                    return oldState.Count + newState.Count;
                }
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public bool Remove(KeyValuePair<K, V> item)
        {
            bool oldB = oldState.Remove(item);
            bool newB = newState.Remove(item);

            return (oldB | newB);
        }

        #endregion

        #region IEnumerable<KeyValuePair<K,V>> Members

        public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
        {
            ICollection<K> keys = this.Keys;
            IList<KeyValuePair<K, V>> list = new List<KeyValuePair<K, V>>();

            foreach (K key in keys)
            {
                KeyValuePair<K, V> pair = new KeyValuePair<K, V>(key, this[key]);
                list.Add(pair);
            }
            
            return list.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }

    public class KeyValueComparator<K,V> : Comparer<KeyValuePair<K,V>>
    {
        public override int Compare(KeyValuePair<K, V> x, KeyValuePair<K, V> y)
        {
            String xString = x.Key.ToString();
            String yString = y.Key.ToString();
            return xString.CompareTo(yString);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace edu.washington.cs.cct.cxp.utilities
{
    class TimeoutMapTest
    {
        public static void Main()
        {
            TimeoutMap<String, String> map = new TimeoutMap<string, string>();

            map.TimeoutEvent +=new TimeoutMapCallback<String,String>(map_TimeoutEvent);


            Console.Out.WriteLine("Current time: " + DateTime.Now);

            map.Put("cat", "meow", 2000);
            map.Put("dog", "bark", 5000);
            map.Put("pig", "oink", 10000);

            while (true)
            {
                Thread.Sleep(5000);
            }
        }

        static void map_TimeoutEvent(TimeoutMap<String,String> map,string entry)
        {
            Console.Out.WriteLine("Key has expired: {0} @  {1}" ,entry,DateTime.Now);
            map.Remove(entry);

        }
    }
}

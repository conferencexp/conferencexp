using System;
using System.Collections;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;


namespace MSR.LST
{
    /// <summary>
    /// A class for tracking memory usage between call points
    /// It works like a stack - push when you want to start a reading
    /// and pop when you want to compare the difference in memory
    /// The methods are static so that a class instance doesn't have to be
    /// global or passed around - not thread safe
    /// 
    /// There were 2 models available, the second one yielded more accurate results...
    /// 
    /// Model 1
    /// Push - Read Memory, New Alloc, Push
    /// Pop  - Pop, Del Alloc, Read Memory
    /// 
    /// Model 2
    /// Push - New Alloc, Push, Read Memory
    /// Pop  - Read Memory, Pop, Del Alloc
    /// </summary>
    public class MemCheck
    {
        // A class for holding the allocation number and memory reading
        // Allocation number can be helpful for tracking down "which" alloc leaked
        private class Alloc
        {
            public long count = 0;
            public long mem = 0;
            public string msg = null;

            public Alloc(long count, long mem, string msg)
            {
                this.count = count;
                this.mem = mem;
                this.msg = msg;
            }
        }

        // Initial capacity == number of memory reads allowed before stack grows
        // and readings become untrustworthy
        private static Stack mem = new Stack(1000);
        private static long count = 0;           // Which allocation, forward incrementing counter
        private static bool check = false;       // Don't run this code by default
        
        //  To avoid allocating memory with a local
        private static long end = 0;             // End memory reading

        static MemCheck()
        {
            // Read configuration data to see if we should do the checks
            if (ConfigurationManager.AppSettings[AppConfig.LST_MemCheck] != null)
            {
                check = bool.Parse(ConfigurationManager.AppSettings[AppConfig.LST_MemCheck]);
            }

            // This works around some kind of hokiness of 4K memory
            // I could not find where the 4K was coming from nor find a way
            // clean it up, but this workaround seems to "fix it"
            if(check)
            {
                Push(null);
                Pop();
            }
        }

        // New Alloc, Push, read memory
        public static void Push(string msg)
        {
            if(check)
            {
                Alloc alloc = new Alloc(++count, 0, msg);
                mem.Push(alloc);
                alloc.mem = GC.GetTotalMemory(true);
                Console.WriteLine(string.Format(CultureInfo.CurrentCulture, "<{0}, {1}>, Start <{2}>", 
                    alloc.count, msg, alloc.mem));
            }
        }

        // Pop, del Alloc, read memory
        public static void Pop()
        {
            if(check)
            {
                end = GC.GetTotalMemory(true);

                Alloc alloc = (Alloc)mem.Pop();
                long count = alloc.count;
                long start = alloc.mem;
                string msg = alloc.msg;

                // Log it
                Console.WriteLine(string.Format(CultureInfo.CurrentCulture, "<{0}, {1}>, Start <{2}>, End <{3}>, Diff <{4}>", 
                    count, msg, start, end, end - start));
            }
        }
    }
}

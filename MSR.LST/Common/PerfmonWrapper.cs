using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;

using Microsoft.Win32;


namespace MSR.LST
{
    /// <FileSumamry>
    /// These two classes provide the functionality necessary to make doing performance counters extremely easy.
    /// See MSR.LST.Rtp.PerformanceCounters.cs for a good example of how to override these classes to create perf counters.
    /// </FileSummary>

    #region BasePCInstaller
    /// <summary>
    /// Used by the Installer to (un)install the performance counters
    /// It forwards the real work to BasePC
    /// </summary>
    public abstract class BasePCInstaller
    {
        public static void Install(Type[] counterTypes)
        {
            foreach(Type counterType in counterTypes)
            {
                MethodInfo mi = counterType.GetMethod("Install", BindingFlags.Static | BindingFlags.Public);
                mi.Invoke(null, new object[]{});
            }

            // The resources have to be released in order to use them in the 
            // same process that installed them
            PerformanceCounter.CloseSharedResources();
        }

        public static void Uninstall(Type[] counterTypes)
        {
            foreach(Type counterType in counterTypes)
            {
                MethodInfo mi = counterType.GetMethod("Uninstall", BindingFlags.Static | BindingFlags.Public);
                mi.Invoke(null, new object[]{});
            }
        }
    }    
    #endregion

    #region BasePC
    /// <summary>
    /// This is the base class that all of the wrappers below inherit from, and provides the
    /// core functionality of (un)installing and creating/removing performance counters.
    /// </summary>
    public abstract class BasePC
    {
        #region Members

        private string instanceName;
        private string categoryName;
        private string[] counterNames;
        private PerformanceCounterWrapper[] pcs;
        
        #endregion Members

        #region Constructors
        /// <summary>
        /// Called by instances that plan to actually log perf data
        /// </summary>
        public BasePC(string categoryName, string[] counterNames, string instanceName)
        {
            this.categoryName = categoryName;
            this.counterNames = counterNames;

            // Make sure instanceName is unique
            UniquifyInstanceName(ref instanceName);
            this.instanceName = instanceName;

            // Create the counters and Initialize them
            pcs = new PerformanceCounterWrapper[counterNames.Length];

            for(int i = 0; i < pcs.Length; i++)
            {
                pcs[i] = new PerformanceCounterWrapper(categoryName, counterNames[i], instanceName, false);
            }
        }

        #endregion Constructors

        #region Properties
        
        /// <summary>
        /// Update a performance counter
        /// </summary>
        protected long this [int index]
        {
            get
            {
                return pcs[index].RawValue;
            }
            set
            {
                pcs[index].RawValue = value;
            }
        }

        public string Name
        {
            get
            {
                return this.instanceName;
            }
        }

        #endregion Indexer

        #region Methods

        public static void Install(string categoryName, string[] counterNames)
        {
            if(!PerformanceCounterCategory.Exists(categoryName))
            {
                CounterCreationData[] ccd = new CounterCreationData[counterNames.Length];

                for(int i = 0; i < counterNames.Length; i++)
                {
                    ccd[i] = new CounterCreationData(counterNames[i], string.Empty, 
                        PerformanceCounterType.NumberOfItems32);
                }

                PerformanceCounterCategory.Create(categoryName, string.Empty, PerformanceCounterCategoryType.Unknown,
                    new CounterCreationDataCollection(ccd));
            }
        }

        public static void Uninstall(string categoryName)
        {
            if(PerformanceCounterCategory.Exists(categoryName))
            {
                PerformanceCounterCategory.Delete(categoryName);
            }
        }
        
        public void Dispose()
        {
            if(pcs != null)
            {
                for(int i = 0; i < pcs.Length; i++)
                {
                    if(pcs[i] != null)
                    {
                        pcs[i].RemoveInstance();
                        pcs[i] = null;
                    }
                }

                pcs = null;
            }
        }

        private void UniquifyInstanceName(ref string instanceName)
        {
            // Through experimentation it has been learned that performance counter instances
            // with names greater than 64 characters use a shared counter.  We have not
            // investigated how to use a shared counter, so limit to 64 characters for now
            //
            // We trim to 60 in order to allow space to uniquify the name _NNN 
            // (up to 1000 instances of the same name)
            if (instanceName.Length > 60)
            {
                instanceName = instanceName.Substring(0, 60);
            }

            // Check to see if the instanceName already exists and choose a different one
            PerformanceCounterCategory pcc =   new PerformanceCounterCategory(categoryName);
            pcc.CategoryName = categoryName;

            // andrew: this was throwing an InvalidOperationException, for reasons I was 
            // not able to understand...

            //if(pcc.InstanceExists(instanceName))
            //{
            //    int i = 0;
            //    while(pcc.InstanceExists(instanceName + "_" + (++i)));

            //    instanceName += "_" + i;
            //}        
        }

        #endregion Methods

        #region Exceptions

        public class BasePCException : ApplicationException
        {
            public BasePCException(string message) : base(message) {}
        }

        #endregion Exceptions

        #region class PerformanceCounterWrapper
        // This wrapper class was created in order to simplify the code using
        // performance counters, as well as provide a central location for adding
        // enhancements or debug code
        [ComVisible(false)]
        public class PerformanceCounterWrapper
        {
            private PerformanceCounter pc = null;
            private static LstEventLog eventLog = new LstEventLog("ConferenceAPI", ".", "PerformanceCounter");

            // Check if user has read access to the Perflib registry key, which is required in order to read
            // performance counters on Win2K3, Vista, and above
            public static bool HasUserPermissions()
            {
                if ((Environment.OSVersion.Version.Major == 5 &&
                     Environment.OSVersion.Version.Minor >= 2) ||
                     Environment.OSVersion.Version.Major >= 6) // Win2K3 or Vista and above (Longhorn Server, etc.)
                {
                    try
                    {
                        using (RegistryKey rk = Registry.LocalMachine.OpenSubKey(
                            @"Software\Microsoft\Windows NT\CurrentVersion\Perflib"))
                        {
                            return true;
                        }
                    }
                    catch (System.Security.SecurityException)
                    {
                        return false;
                    }
                }
                else if (Environment.OSVersion.Version.Major == 5 &&
                         Environment.OSVersion.Version.Minor == 1) // WinXP
                {
                    return true;
                }
                else // Win2K and other down level OSes
                {
                    throw new ApplicationException(Strings.OSVersion);
                }
            }

            public PerformanceCounterWrapper(string categoryName, string counterName, string instanceName, 
                bool readOnly)
            {
                try
                {
                    pc = new PerformanceCounter(categoryName, counterName, instanceName, readOnly);
                }
                    // Many times an InvalidOperationException gets thrown -- this seems to be a CLF bug that is fixed in 1.0 SR 2
                catch (InvalidOperationException){}
                catch(Exception e)
                {
                    if(eventLog != null)
                    {
                        eventLog.WriteEntry(e.ToString(), EventLogEntryType.Error, 100);
                    }
                }
            }

            public long RawValue
            {
                get
                {
                    try
                    {
                        return pc.RawValue;
                    }
                    catch(NullReferenceException){}
                    catch(Exception e)
                    {
                        eventLog.WriteEntry(e.ToString(), EventLogEntryType.Error, 100);
                    }

                    // Return a bogus value
                    return -1;
                }
                set
                {
                    try
                    {
                        pc.RawValue = value;
                    }
                    catch(NullReferenceException){}
                    catch(Exception e)
                    {
                        eventLog.WriteEntry(e.ToString(), EventLogEntryType.Error, 100);
                    }
                }
            }

            public string InstanceName
            {
                get
                {
                    try
                    {
                        return pc.InstanceName;
                    }
                    catch(NullReferenceException){}
                    catch(Exception e)
                    {
                        eventLog.WriteEntry(e.ToString(), EventLogEntryType.Error, 100);
                    }

                    return String.Empty;
                }
                set
                {
                    try
                    {
                        pc.InstanceName = value;
                    }
                    catch(NullReferenceException){}
                    catch(Exception e)
                    {
                        eventLog.WriteEntry(e.ToString(), EventLogEntryType.Error, 100);
                    }
                }
            }

            public void RemoveInstance()
            {
                try
                {
                    pc.RemoveInstance();
                }
                catch(NullReferenceException){}
                catch(Exception e)
                {
                    eventLog.WriteEntry(e.ToString(), EventLogEntryType.Error, 100);
                }
            }
        }    
        #endregion
    }
    #endregion BasePC
}

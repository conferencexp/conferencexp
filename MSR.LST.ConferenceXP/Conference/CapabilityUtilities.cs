using System;
using System.Collections;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

using Microsoft.Win32;


namespace MSR.LST.ConferenceXP
{
    public class CapabilityViewerClassHashtable : DictionaryBase
    {
        public CapabilityViewerClassHashtable(int length) : base() {}
        public CapabilityViewerClassHashtable() : base() {}
        
        public ICollection Keys  
        {
            get  
            {
                return( Dictionary.Keys );
            }
        }

        public ICollection Values  
        {
            get  
            {
                return( Dictionary.Values );
            }
        }

        public object Clone()
        {
            CapabilityViewerClassHashtable cvcHT = new CapabilityViewerClassHashtable();

            foreach( DictionaryEntry de in Dictionary )
                cvcHT.Add( (MSR.LST.ConferenceXP.PayloadType)de.Key, (Type)de.Value);

            return cvcHT;
        }

        public Type this [ MSR.LST.ConferenceXP.PayloadType payloadType ]
        {
            get
            {
                return (Type) Dictionary[payloadType];
            }
            set
            {
                if ( value.GetType() != Type.GetType("Type") )
                {
                    throw new ArgumentException(Strings.ValueMustBeAType, Strings.Value);
                }

                Dictionary[payloadType] = value;
            }
        }

        public void Add ( MSR.LST.ConferenceXP.PayloadType payloadType, Type type )
        {
            Dictionary.Add( payloadType, type );
        }

        public bool Contains ( MSR.LST.ConferenceXP.PayloadType payloadType )
        {
            return Dictionary.Contains ( payloadType );
        }

        public bool ContainsKey ( MSR.LST.ConferenceXP.PayloadType payloadType )
        {
            return Dictionary.Contains ( payloadType );
        }

        public void Remove ( MSR.LST.ConferenceXP.PayloadType payloadType )
        {
            Dictionary.Remove ( payloadType );
        }
    }

    public class CapabilitySenderClassHashtable : DictionaryBase
    {
        public CapabilitySenderClassHashtable(int length) : base() {}
        public CapabilitySenderClassHashtable() : base() {}
        
        public ICollection Keys  
        {
            get  
            {
                return( Dictionary.Keys );
            }
        }

        public ICollection Values  
        {
            get  
            {
                return( Dictionary.Values );
            }
        }

        public object Clone()
        {
            CapabilitySenderClassHashtable cscHT = new CapabilitySenderClassHashtable();

            foreach( DictionaryEntry de in Dictionary )
                cscHT.Add((string)de.Key, (Type)de.Value);

            return cscHT;
        }

        public Type this [ string name ]
        {
            get
            {
                return (Type) Dictionary[name];
            }
            set
            {
                if ( value.GetType() != Type.GetType("Type") )
                {
                    throw new ArgumentException( Strings.ValueMustBeAType, Strings.Value );
                }

                Dictionary[name] = value;
            }
        }

        public void Add ( string name, Type type )
        {
            Dictionary.Add( name, type );
        }

        public bool Contains ( string name )
        {
            return Dictionary.Contains ( name );
        }

        public bool ContainsKey ( string name )
        {
            return Dictionary.Contains ( name );
        }

        public void Remove ( string name )
        {
            Dictionary.Remove ( name );
        }
    }


    public class CapabilityUtilities
    {
        private static ConferenceEventLog eventLog = new ConferenceEventLog(ConferenceEventLog.Source.Conference);

        private CapabilityUtilities() {}

        /// <summary>
        /// Dynamically load assemblies into the API based on what interfaces they implement.  Used to allow
        /// 3rd parties to add custom CapabilitySenders and CapabilityViewers without recompiling the application
        /// or changing its source code.
        /// </summary>
        /// <remarks>
        /// Because this section of code frequently excepts when a developer is creating a new Capability and does something wrong,
        /// we both write to the event log and display a message box so that the developer can find the error quickly.  In experience,
        /// just writing to the event log causes failures that new developers don't easily find.
        /// </remarks>
        public static void InitializeCapabilityClasses(out CapabilityViewerClassHashtable capabilityViewerClasses, 
                                                       out CapabilitySenderClassHashtable capabilitySenderClasses)
        {
            // Initialize outbound parameters
            capabilityViewerClasses = new CapabilityViewerClassHashtable();
            capabilitySenderClasses = new CapabilitySenderClassHashtable();

            #region Default RTDocs viewer from app.config

            // Before starting to iterate through .dll's, check to see if a default 
            // RTDocument CapabilityViewer is identified in app.config. If one is 
            // found in app.config, and there is not already a default stored in 
            // the registry, then set this one as the default.
            RegistryKey rtdocskey = Registry.CurrentUser.CreateSubKey("SOFTWARE\\Microsoft Research\\ConferenceXP\\Client\\" +
                System.Reflection.Assembly.GetEntryAssembly().CodeBase + "\\CapabilityViewers");

            string appconfigEntry;
            if ((appconfigEntry = ConfigurationManager.AppSettings[AppConfig.CXP_RTDocumentViewerDefault]) != null)
            {
                // We only use the app.config setting if there is not already an entry marked as default
                // in the registry
                string[] names = rtdocskey.GetValueNames();
                bool defExists = false;
                foreach (string key in names)
                {
                    object val = rtdocskey.GetValue(key);
                    if (val.ToString() == "default")
                    {
                        defExists = true;
                        break;
                    }
                }

                if (!defExists)
                    rtdocskey.SetValue(appconfigEntry, "default");
            }

            #endregion Default RTDocs viewer from app.config

            // Get the directory of the executing .exe
            DirectoryInfo diExe = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory;

            // Find all the DLLs in that directory, looping through them all
            FileInfo[] fis = diExe.GetFiles("*.dll");
            foreach (FileInfo fiDll in fis)
            {
                try
                {
                    // Load each DLL into memory, getting a list of all root types (including classes and interfaces)
                    Type[] types = Assembly.LoadFrom(fiDll.FullName).GetTypes();

                    foreach (Type type in types)
                    {
                        // You can't instantiate an abstract class
                        if (type.IsAbstract)
                            continue;

                        // If the class defines "IsRunable" (inherited through Capability), run it and check
                        // that this class can be run on the current machine.  If not, skip the class.
                        if (type.IsSubclassOf(typeof(MSR.LST.ConferenceXP.Capability)))
                        {
                            PropertyInfo runableProp = type.GetProperty("IsRunable", BindingFlags.Static | BindingFlags.Public);
                            if (runableProp != null) // if the dll was compiled off of old code, it won't have "IsRunable"
                            {
                                MethodInfo getRunableMethod = runableProp.GetGetMethod();
                                object returnObj = getRunableMethod.Invoke(null, null);
                                if (!(returnObj is bool) || (((bool)returnObj) == false))
                                {
                                    continue; // Skip this type becuase its own method says it isn't runable.
                                }
                            }
                        }

                        // Check required capability attributes
                        Type iCV = type.GetInterface("MSR.LST.ConferenceXP.ICapabilityViewer");
                        Type iCS = type.GetInterface("MSR.LST.ConferenceXP.ICapabilitySender");

                        Capability.PayloadTypeAttribute pt = null;
                        Capability.NameAttribute capabilityName = null;

                        if (iCV != null || iCS != null)
                        {
                            pt = (Capability.PayloadTypeAttribute)Attribute.GetCustomAttribute(type,
                                typeof(Capability.PayloadTypeAttribute));

                            capabilityName = (Capability.NameAttribute)Attribute.GetCustomAttribute(type,
                                typeof(Capability.NameAttribute));

                            // Without a payload type and a name, it is invalidly constructed
                            if (null == pt || null == capabilityName)
                                continue;
                        }

                        // Does it have an ICapabilityViewer interface?
                        if (iCV != null)
                        {
                            AddCapabilityViewerClass(fiDll.Name, type, capabilityName.Name,
                                capabilityViewerClasses, pt, rtdocskey);
                        }

                        // Does it have an ICapabilitySender interface?
                        if (iCS != null)
                        {
                            AddCapabilitySenderClass(fiDll.Name, type, capabilityName.Name,
                                capabilitySenderClasses);
                        }
                    } // end foreach tBaseType

                }
                // Some DLLs in the directory may not be .NET Assemblies (like msvcrt70.dll), and they will throw a BadImageFormatException here if not.
                catch (BadImageFormatException) {}
                // Perhaps a bad or old .Net Dll is here, ignore it.  This was firing for Shockwave Flash COM interop assemblies
                catch (TypeInitializationException) {}
                // Trying to reload an already loaded Dll, occurs for Conference.Dll, so just continue on...
                catch (ReflectionTypeLoadException) {}
            }

            rtdocskey.Flush();
        }

        private static void AddCapabilitySenderClass(string fileName, Type type, string capName,
            CapabilitySenderClassHashtable capabilitySenderClasses)
        {
            try
            {
                if (!capabilitySenderClasses.ContainsKey(capName))
                {
                    capabilitySenderClasses.Add(capName, type);
                }
                else
                {
                    string msg = string.Format(CultureInfo.CurrentCulture, Strings.FoundDuplicateCapabilitySender, 
                        capName, fileName);
                    eventLog.WriteEntry(msg, EventLogEntryType.Error, 23);
                    RtlAwareMessageBox.Show(null, msg, Strings.ErrorInitializingCapabilityClasses, 
                        MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 
                        (MessageBoxOptions)0);
                }
            }
            catch (Exception e)
            {
                eventLog.WriteEntry(string.Format(CultureInfo.CurrentCulture, Strings.AccessingFile, fileName, 
                    e.ToString()), EventLogEntryType.Error, 99);
                RtlAwareMessageBox.Show(null, string.Format(CultureInfo.CurrentCulture, Strings.AccessingFile, 
                    fileName, e.ToString()), Strings.ErrorInitializingCapabilitySender, MessageBoxButtons.OK, 
                    MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
            }
        }

        private static void AddCapabilityViewerClass(string fileName, Type type, string capName, 
            CapabilityViewerClassHashtable capabilityViewerClasses, 
            Capability.PayloadTypeAttribute pt, RegistryKey rtdocskey)
        {
            try
            {
                // Handle RTDocs as a special case. 
                // We checked for a default setting in app.config earlier.
                // If we haven't loaded an RTDocs viewer yet, check the registry
                // to see if the current one is the default viewer.  If it is 
                // load it.  If it isn't in the registry, add it as non-default.
                // If there are no entries in the registry, the first viewer wins.
                // It is possible to get into a state where the default viewer
                // in the registry is uninstalled from the machine, in which case
                // we are in trouble, because they will all be non-default.
                if (pt.PayloadType.ToString() == "RTDocument")
                {
                    // No RTDocs capability viewer has been loaded
                    if (!capabilityViewerClasses.ContainsKey(pt.PayloadType))
                    {
                        // See if this one is listed as the default in the registry
                        if (rtdocskey.ValueCount > 0)
                        {
                            object cpentry = rtdocskey.GetValue(capName);
                            if (cpentry != null) // There is a registry entry for this capability
                            {
                                if (cpentry.ToString() == "default")
                                {
                                    capabilityViewerClasses.Add(pt.PayloadType, type);
                                }
                            }
                            else // No registry entry for this capability
                            {
                                // add to registry, but don't load
                                rtdocskey.SetValue(capName, "non-default");
                            }
                        }
                        else // No entries in registry. First one wins.
                        {
                            rtdocskey.SetValue(capName, "default");
                            capabilityViewerClasses.Add(pt.PayloadType, type);
                        }
                    }
                    else // An RTDocs capability viewer has already been loaded
                    {
                        // Mark the current one as non-default in the registry
                        rtdocskey.SetValue(capName, "non-default");
                    }
                }
                else
                {
                    if (!capabilityViewerClasses.ContainsKey(pt.PayloadType))
                    {
                        capabilityViewerClasses.Add(pt.PayloadType, type);
                    }
                    else
                    {
                        string msg = string.Format(CultureInfo.CurrentCulture, Strings.MultipleCapabilityViewersExist, 
                            pt.PayloadType, capabilityViewerClasses[pt.PayloadType].Name, capName);

                        RtlAwareMessageBox.Show(null, msg, Strings.DuplicateCapabilityViewersDetected, 
                            MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, 
                            (MessageBoxOptions)0);
                    }
                }
            }
            catch (Exception e)
            {
                string msg = string.Format(CultureInfo.CurrentCulture, Strings.AccessingFile, fileName, e.ToString());

                eventLog.WriteEntry(msg, EventLogEntryType.Error, 99);

                RtlAwareMessageBox.Show(null, msg, Strings.ErrorInitializingCapabilityViewer, MessageBoxButtons.OK, 
                    MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
            }
        }
    }
}
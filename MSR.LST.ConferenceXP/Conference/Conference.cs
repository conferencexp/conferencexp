using System;
using System.Collections;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

using Microsoft.Win32;

using MSR.LST.Net.Rtp;

using ReflectorRegistrarClient;
using System.Collections.Generic;


namespace MSR.LST.ConferenceXP
{
    /// <summary>
    /// The top level class of the Conference API for ConferenceXP.
    /// 
    /// Conference is a static class which is used to find Venues &amp; then collaborate within them.
    /// </summary>
    public abstract class Conference
    {
        #region Members

        private static VenueBroker venues = new VenueBroker();
        private static RtpSession rtpSession = null;
        private static ConferenceEventLog eventLog = new ConferenceEventLog(ConferenceEventLog.Source.Conference);
        private static Participant localParticipant = null;
        private static string location = null;
        private static bool autoPlayRemote = false;
        private static bool autoPlayLocal = false;
        private static int venueServiceTimeout = 0;
        private static AutoPositionMode autoPosition = AutoPositionMode.Tiled;
        private static bool logActivity = false;
        private static CapabilityViewerClassHashtable capabilityViewerClasses = null; // Keyed on PayloadType for use looking up capabilityViewer for an incoming RtpStream
        private static CapabilitySenderClassHashtable capabilitySenderClasses = null; // Keyed on CapabilityProperties.Name to allow multiple types of CapabilitySenders for one PayloadType
        private static bool createParticipantUIShowed = false;
        
        private static IVenue activeVenue = null;
        private static ICapabilityHashtable capabilities = new ICapabilityHashtable(); // Indexed by capability.ID
        private static ArrayList capabilityForms = new ArrayList();
        private static DiagnosticMonitor diagnosticMonitor = null;
        
        /// <summary>
        /// CallingForm, if set by the calling application, is used to marshal all events onto the Form
        /// thread, making it much easier to do things like add a ListViewItem to a ListView from the event's
        /// handler.
        /// </summary>
        /// <remarks>
        /// Save this so we can call Form.Invoke on the events to get them back on the right thread
        /// Important: Do not confuse: CallingForm is not the capability form but the main executable form
        /// </remarks>
        private static Form callingForm = null;
       

        internal static IParticipantHashtable participants = new IParticipantHashtable(); // Indexed by Identifier
        internal static ICapabilityHashtable capabilityViewers = new ICapabilityHashtable();   // Indexed by capability.ID
        internal static ICapabilityHashtable capabilitySenders = new ICapabilityHashtable(); // Indexed by capability.ID

        /// <summary>
        /// The reflector DNS resolvable address.
        /// </summary>
        public static string ReflectorAddress = "0.0.0.0";

        /// <summary>
        /// The port used for reflector-based RTP communiation.  
        /// This is also the port used for Join requests.
        /// This should be set by FMain in CXPClient.
        /// </summary>
        public static int ReflectorRtpPort = 7004;

        /// <summary>
        /// Reflector enable/disable flag
        /// </summary>
        public static bool ReflectorEnabled = false;

        /// <summary>
        /// Diagnostic service is configured and enabled
        /// </summary>
        public static bool DiagnosticsEnabled = false;

        /// <summary>
        /// Base Uri for retrieval of Diagnostic information
        /// </summary>
        public static Uri DiagnosticsWebService = null;

        /// <summary>
        /// Persisted window positions
        /// </summary>
        private static Dictionary<string, Rectangle> windowPersistenceData;
        
        /// <summary>
        /// Original window placements: used to tell if a window was moved by the user
        /// </summary>
        private static Dictionary<string, Rectangle> originalWindowPositions;

        #endregion Members
        
        #region Constructors
        
        static Conference()
        {
            CheckInstalled();
            CapabilityUtilities.InitializeCapabilityClasses(out capabilityViewerClasses, out capabilitySenderClasses);
            SetConfiguration();
            GetLocalParticipant();
            originalWindowPositions = new Dictionary<string, Rectangle>();
        }
        
        #endregion
        
        #region Public

        public static VenueBroker VenueServiceWrapper
        {
            get
            {
                return venues;
            }
        }

        public static bool PersistWindowPositions;

        /// <summary>
        /// Conference needs to know when the venue service changes in order to
        /// update the local participant to the one from that venue service.
        /// </summary>
        /// <param name="url"></param>
        public static void SetVenueServiceUrl(string url)
        {
            Conference.VenueServiceWrapper.VenueServiceUrl = url;
            GetLocalParticipant();
        }

        public static IParticipant[] Participants
        {
            get
            {
                ArrayList al = new ArrayList(participants.Values);
                return (IParticipant[])al.ToArray(typeof(IParticipant));
            }
        }

        public static RtpSession RtpSession
        {
            get
            {
                return rtpSession;
            }
        }

        public static Form CallingForm
        {
            set{callingForm = value;}
        }

        /// <summary>
        /// Check if form invoke is requiered and invoke the form on the UI thread
        /// (form.Invoke if not on UI thread, or on the current thread if we are
        /// on the UI thread)
        /// </summary>
        /// <param name="del">Delegate to execute on the UI thread</param>
        public static void FormInvoke(Delegate del, object[] args)
        {
            if ((callingForm != null) && (callingForm.InvokeRequired))
            {
                // The caller is on a different thread than the one the form was
                // created on. So we need to call form.Invoke to execute the delegate
                // on the thread that owns the form. 
                callingForm.Invoke(del, args);
            }
            else
            {
                // Otherwise we are already on the UI thread where the form was created, 
                // so call the delegate code directly on the current thread
                del.DynamicInvoke(args);
            }
        }
                     
        public static bool AutoPlayRemote
        {
            get
            {
                return autoPlayRemote;
            }
            set
            {
                autoPlayRemote = value;
                if (autoPlayRemote && activeVenue != null)
                {
                    foreach (ICapabilityViewer cv in capabilityViewers.Values)
                    {
                        if (!cv.IsPlaying)
                        {
                            if(cv.Owner != LocalParticipant)
                            {
                                if (cv.AutoPlayRemote)
                                {
                                    //Pri2: We should notify the user upon failure.  New event?
                                    cv.Play();
                                }
                            }
                        }
                    }
                }
            }
        }
        public static bool AutoPlayLocal
        {
            get
            {
                return autoPlayLocal;
            }
            set
            {
                autoPlayLocal = value;
                if (autoPlayLocal && activeVenue != null)
                {
                    foreach (ICapabilityViewer cv in capabilityViewers.Values)
                    {
                        if (!cv.IsPlaying)
                        {
                            if (cv.Owner == LocalParticipant)
                            {
                                if (cv.AutoPlayLocal)
                                {
                                    //Pri2: We should notify the user upon failure.  New event?
                                    cv.Play();
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// AutoPosition causes each CapabilityViewer that supports ICapabilityWindow to have their windo
        /// moved into a "docked" position starting with the upper left of the primary monitor as
        /// CapabilityViewer.Play() is called.
        /// </summary>
        public static AutoPositionMode AutoPosition
        {
            get
            {
                return autoPosition;
            }
            set
            {
                autoPosition = value;
            }
        }


        /// <summary>
        /// Location corresponds to the Rtcp property LOC for the LocalParticipant.
        /// See RFC 1889 for a definition of LOC.
        /// </summary>
        public static string Location
        {
            get
            {
                return location;
            }
            set
            {
                location = value;
            }
        }
        /// <summary>
        /// About gets a string that gives you dependency and version information about this assembly.
        /// </summary>
        public static string About
        {
            get
            {
                return
                    GetAssemblyDescription(Assembly.GetExecutingAssembly()) + "\n" +
                    GetAssemblyDescription(Assembly.Load("MSR.LST.Net.Rtp")) + "\n" +
                    GetAssemblyDescription(Assembly.Load("MDShowManager"));
            }
        }

        private static string GetAssemblyDescription(Assembly a)
        {
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(a.Location);
            return a.GetName().Name + " : " + fvi.FileVersion;
        }


        /// <summary>
        /// CapabilitySenderNames returns an array of all the names of the CapabilitySenders installed in the application
        /// </summary>
        public static string[] CapabilitySenderNames
        {
            get
            {
                ArrayList capabilitySenderNames = new ArrayList();
                foreach( DictionaryEntry de in capabilitySenderClasses )
                {
                    capabilitySenderNames.Add(de.Key);
                }
                return (string[])capabilitySenderNames.ToArray(typeof(string));
            }
        }

        /// <summary>
        /// CapabilityViewerNames returns an array of all the names of the CapabilityViewers installed in the application
        /// </summary>
        public static string[] CapabilityViewerNames
        {
            get
            {
                ArrayList capabilityViewerNames = new ArrayList();
                foreach( DictionaryEntry de in capabilityViewerClasses )
                {
                    capabilityViewerNames.Add(de.Key);
                }
                return (string[])capabilityViewerNames.ToArray(typeof(string));
            }
        }

        /// <summary>
        /// Return an array of other CapabilitySenders names (not including audio or video).  3rd party CapabilitySenders
        /// can be added to ConferenceAPI by dropping an assembly into the direction that implements ICapabilitySender.
        /// </summary>
        public static string[] OtherCapabilitySenders
        {
            get
            {
                ArrayList otherCapabilities = new ArrayList();

                foreach(DictionaryEntry de in capabilitySenderClasses)
                {
                    // Skip Audio & Video CapabilitySenders since they are dealt with directly
                    // Pri3: Shouldn't we do this by using the PayloadType, thereby skipping third-party
                    //  A/V senders, and also saving any troubles we might have if we rename our capabilities??
                    if ((string)de.Key == "Video")
                    {
                        continue;
                    }
                    if ((string)de.Key == "Audio")
                    {
                        continue;
                    }

                    otherCapabilities.Add(de.Key);
                }
                return (string[])otherCapabilities.ToArray(typeof(string));
            }
        }

        /// <summary>
        /// Return a list of all CapabilitySenders, useful to do things like ensure no CapabilitySenders are
        /// playing at this time.
        /// </summary>
        public static ICapabilityHashtable CapabilitySenders
        {
            get
            {
                return (ICapabilityHashtable)capabilitySenders.Clone();
            }
        }
        /// <summary>
        /// Return the Participant that corresponds to the person running this instance of the application.
        /// </summary>
        public static Participant LocalParticipant
        {
            get
            {
                return localParticipant;
            }
        }
        /// <summary>
        /// Has a Venue been Joined?  If so, return it, if not return null.
        /// </summary>
        public static IVenue ActiveVenue
        {
            get
            {
                return activeVenue;
            }
        }

        /// <summary>
        /// Return a hashtable of all CapabilityViewers on the system, indexed by SSRC.
        /// </summary>
        public static ICapabilityHashtable CapabilityViewers
        {
            get
            {
                return (ICapabilityHashtable)capabilityViewers.Clone();
            }
        }
        
        /// <summary>
        /// Creates a capability given a name, then calls AddCapabilitySender
        /// </summary>
        public static ICapabilitySender CreateCapabilitySender(string name)
        {
            // Make sure we know how to create a capability with this name
            if (!capabilitySenderClasses.ContainsKey(name))
            {
                throw new Exception(string.Format(CultureInfo.CurrentCulture, Strings.CapabilityClassNotFound, name));
            }

            // Create it
            ICapabilitySender cs = (ICapabilitySender)Activator.CreateInstance(capabilitySenderClasses[name]); // This will allow a form to be shown where the CS name can be set before continuing on

            // Update collections, raise necessary events
            AddCapabilitySender(cs);

            // Remove? This isn't part of managing state - jasonv 10/18/2004
            cs.Send();

            if (cs is ICapabilityViewer)
            {
                AddCapabilityViewer((ICapabilityViewer)cs);
            }

            // Return it
            return cs;
        }

        /// <summary>
        /// Adds a capability to the correct collections and calls Send()
        /// </summary>
        public static void AddCapabilitySender(ICapabilitySender cs)
        {
            if (capabilitySenders.Contains(cs.ID))
            {
                throw new Exception(string.Format(CultureInfo.CurrentCulture, Strings.CapabilitySenderAlreadyExists, cs.Name));
            }

            if (capabilities.Contains(cs.ID))
            {
                throw new Exception(string.Format(CultureInfo.CurrentCulture, 
                    Strings.CapabilitiesCollectionAlreadyContains, cs.Name));
            }

            // Update collections
            capabilitySenders.Add(cs.ID, cs);
            capabilities.Add(cs.ID, cs);
        }


        /// <summary>
        /// Display the default UI to create a new profile for the LocalParticipant
        /// </summary>
        public static void CreateProfileUI()
        {
            try
            {
                // call UI to create a profile and submit
                OptionsForm of = new OptionsForm(OptionsForm.FormState.create);
                of.ShowDialog();
            }
            catch (Exception ex)
            {
                ShowProfileError(ex);
            }
        }
        /// <summary>
        /// Display the default UI to edit the profile for the LocalParticipant
        /// </summary>
        public static void EditProfileUI()
        {
            try
            {
                // call UI to create a profile and submit
                OptionsForm of = new OptionsForm(OptionsForm.FormState.edit);
                of.ShowDialog();
            }
            catch (Exception ex)
            {
                ShowProfileError(ex);
            }
        }
        private static void ShowProfileError(Exception ex)
        {
            if (ex is System.Net.WebException)
            {
                RtlAwareMessageBox.Show(null, Strings.VenueServerCouldNotBeReached, 
                    Strings.ErrorConnectingToVenueServer, MessageBoxButtons.OK, MessageBoxIcon.Error, 
                    MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
            }
            else
            {
                RtlAwareMessageBox.Show(null, string.Format(CultureInfo.CurrentCulture, 
                    Strings.UnexpectedConnectionError, ex.ToString()), Strings.ErrorConnectingToVenueServer, 
                    MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 
                    (MessageBoxOptions)0);
            }
        }
        /// <summary>
        /// Helper function to allow the profile on the Venue Service to be updated programmatically
        /// by a client UI without it needing to connect to the Venue Service via Web Services itself.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="phone"></param>
        /// <param name="email"></param>
        /// <param name="icon"></param>
        /// <param name="notify"></param>
        public static void UpdateProfileOnServer(string name, string phone, string email, Bitmap icon, bool notify)
        {
            if (venues.VenueService != null)
            {
                VenueService.Participant p = new VenueService.Participant();

                p.Identifier = Identity.Identifier;
                p.Name = name;
                p.Email = email;

                if (icon == null)
                {
                    p.Icon = null;
                }
                else
                {
                    p.Icon = Utilities.BitmapToByte(Utilities.GenerateThumbnail(icon));
                }

                venues.VenueService.UpdateParticipant(p);

                GetLocalParticipant();
            }
            else
            {
                RtlAwareMessageBox.Show(null, Strings.ApplicationIsNotConfigured, string.Empty, MessageBoxButtons.OK, 
                    MessageBoxIcon.None, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
            }
        }
        /// <summary>
        /// Helper function to allow the profile on the Venue Service to be added programmatically
        /// by a client UI without it needing to connect to the Venue Service via Web Services itself.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="phone"></param>
        /// <param name="email"></param>
        /// <param name="icon"></param>
        /// <param name="notify"></param>
        public static void AddProfileOnServer(string name, string phone, string email, Bitmap icon, bool notify)
        {
            if (ConfigurationManager.AppSettings[AppConfig.CXP_VenueService] != null)
            {
                VenueService.Participant p = new VenueService.Participant();

                p.Identifier = Identity.Identifier;
                p.Name = name;
                p.Email = email;

                if (icon == null)
                    p.Icon = null;
                else
                    p.Icon = Utilities.BitmapToByte(Utilities.GenerateThumbnail(icon));

                venues.VenueService.AddParticipant(p);

                GetLocalParticipant();
            }
            else
            {
                RtlAwareMessageBox.Show(null, Strings.ApplicationIsNotConfigured, string.Empty, MessageBoxButtons.OK, 
                    MessageBoxIcon.None, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
            }
        }

        public static Bitmap GenerateThumbnail(Bitmap image) {
            return Utilities.GenerateThumbnail(image);
        }

        #endregion Public
        
        #region Internal
        
        internal static bool CallingFormSet
        {
            get{return callingForm != null;}
        }


        /// <summary>
        /// Removes a form from the forms collection
        /// </summary>
        /// <param name="id">SharedFormID or capability.ID, depending how a form was created for the capability</param>
        internal static void RemoveForm(Guid id)
        {
            if(capabilityForms.Contains(id))
            {
                capabilityForms.Remove(id);
            }
        }

        
        #endregion Internal

        #region Private

        private static void DisposeCapability(ICapability capability)
        {
            //Persist window size and location, but only if the window position was changed since it was originally placed.
            persistWindowPosition(capability);

            // Let the capability clean itself up
            capability.Dispose();

            // Now we'll clean up our state
            Guid id = capability.ID;

            // Make sure this Capability is removed from the CapabilitySenders
            if (Conference.capabilitySenders.ContainsKey(id))
            {
                Conference.capabilitySenders.Remove(id);
            }

            // Make sure this Capability is removed from the CapabilityViewers
            if (Conference.capabilityViewers.ContainsKey(id))
            {
                Conference.capabilityViewers.Remove(id);
            }

            // Remove from the main collection
            Debug.Assert(capabilities.ContainsKey(id));
            Conference.capabilities.Remove(id);

            Conference.RaiseCapabilityRemoved(capability);
        }

        /// <summary>
        /// In order to persist window positions:
        /// 1. Conference.PersistWindowPositions must be enabled (corresponds to menu entry in UI)
        /// 2. The capability must be a ICapabilityWindow
        /// 3. The capbility must be the one whose window we originally placed, that is if it used a shared window, it was the first capability in the window.
        /// 4. The window must have been moved or resized since it was originally placed.
        /// </summary>
        /// <param name="capability"></param>
        private static void persistWindowPosition(ICapability capability)
        {
            //TODO: For now we don't persist the Windows Media Playback capability as a special case.  
            //It has two windows with the same name and payloadType which will overlay one another if persisted.
            if (capability.Name.Equals("Windows Media Playback"))
            {
                return;
            }
            if (Conference.PersistWindowPositions)
            {
                ICapabilityWindow icw = capability as ICapabilityWindow;
                if (icw != null)
                {
                    if (originalWindowPositions.ContainsKey(capability.Name))
                    {
                        Rectangle newRect = Rectangle.Empty;
                        try
                        {
                            newRect = icw.Rectangle;
                        }
                        catch (Exception e)
                        {
                            //Saw this once: 'no form set for this capability'.  
                            Debug.WriteLine("Exception while persisting " + capability.Name + ": " + e.Message);
                            return;
                        }
                        if (!newRect.Equals(Rectangle.Empty))
                        {
                            if (windowPositionChanged(originalWindowPositions[capability.Name], newRect))
                            {
                                if (windowPersistenceData.ContainsKey(capability.Name))
                                {
                                    windowPersistenceData.Remove(capability.Name);
                                }
                                windowPersistenceData.Add(capability.Name, newRect);
                                Debug.WriteLine("Persisting window location: " + capability.Name + " at:" + newRect.ToString());
                            }
                            originalWindowPositions.Remove(capability.Name);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Approximate comparison of two rectangles.  In practice the video window size changes a little bit 
        /// probably to account for the aspect ratio of the video itself.  We want to distinguish between
        /// this change and changes to size and location driven by the user.  Return true if X or Y changed, 
        /// or if both Height and Width changed.
        /// </summary>
        /// <param name="rect1"></param>
        /// <param name="rect2"></param>
        /// <returns></returns>
        private static bool windowPositionChanged(Rectangle rect1, Rectangle rect2) {
            if ((rect1.Location.Equals(rect2.Location)) &&
                ((rect1.Height == rect2.Height) || (rect1.Width == rect2.Width))) {
                return false;
            }
            return true;
        }

        private static void AddCapabilityViewer(ICapabilityViewer iCV)
        {
            // Update collections
            if(!capabilities.Contains(iCV.ID))
            {
                capabilities.Add(iCV.ID, iCV);
            }

            capabilityViewers.Add(iCV.ID, iCV);
            
            if (iCV.Owner != null)
            {
                iCV.Owner.AddCapabilityViewer(iCV);
            }

            // Call RaiseCapabilityAdded before calling Play
            RaiseCapabilityAdded(iCV);

            // If (it's a remote stream, && we want to play remote streams, && this stream is ok to play if remote
            //  -OR- it's a local stream, && we want to play local streams, && this stream is ok to play if local (not audio, for instance)
            //  -OR- it's not an AV-stream
            //  ... Play it.
            if ( (autoPlayRemote && iCV.AutoPlayRemote && iCV.Owner != LocalParticipant)
                || (autoPlayLocal && iCV.AutoPlayLocal && iCV.Owner == LocalParticipant)
                || (iCV.PayloadType != PayloadType.dynamicAudio && iCV.PayloadType != PayloadType.dynamicVideo) )
            {
                iCV.Play();
            }
        }

        private static ICapabilityViewer CreateCapabilityForRtpStream(MSR.LST.Net.Rtp.RtpStream rtpStream)
        {
            //Pri2: Should use a match from rtpStream.PayloadType via VenueServer to a GUID which is used to lookup the assembly used to view.
            //Pri2: VenueServer should also have a link from CapabilityViewerGuid to CapabilityViewerAssembly that can be auto-installed on demand
            MSR.LST.ConferenceXP.PayloadType payloadType = (MSR.LST.ConferenceXP.PayloadType)rtpStream.PayloadType;

            if (!capabilityViewerClasses.ContainsKey(payloadType))
            {
                throw new Exception(string.Format(CultureInfo.CurrentCulture, Strings.CapabilityViewerNotFound, 
                    payloadType.ToString()));
            }

            return (ICapabilityViewer)Activator.CreateInstance(
                        capabilityViewerClasses[payloadType],
                        new object[]{Capability.DynaPropsFromRtpStream(rtpStream)});
        }
                
        private delegate void StopPlayingCVDelegate (ICapabilityViewer cv);
        private static void StopPlayingCV(ICapabilityViewer cv)
        {
            try
            {
                cv.StopPlaying();
            }
            catch (ThreadAbortException) {}
            catch (Exception e)
            {
                eventLog.WriteEntry(string.Format(CultureInfo.CurrentCulture, Strings.CapabilityViewer, cv.ToString(), 
                    e.ToString()), EventLogEntryType.Error, 18);
            }
        }

        private delegate void PlayCVDelegate (ICapabilityViewer cv);
        private static void PlayCV(ICapabilityViewer cv)
        {
            try
            {
                cv.Play();
            }
            catch (ThreadAbortException) {}
            catch (Exception e)
            {
                eventLog.WriteEntry(string.Format(CultureInfo.CurrentCulture, Strings.CapabilityViewer, 
                    cv.ToString(), e.ToString()), EventLogEntryType.Error, 18);
            }
        }

        private delegate void SendCSDelegate (ICapabilitySender cs);
        private static void SendCS(ICapabilitySender cs)
        {
            try
            {
                cs.Send();
            }
            catch (ThreadAbortException) {}
            catch (Exception e)
            {
                eventLog.WriteEntry(string.Format(CultureInfo.CurrentCulture, Strings.CapabilitySender, cs.ToString(), 
                    e.ToString()), EventLogEntryType.Error, 18);
            }
        }

        private static void SetConfiguration()
        {
            string setting = null;

            if ((setting = ConfigurationManager.AppSettings[AppConfig.CXP_AutoPlayLocal]) != null)
            {
                autoPlayLocal = bool.Parse(setting);
            }
            if ((setting = ConfigurationManager.AppSettings[AppConfig.CXP_AutoPlayRemote]) != null)
            {
                autoPlayRemote = bool.Parse(setting);
            }
            if ((setting = ConfigurationManager.AppSettings[AppConfig.CXP_AutoPosition]) != null)
            {
                autoPosition = (AutoPositionMode)Enum.Parse(autoPosition.GetType(), setting);
            }
            if ((setting = ConfigurationManager.AppSettings[AppConfig.CXP_LogActivity]) != null) {
                logActivity = bool.Parse(setting);
            }

            if ((setting = ConfigurationManager.AppSettings[AppConfig.CXP_VenueServiceTimeout]) != null) {
                if (Int32.TryParse(setting,out venueServiceTimeout)) {
                    if (venueServiceTimeout > 0)
                        venues.WebServiceTimeout = venueServiceTimeout;
                }
                else {
                    venueServiceTimeout = 0;
                }
            }

            // Only load the venue service from the config file to get old behavior
            setting = ConfigurationManager.AppSettings[AppConfig.CXP_VenueService_AutoLoad];
            if (setting == null || bool.Parse(setting))
            {
                if ((setting = ConfigurationManager.AppSettings[AppConfig.CXP_VenueService]) != null)
                {
                    venues.VenueServiceUrl = setting;
                }
            }

            windowPersistenceData = Utilities.LoadWindowPersistenceData();

        }

        private static void GetLocalParticipant()
        {
            Participant p = null;

            // Are we configured to use a Venue Service?
            if (venues.VenueServiceUrl != null) 
            {
                p = venues.GetParticipantUnmediated(Identity.Identifier);

                if (p == null && !venues.IsUnreachable) // We talked to the Venue Server properly, but no record was found
                {
                    if (createParticipantUIShowed == false)
                    {
                        createParticipantUIShowed = true; // Allows opt out to succeed
                        RaiseCreateProfile(); // This calls us back recursively
                        return;
                    }
                }
            }

            if (p == null)
            {
                // We're explicitly configured *not* to use a Venue Service, so use a default participant.
                // Create a minimal participant
                VenueService.Participant vp = new VenueService.Participant();
                vp.Identifier = Identity.Identifier;
                vp.Name = vp.Identifier;
                
                p = new Participant(vp);
            }

            localParticipant = p;

            // We may be refreshing the Local Participant after edits
            participants[localParticipant.Identifier] = localParticipant;
        }

        
        private static void autoPositionCV(ICapability cv)
        {
            Debug.Assert(Conference.ActiveVenue != null);

            // Assume a standalone capability, override if shared forms are used
            Guid id = cv.UsesSharedForm ? cv.SharedFormID : cv.ID;

            // Only auto-position ourselves once
            if(!capabilityForms.Contains(id))
            {
                if (cv is ICapabilityWindow)
                {
                    ICapabilityWindow iCW = (ICapabilityWindow)cv;

                    // The CapabilityWindow is not initialized or we don't want to auto-position
                    if (iCW.Height == 0 || autoPosition == AutoPositionMode.None) 
                    {
                        return;
                    }

                    //See if we should use a persisted window location.
                    if ((Conference.PersistWindowPositions) && (restorePersistedWindowPosition(cv))) {
                        //A persisted location has been applied, so skip the rest of autoPositonCV.
                        if (originalWindowPositions.ContainsKey(cv.Name)) {
                            originalWindowPositions.Remove(cv.Name);
                        }
                        originalWindowPositions.Add(cv.Name, iCW.Rectangle);
                        capabilityForms.Add(id);
                        return;
                    }

                    // Set bottom and right
                    if (iCW is ICapabilityWindowAutoSize)
                    {
                        autoSizeCapabilityWindow(iCW);
                    }

                    bool foundOpenPosition = false;

                    int workingTop = SystemInformation.WorkingArea.Top;
                    int workingLeft = SystemInformation.WorkingArea.Left;
                    int workingBottom = SystemInformation.WorkingArea.Bottom;
                    int workingRight = SystemInformation.WorkingArea.Right;
                            
                    Rectangle rect = new Rectangle(workingLeft, workingTop, iCW.Width, iCW.Height);
                    int minY = rect.Top + rect.Height; // Assume we want to move down the height of the capability

                    while (!foundOpenPosition)
                    {
                        // Check for horizontal overflow, if so go down a column
                        if (rect.Left + rect.Width > workingRight)
                        {
                            rect = new Rectangle(workingLeft, minY, rect.Width, rect.Height);
                            minY = rect.Top + rect.Height;
                        }

                        // Check for verical overflow, if so go back to the upper left
                        if (rect.Top + rect.Height > workingBottom)
                        {
                            rect = new Rectangle(workingLeft, workingTop, rect.Width, rect.Height);
                            break; // force position, even if overlapping existing capability
                        }

                        // Assume we found an open position, and prove it overlaps an existing one
                        foundOpenPosition = true;

                        // Check for window already present
                        foreach(ICapability ic in capabilities.Values)
                        {
                            Guid icID = ic.UsesSharedForm ? ic.SharedFormID : ic.ID;

                            // Skip me (don't try to check against self)
                            if (icID != id && ic is ICapabilityWindow)
                            {
                                // This is incorrect information - the form may have been moved by user - jasonv 10/13/2004
                                Rectangle icRect = ((ICapabilityWindow)ic).Rectangle;

                                if (rect.IntersectsWith(icRect))
                                {
                                    // Next horizontal position to try
                                    rect = new Rectangle(icRect.Left + icRect.Width, rect.Top, rect.Width, rect.Height);

                                    // Next vertical position to try (in event of horizontal overflow)
                                    if(icRect.Top + icRect.Height < minY)
                                    {
                                        minY = icRect.Top + icRect.Height;
                                    }

                                    foundOpenPosition = false;
                                    break;
                                }
                            }
                        }
                    }

                    iCW.Top = rect.Top;
                    iCW.Left = rect.Left;

                    if (originalWindowPositions.ContainsKey(cv.Name)) {
                        originalWindowPositions.Remove(cv.Name);
                    }
                    originalWindowPositions.Add(cv.Name, iCW.Rectangle);
                    Debug.WriteLine("AutoPositioned Window: " + cv.Name + " to:" + iCW.Rectangle.ToString());

                    capabilityForms.Add(id);
                }
            }
        }


        private static bool restorePersistedWindowPosition(ICapability cv) {
            if (cv is ICapabilityWindow) {
                ICapabilityWindow iCW = (ICapabilityWindow)cv;
                if (Conference.windowPersistenceData.ContainsKey(cv.Name)) {
                    if (windowOnScreen(Conference.windowPersistenceData[cv.Name])) {
                        iCW.Rectangle = Conference.windowPersistenceData[cv.Name];
                        Debug.WriteLine("Restored persisted window position for: " + cv.Name + " to: " + iCW.Rectangle.ToString());
                        return true;
                    }
                }
            }            
            return false;
        }

        /// <summary>
        /// If the users display configuration changed since the window position was persisted, we could end up with a window off-screen
        /// Make sure the Rectangle is safely within the display area with a little bit to spare.  Return true if it is.
        /// </summary>
        /// <param name="rectangle"></param>
        /// <returns></returns>
        private static bool windowOnScreen(Rectangle rect) {

            foreach (Screen scr in Screen.AllScreens) {
                if ((rect.Right - 20 > scr.WorkingArea.Left) &&
                    (rect.Left + 20 < scr.WorkingArea.Right) &&
                    (rect.Top + 20 < scr.WorkingArea.Bottom) &&
                    (rect.Top + 15 > scr.WorkingArea.Top)) {
                    return true;
                }
            }
            Debug.WriteLine("Window Persistence data is outside of the working area.");
            return false;
        }

        private static void autoSizeCapabilityWindow(ICapabilityWindow iCW)
        {
            if (autoPosition == AutoPositionMode.Tiled2x)
            {
                iCW.Height *= 2;
                iCW.Width *= 2;
            }

            if (autoPosition == AutoPositionMode.FourWay)
            {
                iCW.Size = new System.Drawing.Size(SystemInformation.WorkingArea.Width/2, SystemInformation.WorkingArea.Height/2);
            }

            if (autoPosition == AutoPositionMode.FullScreen)
            {
                iCW.Height = SystemInformation.WorkingArea.Height;
                iCW.Width = SystemInformation.WorkingArea.Width;
                iCW.Left = SystemInformation.WorkingArea.Left;
                iCW.Top = SystemInformation.WorkingArea.Top;
                return;
            }
        }
        
        
        #endregion Private

        #region Join/Leave Venue

        public static void JoinVenue(IVenue venue)
        {
            JoinVenue(venue, null);
        }


        /// <summary>
        /// Join the Venue and start participating.  AutoSend default devices if AutoSend == true
        /// The optional password is used generate a symmetric encryption key...
        /// </summary>
        public static void JoinVenue(IVenue venue,String password)
        {
            foreach (Participant p in participants.Values) {
                //If there was an active warning on the local particpant at the time of the 
                //previous venue leave, it needs to be cleaned up.
                p.ThroughputWarnings.Clear();
            }

            lock(venue)
            {
                if (Conference.ActiveVenue != null)
                {
                    throw new InvalidOperationException(Strings.AlreadyJoinedToVenue);
                }

                if (venue.VenueData.VenueType == VenueType.Invalid)
                {
                    throw new ArgumentException(Strings.InvalidVenueType);
                }

                // (pfb) Why are we checking this here?  When would it happen?
                if (rtpSession == null)
                {
                    // Do this first, or else the locks in the eventHandlers will throw, due to null argument
                    Conference.activeVenue = venue;
                    
                    HookRtpEvents();

                    #region Start the RtpSession
                    try
                    {
                        // Pri3: Add casting of a Participant to an RtpParticipant
                        Participant p = Conference.LocalParticipant;
                        RtpParticipant rp = new RtpParticipant(p.Identifier, p.Name);
                        rp.Email = p.Email;
                        rp.Location = Conference.Location;
                        rp.Phone = p.Phone;

                        if (Conference.ReflectorEnabled && MSR.LST.Net.Utility.IsMulticast(venue.EndPoint))
                        {
                            IPEndPoint refEP;
                            IPAddress reflectorIP;
                            
                            // Resolve the reflector address
                            try
                            {
                                reflectorIP = System.Net.Dns.GetHostEntry(
                                    Conference.ReflectorAddress).AddressList[0];
                            }
                            catch(Exception e)
                            {
                                //Starting with .Net 4, passing an IP address to GetHostEntry excepts.  We'll check
                                //here to see if the user entered an IP address before rethowing.
                                if (!IPAddress.TryParse(Conference.ReflectorAddress, out reflectorIP)) {
                                    throw new Exception(string.Format(CultureInfo.CurrentCulture,
                                        Strings.ReflectorAddressNotFound, Conference.ReflectorAddress), e);
                                }
                            }

                            // andrew: reflector registation is now done via UDP, and has been
                            // pushed down into UDPListener...

                            refEP = new IPEndPoint(reflectorIP, ReflectorRtpPort);

                            StartDiagnosticMonitor(venue.Name, venue.EndPoint);
                            
                            rtpSession = new RtpSession(venue.EndPoint, rp, true, true, refEP);
                            if (password != null)
                            {
                                PacketTransform xform = MakePacketTransform(password);
                                rtpSession.PacketTransform = xform;
                            }
                        } 
                        else
                        {
                            StartDiagnosticMonitor(venue.Name, venue.EndPoint);
                            
                            rtpSession = new RtpSession(venue.EndPoint, rp, true, true);
                            if (password != null)
                            {
                                PacketTransform xform = MakePacketTransform(password);
                                rtpSession.PacketTransform = xform;
                            }
                        }

                        // andrew: notify RTP of our venue name; this is for diagnostic purposes
                        rtpSession.VenueName = venue.Name;

                    }
                    catch (System.Net.Sockets.SocketException se)
                    {
                        eventLog.WriteEntry(se.ToString(), EventLogEntryType.Error, 99);
                        LeaveVenue();

                        if (se.ErrorCode == 10013)
                        {
                            throw new Exception(Strings.SecurityErrorOccurredConnecting);
                        }
                        else
                        {
                            throw;
                        }
                    }
                    catch (Exception e)
                    {
                        eventLog.WriteEntry(e.ToString(), EventLogEntryType.Error, 99);                        
                        LeaveVenue();
                        throw;
                    }

                    #endregion
                }
            }
        }

        private static PacketTransform MakePacketTransform(string password)
        {
            return new EncryptionTransform(password);
        }

        /// <summary>
        /// Leave the Venue and do all the cleanup work.
        /// </summary>
        public static void LeaveVenue()
        {
            if( Conference.ActiveVenue == null )
                throw new InvalidOperationException(Strings.CannotExitNotInVenue);

            lock(Conference.ActiveVenue)
            {
                StopDiagnosticMonitor();

                // So nothing new is created from the network
                UnhookRtpEvents();

                // Clean up participants + owned capabilities
                foreach(Participant p in new ArrayList(participants.Values))
                {
                    // A dependency on the local participant dictates that we should always have the local participant
                    //   in the venue.
                    if( p != Conference.localParticipant )
                        RemoveParticipant(p);
                }

                Debug.Assert(participants.Count == 1);

                // Dispose remaining capabilities (channels)
                foreach(ICapability ic in new ArrayList(capabilities.Values))
                {
                    DisposeCapability(ic);
                }

                Debug.Assert(capabilities.Count == 0);
                Debug.Assert(capabilityViewers.Count == 0);
                Debug.Assert(capabilitySenders.Count == 0);
                
                // Clear collection of form positions
                capabilityForms.Clear();

                // Leave the network
                if (rtpSession != null)
                {
                    rtpSession.Dispose();
                    rtpSession = null;
                }

                // andrew: termination of reflector sessions is now done in UdpListner.  This should happen
                // as a side-effect of disposing the RTP session.

                Conference.activeVenue = null;
            }

            originalWindowPositions.Clear();
            Utilities.SaveWindowPersistenceData(windowPersistenceData);
        }

        /// <summary>
        /// Stop any current instance of DiagnosticMonitor.  If Diagnostics are enabled, start a new instance.
        /// </summary>
        /// <param name="venue"></param>
        private static void StartDiagnosticMonitor(string venue, IPEndPoint endpoint) {
            StopDiagnosticMonitor();
            if (Conference.DiagnosticsEnabled) {
                string localCname = null;
                if (localParticipant != null) {
                    localCname = localParticipant.Identifier;
                }
                diagnosticMonitor = new DiagnosticMonitor(venue, endpoint, localCname);
                diagnosticMonitor.DiagnosticUpdate += new DiagnosticMonitor.DiagnosticUpdateEventHandler(DiagnosticUpdateHandler);
                diagnosticMonitor.Start();
            }
        }


        private static void StopDiagnosticMonitor() { 
            if (diagnosticMonitor != null) {
                diagnosticMonitor.DiagnosticUpdate -= new DiagnosticMonitor.DiagnosticUpdateEventHandler(DiagnosticUpdateHandler);
                diagnosticMonitor.Stop();
                diagnosticMonitor = null;
            }           
        }

        private static void HookRtpEvents()
        {
            RtpEvents.RtpParticipantAdded += new RtpEvents.RtpParticipantAddedEventHandler(RtpParticipantAdded);
            RtpEvents.RtpParticipantRemoved += new RtpEvents.RtpParticipantRemovedEventHandler(RtpParticipantRemoved);
            RtpEvents.RtpStreamAdded += new RtpEvents.RtpStreamAddedEventHandler(RtpStreamAdded);
            RtpEvents.RtpStreamRemoved += new RtpEvents.RtpStreamRemovedEventHandler(RtpStreamRemoved);

            RtpEvents.InvalidPacket += new RtpEvents.InvalidPacketEventHandler(RaiseInvalidPacket);
            RtpEvents.NetworkTimeout += new RtpEvents.NetworkTimeoutEventHandler(RaiseNetworkTimeout);
            RtpEvents.RtpParticipantTimeout += new RtpEvents.RtpParticipantTimeoutEventHandler(RaiseParticipantTimeout);
            RtpEvents.RtpStreamTimeout += new RtpEvents.RtpStreamTimeoutEventHandler(RaiseCapabilityTimeout);
            RtpEvents.FrameOutOfSequence += new RtpEvents.FrameOutOfSequenceEventHandler(RaiseFrameOutOfSequence);
            RtpEvents.InvalidPacketInFrame += new RtpEvents.InvalidPacketInFrameEventHandler(RaiseInvalidPacket);
            RtpEvents.PacketOutOfSequence += new RtpEvents.PacketOutOfSequenceEventHandler(RaisePacketOutOfSequence);
            RtpEvents.DuplicateCNameDetected += new RtpEvents.DuplicateCNameDetectedEventHandler(RaiseDuplicateIdentityDetected);
        }

        private static void UnhookRtpEvents()
        {
            RtpEvents.RtpParticipantAdded -= new RtpEvents.RtpParticipantAddedEventHandler(RtpParticipantAdded);
            RtpEvents.RtpParticipantRemoved -= new RtpEvents.RtpParticipantRemovedEventHandler(RtpParticipantRemoved);
            RtpEvents.RtpStreamAdded -= new RtpEvents.RtpStreamAddedEventHandler(RtpStreamAdded);
            RtpEvents.RtpStreamRemoved -= new RtpEvents.RtpStreamRemovedEventHandler(RtpStreamRemoved);

            RtpEvents.InvalidPacket -= new RtpEvents.InvalidPacketEventHandler(RaiseInvalidPacket);
            RtpEvents.NetworkTimeout -= new RtpEvents.NetworkTimeoutEventHandler(RaiseNetworkTimeout);
            RtpEvents.RtpParticipantTimeout -= new RtpEvents.RtpParticipantTimeoutEventHandler(RaiseParticipantTimeout);
            RtpEvents.RtpStreamTimeout -= new RtpEvents.RtpStreamTimeoutEventHandler(RaiseCapabilityTimeout);
            RtpEvents.FrameOutOfSequence -= new RtpEvents.FrameOutOfSequenceEventHandler(RaiseFrameOutOfSequence);
            RtpEvents.InvalidPacketInFrame -= new RtpEvents.InvalidPacketInFrameEventHandler(RaiseInvalidPacket);
            RtpEvents.PacketOutOfSequence -= new RtpEvents.PacketOutOfSequenceEventHandler(RaisePacketOutOfSequence);
            RtpEvents.DuplicateCNameDetected -= new RtpEvents.DuplicateCNameDetectedEventHandler(RaiseDuplicateIdentityDetected);
        }
        #endregion

        #region Events
        #region Activity Events
        #region ParticipantAdded

        public delegate void ParticipantAddedEventHandler(IParticipant participant);
        
        /// <summary>
        /// Called when a Participant enters the Venue.
        /// </summary>
        public static event ParticipantAddedEventHandler ParticipantAdded;

        private static void RtpParticipantAdded(object sender, RtpEvents.RtpParticipantEventArgs ea)
        {
            // Because the RtpSession Adds a Participant in its constructor, the RtpSession
            // reference has not been initialized yet (null).  Check for null, so we don't lose 
            // the participant added event.
            if(RtpSession != null)
            {
                // Filter out events from other RtpSession instances
                if (sender != RtpSession)
                {
                    return;
                }
            }

            if (diagnosticMonitor != null) {
                diagnosticMonitor.AddParticipant(ea.RtpParticipant.CName);
            }

            Conference.FormInvoke(new RtpEvents.RtpParticipantAddedEventHandler(_RtpParticipantAdded), new object[] { sender, ea });        
        }

        private static  void _RtpParticipantAdded(object sender, RtpEvents.RtpParticipantEventArgs ea)
        {
            Debug.Assert(Conference.ActiveVenue != null);

            string identifier = ea.RtpParticipant.CName;
            Participant participant = null;

            lock(Conference.ActiveVenue)
            {
                if (Conference.participants.ContainsKey(identifier))
                {
                    participant = (Participant)participants[identifier];
                }
            }

            // Do the expensive call outside the lock
            if( participant == null )
            {
                participant = venues.GetParticipant(
                    ea.RtpParticipant.CName,
                    ea.RtpParticipant.Name,
                    ea.RtpParticipant.Email,
                    ea.RtpParticipant.Phone );
            }

            lock(Conference.ActiveVenue)
            {
                // If there is a long delay at the Venue Server, this participant could have exited and come back 
                //  already, so they may be already in the venue as well, the Venue may have been exited.
                if (!participants.ContainsKey(participant.Identifier))
                {
                    participants.Add(participant.Identifier, participant);
                }

                try
                {
                    if (ParticipantAdded != null)
                    {
                        FormInvoke(ParticipantAdded, new object[] { participant } );
                    }

                    if (logActivity)
                    {
                        eventLog.WriteEntry(string.Format(CultureInfo.CurrentCulture, Strings.Added, 
                            participant.ToString()), EventLogEntryType.Information, 1);
                    }
                }
                catch (ThreadAbortException) {}
                catch (Exception e)
                {
                    eventLog.WriteEntry(e.ToString(),EventLogEntryType.Error, 99);
                }
            }
        }        
        
        
        #endregion

        #region ParticipantRemoved

        public delegate void ParticipantRemovedEventHandler(IParticipant participant);

        /// <summary>
        /// Called when a Participant leaves the Venue.  Handles cleanup of any CapabilityViewers for that
        /// Participant that may still be left over.
        /// </summary>
        /// <remarks>
        /// Because this event originates from a different thread than the Form thread, it sometimes occurs after Venue.Leave has been called.
        /// If the event handler procedure might do something badly if the UI is in a different state or mode after Venue.Leave, you should
        /// consider including the line:
        ///   if (Conference.ActiveVenue == null) return;
        /// </remarks>
        public static event ParticipantRemovedEventHandler ParticipantRemoved;
        
        private static void RtpParticipantRemoved(object sender, RtpEvents.RtpParticipantEventArgs ea)
        {
            // Filter out events from other RtpSession instances
            if (sender == RtpSession)
            {
                if (diagnosticMonitor != null) {
                    diagnosticMonitor.RemoveParticipant(ea.RtpParticipant.CName);
                }

                Conference.FormInvoke(new RtpEvents.RtpParticipantRemovedEventHandler(
                    _RtpParticipantRemoved), new object[] { sender, ea } );
            }
        }

        private static void _RtpParticipantRemoved(object sender, RtpEvents.RtpParticipantEventArgs ea)
        {
            Debug.Assert(Conference.ActiveVenue != null);

            lock(Conference.ActiveVenue)
            {
                RemoveParticipant(participants[ea.RtpParticipant.CName]);
            }
        }

        private static void RemoveParticipant(IParticipant participant)
        {
            foreach(ICapability ic in participant.Capabilities)
            {
                DisposeCapability(ic);
            }

            participants.Remove(participant.Identifier);

            try
            {
                if (ParticipantRemoved != null)
                {
                    FormInvoke(ParticipantRemoved, new object[] { participant } );
                }

                if (logActivity == true)
                {
                    eventLog.WriteEntry(string.Format(CultureInfo.CurrentCulture, Strings.Removed, 
                        participant.ToString()), EventLogEntryType.Information, 2);
                }
            }
            catch (ThreadAbortException) {}
            catch (Exception e)
            {
                eventLog.WriteEntry(e.ToString() ,EventLogEntryType.Error, 99);
            }
        }
        
        
        #endregion

        #region RtpStreamAdded

        private static void RtpStreamAdded(object sender, RtpEvents.RtpStreamEventArgs ea)
        {
            // Filter out events from other RtpSession instances
            if (sender == rtpSession)
            {
                Conference.FormInvoke(new RtpEvents.RtpStreamAddedEventHandler(
                    _RtpStreamAdded), new object[] { sender, ea } );
            }
        }

        private static void _RtpStreamAdded(object sender, RtpEvents.RtpStreamEventArgs ea)
        {
            Debug.Assert(Conference.ActiveVenue != null);

            lock(Conference.ActiveVenue)
            {
                ICapabilityViewer iCV = null;
                Guid capabilityKey = Capability.IDFromRtpStream(ea.RtpStream);

                if (capabilityViewers.ContainsKey(capabilityKey))
                {
                    iCV = (ICapabilityViewer)capabilityViewers[capabilityKey];

                    if (iCV.Owned && iCV.Owner == null)
                    {
                        // The owner is unknown, and this stream may contain the owner info; if so, set it
                        Capability.CapabilityType ctype = Capability.ChannelFromRtpStream(ea.RtpStream);
                        if (ctype == Capability.CapabilityType.Owned) // The owned type designates that the stream is coming from the Owner
                        {
                            iCV.Owner = participants[ea.RtpStream.Properties.CName];
                            iCV.Owner.AddCapabilityViewer(iCV);
                        }
                    }

                    iCV.StreamAdded(ea.RtpStream);
                }
                else
                {
                    // Try to convert an existing ICapabilitySender into an ICapabilityViewer if it exists...
                    if (capabilitySenders.ContainsKey(capabilityKey))
                    {
                        iCV = capabilitySenders[capabilityKey] as ICapabilityViewer;
                    }
                    
                    if (iCV == null) // None found, create a new CapabilityViewer from scratch
                    {
                        iCV = Conference.CreateCapabilityForRtpStream(ea.RtpStream);
                    }

                    // Make sure we add the stream first before calling RaiseCapabilityViewerAdded so that ICapability.Owner will be set when CapabilityViewerAdded is handled
                    iCV.StreamAdded(ea.RtpStream);
                    
                    AddCapabilityViewer(iCV);
                }

                Conference.RaiseCapabilityParticipantAdded(iCV, participants[ea.RtpStream.Properties.CName]);
            }
        }


        #endregion

        #region RtpStreamRemoved

        private static void RtpStreamRemoved(object sender, RtpEvents.RtpStreamEventArgs ea)
        {
            // Filter out events from other RtpSession instances
            if (sender == RtpSession)
            {
                Conference.FormInvoke(new RtpEvents.RtpStreamRemovedEventHandler(
                    _RtpStreamRemoved), new object[] { sender, ea } );
            }
        }

        private static void _RtpStreamRemoved(object sender, RtpEvents.RtpStreamEventArgs ea)
        {
            lock(Conference.ActiveVenue)
            {
                ICapabilityViewer iCV = (ICapabilityViewer)capabilityViewers[Capability.IDFromRtpStream(ea.RtpStream)];

                if (iCV != null)
                {
                    iCV.StreamRemoved(ea.RtpStream);
                    Conference.RaiseCapabilityParticipantRemoved(iCV, participants[ea.RtpStream.Properties.CName]);

                    // If there are no streams, shut it down
                    if (iCV.RtpStreams.Length == 0)
                    {
                        DisposeCapability(iCV);
                    }
                    else if(iCV.Owner != null)
                    {
                        // Owned capabilities should stop when there are no incoming streams from the owner
                        bool foundOwnerStream = false;
                        string ownerCName = iCV.Owner.Identifier;

                        foreach( RtpStream stream in iCV.RtpStreams )
                        {
                            if( stream.Properties.CName == ownerCName )
                            {
                                foundOwnerStream = true;
                                break;
                            }
                        }

                        if (!foundOwnerStream)
                        {
                            DisposeCapability(iCV);
                        }
                    }
                }
            }
        }
        
        
        #endregion

        #region CapabilityAdded
        public static event CapabilityAddedEventHandler CapabilityAdded;
        /// <summary>
        /// Called when a CapabilityViewer is detected in the Venue.  AutoPlay is handled in this event.
        /// </summary>
        /// <param name="cv"></param>
        private static void RaiseCapabilityAdded(ICapability capability)
        {
            try
            {
                if (CapabilityAdded != null)
                {
                    FormInvoke(CapabilityAdded, new object[] { null, new CapabilityEventArgs(capability) } );
                }

                if (logActivity == true)
                {
                    eventLog.WriteEntry(string.Format(CultureInfo.CurrentCulture, Strings.Added, 
                        capability.ToString()), EventLogEntryType.Information, 3);
                }
            }
            catch (ThreadAbortException) {}
            catch (Exception e)
            {
                eventLog.WriteEntry(e.ToString() , EventLogEntryType.Error, 99);
            }
        }
        #endregion

        #region CapabilityPlaying
        /// <summary>
        /// Called when the Capability.Play() method is ending.
        /// AutoPosition is handled here.
        /// </summary>
        public static event CapabilityPlayingEventHandler CapabilityPlaying;
        public static void RaiseCapabilityPlaying(ICapability capability)
        {
            try
            {
                if (CapabilityPlaying != null)
                {
                    FormInvoke(CapabilityPlaying, new object[] { null, new CapabilityEventArgs(capability) } );
                }

                if (capability is ICapabilityWindow)
                {
                    autoPositionCV(capability);
                }
                
                if (logActivity)
                {
                    eventLog.WriteEntry(string.Format(CultureInfo.CurrentCulture, Strings.Playing, 
                        capability.ToString()), EventLogEntryType.Information, 4);
                }
            }
            catch (ThreadAbortException) {}
            catch (Exception e)
            {
                eventLog.WriteEntry(e.ToString() ,EventLogEntryType.Error, 99);
            }
        }
        #endregion
        
        #region CapabilitySending
        /// <summary>
        /// Called when the Capability.Send() method is ending.
        /// AutoPosition is handled here.
        /// </summary>
        public static event CapabilitySendingEventHandler CapabilitySending;
        public static void RaiseCapabilitySending(ICapability capability)
        {
            try
            {
                if (CapabilitySending != null)
                {
                    FormInvoke(CapabilitySending, new object[] { null, new CapabilityEventArgs(capability) } );
                }

                if (capability is ICapabilityWindow)
                {
                    autoPositionCV(capability);
                }
                
                if (logActivity)
                {
                    eventLog.WriteEntry(string.Format(CultureInfo.CurrentCulture, Strings.Sending, 
                        capability.ToString()), EventLogEntryType.Information, 4);
                }
            }
            catch (ThreadAbortException) {}
            catch (Exception e)
            {
                eventLog.WriteEntry(e.ToString() ,EventLogEntryType.Error, 99);
            }
        }
        #endregion
        
        #region CapabilityRemoved
        /// <summary>
        /// Called when a CapabilityViewer has left the Venue.
        /// </summary>
        /// <remarks>
        /// Because this event originates from a different thread than the Form thread, it sometimes occurs after Venue.Leave has been called.
        /// If the event handler procedure might do something badly if the UI is in a different state or mode after Venue.Leave, you should
        /// consider including the line:
        ///   if (Conference.ActiveVenue == null) return;
        /// </remarks>
        public static event CapabilityRemovedEventHandler CapabilityRemoved;
        private static void RaiseCapabilityRemoved(ICapability capability)
        {
            try
            {
                if (CapabilityRemoved != null)
                {
                    FormInvoke(CapabilityRemoved, new object[] { null, new CapabilityEventArgs(capability) } );
                }

                if (logActivity == true)
                {
                    eventLog.WriteEntry(string.Format(CultureInfo.CurrentCulture, Strings.Removed, 
                        capability.ToString()), EventLogEntryType.Information, 5);
                }
            }
            catch (ThreadAbortException) {}
            catch (Exception e)
            {
                eventLog.WriteEntry(e.ToString() ,EventLogEntryType.Error, 99);
            }
        }
        #endregion
        
        #region CapabilityStoppedPlaying
        /// <summary>
        /// Called at the beginning of Capability.Stop()
        /// </summary>
        /// <remarks>
        /// Because this event originates from a different thread than the Form thread, it sometimes occurs after Venue.Leave has been called.
        /// If the event handler procedure might do something badly if the UI is in a different state or mode after Venue.Leave, you should
        /// consider including the line:
        ///   if (Conference.ActiveVenue == null) return;
        /// </remarks>
        public static event CapabilityStoppedPlayingEventHandler CapabilityStoppedPlaying;
        public static void RaiseCapabilityStoppedPlaying(ICapability capability)
        {
            try
            {
                if (CapabilityStoppedPlaying != null)
                {
                    FormInvoke(CapabilityStoppedPlaying, new object[] { null, new CapabilityEventArgs(capability) } );
                }

                if (logActivity == true)
                {
                    eventLog.WriteEntry(string.Format(CultureInfo.CurrentCulture, Strings.StoppedPlaying, 
                        capability.ToString()), EventLogEntryType.Information, 6);
                }
            }
            catch (ThreadAbortException) {}
            catch (Exception e)
            {
                eventLog.WriteEntry(e.ToString(), EventLogEntryType.Error, 99);
            }
        }
        #endregion
        
        #region CapabilityStoppedSending
        /// <summary>
        /// Called at the beginning of Capability.Stop()
        /// </summary>
        /// <remarks>
        /// Because this event originates from a different thread than the Form thread, it sometimes occurs after Venue.Leave has been called.
        /// If the event handler procedure might do something badly if the UI is in a different state or mode after Venue.Leave, you should
        /// consider including the line:
        ///   if (Conference.ActiveVenue == null) return;
        /// </remarks>
        public static event CapabilityStoppedSendingEventHandler CapabilityStoppedSending;
        public static void RaiseCapabilityStoppedSending(ICapability capability)
        {
            try
            {
                if (CapabilityStoppedSending != null)
                {
                    FormInvoke(CapabilityStoppedSending, new object[] { null, new CapabilityEventArgs(capability) } );
                }

                if (logActivity == true)
                {
                    eventLog.WriteEntry(string.Format(CultureInfo.CurrentCulture, Strings.StoppedSending, 
                        capability.ToString()), EventLogEntryType.Information, 6);
                }
            }
            catch (ThreadAbortException) {}
            catch (Exception e)
            {
                eventLog.WriteEntry(e.ToString(), EventLogEntryType.Error, 99);
            }
        }
        #endregion
        
        #region CapabilityParticipantAdded
        public static event CapabilityParticipantAddedEventHandler CapabilityParticipantAdded;
        private static void RaiseCapabilityParticipantAdded(ICapability capability, IParticipant participant)
        {
            try
            {
                if (CapabilityParticipantAdded != null)
                {
                    FormInvoke(CapabilityParticipantAdded, new object[] { capability, new ParticipantEventArgs(participant) } );
                }

                if (logActivity == true)
                {
                    eventLog.WriteEntry(string.Format(CultureInfo.CurrentCulture, Strings.CapabilityParticipantAdded, 
                        capability.ToString(), participant.ToString()), EventLogEntryType.Information, 1);
                }
            }
            catch (ThreadAbortException) {}
            catch (Exception e)
            {
                eventLog.WriteEntry(e.ToString(),EventLogEntryType.Error, 99);
            }
        }
        #endregion
        
        #region CapabilityParticipantRemoved
        public static event CapabilityParticipantRemovedEventHandler CapabilityParticipantRemoved;
        private static void RaiseCapabilityParticipantRemoved(ICapability capability, IParticipant participant)
        {
            try
            {
                if (CapabilityParticipantRemoved != null)
                {
                    FormInvoke(CapabilityParticipantRemoved, new object[] { capability, new ParticipantEventArgs(participant) } );
                }

                if (logActivity == true)
                {
                    eventLog.WriteEntry(string.Format(CultureInfo.CurrentCulture, Strings.CapabilityParticipantRemoved, 
                        capability.ToString(), participant.ToString()), EventLogEntryType.Information, 1);
                }
            }
            catch (ThreadAbortException) {}
            catch (Exception e)
            {
                eventLog.WriteEntry(e.ToString(),EventLogEntryType.Error, 99);
            }
        }
        #endregion
        #endregion
        
        #region Configuration Events
        public delegate void CreateProfileEventHandler();
        /// <summary>
        /// Called when it is necessary for a new Profile to be created for the LocalParticipant, such as
        /// when the application is run by a user for the first time.
        /// </summary>
        public static event CreateProfileEventHandler CreateProfile;
        private static void RaiseCreateProfile()
        {
            try
            {
                if (CreateProfile != null)
                {
                    FormInvoke(CreateProfile, null);
                }
                else
                {
                    CreateProfileUI();
                }
            }
            catch (ThreadAbortException) {}
            catch (Exception e)
            {
                eventLog.WriteEntry(e.ToString(), EventLogEntryType.Error, 99);
            }

            GetLocalParticipant();
        }
        #endregion
        
        #region Diagnostic Events
        #region Invalid Packet
        public delegate void InvalidPacketEventHandler(ICapability capabilityViewer, string reason);
        /// <summary>
        /// Maps RtpPacket.InvalidRtpPacketException and RtpFrame.InvalidPacketInFrameException onto one event.
        /// Comes to here through RtpStream.InvalidPacketInFrame and RtpListener.InvalidPacket events (these should be combined!)
        /// </summary>
        public static event InvalidPacketEventHandler InvalidPacket;
        private static void RaiseInvalidPacket(object sender, RtpEvents.InvalidPacketInFrameEventArgs ea)
        {
            // Filter out events from other RtpListener instances
            if (!rtpSession.ContainsStream((RtpStream)sender))
            {
                return;
            }

            try
            {
                ICapability cv = capabilityViewers[Capability.IDFromRtpStream(ea.RtpStream)];

                if (InvalidPacket != null)
                {
                    InvalidPacket(cv, ea.Reason);
                }
                else
                {
                    if (cv != null)
                    {
                        eventLog.WriteEntry(string.Format(CultureInfo.CurrentCulture, Strings.InvalidPacketCV, 
                            cv.ToString(), ea.Reason), EventLogEntryType.Warning, 7);
                    }
                    else
                    {
                        eventLog.WriteEntry(string.Format(CultureInfo.CurrentCulture, Strings.InvalidPacketSSRC, 
                            ea.RtpStream.SSRC, ea.Reason), EventLogEntryType.Warning, 7);
                    }
                }
            }
            catch (ThreadAbortException) {}
            catch (Exception e)
            {
                eventLog.WriteEntry(e.ToString(), EventLogEntryType.Error, 99);
            }
        }
        private static void RaiseInvalidPacket(object sender, RtpEvents.InvalidPacketEventArgs ea)
        {
            // Filter out events from other RtpSession instances
            if (sender != rtpSession)
            {
                return;
            }

            try
            {
                if (InvalidPacket != null)
                {
                    InvalidPacket(null, ea.Reason);
                }
                else
                {
                    eventLog.WriteEntry(string.Format(CultureInfo.CurrentCulture, 
                        Strings.InvalidPacketUnknownCapabilityViewer, ea.Reason), EventLogEntryType.Warning, 8);
                }
            }
            catch (ThreadAbortException) {}
            catch (Exception e)
            {
                eventLog.WriteEntry(e.ToString() , EventLogEntryType.Error, 99);
            }
        }
        #endregion

        #region NetworkTimeout
        public delegate void NetworkTimeoutEventHandler();
        /// <summary>
        /// If no data comes over the network from anywhere for a period of time, this event fires.  Generally this means your modem is on fire
        /// or your ISP has suspended your account due to non-payment.  It can also mean that a network connection was added (such as a dialup
        /// PPP connection) which is rerouting your traffic or you've hit a bug such as multicast traffic going off into the ether once a VPN
        /// connection is established.
        /// </summary>
        public static event NetworkTimeoutEventHandler NetworkTimeout;
        private static void RaiseNetworkTimeout(object sender, RtpEvents.NetworkTimeoutEventArgs ea)
        {
            // Filter out events from other RtpListener instances
            if (sender != rtpSession)
            {
                return;
            }

            try
            {
                if (NetworkTimeout != null)
                {
                    NetworkTimeout();
                }
                else
                {
                    eventLog.WriteEntry(Strings.NetworkTimeout, EventLogEntryType.Warning, 9);
                }
            }
            catch (ThreadAbortException) {}
            catch (Exception e)
            {
                eventLog.WriteEntry(e.ToString(), EventLogEntryType.Error, 99);
            }
        }
        #endregion

        #region ParticipantTimeout
        public delegate void ParticipantTimeoutEventHandler(Participant participant);
        /// <summary>
        /// Sometimes you don't receive a BYE from a sender, they just stop transmitting.  Perhaps their BYE packets were lost due to a network,
        /// glitch or perhaps their application crashed.  In this case, a ParticipantTimeout event will fire.
        /// </summary>
        public static event ParticipantTimeoutEventHandler ParticipantTimeout;

        private static void RaiseParticipantTimeout(object sender, RtpEvents.RtpParticipantEventArgs ea)
        {
            // Filter out events from other RtpListener instances
            if (sender != rtpSession)
            {
                return;
            }

            try
            {

                Participant p = (Participant)participants[ea.RtpParticipant.CName];

                if (ParticipantTimeout != null)
                {
                    ParticipantTimeout(p);
                }
                else
                {
                    if (p == null)
                    {
                        eventLog.WriteEntry(string.Format(CultureInfo.CurrentCulture, 
                            Strings.ParticipantTimeoutCName, ea.RtpParticipant.CName), 
                            EventLogEntryType.Warning, 10);
                    }
                    else
                    {
                        eventLog.WriteEntry(string.Format(CultureInfo.CurrentCulture, Strings.ParticipantTimeout, 
                            p.ToString()), EventLogEntryType.Warning, 10);
                    }
                }
            }
            catch (ThreadAbortException) {}
            catch (Exception e)
            {
                eventLog.WriteEntry(e.ToString(), EventLogEntryType.Error, 99);
            }
        }
        #endregion

        #region CapabilityTimeout
        /// <summary>
        /// Sometimes you don't receive a BYE from a sender, they just stop transmitting.  Perhaps their BYE packets were lost due to a network,
        /// glitch or perhaps their application crashed.  In this case, a CapabilityViewerTimeout event will fire.
        /// </summary>
        public static event CapabilityTimeoutEventHandler CapabilityTimeout;
        private static void RaiseCapabilityTimeout(object sender, RtpEvents.RtpStreamEventArgs ea)
        {
            // Filter out events from other RtpListener instances
            if (sender != rtpSession)
            {
                return;
            }

            try
            {
                ICapability capability = capabilityViewers[Capability.IDFromRtpStream(ea.RtpStream)];

                if (CapabilityTimeout != null)
                {
                    CapabilityTimeout(null, new CapabilityEventArgs(capability));
                }
                else
                {
                    if (capability != null)
                    {
                        eventLog.WriteEntry(string.Format(CultureInfo.CurrentCulture, Strings.CapabilityTimeout, 
                            capability.ToString()), EventLogEntryType.Warning, 11);
                    }
                    else
                    {
                        eventLog.WriteEntry(string.Format(CultureInfo.CurrentCulture, 
                            Strings.CapabilityTimeoutSSRC, ea.RtpStream.SSRC), EventLogEntryType.Warning, 11);
                    }
                }
            }
            catch (ThreadAbortException) {}
            catch (Exception e)
            {
                eventLog.WriteEntry(e.ToString(), EventLogEntryType.Error, 99);
            }
        }
        #endregion

        #region FrameOutOfSequence
        public delegate void FrameOutOfSequenceEventHandler(ICapability capabilityViewer, int lostFrames, string message);
        /// <summary>
        /// FrameOutOfSequence occurs when packets come in out of sequence.  While this occasionally happens due to the network reordering packets,
        /// it is much more likely to occur because packets were either dropped on the receiving PC (lack of CPU or a sudden performance dip is
        /// the general cause) or packets were dropped on the network.
        /// </summary>
        public static event FrameOutOfSequenceEventHandler FrameOutOfSequence;
        private static void RaiseFrameOutOfSequence(object sender, RtpEvents.FrameOutOfSequenceEventArgs ea)
        {
            // Filter out events from other RtpListener instances
            if (!rtpSession.ContainsStream((RtpStream)sender))
            {
                return;
            }

            try
            {
                ICapability cv = capabilityViewers[Capability.IDFromRtpStream(ea.RtpStream)];

                if (FrameOutOfSequence != null)
                {
                    FrameOutOfSequence(cv, ea.LostFrames, ea.Message);
                }
                else
                {
                    if (cv != null)
                    {
                        eventLog.WriteEntry(string.Format(CultureInfo.CurrentCulture, 
                            Strings.FrameOutOfSequenceMessage, ea.LostFrames, ea.Message, cv.ToString()), 
                            EventLogEntryType.Warning, 13);
                    }
                    else
                    {
                        eventLog.WriteEntry(string.Format(CultureInfo.CurrentCulture, 
                            Strings.FrameOutOfSequenceMessageSSRC, ea.LostFrames, ea.Message, ea.RtpStream.SSRC), 
                            EventLogEntryType.Warning, 13);
                    }
                }
            }
            catch (ThreadAbortException) {}
            catch (Exception e)
            {
                eventLog.WriteEntry(e.ToString(), EventLogEntryType.Error, 99);
            }
        }
        #endregion

        #region PacketOutOfSequence
        public delegate void PacketOutOfSequenceEventHandler(ICapability capabilityViewer, int lostPackets);
        /// <summary>
        /// When an RtpStream detects a packet that has arrived out of sequence, it fires this event.
        /// 
        /// Since this is a very commonly occuring event, this event should only be hooked for
        /// diagnostic applications -- rather the RtpStream.PacketOutOfSequenceEvents property or the performance counter should be used
        /// in most cases.
        /// </summary>
        public static event PacketOutOfSequenceEventHandler PacketOutOfSequence;
        private static void RaisePacketOutOfSequence(object sender, RtpEvents.PacketOutOfSequenceEventArgs packetOutOfSequenceEventArgs)
        {
            // Filter out events from other RtpListener instances
            if (!rtpSession.ContainsStream((RtpStream)sender))
            {
                return;
            }

            try
            {
                ICapability cv = capabilityViewers[Capability.IDFromRtpStream(packetOutOfSequenceEventArgs.RtpStream)];

                if (PacketOutOfSequence != null)
                {
                    PacketOutOfSequence(cv, packetOutOfSequenceEventArgs.LostPackets);
                }
                else
                {
                    if (cv != null)
                    {
                        eventLog.WriteEntry(string.Format(CultureInfo.CurrentCulture, 
                            Strings.PacketOutOfSequenceMessage, packetOutOfSequenceEventArgs.LostPackets, 
                            cv.ToString()), EventLogEntryType.Warning, 14);
                    }
                    else
                    {
                        eventLog.WriteEntry(string.Format(CultureInfo.CurrentCulture, 
                            Strings.PacketOutOfSequenceMessageSSRC, packetOutOfSequenceEventArgs.LostPackets, 
                            packetOutOfSequenceEventArgs.RtpStream.SSRC), EventLogEntryType.Warning, 14);
                    }
                }
            }
            catch (ThreadAbortException) {}
            catch (Exception e)
            {
                eventLog.WriteEntry(e.ToString(), EventLogEntryType.Error, 99);
            }
        }
        #endregion

        #region DuplicateIdentity

        public class DuplicateIdentityDetectedEventArgs : EventArgs
        {
            public IPAddress[] IPAddresses;
        
            public DuplicateIdentityDetectedEventArgs(IPAddress[] ipAddresses)
            {
                IPAddresses = ipAddresses;
            }
        }

        public delegate void DuplicateIdentityDetectedEventHandler( object conference, DuplicateIdentityDetectedEventArgs ea);
        
        /// <summary>
        /// This event is raised just prior to LeaveVenue being called by Conference.  There is no 
        /// need for external sources to try and clean up the Conference by calling LeaveVenue()
        /// although it won't hurt.
        /// </summary>
        public static event DuplicateIdentityDetectedEventHandler DuplicateIdentityDetected;
        private static void RaiseDuplicateIdentityDetected(object sender, RtpEvents.DuplicateCNameDetectedEventArgs ea)
        {
            // Filter out events from other RtpSession instances
            if (sender == rtpSession)
            {
                Conference.FormInvoke(new RtpEvents.DuplicateCNameDetectedEventHandler(
                    _RaiseDuplicateIdentityDetected), new object[] { sender, ea } );
            }
        }
        
        private static void _RaiseDuplicateIdentityDetected(object sender, RtpEvents.DuplicateCNameDetectedEventArgs ea)
        {
            try
            {
                if (DuplicateIdentityDetected != null)
                {
                    DuplicateIdentityDetected( null, new DuplicateIdentityDetectedEventArgs(ea.IPAddresses));
                }

                if (Conference.ActiveVenue != null)
                {
                    Conference.LeaveVenue();
                }

                string message = string.Format(CultureInfo.CurrentCulture, Strings.DuplicateLocalIdentityError, 
                    ea.IPAddresses[0].ToString(), ea.IPAddresses[1].ToString());

                eventLog.WriteEntry(message, EventLogEntryType.Error, 15);
            }
            catch (ThreadAbortException) {}
            catch (Exception e)
            {
                eventLog.WriteEntry(e.ToString(), EventLogEntryType.Error, 99);
            }

        }
        
        
        #endregion
        #endregion

        #region Diagnostic Monitor Event 

        public delegate void DiagnosticUpdateEventHandler(DiagnosticMonitor.DiagnosticUpdateEventArgs args);
        /// <summary>
        /// Event to raise when Diagnostic Monitor events should be reflected in the UI
        /// </summary>
        public static event DiagnosticUpdateEventHandler DiagnosticUpdate;

        private static void DiagnosticUpdateHandler(DiagnosticMonitor.DiagnosticUpdateEventArgs args) {
            Conference.FormInvoke(new DiagnosticUpdateEventHandler(_DiagnosticUpdateHandler), new object[] { args });
        }

        private static void _DiagnosticUpdateHandler(DiagnosticMonitor.DiagnosticUpdateEventArgs args) { 
            if (DiagnosticUpdate != null) {
                DiagnosticUpdate(args);
            }
        }

        #endregion Diagnostic Monitor Event

        #endregion

        #region Installation
        /// <summary>
        /// Ensure that Conference is properly installed on each run, attempting to make ".NET File Copy Deployment" a reality.
        /// 
        /// This mostly works, but two problems arise from "File Copy Deployment".  First, there's a large delay upon first run while setup executes.
        /// Second, uninstall is never run if the files are just deleted so niggling things like registry entries, performance counters, event logs,
        /// etc. get left around.
        /// 
        /// Because of this, we still deploy the application to the clients using MSIs which call custom actions that run the Installer classes on both
        /// setup and when Add/Remove Programs -> Remove is called.
        /// 
        /// This Dll knows whether to self-install by checking a registry entry that is set upon installation.  If you delete the files without calling
        /// uninstall, be sure to delete HKLM\SOFTWARE\Microsoft Research\ConferenceXP\ConferenceAPIInstalled before reinstalling Rtp in another location.  The clean way to
        /// uninstall without MSIs or Add/Remove Programs is to call "installutil /u Conference.dll" before deleting the file or by having your
        /// app call Installer.Uninstall on the Conference assembly programatically.  See Installation.cs for an example of how to programmatically
        /// install/uninstall an assembly dependency.
        /// </summary>
        public static void CheckInstalled()
        {
            if (!Installation.Installed)
            {
                // Install myself
                IDictionary state = new Hashtable();
                state.Clear();
                Installation inst = new Installation();
                inst.Install(state);
                inst.Commit(state);
            }

        }
        #endregion

        #region Enums
        public enum AutoPositionMode
        {
            None,
            Tiled,
            Tiled2x,
            FourWay,
            FullScreen
        }
        #endregion
    }
}

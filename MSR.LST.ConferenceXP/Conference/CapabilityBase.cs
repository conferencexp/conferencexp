// SFHashTableDiagnostics allows you to trace the history of the content of the hashtables and 
// ArrayList related to form sharing management
// #define SFHashTableDiagnostics

using System;
using System.Collections;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using MSR.LST.Net.Rtp;


namespace MSR.LST.ConferenceXP
{
    /// <summary>
    /// Capability Class
    /// </summary>
    /// <remarks>
    /// Technical explanations and data structure used for shared forms implementation based on CNAME:
    /// ==============================================================================================
    /// We have 2 nested hashtables: htIdToFormTypes that is the global static hashtable ID -> htFormTypes.
    /// The embedded hashtable htFormTypes has the following key/value pair types: formType -> form instance. 
    /// We defined a ISharedForm interface which contains the following methods: 
    /// AddCapability(ICapability capability), RemoveCapability(ICapability capability) and int Count().
    /// We also created a SharedForm class (that implements the ISharedForm interface) and keep track of 
    /// capability objects added/removed in an ArrayList.
    /// So, a shared form must implement the ISharedForm interface and create an instance of a SharedForm 
    /// object. By doing that, every form objects stored inside the htFormTypes hashtable will contain a 
    /// SharedForm object with an updated ArraList of capability objects referring to it at a given time.
    /// We call the AddCapability method of the corresponding form of every capability calling the 
    /// CreateForm method. Ditto with RemoveCapability for CloseForm method.
    /// </remarks>
    public class Capability : ICapability, IDisposable
    {
        #region Statics

        // PEP - private extension prefix
        private const string PEP_CHANNEL = "CH";
        private const string PEP_IDENTIFIER = "CI";
        private const string PEP_SHAREDFORM = "SF";

        public static DynamicProperties DynaPropsFromRtpStream(RtpStream rtpStream)
        {
            DynamicProperties dynaProps = new DynamicProperties();

            dynaProps.ID = IDFromRtpStream(rtpStream);
            dynaProps.Name = rtpStream.Properties.Name;
            dynaProps.SharedFormID = SharedFormIDFromRtpStream(rtpStream);

            // Establish the channelness or ownership of this capability
            CapabilityType streamType = ChannelFromRtpStream(rtpStream);
            // by default, capabilities are owned (i.e. the "unknown" streams are considered owned)
            dynaProps.Channel = (streamType == CapabilityType.Channel);
            if (streamType == CapabilityType.Owned) // the stream is being sent by the owner
            {
                dynaProps.OwnerName = rtpStream.Properties.CName;
            }

            return dynaProps;
        }

        public static CapabilityType ChannelFromRtpStream(RtpStream rtpStream)
        {
            string chan = rtpStream.Properties.GetPrivateExtension(PEP_CHANNEL);
            if(chan != null)
            {
                if (Boolean.Parse(chan))
                {
                    return CapabilityType.Channel;
                }
                else
                {
                    return CapabilityType.Owned;
                }
            }
            else
            {
                return CapabilityType.Unknown;
            }
        }

        public static Guid IDFromRtpStream(RtpStream rtpStream)
        {
            string id = rtpStream.Properties.GetPrivateExtension(PEP_IDENTIFIER);
            if(id != null)
            {
                return new Guid(id);
            }
            else
            {
                throw new InvalidOperationException(Strings.StreamLacksACapabilityidentifier);
            }
        }

        public static Guid SharedFormIDFromRtpStream(RtpStream rtpStream)
        {
            string sfid = rtpStream.Properties.GetPrivateExtension(PEP_SHAREDFORM);
            if(sfid != null)
            {
                return new Guid(sfid);
            }
            else
            {
                return Guid.Empty;
            }
        }

        /// <summary>
        /// Returns whether the Capability is runable in its current environment.
        /// Typically this means checking for the existance of all dependencies.
        /// </summary>
        /// <remarks>
        /// IsRunable allows the developer to circumvent certain undesired behavior
        /// by preventing the Capability from being loaded at startup, should the method return false.
        /// </remarks>
        /// <example>  
        /// This is an example for overriding this base method:
        /// 
        /// public static new bool IsRunable
        /// {
        ///     get
        ///     {
        ///         try
        ///         {
        ///             // find a DLL (or) look up registry settings, etc.
        ///         }
        ///         catch
        ///         {
        ///             return false;
        ///         }
        ///     }
        /// }
        /// </example>
        static public bool IsRunable
        {
            get
            {
                return true;
            }
        }
       
        
        #endregion Statics

        #region Members

        private ArrayList rtpSenders = new ArrayList();
        private MemoryStream msSend = null;
        private MemoryStream msSendBackground = null;
        private BinaryFormatter bfSend = null;
        private BinaryFormatter bfSendBackground = null;
        private BinaryFormatter bf = new BinaryFormatter();
        private bool autoPlayLocal = true;
        private bool autoPlayRemote = true;

        // On another day, we should make this static.  In the meantime, we should treat it as such... :)
        protected Properties capProps;

        protected DynamicProperties dynaProps;

        private delegate void AsyncNullParams();
        private AsyncNullParams beginPlayDelegate;
        private AsyncNullParams beginStopPlayingDelegate;
        private AsyncNullParams beginSendDelegate;
        private AsyncNullParams beginStopSendingDelegate;

        public delegate void SendObjectHandler(object o);
        private SendObjectHandler beginSendObjectDelegate;
        private SendObjectHandler beginSendObjectBackgroundDelegate;

        protected Type formType = null;
        protected System.Windows.Forms.Form form = null;

        protected static ConferenceEventLog eventLog = new ConferenceEventLog(ConferenceEventLog.Source.Capability);
        protected string name = null;
        protected Guid uniqueID = Guid.Empty;
        protected string ownerCName = null;
        protected bool isChannel;
        protected MSR.LST.ConferenceXP.PayloadType payloadType = MSR.LST.ConferenceXP.PayloadType.Unknown;

        /// <summary>
        /// Used to associate capabilities (and their streams) in a "shared" form
        /// </summary>
        protected Guid sharedFormID = Guid.Empty;

        protected System.Drawing.Image icon = null;
        protected bool isPlaying = false;
        protected bool isSending = false;
        protected object tag = null;
        protected RtpSender rtpSender = null;
        protected RtpSender rtpSenderBackground = null;
        protected ArrayList rtpStreams = new ArrayList();
        protected bool disposed = false;
        protected int maximumBandwidthLimiter = 0;
        protected short delayBetweenPackets = 0;
        protected bool isSender = false;
        protected delegate void VoidDelegate();

        // Global hashtable to manage capability forms: ID or SharedFormID -> htFormTypes object
        private static Hashtable htIdToFormTypes = new Hashtable();

        #endregion Members
        
        #region Constructors

        // Required ctor for ICapabilitySender (should we pass in a Name?)
        protected Capability()
        {
            this.uniqueID = Guid.NewGuid();
            this.isSender = true;

            Initialize();

            this.name = capProps.Name;

            this.isChannel = capProps.Channel;
            if( !isChannel ) // if it's not a channel, then it has an owner
            {
                this.ownerCName = Conference.LocalParticipant.Identifier;
            }
        }

        // Required ctor for ICapabilityViewer
        protected Capability(DynamicProperties dynaProps)
        {
            // Read static and overridden properties
            Initialize();

            // Use the properties provided by the capability
            uniqueID = dynaProps.ID;
            name = dynaProps.Name;
            sharedFormID = dynaProps.SharedFormID;
            isChannel = dynaProps.Channel;
            ownerCName = dynaProps.OwnerName;

            isSender = false;
        }

        internal void Initialize()
        {
            InitializeAttributes();
            AppConfigOverride();

            formType = capProps.FormType;

            if (payloadType == MSR.LST.ConferenceXP.PayloadType.Unknown)
            {
                payloadType = capProps.PayloadType;
            }

            MaximumBandwidthLimiter = capProps.MaxBandwidth;

            // Initialize the asynchronous delegate used for BeginSendObject/EndSendObject
            beginSendObjectDelegate = new SendObjectHandler(SendObject);
            beginSendObjectBackgroundDelegate = new SendObjectHandler(SendObjectBackground);
            beginPlayDelegate = new AsyncNullParams(Play);
            beginStopPlayingDelegate = new AsyncNullParams(StopPlaying);
            beginSendDelegate = new AsyncNullParams(Send);
            beginStopSendingDelegate = new AsyncNullParams(StopSending);
        }

        
        /// <summary>
        /// Please do not call this method directly.  Call Conference.DisposeCapability so that 
        /// Conference can clean up its collections and raise events, as well as call this method.
        /// </summary>
        public virtual void Dispose()
        {
            lock(this)
            {
                if (!disposed)
                {
                    disposed = true;
                    
                    if(isPlaying)
                    {
                        StopPlaying();
                    }

                    if(isSending)
                    {
                        StopSending();
                    }

                    if (rtpStreams != null)
                    {
                        // There may be streams that didn't get cleaned up correctly
                        foreach(RtpStream rtpStream in (ArrayList)rtpStreams.Clone())
                        {
                            StreamRemoved(rtpStream);
                        }

                        rtpStreams.Clear();
                        rtpStreams = null;
                    }

                    // Clean up our owner, if we are an owned capability
                    if (Owner != null)
                    {
                        Owner.RemoveCapabilityViewer(this);
                    }

                    tag = null;
                    
                    if(msSend != null)
                    {
                        msSend.Close();
                        msSend = null;
                    }

                    bfSend = null;

                    if(msSendBackground != null)
                    {
                        msSendBackground.Close();
                        msSendBackground = null;
                    }

                    bfSendBackground = null;
                }
            }
        }
      
        #endregion Constructors

        #region Public 

        public RtpSender RtpSender
        {
            get
            {
                return rtpSender;
            }
        }
        public virtual bool AutoPlayLocal
        {
            get{return autoPlayLocal;}
            set{autoPlayLocal = value;}
        }
        public virtual bool AutoPlayRemote
        {
            get{return autoPlayRemote;}
            set{autoPlayRemote = value;}
        }
        public Guid ID
        {
            get
            {
                return uniqueID;
            }
        }
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                
                if (form != null)
                {
                    form.Text = name;
                }
            }
        }
        public PayloadType PayloadType
            {
            get
            {
                return payloadType;
            }
        }
        /// <summary>
        /// Accessor to the shared form type. A form can be shared by CNAME (implemented) 
        /// or by stream type (not implemented). 
        /// </summary>
        public Guid SharedFormID
        {
            get{return sharedFormID;}
            set{sharedFormID = value;}
        }
        public bool UsesSharedForm
        {
            get{return sharedFormID != Guid.Empty;}
        }

        public System.Drawing.Image Icon
        {
            get
            {
                return icon;
            }
        }
        public System.Windows.Forms.Form Form
        {
            get{return form;}
        }

        public virtual bool IsPlaying
        {
            get
            {
                return isPlaying;
            }
        }
        public virtual bool IsSending
        {
            get
            {
                return isSending;
            }
        }
        public virtual bool IsSender
        {
            get
            {
                return isSender;
            }
        }
        public object Tag
        {
            get
            {
                return tag;
            }
            set
            {
                tag = value;
            }
        }
        public bool Channel
        {
            get
            {
                return this.isChannel;
            }
        }
        public bool Owned
        {
            get
            {
                return !this.isChannel;
            }
        }
        public IParticipant Owner
        {
            get
            {
                if( ownerCName != null )
                    return Conference.participants[ownerCName];
                else
                    return null;
            }
            set
            {
                this.ownerCName = value.Identifier;
            }
        }
        public IParticipant[] Participants
        {
            get
            {
                lock(this)
                {
                    if (rtpStreams.Count == 0)
                    {
                        return new IParticipant[0];
                    }
                    else
                    {
                        if (rtpStreams.Count == 0)
                        {
                            return new IParticipant[0];
                        }

                        int size = rtpStreams.Count;
                        IParticipant[] ps = new IParticipant[size];
                        for (int i = 0; i < size; i++)
                        {
                            RtpStream rtpStream = (RtpStream)rtpStreams[i]; 
                            ps[i] = Conference.participants[rtpStream.Properties.CName];
                        }
                        return ps;
                    }
                }
            }
        }
        public RtpStream[] RtpStreams
        {
            get
            {
                lock(this)
                {
                    if (rtpStreams == null || rtpStreams.Count == 0)
                    {
                        return new RtpStream[0];
                    }
                    else
                    {
                        return (RtpStream[])(new ArrayList(rtpStreams).ToArray(typeof(RtpStream)));
                    }
                }
            }
        }

        /// <summary>
        /// In bytes per second
        /// </summary>
        public int MaximumBandwidthLimiter
        {
            get
            {
                return maximumBandwidthLimiter;
            }
            set
            {
                if ( value < 0 || value > 100000000 )
                {
                    throw new ArgumentException(Strings.MaximumBandwidthlimiterError);
                }
                maximumBandwidthLimiter = value;
                if ( maximumBandwidthLimiter == 0 )
                {
                    delayBetweenPackets = 0;
                }
                else
                {
                    float maximumPacketsPerSecond = (float)maximumBandwidthLimiter / (float)Rtp.MAX_PACKET_SIZE;
                    delayBetweenPackets = (short) ( 1000 / ( maximumPacketsPerSecond ) );
                    if (delayBetweenPackets > 30 )
                    {
                        delayBetweenPackets = 30;
                    }
                }

                if (rtpSender != null)
                {
                    rtpSender.DelayBetweenPackets = delayBetweenPackets;
                }

                if (rtpSenderBackground != null)
                {
                    rtpSenderBackground.DelayBetweenPackets = delayBetweenPackets;
                }
            }
        }

        public bool FecEnabled
        {
            get { return capProps.Fec; }
            set { capProps.Fec = value; }
        }

        public ushort FecData
        {
            get { return capProps.FecData; }
        }

        public ushort FecChecksum
        {
            get { return capProps.FecChecksum; }
        }


        public virtual void StreamAdded(RtpStream rtpStream)
        {
            ValidateStream(rtpStream);

            lock(this)
            {
                if (isPlaying)
                {
                    rtpStream.FrameReceived += new RtpStream.FrameReceivedEventHandler(frameReceived);
                }

                rtpStreams.Add(rtpStream);
            }
        }
        public virtual void StreamRemoved(RtpStream rtpStream)
        {
            lock(this)
            {
                if(isPlaying)
                {
                    rtpStream.FrameReceived -= new RtpStream.FrameReceivedEventHandler(frameReceived);
                }

                rtpStreams.Remove(rtpStream);
            }

            // We keep playing regardless of who leaves if it's a capability channel.  
            //    If it's not a channel, then close it if the owner leaves
            // (pfb, 21-Sep-04) Removing code here because Conference checks for no streams left.
        }


        /// <summary>
        /// Note:  We're currently investigating thread safety problems around the Begin* methods
        /// </summary>
        public IAsyncResult BeginPlay(AsyncCallback callback, object state)
        {
            return beginPlayDelegate.BeginInvoke(callback, state);
        }
        /// <summary>
        /// Note:  We're currently investigating thread safety problems around the Begin* methods
        /// </summary>
        public IAsyncResult BeginSend(AsyncCallback callback, object state)
        {
            return beginSendDelegate.BeginInvoke(callback, state);
        }
        /// <summary>
        /// Note:  We're currently investigating thread safety problems around the Begin* methods
        /// </summary>
        public IAsyncResult BeginStopPlaying(AsyncCallback callback, object state)
        {
            return beginStopPlayingDelegate.BeginInvoke(callback, state);
        }
        /// <summary>
        /// Note:  We're currently investigating thread safety problems around the Begin* methods
        /// </summary>
        public IAsyncResult BeginStopSending(AsyncCallback callback, object state)
        {
            return beginStopSendingDelegate.BeginInvoke(callback, state);
        }

        public virtual void Play()
        {
            lock(this)
            {
                if (disposed)
                {
                    throw new ObjectDisposedException(name);
                }

                if (Conference.ActiveVenue == null)
                {
                    throw new Exception(Strings.UnableToPlayACapability);
                }

                try
                {
                    if(!isPlaying)
                    {
                        isPlaying = true;

                        if (formType != null)
                        {
                            CreateForm();
                            ShowForm();
                        }

                        foreach(RtpStream rtpStream in (ArrayList)rtpStreams.Clone())
                        {                    
                            rtpStream.FrameReceived += new RtpStream.FrameReceivedEventHandler(frameReceived);
                        }

                        Conference.RaiseCapabilityPlaying(this);
                    }
                }
                catch (Exception e)
                {
                    eventLog.WriteEntry(string.Format(CultureInfo.CurrentCulture, Strings.CapabilityPlay, 
                        this.ToString(), e.ToString()), EventLogEntryType.Error, 99);
                }
            }
        }

        public virtual void Send()
        {
            lock(this)
            {
                if (disposed)
                {
                    throw new ObjectDisposedException(name);
                }

                if (Conference.ActiveVenue == null)
                {
                    throw new Exception(Strings.UnableToCallSend);
                }

                try
                {
                    if(!isSending)
                    {
                        isSending = true;

                        CreateRtpSenders();

                        if (formType != null)
                        {
                            CreateForm();
                            ShowForm();
                        }

                        Conference.RaiseCapabilitySending(this);
                    }
                }
                catch (System.Runtime.InteropServices.COMException)
                {
                    string msg = string.Format(CultureInfo.CurrentCulture, Strings.JournalViewerNotInstalled);
                    throw new ApplicationException(msg);
                }
                catch (Exception e)
                {
                    eventLog.WriteEntry(string.Format(CultureInfo.CurrentCulture, Strings.CapabilitySend, 
                        this.ToString(), e.ToString()), EventLogEntryType.Error, 99);
                }
            }
        }
        public virtual void StopPlaying()
        {
            lock(this)
            {
                if(isPlaying)
                {
                    isPlaying = false;

                    // Unhook the existing streams before closing the form, in case data comes in
                    // while we are shutting down
                    foreach(RtpStream rtpStream in (ArrayList)rtpStreams.Clone())
                    {
                        rtpStream.FrameReceived -= new RtpStream.FrameReceivedEventHandler(frameReceived);
                    }

                    if(formType != null)
                    {
                        CloseForm();
                    }

                    Conference.RaiseCapabilityStoppedPlaying(this);
                }
            }
        }

        public virtual void StopSending()
        {
            lock(this)
            {
                if(isSending)
                {
                    isSending = false;
 
                    // Call stop playing before closing the form, in case data comes in while we
                    // are shutting down.
                    if (CapabilityViewer != null)
                    {
                        CapabilityViewer.StopPlaying();
                    }

                    if(formType != null)
                    {
                        CloseForm();
                    }

                    DisposeRtpSenders();

                    Conference.RaiseCapabilityStoppedSending(this);
                }
            }
        }

        /// <summary>
        /// Create a form on the main UI thread
        /// </summary>
        protected void CreateForm()
        {
            if(formType == null)
            {
                string msg = Strings.FormTypeCanNotBeNull;

                Debug.Assert(false, "FormType can not be null when calling CreateForm");
                throw new ApplicationException(msg);
            }

            Conference.FormInvoke(new VoidDelegate(_CreateForm), null);
        }

        /// <summary>
        /// Create a capability form
        /// </summary>
        private void _CreateForm()
        {
            Guid id = UsesSharedForm ? SharedFormID : ID;

            #region Diagnostics
            #if SFHashTableDiagnostics
            Trace.WriteLine(System.Environment.NewLine + "_CreateForm shared form for" + this.GetType());
            Trace.WriteLine("CNAME: " + this.ownerCName);
            Trace.WriteLine("FormType: " + formType.ToString());
            #endif
            #endregion Diagnostics

            // Get (or add) the embedded form type hashtable from the global CNAME hashtable 
            Hashtable htFormTypes = (Hashtable)htIdToFormTypes[id];
            if (htFormTypes == null)
            {
                htFormTypes = new Hashtable();
                htIdToFormTypes.Add(id, htFormTypes);

                #region Diagnostics
                #if SFHashTableDiagnostics
                    Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, 
                        "Added a new entry (for a htFormTypes) in htIdToFormTypes with the key  {0}", 
                        this.ownerCName));
                #endif
                #endregion Diagnostics
            }

            // Get (or add) the embedded form from the embedded htFormTypes hashtable
            // Create the form if it does not exist (first capability to refer to this form)
            form = (System.Windows.Forms.Form)htFormTypes[formType];
            if (form == null)
            {
                // Instantiate a new form
                form = (System.Windows.Forms.Form)Activator.CreateInstance(formType);
                htFormTypes.Add(formType, (CapabilityForm)form);

                #region Diagnostics
                #if SFHashTableDiagnostics
                    Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, 
                        "Added a new entry (for a ISharedForm) in htFormTypes with the key formType {0}", 
                        formType.ToString()));
                #endif
                #endregion Diagnostics
            }

            // Call the AddCapability of the appropriate shared form and pass to it the
            // current capability object (this)
            ((CapabilityForm)form).AddCapability(this);

            #region Diagnostics
            #if SFHashTableDiagnostics
                Trace.WriteLine("after update: htIdToFormTypes.Count: " + htIdToFormTypes.Count);
                Trace.WriteLine("after update: htFormTypes.Count: " + htFormTypes.Count + 
                    System.Environment.NewLine);
            #endif
            #endregion Diagnostics
        }

        /// <summary>
        /// Show form on the main UI thread
        /// </summary>
        public virtual void ShowForm()
        {
            Conference.FormInvoke(new VoidDelegate(_ShowForm), null);
        }

        /// <summary>
        /// Set the form name and show the form
        /// </summary>
        private void _ShowForm()
        {
            if(disposed)
            {
                throw new ObjectDisposedException(name);
            }

            form.Text = this.Name;
            form.Show();
        }

        /// <summary>
        /// Close the form on the main UI thread
        /// </summary>
        public virtual void CloseForm()
        {
            Conference.FormInvoke(new VoidDelegate(_CloseForm), null);
        }

        /// <summary>
        /// Close a capability form - the form will be actually closed only if no capabilities
        /// are referring to it.
        /// </summary>
        private void _CloseForm()
        {
            Guid id = UsesSharedForm ? SharedFormID : ID;

            #region Diagnostics
            #if SFHashTableDiagnostics
                Trace.WriteLine("_CloseForm shared form for " + this.GetType());
                Trace.WriteLine("CNAME: " + this.ownerCName);
                Trace.WriteLine("FormType: " + formType.ToString());
            #endif
            #endregion Diagnostics

            // Extract the htFormTypes corresponding to the capability
            Hashtable htFormTypes = (Hashtable)htIdToFormTypes[id];
            if(htFormTypes == null)
            {
                string msg = Strings.RemoveCapabilityFormHashtable;

                Debug.Assert(false, "You are trying to access / remove a capability that is not in form's hashtable");
                throw new ArgumentNullException(msg);
            }

            // Extract the form corresponding to the form type
            CapabilityForm capForm = (CapabilityForm)htFormTypes[formType];
            if(capForm == null)
            {
                string msg = Strings.RemoveCapabilityFormTypeHashtable;

                Debug.Assert(false, "You are trying to access / remove a capability that is not in form's hashtable");
                throw new ArgumentNullException(msg);
            }

            // Remove the capability from the array list
            if(capForm.RemoveCapability(this))
            {
                if (capForm.Count() == 0)
                {
                    // There is no more capability refering to this form, so close the form
                    form.Close();
                    form.Dispose();

                    // Remove the entry for this formType
                    htFormTypes.Remove(formType);

                    // This is the collection that keeps track of where capability forms are on
                    // the screen for tiling purposes.  Once our form disappears, we need to
                    // update the collection.
                    Conference.RemoveForm(id);

                    #region Diagnostics
                    #if SFHashTableDiagnostics
                        Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, 
                            "Close the form + Removed the entry key formType {0}", formType.ToString()));
                    #endif
                    #endregion Diagnostics

                    // Check if the htFormTypes hashtable is empty
                    if (htFormTypes.Count == 0)
                    {
                        // Remove the entry for the CNAME
                        htIdToFormTypes.Remove(id);

                        #region Diagnostics
                        #if SFHashTableDiagnostics
                            Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, 
                                "Close the form + Removed the entry key formType {0}", formType.ToString()));
                        #endif
                        #endregion Diagnostics
                    }
                }

                // Always clean up local member variable, even if we aren't closing the form
                form = null;
            }

            #region Diagnostics
            #if SFHashTableDiagnostics
                Trace.WriteLine("after update: htIdToFormTypes.Count: " + htIdToFormTypes.Count);
                Trace.WriteLine("after update: htFormTypes.Count: " 
                    + htFormTypes.Count + System.Environment.NewLine);
            #endif
            #endregion Diagnostics
        }


        public void SendObject(object o)
        {
            _SendObject(o, rtpSender, msSend, bfSend);
        }

        public void SendObjectBackground(object o)
        {
            if(!capProps.BackgroundSender)
            {
                throw new ApplicationException(Strings.SpecifyBackgroundSenderAttribute);
            }

            _SendObject(o, rtpSenderBackground, msSendBackground, bfSendBackground);
        }

        private void _SendObject(object o, RtpSender rtpSender, MemoryStream ms, BinaryFormatter bf)
        {
            if (disposed)
            {
                throw new ObjectDisposedException(name);
            }

            if(!isSending)
            {
                throw new ApplicationException(string.Format(CultureInfo.CurrentCulture, 
                    Strings.CallSendBeforeSendObject, name));
            }

            ms.Position = 0; // set the "useful bytes" "pointer" back to 0
            bf.Serialize(ms, o); // serialize, which puts the "useful bytes pointer" at the end of hte useful data

            // Get a byte[] of the serialized object
            int numBytes = (int)ms.Position; // record the number of "useful bytes"
            byte[] byteObj = new Byte[numBytes];
            ms.Position = 0; // set the pointer back to 0, so we can read from that point
            ms.Read(byteObj, 0, numBytes); // read all the useful bytes

            rtpSender.Send(byteObj);
        }        
        
        public IAsyncResult BeginSendObject(object o, AsyncCallback callback, object state)
        {
            return beginSendObjectDelegate.BeginInvoke(o, callback, state);
        }

        public void EndSendObject(IAsyncResult iar)
        {
            beginSendObjectDelegate.EndInvoke(iar);
        }

        public IAsyncResult BeginSendObjectBackground(object o, AsyncCallback callback, object state)
        {
            return beginSendObjectBackgroundDelegate.BeginInvoke(o, callback, state);
        }

        public void EndSendObjectBackground(IAsyncResult iar)
        {
            beginSendObjectBackgroundDelegate.EndInvoke(iar);
        }


        public ICapabilityViewer CapabilityViewer
        {
            get{return (ICapabilityViewer)Conference.CapabilityViewers[uniqueID];}
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, 
                "Capability [ Identifier == {0}, Sender == {1}, Name == {2}, PayloadType == {3} ]", ID, 
                ownerCName, Name, payloadType);
        }
        
        
        #endregion Public

        #region Private

        private void frameReceived(object sender, RtpStream.FrameReceivedEventArgs frea)
        {
            MemoryStream ms = new System.IO.MemoryStream((byte[])frea.Frame);
            object o = bf.Deserialize(ms);

            Participant participant = (Participant)Conference.participants[frea.RtpStream.Properties.CName];

            Conference.FormInvoke(ObjectReceived, new object[] { this, new ObjectReceivedEventArgs(participant, o) } );
        }


        
        private void InitializeAttributes()
        {
            #region Collect Attributes

            Hashtable ht = new Hashtable();

            foreach(Attribute attrib in Attribute.GetCustomAttributes(this.GetType()))
            {
                ht.Add(attrib.GetType(), attrib);
            }

            Attribute a;

            #endregion Collect Attributes

            #region Name

            if((a = (Attribute)ht[typeof(NameAttribute)]) != null)
            {
                capProps.Name = ((NameAttribute)a).Name;
            }
            else
            {
                throw new ArgumentException(Strings.CapabilityNameAttributeMustBeSet);
            }

            #endregion Name

            #region PayloadType

            if((a = (Attribute)ht[typeof(PayloadTypeAttribute)]) != null)
            {
                capProps.PayloadType = ((PayloadTypeAttribute)a).PayloadType;
            }
            else
            {
                throw new ArgumentException(Strings.CapabilityPayloadTypeMustBeSet);
            }

            #endregion PayloadType

            #region FormType

            if((a = (Attribute)ht[typeof(FormTypeAttribute)]) != null)
            {
                if (! Conference.CallingFormSet)
                {
                    throw new Exception(Strings.CannotUseCapabilitiesUnless);
                }

                // TODO - one of these isn't needed JVE
                capProps.FormType = ((FormTypeAttribute)a).FormType;
                this.formType = capProps.FormType;
            }

            #endregion FormType

            #region Channel
            
            if((a = (Attribute)ht[typeof(ChannelAttribute)]) != null)
            {
                capProps.Channel = ((ChannelAttribute)a).Channel;
            }

            #endregion Channel

            #region BackgroundSender
            
            if((a = (Attribute)ht[typeof(BackgroundSenderAttribute)]) != null)
            {
                capProps.BackgroundSender = ((BackgroundSenderAttribute)a).BackgroundSender;
            }

            #endregion BackgroundSender

            #region Fec

            if((a = (Attribute)ht[typeof(FecAttribute)]) != null)
            {
                capProps.Fec = ((FecAttribute)a).Fec;
            }

            if((a = (Attribute)ht[typeof(FecRatioAttribute)]) != null)
            {
                capProps.FecChecksum = ((FecRatioAttribute)a).Checksum;
                capProps.FecData = ((FecRatioAttribute)a).Data;
            }

            #endregion Fec

            #region Bandwidth

            if((a = (Attribute)ht[typeof(MaxBandwidthAttribute)]) != null)
            {
                capProps.MaxBandwidth = ((MaxBandwidthAttribute)a).MaxBandwidth;
            }

            #endregion Bandwidth

            #region Version

            if((a = (Attribute)ht[typeof(VersionAttribute)]) != null)
            {
                capProps.Version = ((VersionAttribute)a).Version;
            }

            #endregion Version

            #region Codebase

            if((a = (Attribute)ht[typeof(CodebaseAttribute)]) != null)
            {
                capProps.Codebase = ((CodebaseAttribute)a).Codebase;
            }

            #endregion Codebase

            #region Developer

            if((a = (Attribute)ht[typeof(DeveloperAttribute)]) != null)
            {
                capProps.Developer = ((DeveloperAttribute)a).Developer;
            }

            #endregion Developer

            #region Description

            if((a = (Attribute)ht[typeof(DescriptionAttribute)]) != null)
            {
                capProps.Description = ((DescriptionAttribute)a).Description;
            }

            #endregion Description

            #region Homepage

            if((a = (Attribute)ht[typeof(HomepageURLAttribute)]) != null)
            {
                capProps.HomepageURL = ((HomepageURLAttribute)a).HomepageURL;
            }

            #endregion Homepage

            #region Guid

            if((a = (Attribute)ht[typeof(GuidAttribute)]) != null)
            {
                capProps.Guid = ((GuidAttribute)a).Guid;
            }

            #endregion Guid
        }

        /// <summary>
        /// Override settings using App.Config values
        /// </summary>
        private void AppConfigOverride()
        {
            string key;
            string val;

            #region Fec

            // Insert name of capability into key for lookup in App.Config
            key = AppConfig.CXP_Capability_Fec.Insert(
                AppConfig.CXP_Capability_Fec.IndexOf("..") + 1, capProps.Name);

            if ((val = ConfigurationManager.AppSettings[key]) != null)
            {
                capProps.Fec = bool.Parse(val);
            }

            // Insert name of capability into key for lookup in App.Config
            key = AppConfig.CXP_Capability_FecRatio.Insert( 
                AppConfig.CXP_Capability_FecRatio.IndexOf("..") + 1, capProps.Name);

            if((val = ConfigurationManager.AppSettings[key]) != null)
            {
                // Parse the string into 2 ints
                string[] args = val.Split(new char[]{':'});

                if(args.Length != 2)
                {
                    throw new ArgumentException(Strings.FecRatioRequires2Ints);
                }

                capProps.FecData = ushort.Parse(args[0], CultureInfo.InvariantCulture);
                capProps.FecChecksum = ushort.Parse(args[1], CultureInfo.InvariantCulture);
            }

            #endregion Fec

            #region Bandwidth

            // Insert name of capability into key for lookup in App.Config
            key = AppConfig.CXP_Capability_Bandwidth.Insert( 
                AppConfig.CXP_Capability_Bandwidth.IndexOf("..") + 1, capProps.Name);

            if((val = ConfigurationManager.AppSettings[key]) != null)
            {
                capProps.MaxBandwidth = int.Parse(val, CultureInfo.InvariantCulture);
            }

            #endregion Bandwidth

            #region Channel

            // Insert name of capability into key for lookup in App.Config
            key = AppConfig.CXP_Capability_Channel.Insert( 
                AppConfig.CXP_Capability_Channel.IndexOf("..") + 1, capProps.Name);

            if((val = ConfigurationManager.AppSettings[key]) != null)
            {
                capProps.Channel = bool.Parse(val);
            }

            #endregion Channel

        }
        
        
        private RtpSender CreateRtpSender()
        {
            RtpSenderProperties props;
            props.ID = this.uniqueID;
            props.Name = name;
            props.DelayBetweenPackets = delayBetweenPackets;
            props.Channel = Channel;
            props.OwnedByLocalParticipant = (Owner == Conference.LocalParticipant);
            props.SharedFormID = SharedFormID;
            props.PayloadType = (MSR.LST.Net.Rtp.PayloadType)payloadType;
            props.FecEnabled = capProps.Fec;
            props.FecChecksum = capProps.FecChecksum;
            props.FecData = capProps.FecData;
            
            return CreateRtpSender(props);
        }

        protected RtpSender CreateRtpSender(RtpSenderProperties props)
        {
            // Setup private extensions
            Hashtable priExns = new Hashtable();

            // Add unique ID
            Debug.Assert(props.ID != Guid.Empty);
            priExns.Add(PEP_IDENTIFIER, props.ID.ToString());

            // Set the delay between packets now, so that it happens in the constructor and goes out with
            //  the first RTCP packet
            priExns.Add(Rtcp.PEP_DBP, props.DelayBetweenPackets.ToString(CultureInfo.InvariantCulture));

            // Set the channel-ness or ownership of the stream...
            if(props.Channel && props.OwnedByLocalParticipant)
            {
                throw new ArgumentException(Strings.CannotBeChannelAndOwned);
            }
            if(props.Channel)
            {
                priExns.Add(PEP_CHANNEL, true.ToString());
            }
            else if(props.OwnedByLocalParticipant)
            {
                priExns.Add(PEP_CHANNEL, false.ToString());
            }
            // else neither are true (i.e. it's owned but not by the local participant - a concept only possible in CXP...)

            // For capabilities with shared forms only
            if(props.SharedFormID != Guid.Empty)
            {
                priExns.Add(PEP_SHAREDFORM, props.SharedFormID.ToString());
            }

            // Create the sender
            RtpSender rtpSender = null;
            
            if(props.FecEnabled && props.FecChecksum > 0)
            {
                rtpSender = Conference.RtpSession.CreateRtpSenderFec(props.Name, 
                    props.PayloadType, priExns, props.FecData, props.FecChecksum);
            }
            else
            {
                rtpSender = Conference.RtpSession.CreateRtpSender(props.Name, 
                    props.PayloadType, priExns);
            }
            
            // Add to collection
            rtpSenders.Add(rtpSender);

            return rtpSender;
        }

        protected void DisposeRtpSender(RtpSender rtpSender)
        {
            if(!rtpSenders.Contains(rtpSender))
            {
                throw new ArgumentException(Strings.DidNotOriginateFromHere);
            }

            rtpSenders.Remove(rtpSender);
            rtpSender.Dispose();
        }

        protected virtual void CreateRtpSenders()
        {
            // If the derived class hasn't created its own RtpSender, create one for it
            if (rtpSender == null)
            {
                rtpSender = CreateRtpSender();
                msSend = new MemoryStream();
                bfSend = new BinaryFormatter();
            }

            // If the derived class hasn't created its own RtpSenderBackground, create one for it
            if(capProps.BackgroundSender)
            {
                if (rtpSenderBackground == null)
                {
                    rtpSenderBackground = CreateRtpSender();
                    msSendBackground = new MemoryStream();
                    bfSendBackground = new BinaryFormatter();
                }
            }
        }

        protected void ValidateForm()
        {
            if(form == null)
            {
                throw new Exception(Strings.NoFormSetForThisCapability);
            }
        }

        protected void DisposeRtpSenders()
        {
            if (this.rtpSender != null)
            {
                DisposeRtpSender(this.rtpSender);
                this.rtpSender = null;
            }

            if(rtpSenderBackground != null)
            {
                DisposeRtpSender(rtpSenderBackground);
                rtpSenderBackground = null;
            }

            // Dispose any other RtpSenders that were created
            foreach(RtpSender rtpSender in (ArrayList)rtpSenders.Clone())
            {
                DisposeRtpSender(rtpSender);
            }
        }
        

        private void ValidateStream(RtpStream rtpStream)
        {
            // Make sure that the capability properties of the streams agree
            if((ChannelFromRtpStream(rtpStream) == CapabilityType.Channel) != Channel ||
                IDFromRtpStream(rtpStream) != ID  ||
                SharedFormIDFromRtpStream(rtpStream) != SharedFormID)
            {
                throw new ArgumentException(Strings.StreamDoesNotMatchSettings);
            }
        }

        #endregion Private

        #region Events
        // This event is public so that a reference to a derived classes (that are in a separate assembly for the Capability) can be used
        // to receive objects.  Internal doesn't work because it's in a new assembly and protected doesn't work because a second class (say a
        // WinForm must be able to hook the event off the Capability instance.
        public event CapabilityObjectReceivedEventHandler ObjectReceived;
        #endregion
    
        #region Attributes

        // TODO - combine these 3 structs into 1? jasonv 1/14/2005

        public struct RtpSenderProperties
        {
            public Guid ID;
            public short DelayBetweenPackets;
            public bool Channel;
            public bool OwnedByLocalParticipant;
            public Guid SharedFormID;
            public string Name;
            public MSR.LST.Net.Rtp.PayloadType PayloadType;
            public bool FecEnabled;
            public ushort FecData;
            public ushort FecChecksum;
        }

        public struct DynamicProperties
        {
            // Static attributes of the capability, overrideable in app.config
            public bool Channel;

            public string Name;

            // Completely dynamic - generated at run time or received from remote stream
            public Guid ID;
            public Guid SharedFormID;
            public string OwnerName;
        }

        public struct Properties
        {
            public bool Channel;
            public bool Fec;
            public bool BackgroundSender;

            public ushort FecData;
            public ushort FecChecksum;
            public int MaxBandwidth;
            
            public string Name;
            public string Version;
            public string Codebase;
            public string Developer;
            public string Description;
            public string HomepageURL;

            public Type FormType;
            public Guid Guid;
            public PayloadType PayloadType;
        }
        
        public enum CapabilityType {Owned, Channel, Unknown};

        [AttributeUsage(AttributeTargets.Class)]
        public class NameAttribute : System.Attribute
        {
            private string name;

            public NameAttribute(string name)
            {
                this.name = name;
            }

            public string Name
            {
                get{return name;}
            }
        }
        
        
        [AttributeUsage(AttributeTargets.Class)]
        public class DescriptionAttribute : System.Attribute
        {
            private string desc;

            public DescriptionAttribute(string desc)
            {
                this.desc = desc;
            }

            public string Description
            {
                get{return desc;}
            }
        }
        
        
        [AttributeUsage(AttributeTargets.Class)]
        public class VersionAttribute : System.Attribute
        {
            private string version;

            public VersionAttribute(string version)
            {
                this.version = version;
            }

            public string Version
            {
                get{return version;}
            }
        }
        
        
        [AttributeUsage(AttributeTargets.Class)]
        public class DeveloperAttribute : System.Attribute
        {
            private string dev;

            public DeveloperAttribute(string dev)
            {
                this.dev = dev;
            }

            public string Developer
            {
                get{return dev;}
            }
        }
        
        
        [AttributeUsage(AttributeTargets.Class)]
        public class CodebaseAttribute : System.Attribute
        {
            private string cb;

            public CodebaseAttribute(string cb)
            {
                this.cb = cb;
            }

            public string Codebase
            {
                get{return cb;}
            }
        }
        
         
        [AttributeUsage(AttributeTargets.Class)]
        public class HomepageURLAttribute : System.Attribute
        {
            private string url;

            public HomepageURLAttribute(string url)
            {
                this.url = url;
            }

            public string HomepageURL
            {
                get{return url;}
            }
        }
     
            
        [AttributeUsage(AttributeTargets.Class)]
        public class GuidAttribute : System.Attribute
        {
            private Guid guid;

            public GuidAttribute(Guid guid)
            {
                this.guid = guid;
            }

            public Guid Guid
            {
                get{return guid;}
            }
        }
        

        [AttributeUsage(AttributeTargets.Class)]
        public class PayloadTypeAttribute : System.Attribute
        {
            private PayloadType pt;

            public PayloadTypeAttribute(PayloadType pt)
            {
                this.pt = pt;
            }

            public PayloadType PayloadType
            {
                get{return pt;}
            }
        }


        [AttributeUsage(AttributeTargets.Class)]
        public class FormTypeAttribute : System.Attribute
        {
            private Type formType;
            public FormTypeAttribute(Type formType)
            {
                this.formType = formType;
            }

            public Type FormType
            {
                get{return formType;}
            }
        }

        
        [AttributeUsage(AttributeTargets.Class)]
        public class MaxBandwidthAttribute : System.Attribute
        {
            private int mb;

            public MaxBandwidthAttribute(int mb)
            {
                this.mb = mb;
            }

            public int MaxBandwidth
            {
                get{return mb;}
            }
        }
        
            
        [AttributeUsage(AttributeTargets.Class)]
        public class FecAttribute : System.Attribute
        {
            private bool fec;

            public FecAttribute(bool fec)
            {
                this.fec = fec;
            }

            public bool Fec
            {
                get{return fec;}
            }
        }
        

        [AttributeUsage(AttributeTargets.Class)]
        public class FecRatioAttribute : System.Attribute
        {
            private ushort data;
            private ushort checksum;

            public FecRatioAttribute(ushort data, ushort checksum)
            {
                this.data = data;
                this.checksum = checksum;
            }

            public ushort Data
            {
                get{return data;}
            }

            public ushort Checksum
            {
                get{return checksum;}
            }
        }
        
            
        [AttributeUsage(AttributeTargets.Class)]
        public class ChannelAttribute : System.Attribute
        {
            private bool channel;

            public ChannelAttribute(bool channel)
            {
                this.channel = channel;
            }

            public bool Channel
            {
                get{return channel;}
            }
        }
        
        
            
        [AttributeUsage(AttributeTargets.Class)]
        public class BackgroundSenderAttribute : System.Attribute
        {
            private bool bgs;

            public BackgroundSenderAttribute(bool bgs)
            {
                this.bgs = bgs;
            }

            public bool BackgroundSender
            {
                get{return bgs;}
            }
        }

        #endregion
    }
    public class CapabilityWithWindow : Capability, ICapabilityWindow
    {
        #region Constructors
        protected CapabilityWithWindow(DynamicProperties dynaProps) : base(dynaProps) {}
        protected CapabilityWithWindow() : base() {}
        #endregion
        #region Implementation of ICapabilityWindow

        public int Height
        {
            get
            {
                ValidateForm();
                return form.Height;
            }
            set
            {
                ValidateForm();
                form.Height = value;
            }
        }

        public int Left
        {
            get
            {
                ValidateForm();
                return form.Left;
            }
            set
            {
                ValidateForm();
                form.Left = value;
            }
        }

        public string Caption
        {
            get
            {
                ValidateForm();
                return form.Text;
            }
            set
            {
                ValidateForm();
                form.Text = value;
            }
        }

        public bool Visible
        {
            get
            {
                ValidateForm();
                return form.Visible;
            }
            set
            {
                ValidateForm();
                form.Visible = value;
            }
        }

        public int Width
        {
            get
            {
                ValidateForm();
                return form.Width;
            }
            set
            {
                ValidateForm();
                form.Width = value;
            }
        }

        public int Top
        {
            get
            {
                ValidateForm();
                return form.Top;
            }
            set
            {
                ValidateForm();
                form.Top = value;
            }
        }

        public System.Drawing.Rectangle Rectangle
        {
            get
            {
                ValidateForm();
                return new System.Drawing.Rectangle(Left, Top, Width, Height);
            }
            set
            {
                ValidateForm();
                Left = value.Left;
                Top = value.Top;
                Width = value.Width;
                Height = value.Height;
                form.Refresh();
            }
        }

        public virtual System.Drawing.Point Location
        {
            set
            {
                ValidateForm();
                form.Location = value;
            }
        }

        // Note: Size is overriden in VideoCapability in order to handle
        //       sizing proportional to the video source 
        public virtual System.Drawing.Size Size
        {
            set
            {
                ValidateForm();
                form.Size = value;
            }
        }
        #endregion
    }

}



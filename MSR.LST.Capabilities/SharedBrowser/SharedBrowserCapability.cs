using System;

using MSR.LST.ConferenceXP;

using SHDocVw;


namespace MSR.LST.ConferenceXP
{
    [Capability.Name("Shared Browser")]
    [Capability.PayloadType(PayloadType.SharedBrowser)]
    [Capability.Channel(false)]
    public class SharedBrowser : Capability, ICapabilitySender, ICapabilityViewer
    {
        private InternetExplorerClass ie = null;

        /// <summary>
        /// ICapabilitySender constructor
        /// Adds local participant's name
        /// </summary>
        public SharedBrowser() : base() 
        {
            name = Conference.LocalParticipant.Name + " - " + name;
        }

        /// <summary>
        /// ICapabilityViewer constructor
        /// </summary>
        public SharedBrowser(DynamicProperties dynaProps) : base(dynaProps) {}
        
        public override void Send()
        {
            base.Send();
            Initialize();
        }

        public override void Play()
        {
            // Doesn't make any sense to play your own browser
            if(!IsSender)
            {
                base.Play ();
                Initialize();
            }
        }

        public override void StopPlaying()
        {
            // Not allowed to Play your own browser, so can't StopPlaying either
            if(!IsSender)
            {
                if (ie != null)
                {
                    ie.Visible = false;
                }
    
                base.StopPlaying ();
            }
        }


        private void Initialize()
        {
            if(ie == null)
            {
                ie = new InternetExplorerClass(); 
                ie.OnQuit += new DWebBrowserEvents2_OnQuitEventHandler(OnQuit);

                if(IsSender)
                {
                    // On the sending side, hook as many events as desired (see HookAllEvents for others)
                    // Don't hook events in constructor, because we don't have an RtpSender yet
                    ie.BeforeNavigate += new DWebBrowserEvents_BeforeNavigateEventHandler(OnBeforeNavigate);
                }
                else
                {
                    // Hook ObjectReceived event on capability
                    ObjectReceived += new CapabilityObjectReceivedEventHandler(OnObjectReceived);
                }
            }

            ie.Visible = true;
        }
        
        private void OnQuit()
        {
            // IE was manually shut down
            // No need to unhook events, IE is already gone...
            ie = null;

            if(IsSender)
            {
                base.StopSending();
            }
            else
            {
                base.StopPlaying();
            }
        }

        public override void Dispose()
        {
            if(!IsSender)
            {
                ObjectReceived -= new CapabilityObjectReceivedEventHandler(OnObjectReceived);
            }

            if(ie != null)
            {
                // Unhook before we cause it to fire
                ie.OnQuit -= new DWebBrowserEvents2_OnQuitEventHandler(OnQuit);
                ie.Quit();
                ie = null;
            }

            base.Dispose();
        }
        

        private void OnBeforeNavigate(string s, int i, string s2, ref object o, string s3, ref bool b)
        {
            NavigationData nd = new NavigationData(s, i, s2, o, s3);
            SendObject(nd);
        }

        [Serializable]
        private class NavigationData {
            public string Url;
            public int Flags;
            public string TargetFrame;
            public object PostData;
            public string Headers;
            public NavigationData(string url, int flags, string targetframe, object postdata, string headers) {
                this.Url = url;
                this.Flags = flags;
                this.TargetFrame = targetframe;
                this.PostData = postdata;
                this.Headers = headers;
            }
        }
        
        private void OnObjectReceived(object sender, ObjectReceivedEventArgs ea)
        {
            object flags = null, targetFrameName = null, postData = null, headers = null;
            string url = null;
            if (ea.Data is NavigationData) { 
                NavigationData nd = (NavigationData)ea.Data;
                url = nd.Url;
                flags = (object)nd.Flags;
                targetFrameName = (object)nd.TargetFrame;
                postData = nd.PostData;
                headers = (object)nd.Headers;
            } else if (ea.Data is string)
            {
                //Older versions just sent a string -- maintain compatibility
                url = (string)ea.Data;
            }
            ie.Navigate(url, ref flags, ref targetFrameName, ref postData, ref headers);
        }

       
        /// <summary>
        /// On the sending side, hook as many of these events as desired
        /// </summary>
        private void HookAllEvents()
        {
            // WebBrowserEvents
            ie.BeforeNavigate += new DWebBrowserEvents_BeforeNavigateEventHandler(BeforeNavigate);
            ie.CommandStateChange += new DWebBrowserEvents2_CommandStateChangeEventHandler(CommandStateChange);
            ie.DownloadBegin += new DWebBrowserEvents2_DownloadBeginEventHandler(DownloadBegin);
            ie.DownloadComplete += new DWebBrowserEvents2_DownloadCompleteEventHandler(DownloadComplete);
            ie.FrameBeforeNavigate += new DWebBrowserEvents_FrameBeforeNavigateEventHandler(FrameBeforeNavigate);
            ie.FrameNavigateComplete += new DWebBrowserEvents_FrameNavigateCompleteEventHandler(FrameNavigateComplete);
            ie.FrameNewWindow += new DWebBrowserEvents_FrameNewWindowEventHandler(FrameNewWindow);
            ie.NavigateComplete += new DWebBrowserEvents_NavigateCompleteEventHandler(NavigateComplete);
            ie.NewWindow += new DWebBrowserEvents_NewWindowEventHandler(NewWindow);
            ie.ProgressChange += new DWebBrowserEvents2_ProgressChangeEventHandler(ProgressChange);
            ie.PropertyChange += new DWebBrowserEvents2_PropertyChangeEventHandler(PropertyChange);
            // ie.Quit, listed as an event, but doesn't appear in auto-complete list??
            ie.TitleChange += new DWebBrowserEvents2_TitleChangeEventHandler(TitleChange);
            ie.WindowActivate += new DWebBrowserEvents_WindowActivateEventHandler(WindowActivate);
            ie.WindowMove += new DWebBrowserEvents_WindowMoveEventHandler(WindowMove);;
            ie.WindowResize += new DWebBrowserEvents_WindowResizeEventHandler(WindowResize);
            
            // WebBrowserEvents2
            ie.BeforeNavigate2 += new DWebBrowserEvents2_BeforeNavigate2EventHandler(BeforeNavigate2);
            ie.ClientToHostWindow += new DWebBrowserEvents2_ClientToHostWindowEventHandler(ClientToHostWindow);
            ie.DocumentComplete += new DWebBrowserEvents2_DocumentCompleteEventHandler(DocumentComplete);
            ie.FileDownload += new DWebBrowserEvents2_FileDownloadEventHandler(FileDownload);
            ie.NavigateComplete2 += new DWebBrowserEvents2_NavigateComplete2EventHandler(NavigateComplete2);
            ie.NavigateError += new DWebBrowserEvents2_NavigateErrorEventHandler(NavigateError);
            ie.NewWindow2 += new DWebBrowserEvents2_NewWindow2EventHandler(NewWindow2);
            ie.NewWindow3 += new DWebBrowserEvents2_NewWindow3EventHandler(NewWindow3);
            ie.OnFullScreen += new DWebBrowserEvents2_OnFullScreenEventHandler(OnFullScreen);
            ie.OnMenuBar += new DWebBrowserEvents2_OnMenuBarEventHandler(OnMenuBar);
            ie.OnQuit += new DWebBrowserEvents2_OnQuitEventHandler(OnQuit);
            ie.OnStatusBar += new DWebBrowserEvents2_OnStatusBarEventHandler(OnStatusBar);
            ie.OnTheaterMode += new DWebBrowserEvents2_OnTheaterModeEventHandler(OnTheaterMode);
            ie.OnToolBar += new DWebBrowserEvents2_OnToolBarEventHandler(OnToolBar);
            ie.OnVisible += new DWebBrowserEvents2_OnVisibleEventHandler(OnVisible);
            ie.PrintTemplateInstantiation += new DWebBrowserEvents2_PrintTemplateInstantiationEventHandler(PrintTemplateInstantiation);
            ie.PrintTemplateTeardown += new DWebBrowserEvents2_PrintTemplateTeardownEventHandler(PrintTemplateTeardown);
            ie.PrivacyImpactedStateChange += new DWebBrowserEvents2_PrivacyImpactedStateChangeEventHandler(PrivacyImpactedStateChange);
            ie.SetSecureLockIcon += new DWebBrowserEvents2_SetSecureLockIconEventHandler(SetSecureLockIcon);
            ie.UpdatePageStatus += new DWebBrowserEvents2_UpdatePageStatusEventHandler(UpdatePageStatus);
            ie.WindowClosing += new DWebBrowserEvents2_WindowClosingEventHandler(WindowClosing);
            ie.WindowSetHeight += new DWebBrowserEvents2_WindowSetHeightEventHandler(WindowSetHeight);
            ie.WindowSetLeft += new DWebBrowserEvents2_WindowSetLeftEventHandler(WindowSetLeft);
            ie.WindowSetResizable += new DWebBrowserEvents2_WindowSetResizableEventHandler(WindowSetResizable);
            ie.WindowSetTop += new DWebBrowserEvents2_WindowSetTopEventHandler(WindowSetTop);
            ie.WindowSetWidth += new DWebBrowserEvents2_WindowSetWidthEventHandler(WindowSetWidth);
        }

        private void BeforeNavigate(string s, int i, string s2, ref object o, string s3, ref bool b)
        {
            Log(new object[]{"BeforeNavigate:", s, i, s2, o, s3, b});
        }

        private void BeforeNavigate2(object o, ref object o1, ref object o2, ref object o3, ref object o4, ref object o5, ref bool b)
        {
            Log(new object[]{"BeforeNavigate2:", o, o1, o2, o3, o4, o5, b});
        }

        private void ClientToHostWindow(ref int i, ref int i2)
        {
            Log(new object[]{"ClientToHostWindow:", i, i2});
        }

        private void CommandStateChange(int i, bool b)
        {
            // -1, false fires as you move the mouse around
            if(i != -1 || b != false)
            {
                Log(new object[]{"CommandStateChange:", i, b});
            }
        }

        private void DocumentComplete(object o, ref object o2)
        {
            Log(new object[]{"DocumentComplete:", o, o2});
        }

        private void DownloadBegin()
        {
            Log(new object[]{"DownloadBegin"});
        }
        private void DownloadComplete()
        {
            Log(new object[]{"DownloadComplete"});
        }

        private void FileDownload(bool active, ref bool cancel)
        {
            Log(new object[]{"FileDownload:", cancel});
        }
        private void FrameBeforeNavigate(string s, int i, string s2, ref object o, string s3, ref bool b)
        {
            Log(new object[]{"FrameBeforeNavigate:", s, i, s2, o, s3, b});
        }

        private void FrameNavigateComplete(string s)
        {
            Log(new object[]{"FrameNavigateComplete:", s});
        }
        private void FrameNewWindow(string s, int i, string s2, ref object o, string s3, ref bool b)
        {
            Log(new object[]{"FrameNewWindow:", s, i, s2, o, s3, b});
        }

        private void NavigateComplete(string url)
        {
            Log(new object[]{"NavigateComplete:", url});
        }
        private void NavigateComplete2(object o, ref object o2)
        {
            Log(new object[]{"NavigateComplete2:", o, o2});
        }

        private void NavigateError(object o, ref object o2, ref object o3, ref object o4, ref bool b)
        {
            Log(new object[]{"NavigateError", o, o2, o3, o4, b});
        }

        private void NewWindow(string s, int i, string s2, ref object o, string s3, ref bool b)
        {
            Log(new object[]{"NewWindow:", s, i, s2, o, s3, b});
        }

        private void NewWindow2(ref object o, ref bool b)
        {
            Log(new object[]{"NewWindow2:", o, b});
        }

        private void NewWindow3(ref object o, ref bool b, uint ui, string s, string s2)
        {
            Log(new object[]{"NewWindow3:", o, b, ui, s, s2});
        }

        private void OnFullScreen(bool b)
        {
            Log(new object[]{"OnFullScreen:", b});
        }

        private void OnMenuBar(bool b)
        {
            Log(new object[]{"OnMenuBar:", b});
        }

        private void OnStatusBar(bool b)
        {
            Log(new object[]{"OnStatusBar:", b});
        }

        private void OnTheaterMode(bool b)
        {
            Log(new object[]{"OnTheaterMode:", b});
        }

        private void OnToolBar(bool b)
        {
            Log(new object[]{"OnToolBar:", b});
        }

        private void OnVisible(bool b)
        {
            Log(new object[]{"OnVisible:", b});
        }

        private void PrintTemplateInstantiation(object o)
        {
            Log(new object[]{"PrintTemplateInstantiation:", o});
        }

        private void PrintTemplateTeardown(object o)
        {
            Log(new object[]{"PrintTemplateTeardown:", o});
        }
        private void PrivacyImpactedStateChange(bool b)
        {
            Log(new object[]{"PrivacyImpactedStateChange:", b});
        }

        private void ProgressChange(int i, int i2)
        {
            Log(new object[]{"ProgressChange:", i, i2});
        }
        private void PropertyChange(string prop)
        {
            Log(new object[]{"PropertyChange:", prop});
        }

        private void SetSecureLockIcon(int i)
        {
            Log(new object[]{"SetSecureLockIcon:", i});
        }

        private void TitleChange(string title)
        {
            Log(new object[]{"TitleChange:", title});
        }
        private void UpdatePageStatus(object o, ref object o2, ref object o3)
        {
            Log(new object[]{"UpdatePageStatus:", o, o2, o3});
        }

        private void WindowActivate()
        {
            Log(new object[]{"WindowActivate"});
        }

        private void WindowClosing(bool b, ref bool b2)
        {
            Log(new object[]{"WindowClosing", b, b2});
        }
        private void WindowMove()
        {
            Log(new object[]{"WindowMove"});
        }
        private void WindowResize()
        {
            Log(new object[]{"WindowResize"});
        }
        private void WindowSetHeight(int i)
        {
            Log(new object[]{"WindowSetHeight:", i});
        }
        private void WindowSetLeft(int i)
        {
            Log(new object[]{"WindowSetLeft:", i});
        }
        private void WindowSetResizable(bool b)
        {
            Log(new object[]{"WindowSetResizable:", b});
        }
        private void WindowSetTop(int i)
        {
            Log(new object[]{"WindowSetTop:", i});
        }
        private void WindowSetWidth(int i)
        {
            Log(new object[]{"WindowSetWidth:", i});
        }

        
        private void Log(object[] args)
        {
            string format = string.Empty;

            for(int i = 0; i < args.Length; i++)
            {
                format += "{" + i + "} ";
            }

            Console.WriteLine(format, args);
        }
    }
}
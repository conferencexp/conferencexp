using System;
using System.Collections;
using System.Configuration;
using System.Globalization;
using System.Net;
using System.Runtime.InteropServices; // DllImport, to register the filters
using System.Windows.Forms;

// Add a reference to Control.interop.dll: interfaces used - IVideoWindow
// Add a reference to MDShowManager.dll: classes used - VideoDevice, MDShowUtility 
using MSR.LST.MDShow;

// Add a reference to MSR.LST.Net.Rtp.dll
// Classes used - RtpSession, RtpSender, RtpParticipant, RtpStream
using MSR.LST.Net.Rtp;

// Code Flow (CF)
// 1. Same steps as for RtpChat sample:
//    Hook events, establish session, get RtpSender, send data, respond to incoming events and data.
// 2. Same steps as for DirectShow Interop sample:
//    Choose device, configure, render, run.
// 3. Additionally, we need to manage the receiving-side filtergraph
//    Wait for first frame of data to arrive before creating and rendering the graph, 
//    set the video window, run.


namespace DShowNetwork
{
    public class DShowNetworkFiltersForm : System.Windows.Forms.Form
    {
        #region Windows Form Designer generated code

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() 
        {
            Application.Run(new DShowNetworkFiltersForm());
        }


        public DShowNetworkFiltersForm()
        {
            InitializeComponent();
        }
        private System.Windows.Forms.PictureBox pb1;
        private System.Windows.Forms.CheckedListBox cklbCameras;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        [global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303")]
        private void InitializeComponent()
        {
            this.pb1 = new System.Windows.Forms.PictureBox();
            this.cklbCameras = new System.Windows.Forms.CheckedListBox();
            this.SuspendLayout();
            // 
            // pb1
            // 
            this.pb1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.pb1.Location = new System.Drawing.Point(0, 40);
            this.pb1.Name = "pb1";
            this.pb1.Size = new System.Drawing.Size(400, 300);
            this.pb1.TabIndex = 0;
            this.pb1.TabStop = false;
            // 
            // cklbCameras
            // 
            this.cklbCameras.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.cklbCameras.Location = new System.Drawing.Point(0, 0);
            this.cklbCameras.Name = "cklbCameras";
            this.cklbCameras.Size = new System.Drawing.Size(400, 34);
            this.cklbCameras.TabIndex = 1;
            this.cklbCameras.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.cklbCameras_ItemCheck);
            // 
            // DShowNetworkFiltersForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(400, 342);
            this.Controls.Add(this.cklbCameras);
            this.Controls.Add(this.pb1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "DShowNetworkFiltersForm";
            this.Text = "DirectShow Network Demo";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.DShowNetworkFiltersForm_Closing);
            this.Load += new System.EventHandler(this.DShowNetworkFiltersForm_Load);
            this.ResumeLayout(false);

        }
        protected override void Dispose( bool disposing )
        {
            if( disposing )
            {
                if (components != null) 
                {
                    components.Dispose();
                }
            }
            base.Dispose( disposing );
        }

        #endregion

        #region Statics / App.Config overrides

        private static IPEndPoint ep = RtpSession.DefaultEndPoint;
        private static bool presenter = false;

        static DShowNetworkFiltersForm()
        {
            RegisterCxpRtpFilters();

            string setting;

            if((setting = ConfigurationManager.AppSettings["EndPoint"]) != null)
            {
                string[] args = setting.Split(new char[]{':'}, 2);
                ep = new IPEndPoint(IPAddress.Parse(args[0]), int.Parse(args[1], CultureInfo.InvariantCulture));
            }

            if((setting = ConfigurationManager.AppSettings["Presenter"]) != null)
            {
                presenter = bool.Parse(setting);
            }
        }


        [DllImport("CxpRtpFilters.ax", EntryPoint="DllRegisterServer")]
        private static extern void RegisterCxpRtpFilters();

        #endregion Statics / App.Config overrides

        /// <summary>
        /// Contains filtergraph on the sending side
        /// </summary>
        private VideoCaptureGraph vcg;

        /// <summary>
        /// Filtergraph on the receiving side
        /// </summary>
        private FilgraphManagerClass fgm;

        /// <summary>
        /// Manages the connection to a multicast address and all the objects related to Rtp
        /// </summary>
        private RtpSession rtpSession;
        
        /// <summary>
        /// Sends the data across the network
        /// </summary>
        private RtpSender rtpSender;
        
        /// <summary>
        /// Receives the data across the network
        /// </summary>
        private RtpStream rtpStream;

        // CF1, CF2
        private void DShowNetworkFiltersForm_Load(object sender, System.EventArgs e)
        {
            this.Text = Strings.DirectShowNetworkDemo;

            HookRtpEvents();  // CF1
            JoinRtpSession(); // CF1

            if(presenter)
            {
                // CF2 Add devices to UI
                foreach(FilterInfo fi in VideoSource.Sources())
                {
                    cklbCameras.Items.Add(fi);
                }

                rtpSender = rtpSession.CreateRtpSender("Video", PayloadType.dynamicVideo, null); // CF1
            }
            else
            {
                cklbCameras.Visible = false;
            }
        }


        // CF1
        private void HookRtpEvents()
        {
            RtpEvents.RtpStreamAdded += new RtpEvents.RtpStreamAddedEventHandler(RtpStreamAdded);
            RtpStream.FirstFrameReceived += new RtpStream.FirstFrameReceivedEventHandler(FirstFrameReceived);
        }
        private void JoinRtpSession()
        {
            RtpParticipant participant = new RtpParticipant("Video" + pb1.Handle.ToInt32(), "Video");
            rtpSession = new RtpSession(ep, participant, true, true);
        }
        
        // CF2 (with some changes for network)
        private void cklbCameras_ItemCheck(object sender, System.Windows.Forms.ItemCheckEventArgs e)
        {
            if(e.NewValue == CheckState.Checked)
            {
                UncheckAllDevices();

                // Create the graph for the device
                vcg = new VideoCaptureGraph((FilterInfo)cklbCameras.SelectedItem);
                
                // Add a compressor and configure it
                vcg.AddCompressor(VideoCompressor.DefaultFilterInfo());
                vcg.VideoCompressor.QualityInfo = VideoCompressor.DefaultQualityInfo;
                
                // Add network filter
                vcg.RenderNetwork(rtpSender);

                // Send data to network
                vcg.Run(); 
            }
            else // Unchecked
            {
                DisposeDevice();
            }
        }


        // CF1, CF3
        private void RtpStreamAdded(object sender, RtpEvents.RtpStreamEventArgs ea)
        {
            rtpStream = ea.RtpStream;
        }

        // CF3
        private void FirstFrameReceived(object sender, EventArgs ea)
        {
            // Event is static, so filter on sender
            if(sender == rtpStream)
            {
                // WARNING!!
                // Creating and destroying the filtergraph and the filters in it
                // must occur on same thread, so make it the UI thread
                this.Invoke(new MethodInvoker(CreateReceivingGraph), null);
            }
        }

        private void CreateReceivingGraph()
        {
            // Tell the stream we will poll it for data with our own (DShow) thread
            // Instead of receiving data through the FrameReceived event
            rtpStream.IsUsingNextFrame = true;

            // Create receiving filtergraph
            fgm = new FilgraphManagerClass();
            IGraphBuilder iGB = (IGraphBuilder)fgm;
            
            IBaseFilter rtpSource = RtpSourceClass.CreateInstance();
            ((MSR.LST.MDShow.Filters.IRtpSource)rtpSource).Initialize(rtpStream);

            iGB.AddFilter(rtpSource, "RtpSource");
            iGB.Render(Filter.GetPin(rtpSource, _PinDirection.PINDIR_OUTPUT, Guid.Empty, 
                Guid.Empty, false, 0));

            VideoWindow();
            fgm.Run();
        }

        // CF2,CF 3 (same code, but on the receiving side)
        private void VideoWindow()
        {
            // Must render before adjusting the VideoWindow
            IVideoWindow iVW = (IVideoWindow)fgm;
            iVW.Owner = pb1.Handle.ToInt32();
            iVW.SetWindowPosition(-4, -32, 408, 336);
        }

        
        // CF3, CF2, CF1
        private void Cleanup()
        {
            // CF3, Quit processing received data
            if(fgm != null)
            {
                rtpStream.UnblockNextFrame();
                fgm.Stop();
                FilterGraph.RemoveAllFilters(fgm);
                fgm = null;
            }
 
            DisposeDevice();   // CF2
            LeaveRtpSession(); // CF1
       }

        // CF2
        private void DisposeDevice()
        {
            if(vcg != null)
            {
                vcg.Stop();
                vcg.Dispose();
                vcg = null;
            }
        }

        // CF1
        private void LeaveRtpSession()
        {
            UnhookRtpEvents();

            if(rtpSession != null)
            {
                rtpSession.Dispose();
                rtpSession = null;
                rtpSender = null;
                rtpStream = null;
            }
        }

        private void UnhookRtpEvents()
        {
            RtpStream.FirstFrameReceived -= new RtpStream.FirstFrameReceivedEventHandler(FirstFrameReceived);
            RtpEvents.RtpStreamAdded -= new RtpEvents.RtpStreamAddedEventHandler(RtpStreamAdded);
        }


        private void DShowNetworkFiltersForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Cleanup();
        }


        private void UncheckAllDevices()
        {
            foreach(int i in cklbCameras.CheckedIndices)
            {
                cklbCameras.SetItemChecked(i, false);
            }
        }
    }
}

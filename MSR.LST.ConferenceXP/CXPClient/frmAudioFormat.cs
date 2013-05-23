using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

using MSR.LST.MDShow;


namespace MSR.LST.ConferenceXP
{
    public class frmAudioFormat : System.Windows.Forms.Form
    {
        #region Windows Form Designer generated code

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.ListView lvFormats;

        private System.ComponentModel.Container components = null;

        [global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303")]
        private void InitializeComponent()
        {
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.lvFormats = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Location = new System.Drawing.Point(241, 365);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(77, 25);
            this.btnCancel.TabIndex = 71;
            this.btnCancel.Text = "Cancel";
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Location = new System.Drawing.Point(155, 365);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(77, 25);
            this.btnOK.TabIndex = 68;
            this.btnOK.Text = "OK";
            // 
            // lvFormats
            // 
            this.lvFormats.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.lvFormats.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvFormats.AutoArrange = false;
            this.lvFormats.FullRowSelect = true;
            this.lvFormats.GridLines = true;
            this.lvFormats.Location = new System.Drawing.Point(0, 0);
            this.lvFormats.MultiSelect = false;
            this.lvFormats.Name = "lvFormats";
            this.lvFormats.Size = new System.Drawing.Size(325, 358);
            this.lvFormats.TabIndex = 76;
            this.lvFormats.UseCompatibleStateImageBehavior = false;
            this.lvFormats.View = System.Windows.Forms.View.Details;
            // 
            // frmAudioFormat
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(328, 398);
            this.ControlBox = false;
            this.Controls.Add(this.lvFormats);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.Name = "frmAudioFormat";
            this.Text = "FAudioFormat";
            this.Load += new System.EventHandler(this.FAudioFormat_Load);
            this.ResumeLayout(false);

        }


        protected override void Dispose( bool disposing )
        {
            if( disposing )
            {
                if(components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose( disposing );
        }


        #endregion

        #region Members

        /// <summary>
        /// CaptureGraph might be Audio or DV Capture Graph.
        /// </summary>
        private CaptureGraph captureGraph;

        /// <summary>
        /// Media types available on the device
        /// </summary>
        _AMMediaType[] mts;

        /// <summary>
        /// Format blocks for the media types
        /// </summary>
        object[] fbs;

        /// <summary>
        /// Width percentages of the columns (in order to hi
        /// </summary>
        int[] widths = new int[] {20, 30, 30, 20};

        #endregion Members

        #region Constructor

        public frmAudioFormat(CaptureGraph cg)
        {
            InitializeComponent();
            this.captureGraph = cg;
        }

        
        #endregion Constructor

        #region Public

        /// <summary>
        /// Returns the media type and format block chosen by the user
        /// </summary>
        public void GetMediaType(out _AMMediaType mt, out object fb)
        {
            mt = mts[lvFormats.SelectedIndices[0]];
            fb = fbs[lvFormats.SelectedIndices[0]];
        }

        #endregion Public

        #region Private

        private void FAudioFormat_Load(object sender, System.EventArgs e)
        {
            this.btnCancel.Font = UIFont.StringFont;
            this.btnOK.Font = UIFont.StringFont;
            this.lvFormats.Font = UIFont.StringFont;

            this.btnCancel.Text = Strings.Cancel;
            this.btnOK.Text = Strings.OK;

            this.Text = string.Format(CultureInfo.CurrentCulture, Strings.ChooseAudioFormat, captureGraph.Source.FriendlyName);

            lvFormats.View = View.Details;
            lvFormats.Columns.Add(Strings.Channels, 60, HorizontalAlignment.Right);
            lvFormats.Columns.Add(Strings.Frequency, 90, HorizontalAlignment.Right);
            lvFormats.Columns.Add(Strings.BitsPerSample, 90, HorizontalAlignment.Right);
            lvFormats.Columns.Add(Strings.Kbps, 60, HorizontalAlignment.Right);

            GetMediaTypes();
            SelectCurrent();
        }
        
        /// <summary>
        /// Populates the listview with the available media types
        /// </summary>
        private void GetMediaTypes()
        {
            if (captureGraph is AudioCaptureGraph) {
                AudioSource source = ((AudioCaptureGraph)captureGraph).AudioSource;
                source.GetMediaTypes(out mts, out fbs);
            }
            else if (captureGraph is DVCaptureGraph) {
                ((DVCaptureGraph)captureGraph).GetAudioMediaTypes(out mts, out fbs);
            }
            else {
                return;
            }

            for(int i = 0; i < mts.Length; i++)
            {
                WAVEFORMATEX wfe = (WAVEFORMATEX)fbs[i];

                lvFormats.Items.Add(wfe.Channels.ToString(CultureInfo.CurrentCulture));
                lvFormats.Items[i].SubItems.Add(wfe.SamplesPerSec.ToString(CultureInfo.CurrentCulture));
                lvFormats.Items[i].SubItems.Add(wfe.BitsPerSample.ToString(CultureInfo.CurrentCulture));
                lvFormats.Items[i].SubItems.Add(((int)(wfe.AvgBytesPerSec * 8 / 1000)).ToString(CultureInfo.CurrentCulture));
            }
        }

        /// <summary>
        /// Iterate through the available media types and mark the first one
        /// that matches our current media type
        /// </summary>
        private void SelectCurrent()
        {
            _AMMediaType mt;
            object fb;
            if (captureGraph is AudioCaptureGraph) {
                AudioSource source = ((AudioCaptureGraph)captureGraph).AudioSource;
                source.GetMediaType(out mt, out fb);
            }
            else if (captureGraph is DVCaptureGraph) {
                ((DVCaptureGraph)captureGraph).GetAudioMediaType(out mt, out fb);
            }
            else {
                return;
            }
            WAVEFORMATEX cur = (WAVEFORMATEX)fb;

            for(int i = 0; i < fbs.Length; i++)
            {
                WAVEFORMATEX wfe = (WAVEFORMATEX)fbs[i];

                if(cur.Channels == wfe.Channels &&
                   cur.SamplesPerSec == wfe.SamplesPerSec &&
                   cur.BitsPerSample == wfe.BitsPerSample)
                {
                    this.Visible = true;
                    lvFormats.Focus();
                    lvFormats.Items[i].Selected = true;
                    return;
                }
            }

            Debug.Fail("We didn't find a matching media type");
        }

        #endregion Private
    }
}

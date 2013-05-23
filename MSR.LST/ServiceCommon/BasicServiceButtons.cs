using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;


namespace MSR.LST.Services
{
    /// <summary>
    /// Contains controls shared by ConferenceXP services
    /// </summary>
    public class BasicServiceButtons : System.Windows.Forms.UserControl
    {
        #region Windows Form Designer generated private members

        private System.Windows.Forms.Button helpBtn;
        private System.Windows.Forms.Button aboutBtn;
        private System.Windows.Forms.Button closeBtn;
        private System.ComponentModel.Container components = null;

        #endregion
        
        #region Members

        private string helpUrl;
        public event EventHandler AboutClicked = null;

        #endregion 

        #region Constructor, Dispose

        public BasicServiceButtons()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
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

        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.helpBtn = new System.Windows.Forms.Button();
            this.aboutBtn = new System.Windows.Forms.Button();
            this.closeBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // helpBtn
            // 
            this.helpBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.helpBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.helpBtn.Location = new System.Drawing.Point(0, 0);
            this.helpBtn.Name = "helpBtn";
            this.helpBtn.Size = new System.Drawing.Size(80, 24);
            this.helpBtn.TabIndex = 0;
            this.helpBtn.Text = Strings.Help;
            this.helpBtn.Click += new System.EventHandler(this.helpBtn_Click);
            // 
            // aboutBtn
            // 
            this.aboutBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.aboutBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.aboutBtn.Location = new System.Drawing.Point(96, 0);
            this.aboutBtn.Name = "aboutBtn";
            this.aboutBtn.Size = new System.Drawing.Size(80, 24);
            this.aboutBtn.TabIndex = 1;
            this.aboutBtn.Text = Strings.About;
            this.aboutBtn.Click += new System.EventHandler(this.aboutBtn_Click);
            // 
            // closeBtn
            // 
            this.closeBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.closeBtn.Location = new System.Drawing.Point(192, 0);
            this.closeBtn.Name = "closeBtn";
            this.closeBtn.Size = new System.Drawing.Size(80, 24);
            this.closeBtn.TabIndex = 2;
            this.closeBtn.Text = Strings.Close;
            this.closeBtn.Click += new System.EventHandler(this.closeBtn_Click);
            // 
            // BasicServiceButtons
            // 
            this.Controls.Add(this.helpBtn);
            this.Controls.Add(this.aboutBtn);
            this.Controls.Add(this.closeBtn);
            this.Name = "BasicServiceButtons";
            this.Size = new System.Drawing.Size(272, 24);
            this.ResumeLayout(false);

        }
        #endregion

        #region Properties

        public string HelpUrl
        {
            get
            {
                return helpUrl;
            }
            set
            {
                helpUrl = value;
            }
        }

        #endregion

        #region Event Handlers
        
        private void helpBtn_Click(object sender, System.EventArgs e)
        {
            Process.Start(helpUrl);
        }

        private void closeBtn_Click(object sender, System.EventArgs e)
        {
            this.ParentForm.Close();
        }

        private void aboutBtn_Click(object sender, System.EventArgs e)
        {
            if (AboutClicked != null)
                AboutClicked(sender, e);
        }
        
        #endregion
    }
}

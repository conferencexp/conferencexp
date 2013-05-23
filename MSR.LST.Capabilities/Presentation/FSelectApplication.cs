using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;


namespace MSR.LST.ConferenceXP
{
    /// <summary>
    /// Summary description for FSelectApplication.
    /// </summary>
    public class FSelectApplication : System.Windows.Forms.Form
    {
        private System.Windows.Forms.ComboBox comboBoxApplication;
        private System.Windows.Forms.Button buttonOK;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public FSelectApplication()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
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

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        [global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303")]
        private void InitializeComponent()
        {
            this.comboBoxApplication = new System.Windows.Forms.ComboBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // comboBoxApplication
            // 
            this.comboBoxApplication.Location = new System.Drawing.Point(8, 8);
            this.comboBoxApplication.Name = "comboBoxApplication";
            this.comboBoxApplication.Size = new System.Drawing.Size(280, 21);
            this.comboBoxApplication.TabIndex = 0;
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(296, 8);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.TabIndex = 1;
            this.buttonOK.Text = "OK";
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // FSelectApplication
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(376, 37);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.comboBoxApplication);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.Name = "FSelectApplication";
            this.Text = "Select Application";
            this.Text = Strings.SelectApplication;
            this.Load += new System.EventHandler(this.FSelectApplication_Load);
            this.ResumeLayout(false);

        }
        #endregion

        private void FSelectApplication_Load(object sender, System.EventArgs e)
        {
            this.buttonOK.Text = Strings.OK;
            this.Text = Strings.SelectApplication;

            foreach (Win32Util.Win32Window window in Win32Util.Win32Window.ApplicationWindows)
            {
                comboBoxApplication.Items.Add(window.Text);
            }
        }

        internal Win32Util.Win32Window applicationWindow = null;
        private void buttonOK_Click(object sender, System.EventArgs e)
        {
            foreach (Win32Util.Win32Window window in Win32Util.Win32Window.ApplicationWindows)
            {
                if (window.Text == (string)comboBoxApplication.SelectedItem)
                {
                    applicationWindow = window;
                    this.Close();
                }
            }
        }
    }
}

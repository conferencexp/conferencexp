using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;


namespace MSR.LST.ConferenceXP
{
    /// <summary>
    /// A versatile options dialog box that can be used for most simple pop-up questions.
    /// </summary>
    /// <remarks>
    /// Unnecessary parts can just be set to invisible.
    /// </remarks>
    public class OptionsDialog : System.Windows.Forms.Form
    {
        internal System.Windows.Forms.Label questionLabel;
        internal System.Windows.Forms.Label infoLabel;
        internal System.Windows.Forms.ComboBox options;
        internal System.Windows.Forms.Button okayButton;
        internal System.Windows.Forms.CheckBox checkBox;
        private System.ComponentModel.Container components = null;


        public OptionsDialog()
        {
            // Required for Windows Form Designer support
            InitializeComponent();
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        [global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303")]
        private void InitializeComponent()
        {
            this.questionLabel = new System.Windows.Forms.Label();
            this.infoLabel = new System.Windows.Forms.Label();
            this.options = new System.Windows.Forms.ComboBox();
            this.okayButton = new System.Windows.Forms.Button();
            this.checkBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // questionLabel
            // 
            this.questionLabel.Location = new System.Drawing.Point(16, 16);
            this.questionLabel.Name = "questionLabel";
            this.questionLabel.Size = new System.Drawing.Size(288, 40);
            this.questionLabel.TabIndex = 0;
            this.questionLabel.Text = "<Label>";
            // 
            // infoLabel
            // 
            this.infoLabel.Location = new System.Drawing.Point(16, 64);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size(288, 16);
            this.infoLabel.TabIndex = 0;
            this.infoLabel.Text = "<Label>";
            this.infoLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // options
            // 
            this.options.Location = new System.Drawing.Point(16, 88);
            this.options.Name = "options";
            this.options.Size = new System.Drawing.Size(288, 21);
            this.options.Sorted = true;
            this.options.TabIndex = 1;
            this.options.Text = "<Combo Box>";
            this.options.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.options_KeyPress);
            // 
            // okayButton
            // 
            this.okayButton.Location = new System.Drawing.Point(232, 160);
            this.okayButton.Name = "okayButton";
            this.okayButton.Size = new System.Drawing.Size(64, 24);
            this.okayButton.TabIndex = 3;
            this.okayButton.Text = "Okay";
            this.okayButton.Click += new System.EventHandler(this.okayClicked);
            // 
            // checkBox
            // 
            this.checkBox.Location = new System.Drawing.Point(16, 128);
            this.checkBox.Name = "checkBox";
            this.checkBox.Size = new System.Drawing.Size(280, 24);
            this.checkBox.TabIndex = 2;
            this.checkBox.Text = "<Checkbox>";
            // 
            // OptionsDialog
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(314, 200);
            this.Controls.Add(this.checkBox);
            this.Controls.Add(this.okayButton);
            this.Controls.Add(this.options);
            this.Controls.Add(this.infoLabel);
            this.Controls.Add(this.questionLabel);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "OptionsDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "OneNote Importer";
            this.TopMost = true;
            this.Closing += new System.ComponentModel.CancelEventHandler(this.OptionsDialog_Closing);
            this.ResumeLayout(false);

        }
        #endregion

        private void okayClicked(object sender, System.EventArgs e)
        {
            // Verify the user has selected something permissible
            string trimmedText = options.Text.Trim();
            if( trimmedText == null || trimmedText.Length == 0 )
            {
                RtlAwareMessageBox.Show(this, Strings.PleaseEnterAValidResponse, string.Empty, MessageBoxButtons.OK, 
                    MessageBoxIcon.None, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void OptionsDialog_Closing(object sender, CancelEventArgs e)
        {
            // Verify the user has selected something permissible
            if( options.SelectedText.Trim() == null )
            {
                RtlAwareMessageBox.Show(this, Strings.PleaseEnterAValidResponse, string.Empty, MessageBoxButtons.OK, 
                    MessageBoxIcon.None, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
                return;
            }

            this.DialogResult = DialogResult.OK;
        }

        private void options_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if( e.KeyChar == '\r' || e.KeyChar == '\n' )
                this.okayClicked( this, EventArgs.Empty );
        }

        private void OptionsDialog_Load(object sender, EventArgs e)
        {
            this.questionLabel.Text = Strings.Label;
            this.options.Text = Strings.ComboBox;
            this.okayButton.Text = Strings.OK;
            this.checkBox.Text = Strings.Checkbox;
            this.infoLabel.Text = Strings.Label;
            this.Text = Strings.OneNoteImporter;
        }
    }


}

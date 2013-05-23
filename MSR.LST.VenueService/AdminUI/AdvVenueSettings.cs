using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;


namespace MSR.LST.ConferenceXP.VenueService
{
    /// <summary>
    /// Summary description for AdvVenueSettings.
    /// </summary>
    public class AdvVenueSettings : System.Windows.Forms.Form
    {
        private System.Windows.Forms.TextBox expressionInput;
        private System.Windows.Forms.ListBox expressionList;
        private System.Windows.Forms.Button addBtn;
        private System.Windows.Forms.Button replaceBtn;
        private System.Windows.Forms.Button deleteBtn;
        private System.Windows.Forms.Button cancelBtn;
        private System.Windows.Forms.Button okBtn;
        private System.ComponentModel.Container components = null;
        private System.Windows.Forms.Label useLbl;
        private System.Windows.Forms.Label inputLbl;
        private System.Windows.Forms.Label listLbl;
        private System.Windows.Forms.Label exampleLbl;

        private readonly SecurityPatterns originalList;

        public AdvVenueSettings(SecurityPatterns accessList)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            // Keep a handle on the orignal list
            this.originalList = accessList;

            // Show all of the items in the list
            if (accessList != null)
            {
                string[] expressions = accessList.Patterns;
                foreach(string expression in expressions)
                {
                    this.expressionList.Items.Add(expression);
                }
            }
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
            this.useLbl = new System.Windows.Forms.Label();
            this.inputLbl = new System.Windows.Forms.Label();
            this.listLbl = new System.Windows.Forms.Label();
            this.expressionInput = new System.Windows.Forms.TextBox();
            this.expressionList = new System.Windows.Forms.ListBox();
            this.addBtn = new System.Windows.Forms.Button();
            this.replaceBtn = new System.Windows.Forms.Button();
            this.deleteBtn = new System.Windows.Forms.Button();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.okBtn = new System.Windows.Forms.Button();
            this.exampleLbl = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // useLbl
            // 
            this.useLbl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.useLbl.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.useLbl.Location = new System.Drawing.Point(16, 16);
            this.useLbl.Name = "useLbl";
            this.useLbl.Size = new System.Drawing.Size(312, 28);
            this.useLbl.TabIndex = 0;
            this.useLbl.Text = "Specify who can see this venue by participant identifier. Use regular expressions" +
                " to specify individuals or groups.";
            // 
            // inputLbl
            // 
            this.inputLbl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            this.inputLbl.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.inputLbl.Location = new System.Drawing.Point(16, 72);
            this.inputLbl.Name = "inputLbl";
            this.inputLbl.Size = new System.Drawing.Size(192, 16);
            this.inputLbl.TabIndex = 1;
            this.inputLbl.Text = "Identifier expression:";
            this.inputLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // listLbl
            // 
            this.listLbl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            this.listLbl.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.listLbl.Location = new System.Drawing.Point(16, 120);
            this.listLbl.Name = "listLbl";
            this.listLbl.Size = new System.Drawing.Size(312, 20);
            this.listLbl.TabIndex = 1;
            this.listLbl.Text = "Show venue only to participants matching these expressions:";
            this.listLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // expressionInput
            // 
            this.expressionInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            this.expressionInput.Location = new System.Drawing.Point(16, 88);
            this.expressionInput.Name = "expressionInput";
            this.expressionInput.Size = new System.Drawing.Size(312, 20);
            this.expressionInput.TabIndex = 2;
            this.expressionInput.TextChanged += new System.EventHandler(this.expressionInput_TextChanged);
            // 
            // expressionList
            // 
            this.expressionList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            this.expressionList.Location = new System.Drawing.Point(16, 140);
            this.expressionList.Name = "expressionList";
            this.expressionList.Size = new System.Drawing.Size(312, 108);
            this.expressionList.TabIndex = 3;
            this.expressionList.SelectedIndexChanged += new System.EventHandler(this.expressionList_SelectedIndexChanged);
            // 
            // addBtn
            // 
            this.addBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.addBtn.Enabled = false;
            this.addBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.addBtn.Location = new System.Drawing.Point(80, 260);
            this.addBtn.Name = "addBtn";
            this.addBtn.TabIndex = 4;
            this.addBtn.Text = "Add";
            this.addBtn.Click += new System.EventHandler(this.addBtn_Click);
            // 
            // replaceBtn
            // 
            this.replaceBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.replaceBtn.Enabled = false;
            this.replaceBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.replaceBtn.Location = new System.Drawing.Point(168, 260);
            this.replaceBtn.Name = "replaceBtn";
            this.replaceBtn.TabIndex = 4;
            this.replaceBtn.Text = "Replace";
            this.replaceBtn.Click += new System.EventHandler(this.replaceBtn_Click);
            // 
            // deleteBtn
            // 
            this.deleteBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.deleteBtn.Enabled = false;
            this.deleteBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.deleteBtn.Location = new System.Drawing.Point(256, 260);
            this.deleteBtn.Name = "deleteBtn";
            this.deleteBtn.TabIndex = 4;
            this.deleteBtn.Text = "Delete";
            this.deleteBtn.Click += new System.EventHandler(this.deleteBtn_Click);
            // 
            // cancelBtn
            // 
            this.cancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cancelBtn.Location = new System.Drawing.Point(256, 316);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.TabIndex = 4;
            this.cancelBtn.Text = "Cancel";
            // 
            // okBtn
            // 
            this.okBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.okBtn.Location = new System.Drawing.Point(168, 316);
            this.okBtn.Name = "okBtn";
            this.okBtn.TabIndex = 4;
            this.okBtn.Text = "OK";
            // 
            // exampleLbl
            // 
            this.exampleLbl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            this.exampleLbl.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.exampleLbl.Location = new System.Drawing.Point(16, 44);
            this.exampleLbl.Name = "exampleLbl";
            this.exampleLbl.Size = new System.Drawing.Size(312, 16);
            this.exampleLbl.TabIndex = 5;
            this.exampleLbl.Text = "For example: .*@[O|o]rganization\\.[E|e]du";
            // 
            // AdvVenueSettings
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.cancelBtn;
            this.ClientSize = new System.Drawing.Size(346, 352);
            this.ControlBox = false;
            this.Controls.Add(this.exampleLbl);
            this.Controls.Add(this.addBtn);
            this.Controls.Add(this.expressionList);
            this.Controls.Add(this.expressionInput);
            this.Controls.Add(this.inputLbl);
            this.Controls.Add(this.useLbl);
            this.Controls.Add(this.listLbl);
            this.Controls.Add(this.replaceBtn);
            this.Controls.Add(this.deleteBtn);
            this.Controls.Add(this.cancelBtn);
            this.Controls.Add(this.okBtn);
            this.Font = UIFont.FormFont;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AdvVenueSettings";
            this.ShowInTaskbar = false;
            this.Text = "Advanced Venue Settings";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.AdvVenueSettings_Paint);
            this.Load += new System.EventHandler(this.AdvVenueSettings_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        public SecurityPatterns Patterns
        {
            get
            {
                if (expressionList.Items.Count > 0)
                {
                    string[] list = new string[expressionList.Items.Count];
                    expressionList.Items.CopyTo(list, 0);
                    return new SecurityPatterns(list);
                }
                else
                {
                    return null;
                }
            }
        }

        private void addBtn_Click(object sender, System.EventArgs e)
        {
            expressionList.Items.Add(expressionInput.Text);
            expressionInput.Text = string.Empty;
        }

        private void replaceBtn_Click(object sender, System.EventArgs e)
        {
            DialogResult dr = RtlAwareMessageBox.Show(this, string.Format(CultureInfo.CurrentCulture, 
                Strings.ConfirmReplaceText, expressionList.SelectedItem.ToString(), expressionInput.Text), 
                Strings.ConfirmReplaceTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, 
                MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);

            if (dr == DialogResult.Yes)
            {
                int selected = expressionList.SelectedIndex;
                expressionList.Items[selected] = expressionInput.Text;
                expressionInput.Text = string.Empty;
            }
        }

        private void deleteBtn_Click(object sender, System.EventArgs e)
        {
            DialogResult dr = RtlAwareMessageBox.Show(this, string.Format(CultureInfo.CurrentCulture, 
                Strings.ConfirmDeleteText, expressionList.SelectedItem.ToString()), Strings.ConfirmDeleteTitle, 
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, 
                (MessageBoxOptions)0);

            if (dr == DialogResult.Yes)
            {
                int selected = expressionList.SelectedIndex;
                expressionList.Items.RemoveAt(selected);
            }
        }

        private void expressionList_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (expressionList.SelectedItem == null)
            {
                // If there's no item selected, we can't replace or delete the selected item, no?
                this.replaceBtn.Enabled = false;
                this.deleteBtn.Enabled = false;
            }
            else
            {
                // There's an item selected, so we can delete it
                this.deleteBtn.Enabled = true;

                // Check to see if there's a valid input to replace the selected item with
                CheckForDupInput();

                expressionInput.Text = (string)expressionList.SelectedItem;
            }
        }

        private void expressionInput_TextChanged(object sender, System.EventArgs e)
        {
            // If the input is empty, the buttons can't be enabled
            if (expressionInput.Text == null || expressionInput.Text == String.Empty)
            {
                addBtn.Enabled = false;
                replaceBtn.Enabled = false;
            }
            else
            {
                // Since there's an input, we might be able to use it
                CheckForDupInput();
            }
        }

        /// <summary>
        /// Sets the Enabled property of the Add and Replace buttons based on whether the
        /// input in expressionInput is a duplicate of one that's already in the list.
        /// </summary>
        private void CheckForDupInput()
        {
            bool isDup = false;
            foreach(String item in expressionList.Items)
            {
                if (item.Equals(expressionInput.Text))
                {
                    isDup = true;
                    break;
                }
            }

            if (isDup)
            {
                this.addBtn.Enabled = false;
                this.replaceBtn.Enabled = false;
            }
            else
            {
                this.addBtn.Enabled = true;
                this.replaceBtn.Enabled = true;
            }
        }

        private void AdvVenueSettings_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            // Draw in the three lines in the UI
            using(Graphics g = e.Graphics)
            {
                // Draw line across bottom of UI
                int topButtonsBottom = addBtn.Top + addBtn.Height;
                int lineY = topButtonsBottom + (okBtn.Top - topButtonsBottom)*2/3;
                int lineRight = cancelBtn.Left + cancelBtn.Width;
                int lineLeft = expressionList.Left;
                DrawLine(g, lineY, lineLeft, lineRight);
            }
        }

        private void DrawLine(Graphics g, int lineY, int lineLeft, int lineRight)
        {
            g.DrawLine(SystemPens.ControlDark, lineLeft, lineY, lineRight, lineY);
            lineY += 1;
            g.DrawLine(SystemPens.ControlLightLight, lineLeft, lineY, lineRight, lineY);
        }

        private void AdvVenueSettings_Load(object sender, EventArgs e)
        {
            this.useLbl.Font = UIFont.StringFont;
            this.inputLbl.Font = UIFont.StringFont;
            this.listLbl.Font = UIFont.StringFont;
            this.expressionInput.Font = UIFont.StringFont;
            this.expressionList.Font = UIFont.StringFont;
            this.addBtn.Font = UIFont.StringFont;
            this.replaceBtn.Font = UIFont.StringFont;
            this.deleteBtn.Font = UIFont.StringFont;
            this.cancelBtn.Font = UIFont.StringFont;
            this.okBtn.Font = UIFont.StringFont;
            this.exampleLbl.Font = UIFont.StringFont;

            this.useLbl.Text = Strings.SpecifyWhoCanSeeThisVenue;
            this.inputLbl.Text = Strings.IdentifierExpression;
            this.listLbl.Text = Strings.ShowVenueOnlyToMatchingParticipants;
            this.addBtn.Text = Strings.Add;
            this.replaceBtn.Text = Strings.Replace;
            this.deleteBtn.Text = Strings.Delete;
            this.cancelBtn.Text = Strings.Cancel;
            this.okBtn.Text = Strings.OK;
            this.exampleLbl.Text = Strings.ForExample;
            this.Text = Strings.AdvancedVenueSettings;
        }

    }
}

using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;


namespace MSR.LST.ConferenceXP.ArchiveService
{
    /// <summary>
    /// Summary description for DeleteRange.
    /// </summary>
    public class DeleteRange : System.Windows.Forms.Form
    {
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label fromLbl;
        private System.Windows.Forms.Label toLbl;
        private System.Windows.Forms.Label timeLbl2;
        private System.ComponentModel.Container components = null;

        private bool text1Valid = true;
        private bool text2Valid = true;

        public DeleteRange()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            this.dateTimePicker1.Value = DateTime.Now;
            this.dateTimePicker2.Value = DateTime.Now;

            // Show text for start & end times (properly localized & globalized, blah, blah, blah)
            DateTime start = new DateTime(1, 1, 1, 0, 0, 0, 0);
            this.textBox1.Text = start.ToLongTimeString();
            DateTime end = new DateTime(1, 1, 1, 23, 59, 59, 999);
            this.textBox2.Text = end.ToLongTimeString();
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
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.fromLbl = new System.Windows.Forms.Label();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.toLbl = new System.Windows.Forms.Label();
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.timeLbl2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(58, 80);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(200, 20);
            this.dateTimePicker1.TabIndex = 0;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(58, 112);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(200, 20);
            this.textBox1.TabIndex = 1;
            this.textBox1.TextChanged += new System.EventHandler(this.OnStartTimeChanged);
            // 
            // fromLbl
            // 
            this.fromLbl.AutoSize = true;
            this.fromLbl.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.fromLbl.ForeColor = System.Drawing.Color.Blue;
            this.fromLbl.Location = new System.Drawing.Point(8, 56);
            this.fromLbl.Name = "fromLbl";
            this.fromLbl.Size = new System.Drawing.Size(40, 16);
            this.fromLbl.TabIndex = 2;
            this.fromLbl.Text = "From";
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.Location = new System.Drawing.Point(58, 176);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(200, 20);
            this.dateTimePicker2.TabIndex = 2;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(58, 208);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(200, 20);
            this.textBox2.TabIndex = 3;
            this.textBox2.TextChanged += new System.EventHandler(this.OnEndTimeChanged);
            // 
            // toLbl
            // 
            this.toLbl.AutoSize = true;
            this.toLbl.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.toLbl.ForeColor = System.Drawing.Color.Blue;
            this.toLbl.Location = new System.Drawing.Point(8, 152);
            this.toLbl.Name = "toLbl";
            this.toLbl.Size = new System.Drawing.Size(24, 16);
            this.toLbl.TabIndex = 2;
            this.toLbl.Text = "To";
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cancelButton.Location = new System.Drawing.Point(186, 264);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 5;
            this.cancelButton.Text = "Cancel";
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.okButton.Location = new System.Drawing.Point(98, 264);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 4;
            this.okButton.Text = "OK";
            // 
            // label3
            // 
            this.label3.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label3.Location = new System.Drawing.Point(8, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(252, 32);
            this.label3.TabIndex = 7;
            this.label3.Text = "Specify the conferences you want to delete by creation date:";
            // 
            // label4
            // 
            this.label4.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label4.Location = new System.Drawing.Point(8, 176);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 24);
            this.label4.TabIndex = 2;
            this.label4.Text = "Date:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // timeLbl2
            // 
            this.timeLbl2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.timeLbl2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.timeLbl2.Location = new System.Drawing.Point(8, 208);
            this.timeLbl2.Name = "timeLbl2";
            this.timeLbl2.Size = new System.Drawing.Size(43, 24);
            this.timeLbl2.TabIndex = 8;
            this.timeLbl2.Text = "Time:";
            this.timeLbl2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label6.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label6.Location = new System.Drawing.Point(8, 112);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(43, 24);
            this.label6.TabIndex = 10;
            this.label6.Text = "Time:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label7
            // 
            this.label7.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label7.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label7.Location = new System.Drawing.Point(8, 80);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(43, 24);
            this.label7.TabIndex = 9;
            this.label7.Text = "Date:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // DeleteRange
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(268, 296);
            this.ControlBox = false;
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.timeLbl2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.fromLbl);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.dateTimePicker2);
            this.Controls.Add(this.toLbl);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.label4);
            this.Font = UIFont.FormFont;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DeleteRange";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Delete Range";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.DeleteRange_Paint);
            this.Load += new System.EventHandler(this.DeleteRange_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private void OnStartTimeChanged(object sender, System.EventArgs e)
        {
            this.text1Valid = this.ValidateTime(this.textBox1.Text);

            SetOkStatus();
        }

        private void OnEndTimeChanged(object sender, System.EventArgs e)
        {
            this.text2Valid = this.ValidateTime(this.textBox2.Text);

            SetOkStatus();
        }

        private bool ValidateTime(string timeString)
        {
            try
            {
                DateTime dt = DateTime.Parse(timeString, CultureInfo.InvariantCulture);
                if( dt != DateTime.MinValue )
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        private void SetOkStatus()
        {
            if (text1Valid && text2Valid)
                this.okButton.Enabled = true;
            else
                this.okButton.Enabled = false;
        }

        private void DeleteRange_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            using(Graphics g = e.Graphics)
            {
                // Draw a line next to the "To" label
                int lineLeft = toLbl.Left + toLbl.Width;
                int lineRight = dateTimePicker1.Left + dateTimePicker1.Width;
                int lineY = toLbl.Top + toLbl.Height/2;
                DrawLine(g, lineY, lineLeft, lineRight);

                // Draw a line next to the "From" label
                lineY = fromLbl.Top + fromLbl.Height/2;
                lineLeft = fromLbl.Left + fromLbl.Width;
                DrawLine(g, lineY, lineLeft, lineRight);

                // Draw a line across the bottom
                lineLeft = toLbl.Left;
                int text2bottom = textBox2.Top + textBox2.Height;
                lineY = text2bottom + (okButton.Top - text2bottom)/2 + 3;
                DrawLine(g, lineY, lineLeft, lineRight);
            }
        }

        private void DrawLine(Graphics g, int lineY, int lineLeft, int lineRight)
        {
            g.DrawLine(SystemPens.ControlDark, lineLeft, lineY, lineRight, lineY);
            lineY += 1;
            g.DrawLine(SystemPens.ControlLightLight, lineLeft, lineY, lineRight, lineY);
        }

        internal DateTime StartDateTime
        {
            get
            {
                string sumString = dateTimePicker1.Value.ToShortDateString() + ' ' + textBox1.Text;
                return DateTime.Parse(sumString, CultureInfo.InvariantCulture);
            }
        }

        internal DateTime EndDateTime
        {
            get
            {
                string sumString = dateTimePicker2.Value.ToShortDateString() + ' ' + textBox2.Text;
                return DateTime.Parse(sumString, CultureInfo.InvariantCulture);
            }
        }

        private void DeleteRange_Load(object sender, EventArgs e)
        {
            this.textBox1.Font = UIFont.StringFont;
            this.fromLbl.Font = UIFont.StringFont;
            this.dateTimePicker1.Font = UIFont.StringFont;
            this.textBox2.Font = UIFont.StringFont;
            this.toLbl.Font = UIFont.StringFont;
            this.dateTimePicker2.Font = UIFont.StringFont;
            this.cancelButton.Font = UIFont.StringFont;
            this.okButton.Font = UIFont.StringFont;
            this.label3.Font = UIFont.StringFont;
            this.label4.Font = UIFont.StringFont;
            this.timeLbl2.Font = UIFont.StringFont;
            this.label6.Font = UIFont.StringFont;
            this.label7.Font = UIFont.StringFont;

            this.fromLbl.Text = Strings.From;
            this.toLbl.Text = Strings.To;
            this.cancelButton.Text = Strings.Cancel;
            this.okButton.Text = Strings.OK;
            this.label3.Text = Strings.SpecifyConferencesToDelete;
            this.label4.Text = Strings.Date;
            this.timeLbl2.Text = Strings.Time;
            this.label6.Text = Strings.Time;
            this.label7.Text = Strings.Date;
            this.Text = Strings.DeleteRange;

        }
    }
}

using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;


namespace MSR.LST.Controls.InkToolBarControls
{
    /// <summary>
    /// Lists the colors for the ColorPickerDialog.
    /// </summary>
    internal class ColorListBox : System.Windows.Forms.ListBox
    {
        public ColorListBox()
        {
            InitializeComponent();
        }

        protected override void OnDrawItem(System.Windows.Forms.DrawItemEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle bounds = e.Bounds;
            bool selected = (e.State & DrawItemState.Selected) > 0;
            bool editSel = (e.State & DrawItemState.ComboBoxEdit ) > 0;
            if ( e.Index != -1 )
                DrawListBoxItem(g, bounds, e.Index, selected, editSel);
        }

        private void InitializeComponent()
        {
            // 
            // ColorListBox
            // 
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));

        }

        protected void DrawListBoxItem(Graphics g, Rectangle bounds, int index, bool selected, bool editSel)
        {
            // Draw List box item
            if ( index != -1)
            {
                if ( selected && !editSel)
                    // Draw highlight rectangle
                    g.FillRectangle(new SolidBrush(SystemColors.Highlight), bounds.Left, bounds.Top, bounds.Width, bounds.Height);
                else
                    // Erase highlight rectangle
                    g.FillRectangle(new SolidBrush(SystemColors.Window), bounds.Left, bounds.Top, bounds.Width, bounds.Height);

                string colorName = (string)this.Items[index];
                Color currentColor = Color.FromName(colorName);

                Brush brush;
                if ( selected )
                    brush =  new SolidBrush(SystemColors.HighlightText);
                else
                    brush = new SolidBrush(SystemColors.MenuText);

                g.FillRectangle(new SolidBrush(currentColor), bounds.Left+2, bounds.Top+2, 20, bounds.Height-4);
                Pen blackPen = new Pen(new SolidBrush(Color.Black), 1);
                g.DrawRectangle(blackPen, new Rectangle(bounds.Left+1, bounds.Top+1, 21, bounds.Height-3));
                g.DrawString(colorName, SystemInformation.MenuFont, brush, new Point(bounds.Left + 28, bounds.Top));
            }
        }
    }
}

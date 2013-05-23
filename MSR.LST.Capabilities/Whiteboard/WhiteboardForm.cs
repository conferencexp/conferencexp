using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

using Microsoft.Ink;

using MSR.LST.Controls;


namespace MSR.LST.ConferenceXP
{
    public class WhiteboardForm : CapabilityForm
    {
        #region Windows Form Designer generated code

        private MSR.LST.Controls.InkToolBar inkToolBar;
        private System.Windows.Forms.PictureBox pbInk;
        private System.Windows.Forms.Panel pnlInkToolbar;
        private IContainer components;

        public WhiteboardForm()
        {
            InitializeComponent();
        }

        [global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303")]
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Microsoft.Ink.DrawingAttributes drawingAttributes3 = new Microsoft.Ink.DrawingAttributes();
            Microsoft.Ink.DrawingAttributes drawingAttributes4 = new Microsoft.Ink.DrawingAttributes();
            this.pnlInkToolbar = new System.Windows.Forms.Panel();
            this.inkToolBar = new MSR.LST.Controls.InkToolBar();
            this.pbInk = new System.Windows.Forms.PictureBox();
            this.pnlInkToolbar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbInk)).BeginInit();
            this.SuspendLayout();

            // 
            // pnlInkToolbar
            // 
            this.pnlInkToolbar.Controls.Add(this.inkToolBar);
            this.pnlInkToolbar.Location = new System.Drawing.Point(0, 0);
            this.pnlInkToolbar.Name = "pnlInkToolbar";
            this.pnlInkToolbar.Size = new System.Drawing.Size(224, 48);
            this.pnlInkToolbar.TabIndex = 1;
            // 
            // inkToolBar
            // 
            this.inkToolBar.ButtonSize = new System.Drawing.Size(16, 16);
            this.inkToolBar.Divider = false;
            drawingAttributes3.AntiAliased = true;
            drawingAttributes3.Color = System.Drawing.Color.Yellow;
            drawingAttributes3.FitToCurve = false;
            drawingAttributes3.Height = 600F;
            drawingAttributes3.IgnorePressure = false;
            drawingAttributes3.PenTip = Microsoft.Ink.PenTip.Rectangle;
            drawingAttributes3.RasterOperation = Microsoft.Ink.RasterOperation.CopyPen;
            drawingAttributes3.Transparency = ((byte)(128));
            drawingAttributes3.Width = 100F;
            this.inkToolBar.HighlighterAttributes = drawingAttributes3;
            this.inkToolBar.Location = new System.Drawing.Point(0, 0);
            this.inkToolBar.Name = "inkToolBar";
            drawingAttributes4.AntiAliased = true;
            drawingAttributes4.Color = System.Drawing.Color.Blue;
            drawingAttributes4.FitToCurve = true;
            drawingAttributes4.Height = 60F;
            drawingAttributes4.IgnorePressure = false;
            drawingAttributes4.PenTip = Microsoft.Ink.PenTip.Ball;
            drawingAttributes4.RasterOperation = Microsoft.Ink.RasterOperation.CopyPen;
            drawingAttributes4.Transparency = ((byte)(0));
            drawingAttributes4.Width = 60F;
            this.inkToolBar.PenAttributes = drawingAttributes4;
            this.inkToolBar.Size = new System.Drawing.Size(224, 50);
            this.inkToolBar.TabIndex = 1;
            this.inkToolBar.EraseAllClicked += new System.EventHandler(this.inkToolBar_EraseAllClicked);
            // 
            // pbInk
            // 
            this.pbInk.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pbInk.BackColor = System.Drawing.Color.White;
            this.pbInk.Location = new System.Drawing.Point(0, 48);
            this.pbInk.Name = "pbInk";
            this.pbInk.Size = new System.Drawing.Size(792, 520);
            this.pbInk.TabIndex = 2;
            this.pbInk.TabStop = false;
            // 
            // WhiteboardForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(792, 566);
            this.Controls.Add(this.pbInk);
            this.Controls.Add(this.pnlInkToolbar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.Name = "WhiteboardForm";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.WhiteboardForm_Closing);
            this.Load += new System.EventHandler(this.WhiteBoardForm_Load);
            this.pnlInkToolbar.ResumeLayout(false);
            this.pnlInkToolbar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbInk)).EndInit();
            this.ResumeLayout(false);

        }


        #endregion

        #region Statics

        private enum CursorID {Mouse = 1, Pen, TopOfPen};
        private static readonly Guid StrokeIdentifier = new Guid("{179222D6-BCC1-4570-8D8F-7E8834C1DD2A}");

        #endregion Statics

        #region Members

        private InkOverlay inkOverlay;
        private WhiteboardCapability wb;

        #endregion Members

        #region Dispose

        protected override void Dispose( bool disposing )
        {
            if( disposing )
            {
                if (components != null) 
                {
                    components.Dispose();
                }

                if(inkOverlay != null)
                {
                    inkOverlay.Dispose();
                }
            }
            base.Dispose( disposing );
        }


        #endregion Dispose

        #region ICapabilityForm

        public override void AddCapability(ICapability capability)
        {
            base.AddCapability (capability);

            if(wb == null)
            {
                wb = (WhiteboardCapability)capability;
                wb.ObjectReceived += new CapabilityObjectReceivedEventHandler(OnObjectReceived);
            }
        }

        public override bool RemoveCapability(ICapability capability)
        {
            bool ret = base.RemoveCapability(capability);

            if(ret)
            {
                // Remove the ObjectReceived event handler.
                // This form is going away, but the Capability may be replayed in which case we'd receive this event into a disposed form!
                wb.ObjectReceived -= new CapabilityObjectReceivedEventHandler(OnObjectReceived);
                wb = null;
            }

            return ret;
        }
 

        #endregion ICapabilityForm

        #region Private

        private void WhiteBoardForm_Load(object sender, System.EventArgs e)
        {
            // Provide the InkOverlay with its drawing surface
            inkOverlay = new InkOverlay(pbInk);

            // Hook up stroke events
            inkOverlay.Stroke += new InkCollectorStrokeEventHandler(inkOverlay_Stroke);
            inkOverlay.StrokesDeleting += new InkOverlayStrokesDeletingEventHandler(inkOverlay_StrokesDeleting);
            inkOverlay.CursorInRange += new InkCollectorCursorInRangeEventHandler(inkOverlay_CursorInRange);

            // We're now set to go, so turn on tablet input
            inkOverlay.Enabled = true;

            // Link the inkOverlay to the inkToolBar control
            inkToolBar.InkOverlay = inkOverlay;
        }

        private void WhiteboardForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(wb != null)
            {
                if(wb.IsSending)
                {
                    wb.StopSending();
                }
            }
        }
        
        
        private void inkToolBar_EraseAllClicked(object sender, System.EventArgs e)
        {
            pbInk.Refresh();
            wb.SendObject(new EraseAllInk());
        }


        private void inkOverlay_Stroke(object sender, InkCollectorStrokeEventArgs e)
        {
            Strokes strokes = null;
            Ink copiedInk = null;

            try
            {
                // Eraser strokes are transparent, we don't need to send them
                if (e.Stroke.DrawingAttributes.Transparency != 255)
                {
                    // Give the stroke an identifier, so we can delete it remotely
                    e.Stroke.ExtendedProperties.Add(StrokeIdentifier, Guid.NewGuid().ToString());

                    // Copy the stroke into its own Ink object, so that it can be serialized
                    strokes = inkOverlay.Ink.CreateStrokes(new int[]{e.Stroke.Id});
                    copiedInk = e.Stroke.Ink.ExtractStrokes(strokes, ExtractFlags.CopyFromOriginal);

                    // Send it across the network
                    wb.SendObject(new SerializedInk(copiedInk));
                }
            }
            catch(Exception ex)
            {
                Log(ex.ToString());
            }
            finally
            {
                if(strokes != null)
                {
                    strokes.Dispose();
                }

                if(copiedInk != null)
                {
                    copiedInk.Dispose();
                }
            }
        }

        private void inkOverlay_StrokesDeleting(object sender, InkOverlayStrokesDeletingEventArgs e)
        {
            string[] identifiers = new string[e.StrokesToDelete.Count];

            for(int i = 0; i < e.StrokesToDelete.Count; i++)
            {
                identifiers[i] = (string)e.StrokesToDelete[i].ExtendedProperties[StrokeIdentifier].Data;
            }

            wb.SendObject(new DeletedStrokes(identifiers));
        }

        private void inkOverlay_CursorInRange(object sender, InkCollectorCursorInRangeEventArgs e)
        {
            // Ignore the case of the mouse cursor entering the control
            if (e.Cursor.Id != (int)CursorID.Mouse)
            {
                // Check to see if the pen cursor is inverted or the toolbar is in erase mode
                if(e.Cursor.Inverted || inkToolBar.Mode == InkToolBarMode.Eraser)        
                {
                    if(inkOverlay.EditingMode != InkOverlayEditingMode.Delete)
                    {
                        inkOverlay.EditingMode = InkOverlayEditingMode.Delete;
                    }
                }   
                else
                {   
                    if(inkOverlay.EditingMode != InkOverlayEditingMode.Ink)
                    {
                        inkOverlay.EditingMode = InkOverlayEditingMode.Ink;
                    }
                }
            }
        }

        
        private void OnObjectReceived(object o, ObjectReceivedEventArgs orea)
        {
            try
            {
                // Don't receive your own data
                if (orea.Participant != Conference.LocalParticipant)
                {
                    if (orea.Data is SerializedInk)
                    {
                        Strokes strokes = ((SerializedInk)orea.Data).Strokes;
                        
                        // This call fails, claiming the strokes are in 2 ink objects
                        // inkOverlay.Ink.Strokes.Add(strokes);

                        // But this call works
                        inkOverlay.Ink.AddStrokesAtRectangle(strokes, strokes.GetBoundingBox());
                    }
                    else if(orea.Data is DeletedStrokes)
                    {
                        string[] identifiers = ((DeletedStrokes)orea.Data).Identifiers;
                        Ink ink = inkOverlay.Ink;

                        for(int i = 0; i < identifiers.Length; i++)
                        {
                            for(int j = ink.Strokes.Count - 1; j >= 0; j--)
                            {
                                Stroke stroke = ink.Strokes[j];

                                if((string)stroke.ExtendedProperties[StrokeIdentifier].Data == identifiers[i])
                                {
                                    ink.DeleteStroke(stroke);
                                    break;
                                }
                            }
                        }
                    }
                    else if(orea.Data is EraseAllInk)
                    {
                        inkOverlay.Ink.DeleteStrokes();
                    }

                    // Update the screen when data arrives
                    pbInk.Refresh();
                }
            }
            catch ( Exception e)
            {
                Log(string.Format(CultureInfo.CurrentCulture, "Exception: {0}", e.ToString()));
            }
        }

        
        private void Log(string msg)
        {
            Console.WriteLine(msg);
        }

        
        #endregion Private

        #region Serialization Classes

        [Serializable]
        private class DeletedStrokes
        {
            private string[] identifiers;

            public DeletedStrokes(string[] identifiers)
            {
                this.identifiers = identifiers;
            }

            public string[] Identifiers
            {
                get
                {
                    return identifiers; 
                }
            }
        }

        [Serializable]
        private class SerializedInk
        {
            private byte[] inkData;

            public SerializedInk(Ink ink)
            {
                inkData = ink.Save(PersistenceFormat.InkSerializedFormat);
            }

            public Strokes Strokes
            {
                get
                {
                    if(inkData == null)
                    {
                        throw new Exception(Strings.ThereIsNoDataToConvert);
                    }

                    Ink ink = new Ink();
                    ink.Load(inkData);

                    return ink.Strokes; 
                }
            }
        }

        [Serializable]
        private class EraseAllInk{}

        
        #endregion Serialization Classes
    }
}

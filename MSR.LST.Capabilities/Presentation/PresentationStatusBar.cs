using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;


namespace MSR.LST.ConferenceXP
{
    /// <summary>
    /// A System.Windows.Forms.StatusBar overriden and customized for PresentationCapability, for ease of use.
    /// </summary>
    internal class PresentationStatusBar : System.Windows.Forms.StatusBar
    {
        StatusBarPanel pagePanel, messagePanel;
        int page, maxPage;
        readonly string buffer = "    ";

        /// <summary>
        /// I like status bars.  Status bars are good.
        /// </summary>
        public PresentationStatusBar()
        {
            // Pri4: It occurs to me that messages on a status bar can be much like a stack in
            //   that a new message can occur when one is already being displayed.  Maybe we should
            //   make a nice statusBar that has a built-in stack to push & pop messages (as well
            //   as clear the stack).

            this.ShowPanels = true;

            messagePanel = new StatusBarPanel();
            messagePanel.Style = StatusBarPanelStyle.Text;
            messagePanel.BorderStyle = StatusBarPanelBorderStyle.Sunken;
            messagePanel.AutoSize = StatusBarPanelAutoSize.Spring;

            pagePanel = new StatusBarPanel();
            pagePanel.Style = StatusBarPanelStyle.Text;
            pagePanel.BorderStyle = StatusBarPanelBorderStyle.Sunken;
            pagePanel.AutoSize = StatusBarPanelAutoSize.Contents;
            pagePanel.Alignment = HorizontalAlignment.Center;
            pagePanel.MinWidth = 100;
            pagePanel.Text = Strings.Slide_Of_Placeholder;

            this.Panels.Add(messagePanel);
            this.Panels.Add(pagePanel);
        }

        /// <summary>
        /// Sets the status message and sets the cursor to the AppStartingCursor
        /// </summary>
        public void SetBusyStatusMessage(string message)
        {
            this.TopLevelControl.Cursor = Cursors.AppStarting;
            this.messagePanel.Text = buffer + message;
            this.Invalidate();
        }

        /// <summary>
        /// Sets the status message and sets the cursor to the WaitCursor.
        /// </summary>
        public void SetWaitStatusMessage(string message)
        {
            this.TopLevelControl.Cursor = Cursors.WaitCursor;
            this.messagePanel.Text = buffer + message;
        }

        /// <summary>
        /// Sets the status message to blank and sets the cursor to Default.
        /// </summary>
        public void SetReadyStatusMessage()
        {
            this.TopLevelControl.Cursor = Cursors.Default;
            this.messagePanel.Text = String.Empty;
        }

        /// <summary>
        /// Gets or sets the status message in the left-most StatusBarPanel.
        /// </summary>
        public string StatusMessage
        {
            get
            {
                return this.messagePanel.Text;
            }
            set
            {
                this.messagePanel.Text = value;
            }
        }

        /// <summary>
        /// Gets or sets the current page value.
        /// </summary>
        public int CurrentPage
        {
            get
            {
                return this.page;
            }
            set
            {
                if( page > maxPage )
                    throw new ArgumentOutOfRangeException(Strings.ValueGreaterThanMaxpagenum);
                this.page = value;
                pagePanel.Text = string.Format(CultureInfo.CurrentCulture, Strings.Slide_Of_, page, maxPage);
            }
        }

        /// <summary>
        /// Gets the maximum page value.
        /// </summary>
        public int MaxPageVal
        {
            get
            {
                return this.maxPage;
            }
        }

        /// <summary>
        /// Upon opening a new document, this method allows simultaneously setting the maximum page
        /// value, as well as the current page value.
        /// </summary>
        /// <remarks>
        /// By setting the max page and the current page at the same time, we prevent exceptions in the case
        /// that the new document's max page is smaller than the current document's current page value.
        /// </remarks>
        public void SetMaxPage( int currentPage, int maxPageVal )
        {
            this.maxPage = maxPageVal;
            this.CurrentPage = currentPage;
        }
    }
}

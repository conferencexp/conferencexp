using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Forms;


namespace MSR.LST
{
    /// <summary>
    /// To display a message box correctly for cultures that use a right-to-left reading order, the RightAlign 
    /// and RtlReading members of the MessageBoxOptions enumeration must be passed to the Show method. Examine 
    /// the System.Windows.Forms.Control.RightToLeft property of the containing control to determine whether to 
    /// use a right-to-left reading order.
    /// 
    /// See http://msdn2.microsoft.com/en-us/library/ms182191(VS.80).aspx for more information. CRN
    /// </summary>
    public static class RtlAwareMessageBox
    {
        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons,
            MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options)
        {
            if (IsRightToLeft(owner))
            {
                options |= MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign;
            }

            return MessageBox.Show(owner, text, caption, buttons, icon, defaultButton, options);
        }

        private static bool IsRightToLeft(IWin32Window owner)
        {
            Control control = owner as Control;

            if (control != null)
            {
                return control.RightToLeft == RightToLeft.Yes;
            }

            // If no parent control is available, ask the CurrentUICulture if we are running under right-to-left.
            return CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft;
        }
    }
}

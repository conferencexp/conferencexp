using System;
using System.Collections;
using System.Windows.Forms;


namespace MSR.LST.ConferenceXP
{
    /// <summary>
    /// ICapabilityForm is the new way to add capabilities to a form and reference count the form
    /// so that it doesn not disappear prematurely.
    /// </summary>
    public interface ICapabilityForm
    {
        /// <summary>
        /// Add a capability object to the form
        /// </summary>
        /// <param name="capability">The capability object to add</param>
        void AddCapability(ICapability capability);

        /// <summary>
        /// Remove a capability object from the form
        /// </summary>
        /// <param name="capability">The capability object to remove</param>
        /// <returns>true if there are no more instances of this capability</returns>
        bool RemoveCapability(ICapability capability);

        /// <summary>
        /// Return the count of capability objects referring to the form
        /// </summary>
        /// <returns>The count of capability objects</returns>
        int Count();
    }


    /// <summary>
    /// CapabilityForm is a default implementation of ICapabilityForm
    /// 
    /// Visual Studio automatically adds a form and a resx file when inheriting from Windows.Forms.Form
    /// </summary>
    public class CapabilityForm : Form, ICapabilityForm
    {
        // TODO: We could have a ctor with constraints as param

        // Array list of capability objects referring to the shared form containing an instance
        // of this class
        private ArrayList listCapabilities = new ArrayList();

        /// <summary>
        /// Add a capability object to the form
        /// 
        /// Override this method in order to do something special with the capability being added
        /// </summary>
        /// <param name="capability">The capability object to add</param>
        public virtual void AddCapability(ICapability capability)
        {
            // TODO: add validations "contains"

            listCapabilities.Add(capability);

            #region Diagnostics
            #if SFHashTableDiagnostics
                Trace.WriteLine("    SharedForm.AddCapability " + capability.GetType());
                Trace.WriteLine("    new count after add: listCapabilities.Count: " + listCapabilities.Count);
            #endif
            #endregion Diagnostics
        }

        /// <summary>
        /// Remove a capability object from the form
        /// 
        /// Override this method in order to do something special with the capability being removed
        /// </summary>
        /// <param name="capability">The capability object to remove</param>
        /// <returns>true if there are no more instances of this capability</returns>
        public virtual bool RemoveCapability(ICapability capability)
        {
            // TODO: add validations "contains"

            listCapabilities.Remove(capability);

            #region Diagnostics
#if SFHashTableDiagnostics
                Trace.WriteLine("    SharedForm.RemoveCapability " + capability.GetType());
                Trace.WriteLine("    new count after remove: listCapabilities.Count: " 
                    + listCapabilities.Count);
#endif
            #endregion Diagnostics

            return !listCapabilities.Contains(capability);
        }

        /// <summary>
        /// Return the count of capability objects referring to the form
        /// </summary>
        /// <returns>The count of capability objects</returns>
        public int Count()
        {
            return listCapabilities.Count;
        }
    }
}

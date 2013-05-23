using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Globalization;

namespace MSR.LST.Net.Heartbeat {
    public static class Constants {
        //Configurable constants:
        public static readonly string Address = "233.0.73.19";
        public static readonly int Port = 2112;
        public static readonly int Period = 1000; // ms
        // magic cookie that indicates a legitimate heartbeat message
        public static readonly uint Cookie = 0xDECAFBAD;
        public static readonly ushort Ttl = 32;

        #region App.Config Overrides
        private const string baseName = "MSR.LST.Net.Heartbeat.";

        /// <summary>
        /// This static constructor checks every non-constant static field in the class for an app.config override.
        /// </summary>
        static Constants() {
            Type myType = typeof(Constants);
            FieldInfo[] fields = myType.GetFields(BindingFlags.Public | BindingFlags.Static);
            foreach (FieldInfo field in fields) {
                if (field.IsLiteral) { // Is Constant?
                    continue;
                }
                string fullName = baseName + field.Name;
                string configOverride = System.Configuration.ConfigurationManager.AppSettings[fullName];
                if (configOverride != null && configOverride != String.Empty) {
                    Type newType = field.FieldType;
                    object newValue = Convert.ChangeType(configOverride, newType, CultureInfo.InvariantCulture);
                    field.SetValue(null, newValue);
                }
            }
        }
        #endregion

    }
}

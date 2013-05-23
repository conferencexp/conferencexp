using System;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


namespace MSR.LST.RTDocuments
{
    /// <summary>
    /// Extensibility mechanism for RTObjects
    /// </summary>
    [Serializable]
    public class RTFrame
    {
        public Guid ObjectTypeIdentifier;
        
        private byte[] serializedObject = null;

        public object Object 
        {
            get 
            {
                lock(this) 
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    MemoryStream ms = new MemoryStream(serializedObject);
                    return bf.Deserialize(ms);
                }
            }
        }

        public RTFrame(Guid objectTypeIdentifier, object o)
        {
            lock(this)
            {
                ObjectTypeIdentifier = objectTypeIdentifier;

                // the object o could be null if it's a datalist 
                // message with a guid, but no byte array
                if (o != null)
                {
                    MemoryStream ms = new MemoryStream();
                    BinaryFormatter bf = new BinaryFormatter();

                    bf.Serialize(ms, o);

                    // Get a byte[] of the serialized object
                    int numBytes = (int)ms.Position; // record the number of "useful bytes"
                    serializedObject = new Byte[numBytes];
                    ms.Position = 0; // set the pointer back to 0, so we can read from that point
                    ms.Read(serializedObject, 0, numBytes); // read all the useful bytes
                }
            }
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, "RTFrame [ ObjectTypeIdentifier: {0} ]", 
                ObjectTypeIdentifier.ToString());
        }

    }
}

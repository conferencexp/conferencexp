using System;
using System.Collections;
using System.Net;
using System.Text;

// Most (or all) of the collection classes contained herein can be replaced by a template class 
// as soon as the framework supports templates (generics) JVE 3/12/04


namespace MSR.LST.Net.Rtp
{
    public class CNameToParticipantHashtable : DictionaryBase
    {
        public CNameToParticipantHashtable(int length) : base() {}
        public CNameToParticipantHashtable() : base() {}
        
        public ICollection Keys  
        {
            get{return( Dictionary.Keys );}
        }

        public ICollection Values  
        {
            get{return( Dictionary.Values );}
        }

        public object Clone()
        {
            CNameToParticipantHashtable clone = new CNameToParticipantHashtable();

            foreach(DictionaryEntry de in Dictionary)
            {
                clone.Add((string)de.Key, (RtpParticipant)de.Value);
            }

            return clone;
        }

        public RtpParticipant this[string cName]
        {
            get{return (RtpParticipant) Dictionary[cName];}
            set{Dictionary[cName] = value;}
        }

        public void Add(string cName, RtpParticipant rtpParticipant)
        {
            Dictionary.Add(cName, rtpParticipant);
        }

        public bool Contains(string cName)
        {
            return Dictionary.Contains(cName);
        }

        public bool ContainsKey(string cName)
        {
            return Dictionary.Contains(cName);
        }

        public void Remove(string cName)
        {
            Dictionary.Remove(cName);
        }

    }

    public class SSRCToParticipantHashtable : DictionaryBase
    {
        public SSRCToParticipantHashtable(int length) : base() {}
        public SSRCToParticipantHashtable() : base() {}
        
        public ICollection Keys  
        {
            get{return( Dictionary.Keys );}
        }

        public ICollection Values  
        {
            get{return( Dictionary.Values );}
        }

        public object Clone()
        {
            SSRCToParticipantHashtable clone = new SSRCToParticipantHashtable();

            foreach( DictionaryEntry de in Dictionary )
            {
                clone.Add((uint)de.Key, (RtpParticipant)de.Value);
            }

            return clone;
        }

        public RtpParticipant this[uint ssrc]
        {
            get{return (RtpParticipant) Dictionary[ssrc];}
            set{Dictionary[ssrc] = value;}
        }

        public void Add(uint ssrc, RtpParticipant participant)
        {
            Dictionary.Add(ssrc, participant);
        }

        public bool Contains(uint ssrc)
        {
            return Dictionary.Contains(ssrc);
        }

        public bool ContainsKey(uint ssrc)
        {
            return Dictionary.Contains(ssrc);
        }

        public void Remove(uint ssrc)
        {
            Dictionary.Remove(ssrc);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            //return base.ToString();
            foreach (Object o in Keys) 
            {
                builder.Append(o.ToString() + " == > " + Dictionary[o].ToString() + ", ");
            }

            return builder.ToString();
        }
    }

    public class SSRCToStreamHashtable : DictionaryBase
    {
        public SSRCToStreamHashtable(int length) : base() {}
        public SSRCToStreamHashtable() : base() {}
        
        public ICollection Keys  
        {
            get{return( Dictionary.Keys );}
        }

        public ICollection Values  
        {
            get{return( Dictionary.Values );}
        }

        public object Clone()
        {
            SSRCToStreamHashtable clone = new SSRCToStreamHashtable();

            foreach(DictionaryEntry de in Dictionary)
            {
                clone.Add((uint)de.Key, (RtpStream)de.Value);
            }

            return clone;
        }

        public RtpStream this[uint ssrc]
        {
            get{return (RtpStream)Dictionary[ssrc];}
            set{Dictionary[ssrc] = value;}
        }

        public void Add(uint ssrc, RtpStream rtpStream)
        {
            Dictionary.Add(ssrc, rtpStream);
        }

        public bool Contains(uint ssrc)
        {
            return Dictionary.Contains(ssrc);
        }

        public bool ContainsKey(uint ssrc)
        {
            return Dictionary.Contains(ssrc);
        }

        public void Remove(uint ssrc)
        {
            Dictionary.Remove(ssrc);
        }
    }

    public class IPStreamPairHashtable : DictionaryBase
    {
        public class IPStreamPair
        {
            public IPStreamPair(IPAddress ip, RtpStream rtp)
            { senderIP = ip; stream = rtp; }

            public IPAddress senderIP;
            public RtpStream stream;
        }

        public IPStreamPairHashtable(int length) : base() {}
        public IPStreamPairHashtable() : base() {}
        
        public ICollection Keys  
        {
            get{return(Dictionary.Keys);}
        }

        public ICollection Values  
        {
            get{return(Dictionary.Values);}
        }

        public object Clone()
        {
            IPStreamPairHashtable clone = new IPStreamPairHashtable();

            foreach(DictionaryEntry de in Dictionary)
            {
                clone.Add((uint)de.Key, (IPStreamPair)de.Value);
            }

            return clone;
        }

        public static explicit operator SSRCToStreamHashtable(IPStreamPairHashtable original)
        {
            SSRCToStreamHashtable streams = new SSRCToStreamHashtable(original.Count);
            foreach(DictionaryEntry de in original.Dictionary)
            {
                IPStreamPair ipsp = (IPStreamPair)de.Value;
                if( ipsp != null )
                    streams.Add((uint)de.Key, ipsp.stream);
            }
            return streams;
        }

        public IPStreamPair this[uint ssrc]
        {
            get{return (IPStreamPair)Dictionary[ssrc];}
            set{Dictionary[ssrc] = value;}
        }

        public void Add(uint ssrc, IPStreamPair ipRtpStreamPair)
        {
            Dictionary.Add(ssrc, ipRtpStreamPair);
        }

        public bool Contains(uint ssrc)
        {
            return Dictionary.Contains(ssrc);
        }

        public bool ContainsKey(uint ssrc)
        {
            return Dictionary.Contains(ssrc);
        }

        public RtpStream GetStream(uint ssrc)
        {
            IPStreamPair ipsp = (IPStreamPair)Dictionary[ssrc];
            if( ipsp != null )
                return ipsp.stream;
            else
                return null;
        }

        public IPAddress GetIP(uint ssrc)
        {
            IPStreamPair ipsp = (IPStreamPair)Dictionary[ssrc];
            if( ipsp != null )
                return ipsp.senderIP;
            else
                return null;
        }

        public void Remove(uint ssrc)
        {
            Dictionary.Remove(ssrc);
        }
    }

    public class SsrcToSdesDataHashtable : DictionaryBase
    {
        public SsrcToSdesDataHashtable(int length) : base() {}
        public SsrcToSdesDataHashtable() : base() {}
        
        public ICollection Keys  
        {
            get{return(Dictionary.Keys);}
        }

        public ICollection Values  
        {
            get{return(Dictionary.Values);}
        }

        public SsrcToSdesDataHashtable Clone()
        {
            SsrcToSdesDataHashtable clone = new SsrcToSdesDataHashtable();

            foreach(DictionaryEntry de in Dictionary)
            {
                clone.Add((uint)de.Key, (SdesData)de.Value);
            }

            return clone;
        }

        public SdesData this[uint ssrc]
        {
            get{return (SdesData)Dictionary[ssrc];}
            set{Dictionary[ssrc] = value;}
        }

        public void Add(uint ssrc, SdesData props)
        {
            Dictionary.Add(ssrc, props);
        }

        public bool Contains(uint ssrc)
        {
            return Dictionary.Contains(ssrc);
        }

        public bool ContainsKey(uint ssrc)
        {
            return Dictionary.Contains(ssrc);
        }

        public void Remove(uint ssrc)
        {
            Dictionary.Remove(ssrc);
        }
    }

    public class SSRCToSenderHashtable : DictionaryBase
    {
        public SSRCToSenderHashtable(int length) : base() {}
        public SSRCToSenderHashtable() : base() {}
        
        public ICollection Keys  
        {
            get{return(Dictionary.Keys);}
        }

        public ICollection Values  
        {
            get{return( Dictionary.Values );}
        }

        public object Clone()
        {
            SSRCToSenderHashtable clone = new SSRCToSenderHashtable();

            foreach(DictionaryEntry de in Dictionary)
            {
                clone.Add((uint)de.Key, (RtpSender)de.Value);
            }

            return clone;
        }

        public RtpSender this[uint ssrc]
        {
            get{return (RtpSender) Dictionary[ssrc];}
            set{Dictionary[ssrc] = value;}
        }

        public void Add(uint ssrc, RtpSender rtpSender)
        {
            Dictionary.Add(ssrc, rtpSender);
        }

        public bool Contains(uint ssrc)
        {
            return Dictionary.Contains ( ssrc );
        }

        public bool ContainsKey(uint ssrc)
        {
            return Dictionary.Contains ( ssrc );
        }

        public void Remove(uint ssrc)
        {
            Dictionary.Remove ( ssrc );
        }

    }

    public class SdesPrivateExtensionHashtable : IEnumerable
    {
        private Hashtable h;
        private ByteComparer comparer = new ByteComparer();

        public SdesPrivateExtensionHashtable(int length)
        {
            h = new Hashtable(length, comparer);
        }
        public SdesPrivateExtensionHashtable() : base()
        {
            h = new Hashtable(comparer);
        }
        
        public ICollection Keys  
        {
            get{return(h.Keys);}
        }

        public ICollection Values  
        {
            get{return(h.Values);}
        }

        public object Clone()
        {
            SdesPrivateExtensionHashtable clone = new SdesPrivateExtensionHashtable();

            foreach(DictionaryEntry de in h)
            {
                clone.Add((byte[])de.Key, (byte[])de.Value);
            }

            return clone;
        }

        public byte[] this[byte[] prefix]
        {
            get{return (byte[])h[prefix];}
            set{h[prefix] = value;}
        }

        public void Add(byte[] prefix, byte[] data)
        {
            h.Add(prefix, data);
        }

        public bool Contains(byte[] prefix)
        {
            return h.Contains(prefix);
        }

        public bool ContainsKey(byte[] prefix)
        {
            return h.Contains(prefix);
        }

        public void Remove(byte[] prefix)
        {
            h.Remove(prefix);
        }

        
        IEnumerator IEnumerable.GetEnumerator()
        {
            return h.GetEnumerator();
        }


        private class ByteComparer : IEqualityComparer
        {
            bool IEqualityComparer.Equals(object x, object y)
            {
                byte[] xBytes = (byte[])x;
                byte[] yBytes = (byte[])y;

                // Make sure lengths are the same
                if (xBytes.Length == yBytes.Length)
                {
                    // Compare bytes
                    for (int i = 0; i < xBytes.Length; i++)
                    {
                        if (xBytes[i] != yBytes[i])
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    return false;
                }

                return true;
            }

            int IEqualityComparer.GetHashCode(object obj)
            {
                // This is very specific to this collection and not terribly intelligent
                // but it doesn't need to be, because we don't expect many items in the hashtable
                return ((byte[])obj).Length;
            }
        }
    }
}

using System;
using System.Collections;
using System.Runtime.Serialization;


namespace MSR.LST.RTDocuments
{
    [Serializable]
    public class TOCList : ICollection, IEnumerable, ISerializable, ICloneable
    {
        #region State holders for list
        private ArrayList arrayList = new ArrayList();
        private Hashtable hashTable = new Hashtable();
        #endregion
        #region CTors
        public TOCList() {}
        public TOCList(SerializationInfo info, StreamingContext context)
        {
            TOCNode[] tocNodes = null;

            SerializationInfoEnumerator enumerator = info.GetEnumerator();
            while( enumerator.MoveNext()) 
            {
                switch( enumerator.Name) 
                {
                    case "TOCNodes":
                        tocNodes = (TOCNode[])info.GetValue("TOCNodes", typeof(TOCNode[]));
                        break;
                }
            }

            if (tocNodes == null)
                throw new SerializationException();

            arrayList.Clear();
            hashTable.Clear();

            for (int i = 0; i < tocNodes.Length; i++)
            {
                arrayList.Add(tocNodes[i]);
                hashTable.Add(tocNodes[i].Identifier, tocNodes[i]);
            }
        }

        #endregion
        #region IEnumerable Members
        public IEnumerator GetEnumerator()
        {
            return arrayList.GetEnumerator();
        }

        #endregion
        #region "IList" Members (modified for strongly typed)
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public TOCNode this[int index]
        {
            get
            {
                return (TOCNode)arrayList[index];
            }
            set
            {
                arrayList[index] = value;
                hashTable[value.Identifier] = value;
            }
        }

        public TOCNode this[Guid key]
        {
            get
            {
                return (TOCNode)hashTable[key];
            }
            set
            {
                if (!arrayList.Contains(value))
                {
                    arrayList.Add(value);
                }
                hashTable[key] = value;
            }
        }
        public void RemoveAt(int index)
        {
            TOCNode tocNode = (TOCNode)arrayList[index];
            hashTable.Remove(tocNode.Identifier);
            arrayList.RemoveAt(index);
        }

        public void Insert(int index, TOCNode value)
        {
            arrayList.Insert(index, value);
            hashTable.Add(value.Identifier, value);
        }

        public void Remove(TOCNode value)
        {
            arrayList.Remove(value);
            hashTable.Remove(value.Identifier);
        }
        public void Remove(Guid key)
        {
            TOCNode tn = (TOCNode)hashTable[key];
            arrayList.Remove(tn);
            hashTable.Remove(key);
        }
        public bool Contains(TOCNode value)
        {
            return arrayList.Contains(value);
        }

        public bool ContainsKey(Guid key)
        {
            return hashTable.ContainsKey(key);
        }

        public bool ContainsValue(TOCNode value)
        {
            return hashTable.ContainsValue(value);
        }

        public void Clear()
        {
            arrayList.Clear();
            hashTable.Clear();
        }

        public int IndexOf(TOCNode value)
        {
            return arrayList.IndexOf(value);
        }

        public int Add(TOCNode value)
        {
            hashTable.Add(value.Identifier, value);
            return arrayList.Add(value);
        }

        public bool IsFixedSize
        {
            get
            {
                return false;
            }
        }

        public int Length
        {
            get
            {
                return arrayList.Count;
            }
        }

        #endregion
        #region ICollection Members
        public bool IsSynchronized
        {
            get
            {
                return false;
            }
        }

        public int Count
        {
            get
            {
                return arrayList.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            arrayList.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return null;
            }
        }

        #endregion
        #region ISerializable Members
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            TOCNode[] tns = (TOCNode[])arrayList.ToArray(typeof(TOCNode));
            info.AddValue("TOCNodes", tns, typeof(TOCNode[]));
        }
        #endregion
        public object Clone()
        {
            TOCList tocListNew = new TOCList();

            foreach(TOCNode tn in arrayList)
            {
                tocListNew.Add((TOCNode)tn.Clone());
            }
            return tocListNew;
        }
    }
    [Serializable]
    public class ResourceHashtable : DictionaryBase
    {
        public ResourceHashtable(int length) : base() {}
        public ResourceHashtable() : base() {}
        
        public ICollection Keys  
        {
            get  
            {
                return( Dictionary.Keys );
            }
        }

        public ICollection Values  
        {
            get  
            {
                return( Dictionary.Values );
            }
        }

        public object Clone()
        {
            ResourceHashtable rHT = new ResourceHashtable();

            foreach( DictionaryEntry de in Dictionary )
                rHT.Add((Guid)de.Key, (Resource)de.Value);

            return rHT;
        }

        public Resource this [ Guid identifier ]
        {
            get
            {
                return (Resource) Dictionary[identifier];
            }
            set
            {
                Dictionary[identifier] = value;
            }
        }

        public void Add ( Guid identifier, Resource resource )
        {
            Dictionary.Add( identifier, resource );
        }

        public bool Contains ( Guid identifier )
        {
            return Dictionary.Contains ( identifier );
        }

        public bool ContainsKey ( Guid identifier )
        {
            return Dictionary.Contains ( identifier );
        }

        public void Remove ( Guid identifier )
        {
            Dictionary.Remove ( identifier );
        }
    }
    [Serializable]
    public class PageHashtable : DictionaryBase
    {
        public PageHashtable(int length) : base() {}
        public PageHashtable() : base() {}
        
        public ICollection Keys  
        {
            get  
            {
                return( Dictionary.Keys );
            }
        }

        public ICollection Values  
        {
            get  
            {
                return( Dictionary.Values );
            }
        }

        public object Clone()
        {
            PageHashtable pHT = new PageHashtable();

            foreach( DictionaryEntry de in Dictionary )
                pHT.Add((Guid)de.Key, (Page)de.Value);

            return pHT;
        }

        public Page this [ Guid identifier ]
        {
            get
            {
                return (Page) Dictionary[identifier];
            }
            set
            {
                Dictionary[identifier] = value;
            }
        }

        public void Add ( Guid identifier, Page page )
        {
            Dictionary.Add( identifier, page );
        }

        public bool Contains ( Guid identifier )
        {
            return Dictionary.Contains ( identifier );
        }

        public bool ContainsKey ( Guid identifier )
        {
            return Dictionary.Contains ( identifier );
        }

        public void Remove ( Guid identifier )
        {
            Dictionary.Remove ( identifier );
        }
    }



}

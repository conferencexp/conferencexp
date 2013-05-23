using System;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;


namespace MSR.LST.ConferenceXP.VenueService
{
    public class FileStorage: IDisposable
    {
        #region Private variables
        /// <summary>
        /// The FileInfo object for the file we're reading from & writing to.
        /// </summary>
        FileInfo info;

        /// <summary>
        /// Constantly tracks the last edit on the file by tracking its length.  Unfortunately,
        /// LastWriteTime could not be used because it returns null when the internet guest account
        /// is reading from the file (for reasons not understood).  The last file length should be
        /// sufficient for our purposes.
        /// </summary>
        long lastLength;

        /// <summary>
        /// The file for reading & writing the venue service data to.
        /// </summary>
        FileStream file = null;

        /// <summary>
        /// The serialization formatter for writing to/from the file.
        /// </summary>
        IFormatter formatter = null;

        /// <summary>
        /// In-memory, cached set of participants this venue service holds.
        /// </summary>
        SortedList participants = null;

        /// <summary>
        /// In-memory, cached set of venues this venue service holds.
        /// </summary>
        SortedList venues = null;

        /// <summary>
        /// Associate an (optional) password hash with a venue identifer
        /// </summary>
        IDictionary<String,byte[]> passwords = null;

        /// <summary>
        /// If this is true, all write operations are cached in memory & not written to disk
        /// until it's turned off.
        /// </summary>
        bool cacheWriteOps = false;

        #endregion

        #region Ctor / Init
        public FileStorage( string path )
        {
            // Open the file
            file = new FileStream(path, FileMode.OpenOrCreate);

            // Create the info object
            info = new FileInfo(path);
            lastLength = info.Length;

            try
            {
                // Check for read & write ability
                if( !(file.CanRead && file.CanWrite) )
                    throw new IOException(Strings.FileFilepathCouldNotBeOpened);

                // Create our formatter
                formatter = new BinaryFormatter();

                InitializeDataStructures();
            }
            finally
            {
                file.Close();
            }
        }

        /// <summary>
        /// Reads the data from the file & writes it to the data structures.
        /// </summary>
        private void InitializeDataStructures()
        {
            lock(file)
            {
                // Read in the list of venues, and if none exists, create a new SortedList
                try
                {
                    venues = (SortedList)formatter.Deserialize(file);
                }
                catch
                {
                    venues = new SortedList(CaseInsensitiveComparer.Default);
                }

                // Read in the list of participants, and if none exists, create a new SortedList
                try
                {
                    participants = (SortedList)formatter.Deserialize(file);
                }
                catch
                {
                    participants = new SortedList(CaseInsensitiveComparer.Default);
                }

                // Read in the list of passwords, and if none exists, create a new dictionary
                try
                {
                    passwords = (IDictionary<String,byte[]>)formatter.Deserialize(file);
                }
                catch
                {
                    passwords = new Dictionary<String, byte[]>();
                }

                // Make sure both lists are thread-safe
                venues = SortedList.Synchronized(venues);
                participants = SortedList.Synchronized(participants);
                
            }
        }
        #endregion

        #region IVenueServiceStorage Members

        /// <summary>
        /// At present, the venue has "weak password" status iff the password field
        /// is non-null and no empty.  There is no support for strong (encryption-based)
        /// passwords.
        /// 
        /// </summary>
        /// <param name="v"></param>
        /// <param name="pvs"></param>
        //private void SetPasswordStatus(VenueState vs)
        //{
        //    PrivateVenueState pvs = vs.PrivateVenueState;
        //    Venue v = vs.Venue;

        //    if (pvs.PasswordHash != null && pvs.PasswordHash.Length > 0)
        //    {
        //        passwords.Add(v.Identifier, pvs.PasswordHash);
        //        v.PWStatus = PasswordStatus.WEAK_PASSWORD;
        //    }
        //    else
        //    {
        //        passwords.Remove(v.Identifier);
        //        v.PWStatus = PasswordStatus.NO_PASSWORD;
        //    }
        //}


        private void SetPasswordStatus(VenueState vs)
        {
            Venue vnu = vs.Venue;
            if (vnu.PWStatus == PasswordStatus.NO_PASSWORD)
            {
                passwords.Remove(vnu.Identifier);
            }
            else
            {
                if (passwords.ContainsKey(vnu.Identifier)) {
                    passwords.Remove(vnu.Identifier);
                }
                passwords.Add(vnu.Identifier, vs.PrivateVenueState.PasswordHash);
            }
        }

        public void AddVenue(VenueState vs)
        {
            this.UpdateFromFile();

            Venue v = vs.Venue;
            venues.Add( v.Identifier, v);

            SetPasswordStatus(vs);
            WriteCacheToFile();
        }


        public void UpdateVenue(VenueState vs)
        {
            this.UpdateFromFile();

            Venue v = vs.Venue;
            venues[v.Identifier] = v;
            SetPasswordStatus(vs);
 
            WriteCacheToFile();
        }

        public void DeleteVenue(string venueIdentifier)
        {
            this.UpdateFromFile();

            venues.Remove(venueIdentifier);
            passwords.Remove(venueIdentifier);

            WriteCacheToFile();
        }

        /// <summary>
        /// Return all state (client-visible and server-private) associated with 
        /// the complete list of venues.  This is used by only by the admin
        /// program, so the results are not obfucated (IP addresses 
        /// are preserved).
        /// </summary>
        /// <returns></returns>
        public VenueState[] GetVenuesComplete()
        {
            this.UpdateFromFile();
            ICollection vals = venues.Values;
            VenueState[] venueArray = new VenueState[vals.Count];
            int i=0;

            foreach (Venue venue in  vals)
            {
                PrivateVenueState pvs = this.GetPrivateVenueState(venue.Identifier);  
                VenueState vs = new VenueState(venue,pvs);
                venueArray[i++] = vs;
            }
            return venueArray;
        }

     

        private PrivateVenueState GetPrivateVenueState(String id)
        {
 	        // assume update from file has already occured...
            byte [] passwordHash;

            if (passwords.TryGetValue(id, out passwordHash))
                return new PrivateVenueState(passwordHash);
            else return new PrivateVenueState();
        }

        /// <summary>
        /// Return all state to the client if the provided password is correct.
        /// 
        /// </summary>
        /// <param name="venueIdentifier"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public Venue GetVenueWithPassword(string venueIdentifier, byte[]  userProvidedPasswordHash)
        {
            this.UpdateFromFile();

            Venue venue = (Venue)venues[venueIdentifier];

            //PasswordHasher hasher = PasswordHasher.getInstance();

            //byte[] userProvidedPasswordHash =    hasher.HashPassword(userProvidedPassword.Trim());
            byte[] truePasswordHash;

            if (passwords.TryGetValue(venueIdentifier, out truePasswordHash))
            {
                if (PasswordsEqual(truePasswordHash,userProvidedPasswordHash))
                    return venue;
                else return null;
            }
            else
            {
                // venue did not have a password in the first place...
                return venue;
            }
        }

        private bool PasswordsEqual(byte[] h1, byte[] h2)
        {
            // I don't know if any builtin way to do array comparison in c#
            if (h1 == null && h2 == null)
                return true;

            if (h1 == null || h2 == null)
                return false;

            if (h1.Length != h2.Length)
                return false;

            for (int i = 0; i < h1.Length; i++)
            {
                if (h1[i] != h2[i])
                    return false;
            }

            return true;
        }
        /// <summary>
        /// Returns the client-visible portion of venue state.
        /// We elide the IP address for password-enabled venues..
        /// </summary>
        /// <returns></returns>
        public Venue GetVenue(string venueIdentifier)    
        {
            this.UpdateFromFile();
            Venue venue = (Venue)venues[venueIdentifier];
            return RemoveSensitiveState(venue);
        }

        /// <summary>
        /// Returns the client-visible portion of venue state.  We elide
        /// the IP address for password-enabled venues.
        /// </summary>
        /// <returns></returns>
        public Venue[] GetVenues()
        {
            this.UpdateFromFile();

            ICollection vals = venues.Values;
            Venue[] venueArray = new Venue[vals.Count];
            int i = 0;

            foreach (Venue venue in vals) 
            {
                //venueArray[i++] = RemoveSensitiveState(venue);
                venueArray[i++] = venue;
            }

            return venueArray;
        }

        /// <summary>
        /// Hide the IP Address for venues with password protection.  Make a new object
        /// so we don't destroy the true state.
        /// </summary>
        /// <param name="venue"></param>
        /// <returns></returns>
        private Venue RemoveSensitiveState(Venue venue)
        {
            if (venue.PWStatus == PasswordStatus.NO_PASSWORD)
                return venue;
            else 
            {
                Venue newVenue = new Venue(venue.Identifier, "0.0.0.0", 1, venue.Name,
                 venue.Icon, venue.AccessList);
                newVenue.PWStatus = venue.PWStatus;
                return newVenue;
            }

        }

        public void AddParticipant(Participant part)
        {
            this.UpdateFromFile();

            participants.Add(part.Identifier, part);

            WriteCacheToFile();
        }

        public void UpdateParticipant(Participant part)
        {
            this.UpdateFromFile();

            participants[part.Identifier] = part;

            WriteCacheToFile();
        }

        public void DeleteParticipant(string participantIdentifier)
        {
            this.UpdateFromFile();

            participants.Remove(participantIdentifier);

            WriteCacheToFile();
        }

        public Participant GetParticipant(string participantIdentifier)
        {
            this.UpdateFromFile();

            return (Participant)participants[participantIdentifier];
        }

        public Participant[] GetParticipants()
        {
            this.UpdateFromFile();

            ICollection vals = participants.Values;
            Participant[] partArray = new Participant[vals.Count];
            vals.CopyTo(partArray, 0);
            return partArray;
        }
        #endregion

        #region Caching
        /// <summary>
        /// If this is true, all write operations are cached in memory & not written to disk
        /// until it's turned off.
        /// </summary>
        public bool WriteCaching
        {
            get
            {
                return this.cacheWriteOps;
            }
            set
            {
                this.cacheWriteOps = value;

                if (!value)
                    this.WriteCacheToFile();
            }
        }
        #endregion

        #region Helper methods
        /// <summary>
        /// Checks that the last time the file was written to has not been updated, and if it has, then
        /// reads the data from the file.
        /// </summary>
        private void UpdateFromFile()
        {
            info.Refresh();
            long latestLength = info.Length;
            if (this.lastLength != latestLength)
            {
                this.lastLength = latestLength;
                Console.WriteLine(string.Format(CultureInfo.CurrentCulture, Strings.UpdatingDataStructures,
                    this.lastLength.ToString(CultureInfo.CurrentCulture)));

                lock(info)
                {
                    file = new FileStream(info.FullName, FileMode.OpenOrCreate);
                    InitializeDataStructures();
                    file.Close();
                }
            }
        }

        /// <summary>
        /// Writes the contents of the cached venues & participants back to the file
        /// </summary>
        private void WriteCacheToFile()
        {
            if (this.cacheWriteOps)
                return;

            lock(info)
            {
                Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "Writing cache to file.  Time: {0}", 
                    DateTime.Now.ToLongTimeString()));

                file = new FileStream(info.FullName, FileMode.OpenOrCreate);
                long startLength = file.Length;
                try
                {

                    file.Position = 0;
                    formatter.Serialize(file, venues);
                    formatter.Serialize(file, participants);
                    formatter.Serialize(file, passwords);

                    file.SetLength(file.Position); // shortens the file, if necessary

                    Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, 
                        "Finished writing cache to file.  Time: {0}", DateTime.Now.ToLongTimeString()));
                }
                finally
                {
                    file.Close();
                }

                info.Refresh();
                long endLength = info.Length;
                this.lastLength = endLength;
                Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, 
                    "Old file size: {0}, New file size: {1}", startLength, endLength));
            }
        }
        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            file.Close();
        }

        #endregion

        public void AddVenue(Venue venue, PrivateVenueState privateVenueState)
        {
            AddVenue(new VenueState(venue, privateVenueState));
        }
    }
}
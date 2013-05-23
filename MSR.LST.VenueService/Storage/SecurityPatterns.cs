using System;
using System.Collections;
using System.Configuration;
using System.Text.RegularExpressions;


namespace MSR.LST.ConferenceXP.VenueService
{
    /// <summary>
    /// Stores regular expressions as security patterns to match participant identifiers to
    /// to determine if the participant is allowed into a particular venue.
    /// </summary>
    /// <remarks>
    /// This is a case-insensitive pattern matcher.
    /// </remarks>
    [Serializable]
    public class SecurityPatterns
    {
        /// <summary>
        /// A regular expression for validating an email address.
        /// one or more words followed by an @, followed by one or more words separated by .
        /// </summary>
        public const string EmailValidator = @"[\w-]+@([\w-]+\.)+[\w-]+";

        // The set of Regexs patterns to match to
        private ArrayList patterns = null;

        public SecurityPatterns()
        {
            patterns = new ArrayList();
        }

        public SecurityPatterns(String[] venueSecurityPatterns)
        {
            // Create the array of Regexs
            patterns = new ArrayList();

            foreach( string strPattern in venueSecurityPatterns )
            {
                AddPattern(strPattern);
            }
        }

        /// <summary>
        /// Returns the set of strings that make up the patterns in the set of security patterns
        /// </summary>
        /// <remarks>
        /// The returned patterns may not reflect the same case for letters in the pattern.
        /// </remarks>
        public string[] Patterns
        {
            get
            {
                // Copy the regexs back into an array of strings
                //  (note this does an effective clone on the strings, which is why we don't bother storing
                //   the strings provided to us in the constructor)

                // We make use of ability to implicitly cast a Regex to a string here
                String[] strPatterns = new string[patterns.Count];
                for (int cnt = 0; cnt < patterns.Count; ++cnt)
                {
                    strPatterns[cnt] = ((Regex)patterns[cnt]).ToString();
                }

                return strPatterns;
            }
        }

        /// <summary>
        /// Returns true if none of the patterns match the participant identifier.  If there are
        /// no patterns to match, the participant is considered a match (returns true).
        /// </summary>
        public bool IsMatch(string participantID)
        {
            if (patterns.Count == 0)
                return true;

            // Try each pattern as a match
            foreach( Regex pattern in patterns )
            {
                if( pattern.IsMatch(participantID) )
                    return true;
            }

            // No matches.  Return false.
            return false;
        }

        public void AddPattern(String venueSecurityPattern)
        {
            patterns.Add( new Regex(venueSecurityPattern, RegexOptions.IgnoreCase) );
        }

        public void RemovePattern(String venueSecurityPattern)
        {
            for (int cnt = 0; cnt < patterns.Count; ++cnt)
            {
                Regex expression = (Regex)patterns[cnt];
                if (expression.ToString().Equals(venueSecurityPattern))
                {
                    patterns.RemoveAt(cnt);
                    return;
                }
            }
        }
    }
}
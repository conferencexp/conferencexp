using System;
using System.Collections.Specialized;
using System.Text.RegularExpressions;


namespace MSR.LST
{
    /// <summary>
    /// Parses command-line arguments into a nice string dictionary to make it easier to work with.
    /// </summary>
    public class ArgumentParser
    {
        private StringDictionary arguments;

        public ArgumentParser(string[] args)
        {
            arguments = new StringDictionary();

            // Don't break on colons (:) as they are used in IPv6 addresses
            Regex splitter = new Regex(@"^-{1,2}|^/|=",RegexOptions.IgnoreCase|RegexOptions.Compiled);
            Regex remover = new Regex(@"^['""]?(.*?)['""]?$",RegexOptions.IgnoreCase|RegexOptions.Compiled);
            string parameter = null;
            string[] parts;

            // Valid parameters forms:
            // {-,/,--}param{ ,=,:}((",')value(",'))
            // Examples: -param1 value1 --param2 /param3:"Test-:-work" /param4=happy -param5 '--=nice=--'
            foreach(string arg in args)
            {
                // Look for new parameters (-,/ or --) and a possible enclosed value (=,:)
                parts = splitter.Split(arg,3);
                switch(parts.Length)
                {
                        // Found a value (for the last parameter found (space separator))
                    case 1:
                        if(parameter!=null)
                        {
                            if(!arguments.ContainsKey(parameter))
                            {
                                parts[0]=remover.Replace(parts[0],"$1");
                                arguments.Add(parameter,parts[0]);
                            }
                            parameter=null;
                        }
                        // else Error: no parameter waiting for a value (skipped)
                        break;
                        // Found just a parameter
                    case 2:
                        // The last parameter is still waiting. With no value, set it to true.
                        if(parameter!=null)
                        {
                            if(!arguments.ContainsKey(parameter)) arguments.Add(parameter,"true");
                        }
                        parameter=parts[1];
                        break;
                        // parameter with enclosed value
                    case 3:
                        // The last parameter is still waiting. With no value, set it to true.
                        if(parameter!=null)
                        {
                            if(!arguments.ContainsKey(parameter)) arguments.Add(parameter,"true");
                        }
                        parameter=parts[1];
                        // Remove possible enclosing characters (",')
                        if(!arguments.ContainsKey(parameter))
                        {
                            parts[2]=remover.Replace(parts[2],"$1");
                            arguments.Add(parameter,parts[2]);
                        }
                        parameter=null;
                        break;
                }
            }
            // In case a parameter is still waiting
            if(parameter!=null)
            {
                if(!arguments.ContainsKey(parameter)) arguments.Add(parameter,"true");
            }
        }

        public StringDictionary Parameters
        {
            get
            {
                return arguments;
            }
        }

    }
}

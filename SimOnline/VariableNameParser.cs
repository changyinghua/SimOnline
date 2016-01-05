using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace com.acs.sim.online
{
    public class VariableNameParser
    {
        public VariableNameParser()
        {
        }

        public int[] Parse(string nameToParse, out string parsedName)
        {
            int[] dim = null;
            // split block name and variable name
            int pos = nameToParse.IndexOf('[');
            if (pos >= 0)
            {
                parsedName = nameToParse.Substring(0, pos);
                // pattern = any number of arbitrary characters between square brackets.
                var pattern = @"\[(.*?)\]";
                //var name = "am2.Ta[2][0]";
                var matches = Regex.Matches(nameToParse, pattern);

                if (matches.Count > 0)
                {
                    dim = new int[matches.Count];
                    //foreach (Match m in matches)
                    for (int i = 0; i < matches.Count; i++)
                    {
                        //Console.WriteLine(m.Groups[1]);
                        dim[i] = Convert.ToInt32(matches[i].Groups[1].Value);
                    }
                }
            }
            else
            {
                parsedName = nameToParse;
            }
            return dim;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

using com.acs.custom.config;
using com.acs.sim.online;
using System.Runtime.InteropServices;
using System.Threading;

namespace com.acs.sim.online.console
{
    class SimOnlineConsole
    {
        private const int MF_BYCOMMAND = 0x00000000;
        public const int SC_CLOSE = 0xF060;

        [DllImport("user32.dll")]
        public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();

        static string caseFilePath;
        static IList<string> inputTagNames = new List<string>();
        static IList<InputBlockMap> inputBlockMaps = new List<InputBlockMap> ();
        static IList<InputStreamMap> inputStreamMaps = new List<InputStreamMap>();
        static IList<OutputBlockMap> outputBlockMaps = new List<OutputBlockMap>();
        static IList<OutputStreamMap> outputStreamMaps = new List<OutputStreamMap>();
        static IList<string> checkAliveTagNames = new List<string>();

        static void Main(string[] args)
        {
            DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_CLOSE, MF_BYCOMMAND);

            DateTime startTime = DateTime.Now;
            Console.WriteLine("Start time: {0}", startTime);

            GetApplicationSettings();

            try
            {
                SimOnline sol = new SimOnline();
                sol.CaseFilePath = caseFilePath;
                if (!sol.Configure(inputTagNames, inputBlockMaps, inputStreamMaps, outputBlockMaps, outputStreamMaps))
                {
                    return;
                }
                sol.ConfigAliveTag(checkAliveTagNames);
                if (sol.Open())
                {
                    sol.Run();         
                }
                else
                {
                    return;
                }
                Console.WriteLine("Press any key to stop.");
                Console.ReadKey();

                DateTime stopTime = DateTime.Now;
                Console.WriteLine("End time: {0}", stopTime);
                TimeSpan duration = stopTime - startTime;
                Console.WriteLine("Elapse: {0}", duration);
               // new Thread(new ThreadStart(() => sol.ReportShutdown())).Start();
                sol.ReportShutdown();
                Console.WriteLine("Press any key again to exit.");

                //sol.ReportShutdown();
               // sol.Close();            
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.Out.WriteLine("GpsRawFileLoaderConsole exception ({0}", e.Message);
            }
        }

        public static void GetApplicationSettings()
        {
            string appConfigPath = System.AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;

            object val;
            try
            {
                // app settings
                AppSettingsReader ar = new AppSettingsReader();

                // Get the AppSettings collection.

                val = ar.GetValue("CaseFilePath", typeof(string));
                if (val != null)
                {
                    caseFilePath = (string)val;
                }

                // custom configuration section
                TagMapConfigurationSection tagMapConfigSection = (TagMapConfigurationSection)System.Configuration.ConfigurationManager.GetSection("TagMapConfigurationSection");
                TagConfigurationCollection tcc = tagMapConfigSection.TagConfigurations;
                foreach (TagConfigurationElement tce in tcc)
                {
                    inputTagNames.Add(tce.tagname);
                }
                InputBlockMapCollection ibms = tagMapConfigSection.InputBlockMaps;
                foreach (InputBlockMapElement ibme in ibms)
                {
                    InputBlockMap ibm = new InputBlockMap();
                    ibm.BlockVariableName = ibme.blockvariablename;
                    ibm.TagName = ibme.tagname;             
                    inputBlockMaps.Add(ibm);
                }
                InputStreamMapCollection isms = tagMapConfigSection.InputStreamMaps;
                foreach (InputStreamMapElement isme in isms)
                {
                    InputStreamMap ism = new InputStreamMap();
                    ism.StreamName = isme.streamname;
                    ism.TagName1 = isme.tagname1;
                    ism.Property1 = isme.property1;
                    ism.TagName2 = isme.tagname2;
                    ism.Property2 = isme.property2;
                    ism.BalanceMethod = isme.balancemethod;
                    inputStreamMaps.Add(ism);
                }
                OutputBlockMapCollection obms = tagMapConfigSection.OutputBlockMaps;
                foreach (OutputBlockMapElement obme in obms)
                {
                    OutputBlockMap obm = new OutputBlockMap();
                    obm.BlockVariableName = obme.blockvariablename;
                    obm.TagName = obme.tagname;
                    outputBlockMaps.Add(obm);
                }
                OutputStreamMapCollection osms = tagMapConfigSection.OutputStreamMaps;
                foreach (OutputStreamMapElement osme in osms)
                {
                    OutputStreamMap osm = new OutputStreamMap();
                    osm.Property = osme.property;
                    osm.TagName = osme.tagname;
                    outputStreamMaps.Add(osm);
                }
                CheckAliveTagCollection cams = tagMapConfigSection.CheckAliveTags;
                foreach (CheckAliveTagElement cae in cams)
                {
                    checkAliveTagNames.Add(cae.tagname);
                }

            }
            catch (Exception ex)     // file or definition not found
            {
                Console.Out.WriteLine("Fail to load SimOnlineConsole.exe.config ({0})", ex.Message);
            }
        }
    }
}

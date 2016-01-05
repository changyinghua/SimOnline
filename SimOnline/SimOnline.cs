using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Reactive.Linq;

namespace com.acs.sim.online
{
    // Online simulator
    public class SimOnline
    {
        private IRealtimeDataCollector plantDataCollector = new PlantDataCollector();
       // private IRealtimeDataCollector plantDataCollector = new MockDataCollector();
        private ModelExecutor modelExecutor = new ModelExecutor();
        private ResultBuilder resultBuilder = new ResultBuilder();
      //  private IResultBuilder resultBuilder = new MockResultBuilder();
        private string caseFilePath;

        public string CaseFilePath
        {
            set { this.caseFilePath = value; }
        }

        public SimOnline()
        {
           
        }

        public bool Configure(IList<string> inputTagNames, IList<InputBlockMap> ibm, IList<InputStreamMap> ism, IList<OutputBlockMap> obm, IList<OutputStreamMap> osm)
        {
            plantDataCollector.Configure(inputTagNames);
            modelExecutor.Configure(ibm, ism, obm, osm);
            resultBuilder.Configure(modelExecutor.OutputValues);

            return true;
        }

        public bool Open()
        {
            if (!plantDataCollector.Open())
            {
                return false;
            }
            else
            {
                 resultBuilder.OpcClient = plantDataCollector.OpcClient;
            }

            if (!modelExecutor.Open(this.caseFilePath))
                return false;

            if (!resultBuilder.Open())
                return false;

            return true;
        }

        public void Close()
        {
            resultBuilder.Close();
            modelExecutor.Close();
            plantDataCollector.Close();
        }

        // online simulation main work flow
        public void Run()
        {
            // React to timer event in 30 seconds interval, with 5 seconds delay
            Observable.Timer(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(30)).Subscribe(x =>
            {
                DoWork(x);
            });
        }

        public void DoWork(long x)
        {
            // read tag values via OPC
            plantDataCollector.ReadTagValues();

            // map tag values to model variables
            modelExecutor.MapModelInputs(plantDataCollector.RawData);

            // execute model
            modelExecutor.Run();

            // build model outputs
            resultBuilder.WriteTagValues();

        }
    }
}

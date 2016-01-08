using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using log4net;

using AHU.Model.Flowsheet;

namespace com.acs.sim.online
{
    public class ModelExecutor
    {
        #region attributes
        private Flowsheet flowsheet = new Flowsheet();
        IList<InputBlockMap> inputBlockMaps;
        IList<OutputBlockMap> outputBlockMaps;
        IList<InputStreamMap> inputStreamMaps;
        IList<OutputStreamMap> outputStreamMaps;
        IDictionary<string, double> outputValues = new Dictionary<string, double>();


        // run time
        TimeSpan elapseTime = new TimeSpan();

        // logging facility
       // private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public readonly ILog logger = LogManager.GetLogger(typeof(ModelExecutor));
        #endregion

        #region properties
        // simulation run time
        public TimeSpan Runtime
        {
            get { return this.elapseTime; }
        }

        public IDictionary<string, double> OutputValues
        {
            get { return this.outputValues; }
        }

        #endregion

        #region ctor
        public ModelExecutor()
        {
        }
        #endregion

        #region public methods
        public bool Configure(IList<InputBlockMap> ibm, IList<InputStreamMap> ism, IList<OutputBlockMap> obm, IList<OutputStreamMap> osm)
        {
            // input block variables maps
            this.inputBlockMaps = ibm;

            // input stream maps
            this.inputStreamMaps = ism;

            // output block variables maps
            this.outputBlockMaps = obm;

            // output stream maps
            this.outputStreamMaps = osm;

            // set up output tag names
            foreach (OutputBlockMap map in obm)
            {
                this.outputValues.Add(map.TagName, 0.0);
            }
            foreach (OutputStreamMap map in osm)
            {
                this.outputValues.Add(map.TagName, 0.0);
            }

            return true;
        }

        public bool Open(string casePath)
        {
            if (!this.flowsheet.Load(casePath))
            {
                logger.Debug("flowsheet load error");
                return false;
            }
            return true;
        }

        public void Close()
        {
        }

        public void MapModelInputs(IDictionary<string, double> inputValues)
        {
            MapInputBlockVariables(inputValues);
            MapInputStreams(inputValues);
        }

        // map input tag values to block variables
        private void MapInputBlockVariables(IDictionary<string, double> inputValues)
        {
            foreach (InputBlockMap map in inputBlockMaps)
            {
                // get tag value
                double v;
                if (inputValues.TryGetValue(map.BlockVariableName, out v))
                {
                    //sim_fs.SetBlockVariable("am3.motor_freq", DB_motor_freq);
                    this.flowsheet.SetBlockVariable(map.TagName, v);
                }
            }
        }

        // map input tag values to stream properties
        private void MapInputStreams(IDictionary<string, double> inputValues)
        {
            foreach (InputStreamMap map in inputStreamMaps)
            {
                // get tag value
                double pv1;
                double pv2;
                // sim_fs.SetStreamVariable("OA.pt", DB_OA_pt);
                // sim_fs.SetStreamVariable("OA.rhp", DB_OA_rhp);
                // sim_fs.SetStreamProperties("OA");
                if (inputValues.TryGetValue(map.TagName1, out pv1))
                {
                    //sim_fs.SetBlockVariable("am3.motor_freq", DB_motor_freq);
                    if (inputValues.TryGetValue(map.TagName2, out pv2))
                    {
                        this.flowsheet.SetStreamVariable(map.Property1, pv1);
                        this.flowsheet.SetStreamVariable(map.Property2, pv2);
                        this.flowsheet.SetStreamProperties(map.StreamName);
                    }
                }

            }
        }

        public void Run()
        {
            try
            {
                DateTime stepStartTime = DateTime.Now;

                // execute model
                this.flowsheet.Execute();

                // map model results
                MapModelResults();

                // get elapse time             
                DateTime curTime = DateTime.Now;
                this.elapseTime = curTime - stepStartTime;

            }
            catch (Exception ex)
            {
                Console.Out.WriteLine("ModelExecutor.Run exception ({0})", ex.Message);
                return;
            }
        }

        public void MapModelResults()
        {
            //object TA_val;
            //sim_fs.GetBlockVariable("am2.Ta", out TA_val);
            //double[] TA_out = (double[])TA_val;
            //
            //OPC.Write("FIX.TA0.F_CV", TA_out[0]);
            //OPC.Write("FIX.TA1.F_CV", TA_out[1]);
            //OPC.Write("FIX.TA2.F_CV", TA_out[2]);
            //OPC.Write("FIX.TA3.F_CV", TA_out[3]);
            //OPC.Write("FIX.TA4.F_CV", TA_out[4]);
            //OPC.Write("FIX.TA5.F_CV", TA_out[5]);
            //OPC.Write("FIX.TA6.F_CV", TA_out[6]);
            //OPC.Write("FIX.TA7.F_CV", TA_out[7]);
            //OPC.Write("FIX.TA8.F_CV", TA_out[8]);
            //
            //object WA_val;
            //sim_fs.GetBlockVariable("am2.Wa", out WA_val);
            //double[] WA_out = (double[])WA_val;
            //OPC.Write("FIX.WA0.F_CV", WA_out[0]);
            //OPC.Write("FIX.WA1.F_CV", WA_out[1]);
            //OPC.Write("FIX.WA2.F_CV", WA_out[2]);
            //OPC.Write("FIX.WA3.F_CV", WA_out[3]);
            //OPC.Write("FIX.WA4.F_CV", WA_out[4]);
            //OPC.Write("FIX.WA5.F_CV", WA_out[5]);
            //OPC.Write("FIX.WA6.F_CV", WA_out[6]);
            //OPC.Write("FIX.WA7.F_CV", WA_out[7]);
            //OPC.Write("FIX.WA8.F_CV", WA_out[8]);
            MapOutBlockVariables();

            //object SA_DBT;
            //sim_fs.GetStreamVariable("HMLA.pt", out SA_DBT);
            //OPC.Write("FIX.SA_DBT.F_CV", SA_DBT);
            //
            //object SA_AH;
            //sim_fs.GetStreamVariable("HMLA.w", out SA_AH);
            //OPC.Write("FIX.SA_AH.F_CV", SA_AH);
            MapOutStreams();

        }

        // map block variables to output tags
        private void MapOutBlockVariables()
        {
            VariableNameParser vnp = new VariableNameParser();
            foreach (OutputBlockMap map in outputBlockMaps)
            {
                // parse block variable name
                string parsedName;
                int[] dim = vnp.Parse(map.BlockVariableName, out parsedName);

                // get value from block variable name
                object v;
                this.flowsheet.GetBlockVariable(parsedName, out v);

                // map block variable value to plant tag
                // array variable: am2.Ta[1] <-> FIX.TA1.F_CV
                if (dim != null)
                {
                    this.outputValues[map.TagName] = (v != null) ? ((double[])v)[dim[0]] : 0.0;
                }
                // no dimension
                else
                {
                    this.outputValues[map.TagName] =  (v != null) ? (double)v : 0.0;
                }
            }
        }

        // map stream properties to output tags
        private void MapOutStreams()
        {
            foreach (OutputStreamMap map in outputStreamMaps)
            {
                //object SA_DBT;
                //sim_fs.GetStreamVariable("HMLA.pt", out SA_DBT);
                //OPC.Write("FIX.SA_DBT.F_CV", SA_DBT);
                //
                //object SA_AH;
                //sim_fs.GetStreamVariable("HMLA.w", out SA_AH);
                //OPC.Write("FIX.SA_AH.F_CV", SA_AH);
                object v;
                if (this.flowsheet.GetStreamVariable(map.Property, out v))
                {
                    this.outputValues[map.TagName] = (double)v;
                }
            }
        }
        #endregion
    }

}

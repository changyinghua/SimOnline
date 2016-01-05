using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace com.acs.sim.online
{
    // map input tag name to block variable
    public class InputBlockMap
    {
        public string TagName {get; set;}
        public string BlockVariableName {get; set;}
    }
    
    // map input tag name to stream property
    public class InputStreamMap
    {
        public string StreamName {get; set;}
        public string TagName1 {get; set;}
        public string Property1 {get; set;}
        public string TagName2 {get; set;}
        public string Property2 {get; set;}
        public string BalanceMethod {get; set;}
    }
    
    // map block variables to output tag name
    public class OutputBlockMap
    {
        public string BlockVariableName {get; set;}
        public string TagName {get; set;}
    }

    // map stream property to output tag name
    public class OutputStreamMap
    {
        public string Property {get; set;}
        public string TagName {get; set;}
    }

}
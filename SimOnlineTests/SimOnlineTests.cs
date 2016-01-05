using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using com.acs.sim.online;
using NUnit.Framework;

namespace com.acs.sim.online.tests
{
    [TestFixture]
    public class SimOnlineTests
    {
        [Test]
        public void TestVariableNameParser()
        {
            VariableNameParser p = new VariableNameParser();
            //p.Parse("am2");
            //p.Parse("am2.Ta");
            string name;
            int[] dim = p.Parse("am2.Ta[2][0]", out name);
        }
    }
}

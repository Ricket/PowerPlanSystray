using Microsoft.VisualStudio.TestTools.UnitTesting;
using PowerPlanSystray;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerPlanSystray.Tests
{
    [TestClass]
    public class PowerPlansTests
    {
        [TestMethod]
        public void ManualIntegrationTest()
        {
            PowerPlans plans = PowerPlans.Instance;
            Assert.AreEqual(3, plans.Plans.Count);
            Assert.AreSame(plans.Plans[1], plans.ActivePlan);
            var expectedPlans = new PowerPlan[]
            {
                new PowerPlan("381b4222-f694-41f0-9685-ff5bb260df2e", "Balanced"),
                //new PowerPlan("6d1902df-e109-4499-a36e-39f6393ca9ae", "My Custom Plan 1"),
                new PowerPlan("8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c", "High performance"),
                new PowerPlan("a1841308-3541-4fab-bc81-f71556f20b4a", "Power saver")
            };
        }
    }
}
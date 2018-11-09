using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Net.Surviveplus.Dump;

namespace Net.Surviveplus.Dump.Test
{
    [TestClass]
    public class DumperTest
    {
        [TestMethod]
        public void Test_IsEnabled_DefaultValueIsLoadedFromAppConfig()
        {
            // In this test set App.config 
            // Net.Surviveplus.Dump.Properties.Settings > DumpIsEnabled = True

            Assert.IsTrue(Dumper.IsEnabled, "Defalut value of IsEnabled property is not loaded from App.config");

        } // end function

        [TestMethod]
        public void Test_IsEnabled_ValueCanBeChangeed()
        {
            Dumper.IsEnabled = true;
            Assert.IsTrue(Dumper.IsEnabled, "Value of IsEnabled property can not be changed. (1 true)");

            Dumper.IsEnabled = false;
            Assert.IsFalse(Dumper.IsEnabled, "Value of IsEnabled property can not be changed. (2 false)");

            Dumper.IsEnabled = true;
            Assert.IsTrue(Dumper.IsEnabled, "Value of IsEnabled property can not be changed. (3 true)");

            Dumper.IsEnabled = false;
            Assert.IsFalse(Dumper.IsEnabled, "Value of IsEnabled property can not be changed. (4 false)");

        } // end function

    } // end class
} // end namespace

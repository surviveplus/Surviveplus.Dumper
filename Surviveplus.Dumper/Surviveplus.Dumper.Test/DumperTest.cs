using System;
using System.Diagnostics;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Net.Surviveplus.Dump;

namespace Net.Surviveplus.Dump.Test
{
    [TestClass]
    public class DumperTest
    {
        #region IsEnabled

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
        #endregion

        #region Folder

        [TestMethod]
        public void Test_Folder_DefaultValueIsLoadedFromAppConfig()
        {
            // In this test set App.config 
            // Net.Surviveplus.Dump.Properties.Settings > DumpFolder = "dump"

            var expected = new DirectoryInfo("dump").FullName;
            Debug.WriteLine($"expected: {expected}");

            Assert.AreEqual(expected, Dumper.Folder.FullName, "Default value of Folder property is not loaded from App.config");

        } // end function

        [TestMethod]
        public void Test_Folder_ValueCanBeChanged()
        {
            var expected = @"c:\dump";
            Debug.WriteLine($"expected: {expected}");

            Dumper.Folder = new DirectoryInfo(expected);
            Assert.AreEqual(expected, Dumper.Folder.FullName, "Value of Folder property can not be changed. (1 DirectoryInfo)");

            Dumper.Folder = null;
            Assert.IsNull(Dumper.Folder, "Value of Folder property can not be changed. (2 null)");

        } // end function

        #endregion

    } // end class
} // end namespace

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
        public TestContext TestContext { get; set;  }

        #region IsEnabled

        [TestMethod]
        public void TestProperty_IsEnabled_DefaultValueIsLoadedFromAppConfig()
        {
            // In this test set App.config 
            // Net.Surviveplus.Dump.Properties.Settings > DumpIsEnabled = True

            Assert.IsTrue(Dumper.IsEnabled, "Defalut value of IsEnabled property is not loaded from App.config");

        } // end function

        [TestMethod]
        public void TestProperty_IsEnabled_ValueCanBeChangeed()
        {
            Dumper.IsEnabled = true;
            Assert.IsTrue(Dumper.IsEnabled, "Value of IsEnabled property can not be changed. (1 true)");

            Dumper.IsEnabled = false;
            Assert.IsFalse(Dumper.IsEnabled, "Value of IsEnabled property can not be changed. (2 false)");

            Dumper.IsEnabled = true;
            Assert.IsTrue(Dumper.IsEnabled, "Value of IsEnabled property can not be changed. (3 true)");

            Dumper.IsEnabled = false;
            Assert.IsFalse(Dumper.IsEnabled, "Value of IsEnabled property can not be changed. (4 false)");

            Dumper.IsEnabled = true;

        } // end function
        #endregion

        #region Folder

        [TestMethod]
        public void TestProperty_Folder_DefaultValueIsLoadedFromAppConfig()
        {
            // In this test set App.config 
            // Net.Surviveplus.Dump.Properties.Settings > DumpFolder = "dump"

            var expected = new DirectoryInfo("dump").FullName;
            Debug.WriteLine($"expected: {expected}");

            Assert.AreEqual(expected, Dumper.Folder.FullName, "Default value of Folder property is not loaded from App.config");

        } // end function

        [TestMethod]
        public void TestProperty_Folder_ValueCanBeChanged()
        {
            var expected = @"c:\dump";
            Debug.WriteLine($"expected: {expected}");

            Dumper.Folder = new DirectoryInfo(expected);
            Assert.AreEqual(expected, Dumper.Folder.FullName, "Value of Folder property can not be changed. (1 DirectoryInfo)");

            Dumper.Folder = null;
            Assert.IsNull(Dumper.Folder, "Value of Folder property can not be changed. (2 null)");

        } // end function

        #endregion

        #region GetDumpFile

        [TestMethod]
        public void TestMethod_GetDumpFile_FolderIsCreated()
        {
            Dumper.IsEnabled = true;
            Dumper.Folder = new DirectoryInfo(Path.Combine( this.TestContext.TestRunResultsDirectory, "dump1"));
            Debug.WriteLine($"Dumper.Folder: {Dumper.Folder}");

            var name = "TestMethod_GetDumpFile_FolderIsCreated";
            var f = Dumper.GetDumpFile(name);
            Debug.WriteLine($"GetDumpFile.FullName: {f.FullName}");
            Assert.IsTrue(f.Directory.Exists, "Folder was not created.");

            // Extra tests
            Assert.IsTrue(f.Name.Contains(name), "Name was not used for file name.");
            Assert.AreEqual(".txt", f.Extension, "Default extension is not \".txt\".");
        } // end function

        [TestMethod]
        public void TestMethod_GetDumpFile_FolderIsNotCreated_IsEnabledFalse()
        {
            Dumper.IsEnabled = false;
            Dumper.Folder = new DirectoryInfo(Path.Combine(this.TestContext.TestRunResultsDirectory, "dump2"));
            Debug.WriteLine($"Dumper.Folder: {Dumper.Folder}");

            var name = "TestMethod_GetDumpFile_FolderIsCreated";
            var f = Dumper.GetDumpFile(name);
            Debug.WriteLine($"GetDumpFile.FullName: {f.FullName}");
            Assert.IsFalse(f.Directory.Exists, "Folder was created, even though IsEnabled property is false.");
        } // end function
        #endregion

        #region WriteFile

        [TestMethod]
        public void TestMethod_WriteTextFile_ContentIsWritten()
        {
            Dumper.IsEnabled = true;
            Dumper.Folder = new DirectoryInfo(Path.Combine(this.TestContext.TestRunResultsDirectory, "dump"));
            Debug.WriteLine($"Dumper.Folder: {Dumper.Folder}");

            var expected = "SAMPLE";
            var name = "a";
            Dumper.WriteTextFile(name, ".txt", writer => {
                writer.Write(expected);
            });

            var f = Dumper.GetDumpFile(name, ".txt");
            Assert.IsTrue(f.Exists, "Dump file was not created.");

            //this.TestContext.AddResultFile(f.FullName);

            var actual = File.ReadAllText(f.FullName);
            Assert.AreEqual(expected, actual, "The content of the dump file is not text that is written.");

        } // end function

        [TestMethod]
        public void TestMethod_WriteTextFile_ContentIsWritten2Japanese()
        {
            Dumper.IsEnabled = true;
            Dumper.Folder = new DirectoryInfo(Path.Combine(this.TestContext.TestRunResultsDirectory, "dump"));
            Debug.WriteLine($"Dumper.Folder: {Dumper.Folder}");

            var expected = "Software\nソフトウェア";
            var name = "b";
            Dumper.WriteTextFile(name, ".txt", writer => {
                writer.Write(expected);
            });

            var f = Dumper.GetDumpFile(name, ".txt");
            Assert.IsTrue(f.Exists, "Dump file was not created.");

            //this.TestContext.AddResultFile(f.FullName);

            var actual = File.ReadAllText(f.FullName);
            Assert.AreEqual(expected, actual, "The content of the dump file is not text that is written.");

        } // end function

        #endregion

        #region DumpJson

        [TestMethod]
        public void TestMethod_DumpJson_anonymous()
        {
            Dumper.IsEnabled = true;
            Dumper.Folder = new DirectoryInfo(Path.Combine(this.TestContext.TestRunResultsDirectory, "dump"));
            Debug.WriteLine($"Dumper.Folder: {Dumper.Folder}");

            var sample = new { A = 10, B = true, C = "Sample" };
            var expected = sample.ToJson();
            Debug.WriteLine($"expected: {expected}");

            var name = "anonymous1";
            Dumper.DumpJson(sample, name, a => a);

            var f = Dumper.GetDumpFile(name, ".json");
            Debug.WriteLine(f);
            Assert.IsTrue(f.Exists, "Dump file was not created.");

            var actual = File.ReadAllText(f.FullName);
            Assert.AreEqual(expected, actual);
        } // end function
        #endregion

    } // end class
} // end namespace

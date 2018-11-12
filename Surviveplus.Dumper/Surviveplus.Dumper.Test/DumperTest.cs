using System;
using System.Diagnostics;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Net.Surviveplus.Dump;
using System.Linq;
using System.Diagnostics.CodeAnalysis;

namespace Net.Surviveplus.Dump.Test
{
    [TestClass, ExcludeFromCodeCoverage]
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

            var name = this.TestContext.TestName;
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

            var name = this.TestContext.TestName;
            var f = Dumper.GetDumpFile(name);
            Debug.WriteLine($"GetDumpFile.FullName: {f.FullName}");
            Assert.IsFalse(f.Directory.Exists, "Folder was created, even though IsEnabled property is false.");
        } // end function
        #endregion

        #region WriteFile

        [TestMethod]
        public void TestMethod_WriteTextFile_DoNotWrite_IsEnabledIsFalse()
        {
            Dumper.IsEnabled = false;
            var name = this.TestContext.TestName;
            Dumper.WriteTextFile(name, ".txt", writer =>
            {
                writer.Write("SAMPLE");
                Assert.Fail("Dump file was wrote, even though IsEnabled property is false.");
            });

            var f = Dumper.GetDumpFile(name, ".txt");
            Assert.IsFalse(f.Exists, "Dump file was created, even though IsEnabled property is false.");

        } // end sub

        [TestMethod]
        public void TestMethod_WriteTextFile_DoNotWrite_FolderIsNull()
        {
            Dumper.IsEnabled = true;
            Dumper.Folder = null;
            var name = this.TestContext.TestName;
            Dumper.WriteTextFile(name, ".txt", writer =>
            {
                writer.Write("SAMPLE");
                Assert.Fail("Dump file was wrote, even though Folder property is false.");
            });

        } // end sub

        [TestMethod]
        public void TestMethod_WriteTextFile_DoNotWrite_ActionIsNull()
        {
            Dumper.IsEnabled = true;
            Dumper.Folder = new DirectoryInfo(Path.Combine(this.TestContext.TestRunResultsDirectory, "dump"));
            var name = this.TestContext.TestName;
            Dumper.WriteTextFile(name, ".txt", null);

            var f = Dumper.GetDumpFile(name, ".txt");
            Assert.IsFalse(f.Exists, "Dump file was created, even though action is null.");

        } // end sub

        [TestMethod]
        public void TestMethod_WriteTextFile_ContentIsWritten()
        {
            Dumper.IsEnabled = true;
            Dumper.Folder = new DirectoryInfo(Path.Combine(this.TestContext.TestRunResultsDirectory, "dump"));
            Debug.WriteLine($"Dumper.Folder: {Dumper.Folder}");

            var expected = "SAMPLE";
            var name = this.TestContext.TestName;
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
            var name = this.TestContext.TestName;
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

            var name = this.TestContext.TestName;
            Dumper.DumpJson(sample, name, a => a);

            var f = Dumper.GetDumpFile(name, ".json");
            Debug.WriteLine($"file: {f}");
            Assert.IsTrue(f.Exists, "Dump file was not created.");

            var actual = File.ReadAllText(f.FullName);
            Debug.WriteLine(actual);
            Assert.AreEqual(expected, actual, "The content of the dump file is not text that is expected.");
        } // end function


        [TestMethod]
        public void TestMethod_DumpJson_anonymousWithFormat()
        {
            Dumper.IsEnabled = true;
            Dumper.Folder = new DirectoryInfo(Path.Combine(this.TestContext.TestRunResultsDirectory, "dump"));
            Debug.WriteLine($"Dumper.Folder: {Dumper.Folder}");

            var sample = new { A = 10, B = true, C = "Sample" };
            var sampleFormatted = new { A = 10, C = "Sample" };
            var expected = sampleFormatted.ToJson();

            var name = this.TestContext.TestName;
            Dumper.DumpJson(sample, name, a => new { a.A, a.C } );

            var f = Dumper.GetDumpFile(name, ".json");
            Debug.WriteLine($"file: {f}");
            Assert.IsTrue(f.Exists, "Dump file was not created.");

            var actual = File.ReadAllText(f.FullName);
            Debug.WriteLine(actual);
            Assert.AreEqual(expected, actual, "The content of the dump file is not text that is expected.");
        } // end function

        #endregion

        #region DumpTsvHeader & DumpTsvRecord

        [TestMethod]
        public void TestMethods_DumpTsvHeader_DumpTsvRecord_anonymous()
        {
            Dumper.IsEnabled = true;
            Dumper.Folder = new DirectoryInfo(Path.Combine(this.TestContext.TestRunResultsDirectory, "dump"));
            Debug.WriteLine($"Dumper.Folder: {Dumper.Folder}");

            var sample =
                from i in new int[] { 1, 2, 3 }
                select new { A = i, B = true, C = "Sample" + i.ToString() };
            var name = this.TestContext.TestName;
            var expected = "A\tB\tC\r\n" + "1\tTrue\tSample1\r\n" + "2\tTrue\tSample2\r\n" + "3\tTrue\tSample3\r\n";

            Dumper.DumpTsvHeader(sample.FirstOrDefault(), name, a=>a);
            foreach (var item in sample)
            {
                Dumper.DumpTsvRecord(item, name, a => a);
            } // next item

            var f = Dumper.GetDumpFile(name, ".tsv");
            Debug.WriteLine(f);
            Assert.IsTrue(f.Exists, "Dump file was not created.");
            var actual = File.ReadAllText(f.FullName);

            Debug.WriteLine(actual);
            Assert.AreEqual(expected, actual, "The content of the dump file is not text that is expected.");

        } // end function
        #endregion

        #region DumpTsv


        [TestMethod]
        public void TestMethod_DumpTsv_string()
        {
            Dumper.IsEnabled = true;
            Dumper.Folder = new DirectoryInfo(Path.Combine(this.TestContext.TestRunResultsDirectory, "dump"));
            Debug.WriteLine($"Dumper.Folder: {Dumper.Folder}");

            var sample =
                from i in new int[] { 1, 2, 3 }
                select "Sample" + i.ToString();
            var name = this.TestContext.TestName;
            var expected = "string\r\n" + "Sample1\r\n" + "Sample2\r\n" + "Sample3\r\n";

            Dumper.DumpTsv(sample, name, a => a);

            var f = Dumper.GetDumpFile(name, ".tsv");
            Debug.WriteLine(f);
            Assert.IsTrue(f.Exists, "Dump file was not created.");
            var actual = File.ReadAllText(f.FullName);

            Debug.WriteLine(actual);
            Assert.AreEqual(expected, actual, "The content of the dump file is not text that is expected.");
        } // end sub

        [TestMethod]
        public void TestMethod_DumpTsv_anonymous()
        {
            Dumper.IsEnabled = true;
            Dumper.Folder = new DirectoryInfo(Path.Combine(this.TestContext.TestRunResultsDirectory, "dump"));
            Debug.WriteLine($"Dumper.Folder: {Dumper.Folder}");

            var sample =
                from i in new int[] { 1, 2, 3 }
                select new { A = i, B = true, C = "Sample" + i.ToString() };
            var name = this.TestContext.TestName;
            var expected = "A\tB\tC\r\n" + "1\tTrue\tSample1\r\n" + "2\tTrue\tSample2\r\n" + "3\tTrue\tSample3\r\n";

            Dumper.DumpTsv(sample, name, a => a);

            var f = Dumper.GetDumpFile(name, ".tsv");
            Debug.WriteLine(f);
            Assert.IsTrue(f.Exists, "Dump file was not created.");
            var actual = File.ReadAllText(f.FullName);

            Debug.WriteLine(actual);
            Assert.AreEqual(expected, actual, "The content of the dump file is not text that is expected.");
        } // end sub


        [TestMethod]
        public void TestMethod_DumpTsv_anonymousNoHeader()
        {
            Dumper.IsEnabled = true;
            Dumper.Folder = new DirectoryInfo(Path.Combine(this.TestContext.TestRunResultsDirectory, "dump"));
            Debug.WriteLine($"Dumper.Folder: {Dumper.Folder}");

            var sample =
                from i in new int[] { 1, 2, 3 }
                select new { A = i, B = true, C = "Sample" + i.ToString() };
            var name = this.TestContext.TestName;
            var expected =  "1\tTrue\tSample1\r\n" + "2\tTrue\tSample2\r\n" + "3\tTrue\tSample3\r\n";

            Dumper.DumpTsv(sample, name, a => a, false);

            var f = Dumper.GetDumpFile(name, ".tsv");
            Debug.WriteLine(f);
            Assert.IsTrue(f.Exists, "Dump file was not created.");
            var actual = File.ReadAllText(f.FullName);

            Debug.WriteLine(actual);
            Assert.AreEqual(expected, actual, "The content of the dump file is not text that is expected.");
        } // end sub

        [TestMethod]
        public void TestMethod_DumpTsv_anonymousWithFormat()
        {
            Dumper.IsEnabled = true;
            Dumper.Folder = new DirectoryInfo(Path.Combine(this.TestContext.TestRunResultsDirectory, "dump"));
            Debug.WriteLine($"Dumper.Folder: {Dumper.Folder}");

            var sample =
                from i in new int[] { 1, 2, 3 }
                select new { A = i, B = true, C = "Sample" + i.ToString() };
            var name = this.TestContext.TestName;
            var expected = "A\tC\r\n" + "1\tSample1\r\n" + "2\tSample2\r\n" + "3\tSample3\r\n";

            Dumper.DumpTsv(sample, name, a => new { a.A, a.C } );

            var f = Dumper.GetDumpFile(name, ".tsv");
            Debug.WriteLine(f);
            Assert.IsTrue(f.Exists, "Dump file was not created.");
            var actual = File.ReadAllText(f.FullName);

            Debug.WriteLine(actual);
            Assert.AreEqual(expected, actual, "The content of the dump file is not text that is expected.");
        } // end sub


        [TestMethod]
        public void TestMethod_DumpTsv_SampleClassAB()
        {
            Dumper.IsEnabled = true;
            Dumper.Folder = new DirectoryInfo(Path.Combine(this.TestContext.TestRunResultsDirectory, "dump"));
            Debug.WriteLine($"Dumper.Folder: {Dumper.Folder}");

            var sample =
                from i in new int[] { 1, 2, 3 }
                select new SampleClassAB{ A = i, B = true };
            var name = this.TestContext.TestName;
            var expected = "A\tB\r\n" + "1\tTrue\r\n" + "2\tTrue\r\n" + "3\tTrue\r\n";

            Dumper.DumpTsv(sample, name, a => a);

            var f = Dumper.GetDumpFile(name, ".tsv");
            Debug.WriteLine(f);
            Assert.IsTrue(f.Exists, "Dump file was not created.");
            var actual = File.ReadAllText(f.FullName);

            Debug.WriteLine(actual);
            Assert.AreEqual(expected, actual, "The content of the dump file is not text that is expected.");
        } // end sub

        [TestMethod]
        public void TestMethod_DumpTsv_SampleClassABC()
        {
            Dumper.IsEnabled = true;
            Dumper.Folder = new DirectoryInfo(Path.Combine(this.TestContext.TestRunResultsDirectory, "dump"));
            Debug.WriteLine($"Dumper.Folder: {Dumper.Folder}");

            var sample =
                from i in new int[] { 1, 2, 3 }
                select new SampleClassABC{ AB = new SampleClassAB { A = i, B = true }, C = "Sample" + i.ToString() };
            var name = this.TestContext.TestName;
            var expected = "A\tB\tC\r\n" + "1\tTrue\tSample1\r\n" + "2\tTrue\tSample2\r\n" + "3\tTrue\tSample3\r\n";

            Dumper.DumpTsv(sample, name, a => a);

            var f = Dumper.GetDumpFile(name, ".tsv");
            Debug.WriteLine(f);
            Assert.IsTrue(f.Exists, "Dump file was not created.");
            var actual = File.ReadAllText(f.FullName);

            Debug.WriteLine(actual);
            Assert.AreEqual(expected, actual, "The content of the dump file is not text that is expected.");
        } // end sub
        #endregion

        #region WriteTsvHeader & WriteTsvRecord

        [TestMethod]
        public void TestMethods_WriteTsvHeader_WriteTsvRecord_anonymous()
        {
            Dumper.IsEnabled = true;
            Dumper.Folder = new DirectoryInfo(Path.Combine(this.TestContext.TestRunResultsDirectory, "dump"));
            Debug.WriteLine($"Dumper.Folder: {Dumper.Folder}");

            var sample =
                from i in new int[] { 1, 2, 3 }
                select new object[] { i, true, "Sample" + i.ToString() };
            var name = this.TestContext.TestName;
            var expected = "A\tB\tC\r\n" + "1\tTrue\tSample1\r\n" + "2\tTrue\tSample2\r\n" + "3\tTrue\tSample3\r\n";

            Dumper.WriteTsvHeader(name, new string[] { "A", "B", "C" });
            foreach (var item in sample)
            {
                Dumper.WriteTsvRecord(item, name);
            } // next item


            var f = Dumper.GetDumpFile(name, ".tsv");
            Debug.WriteLine(f);
            Assert.IsTrue(f.Exists, "Dump file was not created.");
            var actual = File.ReadAllText(f.FullName);

            Debug.WriteLine(actual);
            Assert.AreEqual(expected, actual, "The content of the dump file is not text that is expected.");

        } // end sub
        #endregion

        #region WriteTsv

        [TestMethod]
        public void TestMethod_WriteTsv_string()
        {
            Dumper.IsEnabled = true;
            Dumper.Folder = new DirectoryInfo(Path.Combine(this.TestContext.TestRunResultsDirectory, "dump"));
            Debug.WriteLine($"Dumper.Folder: {Dumper.Folder}");

            var sample =
                from i in new int[] { 1, 2, 3 }
                select "Sample" + i.ToString();
            var name = this.TestContext.TestName;
            var expected = "string\r\n" + "Sample1\r\n" + "Sample2\r\n" + "Sample3\r\n";

            Dumper.WriteTsv(sample, name, new string[] { "string"} );

            var f = Dumper.GetDumpFile(name, ".tsv");
            Debug.WriteLine(f);
            Assert.IsTrue(f.Exists, "Dump file was not created.");
            var actual = File.ReadAllText(f.FullName);

            Debug.WriteLine(actual);
            Assert.AreEqual(expected, actual, "The content of the dump file is not text that is expected.");
        } // end sub


        [TestMethod]
        public void TestMethod_WriteTsv_anonymous()
        {
            Dumper.IsEnabled = true;
            Dumper.Folder = new DirectoryInfo(Path.Combine(this.TestContext.TestRunResultsDirectory, "dump"));
            Debug.WriteLine($"Dumper.Folder: {Dumper.Folder}");

            var sample =
                from i in new int[] { 1, 2, 3 }
                select new object[] { i, true,"Sample" + i.ToString() };
            var name = this.TestContext.TestName;
            var expected =  "1\tTrue\tSample1\r\n" + "2\tTrue\tSample2\r\n" + "3\tTrue\tSample3\r\n";

            Dumper.WriteTsv(sample, name);


            var f = Dumper.GetDumpFile(name, ".tsv");
            Debug.WriteLine(f);
            Assert.IsTrue(f.Exists, "Dump file was not created.");
            var actual = File.ReadAllText(f.FullName);

            Debug.WriteLine(actual);
            Assert.AreEqual(expected, actual, "The content of the dump file is not text that is expected.");
        } // end sub

        [TestMethod]
        public void TestMethod_WriteTsv_anonymousWithHeader()
        {
            Dumper.IsEnabled = true;
            Dumper.Folder = new DirectoryInfo(Path.Combine(this.TestContext.TestRunResultsDirectory, "dump"));
            Debug.WriteLine($"Dumper.Folder: {Dumper.Folder}");

            var sample =
                from i in new int[] { 1, 2, 3 }
                select new object[] { i, true, "Sample" + i.ToString() };
            var name = this.TestContext.TestName;
            var expected = "A\tB\tC\r\n" + "1\tTrue\tSample1\r\n" + "2\tTrue\tSample2\r\n" + "3\tTrue\tSample3\r\n";

            Dumper.WriteTsv(sample, name, new string[] { "A", "B", "C" });


            var f = Dumper.GetDumpFile(name, ".tsv");
            Debug.WriteLine(f);
            Assert.IsTrue(f.Exists, "Dump file was not created.");
            var actual = File.ReadAllText(f.FullName);

            Debug.WriteLine(actual);
            Assert.AreEqual(expected, actual, "The content of the dump file is not text that is expected.");
        } // end sub
        #endregion

    } // end class
} // end namespace

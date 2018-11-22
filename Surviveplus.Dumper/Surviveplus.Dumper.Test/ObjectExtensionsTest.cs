using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Surviveplus.Dump.Test
{
    [TestClass, ExcludeFromCodeCoverage]
    public class ObjectExtensionsTest
    {
        public TestContext TestContext { get; set; }


        [TestMethod]
        public void TesExtensiontsMethod_ToJson_anonymous()
        {
            var sample = new { A = 10, B = true, C = "Sample" };
            var expected = "{\"A\":10,\"B\":true,\"C\":\"Sample\"}";
            var actual = sample.ToJson();

            Assert.AreEqual(expected, actual, "The result of ToJson is not text that is expected.");
        } // end function


        [TestMethod]
        public void TestExtensionsMethod_DumpJson_anonymousWithFormat()
        {
            Dumper.IsEnabled = true;
            Dumper.Folder = new DirectoryInfo(Path.Combine(this.TestContext.TestRunResultsDirectory, "dump"));
            Debug.WriteLine($"Dumper.Folder: {Dumper.Folder}");

            var sample = new { A = 10, B = true, C = "Sample" };
            var sampleFormatted = new { A = 10, C = "Sample" };
            var expected = sampleFormatted.ToJson();

            var name = this.TestContext.TestName;
            sample.DumpJson(name, a => new { a.A, a.C });

            var f = Dumper.GetDumpFile(name, ".json");
            Debug.WriteLine($"file: {f}");
            Assert.IsTrue(f.Exists, "Dump file was not created.");

            var actual = File.ReadAllText(f.FullName);
            Debug.WriteLine(actual);
            Assert.AreEqual(expected, actual, "The content of the dump file is not text that is expected.");
        } // end function

        [TestMethod]
        public void TestExtensionsMethod_DumpJson_anonymousWithoutFormat()
        {
            Dumper.IsEnabled = true;
            Dumper.Folder = new DirectoryInfo(Path.Combine(this.TestContext.TestRunResultsDirectory, "dump"));
            Debug.WriteLine($"Dumper.Folder: {Dumper.Folder}");

            var sample = new { A = 10, B = true, C = "Sample" };
            var expected = sample.ToJson();

            var name = this.TestContext.TestName;
            sample.DumpJson(name);

            var f = Dumper.GetDumpFile(name, ".json");
            Debug.WriteLine($"file: {f}");
            Assert.IsTrue(f.Exists, "Dump file was not created.");

            var actual = File.ReadAllText(f.FullName);
            Debug.WriteLine(actual);
            Assert.AreEqual(expected, actual, "The content of the dump file is not text that is expected.");
        } // end function

        [TestMethod]
        public void TestExtensionsMethod_DumpJsonIf_anonymousWithFormat()
        {
            Dumper.IsEnabled = true;
            Dumper.Folder = new DirectoryInfo(Path.Combine(this.TestContext.TestRunResultsDirectory, "dump"));
            Debug.WriteLine($"Dumper.Folder: {Dumper.Folder}");

            var sample = new { A = 10, B = true, C = "Sample" };
            var sampleFormatted = new { A = 10, C = "Sample" };
            var expected = sampleFormatted.ToJson();

            var name = this.TestContext.TestName;
            sample.DumpJsonIf(name, a => a.A > 0, a => new { a.A, a.C });

            var f = Dumper.GetDumpFile(name, ".json");
            Debug.WriteLine($"file: {f}");
            Assert.IsTrue(f.Exists, "Dump file was not created.");

            var actual = File.ReadAllText(f.FullName);
            Debug.WriteLine(actual);
            Assert.AreEqual(expected, actual, "The content of the dump file is not text that is expected.");
        } // end function

        [TestMethod]
        public void TestExtensionsMethod_DumpJsonIf_predicateIsFalse()
        {
            Dumper.IsEnabled = true;
            Dumper.Folder = new DirectoryInfo(Path.Combine(this.TestContext.TestRunResultsDirectory, "dump"));
            Debug.WriteLine($"Dumper.Folder: {Dumper.Folder}");

            var sample = new { A = 10, B = true, C = "Sample" };
            var sampleFormatted = new { A = 10, C = "Sample" };

            var name = this.TestContext.TestName;
            sample.DumpJsonIf(name,
                a => a.A == 0,
                a => {
                    Assert.Fail("Action of format was called, even though result of predicate is false.");
                    return new { a.A, a.C };
                });

            var f = Dumper.GetDumpFile(name, ".json");
            Debug.WriteLine($"file: {f}");
            Assert.IsFalse(f.Exists, "Dump file was created, even though result of predicate is false.");

        } // end function


        [TestMethod]
        public void TestExtensionsMethods_DumpTsvHeader_DumpTsvRecord_anonymous()
        {
            Dumper.IsEnabled = true;
            Dumper.Folder = new DirectoryInfo(Path.Combine(this.TestContext.TestRunResultsDirectory, "dump"));
            Debug.WriteLine($"Dumper.Folder: {Dumper.Folder}");

            var sample =
                from i in new int[] { 1, 2, 3 }
                select new { A = i, B = true, C = "Sample" + i.ToString() };
            var name = this.TestContext.TestName;
            var expected = "A\tB\tC\r\n" + "1\tTrue\tSample1\r\n" + "2\tTrue\tSample2\r\n" + "3\tTrue\tSample3\r\n";

            sample.FirstOrDefault().DumpTsvHeader( name, a => a);
            foreach (var item in sample)
            {
                item.DumpTsvRecord( name, a => a);
            } // next item

            var f = Dumper.GetDumpFile(name, ".tsv");
            Debug.WriteLine(f);
            Assert.IsTrue(f.Exists, "Dump file was not created.");
            var actual = File.ReadAllText(f.FullName);

            Debug.WriteLine(actual);
            Assert.AreEqual(expected, actual, "The content of the dump file is not text that is expected.");

        } // end function

        [TestMethod]
        public void TestExtensionsMethods_DumpTsvHeader_DumpTsvRecord_anonymousWithoutFormat()
        {
            Dumper.IsEnabled = true;
            Dumper.Folder = new DirectoryInfo(Path.Combine(this.TestContext.TestRunResultsDirectory, "dump"));
            Debug.WriteLine($"Dumper.Folder: {Dumper.Folder}");

            var sample =
                from i in new int[] { 1, 2, 3 }
                select new { A = i, B = true, C = "Sample" + i.ToString() };
            var name = this.TestContext.TestName;
            var expected = "A\tB\tC\r\n" + "1\tTrue\tSample1\r\n" + "2\tTrue\tSample2\r\n" + "3\tTrue\tSample3\r\n";

            sample.FirstOrDefault().DumpTsvHeader(name);
            foreach (var item in sample)
            {
                item.DumpTsvRecord(name);
            } // next item

            var f = Dumper.GetDumpFile(name, ".tsv");
            Debug.WriteLine(f);
            Assert.IsTrue(f.Exists, "Dump file was not created.");
            var actual = File.ReadAllText(f.FullName);

            Debug.WriteLine(actual);
            Assert.AreEqual(expected, actual, "The content of the dump file is not text that is expected.");

        } // end function

        [TestMethod]
        public void TestExtensionsMethod_DumpTsv_anonymous()
        {
            Dumper.IsEnabled = true;
            Dumper.Folder = new DirectoryInfo(Path.Combine(this.TestContext.TestRunResultsDirectory, "dump"));
            Debug.WriteLine($"Dumper.Folder: {Dumper.Folder}");

            var sample =
                from i in new int[] { 1, 2, 3 }
                select new { A = i, B = true, C = "Sample" + i.ToString() };
            var name = this.TestContext.TestName;
            var expected = "A\tB\tC\r\n" + "1\tTrue\tSample1\r\n" + "2\tTrue\tSample2\r\n" + "3\tTrue\tSample3\r\n";

            sample.DumpTsv(name, a => a);

            var f = Dumper.GetDumpFile(name, ".tsv");
            Debug.WriteLine(f);
            Assert.IsTrue(f.Exists, "Dump file was not created.");
            var actual = File.ReadAllText(f.FullName);

            Debug.WriteLine(actual);
            Assert.AreEqual(expected, actual, "The content of the dump file is not text that is expected.");
        } // end sub

        [TestMethod]
        public void TestExtensionsMethod_DumpTsv_anonymousWithouyFormat()
        {
            Dumper.IsEnabled = true;
            Dumper.Folder = new DirectoryInfo(Path.Combine(this.TestContext.TestRunResultsDirectory, "dump"));
            Debug.WriteLine($"Dumper.Folder: {Dumper.Folder}");

            var sample =
                from i in new int[] { 1, 2, 3 }
                select new { A = i, B = true, C = "Sample" + i.ToString() };
            var name = this.TestContext.TestName;
            var expected = "A\tB\tC\r\n" + "1\tTrue\tSample1\r\n" + "2\tTrue\tSample2\r\n" + "3\tTrue\tSample3\r\n";

            sample.DumpTsv(name);

            var f = Dumper.GetDumpFile(name, ".tsv");
            Debug.WriteLine(f);
            Assert.IsTrue(f.Exists, "Dump file was not created.");
            var actual = File.ReadAllText(f.FullName);

            Debug.WriteLine(actual);
            Assert.AreEqual(expected, actual, "The content of the dump file is not text that is expected.");
        } // end sub


        [TestMethod]
        public void TestExtensionsMethods_WriteTsvHeader_WriteTsvRecord_anonymous()
        {
            Dumper.IsEnabled = true;
            Dumper.Folder = new DirectoryInfo(Path.Combine(this.TestContext.TestRunResultsDirectory, "dump"));
            Debug.WriteLine($"Dumper.Folder: {Dumper.Folder}");

            var sample =
                from i in new int[] { 1, 2, 3 }
                select new object[] { i, true, "Sample" + i.ToString() };
            var name = this.TestContext.TestName;
            var expected = "A\tB\tC\r\n" + "1\tTrue\tSample1\r\n" + "2\tTrue\tSample2\r\n" + "3\tTrue\tSample3\r\n";

            new string[] { "A", "B", "C" }.WriteTsvHeader(name);
            foreach (var item in sample)
            {
                item.WriteTsvRecord(name);
            } // next item


            var f = Dumper.GetDumpFile(name, ".tsv");
            Debug.WriteLine(f);
            Assert.IsTrue(f.Exists, "Dump file was not created.");
            var actual = File.ReadAllText(f.FullName);

            Debug.WriteLine(actual);
            Assert.AreEqual(expected, actual, "The content of the dump file is not text that is expected.");

        } // end sub

        [TestMethod]
        public void TestExtensionsMethod_WriteTsv_anonymous()
        {
            Dumper.IsEnabled = true;
            Dumper.Folder = new DirectoryInfo(Path.Combine(this.TestContext.TestRunResultsDirectory, "dump"));
            Debug.WriteLine($"Dumper.Folder: {Dumper.Folder}");

            var sample =
                from i in new int[] { 1, 2, 3 }
                select new object[] { i, true, "Sample" + i.ToString() };
            var name = this.TestContext.TestName;
            var expected = "1\tTrue\tSample1\r\n" + "2\tTrue\tSample2\r\n" + "3\tTrue\tSample3\r\n";

            sample.WriteTsv(name);


            var f = Dumper.GetDumpFile(name, ".tsv");
            Debug.WriteLine(f);
            Assert.IsTrue(f.Exists, "Dump file was not created.");
            var actual = File.ReadAllText(f.FullName);

            Debug.WriteLine(actual);
            Assert.AreEqual(expected, actual, "The content of the dump file is not text that is expected.");
        } // end sub

    }
}

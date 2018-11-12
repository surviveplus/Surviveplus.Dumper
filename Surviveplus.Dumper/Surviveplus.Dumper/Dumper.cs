using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Surviveplus.Dump
{
    /// <summary>
    /// <para xml:lang="en">Dump properties of instance.</para>
    /// <para xml:lang="ja">インスタンスのプロパティをダンプします。</para>
    /// </summary>
    public class Dumper
    {
        /// <summary>
        /// <para xml:lang="en">Get or set a value that indicates whether dump output is enabled.</para>
        /// <para xml:lang="ja">ダンプ出力が有効かどうかを表す値を取得、または設定します。</para>
        /// </summary>
        public static bool IsEnabled { get; set; } = Properties.Settings.Default.DumpIsEnabled;

        /// <summary>
        /// <para xml:lang="en">Get or set a object that represents the folder in which the dump file is output. </para>
        /// <para xml:lang="ja">ダンプファイルが出力されるフォルダを表すオブジェクトを取得、または設定します。</para>
        /// </summary>
        public static DirectoryInfo Folder { get; set; } = new DirectoryInfo(Properties.Settings.Default.DumpFolder);

        #region Infrastructure Methods  : GetDumpFile, WriteTextFile 

        /// <summary>
        /// <para xml:lang="en">
        /// Get a object that represents the dump file.
        /// Do not use this method. This is used for the infrastructure of the framework.
        /// </para>
        /// <para xml:lang="ja">
        /// ダンプファイルを表すオブジェクトを取得します。
        /// このメソッドを使用しないでください。これはフレームワークの機能のために使用されます。
        /// </para>
        /// </summary>
        /// <param name="name">
        /// <para xml:lang="en">Text to be used for the file name.</para>
        /// <para xml:lang="ja">ファイルの名前に使用されるテキストです。</para>
        /// </param>
        /// <param name="extension">
        /// <para xml:lang="en">Extension to be used for the file name. Default is ".txt".</para>
        /// <para xml:lang="ja">ファイルの名前に使用される拡張子です。既定値は ".txt" です。</para>
        /// </param>
        /// <returns>
        /// <para xml:lang="en">Return a object that represents a file. A folder of the file is created.</para>
        /// <para xml:lang="ja">ファイルを表すオブジェクトを返します。ファイルのフォルダは作成されます。</para>
        /// </returns>
        public static FileInfo GetDumpFile(string name, string extension = ".txt")
        {
            var file = new FileInfo(Path.Combine(
                Dumper.Folder.FullName, 
                name + extension));

            if (Dumper.IsEnabled && file.Directory.Exists == false)
            {
                file.Directory.Create();
            }

            return file;
        } // end function

        /// <summary>
        /// <para xml:lang="en">
        /// Write text to the dump file.
        /// Do not use this method. This is used for the infrastructure of the framework.
        /// </para>
        /// <para xml:lang="ja">
        /// テキストをダンプファイルに書き込みます。
        /// このメソッドを使用しないでください。これはフレームワークの機能のために使用されます。
        /// </para>
        /// </summary>
        /// <param name="name">
        /// <para xml:lang="en">Text to be used for the file name.</para>
        /// <para xml:lang="ja">ファイルの名前に使用されるテキストです。</para>
        /// </param>
        /// <param name="extension">
        /// <para xml:lang="en">Extension to be used for the file name. Default is ".txt".</para>
        /// <para xml:lang="ja">ファイルの名前に使用される拡張子です。既定値は ".txt" です。</para>
        /// </param>
        /// <param name="action">
        /// <para xml:lang="en">
        /// Action to write text to the file.
        /// Write text to the file using the TextWriter argument.
        /// </para>
        /// <para xml:lang="ja">
        /// ファイルを書き込むアクションを指定します。
        /// 引数のTextWriterを使用してテキストをファイルに書き込みます。
        /// </para>
        /// </param>
        public static void WriteTextFile( string name, string extension , Action<TextWriter> action )
        {
            if (Dumper.IsEnabled == false) return;
            if (Dumper.Folder == null) return;
            if (action == null) return;

            var file = Dumper.GetDumpFile(name, extension).FullName;
            using (var stream = new FileStream(file, FileMode.Append,  FileAccess.Write))
            using (var writer = new StreamWriter(stream, Encoding.UTF8))
            {
                action(writer);
            } // end using (stream, writer)

        } // end sub
        #endregion


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <param name="name"></param>
        /// <param name="format"></param>
        public static void DumpJson<T>(T target, string name, Func<T, object> format)
        {
            Dumper.WriteTextFile(name, ".json" , writer =>
            {
                writer.Write( format(target).ToJson() );
            });
        } // end sub

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <param name="name"></param>
        /// <param name="format"></param>
        public static void DumpTsvHeader<T>(T target, string name, Func<T, object> format)
        {
            Dumper.WriteTextFile(name, ".tsv", writer =>
            {
                var configuration = new CsvHelper.Configuration.Configuration { Delimiter = "\t" };
                using (var tsv = new CsvHelper.CsvWriter(writer, configuration))
                {
                    var v = format(target);
                    tsv.WriteHeader(v.GetType());
                    tsv.NextRecord();
                } // end using (tsv)
            });
        } // end sub

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <param name="name"></param>
        /// <param name="format"></param>
        /// <param name="writeHeader"></param>
        public static void DumpTsvRecord<T>(T target, string name, Func<T, object> format, bool writeHeader = false)
        {
            Dumper.DumpTsv(new T[] { target }, name, format, writeHeader);
        } // end sub

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <param name="name"></param>
        /// <param name="format"></param>
        /// <param name="writeHeader"></param>
        public static void DumpTsv<T>(IEnumerable<T> target, string name, Func<T, object> format, bool writeHeader = true)
        {
            Dumper.WriteTextFile(name, ".tsv", writer =>
            {
                var configuration = new CsvHelper.Configuration.Configuration { Delimiter = "\t" };
                using (var tsv = new CsvHelper.CsvWriter(writer, configuration))
                {
                    var isFirst = true;
                    foreach (var item in target)
                    {
                        var v = format(item);
                        if (isFirst) {
                            isFirst = false;
                            if (writeHeader)
                            {
                                if (typeof(T) == typeof(string))
                                {
                                    tsv.WriteField("string");
                                }
                                else
                                {
                                    tsv.WriteHeader(v.GetType());
                                } // end if
                                tsv.NextRecord();
                            }
                        } // end if

                        if (typeof(T) == typeof(string)) {
                            tsv.WriteField(v);
                        }else
                        {
                            tsv.WriteRecord(v);
                        } // end if
                        tsv.NextRecord();

                    } // next item
                } // end using (tsv)
            });
        } // end sub

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="header"></param>
        public static void WriteTsvHeader<T>(string name, IEnumerable<T> header)
        {
            Dumper.WriteTsv<T>(new IEnumerable<T>[] { header }, name);
        } // end sub

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <param name="name"></param>
        public static void WriteTsvRecord<T>(IEnumerable<T> values, string name)
        {
            Dumper.WriteTsv<T>(new IEnumerable<T>[] { values }, name);
        } // end sub

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <param name="name"></param>
        /// <param name="header"></param>
        public static void WriteTsv<T>(IEnumerable<IEnumerable<T>> target, string name, IEnumerable<string>header = null)
        {
            Dumper.WriteTextFile(name, ".tsv", writer =>
            {
                var configuration = new CsvHelper.Configuration.Configuration { Delimiter = "\t" };
                using (var tsv = new CsvHelper.CsvWriter(writer, configuration))
                {

                    if (header != null)
                    {
                        foreach (var c in header)
                        {
                            tsv.WriteField(c);
                        }
                        tsv.NextRecord();
                    } // end if

                    foreach (var item in target)
                    {
                        foreach (var f in item)
                        {
                            tsv.WriteField(f);
                        }
                        tsv.NextRecord();

                    } // next item
                } // end using (tsv)
            });
        } // end sub

    } // end class
} // end namespace

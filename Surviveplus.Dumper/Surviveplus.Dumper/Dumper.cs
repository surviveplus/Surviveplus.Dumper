using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Surviveplus.Dump
{
    /// <summary>
    /// <para xml:lang="en">Dump properties of a object.</para>
    /// <para xml:lang="ja">オブジェクトのプロパティをダンプします。</para>
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
        /// <para xml:lang="en">Dump properties of a object to a JSON file.</para>
        /// <para xml:lang="ja">オブジェクトのプロパティをJSONファイルにダンプします</para>
        /// </summary>
        /// <typeparam name="T">
        /// <para xml:lang="en">The type of object to dump.</para>
        /// <para xml:lang="ja">ダンプするオブジェクトの型</para>
        /// </typeparam>
        /// <param name="target">
        /// <para xml:lang="en">The object to dump.</para>
        /// <para xml:lang="ja">ダンプするオブジェクト</para>
        /// </param>
        /// <param name="name">
        /// <para xml:lang="en">The name which is used for a file.</para>
        /// <para xml:lang="ja">ファイルに使用される名前。</para>
        /// </param>
        /// <param name="format">
        /// <para xml:lang="en">
        /// The lambda expression which is used for choosing properties to dump.
        /// Set like <code>a=&gt;new {a.A, a.B}</code>  to dump A property and B property of the object.
        /// Set like <code>a=&gt;a</code>  to dump all properties of the object.
        /// </para>
        /// <para xml:lang="ja">
        /// ダンプするプロパティを選択するラムダ式。
        /// オブジェクトのAプロパティとBプロパティをダンプする場合には、<code>a=&gt;new {a.A, a.B}</code> のように指定します。
        /// 全てのプロパティをダンプする場合には<code>a=&gt;a</code>を指定します。
        /// </para>
        /// </param>
        public static void DumpJson<T>(T target, string name, Func<T, object> format)
        {
            Dumper.WriteTextFile(name, ".json" , writer =>
            {
                writer.Write( format(target).ToJson() );
            });
        } // end sub

        /// <summary>
        /// <para xml:lang="en">Output the names of properties of a object as header of a TSV file.</para>
        /// <para xml:lang="ja">TSVファイルヘッダとして、オブジェクトのプロパティの名前を出力します。</para>
        /// </summary>
        /// <typeparam name="T">
        /// <para xml:lang="en">The type of object to dump.</para>
        /// <para xml:lang="ja">ダンプするオブジェクトの型</para>
        /// </typeparam>
        /// <param name="target">
        /// <para xml:lang="en">
        /// The object whose properties names are read. However, those properties values are not outputted.
        /// Use DumpTsvRecord method, DumpTsv method or other methods to output values.
        /// </para>
        /// <para xml:lang="ja">
        /// プロパティの名前を読み取るオブジェクト。ただしプロパティの値は出力されません。
        /// 値を出力するには DumpTsvRecord メソッドやDumpTsvメソッドなどを使用します。
        /// </para>
        /// </param>
        /// <param name="name">
        /// <para xml:lang="en">The name which is used for a file.</para>
        /// <para xml:lang="ja">ファイルに使用される名前。</para>
        /// </param>
        /// <param name="format">
        /// <para xml:lang="en">
        /// The lambda expression which is used for choosing properties to dump.
        /// Set like <code>a=&gt;new {a.A, a.B}</code>  to dump A property and B property of the object.
        /// Set like <code>a=&gt;a</code>  to dump all properties of the object.
        /// </para>
        /// <para xml:lang="ja">
        /// ダンプするプロパティを選択するラムダ式。
        /// オブジェクトのAプロパティとBプロパティをダンプする場合には、<code>a=&gt;new {a.A, a.B}</code> のように指定します。
        /// 全てのプロパティをダンプする場合には<code>a=&gt;a</code>を指定します。
        /// </para>
        /// </param>
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
        /// <para xml:lang="en">Dump properties of a object to a TSV file. Output single objects on one row of the TSV file.</para>
        /// <para xml:lang="ja">オブジェクトのプロパティをTSVファイルにダンプします。1のオブジェクトをTSVファイルの1行に出力します。</para>
        /// </summary>
        /// <typeparam name="T">
        /// <para xml:lang="en">The type of object to dump.</para>
        /// <para xml:lang="ja">ダンプするオブジェクトの型</para>
        /// </typeparam>
        /// <param name="target">
        /// <para xml:lang="en">The object to dump.</para>
        /// <para xml:lang="ja">ダンプするオブジェクト</para>
        /// </param>
        /// <param name="name">
        /// <para xml:lang="en">The name which is used for a file.</para>
        /// <para xml:lang="ja">ファイルに使用される名前。</para>
        /// </param>
        /// <param name="format">
        /// <para xml:lang="en">
        /// The lambda expression which is used for choosing properties to dump.
        /// Set like <code>a=&gt;new {a.A, a.B}</code>  to dump A property and B property of the object.
        /// Set like <code>a=&gt;a</code>  to dump all properties of the object.
        /// </para>
        /// <para xml:lang="ja">
        /// ダンプするプロパティを選択するラムダ式。
        /// オブジェクトのAプロパティとBプロパティをダンプする場合には、<code>a=&gt;new {a.A, a.B}</code> のように指定します。
        /// 全てのプロパティをダンプする場合には<code>a=&gt;a</code>を指定します。
        /// </para>
        /// </param>
        /// <param name="writeHeader">
        /// <para xml:lang="en">
        /// Set true to output a header of a TSV file. The names of properties of the object are output.
        /// Set false and so a header is not outputted. Use the DumpTsvHeader method or the WriteTsvHeader method to output the header, separately.
        /// </para>
        /// <para xml:lang="ja">
        /// TSVファイルの1行目にヘッダを出力する場合はtrueを指定します。オブジェクトのプロパティ名が出力されます。
        /// falseを指定するとヘッダが出力されません。別途、DumpTsvHeaderメソッドやWriteTsvHeaderメソッドを使用してヘッダを出力してください。
        /// </para>
        /// </param>
        public static void DumpTsvRecord<T>(T target, string name, Func<T, object> format, bool writeHeader = false)
        {
            Dumper.DumpTsv(new T[] { target }, name, format, writeHeader);
        } // end sub

        /// <summary>
        /// <para xml:lang="en">Dump properties of a object to a TSV file. Output multiple objects on each rows of the TSV file.</para>
        /// <para xml:lang="ja">オブジェクトのプロパティをTSVファイルにダンプします。複数のオブジェクトをTSVファイルの各行に出力します。</para>
        /// </summary>
        /// <typeparam name="T">
        /// <para xml:lang="en">The type of object to dump.</para>
        /// <para xml:lang="ja">ダンプするオブジェクトの型</para>
        /// </typeparam>
        /// <param name="target">
        /// <para xml:lang="en">The collection of objects to dump.</para>
        /// <para xml:lang="ja">ダンプするオブジェクトのコレクション</para>
        /// </param>
        /// <param name="name">
        /// <para xml:lang="en">The name which is used for a file.</para>
        /// <para xml:lang="ja">ファイルに使用される名前。</para>
        /// </param>
        /// <param name="format">
        /// <para xml:lang="en">
        /// The lambda expression which is used for choosing properties to dump.
        /// Set like <code>a=&gt;new {a.A, a.B}</code>  to dump A property and B property of the object.
        /// Set like <code>a=&gt;a</code>  to dump all properties of the object.
        /// </para>
        /// <para xml:lang="ja">
        /// ダンプするプロパティを選択するラムダ式。
        /// オブジェクトのAプロパティとBプロパティをダンプする場合には、<code>a=&gt;new {a.A, a.B}</code> のように指定します。
        /// 全てのプロパティをダンプする場合には<code>a=&gt;a</code>を指定します。
        /// </para>
        /// </param>
        /// <param name="writeHeader">
        /// <para xml:lang="en">
        /// Set true to output a header of a TSV file. The names of properties of the object are output.
        /// Set false and so a header is not outputted. Use the DumpTsvHeader method or the WriteTsvHeader method to output the header, separately.
        /// </para>
        /// <para xml:lang="ja">
        /// TSVファイルの1行目にヘッダを出力する場合はtrueを指定します。オブジェクトのプロパティ名が出力されます。
        /// falseを指定するとヘッダが出力されません。別途、DumpTsvHeaderメソッドやWriteTsvHeaderメソッドを使用してヘッダを出力してください。
        /// </para>
        /// </param>
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
        /// <para xml:lang="en">Output the collection of names as header of a TSV file.</para>
        /// <para xml:lang="ja">TSVファイルヘッダとして、名前のコレクションを出力します。</para>
        /// </summary>
        /// <param name="name">
        /// <para xml:lang="en">The name which is used for a file.</para>
        /// <para xml:lang="ja">ファイルに使用される名前。</para>
        /// </param>
        /// <param name="header">
        /// <para xml:lang="en">A list of names</para>
        /// <para xml:lang="ja">名前の一覧</para>
        /// </param>
        public static void WriteTsvHeader(string name, IEnumerable<string> header)
        {
            Dumper.WriteTsv<string>(new IEnumerable<string>[] { header }, name);
        } // end sub

        /// <summary>
        /// <para xml:lang="en">Output one row of table format values  to a TSV file. Output single row of multiple values on one row of the TSV file.</para>
        /// <para xml:lang="ja">表形式の値の1行をTSVファイルに出力します。複数の値の1行をTSVファイルの1行に出力します。</para>
        /// </summary>
        /// <typeparam name="T">
        /// <para xml:lang="en">The type of object to dump.</para>
        /// <para xml:lang="ja">ダンプするオブジェクトの型</para>
        /// </typeparam>
        /// <param name="values">
        /// <para xml:lang="en">The collection of values to output.Set one row in the table.</para>
        /// <para xml:lang="ja">出力する値のコレクション。表の1行を指定します。</para>
        /// </param>
        /// <param name="name">
        /// <para xml:lang="en">The name which is used for a file.</para>
        /// <para xml:lang="ja">ファイルに使用される名前。</para>
        /// </param>
        public static void WriteTsvRecord<T>(IEnumerable<T> values, string name)
        {
            Dumper.WriteTsv<T>(new IEnumerable<T>[] { values }, name);
        } // end sub

        /// <summary>
        /// <para xml:lang="en">Output table format values  to a TSV file. Output multiple rows of multiple values on each rows of the TSV file.</para>
        /// <para xml:lang="ja">表形式の値をTSVファイルに出力します。複数の値の複数行をTSVファイルの各行に出力します。</para>
        /// </summary>
        /// <typeparam name="T">
        /// <para xml:lang="en">The type of object to dump.</para>
        /// <para xml:lang="ja">ダンプするオブジェクトの型</para>
        /// </typeparam>
        /// <param name="target">
        /// <para xml:lang="en">The collection of table format values to output.</para>
        /// <para xml:lang="ja">出力する表形式の値のコレクション。</para>
        /// </param>
        /// <param name="name">
        /// <para xml:lang="en">The name which is used for a file.</para>
        /// <para xml:lang="ja">ファイルに使用される名前。</para>
        /// </param>
        /// <param name="header">
        /// <para xml:lang="en">
        /// Set a list of names, to output a header of TSV file.
        /// Set null, and so a header is not outputted. Use the DumpTsvHeader method or the WriteTsvHeader method to output the header, separately.
        /// </para>
        /// <para xml:lang="ja">
        /// TSFファイルのヘッダを出力するときには、名前の一覧を指定します。
        /// nullを指定するとヘッダは出力されません。別途、DumpTsvHeaderメソッドやWriteTsvHeaderメソッドを使用してヘッダを出力してください。
        /// </para>
        /// </param>
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
                        if(item.GetType() == typeof(string))
                        {
                            tsv.WriteField(item);
                        }
                        else
                        {
                            foreach (var f in item)
                            {
                                tsv.WriteField(f);
                            }
                        }

                        tsv.NextRecord();

                    } // next item
                } // end using (tsv)
            });
        } // end sub

    } // end class
} // end namespace

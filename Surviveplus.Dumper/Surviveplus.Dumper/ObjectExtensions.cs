using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Surviveplus.Dump
{
    /// <summary>
    /// <para xml:lang="en">Static class which is defined extension methods.</para>
    /// <para xml:lang="ja">拡張メソッドを追加する静的クラスです。</para>
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// <para xml:lang="en">Returns a json string that represents the current object.</para>
        /// <para xml:lang="ja">現在のオブジェクトを表すJSON文字列を返します。</para>
        /// </summary>
        /// <param name="me">
        /// <para xml:lang="en">The instance of the type which is added this extension method.</para>
        /// <para xml:lang="ja">拡張メソッドを追加する型のインスタンスを指定します。</para>
        /// </param>
        /// <returns>
        /// <para xml:lang="en">Returns a json string that represents the current object.</para>
        /// <para xml:lang="ja">現在のオブジェクトを表すJSON文字列を返します。</para>
        /// </returns>
        public static string ToJson(this object me)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(me);
        } // end function

        /// <summary>
        /// <para xml:lang="en">Dump properties of a instance to a JSON file.</para>
        /// <para xml:lang="ja">インスタンスのプロパティをJSONファイルにダンプします</para>
        /// </summary>
        /// <typeparam name="T">
        /// <para xml:lang="en">The type of object to dump.</para>
        /// <para xml:lang="ja">ダンプするオブジェクトの型</para>
        /// </typeparam>
        /// <param name="me">
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
        /// Set null or  like <code>a=&gt;a</code>  to dump all properties of the object.
        /// </para>
        /// <para xml:lang="ja">
        /// ダンプするプロパティを選択するラムダ式。
        /// オブジェクトのAプロパティとBプロパティをダンプする場合には、<code>a=&gt;new {a.A, a.B}</code> のように指定します。
        /// 全てのプロパティをダンプする場合には null、あるいは<code>a=&gt;a</code>を指定します。
        /// </para>
        /// </param>
        public static void DumpJson<T>(this T me, string name, Func<T, object> format = null)
        {
            Dumper.DumpJson(me, name, format ?? (a => a));
        } // end sub

        /// <summary>
        /// <para xml:lang="en">Dump properties of a object to a JSON file.</para>
        /// <para xml:lang="ja">オブジェクトのプロパティをJSONファイルにダンプします</para>
        /// </summary>
        /// <typeparam name="T">
        /// <para xml:lang="en">The type of object to dump.</para>
        /// <para xml:lang="ja">ダンプするオブジェクトの型</para>
        /// </typeparam>
        /// <param name="me">
        /// <para xml:lang="en">The object to dump.</para>
        /// <para xml:lang="ja">ダンプするオブジェクト</para>
        /// </param>
        /// <param name="name">
        /// <para xml:lang="en">The name which is used for a file.</para>
        /// <para xml:lang="ja">ファイルに使用される名前。</para>
        /// </param>
        /// <param name="predicate">
        /// The lambda expression which determines whether to dump.
        /// The file is outputted  when the return value is true. It will not be dumped when false.
        /// <para xml:lang="en">
        /// </para>
        /// <para xml:lang="ja">
        /// ダンプするかどうかを判定するラムダ式。
        /// 戻り値がTrueのときにファイルが出力されます。Falseの時にはダンプされません。
        /// </para>
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
        public static void DumpJsonIf<T>(this T me, string name, Func<T, bool> predicate, Func<T, object> format = null)
        {
            Dumper.DumpJsonIf(me, name, predicate, format ?? (a => a));
        } // end sub

        /// <summary>
        /// <para xml:lang="en">Output the names of properties of a object as header of a TSV file.</para>
        /// <para xml:lang="ja">TSVファイルヘッダとして、オブジェクトのプロパティの名前を出力します。</para>
        /// </summary>
        /// <typeparam name="T">
        /// <para xml:lang="en">The type of object to dump.</para>
        /// <para xml:lang="ja">ダンプするオブジェクトの型</para>
        /// </typeparam>
        /// <param name="me">
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
        /// Set null or like <code>a=&gt;a</code>  to dump all properties of the object.
        /// </para>
        /// <para xml:lang="ja">
        /// ダンプするプロパティを選択するラムダ式。
        /// オブジェクトのAプロパティとBプロパティをダンプする場合には、<code>a=&gt;new {a.A, a.B}</code> のように指定します。
        /// 全てのプロパティをダンプする場合にはnull、あるいは<code>a=&gt;a</code>を指定します。
        /// </para>
        /// </param>
        public static void DumpTsvHeader<T>(this T me, string name, Func<T, object> format = null)
        {
            Dumper.DumpTsvHeader(me, name, format ?? (a => a));
        }

        /// <summary>
        /// <para xml:lang="en">Dump properties of a object to a TSV file. Output single objects on one row of the TSV file.</para>
        /// <para xml:lang="ja">オブジェクトのプロパティをTSVファイルにダンプします。1のオブジェクトをTSVファイルの1行に出力します。</para>
        /// </summary>
        /// <typeparam name="T">
        /// <para xml:lang="en">The type of object to dump.</para>
        /// <para xml:lang="ja">ダンプするオブジェクトの型</para>
        /// </typeparam>
        /// <param name="me">
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
        /// Set null or like <code>a=&gt;a</code>  to dump all properties of the object.
        /// </para>
        /// <para xml:lang="ja">
        /// ダンプするプロパティを選択するラムダ式。
        /// オブジェクトのAプロパティとBプロパティをダンプする場合には、<code>a=&gt;new {a.A, a.B}</code> のように指定します。
        /// 全てのプロパティをダンプする場合にはnull、あるいは<code>a=&gt;a</code>を指定します。
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
        public static void DumpTsvRecord<T>(this T me, string name, Func<T, object> format = null, bool writeHeader = false)
        {
            Dumper.DumpTsvRecord(me, name, format ?? (a => a), writeHeader);
        }

        /// <summary>
        /// <para xml:lang="en">Dump properties of a object to a TSV file. Output multiple objects on each rows of the TSV file.</para>
        /// <para xml:lang="ja">オブジェクトのプロパティをTSVファイルにダンプします。複数のオブジェクトをTSVファイルの各行に出力します。</para>
        /// </summary>
        /// <typeparam name="T">
        /// <para xml:lang="en">The type of object to dump.</para>
        /// <para xml:lang="ja">ダンプするオブジェクトの型</para>
        /// </typeparam>
        /// <param name="me">
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
        /// Set null or like <code>a=&gt;a</code>  to dump all properties of the object.
        /// </para>
        /// <para xml:lang="ja">
        /// ダンプするプロパティを選択するラムダ式。
        /// オブジェクトのAプロパティとBプロパティをダンプする場合には、<code>a=&gt;new {a.A, a.B}</code> のように指定します。
        /// 全てのプロパティをダンプする場合にはnull、あるいは<code>a=&gt;a</code>を指定します。
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
        public static void DumpTsv<T>(this IEnumerable<T> me, string name, Func<T, object> format = null, bool writeHeader = true)
        {
            Dumper.DumpTsv(me, name, format ?? (a => a), writeHeader);
        }

        /// <summary>
        /// <para xml:lang="en">Output the collection of names as header of a TSV file.</para>
        /// <para xml:lang="ja">TSVファイルヘッダとして、名前のコレクションを出力します。</para>
        /// </summary>
        /// <param name="me">
        /// <para xml:lang="en">A list of names</para>
        /// <para xml:lang="ja">名前の一覧</para>
        /// </param>
        /// <param name="name">
        /// <para xml:lang="en">The name which is used for a file.</para>
        /// <para xml:lang="ja">ファイルに使用される名前。</para>
        /// </param>
        public static void WriteTsvHeader(this IEnumerable<string> me, string name)
        {
            Dumper.WriteTsvHeader(name, me);
        }

        /// <summary>
        /// <para xml:lang="en">Output one row of table format values  to a TSV file. Output single row of multiple values on one row of the TSV file.</para>
        /// <para xml:lang="ja">表形式の値の1行をTSVファイルに出力します。複数の値の1行をTSVファイルの1行に出力します。</para>
        /// </summary>
        /// <typeparam name="T">
        /// <para xml:lang="en">The type of object to dump.</para>
        /// <para xml:lang="ja">ダンプするオブジェクトの型</para>
        /// </typeparam>
        /// <param name="me">
        /// <para xml:lang="en">The collection of values to output.Set one row in the table.</para>
        /// <para xml:lang="ja">出力する値のコレクション。表の1行を指定します。</para>
        /// </param>
        /// <param name="name">
        /// <para xml:lang="en">The name which is used for a file.</para>
        /// <para xml:lang="ja">ファイルに使用される名前。</para>
        /// </param>
        public static void WriteTsvRecord<T>(this IEnumerable<T> me, string name)
        {
            Dumper.WriteTsvRecord( me, name);
        }

        /// <summary>
        /// <para xml:lang="en">Output table format values  to a TSV file. Output multiple rows of multiple values on each rows of the TSV file.</para>
        /// <para xml:lang="ja">表形式の値をTSVファイルに出力します。複数の値の複数行をTSVファイルの各行に出力します。</para>
        /// </summary>
        /// <typeparam name="T">
        /// <para xml:lang="en">The type of object to dump.</para>
        /// <para xml:lang="ja">ダンプするオブジェクトの型</para>
        /// </typeparam>
        /// <param name="me">
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
        public static void WriteTsv<T>(this IEnumerable<IEnumerable<T>> me, string name, IEnumerable<string> header = null)
        {
            Dumper.WriteTsv(me, name, header);
        } // end sub

    } // end class
} // end namespace

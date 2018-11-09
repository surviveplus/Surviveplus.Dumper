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

    } // end class

} // end namespace

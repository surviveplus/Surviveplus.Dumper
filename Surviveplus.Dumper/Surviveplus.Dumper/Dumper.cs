using System;
using System.Collections.Generic;
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
        /// <para xml:lang="ja">ダンプ出力が有効かどうかを表す値を取得、あるいは設定します。</para>
        /// </summary>
        public static bool IsEnabled { get; set; } = Properties.Settings.Default.DumpIsEnabled;

    } // end class

} // end namespace

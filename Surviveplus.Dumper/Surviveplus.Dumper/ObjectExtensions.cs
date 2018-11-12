using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Surviveplus.Dump
{
    /// <summary>
    /// 
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        ///  Returns a json string that represents the current object.
        /// </summary>
        /// <param name="me">The instance of the type which is added this extension method.</param>
        /// <returns>
        ///  Returns a json string that represents the current object.
        /// </returns>
        public static string ToJson(this object me)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(me);
        } // end function

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="me"></param>
        /// <param name="name"></param>
        /// <param name="format"></param>
        public static void DumpJson<T>(this T me, string name, Func<T, object> format = null)
        {
            Dumper.DumpJson(me, name, format == null ? a=>a : format);
        } // end sub

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="me"></param>
        /// <param name="name"></param>
        /// <param name="format"></param>
        public static void DumpTsvHeader<T>(this T me, string name, Func<T, object> format = null)
        {
            Dumper.DumpTsvHeader(me, name, format == null ? a => a : format);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="me"></param>
        /// <param name="name"></param>
        /// <param name="format"></param>
        /// <param name="writeHeader"></param>
        public static void DumpTsvRecord<T>(this T me, string name, Func<T, object> format = null, bool writeHeader = false)
        {
            Dumper.DumpTsvRecord(me, name, format == null ? a => a : format, writeHeader);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="me"></param>
        /// <param name="name"></param>
        /// <param name="format"></param>
        /// <param name="writeHeader"></param>
        public static void DumpTsv<T>(this IEnumerable<T> me, string name, Func<T, object> format = null, bool writeHeader = true)
        {
            Dumper.DumpTsv(me, name, format == null ? a => a : format, writeHeader);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="me"></param>
        /// <param name="name"></param>
        public static void WriteTsvHeader<T>(this IEnumerable<T> me, string name)
        {
            Dumper.WriteTsvHeader(name, me);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="me"></param>
        /// <param name="name"></param>
        public static void WriteTsvRecord<T>(this IEnumerable<T> me, string name)
        {
            Dumper.WriteTsvRecord( me, name);
        }

        public static void WriteTsv<T>(this IEnumerable<IEnumerable<T>> me, string name, IEnumerable<string> header = null)
        {
            Dumper.WriteTsv(me, name, header);
        } // end sub

    } // end class
} // end namespace

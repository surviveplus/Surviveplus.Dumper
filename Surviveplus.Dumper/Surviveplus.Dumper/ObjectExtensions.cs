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
    } // end class
} // end namespace

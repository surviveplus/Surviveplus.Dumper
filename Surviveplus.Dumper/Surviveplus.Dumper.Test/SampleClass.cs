using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Surviveplus.Dump.Test
{
    public class SampleClassAB
    {
        public int A { get; set; }
        public bool B { get; set; }
    } // end class

    public class SampleClassABC
    {
        public SampleClassAB AB { get; set; }
        public string C { get; set; }
    } // end class

} // end namespace

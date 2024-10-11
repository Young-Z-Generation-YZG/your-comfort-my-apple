using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YGZ.Identity.Domain.Common.Errors
{
    internal class SampleErrors
    {
        public static Error SampleError  => new("SampleError", "Sample error");
    }
}

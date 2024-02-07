using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanCodeScaffold.Application.Exceptions
{
    public class FilterException : Exception
    {
        public FilterException()
        {
            VmName = string.Empty;
            ModelName = string.Empty;
        }
        public FilterException(string vmName, string modelName, string message): base(message)
        {
            VmName = vmName;
            ModelName = modelName;
        }
        public string VmName { get; set; }
        public string ModelName { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanCodeScaffold.Application.Dtos
{
    public class FilterVM
    {
        public FilterVM()
        {
            Key = string.Empty;
            Value = string.Empty;
            Operator = string.Empty;
        }
        public string Key { get; set; }
        public string Value { get; set; }
        public string Operator { get; set; }
        public string? PostOperator { get; set; }
    }
}

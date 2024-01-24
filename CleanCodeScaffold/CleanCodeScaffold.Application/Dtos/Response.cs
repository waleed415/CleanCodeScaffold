using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanCodeScaffold.Application.Dtos
{
    public class Response<T>
    {
        public Response()
        {
            Status = string.Empty;
            Message = new List<string>();
            Data = default;
        }
        public string Status { get; set; }
        public List<string> Message { get; set; }
        public T Data { get; set; }
    }
}

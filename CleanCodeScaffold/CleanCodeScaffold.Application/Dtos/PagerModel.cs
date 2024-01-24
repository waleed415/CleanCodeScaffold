using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanCodeScaffold.Application.Dtos
{
    public class PagerModel<T> where T : class
    {
        public PagerModel()
        {
            PageData = new List<T>();
        }
        public int TotalRecords { get; set; }
        public int CurrentPage { get; set; }
        public int RecordsPerPage { get; set; }
        public int TotalPages
        {
            get
            {
                var result = TotalRecords / RecordsPerPage;
                if (TotalRecords % RecordsPerPage > 0)
                    result = result + 1;
                return result;
            }
        }

        public IEnumerable<T> PageData { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanCodeScaffold.Infrastructure.AuditServices
{
    public interface ICurrentUserService
    {
        long GetLoggedInUserId();
    }
}

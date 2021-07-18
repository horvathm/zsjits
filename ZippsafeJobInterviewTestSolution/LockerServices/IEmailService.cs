using System.Collections.Generic;
using System.Threading.Tasks;

namespace LockerServices
{
    public interface IEmailService
    {
        Task SendEmail(IEnumerable<LockerState> lockerState);
    }
}

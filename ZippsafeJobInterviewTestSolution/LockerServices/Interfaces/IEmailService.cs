using System.Collections.Generic;
using System.Threading.Tasks;

namespace LockerServices.Interfaces
{
    public interface IEmailService
    {
        Task SendEmail(IEnumerable<LockerState> lockerState);
    }
}

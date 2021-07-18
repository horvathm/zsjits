using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LockerServices.Fakes
{
    public class LockerSystemManagerFake : ILockerSystemManager
    {
        public Task<IEnumerable<LockerState>> SwitchEcoOff()
        {
            return Task.FromResult(GetLockers(false));
        }

        public Task<IEnumerable<LockerState>> SwitchEcoOn()
        {
            return Task.FromResult(GetLockers(true));
        }

        private IEnumerable<LockerState> GetLockers(bool status) =>
            new List<LockerState>()
            {
                new LockerState(){ LockerGuid = Guid.Parse("b35365c8-7297-45e0-9c1e-07e2b9003726"), RunsInEco = status},
            }.AsEnumerable();
    }
}

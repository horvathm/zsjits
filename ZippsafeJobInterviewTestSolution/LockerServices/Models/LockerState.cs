using System;

namespace LockerServices
{
    public class LockerState
    {
        public Guid LockerGuid { get; init; }

        public bool RunsInEco { get; init; }
    }
}

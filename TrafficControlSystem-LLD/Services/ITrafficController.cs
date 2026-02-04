using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlSystem_LLD.Enums;

namespace TrafficControlSystem_LLD.Services
{
    public interface ITrafficController
    {
        void Start();
        void Stop();
        void SwitchMode(IntersectionMode mode);
        IntersectionMode GetCurrentMode();
    }
}

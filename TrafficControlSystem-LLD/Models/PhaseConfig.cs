using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlSystem_LLD.Enums;

namespace TrafficControlSystem_LLD.Models
{
    public class PhaseConfig
    {
        public int PhaseNumber { get; }
        public IReadOnlyCollection<Direction> GreenDirections { get; }
        public TimeSpan GreenDuration { get; }
        public TimeSpan YellowDuration { get; }

        public PhaseConfig(
            int phaseNumber,
            IEnumerable<Direction> greenDirections,
            TimeSpan greenDuration,
            TimeSpan yellowDuration)
        {
            PhaseNumber = phaseNumber;
            GreenDirections = greenDirections.ToList().AsReadOnly();
            GreenDuration = greenDuration;
            YellowDuration = yellowDuration;
        }
    }
}

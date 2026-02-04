using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlSystem_LLD.Enums;
using TrafficControlSystem_LLD.Models;

namespace TrafficControlSystem_LLD.Services
{
    public class TimingPlanService
    {
        // Provides a simple two-phase plan:
        // Phase 1: NorthSouth green, EastWest red
        // Phase 2: EastWest green, NorthSouth red
        public List<PhaseConfig> GetDefaultPhases()
        {
            var phase1 = new PhaseConfig(
                phaseNumber: 1,
                greenDirections: new[] { Direction.NorthSouth },
                greenDuration: TimeSpan.FromSeconds(10),
                yellowDuration: TimeSpan.FromSeconds(3));

            var phase2 = new PhaseConfig(
                phaseNumber: 2,
                greenDirections: new[] { Direction.EastWest },
                greenDuration: TimeSpan.FromSeconds(10),
                yellowDuration: TimeSpan.FromSeconds(3));

            return new List<PhaseConfig> { phase1, phase2 };
        }
    }
}

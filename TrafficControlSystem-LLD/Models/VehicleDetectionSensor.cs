using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlSystem_LLD.Enums;

namespace TrafficControlSystem_LLD.Models
{
    // Simplified sensor: could be extended to read real-time data
    public class VehicleDetectionSensor
    {
        public Direction Direction { get; }

        public VehicleDetectionSensor(Direction direction)
        {
            Direction = direction;
        }

        public bool IsVehicleWaiting()
        {
            // In a real system this would query hardware or telemetry.
            // For simulation, randomly say yes sometimes.
            return Random.Shared.Next(0, 2) == 1;
        }
    }
}

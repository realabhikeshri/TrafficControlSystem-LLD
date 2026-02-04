using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlSystem_LLD.Enums;

namespace TrafficControlSystem_LLD.Models
{
    public class Intersection
    {
        private readonly Dictionary<Direction, TrafficLight> _lights = new();

        public IReadOnlyList<PhaseConfig> Phases { get; }

        public Intersection(IEnumerable<PhaseConfig> phases)
        {
            _lights[Direction.NorthSouth] = new TrafficLight(Direction.NorthSouth);
            _lights[Direction.EastWest] = new TrafficLight(Direction.EastWest);
            Phases = phases.ToList().AsReadOnly();

            // Initialize all lights to Red
            foreach (var light in _lights.Values)
            {
                light.SetColor(LightColor.Red);
            }
        }

        public TrafficLight GetLight(Direction direction) => _lights[direction];

        public IEnumerable<TrafficLight> GetAllLights() => _lights.Values;

        public void SetAll(LightColor color)
        {
            foreach (var light in _lights.Values)
            {
                light.SetColor(color);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlSystem_LLD.Enums;

namespace TrafficControlSystem_LLD.Models
{
    public class TrafficLight
    {
        private readonly object _lock = new();

        public Direction Direction { get; }
        public LightColor Color { get; private set; }

        public TrafficLight(Direction direction)
        {
            Direction = direction;
            Color = LightColor.Red;
        }

        public void SetColor(LightColor color)
        {
            lock (_lock)
            {
                Color = color;
            }
        }

        public LightColor GetColor()
        {
            lock (_lock)
            {
                return Color;
            }
        }

        public override string ToString()
        {
            return $"{Direction}: {Color}";
        }
    }
}

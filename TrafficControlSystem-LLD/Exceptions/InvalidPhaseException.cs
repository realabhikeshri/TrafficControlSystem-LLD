using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficControlSystem_LLD.Exceptions
{
    public class InvalidPhaseException : Exception
    {
        public InvalidPhaseException(string message) : base(message)
        {
        }
    }
}

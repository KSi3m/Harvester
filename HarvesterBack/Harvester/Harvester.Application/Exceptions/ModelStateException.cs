using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harvester.Application.Exceptions
{
    public class ModelStateException : Exception
    {
        public ModelStateException()
        {
        }

        public ModelStateException(string? message) : base(message)
        {
        }
    }
}

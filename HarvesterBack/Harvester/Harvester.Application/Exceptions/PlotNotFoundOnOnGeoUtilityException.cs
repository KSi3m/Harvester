using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harvester.Application.Exceptions
{
    public class PlotNotFoundOnOnGeoUtilityException: Exception
    {
        public PlotNotFoundOnOnGeoUtilityException()
        {
        }

        public PlotNotFoundOnOnGeoUtilityException(string? message) : base(message)
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harvester.Application.Dtos
{
    public class GeoServiceGetFieldDataResponse
    {
        public decimal AreaHectares { get; set; }

        public GeoPointDto? CenterPoint { get; set; } = default!;

        public GeoMultiPolygonDto? Boundary { get; set; } = default!;
    }
}

using Harvester.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harvester.Application.Interfaces.Services
{
    public interface IOnGeoService
    {
        Task<GeoServiceGetFieldDataResponse> GetDataAsync(string nameIdentifier);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harvester.Application.Interfaces.Services
{
    public interface IOnGeoService
    {
        Task<decimal> GetDataAsync(string nameIdentifier);
    }
}

using GeoLocator.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoLocator.Application.Interfaces
{
    public interface IGeolocationRepository
    {
        Task<IPLookupResponse> GetCountryByIpAsync(string ipAddress);
    }
}
    
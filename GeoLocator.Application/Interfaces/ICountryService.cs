using GeoLocator.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoLocator.Application.Interfaces
{
    public interface ICountryService
    {
        Task<CountryInfo?> GetCountryByCodeAsync(string countryCode);
        bool IsValidCountryCode(string countryCode);
    }
}

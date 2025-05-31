using GeoLocator.Application.Dtos;
using GeoLocator.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoLocator.Application.Interfaces
{
    public interface IBlockedCountryRepository
    {
        Task<bool> AddBlockedCountryAsync(BlockedCountry country);
        Task<bool> RemoveBlockedCountryAsync(string countryCode);
        Task<BlockedCountry?> GetBlockedCountryAsync(string countryCode);
        Task<PaginatedResponse<BlockedCountry>> GetAllBlockedCountriesAsync(PaginationRequest request);
        Task<bool> IsCountryBlockedAsync(string countryCode);
        Task RemoveExpiredBlocksAsync();
    }
}

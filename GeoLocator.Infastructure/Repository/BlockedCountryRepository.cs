using GeoLocator.Application.Interfaces;
using GeoLocator.Domain.Entities;
using GeoLocator.Application.Dtos;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoLocator.Infastructure.Repository
{
    // This repository uses in-memory collections to store blocked countries and temporal blocks,
    // similar to the service implementation. This is suitable for prototyping or testing.
    // For production, consider replacing with persistent storage (e.g., a database).
    public class BlockedCountryRepository : IBlockedCountryRepository
    {
        // Use thread-safe collections to store data in-memory, matching the service logic.
        private readonly ConcurrentDictionary<string, BlockedCountry> _blockedCountries = new();

        public  async Task<bool> AddBlockedCountryAsync(BlockedCountry country)
        {
            return await Task.FromResult(_blockedCountries.TryAdd(country.CountryCode, country));
        }
            
        public async Task<PaginatedResponse<BlockedCountry>> GetAllBlockedCountriesAsync(PaginationRequest request)
        {
            var allBlockedCountries = _blockedCountries.Values.AsEnumerable();


            // Apply filtering based on the search term if provided
            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var searchTerm = request.SearchTerm.ToLower();
                allBlockedCountries = allBlockedCountries.Where(c =>
                    c.CountryCode.ToLower().Contains(searchTerm) ||
                    c.CountryName.ToLower().Contains(searchTerm));
            }

            // Apply pagination

            var paginatedCountries = allBlockedCountries
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            var totalCount = allBlockedCountries.Count();

            return await Task.FromResult(new PaginatedResponse<BlockedCountry>
            {
                Items = paginatedCountries,
                TotalCount = totalCount,
                PageNumber = request.Page,
                PageSize = request.PageSize
            });
        }

        public async Task<BlockedCountry?> GetBlockedCountryAsync(string countryCode)
        {
            _blockedCountries.TryGetValue(countryCode, out var country);
            return await Task.FromResult(country);
        }

        public async Task<bool> IsCountryBlockedAsync(string countryCode)
        {

            return await Task.FromResult(_blockedCountries.ContainsKey(countryCode));
        }

        public async Task<bool> RemoveBlockedCountryAsync(string countryCode)
        {

            if (!_blockedCountries.TryRemove(countryCode, out _))
                throw new Exception($"Country {countryCode} is not blocked.");
            return await Task.FromResult(true);
            //return await Task.FromResult(_blockedCountries.TryRemove(countryCode, out _));
        }

        public Task RemoveExpiredBlocksAsync()
        {
            var now = DateTime.UtcNow;
            var expiredBlocks = _blockedCountries.Values.Where(tb => tb.BlockedAt <= now)
                .Select(s => s.CountryCode).ToList();
            foreach (var block in expiredBlocks)
            {
                _blockedCountries.TryRemove(block, out _);
                    
            }
            return Task.CompletedTask;
        }
    }
}


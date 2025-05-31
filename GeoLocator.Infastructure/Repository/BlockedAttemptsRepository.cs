using GeoLocator.Application.Interfaces;
using GeoLocator.Domain.Entities;
using GeoLocator.Application.Dtos;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoLocator.Infastructure.Repository
{
    public class BlockedAttemptsRepository : IBlockedAttemptsRepository
    {
        private readonly ConcurrentDictionary<Guid, BlockedAttemptLog> _attempts = new();
        public async Task AddBlockedAttemptAsync(BlockedAttemptLog attempt)
        {
            _attempts.TryAdd(attempt.Id, attempt);
            await Task.CompletedTask;   

        }

        public async Task ClearAttemptsAsync(DateTime before)
        {
            var keysToRemove = _attempts
                .Where(kvp => kvp.Value.Timestamp < before)
                .Select(kvp => kvp.Key)
                .ToList();
            foreach (var key in keysToRemove)
            {
                _attempts.TryRemove(key, out _);
            }
            await Task.CompletedTask;
        }

        public async Task<PaginatedResponse<BlockedAttemptLog>> GetBlockedAttemptsAsync(PaginationRequest request)
        {
            var allAttempts = _attempts.Values.AsEnumerable();
            // Apply filtering based on the search term if provided
            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var searchTerm = request.SearchTerm.ToLower();
                allAttempts = allAttempts.Where(a =>
                    a.IPAddress.ToLower().Contains(searchTerm) ||
                    a.CountryCode.ToLower().Contains(searchTerm));
            }
            // Apply pagination
            var paginatedAttempts = allAttempts
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();
            var totalCount = allAttempts.Count();
            return await Task.FromResult(new PaginatedResponse<BlockedAttemptLog>
            {
                Items = paginatedAttempts,
                TotalCount = totalCount,
                PageNumber = request.Page,
                PageSize = request.PageSize
            });
        }
    }
}

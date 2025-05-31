using GeoLocator.Domain.Entities;
using GeoLocator.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoLocator.Application.Interfaces
{
    public interface IBlockedAttemptsRepository
    {
        Task AddBlockedAttemptAsync(BlockedAttemptLog attempt); 
        Task<PaginatedResponse<BlockedAttemptLog>> GetBlockedAttemptsAsync(PaginationRequest request);
        Task ClearAttemptsAsync(DateTime before);
    } 
}

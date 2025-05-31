using GeoLocator.Application.Interfaces;
using GeoLocator.Domain.Entities;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoLocator.Infastructure.Repository
{
    public class TemporaryBlockRepository : BackgroundService
    {
        private readonly IBlockedCountryRepository blockedCountryRepository ;
        private readonly ILogger<TemporaryBlockRepository> _logger;
        private readonly TimeSpan _cleanupInterval = TimeSpan.FromMinutes(5);
        public TemporaryBlockRepository(
            IBlockedCountryRepository blockedCountryRepository,
            ILogger<TemporaryBlockRepository> logger)
        {
            this.blockedCountryRepository = blockedCountryRepository;
            _logger = logger;
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("TemporaryBlockRepository started.");
            return Task.Run(async () =>
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        await blockedCountryRepository.RemoveExpiredBlocksAsync();
                        _logger.LogInformation("Expired temporary blocks removed successfully.");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error occurred while removing expired temporary blocks.");
                    }
                    await Task.Delay(_cleanupInterval, stoppingToken);
                }
            }, stoppingToken);
        }
    }
}

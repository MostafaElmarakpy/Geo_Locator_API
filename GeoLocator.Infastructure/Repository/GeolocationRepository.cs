using GeoLocator.Application.Interfaces;
using GeoLocator.Application.Dtos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GeoLocator.Infastructure.Repository
{
    public class GeolocationRepository : IGeolocationRepository
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<GeolocationRepository> _logger;
        public GeolocationRepository(
            HttpClient httpClient,
            IConfiguration configuration,
            ILogger<GeolocationRepository> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
        }
        public async Task<IPLookupResponse> GetCountryByIpAsync(string ipAddress)
        {
            try
            {
                if (!IsValidIPAddress(ipAddress))
                    throw new ArgumentException("Invalid IP address format", nameof(ipAddress));

                var apiKey = _configuration["IPGeolocation:ApiKey"];
                if (string.IsNullOrEmpty(apiKey))
                    throw new InvalidOperationException("IPGeolocation API key is not configured");

                var baseUrl = _configuration["IPGeolocation:BaseUrl"] ?? "https://api.ipgeolocation.io/";
                var requestUrl = $"{baseUrl.TrimEnd('/')}/ipgeo?apiKey={Uri.EscapeDataString(apiKey)}&ip={Uri.EscapeDataString(ipAddress)}";

                _logger.LogInformation("Requesting geolocation for IP: {IpAddress}", ipAddress);
                var response = await _httpClient.GetAsync(requestUrl);

                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var result = JsonSerializer.Deserialize<IPLookupResponse>(content, options);

                if (result == null)
                    throw new Exception("Failed to deserialize IP lookup response");

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching IP location for {IpAddress}", ipAddress);
                throw;
            }
        }

        private bool IsValidIPAddress(string ipAddress)
        {
            return IPAddress.TryParse(ipAddress, out _);
        }

    }
}

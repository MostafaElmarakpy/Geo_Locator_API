using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoLocator.Domain.Entities
{
    public class TemporalBlock
    {
        public string CountryCode { get; set; }
        public DateTime ExpiryTime { get; set; }
        


        public TemporalBlock(string countryCode, int durationMinutes)
        {
            if (string.IsNullOrEmpty(countryCode) || countryCode.Length != 2)
                throw new ArgumentException("Country code must be a valid 2-letter code");

            if (durationMinutes < 1 || durationMinutes > 1440)
                throw new ArgumentException("Duration must be between 1 and 1440 minutes");

            CountryCode = countryCode.ToUpperInvariant();
            ExpiryTime = DateTime.UtcNow.AddMinutes(durationMinutes);
        }

    }
}

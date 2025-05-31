using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoLocator.Domain.Entities
{
    public class BlockedCountry
    {
        public  string CountryCode { get; set; } = string.Empty;
        public string CountryName { get; set; } = string.Empty;
        public DateTime BlockedAt { get; set; } = DateTime.UtcNow;
        public DateTime ExpiryTime { get; set; }
        public bool IsTemporary { get; set; }

        public bool IsExpired() => DateTime.UtcNow >= ExpiryTime;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoLocator.Domain.Entities
{
    public class BlockedAttemptLog
    {

        public Guid Id { get; set; } 
        public string CountryCode { get; set; } = string.Empty;
        public string CountryName { get; set; } = string.Empty;
        public string UserAgent { get; set; } = string.Empty;
        public string IPAddress { get; set; } = string.Empty;   
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public bool BlockedStatus { get; set; }
        public string RequestPath { get; set; } = string.Empty;

    }
}

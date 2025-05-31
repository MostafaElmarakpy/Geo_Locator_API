using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoLocator.Application.Dtos
{
    public class BlockCountryDto
    {
        public string CountryCode { get; set; } = string.Empty;
        public bool IsTemporary { get; set; }
        public int? DurationMinutes { get; set; }
    }
}

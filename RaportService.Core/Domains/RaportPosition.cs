using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaportService.Core.Domains
{
    public class RaportPosition
    {
        public int Id { get; set; }
        public int RaportId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Value { get; set; }

    }
}

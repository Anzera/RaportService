using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaportService.Core.Domains
{
    public class Raport
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public bool IsSend { get; set; }
        public List<RaportPosition> Positions { get; set; }
    }
}

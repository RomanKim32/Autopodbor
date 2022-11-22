using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Autopodbor_312.Models
{
    public class UploadFile
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public string Type { get; set; }
        public int? NewsId { get; set; }
        public News News { get; set; }
        public int? PortfolioId { get; set; }
        public Portfolio Portfolio { get; set; }
    }
}

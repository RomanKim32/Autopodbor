using Autopodbor_312.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Autopodbor_312.ViewModel
{
    public class PortfolioAndUploadFileViewModel
    {
        public Portfolio Portfolio { get; set; }
        public PortfolioNewsFile MainPic { get; set; }
        public List<PortfolioNewsFile> Pictures { get; set; }
        public List<PortfolioNewsFile> Videos { get; set; }
    }
}

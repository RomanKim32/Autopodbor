using Autopodbor_312.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Autopodbor_312.ViewModel
{
    public class PortfolioViewModel
    {
        List<Portfolio> Portfolios { get; set; }
        List<UploadFile> Pictures { get; set; }
        List<UploadFile> Videos { get; set; }
    }
}

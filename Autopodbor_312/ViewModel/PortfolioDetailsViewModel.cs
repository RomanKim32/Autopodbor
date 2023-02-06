using Autopodbor_312.Models;
using System.Collections.Generic;

namespace Autopodbor_312.ViewModel
{
    public class PortfolioDetailsViewModel
    {
        public Portfolio Portfolio { get; set; }
        public PortfolioNewsFile MainPic { get; set; }
        public List<PortfolioNewsFile> MinorPictures { get; set; }
        public List<PortfolioNewsFile> Videos { get; set; }
        public IEnumerable<CarsBrandsModel> CarsBrandsModels { get; set; }
        public IEnumerable<CarsBrands> CarsBrands1 { get; set; }
    }
}

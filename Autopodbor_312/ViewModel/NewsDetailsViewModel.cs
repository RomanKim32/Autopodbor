using Autopodbor_312.Models;
using System.Collections.Generic;

namespace Autopodbor_312.ViewModel
{
    public class NewsDetailsViewModel
    {
        public News News { get; set; }
        public PortfolioNewsFile MainPic { get; set; }
        public List<PortfolioNewsFile> MinorPictures { get; set; }
        public List<PortfolioNewsFile> Videos { get; set; }
    }

}

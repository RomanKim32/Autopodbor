﻿using Autopodbor_312.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Autopodbor_312.ViewModel
{
    public class PortfolioDetailsViewModel
    {
        public Portfolio Portfolio { get; set; }
        public PortfolioNewsFile MainPic { get; set; }
        public List<PortfolioNewsFile> MinorPictures { get; set; }
        public List<PortfolioNewsFile> Videos { get; set; }
/*        public IEnumerable<CarsBodyTypes> CarsBodyTypes { get; set; }
        public IEnumerable<CarsBrands> CarsBrands { get; set; }
        public IEnumerable<CarsBrandsModel> CarsBrandsModels { get; set; }*/
    }
}

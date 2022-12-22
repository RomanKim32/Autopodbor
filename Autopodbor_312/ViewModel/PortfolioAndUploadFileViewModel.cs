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
        public UploadFile MainPic { get; set; }
        public List<UploadFile> Pictures { get; set; }
        public List<UploadFile> Videos { get; set; }
    }
}

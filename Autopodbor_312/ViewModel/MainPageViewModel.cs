using Autopodbor_312.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Autopodbor_312.ViewModel
{
    public class MainPageViewModel
    {
        public MainPage MainPage { get; set; }
        public MainPageFile MainPic { get; set; }
        public List<MainPage> MainPages { get; set; }
        public List<MainPageFile> MainPageFiles { get; set; }
    }
}

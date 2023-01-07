using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Autopodbor_312.Models
{
    public class MainPageFile
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public int? MainPageId { get; set; }
        public MainPage MainPage { get; set; }
        
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Autopodbor_312.Models
{
    public class MainPage
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Banner { get; set; }
        public DateTime CreatedDate { get; set; }
        [NotMapped]
        public List<MainPageFile> Images { get; set; }
    }
}

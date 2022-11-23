using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Autopodbor_312.Models
{
    public class Portfolio
    {
        public int Id { get; set; }
        public string Name { get; set; }   
        public string Body { get; set; }
        public DateTime CreatedDate { get; set; }
        [NotMapped]
        public UploadFile Image { get; set; }
    }
}

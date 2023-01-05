using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace Autopodbor_312.Models
{
    public class Portfolio
    {
        public int Id { get; set; }
        public string NameRu { get; set; }   
        public string BodyRu { get; set; }
        public string NameKy { get; set; }
        public string BodyKy { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool Publicate { get; set; }
        public string MainImagePath { get; set; }
    }
}

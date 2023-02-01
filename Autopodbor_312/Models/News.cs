using System;

namespace Autopodbor_312.Models
{
    public class News
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

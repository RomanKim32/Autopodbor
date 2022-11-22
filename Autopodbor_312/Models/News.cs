using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Autopodbor_312.Models
{
    public class News
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Body { get; set; }
        public DateTime CreatedDate { get; set; }
        [NotMapped]
        public List<UploadFile> Images { get; set; }
        [NotMapped]
        public List<UploadFile> Videos { get; set; }

    }
}

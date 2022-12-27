﻿using System;
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
        public bool Publicate { get; set; }
        public string MainImagePath { get; set; }

    }
}

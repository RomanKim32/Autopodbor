﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Autopodbor_312.Models
{
    public class Services
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Поле обязательно к заполнению")]
        #nullable enable
		public string? NameRu { get; set; }
        [Required(ErrorMessage = "Поле обязательно к заполнению")]
        public string? DescriptionRu { get; set; }
        [Required(ErrorMessage = "Поле обязательно к заполнению")]
        public string? NameKy { get; set; }
        [Required(ErrorMessage = "Поле обязательно к заполнению")]
        public string? DescriptionKy { get; set; }
        public bool? IsAdditional { get; set; }
        public string? Photo { get; set; }
	}
}

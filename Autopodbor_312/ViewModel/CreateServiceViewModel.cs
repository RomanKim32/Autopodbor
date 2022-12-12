using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Autopodbor_312.ViewModel
{
	public class CreateServiceViewModel
	{
        [Required]
        public string ServiceNameRu { get; set; }
        [Required]
        public string ServiceNameKy { get; set; }
        [Required]
        public string ServiceDescriptionRu { get; set; }
        [Required]
        public string ServiceDescriptionKy { get; set; }
        [Required]
        public IFormFile ServicePhoto { get; set; }
	}
}

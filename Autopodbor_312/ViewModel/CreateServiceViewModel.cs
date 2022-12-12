using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Autopodbor_312.ViewModel
{
	public class CreateServiceViewModel
	{
        [Required(ErrorMessage = "Поле обязательно к заполнению")]
        public string ServiceNameRu { get; set; }
        [Required(ErrorMessage = "Поле обязательно к заполнению")]
        public string ServiceNameKy { get; set; }
        [Required(ErrorMessage = "Поле обязательно к заполнению")]
        public string ServiceDescriptionRu { get; set; }
        [Required(ErrorMessage = "Поле обязательно к заполнению")]
        public string ServiceDescriptionKy { get; set; }
        [Required(ErrorMessage = "Поле обязательно к заполнению")]
        public IFormFile ServicePhoto { get; set; }
	}
}

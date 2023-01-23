﻿using System.ComponentModel.DataAnnotations;

namespace Autopodbor_312.ViewModel
{
    public class EditUserViewModel
    {
        public int Id { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Обязательно к заполнению")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string Role { get; set; }

        [Required(ErrorMessage = "Обязательно к заполнению")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }
    }
}

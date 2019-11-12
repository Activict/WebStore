using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebStore.Models.ViewModels.Account
{
    public class UserVM
    {
        public UserVM()
        {        }
        public UserVM(UserVM row)
        {
            Id = row.Id;
            FirstName = row.FirstName;
            LastName = row.LastName;
            Email = row.Email;
            UserName = row.UserName;
            Password = row.Password;
        }
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
    }
}
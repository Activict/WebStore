using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        [DisplayName("FirstName")]
        public string FirstName { get; set; }
        [Required]
        [DisplayName("LastName")]
        public string LastName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        [DisplayName("Email")]
        public string Email { get; set; }
        [Required]
        [DisplayName("User Name")]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [DisplayName("Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}
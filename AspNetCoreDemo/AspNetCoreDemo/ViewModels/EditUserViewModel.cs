﻿using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AspNetCoreDemo.ViewModels
{
    public class EditUserViewModel
    {
        public string Id { get; set; }

        [Required]
        [DisplayName("User Name")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string City { get; set; }

        public List<string> Claims { get; set; }

        public IList<string> Roles { get; set; }

        public EditUserViewModel()
        {
            Claims = new List<string>();
            Roles = new List<string>();
        }
    }
}
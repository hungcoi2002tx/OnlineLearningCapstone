using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Other
{
    public enum Status
    {
        [Display(Name = "Nhóm tài khoản")]
        AppRole = 0,
        [Display(Name = "Tài khoản")]
        AppUser = 1
    }
}

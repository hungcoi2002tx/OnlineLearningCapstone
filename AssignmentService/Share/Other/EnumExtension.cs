using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Other
{
    public enum AssignmentStatus
    {
        [Display(Name = "Nháp")]
        Draft = 0,
        [Display(Name = "Công khai")]
        Published = 1,
        [Display(Name = "Đóng")]
        Closed = 2
    }

    public enum AssignmentType
    {
        [Display(Name = "Tự luận")]
        Essay = 0,
        [Display(Name = "Trắc nghiệm")]
        Quiz = 1
    }
}

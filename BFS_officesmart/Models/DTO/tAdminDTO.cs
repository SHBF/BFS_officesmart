using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BFS_officesmart.Models.DTO
{
    public class tAdminDTO
    {
        [Display(Name = "用户名")]
        [MaxLength(12, ErrorMessage = "太长了")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "用户名不能为空")]
        //[StringLength(20, MinimumLength = 6, ErrorMessage = "用户名不能大于{2} 且要小于{1}")]
        public string Name { get; set; }
        [Display(Name = "密码")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "密码不能为空")]
        public string Password { get; set; }
        public string Code { get; set; }
    }
}
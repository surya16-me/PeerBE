using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO.Req
{
    public class ReqRegisterUserDto
    {
        [Required(ErrorMessage = "name is required")]
        [MaxLength(30, ErrorMessage = "name cannot exceed 30 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [MaxLength(50, ErrorMessage = "email cannot exceed 30 characters")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
        [MaxLength(50, ErrorMessage = "Password cannot exceed 50 characters")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Role is required")]
        [MaxLength(30, ErrorMessage = "Role cannot exceed 30 characters")]
        public string Role { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Balance must be a positive value")]
        public decimal? Balance { get; set; }

    }
}

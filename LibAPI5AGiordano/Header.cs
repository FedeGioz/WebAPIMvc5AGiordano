using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace LibAPI5AGiordano
{
    public class Header
    {
        [FromHeader]
        [Required]
        [StringLength(3, MinimumLength = 3)]
        [RegularExpression("^[A-Za-z0-9]+$", ErrorMessage = "DevName must contain only letters and digits.")]
        public string DevName { get; set; }

        [FromHeader]
        [Required]
        [StringLength(13)]
        [RegularExpression("^[A-Za-z0-9]+$", ErrorMessage = "Team must contain only letters and digits.")]
        public string Team { get; set; }
    }
}

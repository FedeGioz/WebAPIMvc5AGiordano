using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace WebAPIMvc5AGiordano.Models
{
    public class Reservation
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "ReservationCode length can't be more than 20.")]
        [RegularExpression("^[A-Z0-9]+$", ErrorMessage = "ReservationCode must contain only uppercase letters and digits.")]
        public string ReservationCode { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime ReservationDate { get; set; }

        [StringLength(15, ErrorMessage = "Phone length can't be more than 15.")]
        [RegularExpression(@"^\+?[0-9]*$", ErrorMessage = "Phone must contain only digits and an optional leading '+'.")]
        public string Phone { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "Email length can't be more than 30.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required]
        [StringLength(13, ErrorMessage = "Vat length can't be more than 13.")]
        [RegularExpression("^[A-Z0-9]+$", ErrorMessage = "Vat must contain only uppercase letters and digits.")]
        public string Vat { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "Description length can't be more than 500.")]
        [RegularExpression(@"^[A-Za-z0-9àèéòù' ]+$", ErrorMessage = "Description must contain only letters, digits, and allowed special characters.")]
        public string Description { get; set; }

        [Required]
        [Range(0, 9999.99, ErrorMessage = "Amount must be between 0 and 9999.99.")]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }
    }
}

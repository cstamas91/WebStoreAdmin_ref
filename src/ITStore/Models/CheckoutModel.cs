using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ITStore.Models
{
    /// <summary>
    /// A vásárlás befejezésekor kellő adatok kitöltése.
    /// </summary>
    public class CheckoutModel
    {
        [Required(ErrorMessage = "A név megadása kötelező.")]
        [StringLength(40, ErrorMessage = "A vásárló neve maximum 40 karakter lehet.")]
        public String CustomerName { get; set; }

        [Required(ErrorMessage = "Az e-mail cím megadása kötelező.")]
        [EmailAddress(ErrorMessage = "Az e-mail cím nem megfelelő formátumú.")]
        [DataType(DataType.EmailAddress)]
        public String CustomerEmail { get; set; }

        [Required(ErrorMessage = "A cím megadása kötelező.")]
        public String CustomerAddress { get; set; }

        [Required(ErrorMessage = "A telefonszám megadása kötelező.")]
        [Phone(ErrorMessage = "A telefonszám formátuma nem megfelelő.")]
        [DataType(DataType.PhoneNumber)]
        public String CustomerPhoneNumber { get; set; }
    }
}
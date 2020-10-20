using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WADAuth.Models
{
    public class UserCart
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string UserID { get; set; }
        [Required]
        public int ProductID { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
}
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FiledExercise.Models
{
    public class CardDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CardId { get; set; }
        private DateTime _expirationDate { get; set; }

        [StringLength(16, MinimumLength = 13, ErrorMessage = "Invalid CCN")]
        [Required]
        [DataType(DataType.CreditCard)]
        public string CreditCardNumber { get; set; }

        [Required]
        public string CardHolder { get; set; }

        [Required]
        public DateTime ExpirationDate { get; set; }
        // {
        //     get => _expirationDate;
        //     set => _expirationDate = value <= DateTime.Now ? throw new Exception("Credit Card has expired") : (_expirationDate = value);
        // }

        #nullable enable
        [StringLength(3, MinimumLength = 3)]
        public string? SecurityCode { get; set; }
        #nullable disable

        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }
    }
}

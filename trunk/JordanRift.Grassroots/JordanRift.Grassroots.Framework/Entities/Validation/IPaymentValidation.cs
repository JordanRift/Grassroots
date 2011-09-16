//
// Grassroots is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Grassroots is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with Grassroots.  If not, see <http://www.gnu.org/licenses/>.
//

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace JordanRift.Grassroots.Framework.Entities.Validation
{
    public interface IPaymentValidation
    {
        [Required(ErrorMessage = "Please enter the amount.")]
        [RegularExpression(@"^\$?\d+(\.(\d{2}))?$", ErrorMessage = "Please enter a valid amount.")]
        [Range(1.00, 100000000.00, ErrorMessage = "Please enter an amount greater than $1.00.")]
        decimal Amount { get; set; }

        [DisplayName("Is this a monthly donation?")]
        [UIHint("TransactionType")]
        TransactionType TransactionType { get; set; }

        [DisplayName("My donation should go toward:")]
        string Notes { get; set; }

        [DisplayName("Payment Type")]
        [UIHint("PaymentType")]
        PaymentType PaymentType { get; set; }

        [DisplayName("Account Number")]
        [Required(ErrorMessage = "Please enter your account number.")]
        string AccountNumber { get; set; }

        [DisplayName("Expiration Date")]
        [UIHint("ExpirationDate")]
        DateTime Expiration { get; set; }

        [DisplayName("CID Number")]
        string Cid { get; set; }

        [DisplayName("Routing Number")]
        string RoutingNumber { get; set; }

        [DisplayName("Bank Name")]
        string BankName { get; set; }

        [DisplayName("Check Type")]
        [UIHint("CheckType")]
        CheckType CheckType { get; set; }

        [DisplayName("Check Number")]
        string CheckNumber { get; set; }

        [DisplayName("First Name")]
        [Required(ErrorMessage = "Please enter your first name.")]
        string FirstName { get; set; }

        [DisplayName("Last Name")]
        [Required(ErrorMessage = "Please enter your last name.")]
        string LastName { get; set; }

        [DisplayName("Email Address")]
        [Required(ErrorMessage = "Please enter your email address.")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
            ErrorMessage = "Please enter a valid email address.")]
        string Email { get; set; }

        [DisplayName("Primary Phone")]
        [Required(ErrorMessage = "Please enter your primary phone number.")]
        [RegularExpression(@"^(?:\([2-9]\d{2}\)\ ?|[2-9]\d{2}(?:\-?|\ ?))[2-9]\d{2}[- ]?\d{4}$",
            ErrorMessage = "Please enter a valid phone number.")]
        string PrimaryPhone { get; set; }

        [DisplayName("Billing Address")]
        [Required(ErrorMessage = "Please enter your billing address.")]
        string AddressLine1 { get; set; }

        [DisplayName("City")]
        [Required(ErrorMessage = "Please enter your city.")]
        string City { get; set; }

        [DisplayName("State")]
        [Required(ErrorMessage = "Please enter your state.")]
        [UIHint("State")]
        string State { get; set; }

        [DisplayName("Zip Code")]
        [Required(ErrorMessage = "Please enter your zip code.")]
        [RegularExpression(@"(^\d{5}$)|(^\d{5}-\d{4}$)", ErrorMessage = "Please enter a valid zip code.")]
        string ZipCode { get; set; }

        [Display(Name = "Would you like to donate anonymously?")]
        bool IsAnonymous { get; set; }

        [Display(Name = "Display Name (optional)")]
        string DisplayName { get; set; }
    }
}

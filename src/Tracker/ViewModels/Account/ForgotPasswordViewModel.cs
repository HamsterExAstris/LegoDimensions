using System.ComponentModel.DataAnnotations;

namespace ShatteredTemple.LegoDimensions.Tracker.ViewModels.Account
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}

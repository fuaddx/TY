using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace Pustok2.ViewModel.AuthVM 
{
    public class RegisterVM
    {
        [Required(ErrorMessage ="Ad ve soyad  daxil et!"),MaxLength(35)]
        public string Fullname { get; set; }
        [Required,DataType(DataType.EmailAddress) ]
        public string Email  { get; set; }
        [Required(ErrorMessage = "Istifadeci adini  daxil et!"), MaxLength(24)]
        public  string Username  { get; set; }
        [Required, DataType(DataType.Password),Compare(nameof(ConfirmPassword)), RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{4,}$", ErrorMessage = "Wrong input for password")]
        public string Password { get; set; }
        [Required, DataType(DataType.Password), RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{4,}$", ErrorMessage = "Wrong input for password")]
        public string ConfirmPassword { get; set; }
    }
}

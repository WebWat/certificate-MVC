using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels
{
    public class LinkViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Это обязательное поле")]
        [Url(ErrorMessage = "Некорректный url")]
        public string Name { get; set; }

        public int CertificateId { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels;

public class LinkViewModel
{
    public string Id { get; set; }

    [Required(ErrorMessage = "Required")]
    [Url(ErrorMessage = "IncorrectUrl")]
    public string Url { get; set; }

    public string CertificateId { get; set; }
}

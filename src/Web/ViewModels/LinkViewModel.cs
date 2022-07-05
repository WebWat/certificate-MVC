using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels;

public class LinkViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Required")]
    [Url(ErrorMessage = "IncorrectUrl")]
    public string Url { get; set; } = string.Empty;

    public int CertificateId { get; set; }
}

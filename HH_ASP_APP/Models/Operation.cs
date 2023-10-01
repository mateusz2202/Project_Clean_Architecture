using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace HH_ASP_APP.Models;

public class Operation
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Pole wymagne")]
    [DisplayName("Kod operacji")]
    [MaxLength(50)]
    public string Code { get; set; } = null!;
    [Required(ErrorMessage = "Pole wymagne")]
    [DisplayName("Nazwa operacji")]
    [MaxLength(250)]
    public string Name { get; set; } = null!;
}

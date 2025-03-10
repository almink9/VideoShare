using System.ComponentModel.DataAnnotations;

namespace VideoShare.ViewModels.Admin
{
  public class CAtegoryAddEdit_vm
  {
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
  }
}

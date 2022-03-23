using System.ComponentModel.DataAnnotations;
using CollectIt.MVC.Account.IdentityEntities;

namespace CollectIt.MVC.View.Models;

public class ImageViewModel
{
    [Required]
    public User Owner { get; set; }
    
    public DateTime UploadDate { get; set; }
    
    public string Path { get; set; }
}
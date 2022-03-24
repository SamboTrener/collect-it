using CollectIt.MVC.Account.Infrastructure.Data;
using CollectIt.MVC.Resources.Infrastructure;
using CollectIt.MVC.Resources.Infrastructure.Repositories;
using CollectIt.MVC.View.Models;
using Microsoft.AspNetCore.Mvc;

namespace CollectIt.MVC.View.Controllers;

[Route("resource")]
public class ResourceController : Controller
{
    private ImageRepository imageRepository;
    private ResourceRepository resourceRepository;
    
    public ResourceController(PostgresqlIdentityDbContext context)
    {
        imageRepository = new ImageRepository(context);
        resourceRepository = new ResourceRepository(context);
    }
    
    [HttpGet]
    [Route("resource")]
    public async Task<IActionResult> Resource(int id)
    {
        var source = imageRepository.FindByIdAsync(id).Result;
        var resOwner = resourceRepository.FindByIdAsync(source.Resource.ResourceId).Result.ResourceOwner;
        var imgModel = new ImageViewModel()
        {
            Owner = source.Resource.ResourceOwner,
            UploadDate = source.Resource.UploadDate,
            Path = source.Resource.ResourcePath
        };
        return View(imgModel);
    }
}


using System.IO;
using System.Reactive.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using MLambda.Image.Abstract;

namespace MLambda.Image.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class ImageController : ControllerBase
  {
    private readonly IImageService service;

    public ImageController(IImageService service)
    {
      this.service = service;
    }

    [Authorize(Roles = "store_admin")]
    [HttpDelete("{img}")]
    public async Task<bool> Delete(string img)
    {
      return await this.service.Delete(img);
    }

    [Authorize]
    [HttpPost("directory/create")]
    public async Task<bool> CreateDirectory(string name)
    {
      return await this.service.CreateDirectory(name);
    }

    [Authorize]
    [HttpPost("upload/{folder}")]
    public async Task<string> Upload(string folder, IFormFile file)
    {
      await using var memory = new MemoryStream();
      await file.CopyToAsync(memory);
      memory.Position = 0;
      return await this.service
        .CreateDirectory(folder)
        .SelectMany(c => this.service.Upload(folder, file.FileName, file.ContentType, memory.ToArray()));
    }
  }
}

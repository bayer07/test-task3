using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestTask3.Services;

namespace TestTask3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        public FilesController(FileService fileService)
        {
            _fileService = fileService;
        }

        private readonly FileService _fileService;

        [HttpGet]
        public IActionResult Get()
        {
            return new JsonResult(_fileService.GetAll());
        }

        [HttpPost]
        public IActionResult Post([FromForm] IFormFile customFile)
        {
            _fileService.Add(customFile);
            return Ok("Success!");
        }
    }
}

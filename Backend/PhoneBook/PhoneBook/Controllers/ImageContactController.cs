using Microsoft.AspNetCore.Mvc;
using PhoneBook.Application.ImageContactService;

namespace PhoneBook.API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/PhoneBook/[controller]/[action]")]
    [ApiController]
    public class ImageContactController : ControllerBase
    {
        private readonly IContactImageService imageContactService;
        public ImageContactController(IContactImageService imageContactService)
        {
            this.imageContactService = imageContactService;
        }

        // ------------------ //
        //     GET ACTION     //
        // ------------------ //


        [HttpGet("{fileId}")]
        public async Task<IFormFile> getFileByIdAsync(Guid fileId)
             => await imageContactService.GetFileById(fileId);

        [HttpGet("{path}")]
        public async Task<IFormFile> getFileByPathAsync(string path)
                   => await imageContactService.GetFileByPath(path);

        [HttpGet("{fileId}")]
        public async Task<string> getBase64ByIdAsync(Guid fileId)
        => await imageContactService.GetBase64ById(fileId);

        [HttpGet("{path}")]
        public async Task<string> getBase64ByPathAsync(string path)
            => await imageContactService.GetBase64ByPath(path);


        // --------------- //
        //   POST ACTION   //
        // --------------- //

        [HttpPost]
        public async Task<IActionResult> uploadFileToFolder()
        {
            if (!Request.HasFormContentType)
                return BadRequest();
            var form = Request.Form;
            Guid[] fileIds = new Guid[form.Files.Count];
            int index = 0;
            foreach (var file in form.Files)
            {
                if (file != null && file.Length > 0)
                {
                    fileIds[index] = await imageContactService.UploadFileToFolder(file);
                    index++;
                }
            }
            return Ok(fileIds);
        }
    }
}

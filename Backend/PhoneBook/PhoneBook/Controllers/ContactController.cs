using Microsoft.AspNetCore.Mvc;
using PhoneBook.Application.ContactService;

namespace PhoneBook.API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/PhoneBook/[controller]/[action]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IContactService contactService;
        public ContactController(IContactService contactService)
        {
            this.contactService = contactService;
        }
    }
}

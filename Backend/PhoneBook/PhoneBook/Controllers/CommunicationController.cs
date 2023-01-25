using Microsoft.AspNetCore.Mvc;
using PhoneBook.Application.CommunicationService;

namespace PhoneBook.API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/PhoneBook/[controller]/[action]")]
    [ApiController]
    public class CommunicationController : ControllerBase
    {
        private readonly ICommunicationService communicationService;
        public CommunicationController(ICommunicationService communicationService)
        {
            this.communicationService = communicationService;
        }
    }
 }

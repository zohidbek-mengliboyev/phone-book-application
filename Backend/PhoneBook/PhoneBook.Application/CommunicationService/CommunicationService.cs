using PhoneBook.Domain.Models;

namespace PhoneBook.Application.CommunicationService
{
    public class CommunicationService : ICommunicationService
    {
        public Task<CommunicationDTO> GetCommunicationByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Guid> UpsertCommunicationAsync(CommunicationDTO modelDTO)
        {
            throw new NotImplementedException();
        }
    }
}

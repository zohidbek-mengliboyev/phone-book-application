using PhoneBook.Domain.Models;

namespace PhoneBook.Application.CommunicationService
{
    public interface ICommunicationService
    {
        Task<Guid> UpsertCommunicationAsync(CommunicationDTO modelDTO);
        Task<CommunicationDTO> GetCommunicationByIdAsync(Guid id);
    }
}

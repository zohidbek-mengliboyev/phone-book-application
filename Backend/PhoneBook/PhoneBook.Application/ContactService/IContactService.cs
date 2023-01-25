using PhoneBook.Domain.Models;

namespace PhoneBook.Application.ContactService
{
    public interface IContactService
    {
        Task<Guid> UpsertContactAsync(ContactDTO modelDTO);
        Task<ContactDTO> GetCommunicationByIdAsync(Guid id);
    }
}

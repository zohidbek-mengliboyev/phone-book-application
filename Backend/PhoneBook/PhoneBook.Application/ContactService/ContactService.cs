using PhoneBook.Domain.Models;

namespace PhoneBook.Application.ContactService
{
    public class ContactService : IContactService
    {
        public Task<ContactDTO> GetCommunicationByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Guid> UpsertContactAsync(ContactDTO modelDTO)
        {
            throw new NotImplementedException();
        }
    }
}

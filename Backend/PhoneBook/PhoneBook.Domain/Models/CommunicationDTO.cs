using PhoneBook.Domain.Entities;

namespace PhoneBook.Domain.Models
{
    public class CommunicationDTO
    {
        public Guid Id { get; set; }
        public InformationType InformationType { get; set; }
        public string? InformationContent { get; set; }
    }
}

namespace PhoneBook.Domain.Models
{
    public class ContactDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int ImageContactId { get; set; }
        public string Surname { get; set; }
        public string Company { get; set; }
        public Guid CommunicationId { get; set; }
    }
}

namespace PhoneBook.Domain.Entities
{
    public class Contact
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int ImageContactId { get; set; }
        public string Surname { get; set; }
        public string Company { get; set; }

        public Guid CommunicationId { get; set; }
        public virtual Communication Communication { get; set; }
    }
}

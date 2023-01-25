namespace PhoneBook.Domain.Entities
{
    public class Communication
    {
        public int Id { get; set; }
        public InformationType InformationType { get; set; }
        public string? InformationContent { get; set; }

        public virtual ICollection<Contact> Contacts { get; set; }
    }
}

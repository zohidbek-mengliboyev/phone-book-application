namespace PhoneBook.Domain.Entities
{
    public class Communication : EntityBase
    {
        public InformationType InformationType { get; set; }
        public string? InformationContent { get; set; }

        public virtual ICollection<Contact> Contacts { get; set; }
    }
}

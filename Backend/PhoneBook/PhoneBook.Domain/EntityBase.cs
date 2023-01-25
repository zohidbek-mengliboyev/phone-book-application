namespace PhoneBook.Domain
{
    public abstract class EntityBase
    {
        public Guid Id { get; set; }

        public bool Active { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        public void SetActive()
        {
            this.Active = true;
        }
    }
}

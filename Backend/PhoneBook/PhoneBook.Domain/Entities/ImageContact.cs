namespace PhoneBook.Domain.Entities
{
    public class ImageContact
    {
        public int Id { get; set; }
        public string ContentPath { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public long ContentLength { get; set; }
    }
}

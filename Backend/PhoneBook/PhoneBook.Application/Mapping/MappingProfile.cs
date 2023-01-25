using AutoMapper;
using PhoneBook.Domain.Entities;
using PhoneBook.Domain.Models;

namespace PhoneBook.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<string, int>().ConvertUsing<IntTypeConverter>();
            CreateMap<string, int?>().ConvertUsing<NullIntTypeConverter>();

            CreateMap<Contact, ContactDTO>().ReverseMap();
            CreateMap<ImageContact, ImageContactDTO>().ReverseMap();
            CreateMap<Communication, CommunicationDTO>().ReverseMap();
        }


        #region AutoMapTypeConverters
        private class NullIntTypeConverter : ITypeConverter<string, int?>
        {
            public int? Convert(string source, int? destination, ResolutionContext context)
            {
                if (source == null)
                    return null;
                else
                {
                    int result;
                    return Int32.TryParse(source, out result) ? (int?)result : null;
                }
            }
        }
        // Automapper string to int
        private class IntTypeConverter : ITypeConverter<string, int>
        {
            public int Convert(string source, int destination, ResolutionContext context)
            {
                if (string.IsNullOrEmpty(source))
                    return 0;
                else
                    return Int32.Parse(source);
            }
        }
        #endregion
    }
}

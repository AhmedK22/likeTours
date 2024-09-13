using AutoMapper;
using LikeTours.Data.DTO.AboutUs;
using LikeTours.Data.DTO.Contact;
using LikeTours.Data.DTO.Package;
using LikeTours.Data.DTO.Payment;
using LikeTours.Data.DTO.Place;
using LikeTours.Data.DTO.Question;
using LikeTours.Data.DTO.Review;
using LikeTours.Data.DTO.Type;
using LikeTours.Data.Models;


namespace LikeTours.AutoMapper
{
    public class MappingProfile : Profile
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string baseUrl;
        public MappingProfile(IHttpContextAccessor httpContextAccessor)
        {
            this._httpContextAccessor = httpContextAccessor;
            this.baseUrl = $"{this._httpContextAccessor.HttpContext.Request.Scheme}://{this._httpContextAccessor.HttpContext.Request.Host}";

        }
        public MappingProfile()
        {
           
            CreateMap<Place, PlaceDto>()
                 .ForMember(dest => dest.Image, opt => opt.MapFrom(src => $"{this.baseUrl}{src.Image}"))
                  .ForMember(dest => dest.RefferencePlaces, opt => opt.MapFrom(src => src.RefferencePlaces));

            CreateMap<PlaceDto, Place>()
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => $"{this.baseUrl}{src.Image}"))
                 .ForMember(dest => dest.RefferencePlaces, opt => opt.MapFrom(src => src.RefferencePlaces));


            CreateMap<PlaceCreateDto, Place>();

            CreateMap<TourType, TypeDto>()
                 .ForMember(dest => dest.RefferenceTypes, opt => opt.MapFrom(src => src.RefferenceTypes));

            CreateMap<CreateTypeDto, TourType>();

            CreateMap<CreatePackageDto, Package>()
               .ForMember(dest => dest.RefferencePackages, opt => opt.Ignore())
             .ForMember(dest => dest.Sections, opt => opt.Ignore())
               .ForMember(dest => dest.Images, opt => opt.Ignore());

            CreateMap<Package, PackageDto>()
                 .ForMember(dest => dest.RefferencePackages, opt => opt.MapFrom(src => src.RefferencePackages))

            .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images.Select(img => $"{this.baseUrl}{img.Url}")))
            .ForMember(dest => dest.Place, opt => opt.MapFrom(src => src.Place))
            .ForMember(dest => dest.TourType, opt => opt.MapFrom(src => src.TourType))
            .ForMember(dest => dest.Sections, opt => opt.MapFrom(src => src.Sections)).ReverseMap();


            CreateMap<SectionDto, Section>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // Ignore Id during mapping as it's auto-generated
                .ForMember(dest => dest.Package, opt => opt.Ignore())
                .ReverseMap(); ;


            CreateMap<Contact, ContactDto>().ReverseMap(); ;
            CreateMap<CreateContactDto, Contact>()
             .ForMember(dest => dest.Id, opt => opt.Ignore())
             .ReverseMap();


            CreateMap<Questions, QuestionDto>().ReverseMap(); ;
            CreateMap<CreateQuestionDto, Questions>()
             .ForMember(dest => dest.Id, opt => opt.Ignore())
             .ReverseMap();

            CreateMap<Review, ReviewDto>().ReverseMap(); ;
            CreateMap<CreateReviewDto, Review>()
             .ForMember(dest => dest.Id, opt => opt.Ignore())
             .ReverseMap();

            CreateMap<Payment, PaymentDto>().ReverseMap(); ;
            CreateMap<CreatePaymentDto, Payment>()
             .ForMember(dest => dest.Id, opt => opt.Ignore())
             .ReverseMap();


            CreateMap<About, AboutUsDto>().ReverseMap(); ;
            CreateMap<CreateAboutUsDto, About>()
             .ForMember(dest => dest.Id, opt => opt.Ignore())
             .ReverseMap();

            CreateMap<UpdateAboutUsDto, About>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ReverseMap();

        }
    }
 
}

using AutoMapper;
using LikeTours.Contracts.Repositories;
using LikeTours.Contracts.Services;
using LikeTours.Data;
using LikeTours.Data.DTO;
using LikeTours.Data.DTO.AboutUs;
using LikeTours.Data.Enums;
using LikeTours.Data.Models;
using LikeTours.Exceptions.aboutUs;
using System.Linq.Expressions;

namespace LikeTours.Services
{
    public class AboutUsService : IAboutUsService
    {
        private readonly IGenericRepository<About> _aboutRepository;
        private readonly IMapper _mapper;
        private readonly string _imageStoragePath;
        private readonly ImageService _imageService;
        private readonly AppDbContext _context;


        public AboutUsService(IConfiguration configuration, IMapper mapper, AppDbContext context, ImageService imageService, IGenericRepository<About> aboutRepository)
        {

            _mapper = mapper;
            _imageStoragePath = configuration["ImageStoragePath"];
            _context = context;
            _imageService = imageService;
            _aboutRepository = aboutRepository;
        }

        public async Task<ApiResponse<PaginationDto<AboutUsDto>>> GetAllAsync(AboutUsQueryParams param, OrderType orderType = OrderType.Descending)
        {
            Expression<Func<About, bool>> filter = p =>
             (string.IsNullOrEmpty(param.Lang) || p.Lang == param.Lang);
            

            var abouts = await _aboutRepository.GetAllAsync(param, filter, null, orderType);
            var aboutsDtos = _mapper.Map<IEnumerable<AboutUsDto>>(abouts);

            int totalCount = await _aboutRepository.CountAsync(filter); // Use the same filter for counting
            PaginationDto<AboutUsDto> paginationDto = new PaginationDto<AboutUsDto>()
            {
                Page = param.Page,
                PageSize = param.PageSize,
                Total = totalCount,
                Items = aboutsDtos
            };

            return new ApiResponse<PaginationDto<AboutUsDto>>(paginationDto);
        }




        public async Task<AboutUsDto> GetByIdAsync(int id, string Lang)
        {

            var about = await _aboutRepository.GetByIdAsync(id, includes: p => p.RefferenceAbout);
            if (about == null)
            {
                throw new AboutNotFoundException($"about with id {id} not exist");
            }
            Expression<Func<About, bool>> filter = null;
            About aboutLang;

            if (about.Lang != Lang)
            {
                if (about.Main)
                {
                    filter = p =>
                      (string.IsNullOrEmpty(Lang) || p.Lang == Lang)
                      &&
                      (p.AboutId == about.Id);
                }
                else
                {
                    filter = p =>
                    (string.IsNullOrEmpty(Lang) || p.Lang == Lang)
                     && (p.AboutId == about.AboutId);
                }
                var abouts = await _aboutRepository.GetByColumnsAsync(filter, includes: p => p.RefferenceAbout);
                aboutLang = abouts.FirstOrDefault();
                if (aboutLang == null)
                {
                    throw new AboutNotFoundException($"no abouts with lang {Lang} for id {id} exist");
                }
            }
            else
            {
                aboutLang = about;
            }


            var AboutUsDto = _mapper.Map<AboutUsDto>(aboutLang);

            return AboutUsDto;
        }
        public async Task<string> SaveImageAsync(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
                throw new ArgumentException("Invalid image file");

            var filePath = Path.Combine(_imageStoragePath, Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName));

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return filePath;
        }
        public async Task<int> AddAsync(CreateAboutUsDto CreateAboutUsDto)
        {
            string MainImage = null;
            string WhoAreImage = null;

            if (CreateAboutUsDto.Main)
            {
                CreateAboutUsDto.AboutId = null;
            }
            if (CreateAboutUsDto.MainImage != null)
            {
                MainImage = await SaveImageAsync(CreateAboutUsDto.MainImage);

            }
            if (CreateAboutUsDto.WhoAreImage != null)
            {
                WhoAreImage = await SaveImageAsync(CreateAboutUsDto.WhoAreImage);

            }


            var about = _mapper.Map<About>(CreateAboutUsDto);
           

            about.MainImage = MainImage;
            about.WhoAreImage = WhoAreImage;
            
            return await _aboutRepository.AddAsync(about);
        }

        public async Task UpdateAsync(UpdateAboutUsDto AboutUsDto)
        {
            string MainImage = null;
            string WhoAreImage = null;
            var existingabout = await _aboutRepository.GetByIdAsync(AboutUsDto.Id);
           
            if (existingabout == null)
            {
                throw new AboutNotFoundException($"about with id {AboutUsDto.Id} not found.");
            }

         

            if (existingabout.Main && !AboutUsDto.Main)
            {
                Expression<Func<About, bool>> filter = p => (p.AboutId == existingabout.Id);
                var checkMainPackageAssign = await _aboutRepository.GetByColumnsAsync(filter);
                if (checkMainPackageAssign.Count() > 0)
                {
                    throw new AssignedMainAboutException($"this is main about that assigned to another abouts so remove assighnes abouts first");
                }
            }

            if (AboutUsDto.MainImage != null)
            {
                MainImage = await SaveImageAsync(AboutUsDto.MainImage);

            }
            if (AboutUsDto.WhoAreImage != null)
            {
                WhoAreImage = await SaveImageAsync(AboutUsDto.WhoAreImage);

            }

            var aboutToUpdate = _mapper.Map(AboutUsDto, existingabout);
            if (MainImage != null)
            {
                _imageService.DeleteImageFile(existingabout.MainImage);
              

                aboutToUpdate.MainImage = MainImage;
            }
            if (WhoAreImage != null)
            {
                _imageService.DeleteImageFile(existingabout.WhoAreImage);

                aboutToUpdate.MainImage = MainImage;
            }
            await _aboutRepository.UpdateAsync(aboutToUpdate);

        }

        public async Task DeleteAsync(int id)
        {
            var existingabout = await _aboutRepository.GetByIdAsync(id);

            if (existingabout == null)
            {
                throw new AboutNotFoundException($"about with id {id} not found.");
            }

           

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    _imageService.DeleteImageFile(existingabout.MainImage);
                    _imageService.DeleteImageFile(existingabout.WhoAreImage);
                    await _aboutRepository.DeleteAsync(existingabout);
                    await transaction.CommitAsync();

                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                }
            }

        }
    }
}


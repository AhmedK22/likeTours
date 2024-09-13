using AutoMapper;
using LikeTours.Contracts.Repositories;
using LikeTours.Contracts.Services;
using LikeTours.Data;
using LikeTours.Data.DTO;
using LikeTours.Data.DTO.Place;
using LikeTours.Data.DTO.Review;
using LikeTours.Data.Enums;
using LikeTours.Data.Models;
using LikeTours.Exceptions;
using LikeTours.Exceptions.packages;
using LikeTours.Exceptions.Places;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LikeTours.Services
{
    public class PlaceService : IPlaceService
    {
        private readonly IGenericRepository<Place> _placeRepository;
        private readonly IMapper _mapper;
        private readonly string _imageStoragePath;
        private readonly ImageService _imageService;
        private readonly AppDbContext _context;


        public PlaceService(IGenericRepository<Place> placeRepository, IConfiguration configuration, IMapper mapper, AppDbContext context, ImageService imageService)
        {
            _placeRepository = placeRepository;
            _mapper = mapper;
            _imageStoragePath = configuration["ImageStoragePath"];
            _context = context;
            _imageService = imageService;
        }

        public async Task<ApiResponse<PaginationDto<PlaceDto>>> GetAllAsync(PlaceQueryParams param, OrderType orderType = OrderType.Descending)
        {
            Expression<Func<Place, bool>> filter = p =>
                (string.IsNullOrEmpty(param.Lang) || p.Lang == param.Lang) &&
                (string.IsNullOrEmpty(param.Name) || p.Name.Contains(param.Name));

            var places = await _placeRepository.GetAllAsync(param, filter, null, orderType);
            var placesDtos = _mapper.Map<IEnumerable<PlaceDto>>(places);

            int totalCount = await _placeRepository.CountAsync(filter); // Use the same filter for counting
            PaginationDto<PlaceDto> paginationDto = new PaginationDto<PlaceDto>()
            {
                Page = param.Page,
                PageSize = param.PageSize,
                Total = totalCount,
                Items = placesDtos
            };

            return new ApiResponse<PaginationDto<PlaceDto>>(paginationDto);
        }


        public async Task<ApiResponse<IEnumerable<PlaceDto>>> GetDataAsync(string? Name = null,OrderType orderType = OrderType.Descending)
        {
            var param = new PlaceQueryParams()
            {
                HasPagination = false,
            };

            Expression<Func<Place, bool>> filter = null;

            if(Name != null )
            {
                filter = p =>
             
               (string.IsNullOrEmpty(Name) || p.Name.Contains(Name));
            }

            var places = await _placeRepository.GetAllAsync(param, filter, null, orderType);
            var placesDto = _mapper.Map<IEnumerable<PlaceDto>>(places);
            var result = new ApiResponse<IEnumerable<PlaceDto>>(placesDto);

            return result;
        }


        public async Task<PlaceDto> GetByIdAsync(int id, string Lang)
        {
          
            var place = await _placeRepository.GetByIdAsync(id,includes:p=>p.RefferencePlaces);
            if (place == null)
            {
                throw new PlaceNotFoundException($"place with id {id} not exist");
            }
            Expression<Func<Place, bool>> filter = null;
            Place placeLang;

            if (place.Lang != Lang) 
            {
                if (place.Main) {
                   filter = p =>
                     (string.IsNullOrEmpty(Lang) || p.Lang == Lang)
                     &&
                     (p.PlaceId == place.Id);
                }
                else
                {
                    filter = p =>
                    (string.IsNullOrEmpty(Lang) || p.Lang == Lang)
                     && (p.PlaceId == place.PlaceId);
                }
                var places = await _placeRepository.GetByColumnsAsync(filter, includes: p => p.RefferencePlaces);
                placeLang = places.FirstOrDefault();
                if(placeLang == null)
                {
                    throw new PlaceNotFoundException($"no places with lang {Lang} for id {id} exist");
                }
            }
            else
            {
                placeLang = place;
            }


            var placeDTO = _mapper.Map<PlaceDto>(placeLang);
           
            return placeDTO;
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
        public async Task<int> AddAsync(PlaceCreateDto placeCreateDto)
        {
            var exists = await _placeRepository.GetByColumnsAsync(p => p.Name == placeCreateDto.Name);
            string imageUrl = null;
            if (exists.Count() > 0 )
            {
                throw new DuplicatePlaceException($"Place with name {placeCreateDto.Name} already Exist");
            }
            if(placeCreateDto.Main)
            {
                placeCreateDto.PlaceId = null;
            }
            if (placeCreateDto.Image != null)
            {
                imageUrl = await SaveImageAsync(placeCreateDto.Image);

            }


            var place = _mapper.Map<Place>(placeCreateDto);
            if (imageUrl != null)
            {

                place.Image = imageUrl;
            }
            return await _placeRepository.AddAsync(place); 
        }

        public async Task UpdateAsync(PlaceCreateDto placeDto)
        {
            var existingPlace = await _placeRepository.GetByIdAsync(placeDto.Id);
            string imageUrl= null;
            string deletedimage= null;
            if (existingPlace == null)
            {
                throw new PlaceNotFoundException($"Place with id {placeDto.Id} not found.");
            }
            deletedimage = existingPlace?.Image;

            var nameExists = await _placeRepository.GetByColumnsAsync(p => p.Name == placeDto.Name && p.Id != placeDto.Id);
            if (nameExists.Any())
            {
                throw new DuplicatePlaceException($"Place with name {placeDto.Name} already exists.");
            }

            if (existingPlace.Main && !placeDto.Main)
            {
                Expression<Func<Place, bool>> filter = p => (p.PlaceId == existingPlace.Id);
                var checkMainPackageAssign = await _placeRepository.GetByColumnsAsync(filter);
                if (checkMainPackageAssign.Count() > 0)
                {
                    throw new AssignedMainPlaceException($"this is main place that assigned to another places so remove assighnes places first");
                }
            }

            if (placeDto.Image != null)
            {
                imageUrl = await SaveImageAsync(placeDto.Image);

            }

            var placeToUpdate = _mapper.Map(placeDto, existingPlace);
            if (imageUrl != null)
            {
                _imageService.DeleteImageFile(deletedimage);
                placeToUpdate.Image = imageUrl;
            }
            else
            {
                _imageService.DeleteImageFile(deletedimage);
            }

            await _placeRepository.UpdateAsync(placeToUpdate);

        }

        public async Task DeleteAsync(int id)
        {
            var existingPlace = await _placeRepository.GetByIdAsync(id);

            if (existingPlace == null)
            {
                throw new PlaceNotFoundException($"Place with id {id} not found.");
            }

            Expression<Func<Place,bool>> filter = p => (p.PlaceId == existingPlace.Id);
            var places = await _placeRepository.GetByColumnsAsync(filter);
           
            if (places.Count() > 0)
            {
                throw new ConstraintPlaceErrorException($"this place is refference to another types so delete other first");
            }

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    _imageService.DeleteImageFile(existingPlace.Image);
                    await _placeRepository.DeleteAsync(existingPlace);
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

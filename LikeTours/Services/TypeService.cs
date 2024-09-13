using AutoMapper;
using LikeTours.Contracts.Repositories;
using LikeTours.Contracts.Services;
using LikeTours.Data.DTO;
using LikeTours.Data.DTO.Place;
using LikeTours.Data.DTO.Type;
using LikeTours.Data.Enums;
using LikeTours.Data.Models;
using LikeTours.Exceptions;
using LikeTours.Exceptions.Places;
using LikeTours.Exceptions.Types;
using System.Linq.Expressions;

namespace LikeTours.Services
{
    public class TypeService : ITypeService
    {
        private readonly IGenericRepository<TourType> _typeRepository;
        private readonly IMapper _mapper;

        public TypeService(IGenericRepository<TourType> typeRepository, IMapper mapper)
        {
            _typeRepository = typeRepository;
            _mapper = mapper;
        }
        public async Task<int> AddAsync(CreateTypeDto CreateTypeDto)
        {
            var exists = await _typeRepository.GetByColumnsAsync(p => p.Name == CreateTypeDto.Name);

            if (exists.Count() > 0)
            {
                throw new DuplicateTypeException($"Type with name {CreateTypeDto.Name} already Exist");
            }

            var tourType = _mapper.Map<TourType>(CreateTypeDto);
            if(tourType.Main) { 
                tourType.TourTypeId = null;
            }
            return await _typeRepository.AddAsync(tourType);
        }

        public async Task DeleteAsync(int id)
        {
            var existingType = await _typeRepository.GetByIdAsync(id);

            if (existingType == null)
            {
                throw new TypeNotFoundException($"type with id {id} not found.");
            }

            Expression<Func<TourType, bool>> filter = p => (p.TourTypeId == existingType.Id);
            var types = await _typeRepository.GetByColumnsAsync(filter);

            if (types.Count() > 0)
            {
                throw new ConstraintTypeErrorException($"this type is refference to another types so delete other first");
            }
            await _typeRepository.DeleteAsync(existingType);
        }

        public async Task<ApiResponse<PaginationDto<TypeDto>>> GetAllAsync(TypeQueryParam param, OrderType orderType = OrderType.Descending)
        {
            Expression<Func<TourType, bool>> filter = p =>
                (string.IsNullOrEmpty(param.Lang) || p.Lang == param.Lang) &&
                (string.IsNullOrEmpty(param.Name) || p.Name.Contains(param.Name));

            var types = await _typeRepository.GetAllAsync(param, filter, null, orderType);
            var typesDtos = _mapper.Map<IEnumerable<TypeDto>>(types);

            int totalCount = await _typeRepository.CountAsync(filter);
            PaginationDto<TypeDto> paginationDto = new PaginationDto<TypeDto>()
            {
                Page = param.Page,
                PageSize = param.PageSize,
                Total = totalCount,
                Items = typesDtos
            };

            return new ApiResponse<PaginationDto<TypeDto>>(paginationDto);
        }

        public async Task<ApiResponse<IEnumerable<TypeDto>>> GetDataAsync(string? Name = null, OrderType orderType = OrderType.Descending)
        {
            var param = new TypeQueryParam()
            {
                HasPagination = false,
            };

            Expression<Func<TourType, bool>> filter = null;

            if (Name != null)
            {
                filter = p =>

               (string.IsNullOrEmpty(Name) || p.Name.Contains(Name));
            }

            var types = await _typeRepository.GetAllAsync(param, filter, null, orderType);
            var typesDto = _mapper.Map<IEnumerable<TypeDto>>(types);
            var result = new ApiResponse<IEnumerable<TypeDto>>(typesDto);

            return result;
        }



        public async Task<TypeDto> GetByIdAsync(int id, string Lang)
        {
            var tourType = await _typeRepository.GetByIdAsync(id,includes:t=>t.RefferenceTypes);
            if (tourType == null)
            {
                throw new TypeNotFoundException($"type with id {id} not exist");
            }
            Expression<Func<TourType, bool>> filter = null;
            TourType typeLang;

            if (tourType.Lang != Lang)
            {
                if (tourType.Main)
                {
                    filter = p =>
                      (string.IsNullOrEmpty(Lang) || p.Lang == Lang)
                      &&
                      (p.TourTypeId == tourType.Id);
                }
                else
                {
                    filter = p =>
                    (string.IsNullOrEmpty(Lang) || p.Lang == Lang)
                     && (p.TourTypeId == tourType.TourTypeId);
                }
                var tourTypes = await _typeRepository.GetByColumnsAsync(filter, includes: t => t.RefferenceTypes);
                typeLang = tourTypes.FirstOrDefault();
                if (typeLang == null)
                {
                    throw new TypeNotFoundException($"no type with lang {Lang} for id {id} exist");
                }
            }
            else
            {
                typeLang = tourType;
            }

            var tourTypeDto = _mapper.Map<TypeDto>(typeLang);

            return tourTypeDto;
        }

        public async Task UpdateAsync(CreateTypeDto CreateTypeDto)
        {
            var existingType = await _typeRepository.GetByIdAsync(CreateTypeDto.Id);

            if (existingType == null)
            {
                throw new TypeNotFoundException($"type with id {CreateTypeDto.Id} not found.");
            }
            var nameExists = await _typeRepository.GetByColumnsAsync(p => p.Name == CreateTypeDto.Name && p.Id != CreateTypeDto.Id);
            if (nameExists.Any())
            {
                throw new DuplicateTypeException($"type with name {CreateTypeDto.Name} already exists.");
            }

            if (existingType.Main && !CreateTypeDto.Main)
            {
                Expression<Func<TourType, bool>> filter = p => (p.TourTypeId == existingType.Id);
                var checkMainPackageAssign = await _typeRepository.GetByColumnsAsync(filter);
                if (checkMainPackageAssign.Count() > 0)
                {
                    throw new AssignedMainTypeException($"this is main type that assigned to another type so remove assighnes types first");
                }
            }


            var typeToUpdate = _mapper.Map(CreateTypeDto, existingType);

            await _typeRepository.UpdateAsync(typeToUpdate);
        }
    }
}

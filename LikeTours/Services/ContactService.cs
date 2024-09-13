using AutoMapper;
using LikeTours.Contracts.Repositories;
using LikeTours.Contracts.Services;
using LikeTours.Data.DTO;
using LikeTours.Data.DTO.Contact;
using LikeTours.Data.DTO.Place;
using LikeTours.Data.Enums;
using LikeTours.Data.Models;
using LikeTours.Exceptions.Contacts;
using LikeTours.Exceptions.Places;
using System.Linq.Expressions;


namespace LikeTours.Services
{
    public class ContactService : IContactService
    {
        private readonly IGenericRepository<Contact> _contactRepository;
        private readonly IMapper _mapper;

        public ContactService(IGenericRepository<Contact> contactRepository, IMapper mapper)
        {
            _contactRepository = contactRepository;
            _mapper = mapper;
        }

        public async Task<int> AddAsync(CreateContactDto CreateContactDto)
        {
        
            var contact = _mapper.Map<Contact>(CreateContactDto);
            return await _contactRepository.AddAsync(contact);
        }

        public async Task DeleteAsync(int id)
        {
            var exist = await _contactRepository.GetByIdAsync(id);

            if (exist == null)
            {
                throw new ContactNotFoundException($"contact with id {id} not found.");
            }

            await _contactRepository.DeleteAsync(exist);
        }

        public async Task<ApiResponse<PaginationDto<ContactDto>>> GetAllAsync(ContactQueryParam param, OrderType orderType = OrderType.Descending)
        {
            Expression<Func<Contact, bool>> filter = p =>
             
               (string.IsNullOrEmpty(param.Name) || p.Name.Contains(param.Name));

            var contacts = await _contactRepository.GetAllAsync(param, filter, null, orderType);
            var contactDtos = _mapper.Map<IEnumerable<ContactDto>>(contacts);

            int totalCount = await _contactRepository.CountAsync(filter); 
            PaginationDto<ContactDto> paginationDto = new PaginationDto<ContactDto>()
            {
                Page = param.Page,
                PageSize = param.PageSize,
                Total = totalCount,
                Items = contactDtos
            };

            return new ApiResponse<PaginationDto<ContactDto>>(paginationDto);
        }

        public async Task<ContactDto> GetByIdAsync(int id)
        {
            var contact = await _contactRepository.GetByIdAsync(id);
            if (contact == null)
            {
                throw new ContactNotFoundException($"contact with id {id} not exist");
            }
          
            var contactDto = _mapper.Map<ContactDto>(contact);

            return contactDto;
        }

        public async Task<ApiResponse<IEnumerable<ContactDto>>> GetDataAsync(string? Name = null, OrderType orderType = OrderType.Descending)
        {
            var param = new ContactQueryParam()
            {
                HasPagination = false,
            };

            Expression<Func<Contact, bool>> filter = null;

            if (Name != null)
            {
                filter = p =>

               (string.IsNullOrEmpty(Name) || p.Name.Contains(Name));
            }

            var Contacts = await _contactRepository.GetAllAsync(param, filter, null, orderType);
            var ContactDtos = _mapper.Map<IEnumerable<ContactDto>>(Contacts);
            var result = new ApiResponse<IEnumerable<ContactDto>>(ContactDtos);

            return result;
        }

        public async Task UpdateAsync(CreateContactDto CreateContactDto)
        {
            var exist = await _contactRepository.GetByIdAsync(CreateContactDto.Id);

            if (exist == null)
            {
                throw new ContactNotFoundException($"contact with id {CreateContactDto.Id} not found.");
            }
          

            var updatedContact = _mapper.Map(CreateContactDto, exist);

            await _contactRepository.UpdateAsync(updatedContact);

        }
    }
}

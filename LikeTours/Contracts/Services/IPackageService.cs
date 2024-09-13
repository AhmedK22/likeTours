using LikeTours.Data.DTO;
using LikeTours.Data.DTO.Package;
using LikeTours.Data.Enums;
using LikeTours.Data.Models;

namespace LikeTours.Contracts.Services
{
    public interface IPackageService
    {
        Task<ApiResponse<PaginationDto<PackageDto>>> GetAllPackagesAsync(PackageQueryParam param, OrderType orderType = OrderType.Descending);
     
        Task<PackageDto> GetPackageByIdAsync(int id, string Lang);
        Task<int> AddPackageAsync(Package package, IEnumerable<IFormFile> imageFiles, string sections); // Modified to accept sections
        Task UpdatePackageAsync(CreatePackageDto package, IEnumerable<IFormFile> imageFiles, string sections); // Modified to accept sections
        Task DeletePackageAsync(int id);
        Task<string> SaveImageAsync(IFormFile imageFile);
        Task<IEnumerable<string>> SaveImagesAsync(IEnumerable<IFormFile> imageFiles);
        Task AddImagesToPackageAsync(int packageId, IEnumerable<string> imageUrls);
        Task AddSectionsToPackageAsync(int packageId, IEnumerable<Section> sections); // New method for adding sections to a package
        Task AddSalePackageAsync(int id, int SaleAmmount, string SaleType);
    }


}

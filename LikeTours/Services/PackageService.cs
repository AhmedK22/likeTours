using AutoMapper;
using LikeTours.Contracts.Repositories;
using LikeTours.Contracts.Services;
using LikeTours.Data;
using LikeTours.Data.DTO;
using LikeTours.Data.DTO.Package;
using LikeTours.Data.Enums;
using LikeTours.Data.Models;
using LikeTours.Exceptions.packages;
using LikeTours.Exceptions.Places;
using System;
using System.Data;
using System.Linq.Expressions;
using System.Text.Json;
using Section = LikeTours.Data.Models.Section;

namespace LikeTours.Services
{
    public class PackageService : IPackageService
    {
        private readonly IGenericRepository<Package> _packageRepository;
        private readonly IGenericRepository<Image> _imageRepository;
        private readonly IGenericRepository<Section> _sectionRepository;
        private readonly AppDbContext _context;
        private readonly string _imageStoragePath;
        private readonly IMapper _mapper;
        private readonly ImageService _imageService;

        public PackageService(IGenericRepository<Package> packageRepository, IGenericRepository<Image> imageRepository, IGenericRepository<Section> sectionRepository, AppDbContext context, IConfiguration configuration, IMapper mapper, ImageService imageService)
        {
            _packageRepository = packageRepository;
            _imageRepository = imageRepository;
            _sectionRepository = sectionRepository;
            _context = context;
            _imageStoragePath = configuration["ImageStoragePath"];
            _mapper = mapper;
            _imageService = imageService;
        }

     

        public async Task<ApiResponse<PaginationDto<PackageDto>>> GetAllPackagesAsync(PackageQueryParam param, OrderType orderType = OrderType.Descending)
        {

            Expression<Func<Package, bool>> filter = p =>
            (string.IsNullOrEmpty(param.Lang) || p.Lang == param.Lang) &&
            (string.IsNullOrEmpty(param.Title) || p.Title.Contains(param.Title)) &&
            (!param.PlaceId.HasValue || p.PlaceId == param.PlaceId) &&
            (!param.TourTypeId.HasValue || p.TourTypeId == param.TourTypeId) &&
            (!param.HasSale.HasValue || p.HasSale == param.HasSale) &&
            (string.IsNullOrEmpty(param.SaleType) || p.SaleType == param.SaleType) &&
            (!param.SaleAmount.HasValue || p.SaleAmount == param.SaleAmount);

            Func<IQueryable<Package>, IOrderedQueryable<Package>> orderBy = query =>
            {
                return orderType == OrderType.Descending
                    ? query.OrderByDescending(p => p.PlaceId).ThenByDescending(p => p.TourTypeId)
                    : query.OrderBy(p => p.PlaceId).ThenBy(p => p.TourTypeId);
            };

            var packages = await _packageRepository.GetAllAsync(param, filter, orderBy, includes: new Expression<Func<Package, object>>[]
            {
                p => p.Place,
                p => p.TourType,
                p => p.Images,
                p => p.Sections
            });


          
            var packageDtos = _mapper.Map<IEnumerable<PackageDto>>(packages);

            int totalCount = await _packageRepository.CountAsync(filter); // Use the same filter for counting
            PaginationDto<PackageDto> paginationDto = new PaginationDto<PackageDto>()
            {
                Page = param.Page,
                PageSize = param.PageSize,
                Total = totalCount,
                Items = packageDtos
            };

            return new ApiResponse<PaginationDto<PackageDto>>(paginationDto);
        }

        public async Task<PackageDto> GetPackageByIdAsync(int id, string Lang)
        {
            var package = await _packageRepository.GetByIdAsync(id, includes: new Expression<Func<Package, object>>[]
            {
                p => p.Place,
                p => p.TourType,
                p => p.Images,
                p => p.Sections,
                p => p.RefferencePackages

            });
            if (package == null)
            {
                throw new PackageNotFoundException($"package with id {id} not exist");
            }
            Expression<Func<Package, bool>> filter = null;
            Package packageLang;

            if (package.Lang != Lang)
            {
                if (package.Main)
                {
                    filter = p =>
                      (string.IsNullOrEmpty(Lang) || p.Lang == Lang)
                      &&
                      (p.PackageId == package.Id);
                }
                else
                {
                    filter = p =>
                    (string.IsNullOrEmpty(Lang) || p.Lang == Lang)
                     && (p.PackageId == package.PackageId);
                }
                var packages = await _packageRepository.GetByColumnsAsync(filter, includes: new Expression<Func<Package, object>>[]
                {
                    p => p.Place,
                    p => p.TourType,
                    p => p.Images,
                    p => p.Sections,
                    p => p.RefferencePackages
                });

                packageLang = packages.FirstOrDefault();
                if (packageLang == null)
                {
                    throw new PlaceNotFoundException($"no package with lang {Lang} for id {id} exist");
                }
            }
            else
            {
                packageLang = package;
            }


            var packageDTO = _mapper.Map<PackageDto>(packageLang);

            return packageDTO;
        }

        public async Task<int> AddPackageAsync(Package package, IEnumerable<IFormFile> imageFiles,string sections = null)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    if (package.Main)
                    {
                        package.PackageId = null;
                    }
                    var packageId = await _packageRepository.AddAsync(package);

                    if (imageFiles != null && imageFiles.Any())
                    {
                        var imageUrls = await SaveImagesAsync(imageFiles);
                        await AddImagesToPackageAsync(packageId, imageUrls);
                    }

                    if (sections != null)
                    {
                        var sectionss = JsonSerializer.Deserialize<IList<SectionDto>>(sections);
                        

                        foreach (var section in sectionss)
                        {
                            section.PackageId = packageId; 
                            var newSection = _mapper.Map<Section>(section);
                            await _sectionRepository.AddAsync(newSection);
                        }
                    }

                    await transaction.CommitAsync();

                    return packageId;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message.ToString());
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }


        public async Task UpdatePackageAsync(CreatePackageDto package, IEnumerable<IFormFile> imageFiles, string sections)
        {
            var packageExist = await _packageRepository.GetByIdAsync(package.Id, includes: p => p.Images);

            if (packageExist == null)
                throw new PackageNotFoundException($"package with id {package.Id} not exist");
            if (packageExist.Main && !package.Main)
            {
                Expression<Func<Package, bool>> filter = p => (p.PackageId == packageExist.Id);
                var checkMainPackageAssign = await _packageRepository.GetByColumnsAsync(filter);
                if (checkMainPackageAssign.Count() > 0)
                {
                    throw new AssignedMainPackageException($"this is main package that assigned to another package so remove assighnes packages first");
                }
            }

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var packageToUpdate = _mapper.Map(package, packageExist);
                    await _packageRepository.UpdateAsync(packageToUpdate);

                    if (imageFiles != null && imageFiles.Any())
                    {

                        _imageService.DeleteImageFiles(packageExist.Images);
                        _context.Set<Image>().RemoveRange(packageExist.Images);

                        var imageUrls = await SaveImagesAsync(imageFiles);
                        await AddImagesToPackageAsync(package.Id, imageUrls);
                    }

                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task DeletePackageAsync(int id)
        {
            var existingPackage = await _packageRepository.GetByIdAsync(id, includes: p => p.Images);

            if (existingPackage == null)
            {
                throw new PlaceNotFoundException($"package with id {id} not found.");
            }

            Expression<Func<Package, bool>> filter = p => (p.PackageId == existingPackage.Id);
            var packages = await _packageRepository.GetByColumnsAsync(filter);

            if (packages.Count() > 0)
            {
                throw new ConstraintPlaceErrorException($"this package is refference to another packages so delete other first");
            }
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    _imageService.DeleteImageFiles(existingPackage.Images);

                    _context.Set<Image>().RemoveRange(existingPackage.Images);

                    await _packageRepository.DeleteAsync(existingPackage);
                    await transaction.CommitAsync();

                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                }
            }

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

        public async Task<IEnumerable<string>> SaveImagesAsync(IEnumerable<IFormFile> imageFiles)
        {
            var imageUrls = new List<string>();

            foreach (var imageFile in imageFiles)
            {
                var imageUrl = await SaveImageAsync(imageFile);
                imageUrls.Add(imageUrl);
            }

            return imageUrls;
        }

        public async Task AddImagesToPackageAsync(int packageId, IEnumerable<string> imageUrls)
        {
            var images = imageUrls.Select(url => new Image
            {
                Url = url,
                PackageId = packageId
            });

            foreach (var image in images)
            {
                await _imageRepository.AddAsync(image);
            }
        }

        public async Task AddSectionsToPackageAsync(int packageId, IEnumerable<Section> sections)
        {
            foreach (var section in sections)
            {
                section.PackageId = packageId;
                await _sectionRepository.AddAsync(section);
            }
        }

        public async Task AddSalePackageAsync(int id, int SaleAmmount, string SaleType)
        {
            var packageExist = await _packageRepository.GetByIdAsync(id);
            if (packageExist == null)
                throw new PackageNotFoundException($"package with id {id} not exist");

            packageExist.HasSale = true;
            packageExist.SaleType = SaleType;
            packageExist.SaleAmount = SaleAmmount;

             await _packageRepository.UpdateAsync(packageExist);
        }

    }
}

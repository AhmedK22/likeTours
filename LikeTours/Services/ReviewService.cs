using LikeTours.Contracts.Services;
using LikeTours.Data.DTO.Place;
using LikeTours.Data.DTO.Question;
using LikeTours.Data.DTO;
using LikeTours.Data.Enums;
using LikeTours.Data.Models;
using LikeTours.Exceptions.Questions;
using System.Linq.Expressions;
using AutoMapper;
using LikeTours.Contracts.Repositories;
using LikeTours.Data.DTO.Review;
using System.Linq;
using LikeTours.Exceptions.Review;
using Microsoft.EntityFrameworkCore;
using LikeTours.Data;

namespace LikeTours.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IGenericRepository<Review> _reviewRepo;
        private readonly IMapper _mapper;
        private readonly string _imageStoragePath;
        private readonly ImageService _imageService;
        private readonly AppDbContext _context;
        public ReviewService(IMapper mapper, IGenericRepository<Review> reviewRepo, IConfiguration configuration, ImageService imageService, AppDbContext context)
        {

            _mapper = mapper;
            _reviewRepo = reviewRepo;
            _imageStoragePath = configuration["ImageStoragePath"];
            _imageService = imageService;
            _context = context;
        }

        public async Task<ApiResponse<PaginationDto<ReviewDto>>> GetAllAsync(ReviewQueryParam param, OrderType orderType = OrderType.Descending)
        {
            Expression<Func<Review, bool>> filter = p =>
             (string.IsNullOrEmpty(param.Lang) || p.Lang == param.Lang) &&
             (!param.Type.HasValue || p.TypeId == param.Type) &&
             (!param.PackageId.HasValue || p.PackageId == param.PackageId);

            var reviews = await _reviewRepo.GetAllAsync(param, filter, null, orderType);
            var reviewsDtos = _mapper.Map<IEnumerable<ReviewDto>>(reviews);

            int totalCount = await _reviewRepo.CountAsync(filter); 
            PaginationDto<ReviewDto> paginationDto = new PaginationDto<ReviewDto>()
            {
                Page = param.Page,
                PageSize = param.PageSize,
                Total = totalCount,
                Items = reviewsDtos
            };

            return new ApiResponse<PaginationDto<ReviewDto>>(paginationDto);
        }


        public async Task<ApiResponse<IEnumerable<ReviewDto>>> GetDataAsync(int? packageId = null, OrderType orderType = OrderType.Descending)
        {
            var param = new ReviewQueryParam()
            {
                HasPagination = false,
            };

            Expression<Func<Review, bool>> filter = null;

            if (packageId != null)
            {
                filter = p =>

               ((packageId.HasValue) || p.PackageId == packageId);
            }

            var reviews = await _reviewRepo.GetAllAsync(param, filter, null, orderType);
            var reviewsDto = _mapper.Map<IEnumerable<ReviewDto>>(reviews);
            var result = new ApiResponse<IEnumerable<ReviewDto>>(reviewsDto);

            return result;
        }


        public async Task<ReviewDto> GetByIdAsync(int id, string Lang)
        {

            var review = await _reviewRepo.GetByIdAsync(id, includes: p => p.RefferenceReview);
            if (review == null)
            {
                throw new ReviewNotFoundException($"review with id {id} not exist");
            }
            Expression<Func<Review, bool>> filter = null;
            Review reviewLang;

            if (review.Lang != Lang)
            {
                if (review.Main)
                {
                    filter = p =>
                      (string.IsNullOrEmpty(Lang) || p.Lang == Lang)
                      &&
                      (p.ReviewId == review.Id);
                }
                else
                {
                    filter = p =>
                    (string.IsNullOrEmpty(Lang) || p.Lang == Lang)
                     && (p.ReviewId == review.ReviewId);
                }
                var reviews = await _reviewRepo.GetByColumnsAsync(filter, includes: p => p.RefferenceReview);
                reviewLang = reviews.FirstOrDefault();
                if (reviewLang == null)
                {
                    throw new QuestionNotFoundException($"no reviews with lang {Lang} for id {id} exist");
                }
            }
            else
            {
                reviewLang = review;
            }


            var reviewDto = _mapper.Map<ReviewDto>(reviewLang);

            return reviewDto;
        }

        public async Task<int> AddAsync(CreateReviewDto CreateReviewDto)
        {
            string imageUrl = null;
            if (CreateReviewDto.Main)
            {
                CreateReviewDto.ReviewId = null;
            }
            if (CreateReviewDto.Image != null)
            {
                 imageUrl= await SaveImageAsync(CreateReviewDto.Image);
               
            }


            var review = _mapper.Map<Review>(CreateReviewDto);
            if (imageUrl != null)
            {

                review.Image = imageUrl;
            }
            return await _reviewRepo.AddAsync(review);
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

        public async Task UpdateAsync(CreateReviewDto CreateReviewDto)
        {
            var existingReview = await _reviewRepo.GetByIdAsync(CreateReviewDto.Id);

            if (existingReview == null)
            {
                throw new ReviewNotFoundException($"Review with id {CreateReviewDto.Id} not found.");
            }

            var deletedImage = existingReview.Image;

            if (existingReview.Main && !CreateReviewDto.Main)
            {
                Expression<Func<Review, bool>> filter = p => (p.ReviewId == existingReview.Id);
                var checkMainQuestionAssign = await _reviewRepo.GetByColumnsAsync(filter);
                if (checkMainQuestionAssign.Count() > 0)
                {
                    throw new AssignedMainReviewException($"this is main Review that assigned to another places so remove assighnes Questions first");
                }
            }
            var imageUrl = "";

            if (CreateReviewDto.Image != null)
            {
                _imageService.DeleteImageFile(deletedImage);
                imageUrl = await SaveImageAsync(CreateReviewDto.Image);

            }
            var reviewToUpdate = _mapper.Map(CreateReviewDto, existingReview);
            if(imageUrl != null)
            {

              reviewToUpdate.Image = imageUrl;
            }
            else
            {
                _imageService.DeleteImageFile(deletedImage);
            }

            await _reviewRepo.UpdateAsync(reviewToUpdate);

        }

        public async Task DeleteAsync(int id)
        {
            var existingReview = await _reviewRepo.GetByIdAsync(id);

            if (existingReview == null)
            {
                throw new ReviewNotFoundException($"Review with id {id} not found.");
            }

            Expression<Func<Review, bool>> filter = p => (p.ReviewId == existingReview.Id);
            var reviews = await _reviewRepo.GetByColumnsAsync(filter);

            if (reviews.Count() > 0)
            {
                throw new ConstraintReviewErrorException($"this review is refference to another types so delete other first");
            }

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    _imageService.DeleteImageFile(existingReview.Image);
                    await _reviewRepo.DeleteAsync(existingReview);
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


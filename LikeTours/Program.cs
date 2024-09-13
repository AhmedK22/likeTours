using FluentValidation.AspNetCore;
using FluentValidation;
using LikeTours.Contracts.Repositories;
using LikeTours.Contracts.Services;
using LikeTours.Data;
using LikeTours.Repositories;
using LikeTours.Services;
using LikeTours.Validators.Contacts;
using LikeTours.Validators.packages;
using LikeTours.Validators.Types;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MyProject.Middlewares;
using System.Text;
using LikeTours.Validators.questions;
using LikeTours.Validators.Review;
using LikeTours.Validators.payment;
using LikeTours.Validators.Places;
using LikeTours.Validators.aboutUs;
using static System.Net.WebRequestMethods;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCors(option =>
{
    option.AddDefaultPolicy(op =>
        op.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()
    );
});

// Register DbContext with connection string from appsettings.json
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CS")));

// Add Services to the DI container
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IPlaceService, PlaceService>();
builder.Services.AddScoped<ITypeService, TypeService>();
builder.Services.AddScoped<IPackageService, PackageService>();
builder.Services.AddScoped<ImageService, ImageService>();
builder.Services.AddScoped<IQuestionService, QuestionService>();
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IAboutUsService, AboutUsService>();

// Configure ApiBehaviorOptions
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

// Add FluentValidation and Validators
builder.Services.AddValidatorsFromAssemblyContaining<CreateQuestionDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateAboutDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateTypeValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<AddContactValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<AddSaleValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<packageCreateDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreatePaymentValidator>();
builder.Services.AddFluentValidationAutoValidation();

// Add AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Add Controllers
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAPI", Version = "v1" });
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });

    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

// Configure JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "http://ahmedk2222-001-site1.htempurl.com/",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
    };
});

// Build the app
var app = builder.Build();

// Use Swagger in Development
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
  //  app.UseDeveloperExceptionPage();
//}
//else
//{
  //  app.UseExceptionHandler("/error");
//}

// Middleware and configuration
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseCors();
app.UseHttpsRedirection();
app.UseAuthentication(); // Make sure to use authentication middleware
app.UseAuthorization();

// Seed Data
using (var serviceScope = app.Services.CreateScope())
{
    var dbContext = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
    var serviceProvider = serviceScope.ServiceProvider;
    Seeder.Seed(serviceProvider);
}

// Map controllers
app.MapControllers();

// Run the application
app.Run();

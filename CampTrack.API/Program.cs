using CampTrack.Application.Contracts;
using CampTrack.Application.MappingProfiles;
using CampTrack.Application.Services;
using CampTrack.Infrastructure.Repositories;
using CampTrack.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<ICategoriaService, CategoriaService>();
builder.Services.AddScoped<IEquipoService,    EquipoService>();
builder.Services.AddScoped<IPrestamoService,  PrestamoService>();
builder.Services.AddScoped<IRevisionService,  RevisionService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await SeederDb.SeedAsync(context);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapControllers();
app.Run();

using Domain.Interfaces;
using Domain.Repositories;
using Domain.Services;
using Microsoft.EntityFrameworkCore;
using Model.Configuration;
using Model.Entities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<PaymentDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("PaymentDB")));

// Repository + Domain service registrations (style aligned with Fitness sample)
builder.Services.AddScoped<IRepositoryAsync<Payment>>(sp =>
{
    var context = sp.GetRequiredService<PaymentDbContext>();
    return new PaymentRepository(context);
});
builder.Services.AddScoped<Domain.Services.PaymentService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Ensure local SQLite DB exists (demo out-of-the-box)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PaymentDbContext>();
    db.Database.EnsureCreated();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();

using Domain.Interfaces;
using Domain.Repositories;
using Domain.Services;
using Microsoft.EntityFrameworkCore;
using Model.Configuration;
using Model.Entities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<PaymentDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("PaymentDB")));

builder.Services.AddScoped<IRepositoryAsync<Payment>>(sp =>
{
    var context = sp.GetRequiredService<PaymentDbContext>();
    return new PaymentRepository(context);
});
builder.Services.AddScoped<Domain.Services.PaymentService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Ensure local SQLite DB exists (demo out-of-the-box)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PaymentDbContext>();
    db.Database.EnsureCreated();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Payment}/{action=Index}/{id?}");

app.Run();

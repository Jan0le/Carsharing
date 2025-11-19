using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Carsharing.Services.Interfaces;
using Carsharing.Services.Implementations;
using Carsharing.Data.DbContext;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Register Database Context
builder.Services.AddDbContext<ParticipantDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("ParticipantDB") 
        ?? "Data Source=ParticipantDB.db"));

// Register Carsharing Services
builder.Services.AddSingleton<IVehicleService, VehicleService>();
builder.Services.AddScoped<IParticipantService, ParticipantService>();
builder.Services.AddSingleton<IPaymentService, PaymentService>();
builder.Services.AddScoped<IBookingService>(sp => 
{
    var vehicleService = sp.GetRequiredService<IVehicleService>();
    var participantService = sp.GetRequiredService<IParticipantService>();
    var paymentService = sp.GetRequiredService<IPaymentService>();
    return new BookingService(vehicleService, participantService, paymentService);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();

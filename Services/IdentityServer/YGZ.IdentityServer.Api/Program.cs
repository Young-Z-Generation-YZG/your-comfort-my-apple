using YGZ.IdentityServer.Api;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRazorPages();

builder.Services
    //.AddInfrastructureLayer(builder.Configuration)
    //.AddApplicationLayer(builder.Configuration)
    .AddPresentationLayer();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();
app.UseHttpsRedirection();

app.UseAuthentication(); 
app.UseAuthorization(); 

app.MapControllers();

app.MapRazorPages();

app.Run();

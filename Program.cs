using Employee_Api.Packages;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<IPKG_Employees, PKG_Employees>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllCors", config =>
    {
        config.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAllCors");

app.UseAuthorization();

app.MapControllers();

app.Run();

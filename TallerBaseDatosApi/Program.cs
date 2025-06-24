var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddCarter();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//Database - Cache
builder.Services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
builder.Services.AddDbContext<ApplicationDbContext>((sp, options) => {
        options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
        options.UseSqlServer(builder.Configuration.GetConnectionString("Database"));
    }
);
builder.Services.AddLogging(logging =>
{
    logging.AddConsole();
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Taller Base de Datos API V1");
    });
    await app.InitializeDatabaseAsync(new CancellationToken());
}
app.MapCarter();
app.UseHttpsRedirection();

app.Run();

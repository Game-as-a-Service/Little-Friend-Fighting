using Microsoft.AspNetCore.StaticFiles;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

        builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

// Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseDefaultFiles();
        var provider = new FileExtensionContentTypeProvider
        {
            Mappings =
            {
                [".data"] = "multipart/form-data"
            }
        };

        app.UseStaticFiles(new StaticFileOptions
        {
            ContentTypeProvider = provider
        });
        app.UseHttpsRedirection();

        app.UseAuthorization();

        // app.Map("/", () => "Hello World");
        app.MapControllers();

        app.Run();
    }
}

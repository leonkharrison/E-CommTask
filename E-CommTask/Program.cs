using E_CommTask.DataBase;
using E_CommTask.Services;
using E_CommTask.Services.Interfaces;
using System.Text.Json.Serialization;

namespace E_CommTask
{
    public class Program
    {
        public static void Main( string[] args )
        {
            var builder = WebApplication.CreateBuilder( args );

            // Add services to the container.
            builder.Services.AddDbContext<ApplicationDbContext>();
            builder.Services.AddScoped<IOrdersRepo, OrdersService>();
            builder.Services.AddScoped<IProductRepo, ProductService>();

            builder.Services.AddControllers().AddJsonOptions( x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles );
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if( app.Environment.IsDevelopment() )
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
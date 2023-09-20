// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CarGame.Api.Extensions;
using CarGame.Model.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        builder.Services.AddConfigurationsServices(builder.Configuration);
        builder.Services.AddApplicationServices(builder.Configuration);
        builder.Services.AddIdentityServices(builder.Environment, builder.Configuration);

        var connStringBuilder = new NpgsqlConnectionStringBuilder(
            builder.Configuration.GetConnectionString("DefaultConnection"));

        if (builder.Environment.IsDevelopment())
        {
            connStringBuilder.Password = builder.Configuration["DbPassword"];
        }
        var connString = connStringBuilder.ConnectionString;

        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connString);
        dataSourceBuilder.UseNetTopologySuite();
        var dataSource = dataSourceBuilder.Build();

        builder.Services.AddDbContext<CarGameContext>(opt =>
        {
            opt.UseNpgsql(dataSource, o => o.UseNetTopologySuite());
        });

        var app = builder.Build();

        app.UseCors();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseOpenApi();
            app.UseSwaggerUi3();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}

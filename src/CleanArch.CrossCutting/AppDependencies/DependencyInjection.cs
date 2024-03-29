﻿using CleanArch.Application.Members.Commands.Validations;
using CleanArch.Domain.Abstractions;
using CleanArch.Infrastructure.Context;
using CleanArch.Infrastructure.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using System.Reflection;

namespace CleanArch.CrossCutting.AppDependencies;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        var sqlConnection = configuration.GetConnectionString("ClearArcht");

        services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(sqlConnection));

        // Registrar IDbConnection como uma instância única
        services.AddSingleton<IDbConnection>(provider =>
        {
            var connection = new SqlConnection(sqlConnection);
            connection.Open();
            return connection;
        });


        services.AddScoped<IMemberRepository, MemberRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        var myhandlers = AppDomain.CurrentDomain.Load("CleanArch.Application");
        services.AddMediatR(myhandlers);
        services.AddMediatR(typeof(ValidationBehaviour<,>));

        services.AddValidatorsFromAssembly(Assembly.Load("CleanArch.Application"));

        return services;
    }
}
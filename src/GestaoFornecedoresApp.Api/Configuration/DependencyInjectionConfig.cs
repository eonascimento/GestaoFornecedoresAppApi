using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestaoFornecedoresApp.Business.Interfaces;
using GestaoFornecedoresApp.Business.Interfaces.Respositories;
using GestaoFornecedoresApp.Business.Interfaces.Services;
using GestaoFornecedoresApp.Business.Notifications;
using GestaoFornecedoresApp.Business.Services;
using GestaoFornecedoresApp.Data.Context;
using GestaoFornecedoresApp.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace GestaoFornecedoresApp.Api.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependecies(this IServiceCollection services)
        {
            services.AddScoped<GestaoFornecedoresContext>();
            services.AddScoped<IFornecedorRepository, FornecedorRepository>();
            services.AddScoped<IProdutoRepository, ProdutoRepository>();
            services.AddScoped<IEnderecoRepository, EnderecoRepository>();

            services.AddScoped<INotificador, Notificador>();
            services.AddScoped<IFornecedorService, FornecedorService>();
            services.AddScoped<IProdutoService, ProdutoService>();

            return services;
        }
    }
}

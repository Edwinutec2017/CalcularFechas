using CalendarioSeppNotificaciones.Core.UseCase;
using CalendarioSeppNotificaciones.Core.UseCase.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalendarioSeppNotificaciones.Config
{
    public static class Inyeccion
    {
        public static void Registry(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddScoped<ICalculoFechas, CalculoFechas>();
            serviceCollection.AddScoped<IGenerarScrip, GenerarScrip>();

        }
    }
}

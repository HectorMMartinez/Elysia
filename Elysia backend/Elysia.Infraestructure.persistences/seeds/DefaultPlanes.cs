using Elysia.Core.Domain.Entities;
using Elysia.Infraestructure.persistences.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Infraestructure.persistences.seeds
{
    public static class DefaultPlanes
    {

        public static async Task SeedAsync(ElysiaContext context)
        {
            try
            {
                if (await context.Plans.AnyAsync())
                {
                    Console.WriteLine("Ya hay planes registrados.");
                    return;
                }

                context.Plans.AddRange(
                    new Plan
                    {
                        Nombre = "Plan Simple",
                        Descripcion = "Diseñado para pequeños restaurantes que necesitan administrar sus operaciones esenciales desde una sola plataforma. Incluye gestión de platos, menús, mesas, reservas, pedidos, inventario, personal y turnos, además de un dashboard operativo con indicadores básicos del negocio",
                        PrecioMensual = 1500
                    },
                    new Plan
                    {
                        Nombre = "Plan Premium",
                        Descripcion = "Diseñado para restaurantes que requieren una gestión estratégica y basada en datos. Incluye todas las funcionalidades del Plan Simple, además de reportes operativos avanzados, análisis de ventas, estadísticas de inventario, indicadores de desempeño, alertas inteligentes y recomendaciones automáticas para optimizar la toma de decisiones",
                        PrecioMensual = 3500
                    }
                );

                await context.SaveChangesAsync();

                Console.WriteLine("Planes insertados correctamente.");
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"Ocurrió un error al insertar los planes por defecto: {ex.Message}",
                    ex
                );
            }

        }
    }
}

using Elysia.Core.Domain.Entities;
using Elysia.Infraestructure.persistences.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Infraestructure.persistences.seeds
{
    public static class DefaultPuestoTrabajo
    {

        public static async Task SeedAsync(ElysiaContext context)
        {
            try
            {
                if (await context.Puestos.AnyAsync())
                {
                    Console.WriteLine("Ya hay puestos registrados.");
                    return;
                }

                context.Puestos.AddRange(
                    new Puesto
                    {
                        Name = "Administrador",
                        Description = "Responsable de la gestión general del restaurante."
                    },
                    new Puesto
                    {
                        Name = "Gerente",
                        Description = "Supervisa las operaciones diarias y al personal."
                    },
                    new Puesto
                    {
                        Name = "Supervisor",
                        Description = "Coordina las actividades del equipo durante el turno."
                    },
                    new Puesto
                    {
                        Name = "Cajero",
                        Description = "Gestiona los cobros y pagos de los clientes."
                    },
                    new Puesto
                    {
                        Name = "Mesero",
                        Description = "Atiende a los clientes y gestiona las órdenes."
                    },
                    new Puesto
                    {
                        Name = "Anfitrión",
                        Description = "Recibe a los clientes y administra las reservas."
                    },
                    new Puesto
                    {
                        Name = "Cocinero",
                        Description = "Prepara los alimentos siguiendo los estándares del restaurante."
                    },
                    new Puesto
                    {
                        Name = "Ayudante de Cocina",
                        Description = "Brinda apoyo en la preparación de alimentos y organización de la cocina."
                    },
                    new Puesto
                    {
                        Name = "Bartender",
                        Description = "Prepara bebidas y atiende el área del bar."
                    },
                    new Puesto
                    {
                        Name = "Repartidor",
                        Description = "Realiza las entregas de pedidos a domicilio."
                    },
                    new Puesto
                    {
                        Name = "Personal de Limpieza",
                        Description = "Mantiene limpias y ordenadas las instalaciones."
                    }
                );

                await context.SaveChangesAsync();

                Console.WriteLine("Puestos insertados correctamente.");
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"Ocurrió un error al insertar los puestos por defecto: {ex.Message}",
                    ex
                );
            }
        }

    }

}

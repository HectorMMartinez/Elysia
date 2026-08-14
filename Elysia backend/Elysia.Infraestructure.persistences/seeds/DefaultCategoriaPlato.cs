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
    public static class DefaultCategoriaPlato
    {
        public static async Task SeedAsync(ElysiaContext context)
        {
            try
            {
                if (await context.CategoriaPlatos.AnyAsync())
                {
                    Console.WriteLine("Ya hay planes registrados.");
                    return;
                }


                context.CategoriaPlatos.AddRange(
                    new CategoriaPlato
                    {
                        Nombre = "Entradas",
                        Descripcion = "Platos ligeros servidos antes del plato principal para abrir el apetito.",


                    },
                    new CategoriaPlato
                    {
                        Nombre = "Sopas",
                        Descripcion = "Preparaciones líquidas o cremosas elaboradas con carnes, vegetales o mariscos.",

                    },
                    new CategoriaPlato
                    {
                        Nombre = "Ensaladas",
                        Descripcion = "Platos preparados con vegetales frescos y otros ingredientes, servidos fríos o tibios.",

                    },
                     new CategoriaPlato
                     {
                         Nombre = "Platos fuertes",
                         Descripcion = "Platos principales con mayor cantidad de ingredientes y valor nutricional.",

                     },
                     new CategoriaPlato
                     {
                         Nombre = "Pastas",
                         Descripcion = "Platos principales con mayor cantidad de ingredientes y valor nutricional.",

                     },
                     new CategoriaPlato
                     {
                         Nombre = "Arroces",
                         Descripcion = "Platos cuyo ingrediente principal es el arroz, combinado con carnes, vegetales o mariscos.",

                     },
                      new CategoriaPlato
                      {
                          Nombre = "Carnes",
                          Descripcion = "Platos elaborados principalmente con carne de res, cerdo o cordero",

                      },
                      new CategoriaPlato
                      {
                          Nombre = "Pollo",
                          Descripcion = "Platos preparados a base de pollo en distintas presentaciones y estilos de cocción.",

                      },
                       new CategoriaPlato
                       {
                           Nombre = "Pescados",
                           Descripcion = "Platos cuyo ingrediente principal es pescado fresco o procesado.",

                       },
                        new CategoriaPlato
                        {
                            Nombre = "Mariscos",
                            Descripcion = "Preparaciones elaboradas con camarones, pulpo, calamares, mejillones u otros mariscos.",

                        },
                         new CategoriaPlato
                         {
                             Nombre = "Parrilla",
                             Descripcion = "Platos cocinados a la parrilla o al carbón, resaltando el sabor de las carnes y vegetales.",

                         },
                          new CategoriaPlato
                          {
                              Nombre = "Comida típica",
                              Descripcion = "Platos representativos de la gastronomía tradicional de una región o país.",

                          },
                           new CategoriaPlato
                           {
                               Nombre = "Hamburguesas",
                               Descripcion = "Preparaciones con carne o alternativas vegetales servidas en pan con diversos ingredientes.",

                           },
                           new CategoriaPlato
                           {
                               Nombre = "Pizzas",
                               Descripcion = "Masa horneada cubierta con salsa, queso e ingredientes variados.",

                           },
                           new CategoriaPlato
                           {
                               Nombre = "Sándwiches",
                               Descripcion = "Preparaciones con pan relleno de carnes, vegetales, quesos u otros ingredientes..",

                           },
                           new CategoriaPlato
                           {
                               Nombre = "Acompañamientos",
                               Descripcion = "Guarniciones que complementan los platos principales, como papas, arroz o vegetales..",

                           },
                           new CategoriaPlato
                           {
                               Nombre = "Postres",
                               Descripcion = "Preparaciones dulces servidas al finalizar la comida..",

                           },
                           new CategoriaPlato
                           {
                               Nombre = "Bebidas",
                               Descripcion = "Refrescos, jugos, cafés, tés y otras bebidas para acompañar los alimentos.",

                           },
                           new CategoriaPlato
                           {
                               Nombre = "Cócteles",
                               Descripcion = "Bebidas preparadas mediante la combinación de bebidas alcohólicas y otros ingredientes.",

                           },
                           new CategoriaPlato
                           {
                               Nombre = "Bebidas alcohólicas",
                               Descripcion = "Bebidas con contenido de alcohol, como cerveza, vino, ron o whisky.",

                           },
                           new CategoriaPlato
                           {
                               Nombre = "Menú infantil",
                               Descripcion = "Platos diseñados para niños, con porciones e ingredientes adaptados a sus preferencias.",

                           },
                           new CategoriaPlato
                           {
                               Nombre = "Opciones vegetarianas",
                               Descripcion = "Platos elaborados sin carne, enfocados en vegetales, legumbres y derivados.",

                           },
                           new CategoriaPlato
                           {
                               Nombre = "Opciones veganas",
                               Descripcion = "Platos preparados sin ingredientes de origen animal.",

                           },
                           new CategoriaPlato
                           {
                               Nombre = "Otra",
                               Descripcion = "Categoria no mencionada o identificada",

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

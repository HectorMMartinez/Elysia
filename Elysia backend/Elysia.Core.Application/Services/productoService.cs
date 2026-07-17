using AutoMapper;
using Elysia.Core.Application.Dtos.producto;
using Elysia.Core.Application.Interfaces;
using Elysia.Core.Domain.Entities;
using ReservaBook.Core.Domain.Interfaces;

namespace Elysia.Core.Application.Services
{
    public class productoService
        : GenericService<
            Producto,
            ProductoResponseDto,
            EditarProductoDto,
            CreateProductoDto
        >,
        IProductoService
    {
        private readonly IMapper mapper;
        private readonly IGenericRepository<Producto> productoRepository;

        public productoService(
            IGenericRepository<Producto> genericRepository,
            IMapper mapper
        ) : base(genericRepository, mapper)
        {
            this.mapper = mapper;
            productoRepository = genericRepository;
        }

        public override async Task<ProductoResponseDto?> AddAsync(
            CreateProductoDto? dto
        )
        {
            var response = new ProductoResponseDto
            {
                HasError = false,
                Errors = []
            };

            try
            {
                if (dto == null)
                {
                    response.HasError = true;
                    response.Errors.Add(
                        "Los valores del producto no fueron indicados correctamente."
                    );

                    return response;
                }

                if (
                    string.IsNullOrWhiteSpace(dto.UnidadMedida) ||
                    dto.UnidadMedida.Length > 4
                )
                {
                    response.HasError = true;
                    response.Errors.Add(
                        "La unidad de medida del producto no puede superar los 4 caracteres."
                    );

                    return response;
                }

                if (dto.StockActual <= 0)
                {
                    response.HasError = true;
                    response.Errors.Add(
                        "El stock actual debe ser mayor que cero."
                    );

                    return response;
                }

                if (dto.StockMinimo <= 0)
                {
                    response.HasError = true;
                    response.Errors.Add(
                        "El stock mínimo debe ser mayor que cero."
                    );

                    return response;
                }

                if (dto.StockMinimo > dto.StockActual)
                {
                    response.HasError = true;
                    response.Errors.Add(
                        "El stock mínimo no puede ser mayor que el stock actual."
                    );

                    return response;
                }

                dto.FechaCreacion = DateTime.Now;
                dto.Activo = true;

                var productoCreado = await base.AddAsync(dto);

                return productoCreado;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"Ocurrió un error al intentar agregar el producto: {ex.Message}",
                    ex
                );
            }
        }

        public override async Task<ProductoResponseDto?> UpdateAsync(
            int id,
            EditarProductoDto? dto
        )
        {
            var response = new ProductoResponseDto
            {
                HasError = false,
                Errors = []
            };

            try
            {
                if (id <= 0)
                {
                    response.HasError = true;
                    response.Errors.Add(
                        "Debes indicar un producto válido."
                    );

                    return response;
                }

                if (dto == null)
                {
                    response.HasError = true;
                    response.Errors.Add(
                        "Los valores del producto no fueron indicados correctamente."
                    );

                    return response;
                }

                var productoActual =
                    await productoRepository.GetByIdAsync(id);

                if (productoActual == null)
                {
                    response.HasError = true;
                    response.Errors.Add(
                        "Producto no encontrado, no se encontró el producto a editar."
                    );

                    return response;
                }

                if (
                    string.IsNullOrWhiteSpace(dto.UnidadMedida) ||
                    dto.UnidadMedida.Length > 4
                )
                {
                    response.HasError = true;
                    response.Errors.Add(
                        "La unidad de medida del producto no puede superar los 4 caracteres."
                    );

                    return response;
                }

                if (dto.StockActual <= 0)
                {
                    response.HasError = true;
                    response.Errors.Add(
                        "El stock actual debe ser mayor que cero."
                    );

                    return response;
                }

                if (dto.StockMinimo <= 0)
                {
                    response.HasError = true;
                    response.Errors.Add(
                        "El stock mínimo debe ser mayor que cero."
                    );

                    return response;
                }

                if (dto.StockMinimo > dto.StockActual)
                {
                    response.HasError = true;
                    response.Errors.Add(
                        "El stock mínimo no puede ser mayor que el stock actual."
                    );

                    return response;
                }

                // Solo se actualizan los campos editables.
                productoActual.Nombre = dto.Nombre.Trim();
                productoActual.Descripcion = dto.Descripcion.Trim();
                productoActual.UnidadMedida =
                    dto.UnidadMedida.Trim();
                productoActual.StockActual = dto.StockActual;
                productoActual.StockMinimo = dto.StockMinimo;
                productoActual.Activo = dto.Activo;

                // Solo reemplaza la imagen cuando se envía una nueva.
                if (!string.IsNullOrWhiteSpace(dto.Imagen))
                {
                    productoActual.Imagen = dto.Imagen;
                }

                /*
                 * No se modifica:
                 * productoActual.FechaCreacion
                 * productoActual.IdPropietario
                 *
                 * De esta manera se conservan sus valores originales.
                 */

                var productoActualizado =
                    await productoRepository.UpdateAsync(
                        id,
                        productoActual
                    );

                return mapper.Map<ProductoResponseDto>(
                    productoActualizado
                );
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"Ocurrió un error al intentar editar el producto: {ex.Message}",
                    ex
                );
            }
        }
    }
}
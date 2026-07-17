using AutoMapper;
using Elysia.Core.Application.Dtos.movimientoInventario;
using Elysia.Core.Application.Interfaces;
using Elysia.Core.Domain.Common;
using Elysia.Core.Domain.Entities;
using Elysia.Core.Domain.interfaces;
using ReservaBook.Core.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Services
{
    public class MovimientoInventarioService : GenericService<MovimientoInventario, CrearMovimientoInventarioResponseDto, CrearMovimientoInventarioDto, CrearMovimientoInventarioDto>, IMovimientoInventarioService
    {
        private readonly IMapper _mapper;
        private readonly IMovimientoRepository repo;
        private readonly IProductoRepository productoRepository;

        public MovimientoInventarioService(IMovimientoRepository repo, IProductoRepository productoRepository, IMapper _mapper) : base(repo, _mapper)
        {
            this.repo = repo;
            this._mapper = _mapper;
            this.productoRepository = productoRepository;

        }

        public override async Task<CrearMovimientoInventarioResponseDto?> AddAsync(
     CrearMovimientoInventarioDto? entity
 )
        {
            var response = new CrearMovimientoInventarioResponseDto
            {
                HasError = false,
                Errors = []
            };

            try
            {
                if (entity == null)
                {
                    response.HasError = true;
                    response.Errors.Add(
                        "No se indicaron correctamente los valores del movimiento."
                    );

                    return response;
                }

                var producto = await productoRepository.GetByIdAsync(
                    entity.ProductoId
                );

                if (producto == null)
                {
                    response.HasError = true;
                    response.Errors.Add(
                        "No se encontró el producto especificado. No se puede registrar el movimiento."
                    );

                    return response;
                }

                if (!Enum.IsDefined(
                        typeof(TipoMovimientoInventario),
                        entity.TipoMovimiento
                    ))
                {
                    response.HasError = true;
                    response.Errors.Add(
                        "El tipo de movimiento indicado es incorrecto."
                    );

                    return response;
                }

                // Esta validación solo corresponde a las salidas.
                if (
                    entity.TipoMovimiento == TipoMovimientoInventario.Salida &&
                    entity.Cantidad > producto.StockActual
                )
                {
                    response.HasError = true;
                    response.Errors.Add(
                        "La cantidad de salida no puede ser mayor que el stock actual del producto."
                    );

                    return response;
                }

                if (entity.TipoMovimiento == TipoMovimientoInventario.Entrada)
                {
                    producto.StockActual += entity.Cantidad;
                }
                else
                {
                    producto.StockActual -= entity.Cantidad;
                }

                await productoRepository.UpdateAsync(
                    entity.ProductoId,
                    producto
                );

                var movimientoCreado = await base.AddAsync(entity);

                return movimientoCreado;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    "Ocurrió un error al intentar registrar el movimiento en el inventario.",
                    ex
                );
            }
        }
    }
}

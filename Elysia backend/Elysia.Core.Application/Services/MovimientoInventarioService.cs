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

        public override async Task<CrearMovimientoInventarioResponseDto?> AddAsync(CrearMovimientoInventarioDto? entity)
        {
            var response = new CrearMovimientoInventarioResponseDto() { HasError = false, Errors = [] };

            try
            {
                if (entity == null)
                {
                    response.HasError = true;
                    response.Errors.Add("No se indicaron los valores del movimiento correctamente");
                    return response;

                }

                var producto = await productoRepository.GetByIdAsync(entity.ProductoId);

                if(producto == null)
                {
                    response.HasError = true;
                    response.Errors.Add("No se encontro el producto especificado, no puede registrar el movimiento");
                    return response;
                }

                if (entity.Cantidad > producto.StockActual)
                {
                    response.HasError = true;
                    response.Errors.Add("la cantidad del movimiento no puede ser mayor que el stock del producto");
                    return response;
                }



                if (!Enum.IsDefined(typeof(TipoMovimientoInventario), entity.TipoMovimiento))
                {
                    response.HasError = true;
                    response.Errors.Add("El tipo de movimiento indicado es incorrecto");
                    return response;
                }

                if (entity.TipoMovimiento == TipoMovimientoInventario.Entrada)
                {
                    producto.StockActual += entity.Cantidad;
                }
                else if(entity.TipoMovimiento == TipoMovimientoInventario.Salida)
                {
                    producto.StockActual -= entity.Cantidad;
                }



                await productoRepository.UpdateAsync(entity.ProductoId,producto);
                var data = await base.AddAsync(entity);
                var map = _mapper.Map<CrearMovimientoInventarioResponseDto>(data);
                return map;
            }
            catch (Exception ex)
            {
                throw new NotImplementedException("Ocurrio un error al intentar registrar el movimiento en el inventario");
            }
        }



    }
}

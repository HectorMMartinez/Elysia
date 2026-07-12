using AutoMapper;
using Elysia.Core.Application.Dtos.producto;
using Elysia.Core.Application.Dtos.Tarjeta;
using Elysia.Core.Application.Interfaces;
using Elysia.Core.Domain.Entities;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.JSInterop.Infrastructure;
using ReservaBook.Core.Domain.Interfaces;
using System.Xml;

namespace Elysia.Core.Application.Services
{
    public class productoService : GenericService<Producto, ProductoResponseDto, EditarProductoDto, CreateProductoDto>, IProductoService
    {
         

         private readonly IMapper mapper;

        public productoService(IGenericRepository<Producto> genericRepository, IMapper _mapper) : base(genericRepository, _mapper)
        {
            mapper = _mapper;
        }


        public override async Task<ProductoResponseDto?> AddAsync(CreateProductoDto? dto)
        {
        
            var response = new ProductoResponseDto() { HasError = false, Errors = [] };

            try
            {
                if (dto == null) 
                {
                    response.HasError = true;
                    response.Errors.Add("Los valores del producto no fueron correctamente indicado");
                    return response;
         
                }

                if(dto.UnidadMedida != null && dto.UnidadMedida.Length > 4)
                {
                    response.HasError |= true;
                    response.Errors.Add("La unidad de medida del producto no puede superar los 4 digitos, unidad de medida invalida");
                    return response;
                }


                if(dto.StockMinimo > dto.StockActual)
                {
                    response.HasError = true;
                    response.Errors.Add("El stock minimo no puede ser menor al stock original o actual");
                    return response;
                }


         
                dto.FechaCreacion = DateTime.Now;
                dto.Activo = true;
                var data = await base.AddAsync(dto);
                var map = mapper.Map<ProductoResponseDto>(data);
                return map;

            }
            catch (Exception ex)
            {

                throw new Exception($"Ocurrio un error al intentar agregar el producto:{ex.Message}");
            
            }

        
        
        
        
        }




        public override async Task<ProductoResponseDto?> UpdateAsync(int id,EditarProductoDto? dto)
        {

            var response = new ProductoResponseDto() { HasError = false, Errors = [] };

            try
            {

                var producto = await base.GetByIdAsync(id);

                if(producto == null)
                {
                    response.HasError = true;
                    response.Errors.Add("Producto no encontrado, no se encontro el producto ha editar");
                    return response;
                }


                if (dto == null)
                {
                    response.HasError = true;
                    response.Errors.Add("Los valores del producto no fueron correctamente indicado");
                    return response;

                }

                if (dto.UnidadMedida != null && dto.UnidadMedida.Length > 4)
                {
                    response.HasError |= true;
                    response.Errors.Add("La unidad de medida del producto no puede superar los 4 digitos, unidad de medida invalida");
                    return response;
                }


                if (dto.StockMinimo > dto.StockActual)
                {
                    response.HasError = true;
                    response.Errors.Add("El stock minimo no puede ser menor al stock original o actual");
                    return response;
                }

               

                dto.StockMinimo = dto.StockMinimo > 0 ? dto.StockMinimo : producto.StockMinimo;
                dto.StockActual = dto.StockActual > 0 ? dto.StockActual : producto.StockActual;
                dto.Activo = dto.Activo ? dto.Activo : producto.Activo;
                dto.Nombre = !string.IsNullOrEmpty(dto.Nombre) ? dto.Nombre : producto.Nombre;
                dto.Descripcion = !string.IsNullOrEmpty(dto.Descripcion)  ? dto.Descripcion : producto.Descripcion;
                dto.UnidadMedida = !string.IsNullOrEmpty(dto.UnidadMedida) ? dto.UnidadMedida : producto.UnidadMedida;
                dto.IdPropietario = producto.IdPropietario; 
                dto.Id = id;


                var data = await base.UpdateAsync(id,dto);
                return data;

            }
            catch (Exception ex)
            {

                throw new Exception($"Ocurrio un error al intentar editar el producto:{ex.Message}");

            }





        }




    }
}

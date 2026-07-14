using AutoMapper;
using Elysia.Core.Application.Dtos.plato;
using Elysia.Core.Application.Dtos.producto;
using Elysia.Core.Application.Interfaces;
using Elysia.Core.Domain.Common;
using Elysia.Core.Domain.Entities;
using Elysia.Core.Domain.interfaces;
using ReservaBook.Core.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Elysia.Core.Application.Services
{
    public class PlatoService : GenericService<Plato, PlatoResponseDto, EditarPlatoDto, CreatePlatoDto>, IPlatoService
    {

        private readonly IMapper _mapper;
        private readonly IPlatoProductoRepository platoProductoRepository;
        private readonly IPlatoRepository repo;
        private readonly ICategoriaPlatoRepository categoriaPlatoRepository;
        private readonly IProductoRepository productoRepository;


        public PlatoService(IPlatoRepository repo, IPlatoProductoRepository platoProductoRepository, ICategoriaPlatoRepository categoriaPlatoRepository, IProductoRepository productoRepository, IMapper _mapper) : base(repo, _mapper)
        {
            this._mapper = _mapper;
            this.repo = repo;
            this.platoProductoRepository = platoProductoRepository;
            this.categoriaPlatoRepository = categoriaPlatoRepository;
            this.productoRepository = productoRepository;
        }



        public override async Task<PlatoResponseDto?> AddAsync(CreatePlatoDto? dto)
        {
            var response = new PlatoResponseDto() { HasError = false, Errors = [] };

            try
            {
                if (dto == null)
                {
                    response.HasError = true;
                    response.Errors.Add("Debes introducir correctamente los valores, para crear el plato");
                    return response;
                }


                if (!Enum.IsDefined(typeof(PlatoEstado), dto.Estado))
                {
                    response.HasError = true;
                    response.Errors.Add("Debes seleccionar una categoria valida para el plato");
                    return response;
                }

                if (dto.ProductoQuantityDtos == null || dto.ProductoQuantityDtos.Count == 0)
                {
                    response.HasError = true;
                    response.Errors.Add("no se han seleccionado los productos para el plato, no puedes crearlos");
                    return response;

                }

                var plato = await base.AddAsync(dto);
                var listPlatoProductos = new List<PlatoProducto>();
                foreach (var item in dto.ProductoQuantityDtos)
                {
                    listPlatoProductos.Add(new PlatoProducto() { Cantidad = item.Cantidad, ProductoId = item.Id, PlatoId = plato!.Id });

                }
                await platoProductoRepository.AddRangeAsync(listPlatoProductos);
                var map = _mapper.Map<PlatoResponseDto>(plato);
                return map;
            }
            catch (Exception ex)
            {

                throw new Exception($"Ocurrio un error al intentar agregar el plato:{ex.Message}");


            }


        }



        public override async Task<PlatoResponseDto?> UpdateAsync(int id, EditarPlatoDto? dto)
        {
            var response = new PlatoResponseDto() { HasError = false, Errors = [] };

            try
            {
                if (dto == null)
                {
                    response.HasError = true;
                    response.Errors.Add("Debes introducir correctamente los valores, para crear el plato");
                    return response;
                }

                var plato = await repo.GetByIdAsync(id);

                if (plato == null)
                {
                    response.HasError = true;
                    response.Errors.Add("No se pudo encontrar el plato especificado para editar");
                    return response;
                }


                if (!Enum.IsDefined(typeof(PlatoEstado), dto.Estado))
                {
                    response.HasError = true;
                    response.Errors.Add("Debes seleccionar una categoria valida para el plato");
                    return response;
                }

                var platoProducto = await platoProductoRepository.GetByPlatoId(plato.Id);

                if (platoProducto == null || platoProducto.Count == 0)
                {
                    response.HasError = true;
                    response.Errors.Add("no se econtraron los productos para este plato");
                    return response;
                }
                var actualizar = new List<PlatoProducto>();

                if (dto.ProductoQuantityDtos != null && dto.ProductoQuantityDtos.Count > 0)
                {
                    foreach (var existente in platoProducto)
                    {
                        var dtoProducto = dto.ProductoQuantityDtos
                            .FirstOrDefault(x => x.Id == existente.ProductoId);

                        if (dtoProducto != null)
                        {
                            existente.Cantidad = dtoProducto.Cantidad;
                        }
                    }

                    await platoProductoRepository.UpdateRangeAsync(plato.Id,platoProducto!);

                }




                dto.Estado = !string.IsNullOrEmpty(dto.Estado.ToString()) ? dto.Estado : plato.Estado;
                dto.Id = id;
                dto.Descripcion = !string.IsNullOrEmpty(dto.Descripcion) ? dto.Descripcion : plato.Descripcion;
                dto.Nombre = !string.IsNullOrEmpty(dto.Nombre) ? dto.Nombre : plato.Nombre;
                dto.Precio = dto.Precio > 0 ? dto.Precio : plato.Precio;
                dto.Imagen = !string.IsNullOrEmpty(dto.Imagen) ? dto.Imagen : plato.Imagen;
                dto.IdPropietario = plato.IdPropietario;
                dto.CategoriaId = plato.CategoriaId;
                dto.Codigo = plato.Codigo;
                dto.Fecha = plato.Fecha;
                dto.ProductoQuantityDtos = dto.ProductoQuantityDtos!.Count > 0 ? dto.ProductoQuantityDtos : platoProducto.Select(s => new productoQuantityDto()
                {
                    Id = s.ProductoId,
                    Cantidad = s.Cantidad
                }).ToList();

                var data = await base.UpdateAsync(id, dto);
                var map = _mapper.Map<PlatoResponseDto>(data);
                return map;
            }
            catch (Exception ex)
            {

                throw new Exception($"Ocurrio un error al intentar  editar el plato:{ex.Message}");


            }

        }


        //obtener todos los  platos con sus ingredientes
        public async Task<List<MostrarPlatoConIngredientesDto?>> GetlAllConIngredientesAsync(string propietarioId)
        {
            var platos = await repo.GetAllByPropietarioId(propietarioId);

            var categorias = await categoriaPlatoRepository.GetlAllAsync();

            var platoProductos = await platoProductoRepository.GetlAllAsync();

            var productos = await productoRepository.GetlAllAsync();

            var result = new List<MostrarPlatoConIngredientesDto>();

            foreach (var plato in platos)
            {
                var dto = new MostrarPlatoConIngredientesDto
                {
                    Id = plato.Id,
                    Nombre = plato.Nombre,
                    Descripcion = plato.Descripcion,
                    Precio = plato.Precio,
                    Imagen = plato.Imagen,
                    Fecha = plato.Fecha,
                    Codigo = plato.Codigo,
                    CategoriaId = plato.CategoriaId,
                    Estado = plato.Estado,
                    IdPropietario = plato.IdPropietario
                };

                dto.NombreCategoria = categorias
                    .FirstOrDefault(c => c.Id == plato.CategoriaId)?.Nombre ?? "";

                dto.ListDataProducto = platoProductos
                .Where(pp => pp.PlatoId == plato.Id)
                .Join(
                productos,
                pp => pp.ProductoId,
                p => p.Id,
                (pp, p) => new PlatoProductoDataDto
                {
                 NombreProducto = p.Nombre,
                 CantidaProducto = pp.Cantidad
                }).ToList();

                result.Add(dto);
            }


            return result;

        }




        //obtener un plato por id con su ingredientes 
        public async Task<MostrarPlatoConIngredientesDto?> GetByIdConIgrendientesAsync(int id)
        {
            var plato = await repo.GetByIdAsync(id);

            if (plato == null)
            {
                return null;
            }

            var categoria = await categoriaPlatoRepository.GetByIdAsync(plato.CategoriaId);

            var platoProductos = await platoProductoRepository.GetByPlatoId(plato.Id);

            var productos = await productoRepository.GetByIdsAsync(
                platoProductos.Select(x => x.ProductoId));

            var dto = new MostrarPlatoConIngredientesDto
            {
                Id = plato.Id,
                Nombre = plato.Nombre,
                Descripcion = plato.Descripcion,
                Precio = plato.Precio,
                Imagen = plato.Imagen,
                Fecha = plato.Fecha,
                Codigo = plato.Codigo,
                CategoriaId = plato.CategoriaId,
                NombreCategoria = categoria?.Nombre ?? string.Empty,
                Estado = plato.Estado,

                ListDataProducto = platoProductos
                    .Join(
                        productos,
                        pp => pp.ProductoId,
                        p => p.Id,
                        (pp, p) => new PlatoProductoDataDto
                        {
                            NombreProducto = p.Nombre,
                            CantidaProducto = pp.Cantidad
                        })
                    .ToList()
            };

            return dto;
        }







    }
}

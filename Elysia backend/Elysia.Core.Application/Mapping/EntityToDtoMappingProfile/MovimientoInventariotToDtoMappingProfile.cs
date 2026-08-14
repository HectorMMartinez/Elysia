using AutoMapper;
using Elysia.Core.Application.Dtos.movimientoInventario;
using Elysia.Core.Domain.Entities;

namespace Elysia.Core.Application.Mapping.EntityToDtoMappingProfile
{
    public class MovimientoInventariotToDtoMappingProfile : Profile
    {
        public MovimientoInventariotToDtoMappingProfile()
        {
            CreateMap<
                MovimientoInventario,
                CrearMovimientoInventarioResponseDto
            >();

            CreateMap<
                CrearMovimientoInvetarioRequestDto,
                CrearMovimientoInventarioDto
            >();

            CreateMap<
                CrearMovimientoInventarioDto,
                MovimientoInventario
            >()
            .ForMember(
                destino => destino.FechaMovimiento,
                opcion => opcion.MapFrom(_ => DateTime.Now)
            );
        }
    }
}
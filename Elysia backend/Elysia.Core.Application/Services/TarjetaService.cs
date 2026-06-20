

using AutoMapper;
using Elysia.Core.Application.Dtos.Tarjeta;
using Elysia.Core.Application.Interfaces;
using Elysia.Core.Domain.Common;
using Elysia.Core.Domain.Entities;
using Elysia.Core.Domain.interfaces;
using ReservaBook.Core.Domain.Interfaces;

namespace Elysia.Core.Application.Services
{
    public class TarjetaService : GenericService<Tarjeta, TarjetaResponseDto, EditTarjetaDto, SaveTarjetaDto>, ITarjetaService
    {
        private readonly ITarjetaRepository tarjetaRepository;
        private readonly IMapper mapper;

        public TarjetaService(ITarjetaRepository genericRepository, IMapper _mapper) : base(genericRepository, _mapper)
        {
            this.tarjetaRepository = genericRepository;
            this.mapper = _mapper;
        }


        public override async Task<TarjetaResponseDto?> AddAsync(SaveTarjetaDto? dto)
        {
            var response = new TarjetaResponseDto() { Errors = [], Message = "", HasError = false };
            try
            {

                if (dto == null)
                {
                    response.HasError = true;
                    response.Errors.Add("Ocurrio un error al intentar guardar la tarjeta, no se enviaron los datos correctamente");
                    return response;
                }

                if (!Enum.IsDefined(typeof(TipoTarjeta), dto.Tipo))
                {
                    response.HasError = true;
                    response.Errors.Add("El tipo de tarjeta no es valido, favor verificar");
                    return response;

                }




                if (dto.Tipo == TipoTarjeta.AmericanExpress)
                {
                    if (dto.CVV.Trim().Length != 4)
                    {
                        response.HasError = true;
                        response.Errors.Add("Si el tipo de tarjeta es American Express el CVV debe tener 4 digitos");
                        return response;

                    }

                }



                if (dto.Tipo == TipoTarjeta.Visa || dto.Tipo == TipoTarjeta.Mastercard)
                {
                    if (dto.CVV.Trim().Length != 3)
                    {
                        response.HasError = true;
                        response.Errors.Add("Si el tipo de tarjeta es visa o mastercard el CVV debe tener 3 digitos");
                        return response;

                    }
                }


                 bool longitudValida =
                (dto.Tipo == TipoTarjeta.Visa && dto.NumeroTarjeta.Trim().Length == 16)
                 || (dto.Tipo == TipoTarjeta.Mastercard && dto.NumeroTarjeta.Trim().Length == 16)
                 || (dto.Tipo == TipoTarjeta.AmericanExpress && dto.NumeroTarjeta.Trim().Length == 15);

                if (!longitudValida)
                {
                    response.HasError = true;
                    response.Errors.Add("La longitud del número de tarjeta no corresponde con el tipo seleccionado.");
                    return response;
                }



                if (dto.MesVencimiento < 0 || dto.MesVencimiento > 12)
                {
                    response.HasError = true;
                    response.Errors.Add("El anio de vencimiento debe ser valido, entre 1 y 12");
                    return response;

                }


                int anioActual = DateTime.Now.Year;
                int mesActual = DateTime.Now.Month;
                if (dto.AnioVencimiento < anioActual)
                {
                    response.HasError = true;
                    response.Errors.Add("El año de vencimiento no puede ser menor al año actual");
                    return response;
                }


                if (dto.MesVencimiento < mesActual && dto.AnioVencimiento < anioActual)
                {

                    response.HasError = true;
                    response.Errors.Add("La tarjeta ingresada esta vencida, favor verifivar y volver a intentar");
                    return response;
                }

                int mesVencimiento = dto.MesVencimiento % 100;
                int anioVencimiento = dto.AnioVencimiento % 100;


                var datos = await tarjetaRepository.AddAsync(new Tarjeta()
                {
                    MesVencimiento = mesVencimiento,
                    AnioVencimiento = anioVencimiento,
                    FechaRegistro = DateTime.Now,
                    CVV = dto.CVV,
                    NombreTitular = dto.NombreTitular,
                    NumeroTarjeta = dto.NumeroTarjeta,
                    Tipo = dto.Tipo,
                    UsuarioId = dto.UsuarioId,
                });

                var map = mapper.Map<TarjetaResponseDto>(datos);
                map.Message = "Tarjeta agregada correctamente";
                return map;

            }
            catch (Exception ex)
            {

                throw new Exception("Ocurrio un error al intentar guardar la tarjeta" + ex.Message);

            }

        }





        public override async Task<TarjetaResponseDto?> UpdateAsync(int id, EditTarjetaDto? dto)
        {
            var response = new TarjetaResponseDto() { Errors = [], Message = "", HasError = false };
            try
            {

                if (dto == null)
                {
                    response.HasError = true;
                    response.Errors.Add("Ocurrio un error al intentar guardar la tarjeta, no se enviaron los datos correctamente");
                    return response;
                }

                if (dto.Tipo != TipoTarjeta.AmericanExpress || dto.Tipo != TipoTarjeta.Visa || dto.Tipo != TipoTarjeta.Mastercard)
                {
                    response.HasError = true;
                    response.Errors.Add("El tipo de tarjeta no es valido, favor verificar");
                    return response;

                }



                if (dto.Tipo == TipoTarjeta.AmericanExpress)
                {
                    if (dto.CVV.Length != 4)
                    {
                        response.HasError = true;
                        response.Errors.Add("Si el tipo de tarjeta es American Express el CVV debe tener 4 digitos");

                    }

                }

                if (dto.Tipo == TipoTarjeta.Visa || dto.Tipo == TipoTarjeta.Mastercard)
                {
                    if (dto.CVV.Length != 3)
                    {
                        response.HasError = true;
                        response.Errors.Add("Si el tipo de tarjeta es visa o mastercard el CVV debe tener 3 digitos");
                        return response;

                    }
                }


                if (dto.MesVencimiento < 0 || dto.MesVencimiento > 12)
                {
                    response.HasError = true;
                    response.Errors.Add("El anio de vencimiento debe ser valido, entre 1 y 12");
                    return response;

                }


                int anioActual = DateTime.Now.Year;
                int mesActual = DateTime.Now.Month;
                if (dto.AnioVencimiento < anioActual)
                {
                    response.HasError = true;
                    response.Errors.Add("El año de vencimiento no puede ser menor al año actual");
                    return response;
                }


                if (dto.MesVencimiento < mesActual && dto.AnioVencimiento < anioActual)
                {

                    response.HasError = true;
                    response.Errors.Add("La tarjeta ingresada esta vencida, favor verifivar y volver a intentar");
                    return response;
                }

                int mesVencimiento = dto.MesVencimiento % 100;
                int anioVencimiento = dto.AnioVencimiento % 100;

                var tarjetaExist = await tarjetaRepository.GetByIdAsync(id);

                if (tarjetaExist == null)
                {
                    response.HasError = true;
                    response.Errors.Add("No existe una tarjeta con el id especificado,favor verificar");
                    return response;

                }


                var datos = await tarjetaRepository.UpdateAsync(id, new Tarjeta()
                {
                    MesVencimiento = mesVencimiento,
                    AnioVencimiento = anioVencimiento,
                    FechaRegistro = tarjetaExist.FechaRegistro,
                    CVV = dto.CVV,
                    NombreTitular = dto.NombreTitular,
                    NumeroTarjeta = dto.NumeroTarjeta,
                    Tipo = dto.Tipo,
                    UsuarioId = dto.UsuarioId,
                });

                var map = mapper.Map<TarjetaResponseDto>(datos);
                map.Message = "Tarjeta agregada correctamente";
                return map;

            }
            catch (Exception ex)
            {

                throw new Exception("Ocurrio un error al intentar guardar la tarjeta" + ex.Message);

            }

        }























    }
}

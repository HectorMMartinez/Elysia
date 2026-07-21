using AutoMapper;
using Elysia.Core.Application.Dtos.reservas;
using Elysia.Core.Application.Interfaces;
using Elysia.Core.Domain.Common;
using Elysia.Core.Domain.Entities;
using Elysia.Core.Domain.interfaces;


namespace Elysia.Core.Application.Services
{
    public class ReservaServices : GenericService<Reserva, ReservaResponseDto, EditarReservaDto, CreateReservaDto>, IReservaServices
    {
        private readonly IMapper _mapper;
        private readonly IReservasRepository repo;
        private readonly IMesaRepository mesaRepository;

        public ReservaServices(IReservasRepository repo, IMesaRepository mesaRepository, IMapper _mapper) : base(repo, _mapper)
        {
            this.repo = repo;
            this._mapper = _mapper;
            this.mesaRepository = mesaRepository;
        }



        public override async Task<ReservaResponseDto?> AddAsync(CreateReservaDto? dto)
        {      
             var response = new ReservaResponseDto() { HasError = false, Errors = [] };

            try
            {
                if(dto == null)
                {
                    response.HasError = true;
                    response.Errors.Add("Debes ingresar los datos correctamente para agregar la reserva");
                    return response;
                }

                var fechaActual = DateTime.Now;

                if(dto.DNICliente.Length != 11)
                {
                    response.HasError = true;
                    response.Errors.Add("El DNI del cliente debe tener 11 digitos sin guiones o otro caracter");
                    return response;    
                }

                var reservas = await repo.GetAllReservasByMesaId(dto.MesaId);
                var mesa = await mesaRepository.GetByIdAsync(dto.MesaId);

                if (reservas.Count > 0 || reservas.Any()) {

                    foreach (var reserva in reservas)
                    {
                        if(reserva.Estado == EstadoReserva.Activa || reserva.Estado == EstadoReserva.EnProceso)
                        {
                            if(reserva.FechaReserva == dto.FechaReserva)
                            {
                                response.HasError = true;
                                response.Errors.Add("La reserva no  se puede realizar, esa mesa  esta reservada para la fecha indicada");
                                return response;
                            }
                        }
                    
                    
                    }
                   
                }



                if(mesa == null)
                {
                    response.HasError = true;
                    response.Errors.Add("No se encontro una mesa con ese id, favor verificar no puede reservar una mesa inexistente");
                    return response;
                }

                if(mesa.Capacidad < dto.CantidadPersona)
                {
                    response.HasError = true;
                    response.Errors.Add("La cantidad de personas que ocuparan la reserva no pueder ser mayor a la capacidad de la mesa");
                    return response;
                }


                if (dto.FechaReserva < fechaActual)
                {
                   response.HasError = true;
                   response.Errors.Add("La fecha de la reserva no puede ser anteriol a la fecha actual, no puede reservar para esa fecha");
                   return response;
                }

                if (dto.FechaActualizacion < dto.FechaCreacion)
                {
                    response.HasError = true;
                    response.Errors.Add("La fecha de actualizacion no puede ser anteriol a la fecha de creacion");
                    return response;
                }

                if(dto.FechaReserva < dto.FechaCreacion)
                {
                    response.HasError = true;
                    response.Errors.Add("La fecha de reserva no pueder anterior a la fecha de creacion");
                    return response;
                }

                var data = await base.AddAsync(dto);
                mesa.Estado = MesaEstado.Reservada;
                await mesaRepository.UpdateAsync(mesa.Id,mesa);
                var map = _mapper.Map<ReservaResponseDto>(data);
                return map;



            }
            catch (Exception ex)
            {

                throw new Exception("Ocurrio un error al intentar guardar la reserva");
            
            
            }
        }





        public override async Task<ReservaResponseDto?> UpdateAsync(int id, EditarReservaDto? dto)
        {
            var response = new ReservaResponseDto() { HasError = false, Errors = [] };

            try
            {
                if (dto == null)
                {
                    response.HasError = true;
                    response.Errors.Add("Debes ingresar los datos correctamente para agregar la reserva");
                    return response;
                }


                var reserva = await repo.GetByIdAsync(id);

                if(reserva == null)
                {
                    response.HasError = true;
                    response.Errors.Add("No se encontro una reserva con ese id");
                    return response;
                }

                var reservas = await repo.GetAllReservasByMesaId(dto.MesaId);
                var mesa = await mesaRepository.GetByIdAsync(dto.MesaId);


                if (reservas.Count > 0 || reservas.Any())
                {

                    foreach (var _reserva in reservas)
                    {
                        if (_reserva.Estado == EstadoReserva.Activa || _reserva.Estado == EstadoReserva.EnProceso)
                        {
                            if (_reserva.FechaReserva == dto.FechaReserva)
                            {
                                response.HasError = true;
                                response.Errors.Add("La reserva no  se puede realizar, esa mesa  esta reservada para la fecha indicada");
                                return response;
                            }
                        }


                    }

                }




                if (mesa == null)
                {
                    response.HasError = true;
                    response.Errors.Add("No se encontro una mesa con ese id, favor verificar no puede reservar una mesa inexistente");
                    return response;
                }

                if (mesa.Capacidad < dto.CantidadPersona)
                {
                    response.HasError = true;
                    response.Errors.Add("La cantidad de personas que ocuparan la reserva no pueder ser mayor a la capacidad de la mesa");
                    return response;
                }

                if (dto.DNICliente.Length != 11)
                {
                    response.HasError = true;
                    response.Errors.Add("El DNI del cliente debe tener 11 digitos sin guiones o otro caracter");
                    return response;
                }

                var fechaActual = DateTime.Now;

                if (dto.FechaReserva < fechaActual)
                {
                    response.HasError = true;
                    response.Errors.Add("La fecha de la reserva no puede ser anteriol a la fecha actual, no puede reservar para esa fecha");
                    return response;
                }

                if (dto.FechaActualizacion < dto.FechaCreacion)
                {
                    response.HasError = true;
                    response.Errors.Add("La fecha de actualizacion no puede ser anteriol a la fecha de creacion");
                    return response;
                }

                if (dto.FechaCreacion < dto.FechaReserva)
                {
                    response.HasError = true;
                    response.Errors.Add("La fecha de reserva no pueder anterior a la fecha de creacion");
                }

                if(dto.Estado == EstadoReserva.EnProceso)
                {
                    mesa.Estado = MesaEstado.Ocupada;
                    await mesaRepository.UpdateAsync(mesa.Id,mesa);
                }


                if(dto.Estado == EstadoReserva.Finalizada)
                {
                    mesa.Estado = MesaEstado.Disponible;
                    await mesaRepository.UpdateAsync(mesa.Id, mesa);
                }
                
                if(dto.Estado == EstadoReserva.Cancelada)
                {
                    mesa.Estado = MesaEstado.Disponible;
                    await mesaRepository.UpdateAsync(mesa.Id, mesa);
                }



                if (dto.Estado == EstadoReserva.NoAsistio)
                {
                    mesa.Estado = MesaEstado.Disponible;
                    await mesaRepository.UpdateAsync(mesa.Id, mesa);
                }



                dto.FechaCreacion = reserva.FechaCreacion;
                dto.IdPropietario = reserva.IdPropietario;
                dto.Id = reserva.Id;    
                var data = await base.UpdateAsync(id,dto);
                var map = _mapper.Map<ReservaResponseDto>(data);
                return map;



            }
            catch (Exception ex)
            {

                throw new Exception("Ocurrio un error al intentar guardar la reserva");


            }
        }



        public async Task<List<ReservaResponseDto?>> GetReservasActivasByPropietario(string propietario)
        {
            try
            {
                var data = await repo.GetReservasActivasByPropietario(propietario);

                if (data == null) {

                    return [];
                   
                }

                var map = _mapper.Map<List<ReservaResponseDto>>(data);
                return map;

            }
            catch (Exception ex)
            {


                throw new Exception("Ocurrio un error al intentar obtener las reservas con estado (activas)");
            
            
            }
        }



        public async Task<List<ReservaResponseDto?>> GetReservasCanceladaByPropietario(string propietario)
        {
            try
            {
                var data = await repo.GetReservasCanceladaByPropietario(propietario);

                if (data == null)
                {

                    return [];

                }

                var map = _mapper.Map<List<ReservaResponseDto>>(data);
                return map;

            }
            catch (Exception ex)
            {


                throw new Exception("Ocurrio un error al intentar obtener las reservas con estado (Canceladas)");


            }
        }





        public async Task<List<ReservaResponseDto?>> GetReservasFinalizadasByPropietario(string propietario)
        {

            try
            {
                var data = await repo.GetReservasFinalizadasByPropietario(propietario);

                if (data == null)
                {

                    return [];

                }

                var map = _mapper.Map<List<ReservaResponseDto>>(data);
                return map;

            }
            catch (Exception ex)
            {


                throw new Exception("Ocurrio un error al intentar obtener las reservas con estado (Finalizada)");


            }

        }




        public async Task<List<ReservaResponseDto?>> GetReservasNoAsistioByPropietario(string propietario)
        {
            try
            {
                var data = await repo.GetReservasNoAsistioByPropietario(propietario);

                if (data == null)
                {

                    return [];

                }

                var map = _mapper.Map<List<ReservaResponseDto>>(data);
                return map;

            }
            catch (Exception ex)
            {


                throw new Exception("Ocurrio un error al intentar obtener las reservas con estado (NoAsistio)");


            }
        }


        public async Task<List<ReservaResponseDto?>> GetAllReservasByPropietario(string propietario)
        {

            var response = new ReservaResponseDto() { HasError = false, Errors = [] };
            try
            {
                var data = await repo.GetAllReservasByPropietario(propietario);

                if (data.Any())
                {
                    var map = _mapper.Map<List<ReservaResponseDto>>(data);
                    return map;
                }

                return [];


            }catch(Exception ex)
            {

                throw new Exception("Ocurrio un error al intentar obtener todas las reservas del propietario");


            }

        }







    }
}

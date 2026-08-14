// src/services/reservaService.js

import axiosClient from "../api/axiosClient";
import ENDPOINTS from "../api/endpoints";



const validarId = (id, entidad = "reserva") => {
  const idNumerico = Number(id);

  if (!Number.isInteger(idNumerico) || idNumerico <= 0) {
    throw new Error(`El id de la ${entidad} no es válido.`);
  }

  return idNumerico;
};

const obtenerMensajeError = (error) => {
  const data = error.response?.data;

  console.error("Error de API (Reserva):", {
    status: error.response?.status,
    data,
  });

  if (typeof data === "string") {
    return data;
  }

  if (data?.errors) {
    if (Array.isArray(data.errors)) {
      return data.errors.join(" ");
    }

    return Object.entries(data.errors)
      .flatMap(([campo, mensajes]) => {
        const lista = Array.isArray(mensajes) ? mensajes : [mensajes];
        return lista.map((mensaje) => `${campo}: ${mensaje}`);
      })
      .join(" ");
  }

  if (data?.title) {
    return data.title;
  }

  if (data?.message) {
    return data.message;
  }

  return (
    error.message || "Ocurrió un error al procesar la solicitud de reserva."
  );
};


const ESTADOS_RESERVA = [
  "Activa",
  "EnProceso",
  "NoAsistio",
  "Finalizada",
  "Cancelada",
];

const validarReserva = (reserva, esEdicion = false) => {
  if (!reserva) {
    throw new Error("Los datos de la reserva son obligatorios.");
  }

  if (!reserva.nombreCliente?.trim()) {
    throw new Error("El nombre del cliente es obligatorio.");
  }

  if (!reserva.dniCliente?.trim()) {
    throw new Error("El DNI del cliente es obligatorio.");
  }

  const mesaId = Number(reserva.mesaId);
  if (!Number.isInteger(mesaId) || mesaId <= 0) {
    throw new Error("Debes seleccionar una mesa válida.");
  }

  const cantidadPersona = Number(reserva.cantidadPersona);
  if (!Number.isInteger(cantidadPersona) || cantidadPersona <= 0) {
    throw new Error("La cantidad de personas debe ser mayor que cero.");
  }

  if (!reserva.fechaReserva) {
    throw new Error("La fecha de la reserva es obligatoria.");
  }

  // Validar que sea una fecha válida, pero SIN convertir a UTC
  const fecha = new Date(reserva.fechaReserva);
  if (isNaN(fecha.getTime())) {
    throw new Error("La fecha de la reserva no es válida.");
  }

  return {
    nombreCliente: reserva.nombreCliente.trim(),
    dniCliente: reserva.dniCliente.trim(),
    mesaId,
    cantidadPersona,
    // 🔑 Enviar exactamente lo que viene del front (hora local)
    fechaReserva: reserva.fechaReserva,
    observaciones: reserva.observaciones?.trim() || null,
    ...(esEdicion && { estado: reserva.estado }),
  };
};



export const obtenerReservas = async () => {
  try {
    const response = await axiosClient.get(ENDPOINTS.RESERVA.GET_ALL);
    return response.data;
  } catch (error) {
    throw new Error(obtenerMensajeError(error), { cause: error });
  }
};

export const obtenerReservaPorId = async (id) => {
  const reservaId = validarId(id);

  try {
    const response = await axiosClient.get(
      ENDPOINTS.RESERVA.GET_BY_ID(reservaId)
    );
    return response.data;
  } catch (error) {
    throw new Error(obtenerMensajeError(error), { cause: error });
  }
};

export const crearReserva = async (reserva) => {
  const datos = validarReserva(reserva);

  try {
    const response = await axiosClient.post(
      ENDPOINTS.RESERVA.CREATE,
      datos
    );
    return response.data;
  } catch (error) {
    throw new Error(obtenerMensajeError(error), { cause: error });
  }
};

export const editarReserva = async (id, reserva) => {
  const reservaId = validarId(id);
  const datos = validarReserva(reserva, true);

  try {
    const response = await axiosClient.put(
      ENDPOINTS.RESERVA.UPDATE(reservaId),
      datos
    );
    return response.data;
  } catch (error) {
    throw new Error(obtenerMensajeError(error), { cause: error });
  }
};

export const eliminarReserva = async (id) => {
  const reservaId = validarId(id);

  try {
    await axiosClient.delete(ENDPOINTS.RESERVA.DELETE(reservaId));
    return true;
  } catch (error) {
    throw new Error(obtenerMensajeError(error), { cause: error });
  }
};

export const cambiarReservaEnProceso = async (id) => {
  const reservaId = validarId(id);

  try {
    const response = await axiosClient.put(
      ENDPOINTS.RESERVA.CAMBIAR_EN_PROCESO(reservaId)
    );
    return response.data;
  } catch (error) {
    throw new Error(obtenerMensajeError(error), { cause: error });
  }
};

export const cambiarReservaNoAsistio = async (id) => {
  const reservaId = validarId(id);

  try {
    const response = await axiosClient.put(
      ENDPOINTS.RESERVA.CAMBIAR_NO_ASISTIO(reservaId)
    );
    return response.data;
  } catch (error) {
    throw new Error(obtenerMensajeError(error), { cause: error });
  }
};

export const finalizarReserva = async (id) => {
  const reservaId = validarId(id);

  try {
    const response = await axiosClient.put(
      ENDPOINTS.RESERVA.FINALIZAR(reservaId)
    );
    return response.data;
  } catch (error) {
    throw new Error(obtenerMensajeError(error), { cause: error });
  }
};

export const cancelarReserva = async (id) => {
  const reservaId = validarId(id);

  try {
    const response = await axiosClient.put(
      ENDPOINTS.RESERVA.CANCELAR(reservaId)
    );
    return response.data;
  } catch (error) {
    throw new Error(obtenerMensajeError(error), { cause: error });
  }
};

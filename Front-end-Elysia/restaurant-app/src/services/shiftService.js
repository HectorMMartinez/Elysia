import axiosClient from "../api/axiosClient";
import ENDPOINTS from "../api/endpoints";

const validarId = (id, entidad = "turno") => {
  const idNumerico = Number(id);
  if (!Number.isInteger(idNumerico) || idNumerico <= 0) {
    throw new Error(`El id del ${entidad} no es válido.`);
  }
  return idNumerico;
};

const obtenerMensajeError = (error) => {
  const data = error.response?.data;

  console.error("Error de API (Shift):", {
    status: error.response?.status,
    data,
  });

  if (typeof data === "string") return data;

  if (data?.errors) {
    if (Array.isArray(data.errors)) return data.errors.join(" ");
    return Object.entries(data.errors)
      .flatMap(([campo, mensajes]) => {
        const lista = Array.isArray(mensajes) ? mensajes : [mensajes];
        return lista.map((m) => `${campo}: ${m}`);
      })
      .join(" ");
  }

  if (data?.title) return data.title;
  if (data?.message) return data.message;

  return error.message || "Ocurrió un error al procesar la solicitud del turno.";
};


const validarTurno = (turno) => {
  if (!turno) throw new Error("Los datos del turno son obligatorios.");

  const name = turno.name?.trim();
  if (!name) throw new Error("El nombre del turno es obligatorio.");

  if (!turno.startTime) throw new Error("La hora de inicio es obligatoria.");
  if (!turno.endTime) throw new Error("La hora de fin es obligatoria.");

  // Convertimos a string "HH:mm" si viene como TimeOnly o Date
  const start = typeof turno.startTime === "string" 
    ? turno.startTime 
    : turno.startTime?.toString?.().slice(0, 5) || turno.startTime;

  const end = typeof turno.endTime === "string" 
    ? turno.endTime 
    : turno.endTime?.toString?.().slice(0, 5) || turno.endTime;

  if (start >= end) {
    throw new Error("La hora de inicio debe ser anterior a la hora de fin.");
  }

  return {
    name,
    startTime: start,
    endTime: end,
  };
};

const validarAsociacion = (data) => {
  if (!data) throw new Error("Los datos de la asociación son obligatorios.");

  const empleadoId = Number(data.empleadoId);
  const shiftId = Number(data.shiftId);
  const workDate = data.workDate;

  if (!Number.isInteger(empleadoId) || empleadoId <= 0) {
    throw new Error("Debes seleccionar un empleado válido.");
  }
  if (!Number.isInteger(shiftId) || shiftId <= 0) {
    throw new Error("Debes seleccionar un turno válido.");
  }
  if (!workDate) {
    throw new Error("La fecha de trabajo es obligatoria.");
  }

 
  const hoy = new Date();
  hoy.setHours(0, 0, 0, 0);
  const fechaSeleccionada = new Date(workDate);
  if (fechaSeleccionada < hoy) {
    throw new Error("La fecha de trabajo no puede ser anterior a hoy.");
  }

  return {
    empleadoId,
    shiftId,
    workDate,
  };
};


export const obtenerTurnos = async () => {
  try {
    const response = await axiosClient.get(ENDPOINTS.SHIFT.GET_ALL);
    return response.data;
  } catch (error) {
    throw new Error(obtenerMensajeError(error), { cause: error });
  }
};

export const obtenerTurnoPorId = async (id) => {
  const turnoId = validarId(id);
  try {
    const response = await axiosClient.get(ENDPOINTS.SHIFT.GET_BY_ID(turnoId));
    return response.data;
  } catch (error) {
    throw new Error(obtenerMensajeError(error), { cause: error });
  }
};

export const crearTurno = async (turno) => {
  const datos = validarTurno(turno);
  try {
    const response = await axiosClient.post(ENDPOINTS.SHIFT.CREATE, datos);
    return response.data;
  } catch (error) {
    throw new Error(obtenerMensajeError(error), { cause: error });
  }
};

export const editarTurno = async (id, turno) => {
  const turnoId = validarId(id);
  const datos = validarTurno(turno);
  try {
    const response = await axiosClient.put(ENDPOINTS.SHIFT.UPDATE(turnoId), datos);
    return response.data;
  } catch (error) {
    throw new Error(obtenerMensajeError(error), { cause: error });
  }
};

export const eliminarTurno = async (id) => {
  const turnoId = validarId(id);
  try {
    await axiosClient.delete(ENDPOINTS.SHIFT.DELETE(turnoId));
    return true;
  } catch (error) {
    throw new Error(obtenerMensajeError(error), { cause: error });
  }
};


export const obtenerEmpleadosConTurnos = async () => {
  try {
    const response = await axiosClient.get(ENDPOINTS.SHIFT.GET_ALL_EMPLEADOS_TURNOS);
    return response.data;
  } catch (error) {
    throw new Error(obtenerMensajeError(error), { cause: error });
  }
};

export const asociarEmpleadoATurno = async (data) => {
  const datos = validarAsociacion(data);
  try {
    const response = await axiosClient.post(ENDPOINTS.SHIFT.ASOCIAR_EMPLEADO, datos);
    return response.data;
  } catch (error) {
    throw new Error(obtenerMensajeError(error), { cause: error });
  }
};

export const desasociarEmpleadoDeTurno = async (id) => {
  const asociacionId = validarId(id, "asociación");
  try {
    await axiosClient.delete(ENDPOINTS.SHIFT.DESASOCIAR_EMPLEADO(asociacionId));
    return true;
  } catch (error) {
    throw new Error(obtenerMensajeError(error), { cause: error });
  }
};
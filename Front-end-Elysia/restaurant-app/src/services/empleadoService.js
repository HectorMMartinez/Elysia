import axiosClient from "../api/axiosClient";
import ENDPOINTS from "../api/endpoints";

const validarId = (id, entidad = "empleado") => {
  const idNumerico = Number(id);
  if (!Number.isInteger(idNumerico) || idNumerico <= 0) {
    throw new Error(`El id del ${entidad} no es válido.`);
  }
  return idNumerico;
};

const obtenerMensajeError = (error) => {
  const data = error.response?.data;

  console.error("Error de API (Empleado):", {
    status: error.response?.status,
    data,
  });

  if (typeof data === "string") return data;

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

  if (data?.title) return data.title;
  if (data?.message) return data.message;

  return error.message || "Ocurrió un error al procesar la solicitud del empleado.";
};

// ===================== VALIDACIONES =====================
const validarEmpleadoCreate = (empleado) => {
  if (!empleado) throw new Error("Los datos del empleado son obligatorios.");

  const firstName = empleado.firstName?.trim();
  const lastName = empleado.lastName?.trim();
  const email = empleado.email?.trim();
  const phone = empleado.phone?.trim();
  const salary = Number(empleado.salary);
  const puestoId = Number(empleado.puestoId);

  if (!firstName) throw new Error("El nombre del empleado es obligatorio.");
  if (!lastName) throw new Error("El apellido del empleado es obligatorio.");
  if (!email) throw new Error("El correo del empleado es obligatorio.");
  if (!phone) throw new Error("El teléfono del empleado es obligatorio.");
  if (isNaN(salary) || salary <= 0) throw new Error("Debes ingresar un salario válido mayor a 0.");
  if (!Number.isInteger(puestoId) || puestoId <= 0) throw new Error("Debes seleccionar un puesto válido.");

  return {
    firstName,
    lastName,
    email,
    phone,
    salary,
    puestoId,
  };
};

const validarEmpleadoEdit = (empleado) => {
  if (!empleado) throw new Error("Los datos del empleado son obligatorios.");

  const firstName = empleado.firstName?.trim();
  const lastName = empleado.lastName?.trim();
  const email = empleado.email?.trim() || "";
  const phone = empleado.phone?.trim() || "";
  const salary = Number(empleado.salary);
  const puestoId = Number(empleado.puestoId);

  if (!firstName) throw new Error("El nombre del empleado es obligatorio.");
  if (!lastName) throw new Error("El apellido del empleado es obligatorio.");
  if (isNaN(salary) || salary <= 0) throw new Error("Debes ingresar un salario válido mayor a 0.");
  if (!Number.isInteger(puestoId) || puestoId <= 0) throw new Error("Debes seleccionar un puesto válido.");

  return {
    firstName,
    lastName,
    email,
    phone,
    salary,
    puestoId,
  };
};

// ===================== API CALLS =====================
export const obtenerEmpleados = async () => {
  try {
    const response = await axiosClient.get(ENDPOINTS.EMPLEADO.GET_ALL);
    return response.data;
  } catch (error) {
    throw new Error(obtenerMensajeError(error), { cause: error });
  }
};

export const obtenerEmpleadosActivos = async () => {
  try {
    const response = await axiosClient.get(ENDPOINTS.EMPLEADO.GET_ALL_ACTIVOS);
    return response.data;
  } catch (error) {
    throw new Error(obtenerMensajeError(error), { cause: error });
  }
};

export const obtenerEmpleadoPorId = async (id) => {
  const empleadoId = validarId(id);
  try {
    const response = await axiosClient.get(ENDPOINTS.EMPLEADO.GET_BY_ID(empleadoId));
    return response.data;
  } catch (error) {
    throw new Error(obtenerMensajeError(error), { cause: error });
  }
};

export const crearEmpleado = async (empleado) => {
  const datos = validarEmpleadoCreate(empleado);
  try {
    const response = await axiosClient.post(ENDPOINTS.EMPLEADO.CREATE, datos);
    return response.data;
  } catch (error) {
    throw new Error(obtenerMensajeError(error), { cause: error });
  }
};

export const editarEmpleado = async (id, empleado) => {
  const empleadoId = validarId(id);
  const datos = validarEmpleadoEdit(empleado);
  try {
    const response = await axiosClient.put(ENDPOINTS.EMPLEADO.UPDATE(empleadoId), datos);
    return response.data;
  } catch (error) {
    throw new Error(obtenerMensajeError(error), { cause: error });
  }
};

export const eliminarEmpleado = async (id) => {
  const empleadoId = validarId(id);
  try {
    await axiosClient.delete(ENDPOINTS.EMPLEADO.DELETE(empleadoId));
    return true;
  } catch (error) {
    throw new Error(obtenerMensajeError(error), { cause: error });
  }
};

export const activarEmpleado = async (id) => {
  const empleadoId = validarId(id);
  try {
    const response = await axiosClient.put(ENDPOINTS.EMPLEADO.ACTIVAR(empleadoId));
    return response.data;
  } catch (error) {
    throw new Error(obtenerMensajeError(error), { cause: error });
  }
};

export const inactivarEmpleado = async (id) => {
  const empleadoId = validarId(id);
  try {
    const response = await axiosClient.put(ENDPOINTS.EMPLEADO.INACTIVAR(empleadoId));
    return response.data;
  } catch (error) {
    throw new Error(obtenerMensajeError(error), { cause: error });
  }
};

// Puestos (para el select del formulario)
export const obtenerPuestos = async () => {
  try {
    const response = await axiosClient.get(ENDPOINTS.PUESTO.GET_ALL);
    return response.data;
  } catch (error) {
    throw new Error(obtenerMensajeError(error), { cause: error });
  }
};
import axiosClient from "../api/axiosClient";
import ENDPOINTS from "../api/endpoints";


const validarId = (id, entidad = "pedido") => {
  const idNumerico = Number(id);

  if (!Number.isInteger(idNumerico) || idNumerico <= 0) {
    throw new Error(`El id del ${entidad} no es válido.`);
  }

  return idNumerico;
};

const obtenerMensajeError = (error) => {
  const data = error.response?.data;

  console.error("Error de API (Pedido):", {
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
    error.message || "Ocurrió un error al procesar la solicitud del pedido."
  );
};



const ESTADOS_PEDIDO = [
  "Pendiente",
  "EnPreparacion",
  "Listo",
  "Cancelado",
  "Finalizado",
];

const validarDetalle = (detalle) => {
  const platoId = Number(detalle?.platoId ?? detalle?.PlatoId);
  const cantidad = Number(detalle?.cantidad ?? detalle?.Cantidad);

  if (!Number.isInteger(platoId) || platoId <= 0) {
    throw new Error("Debes seleccionar un plato válido en cada detalle.");
  }

  if (!Number.isInteger(cantidad) || cantidad <= 0) {
    throw new Error("La cantidad de cada plato debe ser mayor que cero.");
  }

  return {
    platoId,
    cantidad,
    observaciones: detalle?.observaciones?.trim() || detalle?.Observaciones?.trim() || null,
  };
};

const validarPedidoCreate = (pedido) => {
  if (!pedido) {
    throw new Error("Los datos del pedido son obligatorios.");
  }

  const idMesa = Number(pedido.idMesa);
  if (!Number.isInteger(idMesa) || idMesa <= 0) {
    throw new Error("Debes seleccionar una mesa válida.");
  }

  if (!Array.isArray(pedido.detallesPedido) || pedido.detallesPedido.length === 0) {
    throw new Error("Debes agregar al menos un plato al pedido.");
  }

  const detalles = pedido.detallesPedido.map(validarDetalle);

  // No permitir el mismo plato repetido
  const ids = detalles.map((d) => d.platoId);
  if (new Set(ids).size !== ids.length) {
    throw new Error("No puedes repetir el mismo plato en el pedido. Suma la cantidad en uno solo.");
  }

  return {
    idMesa,
    detallesPedido: detalles,
  };
};

const validarPedidoEdit = (pedido) => {
  if (!pedido) {
    throw new Error("Los datos del pedido son obligatorios.");
  }

  if (!pedido.estado || !ESTADOS_PEDIDO.includes(pedido.estado)) {
    throw new Error("Debes indicar un estado válido para el pedido.");
  }

  const payload = {
    estado: pedido.estado,
    detallesPedido: [],
    idMesa: 0,
  };

  // Mesa opcional en edición
  if (pedido.idMesa != null && pedido.idMesa !== "") {
    const idMesa = Number(pedido.idMesa);
    if (Number.isInteger(idMesa) && idMesa > 0) {
      payload.idMesa = idMesa;
    }
  }

  // Detalles opcionales en edición
  if (Array.isArray(pedido.detallesPedido) && pedido.detallesPedido.length > 0) {
    payload.detallesPedido = pedido.detallesPedido.map(validarDetalle);

    const ids = payload.detallesPedido.map((d) => d.platoId);
    if (new Set(ids).size !== ids.length) {
      throw new Error("No puedes repetir el mismo plato en el pedido.");
    }
  }

  return payload;
};


export const obtenerPedidos = async () => {
  try {
    const response = await axiosClient.get(ENDPOINTS.PEDIDO.GET_ALL);
    return response.data;
  } catch (error) {
    throw new Error(obtenerMensajeError(error), { cause: error });
  }
};

export const obtenerPedidoPorId = async (id) => {
  const pedidoId = validarId(id);

  try {
    const response = await axiosClient.get(
      ENDPOINTS.PEDIDO.GET_BY_ID(pedidoId)
    );
    return response.data;
  } catch (error) {
    throw new Error(obtenerMensajeError(error), { cause: error });
  }
};

export const crearPedido = async (pedido) => {
  const datos = validarPedidoCreate(pedido);

  try {
    const response = await axiosClient.post(ENDPOINTS.PEDIDO.CREATE, datos);
    return response.data;
  } catch (error) {
    throw new Error(obtenerMensajeError(error), { cause: error });
  }
};

export const editarPedido = async (id, pedido) => {
  const pedidoId = validarId(id);
  const datos = validarPedidoEdit(pedido);

  try {
    const response = await axiosClient.put(
      ENDPOINTS.PEDIDO.UPDATE(pedidoId),
      datos
    );
    return response.data;
  } catch (error) {
    throw new Error(obtenerMensajeError(error), { cause: error });
  }
};

export const eliminarPedido = async (id) => {
  const pedidoId = validarId(id);

  try {
    await axiosClient.delete(ENDPOINTS.PEDIDO.DELETE(pedidoId));
    return true;
  } catch (error) {
    throw new Error(obtenerMensajeError(error), { cause: error });
  }
};

export const cambiarPedidoEnPreparacion = async (id) => {
  const pedidoId = validarId(id);

  try {
    const response = await axiosClient.put(
      ENDPOINTS.PEDIDO.CAMBIAR_EN_PREPARACION(pedidoId)
    );
    return response.data;
  } catch (error) {
    throw new Error(obtenerMensajeError(error), { cause: error });
  }
};

export const cambiarPedidoListo = async (id) => {
  const pedidoId = validarId(id);

  try {
    const response = await axiosClient.put(
      ENDPOINTS.PEDIDO.CAMBIAR_LISTO(pedidoId)
    );
    return response.data;
  } catch (error) {
    throw new Error(obtenerMensajeError(error), { cause: error });
  }
};

export const cancelarPedido = async (id) => {
  const pedidoId = validarId(id);

  try {
    const response = await axiosClient.put(
      ENDPOINTS.PEDIDO.CANCELAR(pedidoId)
    );
    return response.data;
  } catch (error) {
    throw new Error(obtenerMensajeError(error), { cause: error });
  }
};

export const finalizarPedido = async (id) => {
  const pedidoId = validarId(id);

  try {
    const response = await axiosClient.put(
      ENDPOINTS.PEDIDO.FINALIZAR(pedidoId)
    );
    return response.data;
  } catch (error) {
    throw new Error(obtenerMensajeError(error), { cause: error });
  }
};
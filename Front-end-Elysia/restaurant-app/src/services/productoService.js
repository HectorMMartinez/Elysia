import axiosClient from "../api/axiosClient";
import ENDPOINTS from "../api/endpoints";

const validarId = (id) => {
  const idNumerico = Number(id);

  if (!Number.isInteger(idNumerico) || idNumerico <= 0) {
    throw new Error("El id del producto no es válido.");
  }

  return idNumerico;
};

const validarMovimiento = (movimiento) => {
  if (!movimiento) {
    throw new Error("Los datos del movimiento son obligatorios.");
  }

  const productoId = Number(movimiento.productoId);
  const cantidad = Number(movimiento.cantidad);

  if (!Number.isInteger(productoId) || productoId <= 0) {
    throw new Error("El producto indicado no es válido.");
  }

  if (!Number.isFinite(cantidad) || cantidad <= 0) {
    throw new Error("La cantidad debe ser mayor que cero.");
  }

  return {
    productoId,
    cantidad,
  };
};

const validarProducto = (producto, esEdicion = false) => {
  if (!producto) {
    throw new Error("Los datos del producto son obligatorios.");
  }

  if (!producto.nombre?.trim()) {
    throw new Error("El nombre del producto es obligatorio.");
  }

  if (!producto.descripcion?.trim()) {
    throw new Error("La descripción del producto es obligatoria.");
  }

  if (!producto.unidadMedida?.trim()) {
    throw new Error("La unidad de medida es obligatoria.");
  }

  const stockActual = Number(producto.stockActual);
  const stockMinimo = Number(producto.stockMinimo);

  if (!Number.isFinite(stockActual) || stockActual <= 0) {
    throw new Error("El stock actual debe ser mayor que cero.");
  }

  if (!Number.isFinite(stockMinimo) || stockMinimo <= 0) {
    throw new Error("El stock mínimo debe ser mayor que cero.");
  }

  if (!esEdicion && !(producto.imagen instanceof File)) {
    throw new Error("La imagen del producto es obligatoria.");
  }

  if (esEdicion && typeof producto.activo !== "boolean") {
    throw new Error("Debes indicar si el producto está activo.");
  }
};

const construirProductoFormData = (producto, esEdicion = false) => {
  validarProducto(producto, esEdicion);

  const formData = new FormData();

  formData.append("Nombre", producto.nombre.trim());
  formData.append("Descripcion", producto.descripcion.trim());
  formData.append("UnidadMedida", producto.unidadMedida.trim());
  formData.append("StockActual", String(producto.stockActual));
  formData.append("StockMinimo", String(producto.stockMinimo));

  if (esEdicion) {
    formData.append("Activo", String(producto.activo));
  }

  if (producto.imagen instanceof File) {
    formData.append("Imagen", producto.imagen);
  }

  return formData;
};

const obtenerMensajeError = (error) => {
  const data = error.response?.data;

  console.error("Error de API:", {
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
        const lista = Array.isArray(mensajes)
          ? mensajes
          : [mensajes];

        return lista.map(
          (mensaje) => `${campo}: ${mensaje}`
        );
      })
      .join(" ");
  }

  if (data?.title) {
    return data.title;
  }

  return (
    error.message ||
    "Ocurrió un error al procesar la solicitud."
  );
};

export const obtenerProductos = async () => {
  try {
    const response = await axiosClient.get(
      ENDPOINTS.PRODUCTO.GET_ALL
    );

    return response.data;
  } catch (error) {
    throw new Error(obtenerMensajeError(error));
  }
};

export const obtenerProductoPorId = async (id) => {
  const productoId = validarId(id);

  try {
    const response = await axiosClient.get(
      `${ENDPOINTS.PRODUCTO.GET_BY_ID}/${productoId}`
    );

    return response.data;
  } catch (error) {
    throw new Error(obtenerMensajeError(error));
  }
};

export const crearProducto = async (producto) => {
  const formData = construirProductoFormData(producto);

  try {
    const response = await axiosClient.post(
      ENDPOINTS.PRODUCTO.CREATE,
      formData
    );

    return response.data;
  } catch (error) {
    throw new Error(obtenerMensajeError(error));
  }
};

export const editarProducto = async (id, producto) => {
  const productoId = validarId(id);
  const formData = construirProductoFormData(
    producto,
    true
  );

  try {
    const response = await axiosClient.put(
      `${ENDPOINTS.PRODUCTO.UPDATE}/${productoId}`,
      formData
    );

    return response.data;
  } catch (error) {
    throw new Error(obtenerMensajeError(error));
  }
};

export const eliminarProducto = async (id) => {
  const productoId = validarId(id);

  try {
    await axiosClient.delete(
      `${ENDPOINTS.PRODUCTO.DELETE}/${productoId}`
    );

    return true;
  } catch (error) {
    throw new Error(obtenerMensajeError(error));
  }
};

export const registrarEntrada = async (movimiento) => {
  const datos = validarMovimiento(movimiento);

  try {
    const response = await axiosClient.post(
      ENDPOINTS.PRODUCTO.ADD_ENTRADA,
      datos
    );

    return response.data;
  } catch (error) {
    throw new Error(obtenerMensajeError(error));
  }
};

export const registrarSalida = async (movimiento) => {
  const datos = validarMovimiento(movimiento);

  try {
    const response = await axiosClient.post(
      ENDPOINTS.PRODUCTO.ADD_SALIDA,
      datos
    );

    return response.data;
  } catch (error) {
    throw new Error(obtenerMensajeError(error));
  }
};
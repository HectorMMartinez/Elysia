import axiosClient from "../api/axiosClient";
import ENDPOINTS from "../api/endpoints";

const ESTADOS_PLATO = [1, 2];

const validarId = (id, entidad = "plato") => {
  const idNumerico = Number(id);

  if (!Number.isInteger(idNumerico) || idNumerico <= 0) {
    throw new Error(`El id del ${entidad} no es valido.`);
  }

  return idNumerico;
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
    "Ocurrio un error al procesar la solicitud."
  );
};

const validarIngrediente = (ingrediente) => {
  const id = Number(ingrediente?.id ?? ingrediente?.productoId);
  const cantidad = Number(ingrediente?.cantidad);

  if (!Number.isInteger(id) || id <= 0) {
    throw new Error("Debes seleccionar ingredientes existentes.");
  }

  if (!Number.isFinite(cantidad) || cantidad <= 0) {
    throw new Error(
      "La cantidad de cada ingrediente debe ser mayor que cero."
    );
  }

  return { id, cantidad };
};

const validarPlato = (plato, esEdicion = false) => {
  if (!plato) {
    throw new Error("Los datos del plato son obligatorios.");
  }

  if (!plato.nombre?.trim()) {
    throw new Error("El nombre del plato es obligatorio.");
  }

  if (!plato.descripcion?.trim()) {
    throw new Error("La descripcion del plato es obligatoria.");
  }

  const precio = Number(plato.precio);

  if (!Number.isFinite(precio) || precio <= 0) {
    throw new Error("El precio del plato debe ser mayor que cero.");
  }

  const categoriaId = Number(plato.categoriaId);

  if (!Number.isInteger(categoriaId) || categoriaId <= 0) {
    throw new Error("Debes seleccionar una categoria valida.");
  }

  const estado = Number(plato.estado);

  if (!ESTADOS_PLATO.includes(estado)) {
    throw new Error("Debes seleccionar un estado valido para el plato.");
  }

  if (!Array.isArray(plato.ingredientes) || plato.ingredientes.length === 0) {
    throw new Error("Debes seleccionar al menos un ingrediente.");
  }

  const ingredientesNormalizados = plato.ingredientes.map(validarIngrediente);
  const ids = ingredientesNormalizados.map((ingrediente) => ingrediente.id);
  const idsUnicos = new Set(ids);

  if (idsUnicos.size !== ids.length) {
    throw new Error("No puedes repetir ingredientes en el mismo plato.");
  }

  if (!esEdicion && !(plato.imagen instanceof File)) {
    throw new Error("La imagen del plato es obligatoria.");
  }

  return {
    nombre: plato.nombre.trim(),
    descripcion: plato.descripcion.trim(),
    precio,
    categoriaId,
    estado,
    imagen: plato.imagen,
    ingredientes: ingredientesNormalizados,
  };
};

const construirPlatoFormData = (plato, esEdicion = false) => {
  const datos = validarPlato(plato, esEdicion);
  const formData = new FormData();

  formData.append("Nombre", datos.nombre);
  formData.append("Descripcion", datos.descripcion);
  formData.append("Precio", String(datos.precio));
  formData.append("CategoriaId", String(datos.categoriaId));
  formData.append("Estado", String(datos.estado));

  if (datos.imagen instanceof File) {
    formData.append("Imagen", datos.imagen);
  }

  datos.ingredientes.forEach((ingrediente, index) => {
    formData.append(
      `ProductoQuantityDtos[${index}].Id`,
      String(ingrediente.id)
    );
    formData.append(
      `ProductoQuantityDtos[${index}].Cantidad`,
      String(ingrediente.cantidad)
    );
  });

  return formData;
};

export const obtenerPlatos = async () => {
  try {
    const response = await axiosClient.get(
      ENDPOINTS.PLATO.GET_ALL_WITH_INGREDIENTS
    );

    return response.data;
  } catch (error) {
    throw new Error(obtenerMensajeError(error), {
      cause: error,
    });
  }
};

export const obtenerPlatoPorId = async (id) => {
  const platoId = validarId(id);

  try {
    const response = await axiosClient.get(
      `${ENDPOINTS.PLATO.GET_BY_ID_WITH_INGREDIENTS}/${platoId}`
    );

    return response.data;
  } catch (error) {
    throw new Error(obtenerMensajeError(error), {
      cause: error,
    });
  }
};

export const crearPlato = async (plato) => {
  const formData = construirPlatoFormData(plato);

  try {
    const response = await axiosClient.post(
      ENDPOINTS.PLATO.CREATE,
      formData
    );

    return response.data;
  } catch (error) {
    throw new Error(obtenerMensajeError(error), {
      cause: error,
    });
  }
};

export const editarPlato = async (id, plato) => {
  const platoId = validarId(id);
  const formData = construirPlatoFormData(plato, true);

  try {
    const response = await axiosClient.put(
      `${ENDPOINTS.PLATO.UPDATE}/${platoId}`,
      formData
    );

    return response.data;
  } catch (error) {
    throw new Error(obtenerMensajeError(error), {
      cause: error,
    });
  }
};

export const eliminarPlato = async (id) => {
  const platoId = validarId(id);

  try {
    await axiosClient.delete(
      `${ENDPOINTS.PLATO.DELETE}/${platoId}`
    );

    return true;
  } catch (error) {
    throw new Error(obtenerMensajeError(error), {
      cause: error,
    });
  }
};

export const obtenerCategoriasPlato = async () => {
  try {
    const response = await axiosClient.get(
      ENDPOINTS.CATEGORIA_PLATO.GET_ALL
    );

    return response.data;
  } catch (error) {
    throw new Error(obtenerMensajeError(error), {
      cause: error,
    });
  }
};

export const obtenerProductosParaIngredientes = async () => {
  try {
    const response = await axiosClient.get(
      ENDPOINTS.PRODUCTO.GET_ALL
    );

    return response.data;
  } catch (error) {
    throw new Error(obtenerMensajeError(error), {
      cause: error,
    });
  }
};

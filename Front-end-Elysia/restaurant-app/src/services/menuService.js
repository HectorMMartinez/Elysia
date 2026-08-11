import axiosClient from "../api/axiosClient";
import ENDPOINTS from "../api/endpoints";

const validarId = (id, entidad = "menú") => {
  const idNumerico = Number(id);
  if (!Number.isInteger(idNumerico) || idNumerico <= 0) {
    throw new Error(`El ID del ${entidad} no es válido.`);
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
        const lista = Array.isArray(mensajes) ? mensajes : [mensajes];
        return lista.map((mensaje) => `${campo}: ${mensaje}`);
      })
      .join(" ");
  }

  if (data?.title) {
    return data.title;
  }

  return error.message || "Ocurrió un error al procesar la solicitud.";
};

export const obtenerMenusConPlatos = async () => {
  try {
    const response = await axiosClient.get(
      ENDPOINTS.MENU.GET_ALL_WITH_PLATOS
    );
    return (
      response.data?.mostrarMenuConPlatosDtos ||
      response.data?.data ||
      (Array.isArray(response.data) ? response.data : [])
    );
  } catch (error) {
    throw new Error(obtenerMensajeError(error), { cause: error });
  }
};

export const obtenerMenuPorId = async (id) => {
  const menuId = validarId(id);

  try {
    const response = await axiosClient.get(ENDPOINTS.MENU.GET_BY_ID(menuId));
    return response.data;
  } catch (error) {
    throw new Error(obtenerMensajeError(error), { cause: error });
  }
};

export const crearMenu = async (menuData) => {
  const nombre = menuData?.nombre || menuData?.nombreMenu || "";
  const descripcion = menuData?.descripcion || menuData?.descripcionMenu || "";

  if (!nombre.trim()) {
    throw new Error("El nombre del menú es obligatorio.");
  }

  if (!descripcion.trim()) {
    throw new Error("La descripción del menú es obligatoria.");
  }

  try {
    const esPrincipalBool = Boolean(menuData.isPrincipal ?? menuData.esPrincipal);

    // Formatear arreglos de IDs de platos de manera limpia
    const idsBrutos = menuData.platoIds || menuData.platosIds || menuData.platos || [];
    const listaIds = Array.isArray(idsBrutos)
      ? idsBrutos.map((id) => Number(id)).filter((id) => !isNaN(id) && id > 0)
      : [];

    const payload = {
      nombre: nombre.trim(),
      nombreMenu: nombre.trim(),
      descripcion: descripcion.trim(),
      descripcionMenu: descripcion.trim(),
      estado: menuData.estado || menuData.menuEstado || "Disponible",
      menuEstado: menuData.estado || menuData.menuEstado || "Disponible",
      isPrincipal: esPrincipalBool,
      esPrincipal: esPrincipalBool,
      platoIds: listaIds,
      platosIds: listaIds,
      platosId: listaIds,
      platoId: listaIds,
    };

    const response = await axiosClient.post(ENDPOINTS.MENU.CREATE, payload);

    // Si el endpoint de creación no vincula relacionalmente en C#, forzamos la llamada secundaria
    if (listaIds.length > 0) {
      const nuevoId = response.data?.idMenu || response.data?.id || response.data?.menuId;
      if (nuevoId) {
        try {
          await asignarPlatosAMenu(nuevoId, listaIds);
        } catch (e) {
          console.warn("No se pudo realizar la asignación de platos secundaria:", e);
        }
      }
    }

    return response.data;
  } catch (error) {
    throw new Error(obtenerMensajeError(error), { cause: error });
  }
};

export const editarMenu = async (id, menuData) => {
  const menuId = validarId(id);

  const nombre = menuData?.nombre || menuData?.nombreMenu || "";
  const descripcion = menuData?.descripcion || menuData?.descripcionMenu || "";

  if (!nombre.trim()) {
    throw new Error("El nombre del menú es obligatorio.");
  }

  if (!descripcion.trim()) {
    throw new Error("La descripción del menú es obligatoria.");
  }

  try {
    const esPrincipalBool = Boolean(menuData.isPrincipal ?? menuData.esPrincipal);

    const idsBrutos = menuData.platoIds || menuData.platosIds || menuData.platos || [];
    const listaIds = Array.isArray(idsBrutos)
      ? idsBrutos.map((id) => Number(id)).filter((id) => !isNaN(id) && id > 0)
      : [];

    const payload = {
      id: menuId,
      menuId: menuId,
      nombre: nombre.trim(),
      nombreMenu: nombre.trim(),
      descripcion: descripcion.trim(),
      descripcionMenu: descripcion.trim(),
      estado: menuData.estado || menuData.menuEstado || "Disponible",
      menuEstado: menuData.estado || menuData.menuEstado || "Disponible",
      isPrincipal: esPrincipalBool,
      esPrincipal: esPrincipalBool,
      platoIds: listaIds,
      platosIds: listaIds,
      platosId: listaIds,
      platoId: listaIds,
    };

    // 1. Actualización de cabecera del menú en la API
    const response = await axiosClient.put(
      ENDPOINTS.MENU.UPDATE(menuId),
      payload
    );

    // 2. Sincronización explícita de platos para asegurar que la relación se persista en DB
    try {
      await asignarPlatosAMenu(menuId, listaIds);
    } catch (e) {
      console.warn("Sincronización de platos durante edición no completada en endpoint secundario:", e);
    }

    return response.data;
  } catch (error) {
    throw new Error(obtenerMensajeError(error), { cause: error });
  }
};

export const eliminarMenu = async (id) => {
  const menuId = validarId(id);

  try {
    await axiosClient.delete(ENDPOINTS.MENU.DELETE(menuId));
    return true;
  } catch (error) {
    throw new Error(obtenerMensajeError(error), { cause: error });
  }
};

export const asignarPlatosAMenu = async (idMenu, platoIds = []) => {
  const menuId = validarId(idMenu, "menú");

  if (!Array.isArray(platoIds)) {
    throw new Error("Los platos seleccionados deben ser una lista.");
  }

  try {
    const payload = {
      idMenu: menuId,
      menuId: menuId,
      platoIds: platoIds.map((pId) => Number(pId)),
      platosIds: platoIds.map((pId) => Number(pId)),
    };

    const response = await axiosClient.post(
      ENDPOINTS.MENU.ADD_PLATOS,
      payload
    );
    return response.data;
  } catch (error) {
    throw new Error(obtenerMensajeError(error), { cause: error });
  }
};

export const eliminarPlatoDeMenu = async (idRelacion) => {
  const relacionId = validarId(idRelacion, "plato del menú");

  try {
    await axiosClient.delete(ENDPOINTS.MENU.DELETE_PLATO(relacionId));
    return true;
  } catch (error) {
    throw new Error(obtenerMensajeError(error), { cause: error });
  }
};
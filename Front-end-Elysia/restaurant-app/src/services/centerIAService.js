import axiosClient from "../api/axiosClient";
import ENDPOINTS from "../api/endpoints";

const obtenerMensajeError = (error) => {
  const data = error.response?.data;
  console.error("Error de API CenterIA:", {
    status: error.response?.status,
    data,
  });

  if (typeof data === "string") return data;
  if (data?.title) return data.title;
  if (data?.message) return data.message;
  if (data?.errors) {
    if (Array.isArray(data.errors)) return data.errors.join(" ");
    return Object.entries(data.errors)
      .flatMap(([campo, mensajes]) => {
        const lista = Array.isArray(mensajes) ? mensajes : [mensajes];
        return lista.map((m) => `${campo}: ${m}`);
      })
      .join(" ");
  }
  return error.message || "Ocurrió un error al procesar la solicitud.";
};

export const obtenerAnalisisRestaurante = async () => {
  try {
    const response = await axiosClient.get(
      ENDPOINTS.CENTER_IA.GET_ANALISIS_RESTAURANTE
    );
    return response.data;
  } catch (error) {
    throw new Error(obtenerMensajeError(error), { cause: error });
  }
};

export const obtenerOptimizacionMenu = async () => {
  try {
    const response = await axiosClient.get(
      ENDPOINTS.CENTER_IA.GET_OPTIMIZACION_MENU
    );
    return response.data;
  } catch (error) {
    throw new Error(obtenerMensajeError(error), { cause: error });
  }
};

export const consultarAsistente = async (pregunta, historial = []) => {
  try {
    const payload = {
      pregunta,
      historial: historial.map((m) => ({
        rol: m.rol,
        contenido: m.contenido,
      })),
    };

    const response = await axiosClient.post(
      ENDPOINTS.CENTER_IA.CONSULTAR_ASISTENTE,
      payload
    );
    return response.data; // { pregunta, respuesta }
  } catch (error) {
    throw new Error(obtenerMensajeError(error), { cause: error });
  }
};

export default {
  obtenerAnalisisRestaurante,
  obtenerOptimizacionMenu,
  consultarAsistente,
};
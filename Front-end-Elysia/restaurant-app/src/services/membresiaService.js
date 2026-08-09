import axiosClient from "../api/axiosClient";
import ENDPOINTS from "../api/endpoints";

const membresiaService = {

  // Obtener todas las membresías
  getAll: async () => {

    const response = await axiosClient.get(
      ENDPOINTS.MEMBRESIA.GET_ALL
    );

    return response.data;
  },


  // Cancelar membresía
  cancelar: async (id) => {

    const response = await axiosClient.put(
      ENDPOINTS.MEMBRESIA.CANCELAR(id)
    );

    return response.data;
  },


  // Suspender membresía
  suspender: async (id) => {

    const response = await axiosClient.put(
      ENDPOINTS.MEMBRESIA.SUSPENDER(id)
    );

    return response.data;
  },


  // Activar membresía por un mes
  activarPorUnMes: async (id) => {

    const response = await axiosClient.put(
      ENDPOINTS.MEMBRESIA.ACTIVAR_POR_UN_MES(id)
    );

    return response.data;
  },

};

export default membresiaService;
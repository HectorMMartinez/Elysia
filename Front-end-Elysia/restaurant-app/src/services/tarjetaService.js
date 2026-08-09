import axiosClient from "../api/axiosClient";
import ENDPOINTS from "../api/endpoints";

const tarjetaService = {
  // Obtener todas las tarjetas
  getAll: async () => {
    const response = await axiosClient.get(
      ENDPOINTS.MANAGER_TARJETA.GET_ALL
    );
    return response.data;
  },

  // Obtener tarjeta por ID
  getById: async (id) => {
    const response = await axiosClient.get(
      ENDPOINTS.MANAGER_TARJETA.GET_BY_ID(id)
    );
    return response.data;
  },

  // Editar tarjeta
  editar: async (id, data) => {
    const response = await axiosClient.put(
      ENDPOINTS.MANAGER_TARJETA.EDITAR(id),
      data
    );
    return response.data;
  },
};

export default tarjetaService;
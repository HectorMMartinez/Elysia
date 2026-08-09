// src/services/mesaService.js

import axiosClient from "../api/axiosClient";
import ENDPOINTS from "../api/endpoints";

export const mesaService = {
  // 1. Obtener todas las mesas
  getAll: async () => {
    const response = await axiosClient.get(ENDPOINTS.MESA.GET_ALL);
    return response.data;
  },

  // 2. Obtener solo disponibles
  getAllDisponibles: async () => {
    const response = await axiosClient.get(ENDPOINTS.MESA.GET_ALL_DISPONIBLES);
    return response.data;
  },

  // 3. Obtener mesa por ID
  getById: async (id) => {
    const response = await axiosClient.get(ENDPOINTS.MESA.GET_BY_ID(id));
    return response.data;
  },

  // 4. Crear una nueva mesa
  create: async (mesaData) => {
    const formData = new FormData();
    formData.append("nombre", mesaData.nombre);
    formData.append("descripcion", mesaData.descripcion);
    formData.append("estado", mesaData.estado || "Disponible");
    formData.append("capacidad", mesaData.capacidad || 0);

    // Si seleccionaron archivo de imagen, lo adjuntamos
    if (mesaData.imagen && mesaData.imagen[0]) {
      formData.append("imagen", mesaData.imagen[0]);
    }

    const response = await axiosClient.post(ENDPOINTS.MESA.CREATE, formData);
    return response.data;
  },

  // 5. Actualizar una mesa existente
  update: async (id, mesaData) => {
    const formData = new FormData();
    formData.append("nombre", mesaData.nombre);
    formData.append("descripcion", mesaData.descripcion);
    formData.append("estado", mesaData.estado);
    formData.append("capacidad", mesaData.capacidad);

    if (mesaData.imagen && mesaData.imagen[0] instanceof File) {
      formData.append("imagen", mesaData.imagen[0]);
    }

    const response = await axiosClient.put(ENDPOINTS.MESA.UPDATE(id), formData);
    return response.data;
  },

  // 6. Eliminar mesa
  delete: async (id) => {
    const response = await axiosClient.delete(ENDPOINTS.MESA.DELETE(id));
    return response.data;
  }
};
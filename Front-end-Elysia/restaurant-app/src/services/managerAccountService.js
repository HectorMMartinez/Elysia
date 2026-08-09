import axiosClient from "../api/axiosClient";
import ENDPOINTS from "../api/endpoints";

const managerAccountService = {

  // Obtener todos los propietarios
  getAllPropietarios: async () => {
    const response = await axiosClient.get(
      ENDPOINTS.MANAGER_ACCOUNT.GET_ALL_PROPIETARIOS
    );

    return response.data;
  },

  // Obtener todos los administradores
  getAllAdmins: async () => {
    const response = await axiosClient.get(
      ENDPOINTS.MANAGER_ACCOUNT.GET_ALL_ADMINS
    );

    return response.data;
  },

  // Obtener usuario por ID
  getById: async (id) => {
    const response = await axiosClient.get(
      ENDPOINTS.MANAGER_ACCOUNT.GET_BY_ID(id)
    );

    return response.data;
  },

  // Activar usuario
  activar: async (id) => {
    const response = await axiosClient.post(
      ENDPOINTS.MANAGER_ACCOUNT.ACTIVAR(id)
    );

    return response.data;
  },

  // Inactivar usuario
  inactivar: async (id) => {
    const response = await axiosClient.post(
      ENDPOINTS.MANAGER_ACCOUNT.INACTIVAR(id)
    );

    return response.data;
  },



   // Obtener perfil del usuario logueado
     getPerfil: async () => {
    const response = await axiosClient.get(
      ENDPOINTS.MANAGER_ACCOUNT.GET_PERFIL
    );

    return response.data;
  },


  // Editar perfil del usuario logueado
  editarPerfil: async (formData) => {
    const response = await axiosClient.put(
      ENDPOINTS.MANAGER_ACCOUNT.EDITAR_PERFIL,
      formData
    );

    return response.data;
  },

};

export default managerAccountService;
const ENDPOINTS = {
  AUTH: {
    LOGIN: "/Account/login-user",
    REGISTER: "/Account/register-user",
    CONFIRM_ACCOUNT: "/Account/confirm-account",
    FORGOT_PASSWORD: "/Account/get-resset-token",
    RESET_PASSWORD: "/Account/change-password",
  },

  PLAN: {
    GET_ALL: "/Plans/Get-All-Planes",
  },
  
  PRODUCTO: {
    GET_ALL: "/Producto/get-all-product",
    GET_BY_ID: "/Producto/get-product-by-id",
    CREATE: "/Producto/add-product",
    UPDATE: "/Producto/edit-product",
    DELETE: "/Producto/delete-product",
    ADD_ENTRADA: "/Producto/add-entrada",
    ADD_SALIDA: "/Producto/add-salida",
  },

  PLATO: {
    GET_ALL_WITH_INGREDIENTS: "/Plato/get-all-con-ingredientes",
    GET_BY_ID_WITH_INGREDIENTS: "/Plato/get-by-id-con-ingrediente",
    CREATE: "/Plato/add-plato",
    UPDATE: "/Plato/update-Plato",
    DELETE: "/Plato/delete-by-id",
  },

  CATEGORIA_PLATO: {
    GET_ALL: "/CategoriaPlato/get-all-categoriaPlatos",
  },


  MESA: {
    GET_ALL: "/Mesa/get-all",
    GET_ALL_DISPONIBLES: "/Mesa/get-all-disponibles",
    GET_BY_ID: (id) => `/Mesa/get-by-id/${id}`,
    CREATE: "/Mesa/add-mesa",
    UPDATE: (id) => `/Mesa/update-mesa/${id}`,
    DELETE: (id) => `/Mesa/delete-by-id/${id}`,
  },

  DASHBOARD: {
    GET_PANEL: "/DashboardPropietario/get-panel-propietario",
  },

  PLAN: {
    // si ya tienes GET_ALL, solo agrega esta línea
    CAMBIAR_PLAN: "/Plans/cambiar-plan-usuario",
  },

  DASHBOARD_ADMIN: {
    GET_PANEL: "/DashboardAdmin/get-panel-admin",
  },

  MANAGER_ACCOUNT: {
  GET_ALL_PROPIETARIOS: "/ManagerAccount/get-all-user-propietario",
  GET_ALL_ADMINS: "/ManagerAccount/get-all-user-admin",
  GET_BY_ID: (id) => `/ManagerAccount/get-user-by-id/${id}`,
  ACTIVAR: (id) => `/ManagerAccount/activar-user-by-id/${id}`,
  INACTIVAR: (id) => `/ManagerAccount/inactivar-user-by-id/${id}`,
  GET_PERFIL: "/ManagerAccount/get-perfil-usuario",
  EDITAR_PERFIL: "/ManagerAccount/edit-perfil",
  },


  MEMBRESIA: {
  GET_ALL: "/Membresia/get-all-membresia",
  CANCELAR: (id) =>
    `/Membresia/cancelar-membresia/${id}`,
  SUSPENDER: (id) =>
    `/Membresia/suspender-membresia/${id}`,
  ACTIVAR_POR_UN_MES: (id) =>
    `/Membresia/activar-membresia-por-un-mes/${id}`,
   },

   MANAGER_TARJETA: {
    GET_ALL: "/ManagerTarjeta/get-all-tarjeta",
    GET_BY_ID: (id) => `/ManagerTarjeta/get-tarjeta-by-id/${id}`,
    EDITAR: (id) => `/ManagerTarjeta/editar-tarjeta/${id}`,
   },

  RESERVA: {
  GET_ALL: "/Reserva/get-all-reservas",
  GET_BY_ID: (id) => `/Reserva/get-by-id/${id}`,
  CREATE: "/Reserva/add-reserva",
  UPDATE: (id) => `/Reserva/update-reserva/${id}`,
  DELETE: (id) => `/Reserva/delete-by-id/${id}`,
  CAMBIAR_EN_PROCESO: (id) => `/Reserva/cambiar-reserva-en-Proceso/${id}`,
  CAMBIAR_NO_ASISTIO: (id) => `/Reserva/cambiar-reserva-no-asistio/${id}`,
  FINALIZAR: (id) => `/Reserva/finalizar-reserva/${id}`,
  CANCELAR: (id) => `/Reserva/cancelar-reserva/${id}`,
},

};

export default ENDPOINTS;
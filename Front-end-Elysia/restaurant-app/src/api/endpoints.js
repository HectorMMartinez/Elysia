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
};

export default ENDPOINTS;

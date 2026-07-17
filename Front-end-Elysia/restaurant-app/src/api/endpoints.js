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
};

export default ENDPOINTS;

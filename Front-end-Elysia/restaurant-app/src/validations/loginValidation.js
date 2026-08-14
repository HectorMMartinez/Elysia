export const loginValidation = {
  email: {
    required: "El correo electrónico es requerido",
    pattern: {
      value: /^[^\s@]+@[^\s@]+\.[^\s@]+$/,
      message: "Por favor ingresa un correo electrónico válido"
    }
  },
  password: {
    required: "La contraseña es requerida",
    minLength: {
      value: 6,
      message: "La contraseña debe tener al menos 6 caracteres"
    }
  }
};
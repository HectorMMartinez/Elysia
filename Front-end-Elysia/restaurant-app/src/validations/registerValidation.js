export const registerValidation = {
  // ===== PASO 1: INFORMACIÓN PERSONAL =====
  profileImage: {
  required: "La foto es obligatoria"
 },
  name: {
    required: "El nombre es requerido",
    minLength: {
      value: 2,
      message: "El nombre debe tener al menos 2 caracteres"
    },
    pattern: {
      value: /^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]*$/,
      message: "El nombre solo puede contener letras"
    }
  },

  lastName: {
    required: "El apellido es requerido",
    minLength: {
      value: 2,
      message: "El apellido debe tener al menos 2 caracteres"
    },
    pattern: {
      value: /^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]*$/,
      message: "El apellido solo puede contener letras"
    }
  },

  username: {
    required: "El usuario es requerido",
    minLength: {
      value: 4,
      message: "El usuario debe tener al menos 4 caracteres"
    },
    maxLength: {
      value: 20,
      message: "El usuario no puede exceder 20 caracteres"
    },
    pattern: {
      value: /^[a-zA-Z0-9_-]*$/,
      message: "El usuario solo puede contener letras, números, guiones y guiones bajos"
    }
  },

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
      value: 8,
      message: "La contraseña debe tener exactamente 8 caracteres"
    },
    maxLength: {
      value: 8,
      message: "La contraseña debe tener exactamente 8 caracteres"
    },
    validate: {
      hasUpperCase: (value) =>
        /[A-Z]/.test(value) || "Debe contener al menos una letra mayúscula",
      hasLowerCase: (value) =>
        /[a-z]/.test(value) || "Debe contener al menos una letra minúscula",
      hasNumber: (value) =>
        /[0-9]/.test(value) || "Debe contener al menos un número",
      hasSpecialChar: (value) =>
        /[!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?]/.test(value) ||
        "Debe contener al menos un carácter especial (!@#$%^&*...)"
    }
  },

  confirmPassword: {
    required: "Debe confirmar la contraseña",
    validate: {
      matchPassword: (value, formValues) =>
        value === formValues.password || "Las contraseñas no coinciden"
    }
  },

  phone: {
    required: "El teléfono es requerido",
    pattern: {
      value: /^[0-9\-\+\(\)\s]*$/,
      message: "El teléfono solo puede contener números"
    },
    minLength: {
      value: 10,
      message: "El teléfono debe tener al menos 10 dígitos"
    }
  },

  cardId: {
    required: "La cédula es requerida",
    pattern: {
      value: /^[0-9]{11}$/,
      message: "La cédula debe contener exactamente 11 números sin guiones"
    }
  },

  // ===== PASO 2: INFORMACIÓN DEL RESTAURANTE =====

   restaurantLogo: {
      required: "El logo del restaurante es obligatorio"
 },
  restaurantName: {
    required: "El nombre del restaurante es requerido",
    minLength: {
      value: 2,
      message: "El nombre debe tener al menos 2 caracteres"
    },
    maxLength: {
      value: 100,
      message: "El nombre no puede exceder 100 caracteres"
    }
  },

  specialty: {
    required: "La especialidad es requerida",
    minLength: {
      value: 2,
      message: "La especialidad debe tener al menos 2 caracteres"
    }
  },

  restaurantPhone: {
    required: "El teléfono del restaurante es requerido",
    pattern: {
      value: /^[0-9\-\+\(\)\s]*$/,
      message: "El teléfono solo puede contener números"
    },
    minLength: {
      value: 10,
      message: "El teléfono debe tener al menos 10 dígitos"
    }
  },

  rnc: {
    required: "El RNC es requerido",
    pattern: {
      value: /^[0-9]{9}$/,
      message: "El RNC debe contener exactamente 9 números"
    }
  },

  openingTime: {
    required: "La hora de apertura es requerida"
  },

  closingTime: {
    required: "La hora de cierre es requerida",
    validate: {
      isAfterOpening: (value, formValues) =>
        !formValues.openingTime ||
        value > formValues.openingTime ||
        "La hora de cierre debe ser posterior a la hora de apertura"
    }
  },

  address: {
    required: "La dirección es requerida",
    minLength: {
      value: 10,
      message: "La dirección debe tener al menos 10 caracteres"
    },
    maxLength: {
      value: 200,
      message: "La dirección no puede exceder 200 caracteres"
    }
  },

  // ===== PASO 3: PLAN Y MÉTODO DE PAGO =====
  termsAccepted: {
    required: "Debes aceptar los términos y condiciones"
  },

  cardholderName: {
    required: "El nombre del titular es requerido",
    minLength: {
      value: 2,
      message: "El nombre debe tener al menos 2 caracteres"
    },
    pattern: {
      value: /^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]*$/,
      message: "El nombre solo puede contener letras"
    }
  },

  cardNumber: {
    required: "El número de tarjeta es requerido",
    pattern: {
      value: /^[0-9]{16}$/,
      message: "El número de tarjeta debe contener exactamente 16 números sin guiones ni espacios"
    }
  },

  cardType: {
    required: "El tipo de tarjeta es requerido",
    validate: {
      isValid: (value) =>
        ["Visa", "MasterCard", "American Express"].includes(value) ||
        "Selecciona un tipo de tarjeta válido"
    }
  },


  cvv: {
    required: "El CVV es requerido",
    pattern: {
      value: /^[0-9]{3,4}$/,
      message: "El CVV debe contener 3 o 4 números"
    }
  },

  expiryMonth: {
    required: "El mes de vencimiento es requerido",
    pattern: {
      value: /^(0[1-9]|1[0-2])$/,
      message: "El mes debe estar entre 01 y 12"
    }
  },

  expiryYear: {
    required: "El año de vencimiento es requerido",
    pattern: {
      value: /^[0-9]{4}$/,
      message: "El año debe tener 4 dígitos"
    },
    validate: {
      isValid: (value) => {
        const currentYear = new Date().getFullYear();
        return parseInt(value) >= currentYear || "El año de vencimiento debe ser válido"
      }
    }
  }
};

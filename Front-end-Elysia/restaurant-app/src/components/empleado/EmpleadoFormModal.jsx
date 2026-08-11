import React, { useState, useEffect } from "react";
import { FiX, FiUser, FiMail, FiPhone, FiDollarSign, FiBriefcase } from "react-icons/fi";
import { obtenerPuestos } from "../../services/empleadoService";

export const EmpleadoFormModal = ({
  isOpen,
  onClose,
  onSubmit,
  initialData = null,
  isLoading = false,
}) => {
  const [formData, setFormData] = useState({
    firstName: "",
    lastName: "",
    email: "",
    phone: "",
    salary: "",
    puestoId: "",
  });

  const [puestos, setPuestos] = useState([]);
  const [loadingPuestos, setLoadingPuestos] = useState(false);
  const [error, setError] = useState(null);
  const [fieldErrors, setFieldErrors] = useState({});

  const isEditing = !!initialData;

  // Cargar puestos
  useEffect(() => {
    if (isOpen) {
      const cargarPuestos = async () => {
        try {
          setLoadingPuestos(true);
          const data = await obtenerPuestos();
          setPuestos(Array.isArray(data) ? data : []);
        } catch (err) {
          console.error("Error cargando puestos:", err);
        } finally {
          setLoadingPuestos(false);
        }
      };
      cargarPuestos();
    }
  }, [isOpen]);

  // Reset / precargar formulario
  useEffect(() => {
    if (isOpen) {
      if (initialData) {
        setFormData({
          firstName: initialData.firstName || "",
          lastName: initialData.lastName || "",
          email: initialData.email || "",
          phone: initialData.phone || "",
          salary: initialData.salary ?? "",
          puestoId: initialData.puestoId ?? "",
        });
      } else {
        setFormData({
          firstName: "",
          lastName: "",
          email: "",
          phone: "",
          salary: "",
          puestoId: "",
        });
      }
      setError(null);
      setFieldErrors({});
    }
  }, [isOpen, initialData]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
    // Limpiar error del campo
    if (fieldErrors[name]) {
      setFieldErrors((prev) => ({ ...prev, [name]: null }));
    }
  };

  const validate = () => {
    const errors = {};
    if (!formData.firstName.trim()) errors.firstName = "El nombre es obligatorio";
    if (!formData.lastName.trim()) errors.lastName = "El apellido es obligatorio";
    if (!formData.email.trim()) errors.email = "El correo es obligatorio";
    if (!formData.phone.trim()) errors.phone = "El teléfono es obligatorio";
    if (!formData.salary || Number(formData.salary) <= 0) {
      errors.salary = "Debes ingresar un salario válido";
    }
    if (!formData.puestoId) errors.puestoId = "Debes seleccionar un puesto";

    setFieldErrors(errors);
    return Object.keys(errors).length === 0;
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError(null);

    if (!validate()) return;

    try {
      await onSubmit({
        firstName: formData.firstName.trim(),
        lastName: formData.lastName.trim(),
        email: formData.email.trim(),
        phone: formData.phone.trim(),
        salary: Number(formData.salary),
        puestoId: Number(formData.puestoId),
      });
      // Si llega aquí sin throw → éxito (el padre cierra el modal)
    } catch (err) {
      // Importante: el error del backend se muestra aquí y el modal NO se cierra
      setError(err.message || "Ocurrió un error al guardar el empleado.");
    }
  };

  if (!isOpen) return null;

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center p-4 bg-black/50 backdrop-blur-sm">
      <div className="bg-white rounded-2xl shadow-2xl w-full max-w-lg max-h-[90vh] overflow-y-auto">
        {/* Header */}
        <div className="flex items-center justify-between p-6 border-b border-gray-100">
          <div>
            <h2 className="text-xl font-bold text-slate-800">
              {isEditing ? "Editar Empleado" : "Nuevo Empleado"}
            </h2>
            <p className="text-sm text-slate-500 mt-0.5">
              {isEditing
                ? "Modifica los datos del empleado"
                : "Completa los datos para registrar un nuevo empleado"}
            </p>
          </div>
          <button
            onClick={onClose}
            disabled={isLoading}
            className="p-2 text-gray-400 hover:text-gray-600 hover:bg-gray-100 rounded-lg transition"
          >
            <FiX size={20} />
          </button>
        </div>

        {/* Body */}
        <form onSubmit={handleSubmit} className="p-6 space-y-5">
          {/* Error general del backend */}
          {error && (
            <div className="p-4 bg-red-50 border border-red-100 text-red-700 text-sm rounded-xl">
              {error}
            </div>
          )}

          <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
            {/* Nombre */}
            <div>
              <label className="block text-sm font-medium text-slate-700 mb-1.5">
                Nombre <span className="text-red-500">*</span>
              </label>
              <div className="relative">
                <FiUser className="absolute left-3 top-3 text-gray-400" size={16} />
                <input
                  type="text"
                  name="firstName"
                  value={formData.firstName}
                  onChange={handleChange}
                  disabled={isLoading}
                  className={`w-full pl-10 pr-4 py-2.5 rounded-xl border text-sm focus:outline-none focus:ring-2 focus:ring-purple-500/20 ${
                    fieldErrors.firstName ? "border-red-300" : "border-gray-200"
                  }`}
                  placeholder="Juan"
                />
              </div>
              {fieldErrors.firstName && (
                <p className="text-xs text-red-500 mt-1">{fieldErrors.firstName}</p>
              )}
            </div>

            {/* Apellido */}
            <div>
              <label className="block text-sm font-medium text-slate-700 mb-1.5">
                Apellido <span className="text-red-500">*</span>
              </label>
              <div className="relative">
                <FiUser className="absolute left-3 top-3 text-gray-400" size={16} />
                <input
                  type="text"
                  name="lastName"
                  value={formData.lastName}
                  onChange={handleChange}
                  disabled={isLoading}
                  className={`w-full pl-10 pr-4 py-2.5 rounded-xl border text-sm focus:outline-none focus:ring-2 focus:ring-purple-500/20 ${
                    fieldErrors.lastName ? "border-red-300" : "border-gray-200"
                  }`}
                  placeholder="Pérez"
                />
              </div>
              {fieldErrors.lastName && (
                <p className="text-xs text-red-500 mt-1">{fieldErrors.lastName}</p>
              )}
            </div>
          </div>

          {/* Email */}
          <div>
            <label className="block text-sm font-medium text-slate-700 mb-1.5">
              Correo electrónico <span className="text-red-500">*</span>
            </label>
            <div className="relative">
              <FiMail className="absolute left-3 top-3 text-gray-400" size={16} />
              <input
                type="email"
                name="email"
                value={formData.email}
                onChange={handleChange}
                disabled={isLoading}
                className={`w-full pl-10 pr-4 py-2.5 rounded-xl border text-sm focus:outline-none focus:ring-2 focus:ring-purple-500/20 ${
                  fieldErrors.email ? "border-red-300" : "border-gray-200"
                }`}
                placeholder="juan.perez@email.com"
              />
            </div>
            {fieldErrors.email && (
              <p className="text-xs text-red-500 mt-1">{fieldErrors.email}</p>
            )}
          </div>

          {/* Teléfono */}
          <div>
            <label className="block text-sm font-medium text-slate-700 mb-1.5">
              Teléfono <span className="text-red-500">*</span>
            </label>
            <div className="relative">
              <FiPhone className="absolute left-3 top-3 text-gray-400" size={16} />
              <input
                type="text"
                name="phone"
                value={formData.phone}
                onChange={handleChange}
                disabled={isLoading}
                className={`w-full pl-10 pr-4 py-2.5 rounded-xl border text-sm focus:outline-none focus:ring-2 focus:ring-purple-500/20 ${
                  fieldErrors.phone ? "border-red-300" : "border-gray-200"
                }`}
                placeholder="809-123-4567"
              />
            </div>
            {fieldErrors.phone && (
              <p className="text-xs text-red-500 mt-1">{fieldErrors.phone}</p>
            )}
          </div>

          <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
            {/* Salario */}
            <div>
              <label className="block text-sm font-medium text-slate-700 mb-1.5">
                Salario <span className="text-red-500">*</span>
              </label>
              <div className="relative">
                <FiDollarSign className="absolute left-3 top-3 text-gray-400" size={16} />
                <input
                  type="number"
                  name="salary"
                  value={formData.salary}
                  onChange={handleChange}
                  disabled={isLoading}
                  min="1"
                  step="0.01"
                  className={`w-full pl-10 pr-4 py-2.5 rounded-xl border text-sm focus:outline-none focus:ring-2 focus:ring-purple-500/20 ${
                    fieldErrors.salary ? "border-red-300" : "border-gray-200"
                  }`}
                  placeholder="25000"
                />
              </div>
              {fieldErrors.salary && (
                <p className="text-xs text-red-500 mt-1">{fieldErrors.salary}</p>
              )}
            </div>

            {/* Puesto */}
            <div>
              <label className="block text-sm font-medium text-slate-700 mb-1.5">
                Puesto <span className="text-red-500">*</span>
              </label>
              <div className="relative">
                <FiBriefcase className="absolute left-3 top-3 text-gray-400" size={16} />
                <select
                  name="puestoId"
                  value={formData.puestoId}
                  onChange={handleChange}
                  disabled={isLoading || loadingPuestos}
                  className={`w-full pl-10 pr-4 py-2.5 rounded-xl border text-sm focus:outline-none focus:ring-2 focus:ring-purple-500/20 appearance-none bg-white ${
                    fieldErrors.puestoId ? "border-red-300" : "border-gray-200"
                  }`}
                >
                  <option value="">
                    {loadingPuestos ? "Cargando..." : "Seleccionar puesto"}
                  </option>
                  {puestos.map((p) => (
                    <option key={p.id} value={p.id}>
                      {p.nombre || p.name || `Puesto #${p.id}`}
                    </option>
                  ))}
                </select>
              </div>
              {fieldErrors.puestoId && (
                <p className="text-xs text-red-500 mt-1">{fieldErrors.puestoId}</p>
              )}
            </div>
          </div>

          {/* Footer */}
          <div className="flex gap-3 pt-2">
            <button
              type="button"
              onClick={onClose}
              disabled={isLoading}
              className="flex-1 px-4 py-2.5 rounded-xl border border-gray-200 text-slate-600 font-semibold text-sm hover:bg-gray-50 transition disabled:opacity-50"
            >
              Cancelar
            </button>
            <button
              type="submit"
              disabled={isLoading}
              className="flex-1 px-4 py-2.5 rounded-xl bg-purple-600 text-white font-semibold text-sm hover:bg-purple-700 transition disabled:opacity-50 flex items-center justify-center gap-2"
            >
              {isLoading ? (
                <>
                  <span className="w-4 h-4 border-2 border-white/30 border-t-white rounded-full animate-spin" />
                  Guardando...
                </>
              ) : isEditing ? (
                "Guardar cambios"
              ) : (
                "Crear empleado"
              )}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};
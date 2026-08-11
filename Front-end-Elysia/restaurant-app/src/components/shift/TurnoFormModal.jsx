import React, { useState, useEffect } from "react";
import { FiX, FiClock, FiType } from "react-icons/fi";

export const TurnoFormModal = ({
  isOpen,
  onClose,
  onSubmit,
  initialData = null,
  isLoading = false,
}) => {
  const [formData, setFormData] = useState({
    name: "",
    startTime: "",
    endTime: "",
  });
  const [error, setError] = useState(null);
  const [fieldErrors, setFieldErrors] = useState({});

  const isEditing = !!initialData;

  useEffect(() => {
    if (isOpen) {
      if (initialData) {
        setFormData({
          name: initialData.name || "",
          startTime: initialData.startTime
            ? String(initialData.startTime).slice(0, 5)
            : "",
          endTime: initialData.endTime
            ? String(initialData.endTime).slice(0, 5)
            : "",
        });
      } else {
        setFormData({ name: "", startTime: "", endTime: "" });
      }
      setError(null);
      setFieldErrors({});
    }
  }, [isOpen, initialData]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
    if (fieldErrors[name]) {
      setFieldErrors((prev) => ({ ...prev, [name]: null }));
    }
  };

  const validate = () => {
    const errors = {};
    if (!formData.name.trim()) errors.name = "El nombre es obligatorio";
    if (!formData.startTime) errors.startTime = "La hora de inicio es obligatoria";
    if (!formData.endTime) errors.endTime = "La hora de fin es obligatoria";
    if (formData.startTime && formData.endTime && formData.startTime >= formData.endTime) {
      errors.endTime = "La hora de fin debe ser posterior a la de inicio";
    }
    setFieldErrors(errors);
    return Object.keys(errors).length === 0;
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError(null);
    if (!validate()) return;

    try {
      await onSubmit({
        name: formData.name.trim(),
        startTime: formData.startTime,
        endTime: formData.endTime,
      });
    } catch (err) {
      setError(err.message || "Ocurrió un error al guardar el turno.");
    }
  };

  if (!isOpen) return null;

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center p-4 bg-black/50 backdrop-blur-sm">
      <div className="bg-white rounded-2xl shadow-2xl w-full max-w-md">
        <div className="flex items-center justify-between p-6 border-b border-gray-100">
          <div>
            <h2 className="text-xl font-bold text-slate-800">
              {isEditing ? "Editar Turno" : "Nuevo Turno"}
            </h2>
            <p className="text-sm text-slate-500 mt-0.5">
              Define el nombre y horario del turno
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

        <form onSubmit={handleSubmit} className="p-6 space-y-5">
          {error && (
            <div className="p-4 bg-red-50 border border-red-100 text-red-700 text-sm rounded-xl">
              {error}
            </div>
          )}

          {/* Nombre */}
          <div>
            <label className="block text-sm font-medium text-slate-700 mb-1.5">
              Nombre del turno <span className="text-red-500">*</span>
            </label>
            <div className="relative">
              <FiType className="absolute left-3 top-3 text-gray-400" size={16} />
              <input
                type="text"
                name="name"
                value={formData.name}
                onChange={handleChange}
                disabled={isLoading}
                className={`w-full pl-10 pr-4 py-2.5 rounded-xl border text-sm focus:outline-none focus:ring-2 focus:ring-purple-500/20 ${
                  fieldErrors.name ? "border-red-300" : "border-gray-200"
                }`}
                placeholder="Ej: Turno Mañana, Turno Noche..."
              />
            </div>
            {fieldErrors.name && (
              <p className="text-xs text-red-500 mt-1">{fieldErrors.name}</p>
            )}
          </div>

          <div className="grid grid-cols-2 gap-4">
            {/* Hora Inicio */}
            <div>
              <label className="block text-sm font-medium text-slate-700 mb-1.5">
                Hora inicio <span className="text-red-500">*</span>
              </label>
              <div className="relative">
                <FiClock className="absolute left-3 top-3 text-gray-400" size={16} />
                <input
                  type="time"
                  name="startTime"
                  value={formData.startTime}
                  onChange={handleChange}
                  disabled={isLoading}
                  className={`w-full pl-10 pr-4 py-2.5 rounded-xl border text-sm focus:outline-none focus:ring-2 focus:ring-purple-500/20 ${
                    fieldErrors.startTime ? "border-red-300" : "border-gray-200"
                  }`}
                />
              </div>
              {fieldErrors.startTime && (
                <p className="text-xs text-red-500 mt-1">{fieldErrors.startTime}</p>
              )}
            </div>

            {/* Hora Fin */}
            <div>
              <label className="block text-sm font-medium text-slate-700 mb-1.5">
                Hora fin <span className="text-red-500">*</span>
              </label>
              <div className="relative">
                <FiClock className="absolute left-3 top-3 text-gray-400" size={16} />
                <input
                  type="time"
                  name="endTime"
                  value={formData.endTime}
                  onChange={handleChange}
                  disabled={isLoading}
                  className={`w-full pl-10 pr-4 py-2.5 rounded-xl border text-sm focus:outline-none focus:ring-2 focus:ring-purple-500/20 ${
                    fieldErrors.endTime ? "border-red-300" : "border-gray-200"
                  }`}
                />
              </div>
              {fieldErrors.endTime && (
                <p className="text-xs text-red-500 mt-1">{fieldErrors.endTime}</p>
              )}
            </div>
          </div>

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
                "Crear turno"
              )}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};
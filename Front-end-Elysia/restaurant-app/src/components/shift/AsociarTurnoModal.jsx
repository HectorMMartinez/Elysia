import React, { useState, useEffect } from "react";
import { FiX, FiUser, FiClock, FiCalendar } from "react-icons/fi";
import { obtenerEmpleadosActivos } from "../../services/empleadoService"; // del módulo anterior
import { obtenerTurnos } from "../../services/shiftService";

export const AsociarTurnoModal = ({
  isOpen,
  onClose,
  onSubmit,
  isLoading = false,
}) => {
  const [formData, setFormData] = useState({
    empleadoId: "",
    shiftId: "",
    workDate: "",
  });
  const [empleados, setEmpleados] = useState([]);
  const [turnos, setTurnos] = useState([]);
  const [loadingData, setLoadingData] = useState(false);
  const [error, setError] = useState(null);
  const [fieldErrors, setFieldErrors] = useState({});

  useEffect(() => {
    if (isOpen) {
      const cargarDatos = async () => {
        try {
          setLoadingData(true);
          const [emps, turns] = await Promise.all([
            obtenerEmpleadosActivos(),
            obtenerTurnos(),
          ]);
          setEmpleados(Array.isArray(emps) ? emps : []);
          setTurnos(Array.isArray(turns) ? turns : []);
        } catch (err) {
          console.error(err);
        } finally {
          setLoadingData(false);
        }
      };
      cargarDatos();
      setFormData({ empleadoId: "", shiftId: "", workDate: "" });
      setError(null);
      setFieldErrors({});
    }
  }, [isOpen]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
    if (fieldErrors[name]) {
      setFieldErrors((prev) => ({ ...prev, [name]: null }));
    }
  };

  const validate = () => {
    const errors = {};
    if (!formData.empleadoId) errors.empleadoId = "Debes seleccionar un empleado";
    if (!formData.shiftId) errors.shiftId = "Debes seleccionar un turno";
    if (!formData.workDate) errors.workDate = "La fecha es obligatoria";
    setFieldErrors(errors);
    return Object.keys(errors).length === 0;
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError(null);
    if (!validate()) return;

    try {
      await onSubmit({
        empleadoId: Number(formData.empleadoId),
        shiftId: Number(formData.shiftId),
        workDate: formData.workDate,
      });
    } catch (err) {
      setError(err.message || "Ocurrió un error al asociar el empleado al turno.");
    }
  };

  if (!isOpen) return null;

  // Fecha mínima = hoy
  const hoy = new Date().toISOString().split("T")[0];

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center p-4 bg-black/50 backdrop-blur-sm">
      <div className="bg-white rounded-2xl shadow-2xl w-full max-w-md">
        <div className="flex items-center justify-between p-6 border-b border-gray-100">
          <div>
            <h2 className="text-xl font-bold text-slate-800">Asignar Turno</h2>
            <p className="text-sm text-slate-500 mt-0.5">
              Asocia un empleado activo a un turno
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

          {/* Empleado */}
          <div>
            <label className="block text-sm font-medium text-slate-700 mb-1.5">
              Empleado <span className="text-red-500">*</span>
            </label>
            <div className="relative">
              <FiUser className="absolute left-3 top-3 text-gray-400" size={16} />
              <select
                name="empleadoId"
                value={formData.empleadoId}
                onChange={handleChange}
                disabled={isLoading || loadingData}
                className={`w-full pl-10 pr-4 py-2.5 rounded-xl border text-sm focus:outline-none focus:ring-2 focus:ring-purple-500/20 appearance-none bg-white ${
                  fieldErrors.empleadoId ? "border-red-300" : "border-gray-200"
                }`}
              >
                <option value="">
                  {loadingData ? "Cargando empleados..." : "Seleccionar empleado"}
                </option>
                {empleados.map((e) => (
                  <option key={e.id} value={e.id}>
                    {e.firstName} {e.lastName}
                  </option>
                ))}
              </select>
            </div>
            {fieldErrors.empleadoId && (
              <p className="text-xs text-red-500 mt-1">{fieldErrors.empleadoId}</p>
            )}
          </div>

          {/* Turno */}
          <div>
            <label className="block text-sm font-medium text-slate-700 mb-1.5">
              Turno <span className="text-red-500">*</span>
            </label>
            <div className="relative">
              <FiClock className="absolute left-3 top-3 text-gray-400" size={16} />
              <select
                name="shiftId"
                value={formData.shiftId}
                onChange={handleChange}
                disabled={isLoading || loadingData}
                className={`w-full pl-10 pr-4 py-2.5 rounded-xl border text-sm focus:outline-none focus:ring-2 focus:ring-purple-500/20 appearance-none bg-white ${
                  fieldErrors.shiftId ? "border-red-300" : "border-gray-200"
                }`}
              >
                <option value="">
                  {loadingData ? "Cargando turnos..." : "Seleccionar turno"}
                </option>
                {turnos.map((t) => (
                  <option key={t.id} value={t.id}>
                    {t.name} ({String(t.startTime).slice(0, 5)} - {String(t.endTime).slice(0, 5)})
                  </option>
                ))}
              </select>
            </div>
            {fieldErrors.shiftId && (
              <p className="text-xs text-red-500 mt-1">{fieldErrors.shiftId}</p>
            )}
          </div>

          {/* Fecha */}
          <div>
            <label className="block text-sm font-medium text-slate-700 mb-1.5">
              Fecha de trabajo <span className="text-red-500">*</span>
            </label>
            <div className="relative">
              <FiCalendar className="absolute left-3 top-3 text-gray-400" size={16} />
              <input
                type="date"
                name="workDate"
                value={formData.workDate}
                onChange={handleChange}
                min={hoy}
                disabled={isLoading}
                className={`w-full pl-10 pr-4 py-2.5 rounded-xl border text-sm focus:outline-none focus:ring-2 focus:ring-purple-500/20 ${
                  fieldErrors.workDate ? "border-red-300" : "border-gray-200"
                }`}
              />
            </div>
            {fieldErrors.workDate && (
              <p className="text-xs text-red-500 mt-1">{fieldErrors.workDate}</p>
            )}
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
              disabled={isLoading || loadingData}
              className="flex-1 px-4 py-2.5 rounded-xl bg-purple-600 text-white font-semibold text-sm hover:bg-purple-700 transition disabled:opacity-50 flex items-center justify-center gap-2"
            >
              {isLoading ? (
                <>
                  <span className="w-4 h-4 border-2 border-white/30 border-t-white rounded-full animate-spin" />
                  Asignando...
                </>
              ) : (
                "Asignar turno"
              )}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};
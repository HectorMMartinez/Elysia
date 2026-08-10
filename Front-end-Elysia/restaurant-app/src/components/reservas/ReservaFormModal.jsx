import { useState, useEffect } from "react";
import { FaTimes, FaSave, FaSpinner } from "react-icons/fa";
import { mesaService } from "../../services/mesaService";

export const ReservaFormModal = ({
  isOpen,
  onClose,
  onSubmit,
  initialData = null,
  isLoading = false,
}) => {
  const [form, setForm] = useState({
    nombreCliente: "",
    dniCliente: "",
    mesaId: "",
    cantidadPersona: 1,
    fechaReserva: "",
    observaciones: "",
  });

  const [mesasDisponibles, setMesasDisponibles] = useState([]);
  const [loadingMesas, setLoadingMesas] = useState(false);
  const [errors, setErrors] = useState({});
  const [submitError, setSubmitError] = useState(null); // ← NUEVO

  // Cargar mesas disponibles
  useEffect(() => {
    if (isOpen) {
      loadMesas();
      setSubmitError(null); // limpiar error al abrir
    }
  }, [isOpen]);

  // Cargar datos si es edición
  useEffect(() => {
    if (initialData) {
      // Convertir la fecha del backend a formato datetime-local (hora local)
      let fechaLocal = "";
      if (initialData.fechaReserva) {
        const date = new Date(initialData.fechaReserva);
        // Ajuste para mostrar la hora local correctamente en el input
        const offset = date.getTimezoneOffset() * 60000;
        const localDate = new Date(date.getTime() - offset);
        fechaLocal = localDate.toISOString().slice(0, 16);
      }

      setForm({
        nombreCliente: initialData.nombreCliente || "",
        dniCliente: initialData.dniCliente || "",
        mesaId: initialData.mesaId || "",
        cantidadPersona: initialData.cantidadPersona || 1,
        fechaReserva: fechaLocal,
        observaciones: initialData.observaciones || "",
      });
    } else {
      setForm({
        nombreCliente: "",
        dniCliente: "",
        mesaId: "",
        cantidadPersona: 1,
        fechaReserva: "",
        observaciones: "",
      });
    }
    setErrors({});
    setSubmitError(null);
  }, [initialData, isOpen]);

  const loadMesas = async () => {
    try {
      setLoadingMesas(true);
      const data = await mesaService.getAllDisponibles();
      setMesasDisponibles(Array.isArray(data) ? data : []);
    } catch (err) {
      console.error("Error cargando mesas disponibles:", err);
      setMesasDisponibles([]);
    } finally {
      setLoadingMesas(false);
    }
  };

  const handleChange = (e) => {
    const { name, value } = e.target;
    setForm((prev) => ({ ...prev, [name]: value }));
    // Limpiar error de submit al cambiar algo
    if (submitError) setSubmitError(null);
  };

  const validate = () => {
    const newErrors = {};
    if (!form.nombreCliente.trim()) newErrors.nombreCliente = "Nombre obligatorio";
    if (!form.dniCliente.trim()) newErrors.dniCliente = "DNI obligatorio";
    if (!form.mesaId) newErrors.mesaId = "Debes seleccionar una mesa";
    if (!form.cantidadPersona || form.cantidadPersona < 1)
      newErrors.cantidadPersona = "Cantidad inválida";
    if (!form.fechaReserva) newErrors.fechaReserva = "Fecha obligatoria";
    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (!validate()) return;

    setSubmitError(null);

    // 🔑 FIX DE HORA: enviamos la fecha local sin convertir a UTC
    // datetime-local ya viene como "YYYY-MM-DDTHH:mm"
    // Le agregamos segundos para que .NET lo parseé bien
    const fechaLocal = form.fechaReserva.length === 16
      ? `${form.fechaReserva}:00`
      : form.fechaReserva;

    const payload = {
      nombreCliente: form.nombreCliente.trim(),
      dniCliente: form.dniCliente.trim(),
      mesaId: Number(form.mesaId),
      cantidadPersona: Number(form.cantidadPersona),
      fechaReserva: fechaLocal, // ← SIN toISOString()
      observaciones: form.observaciones?.trim() || null,
    };

    if (initialData) {
      payload.estado = initialData.estado;
    }

    try {
      await onSubmit(payload);
      // Si llega aquí, el padre cerró el modal (éxito)
    } catch (err) {
      // Mostrar el error del backend y mantener el modal abierto
      setSubmitError(err.message || "Ocurrió un error al guardar la reserva.");
    }
  };

  if (!isOpen) return null;

  return (
    <div className="fixed inset-0 z-[90] flex items-center justify-center p-4">
      <div
        className="absolute inset-0 bg-black/50 backdrop-blur-sm"
        onClick={isLoading ? undefined : onClose}
      />

      <div className="relative w-full max-w-2xl bg-white rounded-2xl shadow-2xl overflow-hidden max-h-[90vh] flex flex-col">
        {/* Header */}
        <div className="flex items-center justify-between px-6 py-5 border-b border-slate-100">
          <div>
            <h3 className="text-xl font-bold text-slate-800">
              {initialData ? "Editar Reserva" : "Nueva Reserva"}
            </h3>
            <p className="text-sm text-slate-500 mt-0.5">
              {initialData
                ? "Modifica los datos de la reserva"
                : "Completa los datos del cliente y la mesa"}
            </p>
          </div>
          <button
            onClick={onClose}
            disabled={isLoading}
            className="p-2 rounded-lg text-slate-400 hover:bg-slate-100 transition disabled:opacity-50"
          >
            <FaTimes size={18} />
          </button>
        </div>

        {/* Body */}
        <form onSubmit={handleSubmit} className="flex-1 overflow-y-auto p-6 space-y-5">
          
          {/* 🔑 MENSAJE DE ERROR DEL BACKEND */}
          {submitError && (
            <div className="p-4 rounded-xl bg-red-50 border border-red-200 text-red-700 text-sm font-medium">
              {submitError}
            </div>
          )}

          <div className="grid grid-cols-1 md:grid-cols-2 gap-5">
            {/* Nombre Cliente */}
            <div>
              <label className="block text-sm font-semibold text-slate-700 mb-1.5">
                Nombre del Cliente *
              </label>
              <input
                type="text"
                name="nombreCliente"
                value={form.nombreCliente}
                onChange={handleChange}
                className={`w-full px-4 py-2.5 rounded-xl border text-sm focus:outline-none focus:ring-2 focus:ring-purple-500/20 ${
                  errors.nombreCliente ? "border-red-300" : "border-gray-200"
                }`}
                placeholder="Ej: Juan Pérez"
              />
              {errors.nombreCliente && (
                <p className="text-xs text-red-500 mt-1">{errors.nombreCliente}</p>
              )}
            </div>

            {/* DNI */}
            <div>
              <label className="block text-sm font-semibold text-slate-700 mb-1.5">
                DNI del Cliente *
              </label>
              <input
                type="text"
                name="dniCliente"
                value={form.dniCliente}
                onChange={handleChange}
                className={`w-full px-4 py-2.5 rounded-xl border text-sm focus:outline-none focus:ring-2 focus:ring-purple-500/20 ${
                  errors.dniCliente ? "border-red-300" : "border-gray-200"
                }`}
                placeholder="Ej: 001-1234567-8"
              />
              {errors.dniCliente && (
                <p className="text-xs text-red-500 mt-1">{errors.dniCliente}</p>
              )}
            </div>

            {/* Mesa */}
            <div>
              <label className="block text-sm font-semibold text-slate-700 mb-1.5">
                Mesa *
              </label>
              <select
                name="mesaId"
                value={form.mesaId}
                onChange={handleChange}
                disabled={loadingMesas}
                className={`w-full px-4 py-2.5 rounded-xl border text-sm focus:outline-none focus:ring-2 focus:ring-purple-500/20 ${
                  errors.mesaId ? "border-red-300" : "border-gray-200"
                }`}
              >
                <option value="">
                  {loadingMesas ? "Cargando mesas..." : "Selecciona una mesa"}
                </option>
                {mesasDisponibles.map((mesa) => (
                  <option key={mesa.id} value={mesa.id}>
                    {mesa.nombre} — Capacidad: {mesa.capacidad} personas
                  </option>
                ))}
              </select>
              {errors.mesaId && (
                <p className="text-xs text-red-500 mt-1">{errors.mesaId}</p>
              )}
            </div>

            {/* Cantidad Personas */}
            <div>
              <label className="block text-sm font-semibold text-slate-700 mb-1.5">
                Cantidad de Personas *
              </label>
              <input
                type="number"
                name="cantidadPersona"
                min="1"
                value={form.cantidadPersona}
                onChange={handleChange}
                className={`w-full px-4 py-2.5 rounded-xl border text-sm focus:outline-none focus:ring-2 focus:ring-purple-500/20 ${
                  errors.cantidadPersona ? "border-red-300" : "border-gray-200"
                }`}
              />
              {errors.cantidadPersona && (
                <p className="text-xs text-red-500 mt-1">{errors.cantidadPersona}</p>
              )}
            </div>

            {/* Fecha y Hora */}
            <div className="md:col-span-2">
              <label className="block text-sm font-semibold text-slate-700 mb-1.5">
                Fecha y Hora de la Reserva *
              </label>
              <input
                type="datetime-local"
                name="fechaReserva"
                value={form.fechaReserva}
                onChange={handleChange}
                className={`w-full px-4 py-2.5 rounded-xl border text-sm focus:outline-none focus:ring-2 focus:ring-purple-500/20 ${
                  errors.fechaReserva ? "border-red-300" : "border-gray-200"
                }`}
              />
              {errors.fechaReserva && (
                <p className="text-xs text-red-500 mt-1">{errors.fechaReserva}</p>
              )}
            </div>

            {/* Observaciones */}
            <div className="md:col-span-2">
              <label className="block text-sm font-semibold text-slate-700 mb-1.5">
                Observaciones
              </label>
              <textarea
                name="observaciones"
                value={form.observaciones}
                onChange={handleChange}
                rows={3}
                className="w-full px-4 py-2.5 rounded-xl border border-gray-200 text-sm focus:outline-none focus:ring-2 focus:ring-purple-500/20 resize-none"
                placeholder="Notas adicionales (opcional)"
              />
            </div>
          </div>
        </form>

        {/* Footer */}
        <div className="flex gap-3 px-6 py-5 border-t border-slate-100 bg-slate-50/50">
          <button
            type="button"
            onClick={onClose}
            disabled={isLoading}
            className="flex-1 px-4 py-3 rounded-xl border border-slate-200 text-slate-600 font-medium hover:bg-white transition disabled:opacity-50"
          >
            Cancelar
          </button>
          <button
            type="button"
            onClick={handleSubmit}
            disabled={isLoading}
            className="flex-1 px-4 py-3 rounded-xl bg-purple-600 text-white font-medium hover:bg-purple-700 transition disabled:opacity-50 flex items-center justify-center gap-2"
          >
            {isLoading ? (
              <>
                <FaSpinner className="animate-spin" />
                Guardando...
              </>
            ) : (
              <>
                <FaSave />
                {initialData ? "Actualizar Reserva" : "Crear Reserva"}
              </>
            )}
          </button>
        </div>
      </div>
    </div>
  );
};
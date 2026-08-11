import { useState, useEffect } from "react";
import { FaTimes, FaSave, FaSpinner, FaPlus, FaTrash } from "react-icons/fa";
import { mesaService } from "../../services/mesaService";
import { obtenerPlatosAsociadosMenu } from "../../services/platoService";

export const PedidoFormModal = ({
  isOpen,
  onClose,
  onSubmit,
  initialData = null,
  isLoading = false,
}) => {
  const [form, setForm] = useState({
    idMesa: "",
    detallesPedido: [{ platoId: "", cantidad: 1, observaciones: "" }],
  });

  const [mesas, setMesas] = useState([]);
  const [platos, setPlatos] = useState([]);
  const [loadingData, setLoadingData] = useState(false);
  const [errors, setErrors] = useState({});
  const [submitError, setSubmitError] = useState(null);

  useEffect(() => {
    if (isOpen) {
      loadData();
      setSubmitError(null);
    }
  }, [isOpen]);

  useEffect(() => {
  if (initialData) {
    const detalles =
      initialData.mostrarDetalles?.map((d) => ({
        platoId: d.idPlato || "",
        cantidad: d.cantidaPlato || 1,
        observaciones: d.observaciones || "",
      })) || [{ platoId: "", cantidad: 1, observaciones: "" }];

   setForm({
      idMesa: initialData.idMesa != null
     ? String(initialData.idMesa)
      : "",
  detallesPedido: detalles,
  estado: initialData.estado,
});
  } else {
    setForm({
      idMesa: "",
      detallesPedido: [{ platoId: "", cantidad: 1, observaciones: "" }],
    });
  }
  setErrors({});
  setSubmitError(null);
}, [initialData, isOpen]);


const loadData = async () => {
    try {
        setLoadingData(true);
        setSubmitError(null);

        let errorMessage = null;
        try {
           const mesasData = await mesaService.getAllDisponibles();

           let mesasDisponibles = Array.isArray(mesasData)? mesasData : [];

            if (initialData?.idMesa) {
            const mesaActualExiste = mesasDisponibles.some(
            (mesa) =>
            Number(mesa.id) === Number(initialData.idMesa));

            console.log("Mesa del pedido:", initialData.idMesa);
            console.log("Mesa encontrada:", mesaActualExiste);
            console.log("Mesas disponibles:", mesasDisponibles);

            if (!mesaActualExiste) {
               console.warn("La mesa del pedido no está en getAllDisponibles()");
  }
}

setMesas(mesasDisponibles);
        } catch (err) {
            console.error("Error cargando mesas:", err);

            setMesas([]);

            errorMessage =
                err.message || "No se pudieron cargar las mesas disponibles.";
        }

        try {
            const platosData = await obtenerPlatosAsociadosMenu();

            setPlatos(
                Array.isArray(platosData)
                    ? platosData
                    : []
            );
        } catch (err) {
            console.error("Error cargando platos:", err);

            setPlatos([]);

            errorMessage =
                err.message || "No se pudieron cargar los platos del menú.";
        }

        if (errorMessage) {
            setSubmitError(errorMessage);
        }

    } finally {
        setLoadingData(false);
    }
};

const calcularTotal = () => {
    return form.detallesPedido.reduce((total, detalle) => {
        const plato = platos.find(
            (p) => Number(p.id) === Number(detalle.platoId)
        );

        if (!plato) return total;

        return total + Number(plato.precio) * Number(detalle.cantidad || 0);
    }, 0);
};


  const handleChange = (e) => {
    const { name, value } = e.target;
    setForm((prev) => ({ ...prev, [name]: value }));
    if (submitError) setSubmitError(null);
  };

  const handleDetalleChange = (index, field, value) => {
    setForm((prev) => {
      const detalles = [...prev.detallesPedido];
      detalles[index] = { ...detalles[index], [field]: value };
      return { ...prev, detallesPedido: detalles };
    });
    if (submitError) setSubmitError(null);
  };

  const addDetalle = () => {
    setForm((prev) => ({
      ...prev,
      detallesPedido: [
        ...prev.detallesPedido,
        { platoId: "", cantidad: 1, observaciones: "" },
      ],
    }));
  };

  const removeDetalle = (index) => {
    setForm((prev) => {
      if (prev.detallesPedido.length <= 1) return prev;
      return {
        ...prev,
        detallesPedido: prev.detallesPedido.filter((_, i) => i !== index),
      };
    });
  };

  const validate = () => {
    const newErrors = {};
    if (!form.idMesa) newErrors.idMesa = "Debes seleccionar una mesa";

    form.detallesPedido.forEach((d, i) => {
      if (!d.platoId) newErrors[`plato_${i}`] = "Selecciona un plato";
      if (!d.cantidad || Number(d.cantidad) < 1)
        newErrors[`cantidad_${i}`] = "Cantidad inválida";
    });

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (!validate()) return;

    setSubmitError(null);

    const payload = {
      idMesa: Number(form.idMesa),
      detallesPedido: form.detallesPedido.map((d) => ({
        platoId: Number(d.platoId),
        cantidad: Number(d.cantidad),
        observaciones: d.observaciones?.trim() || null,
      })),
    };

    if (initialData) {
      payload.estado = form.estado || initialData.estado;
    }

    try {
      await onSubmit(payload);
    } catch (err) {
      setSubmitError(err.message || "Ocurrió un error al guardar el pedido.");
    }
  };

  if (!isOpen) return null;

  return (
    <div className="fixed inset-0 z-[90] flex items-center justify-center p-4">
      <div
        className="absolute inset-0 bg-black/50 backdrop-blur-sm"
        onClick={isLoading ? undefined : onClose}
      />

      <div className="relative w-full max-w-3xl bg-white rounded-2xl shadow-2xl overflow-hidden max-h-[90vh] flex flex-col">
        {/* Header */}
        <div className="flex items-center justify-between px-6 py-5 border-b border-slate-100">
          <div>
            <h3 className="text-xl font-bold text-slate-800">
              {initialData ? "Editar Pedido" : "Nuevo Pedido"}
            </h3>
            <p className="text-sm text-slate-500 mt-0.5">
              {initialData
                ? "Modifica mesa y platos del pedido"
                : "Selecciona mesa y agrega platos del menú"}
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
        <form
          onSubmit={handleSubmit}
          className="flex-1 overflow-y-auto p-6 space-y-5"
        >
          {submitError && (
            <div className="p-4 rounded-xl bg-red-50 border border-red-200 text-red-700 text-sm font-medium">
              {submitError}
            </div>
          )}

          {/* Mesa */}
          <div>
            <label className="block text-sm font-semibold text-slate-700 mb-1.5">
              Mesa *
            </label>
            <select
              name="idMesa"
              value={form.idMesa}
              onChange={handleChange}
              disabled={loadingData}
              className={`w-full px-4 py-2.5 rounded-xl border text-sm focus:outline-none focus:ring-2 focus:ring-purple-500/20 ${
                errors.idMesa ? "border-red-300" : "border-gray-200"
              }`}
            >
              <option value="">
                {loadingData ? "Cargando mesas..." : "Selecciona una mesa"}
              </option>
              {mesas.map((mesa) => (
                <option key={mesa.id} value={mesa.id}>
                  {mesa.nombre} — Capacidad: {mesa.capacidad} personas
                </option>
              ))}
            </select>
            {errors.idMesa && (
              <p className="text-xs text-red-500 mt-1">{errors.idMesa}</p>
            )}
          </div>

          {/* Detalles / Platos */}
          <div className="space-y-3">
            <div className="flex items-center justify-between">
              <label className="text-sm font-semibold text-slate-700">
                Platos del pedido *
              </label>
              <button
                type="button"
                onClick={addDetalle}
                className="flex items-center gap-1.5 text-sm font-medium text-purple-600 hover:text-purple-700"
              >
                <FaPlus size={12} />
                Agregar plato
              </button>
            </div>

            {form.detallesPedido.map((detalle, index) => (
              <div
                key={index}
                className="p-4 rounded-xl border border-gray-100 bg-slate-50/50 space-y-3"
              >
                <div className="flex items-start gap-3">
                  <div className="flex-1 grid grid-cols-1 md:grid-cols-2 gap-3">
                    {/* Plato */}
                    <div>
                      <label className="block text-xs font-medium text-slate-500 mb-1">
                        Plato
                      </label>
                      <select
                        value={detalle.platoId}
                        onChange={(e) =>
                          handleDetalleChange(index, "platoId", e.target.value)
                        }
                        disabled={loadingData}
                        className={`w-full px-3 py-2 rounded-lg border text-sm focus:outline-none focus:ring-2 focus:ring-purple-500/20 ${
                          errors[`plato_${index}`]
                            ? "border-red-300"
                            : "border-gray-200"
                        }`}
                      >
                        <option value="">Selecciona plato</option>
                        {platos.map((plato) => (
                          <option key={plato.id} value={plato.id}>
                            {plato.nombre} — RD$ {plato.precio}
                          </option>
                        ))}
                      </select>
                      {errors[`plato_${index}`] && (
                        <p className="text-xs text-red-500 mt-1">
                          {errors[`plato_${index}`]}
                        </p>
                      )}
                    </div>

                    {/* Cantidad */}
                    <div>
                      <label className="block text-xs font-medium text-slate-500 mb-1">
                        Cantidad
                      </label>
                      <input
                        type="number"
                        min="1"
                        value={detalle.cantidad}
                        onChange={(e) =>
                          handleDetalleChange(index, "cantidad", e.target.value)
                        }
                        className={`w-full px-3 py-2 rounded-lg border text-sm focus:outline-none focus:ring-2 focus:ring-purple-500/20 ${
                          errors[`cantidad_${index}`]
                            ? "border-red-300"
                            : "border-gray-200"
                        }`}
                      />
                      {errors[`cantidad_${index}`] && (
                        <p className="text-xs text-red-500 mt-1">
                          {errors[`cantidad_${index}`]}
                        </p>
                      )}
                    </div>
                  </div>

                  {form.detallesPedido.length > 1 && (
                    <button
                      type="button"
                      onClick={() => removeDetalle(index)}
                      className="mt-6 p-2 text-red-500 hover:bg-red-50 rounded-lg transition"
                      title="Quitar plato"
                    >
                      <FaTrash size={14} />
                    </button>
                  )}
                </div>

                {/* Observaciones del plato */}
                <div>
                  <label className="block text-xs font-medium text-slate-500 mb-1">
                    Observaciones (opcional)
                  </label>
                  <input
                    type="text"
                    value={detalle.observaciones}
                    onChange={(e) =>
                      handleDetalleChange(index, "observaciones", e.target.value)
                    }
                    placeholder="Ej: Sin cebolla, término medio..."
                    className="w-full px-3 py-2 rounded-lg border border-gray-200 text-sm focus:outline-none focus:ring-2 focus:ring-purple-500/20"
                  />
                </div>
              </div>
            ))}
          </div>
          <div className="mt-6 pt-5 border-t border-slate-200">
            <div className="flex items-center justify-between">
              <span className="text-base font-semibold text-slate-700">
                Total del pedido
              </span>

              <span className="text-2xl font-bold text-purple-600">
                RD$ {calcularTotal().toLocaleString("es-DO", {
                  minimumFractionDigits: 2,
                  maximumFractionDigits: 2,
                })}
              </span>
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
            disabled={isLoading || loadingData}
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
                {initialData ? "Actualizar Pedido" : "Crear Pedido"}
              </>
            )}
          </button>
        </div>
      </div>
    </div>
  );
};
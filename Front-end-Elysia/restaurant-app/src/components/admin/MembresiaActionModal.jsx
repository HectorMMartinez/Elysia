export default function MembresiaActionModal({
  membresia,
  action,
  loading,
  onConfirm,
  onCancel,
}) {
  if (!membresia || !action) {
    return null;
  }

  const configuracion = {
    cancelar: {
      titulo: "Cancelar membresía",
      mensaje: "¿Está seguro de que desea cancelar esta membresía?",
      boton: "Sí, cancelar membresía",
      color: "red",
    },
    suspender: {
      titulo: "Suspender membresía",
      mensaje: "¿Está seguro de que desea suspender esta membresía?",
      boton: "Sí, suspender membresía",
      color: "amber",
    },
    activar: {
      titulo: "Activar membresía",
      mensaje: "¿Está seguro de que desea activar esta membresía por un mes?",
      boton: "Sí, activar por un mes",
      color: "green",
    },
    "cambiar-simple": {
      titulo: "Cambiar a Plan Simple",
      mensaje: `¿Está seguro de que desea cambiar el plan de "${membresia.userName}" a Plan Simple? Esta acción no se puede deshacer fácilmente.`,
      boton: "Sí, cambiar a Simple",
      color: "indigo",
    },
  };

  const config = configuracion[action];

  if (!config) {
    return null;
  }

  const colores = {
    red: {
      icon: "bg-red-100 text-red-600",
      button: "bg-red-600 hover:bg-red-700",
      border: "border-red-200",
    },
    amber: {
      icon: "bg-amber-100 text-amber-600",
      button: "bg-amber-500 hover:bg-amber-600",
      border: "border-amber-200",
    },
    green: {
      icon: "bg-green-100 text-green-600",
      button: "bg-green-600 hover:bg-green-700",
      border: "border-green-200",
    },
    indigo: {
      icon: "bg-indigo-100 text-indigo-600",
      button: "bg-indigo-600 hover:bg-indigo-700",
      border: "border-indigo-200",
    },
  };

  const color = colores[config.color];

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/40 px-4">
      <div className="w-full max-w-md rounded-2xl bg-white shadow-xl">
        {/* HEADER */}
        <div className="p-6">
          <div className="flex items-start gap-4">
            <div
              className={`flex h-11 w-11 shrink-0 items-center justify-center rounded-full ${color.icon}`}
            >
              <span className="text-lg font-bold">!</span>
            </div>

            <div>
              <h2 className="text-xl font-bold text-slate-800">
                {config.titulo}
              </h2>
              <p className="mt-2 text-sm text-slate-500">{config.mensaje}</p>
            </div>
          </div>

          {/* INFORMACIÓN */}
          <div className="mt-5 rounded-xl border border-slate-200 bg-slate-50 p-4">
            <div className="space-y-2">
              <div className="flex justify-between gap-4">
                <span className="text-sm text-slate-500">Propietario</span>
                <span className="text-sm font-semibold text-slate-700">
                  {membresia.userName}
                </span>
              </div>

              <div className="flex justify-between gap-4">
                <span className="text-sm text-slate-500">Restaurante</span>
                <span className="text-sm font-semibold text-slate-700 text-right">
                  {membresia.nombreRestaurante}
                </span>
              </div>

              <div className="flex justify-between gap-4">
                <span className="text-sm text-slate-500">Plan</span>
                <span className="text-sm font-semibold text-slate-700">
                  {membresia.nombrePlan}
                </span>
              </div>

              <div className="flex justify-between gap-4">
                <span className="text-sm text-slate-500">Estado actual</span>
                <span className="text-sm font-semibold text-slate-700">
                  {String(membresia.estado)}
                </span>
              </div>
            </div>
          </div>
        </div>

        {/* ACCIONES */}
        <div className="flex justify-end gap-3 border-t border-slate-100 bg-slate-50 px-6 py-4">
          <button
            type="button"
            onClick={onCancel}
            disabled={loading}
            className="rounded-xl border border-slate-200 bg-white px-4 py-2.5 text-sm font-medium text-slate-600 transition hover:bg-slate-100 disabled:cursor-not-allowed disabled:opacity-50"
          >
            Cancelar
          </button>

          <button
            type="button"
            onClick={onConfirm}
            disabled={loading}
            className={`rounded-xl px-4 py-2.5 text-sm font-medium text-white transition disabled:cursor-not-allowed disabled:opacity-50 ${color.button}`}
          >
            {loading ? "Procesando..." : config.boton}
          </button>
        </div>
      </div>
    </div>
  );
}
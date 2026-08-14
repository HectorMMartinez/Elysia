import { FaTimes, FaExclamationTriangle, FaSpinner } from "react-icons/fa";

export default function ConfirmActionModalPedido({
  isOpen,
  loading,
  title,
  subtitle,
  message,
  confirmText = "Confirmar",
  confirmColor = "bg-violet-600 hover:bg-violet-700",
  onConfirm,
  onCancel,
}) {
  if (!isOpen) return null;

  return (
    <div className="fixed inset-0 z-[100] flex items-center justify-center p-4">
      <div
        className="absolute inset-0 bg-black/50 backdrop-blur-sm"
        onClick={loading ? undefined : onCancel}
      />

      <div className="relative w-full max-w-md bg-white rounded-2xl shadow-2xl overflow-hidden">
        <div className="flex items-center justify-between px-6 py-5 border-b border-slate-100">
          <div className="flex items-center gap-3">
            <div className="w-11 h-11 rounded-xl bg-amber-50 flex items-center justify-center">
              <FaExclamationTriangle className="text-amber-500" />
            </div>
            <div>
              <h3 className="text-lg font-semibold text-slate-800">{title}</h3>
              <p className="text-sm text-slate-500">{subtitle}</p>
            </div>
          </div>
          <button
            type="button"
            onClick={onCancel}
            disabled={loading}
            className="p-2 rounded-lg text-slate-400 hover:bg-slate-100 transition disabled:opacity-50"
          >
            <FaTimes />
          </button>
        </div>

        <div className="p-6">
          <div className="flex items-start gap-3 p-4 rounded-xl bg-amber-50 border border-amber-200">
            <FaExclamationTriangle className="text-amber-500 mt-0.5 flex-shrink-0" />
            <p className="text-sm text-amber-800 font-medium">{message}</p>
          </div>
        </div>

        <div className="flex gap-3 px-6 pb-6">
          <button
            type="button"
            onClick={onCancel}
            disabled={loading}
            className="flex-1 px-4 py-3 rounded-xl border border-slate-200 text-slate-600 font-medium hover:bg-slate-50 transition disabled:opacity-50"
          >
            Cancelar
          </button>
          <button
            type="button"
            onClick={onConfirm}
            disabled={loading}
            className={`flex-1 px-4 py-3 rounded-xl text-white font-medium transition disabled:opacity-50 flex items-center justify-center gap-2 ${confirmColor}`}
          >
            {loading ? (
              <>
                <FaSpinner className="animate-spin" />
                Procesando...
              </>
            ) : (
              confirmText
            )}
          </button>
        </div>
      </div>
    </div>
  );
}
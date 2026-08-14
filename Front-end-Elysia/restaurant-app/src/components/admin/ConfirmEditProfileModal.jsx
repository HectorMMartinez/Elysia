import {
  FaTimes,
  FaUserEdit,
  FaExclamationTriangle,
  FaSpinner,
} from "react-icons/fa";

export default function ConfirmEditProfileModal({
  isOpen,
  loading,
  onConfirm,
  onCancel,
}) {
  if (!isOpen) {
    return null;
  }

  return (
    <div className="fixed inset-0 z-[100] flex items-center justify-center p-4">
      
      {/* Overlay */}
      <div
        className="absolute inset-0 bg-black/50 backdrop-blur-sm"
        onClick={loading ? undefined : onCancel}
      />

      {/* Modal */}
      <div className="relative w-full max-w-md bg-white rounded-2xl shadow-2xl overflow-hidden">

        {/* =====================================================
            HEADER
        ====================================================== */}

        <div className="flex items-center justify-between px-6 py-5 border-b border-slate-100">

          <div className="flex items-center gap-3">

            <div className="w-11 h-11 rounded-xl bg-violet-100 flex items-center justify-center">
              <FaUserEdit className="text-violet-600 text-lg" />
            </div>

            <div>
              <h3 className="text-lg font-semibold text-slate-800">
                Confirmar cambios
              </h3>

              <p className="text-sm text-slate-500">
                Actualización del perfil
              </p>
            </div>

          </div>

          <button
            type="button"
            onClick={onCancel}
            disabled={loading}
            className="p-2 rounded-lg text-slate-400 hover:bg-slate-100 hover:text-slate-600 transition disabled:opacity-50"
          >
            <FaTimes />
          </button>

        </div>

        {/* =====================================================
            BODY
        ====================================================== */}

        <div className="p-6">

          <div className="flex items-start gap-3 p-4 rounded-xl bg-amber-50 border border-amber-200">

            <FaExclamationTriangle className="text-amber-500 mt-0.5 flex-shrink-0" />

            <div>
              <p className="text-sm font-semibold text-amber-800">
                ¿Deseas guardar estos cambios?
              </p>

              <p className="text-sm text-amber-700 mt-1">
                La información de tu perfil será actualizada.
              </p>
            </div>

          </div>

          <p className="text-sm text-slate-500 mt-4">
            Si modificaste tu contraseña, también será actualizada.
            Asegúrate de que todos los datos sean correctos antes de
            continuar.
          </p>

        </div>

        {/* =====================================================
            ACTIONS
        ====================================================== */}

        <div className="flex gap-3 px-6 pb-6">

          <button
            type="button"
            onClick={onCancel}
            disabled={loading}
            className="flex-1 px-4 py-3 rounded-xl border border-slate-200 text-slate-600 font-medium hover:bg-slate-50 transition disabled:opacity-50 disabled:cursor-not-allowed"
          >
            Cancelar
          </button>

          <button
            type="button"
            onClick={onConfirm}
            disabled={loading}
            className="flex-1 px-4 py-3 rounded-xl bg-violet-600 text-white font-medium hover:bg-violet-700 transition disabled:opacity-50 disabled:cursor-not-allowed flex items-center justify-center gap-2"
          >

            {loading ? (
              <>
                <FaSpinner className="animate-spin" />
                Guardando...
              </>
            ) : (
              <>
                <FaUserEdit />
                Confirmar
              </>
            )}

          </button>

        </div>

      </div>
    </div>
  );
}




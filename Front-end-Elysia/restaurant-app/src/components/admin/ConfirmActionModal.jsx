import {
  FaTimes,
  FaCheckCircle,
  FaExclamationTriangle,
} from "react-icons/fa";

export default function ConfirmActionModal({
  isOpen,
  action,
  user,
  loading,
  onConfirm,
  onCancel,
}) {
  if (!isOpen || !user) {
    return null;
  }

  const isActivate = action === "activate";

  const title = isActivate
    ? "Activar propietario"
    : "Inactivar propietario";

  const description = isActivate
    ? "El propietario podrá volver a utilizar su cuenta y acceder al sistema."
    : "El propietario dejará de tener acceso al sistema hasta que vuelva a ser activado.";

  const buttonText = isActivate
    ? "Sí, activar"
    : "Sí, inactivar";

  return (
    <div
      className="fixed inset-0 z-[60] flex items-center justify-center bg-black/50 backdrop-blur-sm p-4"
      onMouseDown={(e) => {
        if (e.target === e.currentTarget && !loading) {
          onCancel();
        }
      }}
    >
      <div className="bg-white w-full max-w-md rounded-2xl shadow-2xl overflow-hidden">

        {/* HEADER */}

        <div className="flex items-center justify-between px-6 py-5 border-b border-slate-100">
          <h2 className="text-xl font-bold text-slate-800">
            {title}
          </h2>

          <button
            type="button"
            onClick={onCancel}
            disabled={loading}
            className="w-9 h-9 flex items-center justify-center rounded-lg text-slate-400 hover:bg-slate-100 hover:text-slate-700 transition disabled:opacity-50"
          >
            <FaTimes />
          </button>
        </div>

        {/* CONTENIDO */}

        <div className="px-6 py-6">

          <div
            className={`w-14 h-14 rounded-full flex items-center justify-center mb-5 ${
              isActivate
                ? "bg-green-100"
                : "bg-red-100"
            }`}
          >
            {isActivate ? (
              <FaCheckCircle
                className="text-green-600 text-2xl"
              />
            ) : (
              <FaExclamationTriangle
                className="text-red-600 text-2xl"
              />
            )}
          </div>

          <p className="text-slate-700 text-base leading-relaxed">
            ¿Estás seguro de que deseas{" "}
            <span className="font-bold">
              {isActivate ? "activar" : "inactivar"}
            </span>{" "}
            al propietario?
          </p>

          <div className="mt-4 bg-slate-50 rounded-xl p-4">
            <p className="font-semibold text-slate-800">
              {user.name} {user.lastName}
            </p>

            <p className="text-sm text-slate-500 mt-1">
              @{user.userName}
            </p>

            <p className="text-sm text-slate-500">
              {user.email}
            </p>
          </div>

          <p className="text-sm text-slate-500 mt-4 leading-relaxed">
            {description}
          </p>
        </div>

        {/* FOOTER */}

        <div className="px-6 py-4 bg-slate-50 border-t border-slate-100 flex justify-end gap-3">

          <button
            type="button"
            onClick={onCancel}
            disabled={loading}
            className="px-5 py-2.5 rounded-xl border border-slate-200 bg-white text-slate-700 font-medium hover:bg-slate-100 transition disabled:opacity-50"
          >
            Cancelar
          </button>

          <button
            type="button"
            onClick={onConfirm}
            disabled={loading}
            className={`px-5 py-2.5 rounded-xl text-white font-medium transition flex items-center gap-2 ${
              isActivate
                ? "bg-green-600 hover:bg-green-700"
                : "bg-red-600 hover:bg-red-700"
            } disabled:opacity-50 disabled:cursor-not-allowed`}
          >
            {loading && (
              <span className="w-4 h-4 border-2 border-white/30 border-t-white rounded-full animate-spin" />
            )}

            {loading ? "Procesando..." : buttonText}
          </button>

        </div>
      </div>
    </div>
  );
}
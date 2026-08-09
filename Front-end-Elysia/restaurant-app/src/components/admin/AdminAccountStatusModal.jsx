export default function AdminAccountStatusModal({
  admin,
  action,
  loading,
  onConfirm,
  onCancel,
}) {
  if (!admin) return null;

  const isActivating = action === "activar";

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/50 px-4">
      <div className="w-full max-w-md rounded-2xl bg-white p-6 shadow-2xl">

        {/* Icono */}
        <div
          className={`mx-auto flex h-14 w-14 items-center justify-center rounded-full ${
            isActivating ? "bg-green-100" : "bg-red-100"
          }`}
        >
          {isActivating ? (
            <svg
              className="h-7 w-7 text-green-600"
              fill="none"
              stroke="currentColor"
              viewBox="0 0 24 24"
            >
              <path
                strokeLinecap="round"
                strokeLinejoin="round"
                strokeWidth={2}
                d="M5 13l4 4L19 7"
              />
            </svg>
          ) : (
            <svg
              className="h-7 w-7 text-red-600"
              fill="none"
              stroke="currentColor"
              viewBox="0 0 24 24"
            >
              <path
                strokeLinecap="round"
                strokeLinejoin="round"
                strokeWidth={2}
                d="M12 9v2m0 4h.01M5.07 19h13.86c1.54 0 2.5-1.67 1.73-3L13.73 4c-.77-1.33-2.69-1.33-3.46 0L3.34 16c-.77 1.33.19 3 1.73 3z"
              />
            </svg>
          )}
        </div>

        {/* Título */}
        <h2 className="mt-4 text-center text-xl font-bold text-gray-800">
          {isActivating
            ? "Activar administrador"
            : "Inactivar administrador"}
        </h2>

        {/* Mensaje */}
        <p className="mt-3 text-center text-sm leading-6 text-gray-500">
          ¿Estás seguro de que deseas{" "}
          <span className="font-semibold text-gray-700">
            {isActivating ? "activar" : "inactivar"}
          </span>{" "}
          la cuenta de{" "}
          <span className="font-semibold text-gray-800">
            {admin.name} {admin.lastName}
          </span>
          ?
        </p>

        {!isActivating && (
          <p className="mt-2 text-center text-xs text-gray-400">
            El administrador no podrá utilizar su cuenta mientras esté
            inactiva.
          </p>
        )}

        {/* Botones */}
        <div className="mt-6 flex gap-3">
          <button
            type="button"
            onClick={onCancel}
            disabled={loading}
            className="flex-1 rounded-lg border border-gray-300 px-4 py-2.5 text-sm font-medium text-gray-700 transition hover:bg-gray-50 disabled:cursor-not-allowed disabled:opacity-50"
          >
            Cancelar
          </button>

          <button
            type="button"
            onClick={onConfirm}
            disabled={loading}
            className={`flex-1 rounded-lg px-4 py-2.5 text-sm font-medium text-white transition disabled:cursor-not-allowed disabled:opacity-50 ${
              isActivating
                ? "bg-green-600 hover:bg-green-700"
                : "bg-red-600 hover:bg-red-700"
            }`}
          >
            {loading
              ? "Procesando..."
              : isActivating
              ? "Sí, activar"
              : "Sí, inactivar"}
          </button>
        </div>
      </div>
    </div>
  );
}
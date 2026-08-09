import { FaEye, FaCheck, FaBan } from "react-icons/fa";

export default function UserActions({
  user,
  onView,
  onActivate,
  onDeactivate,
}) {
  return (
    <div className="flex items-center justify-end gap-2">
      {/* Ver detalles */}
      <button
        type="button"
        onClick={() => onView(user)}
        title="Ver detalles"
        className="p-2 rounded-lg text-slate-500 hover:bg-slate-100 hover:text-slate-700 transition"
      >
        <FaEye />
      </button>

      {/* Activar / Inactivar */}
      {user.isActive ? (
        <button
          type="button"
          onClick={() => onDeactivate(user)}
          title="Inactivar usuario"
          className="p-2 rounded-lg text-red-500 hover:bg-red-50 hover:text-red-600 transition"
        >
          <FaBan />
        </button>
      ) : (
        <button
          type="button"
          onClick={() => onActivate(user)}
          title="Activar usuario"
          className="p-2 rounded-lg text-green-500 hover:bg-green-50 hover:text-green-600 transition"
        >
          <FaCheck />
        </button>
      )}
    </div>
  );
}
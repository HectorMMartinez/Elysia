import { FaCheckCircle, FaTimesCircle } from "react-icons/fa";

export default function UserStatusBadge({ isActive }) {
  if (isActive) {
    return (
      <span className="inline-flex items-center gap-1.5 px-3 py-1 rounded-full text-xs font-semibold bg-green-100 text-green-700">
        <FaCheckCircle />
        Activo
      </span>
    );
  }

  return (
    <span className="inline-flex items-center gap-1.5 px-3 py-1 rounded-full text-xs font-semibold bg-red-100 text-red-700">
      <FaTimesCircle />
      Inactivo
    </span>
  );
}
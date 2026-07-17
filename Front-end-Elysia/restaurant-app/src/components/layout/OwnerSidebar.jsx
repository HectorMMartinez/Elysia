import { NavLink, useNavigate } from "react-router-dom";
import {
  FaUtensils,
  FaCalendarCheck,
  FaChair,
  FaDollarSign,
  FaBoxOpen,
  FaUsers,
  FaChartLine,
  FaSignOutAlt,
  FaClipboardList,
  FaUserCircle,
  FaClock,
} from "react-icons/fa";

import { storage } from "../../utils/storage";

export default function OwnerSidebar() {
  const navigate = useNavigate();

  const menuItems = [
    {
      label: "Dashboard",
      path: "/dashboard",
      icon: FaChartLine,
    },
    {
      label: "Inventario",
      path: "/inventario",
      icon: FaBoxOpen,
    },
    {
      label: "Platos",
      path: "/platos",
      icon: FaUtensils,
    },
    {
      label: "Menú",
      path: "/menu",
      icon: FaClipboardList,
    },
    {
      label: "Mesas",
      path: "/mesas",
      icon: FaChair,
    },
    {
      label: "Reservas",
      path: "/reservas",
      icon: FaCalendarCheck,
    },
    {
      label: "Pedidos",
      path: "/pedidos",
      icon: FaDollarSign,
    },
    {
      label: "Empleados",
      path: "/empleados",
      icon: FaUsers,
    },
    {
      label: "Turnos",
      path: "/turnos",
      icon: FaClock,
    },
    {
      label: "Perfil",
      path: "/perfil",
      icon: FaUserCircle,
    },
  ];

  const handleLogout = () => {
    storage.clearAuth();
    navigate("/", { replace: true });
  };

  return (
    <aside className="w-72 min-h-screen bg-slate-900 text-white flex flex-col">
      <div className="p-8 border-b border-slate-700">
        <h1 className="text-3xl font-bold">
          Elysia
        </h1>

        <p className="text-slate-400 mt-2">
          Restaurant Management
        </p>
      </div>

      <nav className="flex-1 p-4 space-y-2">
        {menuItems.map((item) => {
          const Icon = item.icon;

          return (
            <NavLink
              key={item.path}
              to={item.path}
              className={({ isActive }) =>
                `w-full flex items-center gap-3 px-4 py-3 rounded-xl transition ${
                  isActive
                    ? "bg-violet-600 text-white"
                    : "text-white hover:bg-slate-800"
                }`
              }
            >
              <Icon />
              {item.label}
            </NavLink>
          );
        })}
      </nav>

      <div className="p-4 border-t border-slate-700">
        <button
          type="button"
          onClick={handleLogout}
          className="w-full flex items-center gap-3 px-4 py-3 rounded-xl bg-red-500 hover:bg-red-600 transition"
        >
          <FaSignOutAlt />
          Cerrar Sesión
        </button>
      </div>
    </aside>
  );
}
import { useState } from "react";
import { NavLink, useNavigate } from "react-router-dom";
import {
  FaAngleLeft,
  FaBars,
  FaChartLine,
  FaUsers,
  FaUserShield,
  FaIdCard,
  FaCreditCard,
  FaUserCircle,
  FaSignOutAlt,
} from "react-icons/fa";

import { storage } from "../../utils/storage";

export default function AdminSidebar() {
  const navigate = useNavigate();
  const [collapsed, setCollapsed] = useState(false);

  const menuItems = [
    {
      label: "Dashboard",
      path: "/admin/dashboard",
      icon: FaChartLine,
    },
    {
      label: "Propietarios",
      path: "/admin/propietarios",
      icon: FaUsers,
    },
    {
      label: "Admins",
      path: "/admin/administradores",
      icon: FaUserShield,
    },
    {
      label: "Membresías",
      path: "/admin/membresias",
      icon: FaIdCard,
    },
    {
      label: "Tarjetas",
      path: "/admin/tarjetas",
      icon: FaCreditCard,
    },
    {
      label: "Perfil",
      path: "/admin/perfil",
      icon: FaUserCircle,
    },
  ];

  const handleLogout = () => {
    storage.clearAuth();
    navigate("/", { replace: true });
  };

  return (
    <aside
      className={`flex min-h-screen flex-col bg-slate-900 text-white transition-all duration-300 ${
        collapsed ? "w-20" : "w-72"
      }`}
    >
      {/* Header */}
      <div className={`border-b border-slate-700 ${collapsed ? "p-4" : "p-8"}`}>
        <div
          className={`flex items-center ${
            collapsed ? "justify-center" : "justify-between gap-4"
          }`}
        >
          {!collapsed && (
            <div>
              <h1 className="text-3xl font-bold">Elysia</h1>
              <p className="mt-2 text-slate-400">Panel Administrador</p>
            </div>
          )}

          <button
            type="button"
            onClick={() => setCollapsed((actual) => !actual)}
            className="flex h-10 w-10 shrink-0 items-center justify-center rounded-xl text-slate-300 transition hover:bg-slate-800 hover:text-white"
            title={collapsed ? "Expandir menú" : "Reducir menú"}
          >
            {collapsed ? <FaBars /> : <FaAngleLeft />}
          </button>
        </div>
      </div>

      {/* Menú */}
      <nav className="flex-1 space-y-2 p-4">
        {menuItems.map((item) => {
          const Icon = item.icon;

          return (
            <NavLink
              key={item.path}
              to={item.path}
              title={collapsed ? item.label : undefined}
              className={({ isActive }) =>
                `flex w-full items-center rounded-xl transition ${
                  collapsed
                    ? "justify-center px-0 py-3"
                    : "gap-3 px-4 py-3"
                } ${
                  isActive
                    ? "bg-violet-600 text-white"
                    : "text-white hover:bg-slate-800"
                }`
              }
            >
              <Icon className="shrink-0" />
              {!collapsed && <span>{item.label}</span>}
            </NavLink>
          );
        })}
      </nav>

      {/* Logout */}
      <div className="border-t border-slate-700 p-4">
        <button
          type="button"
          onClick={handleLogout}
          title={collapsed ? "Cerrar sesión" : undefined}
          className={`flex w-full items-center rounded-xl bg-red-500 transition hover:bg-red-600 ${
            collapsed ? "justify-center px-0 py-3" : "gap-3 px-4 py-3"
          }`}
        >
          <FaSignOutAlt className="shrink-0" />
          {!collapsed && <span>Cerrar Sesión</span>}
        </button>
      </div>
    </aside>
  );
}
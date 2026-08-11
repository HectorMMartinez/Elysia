import { useState, useEffect } from "react";
import { NavLink, useNavigate } from "react-router-dom";
import {
  FaAngleLeft,
  FaBars,
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
  FaChartBar,
  FaLock,
} from "react-icons/fa";

import { storage } from "../../utils/storage";
import dashboardService from "../../services/dashboardPropietarioService";

export default function OwnerSidebar({ children }) {
  const navigate = useNavigate();
  const [collapsed, setCollapsed] = useState(false);
  const [planId, setPlanId] = useState(null);

  useEffect(() => {
    const cargarPlan = async () => {
      // Primero intentamos leer del storage
      const auth = storage.getAuth();
      if (auth?.planId) {
        setPlanId(auth.planId);
        return;
      }

      // Si no está, lo pedimos al backend
      const res = await dashboardService.getPanel();
      if (res.success) {
        setPlanId(res.data.planId);

        if (auth) {
          storage.saveAuth({
            ...auth,
            planId: res.data.planId,
          });
        }
      }
    };

    cargarPlan();
  }, []);

  const esPremium = planId === 2;

  const menuItems = [
    {
      label: "Dashboard",
      path: "/dashboard",
      icon: FaChartLine,
      premiumOnly: false,
    },
    {
      label: "Inventario",
      path: "/inventario",
      icon: FaBoxOpen,
      premiumOnly: false,
    },
    {
      label: "Platos",
      path: "/platos",
      icon: FaUtensils,
      premiumOnly: false,
    },
    {
      label: "Menu",
      path: "/menu",
      icon: FaClipboardList,
      premiumOnly: false,
    },
    {
      label: "Mesas",
      path: "/mesas",
      icon: FaChair,
      premiumOnly: false,
    },
    {
      label: "Reservas",
      path: "/reservas",
      icon: FaCalendarCheck,
      premiumOnly: false,
    },
    {
      label: "Pedidos",
      path: "/pedidos",
      icon: FaDollarSign,
      premiumOnly: false,
    },
    {
      label: "Turnos",
      path: "/turnos",
      icon: FaClock,
      premiumOnly: true,
    },
    {
      label: "Empleados",
      path: "/empleados",
      icon: FaUsers,
      premiumOnly: true,
    },
    {
      label: "Central de Inteligencia",
      path: "/central-inteligencia",
      icon: FaChartBar,
      premiumOnly: true,
    },
    {
      label: "Perfil",
      path: "/perfil",
      icon: FaUserCircle,
      premiumOnly: false,
    },
  ];

  const handleLogout = () => {
    storage.clearAuth();
    navigate("/", { replace: true });
  };

  const menuVisible = menuItems.filter((item) => {
    if (item.premiumOnly && !esPremium) return false;
    return true;
  });

  return (
    <div className="flex h-screen w-full overflow-hidden bg-slate-50">
      {/* Tu menú lateral original */}
      <aside
        className={`flex h-full shrink-0 overflow-y-auto flex-col bg-slate-900 text-white transition-all duration-300 ${
          collapsed ? "w-20" : "w-72"
        }`}
      >
        <div className={`border-b border-slate-700 ${collapsed ? "p-4" : "p-8"}`}>
          <div
            className={`flex items-center ${
              collapsed ? "justify-center" : "justify-between gap-4"
            }`}
          >
            {!collapsed && (
              <div>
                <h1 className="text-3xl font-bold">Elysia</h1>
                <p className="mt-2 text-slate-400">Restaurant Management</p>
              </div>
            )}
            <button
              type="button"
              onClick={() => setCollapsed((actual) => !actual)}
              className="flex h-10 w-10 shrink-0 items-center justify-center rounded-xl text-slate-300 transition hover:bg-slate-800 hover:text-white"
              aria-label={collapsed ? "Expandir menu" : "Reducir menu"}
              title={collapsed ? "Expandir menu" : "Reducir menu"}
            >
              {collapsed ? <FaBars /> : <FaAngleLeft />}
            </button>
          </div>
        </div>

        <nav className="flex-1 space-y-2 p-4">
          {menuVisible.map((item) => {
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

          {!esPremium && !collapsed && (
            <div className="mt-6 rounded-xl bg-amber-500/10 border border-amber-500/30 p-4">
              <div className="flex items-center gap-2 text-amber-400 text-sm font-medium mb-1">
                <FaLock size={14} />
                <span>Plan Simple</span>
              </div>
              <p className="text-xs text-slate-400">
                Mejora a Premium para acceder a Turnos, Empleados y Central de
                Inteligencia.
              </p>
            </div>
          )}
        </nav>

        <div className="border-t border-slate-700 p-4">
          <button
            type="button"
            onClick={handleLogout}
            title={collapsed ? "Cerrar sesion" : undefined}
            className={`flex w-full items-center rounded-xl bg-red-500 transition hover:bg-red-600 ${
              collapsed ? "justify-center px-0 py-3" : "gap-3 px-4 py-3"
            }`}
          >
            <FaSignOutAlt className="shrink-0" />
            {!collapsed && <span>Cerrar Sesion</span>}
          </button>
        </div>
      </aside>

      {/* Área donde se renderiza la vista activa (Mesas, Inventario, Platos, etc.) */}
      <main className="flex-1 min-w-0 h-full overflow-y-auto">
        {children}
      </main>
    </div>
  );
}
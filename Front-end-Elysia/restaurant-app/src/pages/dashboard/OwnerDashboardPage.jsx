import { useEffect, useState } from "react";
import {
  FaCalendarCheck,
  FaChair,
  FaBoxOpen,
  FaUtensils,
  FaUsers,
  FaClipboardList,
  FaUserCheck,
  FaUserTimes,
  FaClock,
  FaCheckCircle,
  FaHourglassHalf,
} from "react-icons/fa";

import OwnerSidebar from "../../components/layout/OwnerSidebar";
import dashboardService from "../../services/dashboardPropietarioService";
import { useAuth } from "../../context/AuthContext";
import { storage } from "../../utils/storage";

export default function OwnerDashboardPage() {
  const { auth } = useAuth();
  const [panel, setPanel] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [showModal, setShowModal] = useState(false);
  const [cambiandoPlan, setCambiandoPlan] = useState(false);

  const cargarPanel = async () => {
    setLoading(true);
    setError(null);

    const res = await dashboardService.getPanel();

    if (res.success) {
      setPanel(res.data);

      // Guardamos el planId actual en el storage
      const authActual = storage.getAuth();
      if (authActual) {
        storage.saveAuth({
          ...authActual,
          planId: res.data.planId,
        });
      }
    } else {
      setError(res.message || "Error al cargar el panel");
    }

    setLoading(false);
  };

  useEffect(() => {
    cargarPanel();
  }, []);

  const handleCambiarPlan = async () => {
    const usuarioId =
      auth?.usuarioId ||
      auth?.UsuarioId ||
      auth?.id ||
      auth?.userId;

    if (!usuarioId) {
      alert("No se pudo obtener el ID del usuario");
      return;
    }

    setCambiandoPlan(true);

    const res = await dashboardService.cambiarPlan(usuarioId);

    if (res.success) {
      // Actualizamos el plan en el storage
      const authActual = storage.getAuth();
      if (authActual) {
        storage.saveAuth({
          ...authActual,
          planId: 2,
        });
      }

      setShowModal(false);

      // Recargamos para que el Sidebar muestre las nuevas opciones
      window.location.reload();
    } else {
      alert(res.message || "No se pudo cambiar el plan");
      setCambiandoPlan(false);
    }
  };

  if (loading) {
    return (
      <div className="min-h-screen bg-slate-100 flex">
        <OwnerSidebar />
        <main className="flex-1 p-8 flex items-center justify-center">
          <div className="text-center">
            <div className="w-12 h-12 border-4 border-violet-600 border-t-transparent rounded-full animate-spin mx-auto"></div>
            <p className="mt-4 text-slate-600">Cargando panel...</p>
          </div>
        </main>
      </div>
    );
  }

  if (error) {
    return (
      <div className="min-h-screen bg-slate-100 flex">
        <OwnerSidebar />
        <main className="flex-1 p-8 flex items-center justify-center">
          <div className="bg-white p-8 rounded-2xl shadow-sm text-center max-w-md">
            <p className="text-red-600 font-medium">{error}</p>
            <button
              onClick={cargarPanel}
              className="mt-4 bg-violet-600 text-white px-6 py-2 rounded-lg hover:bg-violet-700"
            >
              Reintentar
            </button>
          </div>
        </main>
      </div>
    );
  }

  if (!panel) return null;

  const esPremium = panel.planId === 2;

  return (
    <div className="min-h-screen bg-slate-100 flex">
      <OwnerSidebar />

      <main className="flex-1 p-8">
        {/* HEADER */}
        <div className="mb-8 flex items-center justify-between flex-wrap gap-4">
          <div className="flex items-center gap-4">
            <img
              src={panel.image}
              alt="Foto de perfil"
              className="w-16 h-16 rounded-full object-cover border-4 border-white shadow"
              onError={(e) => {
                e.target.src =
                  "https://cdn-icons-png.flaticon.com/512/149/149071.png";
              }}
            />
            <div>
              <h2 className="text-3xl font-bold text-slate-800">
                Hola, {panel.namePropietario}
              </h2>
              <p className="text-slate-500 mt-1">
                Bienvenido a Elysia Restaurant Management
              </p>
            </div>
          </div>

          <span
            className={`px-4 py-2 rounded-full text-sm font-semibold ${
              esPremium
                ? "bg-violet-100 text-violet-700"
                : "bg-amber-100 text-amber-700"
            }`}
          >
            {esPremium ? "Plan Premium" : "Plan Simple"}
          </span>
        </div>

        {/* CARDS PRINCIPALES */}
        <div className="grid md:grid-cols-2 xl:grid-cols-4 gap-6 mb-8">
          <Card
            title="Total Pedidos"
            value={panel.totalPedidos}
            icon={<FaClipboardList size={36} className="text-violet-500" />}
          />
          <Card
            title="Total Reservas"
            value={panel.totalReservas}
            icon={<FaCalendarCheck size={36} className="text-amber-500" />}
          />
          <Card
            title="Productos"
            value={panel.cantidadProducto}
            icon={<FaBoxOpen size={36} className="text-blue-500" />}
          />
          <Card
            title="Platos"
            value={panel.cantidadPlato}
            icon={<FaUtensils size={36} className="text-orange-500" />}
          />
        </div>

        {/* INDICADORES DE MESAS */}
        <div className="grid md:grid-cols-3 gap-6 mb-8">
          <Card
            title="Mesas Disponibles"
            value={panel.mesasDisponibles ?? 0}
            icon={<FaCheckCircle size={36} className="text-green-500" />}
          />
          <Card
            title="Mesas Ocupadas"
            value={panel.mesasOcupadas ?? 0}
            icon={<FaHourglassHalf size={36} className="text-amber-500" />}
          />
          <Card
            title="Mesas Reservadas"
            value={panel.mesasReservadas ?? 0}
            icon={<FaCalendarCheck size={36} className="text-blue-500" />}
          />
        </div>

        {/* SEGUNDA FILA */}
        <div className="grid md:grid-cols-2 xl:grid-cols-4 gap-6 mb-8">
          <Card
            title="Menús"
            value={panel.cantidadMenu}
            icon={<FaUtensils size={36} className="text-rose-500" />}
          />
          <Card
            title="Platos en Menú"
            value={panel.platoAsociadoAUnMenu}
            icon={<FaClipboardList size={36} className="text-indigo-500" />}
          />
          <Card
            title="Total Mesas"
            value={panel.cantidadMesa}
            icon={<FaChair size={36} className="text-green-600" />}
          />
          <Card
            title="Pedidos Pendientes"
            value={panel.pedidoPendiente}
            icon={<FaClock size={36} className="text-yellow-500" />}
          />
        </div>

        {/* SOLO PREMIUM */}
        {esPremium && (
          <div className="grid md:grid-cols-2 xl:grid-cols-4 gap-6 mb-8">
            <Card
              title="Empleados Activos"
              value={panel.empleadoActivos}
              icon={<FaUserCheck size={36} className="text-green-600" />}
            />
            <Card
              title="Empleados Inactivos"
              value={panel.empleadoNoActivos}
              icon={<FaUserTimes size={36} className="text-red-500" />}
            />
            <Card
              title="Total Empleados"
              value={panel.totalEmpleados}
              icon={<FaUsers size={36} className="text-blue-600" />}
            />
            <Card
              title="Turnos"
              value={panel.totalTurno}
              icon={<FaClock size={36} className="text-purple-500" />}
            />
          </div>
        )}

        {/* DETALLE + PLAN */}
        <div className="grid lg:grid-cols-3 gap-6">
          {/* Pedidos */}
          <div className="bg-white rounded-2xl shadow-sm p-6">
            <h3 className="text-xl font-bold mb-5">Estado de Pedidos</h3>
            <div className="space-y-3">
              <Item label="Pendientes" value={panel.pedidoPendiente} color="bg-yellow-400" />
              <Item label="En Proceso" value={panel.pedidosEnProceso} color="bg-blue-400" />
              <Item label="Listos" value={panel.pedidosListo} color="bg-indigo-400" />
              <Item label="Entregados" value={panel.pedidosEntregado} color="bg-green-400" />
              <Item label="Finalizados" value={panel.pedidosFinalizado} color="bg-emerald-500" />
              <Item label="Cancelados" value={panel.pedidosCancelados} color="bg-red-400" />
            </div>
          </div>

          {/* Reservas */}
          <div className="bg-white rounded-2xl shadow-sm p-6">
            <h3 className="text-xl font-bold mb-5">Estado de Reservas</h3>
            <div className="space-y-3">
              <Item label="Activas" value={panel.reservasActivas} color="bg-green-400" />
              <Item label="En Proceso" value={panel.reservaEnProceso} color="bg-blue-400" />
              <Item label="Finalizadas" value={panel.reservasFinalizadas} color="bg-emerald-500" />
              <Item label="Canceladas" value={panel.reservasCanceladas} color="bg-red-400" />
              <Item label="No Asistió" value={panel.reservasNoAsistio} color="bg-orange-400" />
            </div>
          </div>

          {/* Tarjeta del Plan */}
          <div
            className={`rounded-2xl text-white p-6 shadow-lg flex flex-col ${
              esPremium
                ? "bg-gradient-to-br from-violet-600 to-indigo-700"
                : "bg-gradient-to-br from-amber-500 to-orange-600"
            }`}
          >
            <h3 className="text-2xl font-bold">
              {esPremium ? "Plan Premium" : "Plan Simple"}
            </h3>

            {esPremium ? (
              <>
                <p className="mt-3 opacity-90 text-sm leading-relaxed">
                  Tu membresía Premium está activa. Tienes acceso completo a
                  todas las funcionalidades avanzadas.
                </p>
                <div className="mt-auto pt-8">
                  <p className="opacity-80 text-sm">Estado</p>
                  <span className="inline-block mt-2 bg-green-500 px-4 py-2 rounded-full text-sm font-medium">
                    Activo
                  </span>
                </div>
              </>
            ) : (
              <>
                <p className="mt-3 opacity-95 text-sm leading-relaxed">
                  Diseñado para restaurantes que requieren una gestión estratégica
                  y basada en datos. Incluye todas las funcionalidades del Plan
                  Simple, además de reportes operativos avanzados, análisis de
                  ventas, estadísticas de inventario, indicadores de desempeño,gestion de empleados,
                  gestión de turnos, centrar de inteligencia,
                  alertas inteligentes y recomendaciones automáticas.
                </p>

                <div className="mt-4">
                  <p className="text-3xl font-bold">
                    RD$ 3,500
                    <span className="text-sm font-normal opacity-80"> /mes</span>
                  </p>
                </div>

                <button
                  onClick={() => setShowModal(true)}
                  className="mt-6 w-full bg-white text-orange-600 font-bold py-3 rounded-xl hover:bg-orange-50 transition"
                >
                  Mejorar a Premium
                </button>
              </>
            )}
          </div>
        </div>
      </main>

      {/* MODAL */}
      {showModal && (
        <div className="fixed inset-0 bg-black/50 flex items-center justify-center z-50 p-4">
          <div className="bg-white rounded-2xl p-8 max-w-md w-full shadow-2xl">
            <h3 className="text-2xl font-bold text-slate-800 mb-3">
              ¿Mejorar a Plan Premium?
            </h3>
            <p className="text-slate-600 mb-2">
              Al confirmar, tu cuenta pasará al Plan Premium (RD$ 3,500 / mes).
            </p>
            <p className="text-slate-500 text-sm mb-6">
              Tendrás acceso a Turnos, Empleados, Central de Inteligencia y todos
              los reportes avanzados.
            </p>

            <div className="flex gap-3">
              <button
                onClick={() => setShowModal(false)}
                disabled={cambiandoPlan}
                className="flex-1 border border-slate-300 text-slate-700 py-3 rounded-xl font-medium hover:bg-slate-50"
              >
                Cancelar
              </button>
              <button
                onClick={handleCambiarPlan}
                disabled={cambiandoPlan}
                className="flex-1 bg-violet-600 text-white py-3 rounded-xl font-medium hover:bg-violet-700 disabled:opacity-60"
              >
                {cambiandoPlan ? "Cambiando..." : "Confirmar"}
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}

function Card({ title, value, icon }) {
  return (
    <div className="bg-white rounded-2xl p-6 shadow-sm">
      <div className="flex justify-between items-center">
        <div>
          <p className="text-slate-500 text-sm">{title}</p>
          <h3 className="text-3xl font-bold mt-2 text-slate-800">
            {value ?? 0}
          </h3>
        </div>
        {icon}
      </div>
    </div>
  );
}

function Item({ label, value, color }) {
  return (
    <div className="flex items-center justify-between">
      <div className="flex items-center gap-3">
        <div className={`w-3 h-3 rounded-full ${color}`}></div>
        <span className="text-slate-600">{label}</span>
      </div>
      <span className="font-semibold text-slate-800">{value ?? 0}</span>
    </div>
  );
}
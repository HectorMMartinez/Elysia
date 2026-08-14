import { useEffect, useState } from "react";
import {
  FaUsers,
  FaUserShield,
  FaIdCard,
  FaCreditCard,
  FaChartLine,
  FaCheckCircle,
  FaTimesCircle,
  FaExclamationTriangle,
  FaClock,
} from "react-icons/fa";

import AdminSidebar from "../../components/layout/AdminSidebar";
import dashboardAdminService from "../../services/dashboardAdminService";

export default function AdminDashboardPage() {
  const [panel, setPanel] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const API_URL = "https://localhost:7108/";

  const cargarPanel = async () => {
    setLoading(true);
    setError(null);

    const res = await dashboardAdminService.getPanel();

    if (res.success) {
      setPanel(res.data);
    } else {
      setError(res.message || "Error al cargar el panel");
    }

    setLoading(false);
  };

  useEffect(() => {
    cargarPanel();
  }, []);

  if (loading) {
    return (
      <div className="min-h-screen bg-slate-100 flex">
        <AdminSidebar />
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
        <AdminSidebar />
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

  return (
    <div className="min-h-screen bg-slate-100 flex">
      <AdminSidebar />

      <main className="flex-1 p-8">
        {/* HEADER */}
        <div className="mb-8 flex items-center justify-between flex-wrap gap-4">
          <div className="flex items-center gap-4">
            <img
              src={API_URL + panel.image}
              alt="Foto de perfil"
              className="w-16 h-16 rounded-full object-cover border-4 border-white shadow"
              onError={(e) => {
                e.target.src =
                  "https://cdn-icons-png.flaticon.com/512/149/149071.png";
              }}
            />
            <div>
              <h2 className="text-3xl font-bold text-slate-800">
                Hola, {panel.name}
              </h2>
              <p className="text-slate-500 mt-1">
                Panel de Administración - Elysia
              </p>
            </div>
          </div>

          <span className="px-4 py-2 rounded-full text-sm font-semibold bg-violet-100 text-violet-700">
            Administrador
          </span>
        </div>

        {/* CARDS PRINCIPALES */}
        <div className="grid md:grid-cols-2 xl:grid-cols-4 gap-6 mb-8">
          <Card
            title="Propietarios"
            value={panel.cantidadPropietario}
            icon={<FaUsers size={36} className="text-violet-500" />}
          />
          <Card
            title="Administradores"
            value={panel.cantidadAdmin}
            icon={<FaUserShield size={36} className="text-blue-500" />}
          />
          <Card
            title="Planes"
            value={panel.cantidadPlanes}
            icon={<FaIdCard size={36} className="text-amber-500" />}
          />
          <Card
            title="Total Tarjetas"
            value={panel.cantidadTotalTarjeta}
            icon={<FaCreditCard size={36} className="text-green-500" />}
          />
        </div>

        {/* TARJETAS POR TIPO */}
        <div className="grid md:grid-cols-3 gap-6 mb-8">
          <Card
            title="Visa"
            value={panel.cantidadTarjetaVisa}
            icon={<FaCreditCard size={36} className="text-blue-600" />}
          />
          <Card
            title="Mastercard"
            value={panel.cantidadTarjetaMastercard}
            icon={<FaCreditCard size={36} className="text-orange-500" />}
          />
          <Card
            title="American Express"
            value={panel.cantidadTarjetaAmericanExpress}
            icon={<FaCreditCard size={36} className="text-emerald-600" />}
          />
        </div>

        {/* MEMBRESÍAS + RESUMEN */}
        <div className="grid lg:grid-cols-3 gap-6">
          {/* Estado de Membresías */}
          <div className="bg-white rounded-2xl shadow-sm p-6 lg:col-span-2">
            <h3 className="text-xl font-bold mb-5">Estado de Membresías</h3>

            <div className="grid sm:grid-cols-2 gap-4">
              <Item
                label="Activas"
                value={panel.membresiasActivas}
                color="bg-green-400"
                icon={<FaCheckCircle className="text-green-500" />}
              />
              <Item
                label="Suspendidas"
                value={panel.membresiaSuspendida}
                color="bg-amber-400"
                icon={<FaExclamationTriangle className="text-amber-500" />}
              />
              <Item
                label="Vencidas"
                value={panel.membresiaVencida}
                color="bg-orange-400"
                icon={<FaClock className="text-orange-500" />}
              />
              <Item
                label="Canceladas"
                value={panel.membresiasCanceladas}
                color="bg-red-400"
                icon={<FaTimesCircle className="text-red-500" />}
              />
            </div>

            <div className="mt-6 pt-5 border-t border-slate-100 flex justify-between items-center">
              <span className="text-slate-600 font-medium">Total Membresías</span>
              <span className="text-2xl font-bold text-slate-800">
                {panel.totalMembresias}
              </span>
            </div>
          </div>

          {/* Card lateral */}
          <div className="bg-gradient-to-br from-violet-600 to-indigo-700 rounded-2xl text-white p-6 shadow-lg flex flex-col">
            <div className="flex items-center gap-3 mb-4">
              <FaChartLine size={28} />
              <h3 className="text-2xl font-bold">Sistema Elysia</h3>
            </div>

            <p className="text-violet-100 text-sm leading-relaxed">
              Panel de control central. Desde aquí puedes gestionar propietarios,
              administradores, membresías y tarjetas del sistema.
            </p>

            <div className="mt-auto pt-8">
              <p className="text-violet-200 text-sm">Estado del sistema</p>
              <span className="inline-block mt-2 bg-green-500 px-4 py-2 rounded-full text-sm font-medium">
                Operativo
              </span>
            </div>
          </div>
        </div>
      </main>
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

function Item({ label, value, color, icon }) {
  return (
    <div className="flex items-center justify-between p-4 rounded-xl bg-slate-50">
      <div className="flex items-center gap-3">
        {icon}
        <span className="text-slate-700 font-medium">{label}</span>
      </div>
      <span className="text-xl font-bold text-slate-800">{value ?? 0}</span>
    </div>
  );
}
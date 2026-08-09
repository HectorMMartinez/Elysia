import { useEffect, useMemo, useState } from "react";
import {
  FaCreditCard,
  FaSearch,
  FaFilter,
  FaSyncAlt,
  FaExclamationTriangle,
  FaCheckCircle,
  FaPauseCircle,
  FaTimesCircle,
  FaUsers,
  FaEye,
} from "react-icons/fa";

import AdminSidebar from "../../components/layout/AdminSidebar";
import MembresiaActionModal from "../../components/admin/MembresiaActionModal";
import membresiaService from "../../services/membresiaService";

// Helper para badges de estado
const getStatusConfig = (estado) => {
  const value = String(estado ?? "").toLowerCase();

  if (value.includes("activ")) {
    return {
      label: estado,
      bg: "bg-emerald-50",
      text: "text-emerald-700",
      border: "border-emerald-200",
      icon: <FaCheckCircle className="text-emerald-500" />,
    };
  }
  if (value.includes("suspend")) {
    return {
      label: estado,
      bg: "bg-amber-50",
      text: "text-amber-700",
      border: "border-amber-200",
      icon: <FaPauseCircle className="text-amber-500" />,
    };
  }
  if (value.includes("cancel") || value.includes("inactiv")) {
    return {
      label: estado,
      bg: "bg-rose-50",
      text: "text-rose-700",
      border: "border-rose-200",
      icon: <FaTimesCircle className="text-rose-500" />,
    };
  }
  return {
    label: estado || "—",
    bg: "bg-slate-100",
    text: "text-slate-700",
    border: "border-slate-200",
    icon: null,
  };
};

// Formateo seguro de fechas
const formatDate = (date) => {
  if (!date) return "—";
  const d = new Date(date);
  return isNaN(d.getTime())
    ? "—"
    : d.toLocaleDateString("es-ES", {
        day: "2-digit",
        month: "short",
        year: "numeric",
      });
};

export default function MembresiasPage() {
  const [membresias, setMembresias] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  const [search, setSearch] = useState("");
  const [statusFilter, setStatusFilter] = useState("todos");

  const [selectedMembresia, setSelectedMembresia] = useState(null);
  const [action, setAction] = useState(null);
  const [actionLoading, setActionLoading] = useState(false);

  // ============================================================
  // CARGAR MEMBRESÍAS
  // ============================================================
  const cargarMembresias = async () => {
    try {
      setLoading(true);
      setError(null);

      const data = await membresiaService.getAll();
      setMembresias(Array.isArray(data) ? data : []);
    } catch (err) {
      console.error("Error al cargar membresías:", err);
      const message =
        err.response?.data ||
        err.message ||
        "No fue posible cargar las membresías.";
      setError(typeof message === "string" ? message : "No fue posible cargar las membresías.");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    cargarMembresias();
  }, []);

  // ============================================================
  // FILTRADO
  // ============================================================
  const filteredMembresias = useMemo(() => {
    const searchText = search.toLowerCase().trim();

    return membresias.filter((m) => {
      const username = (m.userName || "").toLowerCase();
      const restaurante = (m.nombreRestaurante || "").toLowerCase();
      const plan = (m.nombrePlan || "").toLowerCase();

      const matchesSearch =
        !searchText ||
        username.includes(searchText) ||
        restaurante.includes(searchText) ||
        plan.includes(searchText);

      const estado = String(m.estado ?? "").toLowerCase();
      const matchesStatus =
        statusFilter === "todos" || estado === statusFilter.toLowerCase();

      return matchesSearch && matchesStatus;
    });
  }, [membresias, search, statusFilter]);

  // ============================================================
  // ESTADÍSTICAS
  // ============================================================
  const stats = useMemo(() => {
    const total = membresias.length;
    let activas = 0;
    let suspendidas = 0;
    let canceladas = 0;

    membresias.forEach((m) => {
      const e = String(m.estado ?? "").toLowerCase();
      if (e.includes("activ")) activas++;
      else if (e.includes("suspend")) suspendidas++;
      else if (e.includes("cancel") || e.includes("inactiv")) canceladas++;
    });

    return { total, activas, suspendidas, canceladas };
  }, [membresias]);

  // ============================================================
  // ACCIONES
  // ============================================================
  const abrirModalAccion = (membresia, tipoAccion) => {
    setSelectedMembresia(membresia);
    setAction(tipoAccion);
  };

  const cerrarModalAccion = () => {
    if (actionLoading) return;
    setSelectedMembresia(null);
    setAction(null);
  };

  const ejecutarAccion = async () => {
    if (!selectedMembresia || !action) return;

    try {
      setActionLoading(true);
      setError(null);

      if (action === "activar") {
        await membresiaService.activarPorUnMes(selectedMembresia.id);
      } else if (action === "suspender") {
        await membresiaService.suspender(selectedMembresia.id);
      } else if (action === "cancelar") {
        await membresiaService.cancelar(selectedMembresia.id);
      }

      setSelectedMembresia(null);
      setAction(null);
      await cargarMembresias();
    } catch (err) {
      console.error("Error al cambiar estado de membresía:", err);
      const message =
        err.response?.data ||
        err.message ||
        "No fue posible cambiar el estado de la membresía.";
      setError(typeof message === "string" ? message : "No fue posible cambiar el estado de la membresía.");
    } finally {
      setActionLoading(false);
    }
  };

  // ============================================================
  // RENDER
  // ============================================================
  return (
    <div className="min-h-screen bg-slate-50 flex">
      <AdminSidebar />

      <main className="flex-1 p-6 lg:p-8">
        {/* HEADER */}
        <div className="mb-8 flex flex-col sm:flex-row sm:items-center justify-between gap-4">
          <div className="flex items-center gap-4">
            <div className="w-12 h-12 rounded-2xl bg-violet-100 flex items-center justify-center shadow-sm">
              <FaCreditCard className="text-violet-600 text-xl" />
            </div>
            <div>
              <h1 className="text-2xl lg:text-3xl font-bold text-slate-800 tracking-tight">
                Membresías
              </h1>
              <p className="text-slate-500 text-sm mt-0.5">
                Gestiona las membresías de los propietarios de Elysia
              </p>
            </div>
          </div>

          <button
            type="button"
            onClick={cargarMembresias}
            disabled={loading || actionLoading}
            className="inline-flex items-center gap-2 px-4 py-2.5 bg-white border border-slate-200 rounded-xl text-slate-600 font-medium text-sm hover:bg-slate-50 hover:border-slate-300 transition-all disabled:opacity-50 disabled:cursor-not-allowed shadow-sm"
          >
            <FaSyncAlt className={loading ? "animate-spin" : ""} />
            Actualizar
          </button>
        </div>

        {/* ERROR */}
        {error && (
          <div className="mb-6 bg-red-50 border border-red-200 rounded-2xl p-4 flex items-start gap-3 animate-in fade-in">
            <FaExclamationTriangle className="text-red-500 mt-0.5 shrink-0" />
            <div>
              <p className="font-semibold text-red-700">Ocurrió un problema</p>
              <p className="text-sm text-red-600 mt-1">{error}</p>
            </div>
          </div>
        )}

        {/* ESTADÍSTICAS */}
        <div className="grid grid-cols-2 lg:grid-cols-4 gap-4 mb-6">
          <div className="bg-white rounded-2xl border border-slate-100 shadow-sm p-5 hover:shadow-md transition-shadow">
            <div className="flex items-center justify-between">
              <p className="text-sm font-medium text-slate-500">Total</p>
              <FaUsers className="text-slate-300" />
            </div>
            <p className="text-3xl font-bold text-slate-800 mt-2">{stats.total}</p>
          </div>

          <div className="bg-white rounded-2xl border border-slate-100 shadow-sm p-5 hover:shadow-md transition-shadow">
            <div className="flex items-center justify-between">
              <p className="text-sm font-medium text-slate-500">Activas</p>
              <FaCheckCircle className="text-emerald-400" />
            </div>
            <p className="text-3xl font-bold text-emerald-600 mt-2">{stats.activas}</p>
          </div>

          <div className="bg-white rounded-2xl border border-slate-100 shadow-sm p-5 hover:shadow-md transition-shadow">
            <div className="flex items-center justify-between">
              <p className="text-sm font-medium text-slate-500">Suspendidas</p>
              <FaPauseCircle className="text-amber-400" />
            </div>
            <p className="text-3xl font-bold text-amber-600 mt-2">{stats.suspendidas}</p>
          </div>

          <div className="bg-white rounded-2xl border border-slate-100 shadow-sm p-5 hover:shadow-md transition-shadow">
            <div className="flex items-center justify-between">
              <p className="text-sm font-medium text-slate-500">Canceladas</p>
              <FaTimesCircle className="text-rose-400" />
            </div>
            <p className="text-3xl font-bold text-rose-600 mt-2">{stats.canceladas}</p>
          </div>
        </div>

        {/* FILTROS */}
        <div className="bg-white rounded-2xl border border-slate-100 shadow-sm p-5 mb-6">
          <div className="flex flex-col lg:flex-row gap-4">
            <div className="relative flex-1">
              <FaSearch className="absolute left-4 top-1/2 -translate-y-1/2 text-slate-400" />
              <input
                type="text"
                value={search}
                onChange={(e) => setSearch(e.target.value)}
                placeholder="Buscar por usuario, restaurante o plan..."
                className="w-full pl-11 pr-4 py-3 border border-slate-200 rounded-xl outline-none focus:ring-2 focus:ring-violet-500/30 focus:border-violet-500 text-slate-700 placeholder:text-slate-400 transition"
              />
            </div>

            <div className="relative lg:w-56">
              <FaFilter className="absolute left-4 top-1/2 -translate-y-1/2 text-slate-400 pointer-events-none" />
              <select
                value={statusFilter}
                onChange={(e) => setStatusFilter(e.target.value)}
                className="w-full appearance-none pl-11 pr-10 py-3 border border-slate-200 rounded-xl outline-none focus:ring-2 focus:ring-violet-500/30 focus:border-violet-500 text-slate-700 bg-white cursor-pointer"
              >
                <option value="todos">Todos los estados</option>
                {[...new Set(membresias.map((m) => String(m.estado ?? "")))]
                  .filter(Boolean)
                  .map((estado) => (
                    <option key={estado} value={estado}>
                      {estado}
                    </option>
                  ))}
              </select>
            </div>
          </div>

          <div className="mt-4 flex items-center gap-2 text-sm text-slate-500">
            <FaEye className="text-slate-400" />
            <span>
              Mostrando{" "}
              <span className="font-semibold text-slate-700">
                {filteredMembresias.length}
              </span>{" "}
              de{" "}
              <span className="font-semibold text-slate-700">{stats.total}</span>{" "}
              membresías
            </span>
          </div>
        </div>

        {/* TABLA */}
        <div className="bg-white rounded-2xl border border-slate-100 shadow-sm overflow-hidden">
          <div className="overflow-x-auto">
            <table className="w-full text-left">
              <thead className="bg-slate-50/80 border-b border-slate-200">
                <tr>
                  <th className="px-6 py-4 text-xs font-semibold text-slate-500 uppercase tracking-wider">
                    Propietario
                  </th>
                  <th className="px-6 py-4 text-xs font-semibold text-slate-500 uppercase tracking-wider">
                    Restaurante
                  </th>
                  <th className="px-6 py-4 text-xs font-semibold text-slate-500 uppercase tracking-wider">
                    Plan
                  </th>
                  <th className="px-6 py-4 text-xs font-semibold text-slate-500 uppercase tracking-wider">
                    Inicio
                  </th>
                  <th className="px-6 py-4 text-xs font-semibold text-slate-500 uppercase tracking-wider">
                    Fin
                  </th>
                  <th className="px-6 py-4 text-xs font-semibold text-slate-500 uppercase tracking-wider">
                    Estado
                  </th>
                  <th className="px-6 py-4 text-xs font-semibold text-slate-500 uppercase tracking-wider text-right">
                    Acciones
                  </th>
                </tr>
              </thead>

              <tbody className="divide-y divide-slate-100">
                {loading ? (
                  // Skeleton loading
                  Array.from({ length: 5 }).map((_, i) => (
                    <tr key={i}>
                      {Array.from({ length: 7 }).map((_, j) => (
                        <td key={j} className="px-6 py-5">
                          <div className="h-4 bg-slate-100 rounded animate-pulse w-3/4" />
                        </td>
                      ))}
                    </tr>
                  ))
                ) : filteredMembresias.length === 0 ? (
                  <tr>
                    <td colSpan={7} className="px-6 py-16 text-center">
                      <div className="flex flex-col items-center gap-3">
                        <div className="w-14 h-14 rounded-full bg-slate-100 flex items-center justify-center">
                          <FaCreditCard className="text-slate-400 text-xl" />
                        </div>
                        <p className="text-slate-500 font-medium">
                          No se encontraron membresías
                        </p>
                        <p className="text-sm text-slate-400">
                          Prueba cambiando los filtros o el término de búsqueda
                        </p>
                      </div>
                    </td>
                  </tr>
                ) : (
                  filteredMembresias.map((membresia) => {
                    const status = getStatusConfig(membresia.estado);
                    const estadoLower = String(membresia.estado ?? "").toLowerCase();
                    const isActive = estadoLower.includes("activ");
                    const isSuspended = estadoLower.includes("suspend");
                    const isCancelled =
                      estadoLower.includes("cancel") ||
                      estadoLower.includes("inactiv");

                    return (
                      <tr
                        key={membresia.id}
                        className="hover:bg-slate-50/70 transition-colors"
                      >
                        <td className="px-6 py-4">
                          <p className="font-medium text-slate-800">
                            {membresia.userName || "—"}
                          </p>
                        </td>

                        <td className="px-6 py-4 text-slate-600">
                          {membresia.nombreRestaurante || "—"}
                        </td>

                        <td className="px-6 py-4">
                          <span className="inline-flex items-center px-2.5 py-1 rounded-lg bg-violet-50 text-violet-700 text-sm font-medium">
                            {membresia.nombrePlan || "—"}
                          </span>
                        </td>

                        <td className="px-6 py-4 text-slate-600 text-sm">
                          {formatDate(membresia.fechaInicio)}
                        </td>

                        <td className="px-6 py-4 text-slate-600 text-sm">
                          {formatDate(membresia.fechaFin)}
                        </td>

                        <td className="px-6 py-4">
                          <span
                            className={`inline-flex items-center gap-1.5 rounded-full border px-2.5 py-1 text-xs font-semibold ${status.bg} ${status.text} ${status.border}`}
                          >
                            {status.icon}
                            {status.label}
                          </span>
                        </td>

                        <td className="px-6 py-4">
                          <div className="flex flex-wrap justify-end gap-2">
                            {!isActive && (
                              <button
                                type="button"
                                onClick={() =>
                                  abrirModalAccion(membresia, "activar")
                                }
                                disabled={actionLoading}
                                className="rounded-lg border border-emerald-200 bg-emerald-50 px-3 py-1.5 text-xs font-medium text-emerald-700 transition hover:bg-emerald-100 disabled:opacity-50 disabled:cursor-not-allowed"
                              >
                                Activar
                              </button>
                            )}

                            {!isSuspended && !isCancelled && (
                              <button
                                type="button"
                                onClick={() =>
                                  abrirModalAccion(membresia, "suspender")
                                }
                                disabled={actionLoading}
                                className="rounded-lg border border-amber-200 bg-amber-50 px-3 py-1.5 text-xs font-medium text-amber-700 transition hover:bg-amber-100 disabled:opacity-50 disabled:cursor-not-allowed"
                              >
                                Suspender
                              </button>
                            )}

                            {!isCancelled && (
                              <button
                                type="button"
                                onClick={() =>
                                  abrirModalAccion(membresia, "cancelar")
                                }
                                disabled={actionLoading}
                                className="rounded-lg border border-rose-200 bg-rose-50 px-3 py-1.5 text-xs font-medium text-rose-700 transition hover:bg-rose-100 disabled:opacity-50 disabled:cursor-not-allowed"
                              >
                                Cancelar
                              </button>
                            )}
                          </div>
                        </td>
                      </tr>
                    );
                  })
                )}
              </tbody>
            </table>
          </div>
        </div>
      </main>

      {/* MODAL */}
      {selectedMembresia && action && (
        <MembresiaActionModal
          membresia={selectedMembresia}
          action={action}
          loading={actionLoading}
          onConfirm={ejecutarAccion}
          onCancel={cerrarModalAccion}
        />
      )}

      {/* TOAST DE CARGA */}
      {actionLoading && (
        <div className="fixed bottom-6 right-6 bg-slate-800 text-white px-5 py-3 rounded-xl shadow-xl flex items-center gap-3 z-50 animate-in slide-in-from-bottom-4">
          <div className="w-5 h-5 border-2 border-white/30 border-t-white rounded-full animate-spin" />
          <span className="text-sm font-medium">Actualizando membresía...</span>
        </div>
      )}
    </div>
  );
}


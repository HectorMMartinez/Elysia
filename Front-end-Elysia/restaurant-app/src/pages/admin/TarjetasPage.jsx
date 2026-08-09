import { useEffect, useMemo, useState } from "react";
import {
  FaCreditCard,
  FaSearch,
  FaSyncAlt,
  FaExclamationTriangle,
  FaEdit,
  FaStore,
  FaCalendarAlt,
} from "react-icons/fa";

import AdminSidebar from "../../components/layout/AdminSidebar";
import EditarTarjetaModal from "../../components/admin/EditarTarjetaModal";
import tarjetaService from "../../services/tarjetaService";

// Enmascarar número de tarjeta
const maskCardNumber = (number) => {
  if (!number) return "•••• •••• •••• ••••";
  const cleaned = String(number).replace(/\s/g, "");
  if (cleaned.length < 4) return "•••• •••• •••• ••••";
  const last4 = cleaned.slice(-4);
  return `•••• •••• •••• ${last4}`;
};

const getTipoLabel = (tipo) => {
  if (!tipo) return "Desconocido";

  return String(tipo);
};

const getTipoBadge = (tipo) => {
  const normalized = String(tipo || "").toLowerCase();

  if (normalized === "visa") {
    return "bg-blue-50 text-blue-700 border-blue-200";
  }

  if (normalized === "mastercard") {
    return "bg-orange-50 text-orange-700 border-orange-200";
  }

  if (
    normalized === "american express" ||
    normalized === "americanexpress"
  ) {
    return "bg-emerald-50 text-emerald-700 border-emerald-200";
  }

  return "bg-slate-100 text-slate-700 border-slate-200";
};


export default function TarjetasPage() {
  const [tarjetas, setTarjetas] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [search, setSearch] = useState("");

  const [selectedTarjeta, setSelectedTarjeta] = useState(null);
  const [actionLoading, setActionLoading] = useState(false);

  // ============================================================
  // CARGAR TARJETAS
  // ============================================================
  const cargarTarjetas = async () => {
    try {
      setLoading(true);
      setError(null);

      const data = await tarjetaService.getAll();
      setTarjetas(Array.isArray(data) ? data : []);
    } catch (err) {
      console.error("Error al cargar tarjetas:", err);
      const message =
        err.response?.data ||
        err.message ||
        "No fue posible cargar las tarjetas.";
      setError(
        typeof message === "string"
          ? message
          : "No fue posible cargar las tarjetas."
      );
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    cargarTarjetas();
  }, []);

  // ============================================================
  // FILTRADO
  // ============================================================
  const filteredTarjetas = useMemo(() => {
    const searchText = search.toLowerCase().trim();
    if (!searchText) return tarjetas;

    return tarjetas.filter((t) => {
      const titular = (t.nombreTitular || "").toLowerCase();
      const restaurante = (t.nombreRestaurante || "").toLowerCase();
      const direccion = (t.direccionRestaurante || "").toLowerCase();
      const last4 = String(t.numeroTarjeta || "").slice(-4);

      return (
        titular.includes(searchText) ||
        restaurante.includes(searchText) ||
        direccion.includes(searchText) ||
        last4.includes(searchText)
      );
    });
  }, [tarjetas, search]);

  // ============================================================
  // ESTADÍSTICAS
  // ============================================================
  const stats = useMemo(() => {
  const total = tarjetas.length;

  const visa = tarjetas.filter(
    (t) => String(t.tipo).toLowerCase() === "visa"
  ).length;

  const mastercard = tarjetas.filter(
    (t) => String(t.tipo).toLowerCase() === "mastercard"
  ).length;

  const amex = tarjetas.filter(
    (t) => {
      const tipo = String(t.tipo).toLowerCase();

      return (
        tipo === "american express" ||
        tipo === "americanexpress"
      );
    }
  ).length;

  return {
    total,
    visa,
    mastercard,
    amex,
  };
}, [tarjetas]);

  // ============================================================
  // EDITAR
  // ============================================================
  const abrirEditar = (tarjeta) => {
    setSelectedTarjeta(tarjeta);
  };

  const cerrarModal = () => {
    if (actionLoading) return;
    setSelectedTarjeta(null);
  };

  const guardarEdicion = async (formData) => {
    if (!selectedTarjeta) return;

    try {
      setActionLoading(true);
      setError(null);

      await tarjetaService.editar(selectedTarjeta.id, formData);

      setSelectedTarjeta(null);
      await cargarTarjetas();
    } catch (err) {
      console.error("Error al editar tarjeta:", err);
      const message =
        err.response?.data ||
        err.message ||
        "No fue posible editar la tarjeta.";
      setError(
        typeof message === "string"
          ? message
          : "No fue posible editar la tarjeta."
      );
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
                Tarjetas
              </h1>
              <p className="text-slate-500 text-sm mt-0.5">
                Gestiona las tarjetas registradas de los restaurantes
              </p>
            </div>
          </div>

          <button
            type="button"
            onClick={cargarTarjetas}
            disabled={loading || actionLoading}
            className="inline-flex items-center gap-2 px-4 py-2.5 bg-white border border-slate-200 rounded-xl text-slate-600 font-medium text-sm hover:bg-slate-50 hover:border-slate-300 transition-all disabled:opacity-50 disabled:cursor-not-allowed shadow-sm"
          >
            <FaSyncAlt className={loading ? "animate-spin" : ""} />
            Actualizar
          </button>
        </div>

        {/* ERROR */}
        {error && (
          <div className="mb-6 bg-red-50 border border-red-200 rounded-2xl p-4 flex items-start gap-3">
            <FaExclamationTriangle className="text-red-500 mt-0.5 shrink-0" />
            <div>
              <p className="font-semibold text-red-700">Ocurrió un problema</p>
              <p className="text-sm text-red-600 mt-1">{error}</p>
            </div>
          </div>
        )}

        {/* ESTADÍSTICAS */}
        <div className="grid grid-cols-2 lg:grid-cols-4 gap-4 mb-6">
          <div className="bg-white rounded-2xl border border-slate-100 shadow-sm p-5">
            <p className="text-sm font-medium text-slate-500">Total tarjetas</p>
            <p className="text-3xl font-bold text-slate-800 mt-2">
              {stats.total}
            </p>
          </div>
          <div className="bg-white rounded-2xl border border-slate-100 shadow-sm p-5">
            <p className="text-sm font-medium text-slate-500">Visa</p>
            <p className="text-3xl font-bold text-blue-600 mt-2">
              {stats.visa}
            </p>
          </div>
          <div className="bg-white rounded-2xl border border-slate-100 shadow-sm p-5">
            <p className="text-sm font-medium text-slate-500">Mastercard</p>
            <p className="text-3xl font-bold text-orange-600 mt-2">
              {stats.mastercard}
            </p>
          </div>
          <div className="bg-white rounded-2xl border border-slate-100 shadow-sm p-5">
            <p className="text-sm font-medium text-slate-500">Amex</p>
            <p className="text-3xl font-bold text-emerald-600 mt-2">
              {stats.amex}
            </p>
          </div>
        </div>

        {/* BUSCADOR */}
        <div className="bg-white rounded-2xl border border-slate-100 shadow-sm p-5 mb-6">
          <div className="relative">
            <FaSearch className="absolute left-4 top-1/2 -translate-y-1/2 text-slate-400" />
            <input
              type="text"
              value={search}
              onChange={(e) => setSearch(e.target.value)}
              placeholder="Buscar por titular, restaurante o últimos 4 dígitos..."
              className="w-full pl-11 pr-4 py-3 border border-slate-200 rounded-xl outline-none focus:ring-2 focus:ring-violet-500/30 focus:border-violet-500 text-slate-700 placeholder:text-slate-400 transition"
            />
          </div>
          <div className="mt-3 text-sm text-slate-500">
            Mostrando{" "}
            <span className="font-semibold text-slate-700">
              {filteredTarjetas.length}
            </span>{" "}
            de{" "}
            <span className="font-semibold text-slate-700">{stats.total}</span>{" "}
            tarjetas
          </div>
        </div>

        {/* TABLA */}
        <div className="bg-white rounded-2xl border border-slate-100 shadow-sm overflow-hidden">
          <div className="overflow-x-auto">
            <table className="w-full text-left">
              <thead className="bg-slate-50/80 border-b border-slate-200">
                <tr>
                  <th className="px-6 py-4 text-xs font-semibold text-slate-500 uppercase tracking-wider">
                    Restaurante
                  </th>
                  <th className="px-6 py-4 text-xs font-semibold text-slate-500 uppercase tracking-wider">
                    Titular
                  </th>
                  <th className="px-6 py-4 text-xs font-semibold text-slate-500 uppercase tracking-wider">
                    Número
                  </th>
                  <th className="px-6 py-4 text-xs font-semibold text-slate-500 uppercase tracking-wider">
                    Tipo
                  </th>
                  <th className="px-6 py-4 text-xs font-semibold text-slate-500 uppercase tracking-wider">
                    Vencimiento
                  </th>
                  <th className="px-6 py-4 text-xs font-semibold text-slate-500 uppercase tracking-wider text-right">
                    Acciones
                  </th>
                </tr>
              </thead>

              <tbody className="divide-y divide-slate-100">
                {loading ? (
                  Array.from({ length: 5 }).map((_, i) => (
                    <tr key={i}>
                      {Array.from({ length: 6 }).map((_, j) => (
                        <td key={j} className="px-6 py-5">
                          <div className="h-4 bg-slate-100 rounded animate-pulse w-3/4" />
                        </td>
                      ))}
                    </tr>
                  ))
                ) : filteredTarjetas.length === 0 ? (
                  <tr>
                    <td colSpan={6} className="px-6 py-16 text-center">
                      <div className="flex flex-col items-center gap-3">
                        <div className="w-14 h-14 rounded-full bg-slate-100 flex items-center justify-center">
                          <FaCreditCard className="text-slate-400 text-xl" />
                        </div>
                        <p className="text-slate-500 font-medium">
                          No se encontraron tarjetas
                        </p>
                        <p className="text-sm text-slate-400">
                          Prueba cambiando el término de búsqueda
                        </p>
                      </div>
                    </td>
                  </tr>
                ) : (
                  filteredTarjetas.map((tarjeta) => (
                    <tr
                      key={tarjeta.id}
                      className="hover:bg-slate-50/70 transition-colors"
                    >
                      <td className="px-6 py-4">
                        <div className="flex items-start gap-2">
                          <FaStore className="text-slate-400 mt-0.5 shrink-0" />
                          <div>
                            <p className="font-medium text-slate-800">
                              {tarjeta.nombreRestaurante || "—"}
                            </p>
                            <p className="text-xs text-slate-500 mt-0.5">
                              {tarjeta.direccionRestaurante || "Sin dirección"}
                            </p>
                          </div>
                        </div>
                      </td>

                      <td className="px-6 py-4 text-slate-700 font-medium">
                        {tarjeta.nombreTitular || "—"}
                      </td>

                      <td className="px-6 py-4">
                        <span className="font-mono text-slate-700 tracking-wider">
                          {maskCardNumber(tarjeta.numeroTarjeta)}
                        </span>
                      </td>

                      <td className="px-6 py-4">
                        <span
                          className={`inline-flex items-center px-2.5 py-1 rounded-full border text-xs font-semibold ${getTipoBadge(
                            tarjeta.tipo
                          )}`}
                        >
                          {getTipoLabel(tarjeta.tipo)}
                        </span>
                      </td>

                      <td className="px-6 py-4">
                        <div className="flex items-center gap-1.5 text-slate-600 text-sm">
                          <FaCalendarAlt className="text-slate-400 text-xs" />
                          {String(tarjeta.mesVencimiento).padStart(2, "0")}/
                          {tarjeta.anioVencimiento}
                        </div>
                      </td>

                      <td className="px-6 py-4 text-right">
                        <button
                          type="button"
                          onClick={() => abrirEditar(tarjeta)}
                          disabled={actionLoading}
                          className="inline-flex items-center gap-1.5 rounded-lg border border-violet-200 bg-violet-50 px-3 py-1.5 text-xs font-medium text-violet-700 transition hover:bg-violet-100 disabled:opacity-50 disabled:cursor-not-allowed"
                        >
                          <FaEdit />
                          Editar
                        </button>
                      </td>
                    </tr>
                  ))
                )}
              </tbody>
            </table>
          </div>
        </div>
      </main>

      {/* MODAL DE EDICIÓN */}
      {selectedTarjeta && (
        <EditarTarjetaModal
          tarjeta={selectedTarjeta}
          loading={actionLoading}
          onConfirm={guardarEdicion}
          onCancel={cerrarModal}
        />
      )}
    </div>
  );
}
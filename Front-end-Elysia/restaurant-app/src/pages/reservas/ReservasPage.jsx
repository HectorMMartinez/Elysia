import React, { useState, useEffect, useMemo } from "react";
import OwnerSidebar from "../../components/layout/OwnerSidebar";
import {
  obtenerReservas,
  crearReserva,
  editarReserva,
  eliminarReserva,
  cambiarReservaEnProceso,
  cambiarReservaNoAsistio,
  finalizarReserva,
  cancelarReserva,
} from "../../services/reservaService";
import { ReservaFormModal } from "../../components/reservas/ReservaFormModal";
import ConfirmActionModalReserva from "../../components/common/ConfirmActionModalReserva";
import {
  FiPlus,
  FiSearch,
  FiRefreshCw,
  FiUsers,
  FiAlertCircle,
  FiEdit2,
  FiTrash2,
  FiPlay,
  FiXCircle,
  FiCheck,
  FiUserX,
} from "react-icons/fi";

export default function ReservasPage() {
  const [reservas, setReservas] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [searchTerm, setSearchTerm] = useState("");
  const [selectedEstado, setSelectedEstado] = useState("Todos");

  // Modal Formulario
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [editingReserva, setEditingReserva] = useState(null);
  const [formSubmitting, setFormSubmitting] = useState(false);

  // Modal de Confirmación
  const [confirmModal, setConfirmModal] = useState({
    isOpen: false,
    type: null, // "delete" | "enProceso" | "noAsistio" | "finalizar" | "cancelar"
    reserva: null,
    loading: false,
  });

  // ===================== FETCH =====================
  const fetchReservas = async () => {
    try {
      setLoading(true);
      setError(null);
      const data = await obtenerReservas();
      setReservas(Array.isArray(data) ? data : []);
    } catch (err) {
      console.error("Error al cargar reservas:", err);
      setError(err.message || "No pudimos cargar la lista de reservas.");
    } finally {
      setLoading(false);
    }
  };


  useEffect(() => {
    fetchReservas();
  }, []);

  // ===================== STATS =====================
  const stats = useMemo(() => {
    return {
      total: reservas.length,
      activas: reservas.filter((r) => r.estado === "Activa").length,
      enProceso: reservas.filter((r) => r.estado === "EnProceso").length,
      finalizadas: reservas.filter((r) => r.estado === "Finalizada").length,
      canceladas: reservas.filter((r) => r.estado === "Cancelada").length,
    };
  }, [reservas]);

  // ===================== FILTROS =====================
  const filteredReservas = useMemo(() => {
    return reservas.filter((r) => {
      const matchSearch =
        r.nombreCliente?.toLowerCase().includes(searchTerm.toLowerCase()) ||
        r.dniCliente?.toLowerCase().includes(searchTerm.toLowerCase()) ||
        r.observaciones?.toLowerCase().includes(searchTerm.toLowerCase());

      const matchEstado =
        selectedEstado === "Todos" || r.estado === selectedEstado;

      return matchSearch && matchEstado;
    });
  }, [reservas, searchTerm, selectedEstado]);

  // ===================== HANDLERS FORM =====================
  const handleOpenCreateModal = () => {
    setEditingReserva(null);
    setIsModalOpen(true);
  };

  const handleOpenEditModal = (reserva) => {
    setEditingReserva(reserva);
    setIsModalOpen(true);
  };


const handleFormSubmit = async (formData) => {
  try {
    setFormSubmitting(true);

    if (editingReserva) {
      await editarReserva(editingReserva.id, formData);
    } else {
      await crearReserva(formData);
    }

    setIsModalOpen(false);
    fetchReservas();
  } catch (err) {
    // 🔑 Importante: relanzar para que el modal lo capture
    throw err;
  } finally {
    setFormSubmitting(false);
  }
};


  // ===================== HANDLERS CONFIRMACIÓN =====================
  const openConfirm = (type, reserva) => {
    setConfirmModal({ isOpen: true, type, reserva, loading: false });
  };

  const closeConfirm = () => {
    setConfirmModal({
      isOpen: false,
      type: null,
      reserva: null,
      loading: false,
    });
  };

  const handleConfirmAction = async () => {
    const { type, reserva } = confirmModal;
    if (!reserva) return;

    try {
      setConfirmModal((prev) => ({ ...prev, loading: true }));

      switch (type) {
        case "delete":
          await eliminarReserva(reserva.id);
          break;
        case "enProceso":
          await cambiarReservaEnProceso(reserva.id);
          break;
        case "noAsistio":
          await cambiarReservaNoAsistio(reserva.id);
          break;
        case "finalizar":
          await finalizarReserva(reserva.id);
          break;
        case "cancelar":
          await cancelarReserva(reserva.id);
          break;
        default:
          break;
      }

      closeConfirm();
      fetchReservas();
    } catch (err) {
      console.error("Error en acción de reserva:", err);
      alert(err.message || "Ocurrió un error al procesar la acción.");
      setConfirmModal((prev) => ({ ...prev, loading: false }));
    }
  };

  // ===================== HELPERS UI =====================
  const getStatusBadge = (estado) => {
    const styles = {
      Activa: "bg-emerald-100 text-emerald-700",
      EnProceso: "bg-blue-100 text-blue-700",
      NoAsistio: "bg-orange-100 text-orange-700",
      Finalizada: "bg-slate-100 text-slate-700",
      Cancelada: "bg-red-100 text-red-700",
    };

    return (
      <span
        className={`px-3 py-1 text-xs font-bold rounded-full ${
          styles[estado] || "bg-gray-100 text-gray-700"
        }`}
      >
        {estado || "N/A"}
      </span>
    );
  };

  const formatDate = (dateStr) => {
    if (!dateStr) return "—";
    return new Date(dateStr).toLocaleString("es-DO", {
      day: "2-digit",
      month: "short",
      year: "numeric",
      hour: "2-digit",
      minute: "2-digit",
    });
  };

  const getConfirmConfig = () => {
    const { type, reserva } = confirmModal;
    if (!reserva) return {};

    const configs = {
      delete: {
        title: "Eliminar Reserva",
        subtitle: "Acción irreversible",
        message: `¿Estás seguro de eliminar la reserva de ${reserva.nombreCliente}? Esta acción no se puede deshacer.`,
        confirmText: "Eliminar",
        confirmColor: "bg-red-600 hover:bg-red-700",
      },
      enProceso: {
        title: "Poner en Proceso",
        subtitle: "Cambiar estado",
        message: `¿Deseas marcar la reserva de ${reserva.nombreCliente} como "En Proceso"?`,
        confirmText: "Poner en Proceso",
        confirmColor: "bg-blue-600 hover:bg-blue-700",
      },
      noAsistio: {
        title: "Marcar No Asistió",
        subtitle: "Cambiar estado",
        message: `¿Confirmas que el cliente ${reserva.nombreCliente} no asistió a la reserva?`,
        confirmText: "Marcar No Asistió",
        confirmColor: "bg-orange-600 hover:bg-orange-700",
      },
      finalizar: {
        title: "Finalizar Reserva",
        subtitle: "Cambiar estado",
        message: `¿Deseas finalizar la reserva de ${reserva.nombreCliente}?`,
        confirmText: "Finalizar",
        confirmColor: "bg-emerald-600 hover:bg-emerald-700",
      },
      cancelar: {
        title: "Cancelar Reserva",
        subtitle: "Cambiar estado",
        message: `¿Estás seguro de cancelar la reserva de ${reserva.nombreCliente}?`,
        confirmText: "Cancelar Reserva",
        confirmColor: "bg-red-600 hover:bg-red-700",
      },
    };

    return configs[type] || {};
  };

  // ===================== RENDER =====================
  return (
    <OwnerSidebar>
      <div className="p-8 max-w-[1600px] mx-auto space-y-8 bg-slate-50/50 min-h-screen">
        {/* Header */}
        <div className="flex flex-col sm:flex-row justify-between items-start sm:items-center gap-4">
          <div>
            <h1 className="text-3xl font-bold text-slate-800">Reservas</h1>
            <p className="text-slate-500 mt-1">
              Gestiona las reservas de mesas de tu restaurante.
            </p>
          </div>
          <button
            onClick={handleOpenCreateModal}
            className="flex items-center gap-2 px-5 py-3 bg-purple-600 text-white font-semibold text-sm rounded-xl hover:bg-purple-700 shadow-lg shadow-purple-200 transition-all"
          >
            <FiPlus size={18} />
            <span>Nueva Reserva</span>
          </button>
        </div>

        {/* KPIs */}
        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-5 gap-5">
          <div className="bg-white p-5 rounded-2xl border border-gray-100 shadow-sm">
            <p className="text-xs font-medium text-slate-400">Total</p>
            <h3 className="text-2xl font-extrabold text-slate-800 mt-1">
              {stats.total}
            </h3>
          </div>
          <div className="bg-white p-5 rounded-2xl border border-gray-100 shadow-sm">
            <p className="text-xs font-medium text-slate-400">Activas</p>
            <h3 className="text-2xl font-extrabold text-emerald-600 mt-1">
              {stats.activas}
            </h3>
          </div>
          <div className="bg-white p-5 rounded-2xl border border-gray-100 shadow-sm">
            <p className="text-xs font-medium text-slate-400">En Proceso</p>
            <h3 className="text-2xl font-extrabold text-blue-600 mt-1">
              {stats.enProceso}
            </h3>
          </div>
          <div className="bg-white p-5 rounded-2xl border border-gray-100 shadow-sm">
            <p className="text-xs font-medium text-slate-400">Finalizadas</p>
            <h3 className="text-2xl font-extrabold text-slate-600 mt-1">
              {stats.finalizadas}
            </h3>
          </div>
          <div className="bg-white p-5 rounded-2xl border border-gray-100 shadow-sm">
            <p className="text-xs font-medium text-slate-400">Canceladas</p>
            <h3 className="text-2xl font-extrabold text-red-600 mt-1">
              {stats.canceladas}
            </h3>
          </div>
        </div>

        {/* Tabla / Listado */}
        <div className="bg-white rounded-3xl border border-gray-100 shadow-sm p-6 space-y-6">
          {/* Filtros */}
          <div className="flex flex-col sm:flex-row justify-between gap-4">
            <div className="relative flex-1 max-w-md">
              <FiSearch
                className="absolute left-4 top-3.5 text-gray-400"
                size={18}
              />
              <input
                type="text"
                placeholder="Buscar por cliente, DNI u observaciones..."
                value={searchTerm}
                onChange={(e) => setSearchTerm(e.target.value)}
                className="w-full pl-11 pr-4 py-2.5 rounded-xl border border-gray-200 text-sm focus:outline-none focus:ring-2 focus:ring-purple-500/20"
              />
            </div>

            <div className="flex gap-3">
              <select
                value={selectedEstado}
                onChange={(e) => setSelectedEstado(e.target.value)}
                className="px-4 py-2.5 rounded-xl border border-gray-200 text-sm bg-white font-medium text-gray-700 focus:outline-none focus:ring-2 focus:ring-purple-500/20"
              >
                <option value="Todos">Todos los estados</option>
                <option value="Activa">Activa</option>
                <option value="EnProceso">En Proceso</option>
                <option value="NoAsistio">No Asistió</option>
                <option value="Finalizada">Finalizada</option>
                <option value="Cancelada">Cancelada</option>
              </select>

              <button
                onClick={fetchReservas}
                className="flex items-center gap-2 px-4 py-2.5 rounded-xl border border-gray-200 text-gray-600 font-semibold text-sm hover:bg-gray-50 transition-colors"
              >
                <FiRefreshCw size={16} />
                <span>Actualizar</span>
              </button>
            </div>
          </div>

          {/* Contenido */}
          {loading ? (
            <div className="py-20 text-center space-y-3">
              <FiRefreshCw className="w-8 h-8 text-purple-600 animate-spin mx-auto" />
              <p className="text-gray-500 font-semibold">
                Cargando reservas...
              </p>
            </div>
          ) : error ? (
            <div className="py-16 text-center space-y-4">
              <div className="p-4 bg-red-50 text-red-500 rounded-full w-fit mx-auto">
                <FiAlertCircle size={32} />
              </div>
              <p className="text-gray-800 font-bold">{error}</p>
              <button
                onClick={fetchReservas}
                className="px-6 py-2.5 bg-purple-600 text-white text-sm font-semibold rounded-xl hover:bg-purple-700"
              >
                Intentar de nuevo
              </button>
            </div>
          ) : filteredReservas.length === 0 ? (
            <div className="py-16 text-center text-gray-400 font-medium">
              No se encontraron reservas.
            </div>
          ) : (
            <div className="overflow-x-auto">
              <table className="w-full text-left">
                <thead>
                  <tr className="border-b border-gray-100 text-xs font-semibold text-slate-400 uppercase tracking-wider">
                    <th className="pb-4 pl-2">Cliente</th>
                    <th className="pb-4">Mesa</th>
                    <th className="pb-4">Personas</th>
                    <th className="pb-4">Fecha Reserva</th>
                    <th className="pb-4">Estado</th>
                    <th className="pb-4 text-right pr-2">Acciones</th>
                  </tr>
                </thead>
                <tbody className="divide-y divide-gray-50">
                  {filteredReservas.map((reserva) => (
                    <tr
                      key={reserva.id}
                      className="hover:bg-slate-50/50 transition"
                    >
                      <td className="py-4 pl-2">
                        <div>
                          <p className="font-semibold text-slate-800">
                            {reserva.nombreCliente}
                          </p>
                          <p className="text-xs text-slate-500">
                            {reserva.dniCliente}
                          </p>
                        </div>
                      </td>
                      <td className="py-4 text-sm text-slate-600">
                        Mesa #{reserva.mesaId}
                      </td>
                      <td className="py-4">
                        <div className="flex items-center gap-1.5 text-sm text-slate-600">
                          <FiUsers size={14} className="text-purple-500" />
                          {reserva.cantidadPersona}
                        </div>
                      </td>
                      <td className="py-4 text-sm text-slate-600">
                        {formatDate(reserva.fechaReserva)}
                      </td>
                      <td className="py-4">
                        {getStatusBadge(reserva.estado)}
                      </td>
                      <td className="py-4 pr-2">
                        <div className="flex items-center justify-end gap-1.5">
                          {/* Botones según estado */}
                          {reserva.estado === "Activa" && (
                            <>
                              <button
                                onClick={() =>
                                  openConfirm("enProceso", reserva)
                                }
                                className="p-2 text-blue-600 hover:bg-blue-50 rounded-lg transition"
                                title="Poner en Proceso"
                              >
                                <FiPlay size={16} />
                              </button>
                              <button
                                onClick={() =>
                                  openConfirm("cancelar", reserva)
                                }
                                className="p-2 text-red-500 hover:bg-red-50 rounded-lg transition"
                                title="Cancelar"
                              >
                                <FiXCircle size={16} />
                              </button>
                            </>
                          )}

                          {reserva.estado === "EnProceso" && (
                            <>
                              <button
                                onClick={() =>
                                  openConfirm("finalizar", reserva)
                                }
                                className="p-2 text-emerald-600 hover:bg-emerald-50 rounded-lg transition"
                                title="Finalizar"
                              >
                                <FiCheck size={16} />
                              </button>
                              <button
                                onClick={() =>
                                  openConfirm("noAsistio", reserva)
                                }
                                className="p-2 text-orange-600 hover:bg-orange-50 rounded-lg transition"
                                title="No Asistió"
                              >
                                <FiUserX size={16} />
                              </button>
                            </>
                          )}

                          {/* Editar solo si está Activa o EnProceso */}
                          {(reserva.estado === "Activa" ||
                            reserva.estado === "EnProceso") && (
                            <button
                              onClick={() => handleOpenEditModal(reserva)}
                              className="p-2 text-gray-500 hover:text-purple-600 hover:bg-purple-50 rounded-lg transition"
                              title="Editar"
                            >
                              <FiEdit2 size={16} />
                            </button>
                          )}

                          {/* Eliminar */}
                          <button
                            onClick={() => openConfirm("delete", reserva)}
                            className="p-2 text-gray-500 hover:text-red-600 hover:bg-red-50 rounded-lg transition"
                            title="Eliminar"
                          >
                            <FiTrash2 size={16} />
                          </button>
                        </div>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          )}
        </div>

        {/* Modal Formulario */}
        <ReservaFormModal
          isOpen={isModalOpen}
          onClose={() => setIsModalOpen(false)}
          onSubmit={handleFormSubmit}
          initialData={editingReserva}
          isLoading={formSubmitting}
        />

        {/* Modal de Confirmación */}
        <ConfirmActionModalReserva
          isOpen={confirmModal.isOpen}
          loading={confirmModal.loading}
          onConfirm={handleConfirmAction}
          onCancel={closeConfirm}
          {...getConfirmConfig()}
        />
      </div>
    </OwnerSidebar>
  );
};
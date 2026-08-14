import React, { useState, useEffect, useMemo } from "react";
import OwnerSidebar from "../../components/layout/OwnerSidebar";
import {
  obtenerPedidos,
  crearPedido,
  editarPedido,
  eliminarPedido,
  cambiarPedidoEnPreparacion,
  cambiarPedidoListo,
  cancelarPedido,
  finalizarPedido,
} from "../../services/pedidoService";
import { PedidoFormModal } from "../../components/pedidos/PedidoFormModal";
import ConfirmActionModalPedido from "../../components/common/ConfirmActionModalPedido";
import {
  FiPlus,
  FiSearch,
  FiRefreshCw,
  FiAlertCircle,
  FiEdit2,
  FiTrash2,
  FiPlay,
  FiCheck,
  FiXCircle,
  FiPackage,
} from "react-icons/fi";

export default function PedidosPage() {
  const [pedidos, setPedidos] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [searchTerm, setSearchTerm] = useState("");
  const [selectedEstado, setSelectedEstado] = useState("Todos");

  const [isModalOpen, setIsModalOpen] = useState(false);
  const [editingPedido, setEditingPedido] = useState(null);
  const [formSubmitting, setFormSubmitting] = useState(false);

  const [confirmModal, setConfirmModal] = useState({
    isOpen: false,
    type: null,
    pedido: null,
    loading: false,
  });

  const fetchPedidos = async () => {
    try {
      setLoading(true);
      setError(null);
      const data = await obtenerPedidos();
      setPedidos(Array.isArray(data) ? data : []);
    } catch (err) {
      console.error("Error al cargar pedidos:", err);
      setError(err.message || "No pudimos cargar la lista de pedidos.");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchPedidos();
  }, []);

  const stats = useMemo(() => {
    return {
      total: pedidos.length,
      pendientes: pedidos.filter((p) => p.estado === "Pendiente").length,
      enPreparacion: pedidos.filter((p) => p.estado === "EnPreparacion").length,
      listos: pedidos.filter((p) => p.estado === "Listo").length,
      finalizados: pedidos.filter((p) => p.estado === "Finalizado").length,
      cancelados: pedidos.filter((p) => p.estado === "Cancelado").length,
    };
  }, [pedidos]);

  const filteredPedidos = useMemo(() => {
    return pedidos.filter((p) => {
      const texto = [
        String(p.id),
        String(p.idMesa),
        p.nombreMesa || "",
        ...(p.mostrarDetalles || []).map((d) => d.nombrePlato || ""),
      ]
        .join(" ")
        .toLowerCase();

      const matchSearch = texto.includes(searchTerm.toLowerCase());
      const matchEstado =
        selectedEstado === "Todos" || p.estado === selectedEstado;

      return matchSearch && matchEstado;
    });
  }, [pedidos, searchTerm, selectedEstado]);

  const handleOpenCreateModal = () => {
    setEditingPedido(null);
    setIsModalOpen(true);
  };

  const handleOpenEditModal = (pedido) => {
    setEditingPedido(pedido);
    setIsModalOpen(true);
  };

  const handleFormSubmit = async (formData) => {
    try {
      setFormSubmitting(true);

      if (editingPedido) {
        await editarPedido(editingPedido.id, formData);
      } else {
        await crearPedido(formData);
      }

      setIsModalOpen(false);
      fetchPedidos();
    } catch (err) {
      throw err; // para que el modal muestre el error y no se cierre
    } finally {
      setFormSubmitting(false);
    }
  };

  const openConfirm = (type, pedido) => {
    setConfirmModal({ isOpen: true, type, pedido, loading: false });
  };

  const closeConfirm = () => {
    setConfirmModal({
      isOpen: false,
      type: null,
      pedido: null,
      loading: false,
    });
  };

  const handleConfirmAction = async () => {
    const { type, pedido } = confirmModal;
    if (!pedido) return;

    try {
      setConfirmModal((prev) => ({ ...prev, loading: true }));

      switch (type) {
        case "delete":
          await eliminarPedido(pedido.id);
          break;
        case "enPreparacion":
          await cambiarPedidoEnPreparacion(pedido.id);
          break;
        case "listo":
          await cambiarPedidoListo(pedido.id);
          break;
        case "cancelar":
          await cancelarPedido(pedido.id);
          break;
        case "finalizar":
          await finalizarPedido(pedido.id);
          break;
        default:
          break;
      }

      closeConfirm();
      fetchPedidos();
    } catch (err) {
      console.error("Error en acción de pedido:", err);
      alert(err.message || "Ocurrió un error al procesar la acción.");
      setConfirmModal((prev) => ({ ...prev, loading: false }));
    }
  };

  const getStatusBadge = (estado) => {
    const styles = {
      Pendiente: "bg-amber-100 text-amber-700",
      EnPreparacion: "bg-blue-100 text-blue-700",
      Listo: "bg-emerald-100 text-emerald-700",
      Finalizado: "bg-slate-100 text-slate-700",
      Cancelado: "bg-red-100 text-red-700",
    };

    const labels = {
      Pendiente: "Pendiente",
      EnPreparacion: "En Preparación",
      Listo: "Listo",
      Finalizado: "Finalizado",
      Cancelado: "Cancelado",
    };

    return (
      <span
        className={`px-3 py-1 text-xs font-bold rounded-full ${
          styles[estado] || "bg-gray-100 text-gray-700"
        }`}
      >
        {labels[estado] || estado || "N/A"}
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

  const formatMoney = (value) => {
    if (value == null) return "—";
    return `RD$ ${Number(value).toLocaleString("es-DO", {
      minimumFractionDigits: 2,
      maximumFractionDigits: 2,
    })}`;
  };

  const getConfirmConfig = () => {
    const { type, pedido } = confirmModal;
    if (!pedido) return {};

    const configs = {
      delete: {
        title: "Eliminar Pedido",
        subtitle: "Acción irreversible",
        message: `¿Estás seguro de eliminar el pedido #${pedido.id}? Esta acción no se puede deshacer.`,
        confirmText: "Eliminar",
        confirmColor: "bg-red-600 hover:bg-red-700",
      },
      enPreparacion: {
        title: "En Preparación",
        subtitle: "Cambiar estado",
        message: `¿Deseas marcar el pedido #${pedido.id} como "En Preparación"?`,
        confirmText: "Poner en Preparación",
        confirmColor: "bg-blue-600 hover:bg-blue-700",
      },
      listo: {
        title: "Pedido Listo",
        subtitle: "Cambiar estado",
        message: `¿Confirmas que el pedido #${pedido.id} está listo?`,
        confirmText: "Marcar Listo",
        confirmColor: "bg-emerald-600 hover:bg-emerald-700",
      },
      cancelar: {
        title: "Cancelar Pedido",
        subtitle: "Cambiar estado",
        message: `¿Estás seguro de cancelar el pedido #${pedido.id}?`,
        confirmText: "Cancelar Pedido",
        confirmColor: "bg-red-600 hover:bg-red-700",
      },
      finalizar: {
        title: "Finalizar Pedido",
        subtitle: "Cambiar estado",
        message: `¿Deseas finalizar el pedido #${pedido.id}?`,
        confirmText: "Finalizar",
        confirmColor: "bg-slate-700 hover:bg-slate-800",
      },
    };

    return configs[type] || {};
  };

  const totalItems = (pedido) =>
    (pedido.mostrarDetalles || []).reduce(
      (acc, d) => acc + (d.cantidaPlato || 0),
      0
    );

  return (
    <OwnerSidebar>
      <div className="p-8 max-w-[1600px] mx-auto space-y-8 bg-slate-50/50 min-h-screen">
        {/* Header */}
        <div className="flex flex-col sm:flex-row justify-between items-start sm:items-center gap-4">
          <div>
            <h1 className="text-3xl font-bold text-slate-800">Pedidos</h1>
            <p className="text-slate-500 mt-1">
              Gestiona los pedidos de mesas de tu restaurante.
            </p>
          </div>
          <button
            onClick={handleOpenCreateModal}
            className="flex items-center gap-2 px-5 py-3 bg-purple-600 text-white font-semibold text-sm rounded-xl hover:bg-purple-700 shadow-lg shadow-purple-200 transition-all"
          >
            <FiPlus size={18} />
            <span>Nuevo Pedido</span>
          </button>
        </div>

        {/* KPIs */}
        <div className="grid grid-cols-2 sm:grid-cols-3 lg:grid-cols-6 gap-4">
          {[
            { label: "Total", value: stats.total, color: "text-slate-800" },
            {
              label: "Pendientes",
              value: stats.pendientes,
              color: "text-amber-600",
            },
            {
              label: "En Preparación",
              value: stats.enPreparacion,
              color: "text-blue-600",
            },
            { label: "Listos", value: stats.listos, color: "text-emerald-600" },
            {
              label: "Finalizados",
              value: stats.finalizados,
              color: "text-slate-600",
            },
            {
              label: "Cancelados",
              value: stats.cancelados,
              color: "text-red-600",
            },
          ].map((kpi) => (
            <div
              key={kpi.label}
              className="bg-white p-4 rounded-2xl border border-gray-100 shadow-sm"
            >
              <p className="text-xs font-medium text-slate-400">{kpi.label}</p>
              <h3 className={`text-2xl font-extrabold mt-1 ${kpi.color}`}>
                {kpi.value}
              </h3>
            </div>
          ))}
        </div>

        {/* Tabla */}
        <div className="bg-white rounded-3xl border border-gray-100 shadow-sm p-6 space-y-6">
          <div className="flex flex-col sm:flex-row justify-between gap-4">
            <div className="relative flex-1 max-w-md">
              <FiSearch
                className="absolute left-4 top-3.5 text-gray-400"
                size={18}
              />
              <input
                type="text"
                placeholder="Buscar por #pedido, mesa o plato..."
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
                <option value="Pendiente">Pendiente</option>
                <option value="EnPreparacion">En Preparación</option>
                <option value="Listo">Listo</option>
                <option value="Finalizado">Finalizado</option>
                <option value="Cancelado">Cancelado</option>
              </select>

              <button
                onClick={fetchPedidos}
                className="flex items-center gap-2 px-4 py-2.5 rounded-xl border border-gray-200 text-gray-600 font-semibold text-sm hover:bg-gray-50 transition-colors"
              >
                <FiRefreshCw size={16} />
                <span>Actualizar</span>
              </button>
            </div>
          </div>

          {loading ? (
            <div className="py-20 text-center space-y-3">
              <FiRefreshCw className="w-8 h-8 text-purple-600 animate-spin mx-auto" />
              <p className="text-gray-500 font-semibold">Cargando pedidos...</p>
            </div>
          ) : error ? (
            <div className="py-16 text-center space-y-4">
              <div className="p-4 bg-red-50 text-red-500 rounded-full w-fit mx-auto">
                <FiAlertCircle size={32} />
              </div>
              <p className="text-gray-800 font-bold">{error}</p>
              <button
                onClick={fetchPedidos}
                className="px-6 py-2.5 bg-purple-600 text-white text-sm font-semibold rounded-xl hover:bg-purple-700"
              >
                Intentar de nuevo
              </button>
            </div>
          ) : filteredPedidos.length === 0 ? (
            <div className="py-16 text-center text-gray-400 font-medium">
              No se encontraron pedidos.
            </div>
          ) : (
            <div className="overflow-x-auto">
              <table className="w-full text-left">
                <thead>
                  <tr className="border-b border-gray-100 text-xs font-semibold text-slate-400 uppercase tracking-wider">
                    <th className="pb-4 pl-2">Pedido</th>
                    <th className="pb-4">Mesa</th>
                    <th className="pb-4">Platos</th>
                    <th className="pb-4">Total</th>
                    <th className="pb-4">Fecha</th>
                    <th className="pb-4">Estado</th>
                    <th className="pb-4 text-right pr-2">Acciones</th>
                  </tr>
                </thead>
                <tbody className="divide-y divide-gray-50">
                  {filteredPedidos.map((pedido) => (
                    <tr
                      key={pedido.id}
                      className="hover:bg-slate-50/50 transition"
                    >
                      <td className="py-4 pl-2">
                        <p className="font-semibold text-slate-800">
                          #{pedido.id}
                        </p>
                      </td>
                      <td className="py-4 text-sm text-slate-600">
                        {pedido.nombreMesa || `Mesa #${pedido.idMesa}`}
                      </td>
                      <td className="py-4">
                        <div className="flex items-center gap-1.5 text-sm text-slate-600">
                          <FiPackage size={14} className="text-purple-500" />
                          {totalItems(pedido)} ítems
                        </div>
                        <p className="text-xs text-slate-400 mt-0.5 line-clamp-1 max-w-[220px]">
                          {(pedido.mostrarDetalles || [])
                            .map(
                              (d) =>
                                d.nombrePlato || `Plato ${d.idPlato}`
                            )
                            .join(", ")}
                        </p>
                      </td>
                      <td className="py-4 text-sm font-semibold text-purple-700">
                        {formatMoney(pedido.totalPedido)}
                      </td>
                      <td className="py-4 text-sm text-slate-600">
                        {formatDate(pedido.fechaCreacion)}
                      </td>
                      <td className="py-4">{getStatusBadge(pedido.estado)}</td>
                      <td className="py-4 pr-2">
                        <div className="flex items-center justify-end gap-1.5">
                          {pedido.estado === "Pendiente" && (
                            <>
                              <button
                                onClick={() =>
                                  openConfirm("enPreparacion", pedido)
                                }
                                className="p-2 text-blue-600 hover:bg-blue-50 rounded-lg transition"
                                title="En Preparación"
                              >
                                <FiPlay size={16} />
                              </button>
                              <button
                                onClick={() =>
                                  openConfirm("cancelar", pedido)
                                }
                                className="p-2 text-red-500 hover:bg-red-50 rounded-lg transition"
                                title="Cancelar"
                              >
                                <FiXCircle size={16} />
                              </button>
                            </>
                          )}

                          {pedido.estado === "EnPreparacion" && (
                            <>
                              <button
                                onClick={() => openConfirm("listo", pedido)}
                                className="p-2 text-emerald-600 hover:bg-emerald-50 rounded-lg transition"
                                title="Marcar Listo"
                              >
                                <FiCheck size={16} />
                              </button>
                              <button
                                onClick={() =>
                                  openConfirm("cancelar", pedido)
                                }
                                className="p-2 text-red-500 hover:bg-red-50 rounded-lg transition"
                                title="Cancelar"
                              >
                                <FiXCircle size={16} />
                              </button>
                            </>
                          )}

                          {pedido.estado === "Listo" && (
                            <button
                              onClick={() => openConfirm("finalizar", pedido)}
                              className="p-2 text-slate-600 hover:bg-slate-100 rounded-lg transition"
                              title="Finalizar"
                            >
                              <FiCheck size={16} />
                            </button>
                          )}

                          {(pedido.estado === "Pendiente" ||
                            pedido.estado === "EnPreparacion") && (
                            <button
                              onClick={() => handleOpenEditModal(pedido)}
                              className="p-2 text-gray-500 hover:text-purple-600 hover:bg-purple-50 rounded-lg transition"
                              title="Editar"
                            >
                              <FiEdit2 size={16} />
                            </button>
                          )}

                          <button
                            onClick={() => openConfirm("delete", pedido)}
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

        <PedidoFormModal
          isOpen={isModalOpen}
          onClose={() => setIsModalOpen(false)}
          onSubmit={handleFormSubmit}
          initialData={editingPedido}
          isLoading={formSubmitting}
        />

        <ConfirmActionModalPedido
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
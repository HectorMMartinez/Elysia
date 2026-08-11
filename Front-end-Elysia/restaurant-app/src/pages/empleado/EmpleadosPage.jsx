import React, { useState, useEffect, useMemo } from "react";
import OwnerSidebar from "../../components/layout/OwnerSidebar";
import {
  obtenerEmpleados,
  crearEmpleado,
  editarEmpleado,
  eliminarEmpleado,
  activarEmpleado,
  inactivarEmpleado,
} from "../../services/empleadoService";
import { EmpleadoFormModal } from "../../components/empleado/EmpleadoFormModal";
import ConfirmActionModalEmpleado from "../../components/common/ConfirmActionModalEmpleado";
import {
  FiPlus,
  FiSearch,
  FiRefreshCw,
  FiAlertCircle,
  FiEdit2,
  FiTrash2,
  FiUserCheck,
  FiUserX,
  FiUsers,
  FiDollarSign,
} from "react-icons/fi";

export default function EmpleadosPage() {
  const [empleados, setEmpleados] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [searchTerm, setSearchTerm] = useState("");
  const [selectedEstado, setSelectedEstado] = useState("Todos");

  // Modal Formulario
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [editingEmpleado, setEditingEmpleado] = useState(null);
  const [formSubmitting, setFormSubmitting] = useState(false);

  // Modal de Confirmación
  const [confirmModal, setConfirmModal] = useState({
    isOpen: false,
    type: null, // "delete" | "activar" | "inactivar"
    empleado: null,
    loading: false,
  });


  const fetchEmpleados = async () => {
    try {
      setLoading(true);
      setError(null);
      const data = await obtenerEmpleados();
      setEmpleados(Array.isArray(data) ? data : []);
    } catch (err) {
      console.error("Error al cargar empleados:", err);
      setError(err.message || "No pudimos cargar la lista de empleados.");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchEmpleados();
  }, []);


  const stats = useMemo(() => {
    return {
      total: empleados.length,
      activos: empleados.filter((e) => e.isActive).length,
      inactivos: empleados.filter((e) => !e.isActive).length,
    };
  }, [empleados]);


  const filteredEmpleados = useMemo(() => {
    return empleados.filter((e) => {
      const fullName = `${e.firstName} ${e.lastName}`.toLowerCase();
      const matchSearch =
        fullName.includes(searchTerm.toLowerCase()) ||
        e.email?.toLowerCase().includes(searchTerm.toLowerCase()) ||
        e.phone?.toLowerCase().includes(searchTerm.toLowerCase()) ||
        e.nombrePuesto?.toLowerCase().includes(searchTerm.toLowerCase());

      const matchEstado =
        selectedEstado === "Todos" ||
        (selectedEstado === "Activos" && e.isActive) ||
        (selectedEstado === "Inactivos" && !e.isActive);

      return matchSearch && matchEstado;
    });
  }, [empleados, searchTerm, selectedEstado]);


  const handleOpenCreateModal = () => {
    setEditingEmpleado(null);
    setIsModalOpen(true);
  };

  const handleOpenEditModal = (empleado) => {
    setEditingEmpleado(empleado);
    setIsModalOpen(true);
  };

  const handleFormSubmit = async (formData) => {
    try {
      setFormSubmitting(true);

      if (editingEmpleado) {
        await editarEmpleado(editingEmpleado.id, formData);
      } else {
        await crearEmpleado(formData);
      }

      setIsModalOpen(false);
      fetchEmpleados();
    } catch (err) {
     
      throw err;
    } finally {
      setFormSubmitting(false);
    }
  };


  const openConfirm = (type, empleado) => {
    setConfirmModal({ isOpen: true, type, empleado, loading: false });
  };

  const closeConfirm = () => {
    setConfirmModal({
      isOpen: false,
      type: null,
      empleado: null,
      loading: false,
    });
  };

  const handleConfirmAction = async () => {
    const { type, empleado } = confirmModal;
    if (!empleado) return;

    try {
      setConfirmModal((prev) => ({ ...prev, loading: true }));

      switch (type) {
        case "delete":
          await eliminarEmpleado(empleado.id);
          break;
        case "activar":
          await activarEmpleado(empleado.id);
          break;
        case "inactivar":
          await inactivarEmpleado(empleado.id);
          break;
        default:
          break;
      }

      closeConfirm();
      fetchEmpleados();
    } catch (err) {
      console.error("Error en acción de empleado:", err);
      alert(err.message || "Ocurrió un error al procesar la acción.");
      setConfirmModal((prev) => ({ ...prev, loading: false }));
    }
  };


  const getStatusBadge = (isActive) => {
    return isActive ? (
      <span className="px-3 py-1 text-xs font-bold rounded-full bg-emerald-100 text-emerald-700">
        Activo
      </span>
    ) : (
      <span className="px-3 py-1 text-xs font-bold rounded-full bg-red-100 text-red-700">
        Inactivo
      </span>
    );
  };

  const formatCurrency = (value) => {
    return new Intl.NumberFormat("es-DO", {
      style: "currency",
      currency: "DOP",
      minimumFractionDigits: 0,
    }).format(value || 0);
  };

  const formatDate = (dateStr) => {
    if (!dateStr) return "—";
    return new Date(dateStr).toLocaleDateString("es-DO", {
      day: "2-digit",
      month: "short",
      year: "numeric",
    });
  };

  const getConfirmConfig = () => {
    const { type, empleado } = confirmModal;
    if (!empleado) return {};

    const nombre = `${empleado.firstName} ${empleado.lastName}`;

    const configs = {
      delete: {
        title: "Eliminar Empleado",
        subtitle: "Acción irreversible",
        message: `¿Estás seguro de eliminar a ${nombre}? Esta acción no se puede deshacer.`,
        confirmText: "Eliminar",
        confirmColor: "bg-red-600 hover:bg-red-700",
      },
      activar: {
        title: "Activar Empleado",
        subtitle: "Cambiar estado",
        message: `¿Deseas activar a ${nombre}? Podrá ser asignado a turnos nuevamente.`,
        confirmText: "Activar",
        confirmColor: "bg-emerald-600 hover:bg-emerald-700",
      },
      inactivar: {
        title: "Inactivar Empleado",
        subtitle: "Cambiar estado",
        message: `¿Deseas inactivar a ${nombre}? Mientras esté inactivo no podrá ser asignado a turnos.`,
        confirmText: "Inactivar",
        confirmColor: "bg-orange-600 hover:bg-orange-700",
      },
    };

    return configs[type] || {};
  };

  
  return (
    <OwnerSidebar>
      <div className="p-8 max-w-[1600px] mx-auto space-y-8 bg-slate-50/50 min-h-screen">
        {/* Header */}
        <div className="flex flex-col sm:flex-row justify-between items-start sm:items-center gap-4">
          <div>
            <h1 className="text-3xl font-bold text-slate-800">Empleados</h1>
            <p className="text-slate-500 mt-1">
              Gestiona el personal de tu restaurante.
            </p>
          </div>
          <button
            onClick={handleOpenCreateModal}
            className="flex items-center gap-2 px-5 py-3 bg-purple-600 text-white font-semibold text-sm rounded-xl hover:bg-purple-700 shadow-lg shadow-purple-200 transition-all"
          >
            <FiPlus size={18} />
            <span>Nuevo Empleado</span>
          </button>
        </div>

        {/* KPIs */}
        <div className="grid grid-cols-1 sm:grid-cols-3 gap-5">
          <div className="bg-white p-5 rounded-2xl border border-gray-100 shadow-sm">
            <div className="flex items-center gap-3">
              <div className="p-2.5 bg-purple-50 text-purple-600 rounded-xl">
                <FiUsers size={20} />
              </div>
              <div>
                <p className="text-xs font-medium text-slate-400">Total</p>
                <h3 className="text-2xl font-extrabold text-slate-800">
                  {stats.total}
                </h3>
              </div>
            </div>
          </div>
          <div className="bg-white p-5 rounded-2xl border border-gray-100 shadow-sm">
            <div className="flex items-center gap-3">
              <div className="p-2.5 bg-emerald-50 text-emerald-600 rounded-xl">
                <FiUserCheck size={20} />
              </div>
              <div>
                <p className="text-xs font-medium text-slate-400">Activos</p>
                <h3 className="text-2xl font-extrabold text-emerald-600">
                  {stats.activos}
                </h3>
              </div>
            </div>
          </div>
          <div className="bg-white p-5 rounded-2xl border border-gray-100 shadow-sm">
            <div className="flex items-center gap-3">
              <div className="p-2.5 bg-red-50 text-red-600 rounded-xl">
                <FiUserX size={20} />
              </div>
              <div>
                <p className="text-xs font-medium text-slate-400">Inactivos</p>
                <h3 className="text-2xl font-extrabold text-red-600">
                  {stats.inactivos}
                </h3>
              </div>
            </div>
          </div>
        </div>

        {/* Tabla */}
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
                placeholder="Buscar por nombre, correo, teléfono o puesto..."
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
                <option value="Todos">Todos</option>
                <option value="Activos">Activos</option>
                <option value="Inactivos">Inactivos</option>
              </select>

              <button
                onClick={fetchEmpleados}
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
              <p className="text-gray-500 font-semibold">Cargando empleados...</p>
            </div>
          ) : error ? (
            <div className="py-16 text-center space-y-4">
              <div className="p-4 bg-red-50 text-red-500 rounded-full w-fit mx-auto">
                <FiAlertCircle size={32} />
              </div>
              <p className="text-gray-800 font-bold">{error}</p>
              <button
                onClick={fetchEmpleados}
                className="px-6 py-2.5 bg-purple-600 text-white text-sm font-semibold rounded-xl hover:bg-purple-700"
              >
                Intentar de nuevo
              </button>
            </div>
          ) : filteredEmpleados.length === 0 ? (
            <div className="py-16 text-center text-gray-400 font-medium">
              No se encontraron empleados.
            </div>
          ) : (
            <div className="overflow-x-auto">
              <table className="w-full text-left">
                <thead>
                  <tr className="border-b border-gray-100 text-xs font-semibold text-slate-400 uppercase tracking-wider">
                    <th className="pb-4 pl-2">Empleado</th>
                    <th className="pb-4">Contacto</th>
                    <th className="pb-4">Puesto</th>
                    <th className="pb-4">Salario</th>
                    <th className="pb-4">Fecha Ingreso</th>
                    <th className="pb-4">Estado</th>
                    <th className="pb-4 text-right pr-2">Acciones</th>
                  </tr>
                </thead>
                <tbody className="divide-y divide-gray-50">
                  {filteredEmpleados.map((empleado) => (
                    <tr
                      key={empleado.id}
                      className="hover:bg-slate-50/50 transition"
                    >
                      <td className="py-4 pl-2">
                        <div>
                          <p className="font-semibold text-slate-800">
                            {empleado.firstName} {empleado.lastName}
                          </p>
                          <p className="text-xs text-slate-400">ID: {empleado.id}</p>
                        </div>
                      </td>
                      <td className="py-4">
                        <div className="text-sm">
                          <p className="text-slate-700">{empleado.email}</p>
                          <p className="text-slate-500 text-xs">{empleado.phone}</p>
                        </div>
                      </td>
                      <td className="py-4 text-sm text-slate-600">
                        {empleado.nombrePuesto || "—"}
                      </td>
                      <td className="py-4">
                        <div className="flex items-center gap-1.5 text-sm font-medium text-slate-700">
                          <FiDollarSign size={14} className="text-emerald-500" />
                          {formatCurrency(empleado.salary)}
                        </div>
                      </td>
                      <td className="py-4 text-sm text-slate-600">
                        {formatDate(empleado.hireDate)}
                      </td>
                      <td className="py-4">{getStatusBadge(empleado.isActive)}</td>
                      <td className="py-4 pr-2">
                        <div className="flex items-center justify-end gap-1.5">
                          {/* Activar / Inactivar */}
                          {empleado.isActive ? (
                            <button
                              onClick={() => openConfirm("inactivar", empleado)}
                              className="p-2 text-orange-600 hover:bg-orange-50 rounded-lg transition"
                              title="Inactivar"
                            >
                              <FiUserX size={16} />
                            </button>
                          ) : (
                            <button
                              onClick={() => openConfirm("activar", empleado)}
                              className="p-2 text-emerald-600 hover:bg-emerald-50 rounded-lg transition"
                              title="Activar"
                            >
                              <FiUserCheck size={16} />
                            </button>
                          )}

                          {/* Editar */}
                          <button
                            onClick={() => handleOpenEditModal(empleado)}
                            className="p-2 text-gray-500 hover:text-purple-600 hover:bg-purple-50 rounded-lg transition"
                            title="Editar"
                          >
                            <FiEdit2 size={16} />
                          </button>

                          {/* Eliminar */}
                          <button
                            onClick={() => openConfirm("delete", empleado)}
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
        <EmpleadoFormModal
          isOpen={isModalOpen}
          onClose={() => setIsModalOpen(false)}
          onSubmit={handleFormSubmit}
          initialData={editingEmpleado}
          isLoading={formSubmitting}
        />

        {/* Modal de Confirmación */}
        <ConfirmActionModalEmpleado
          isOpen={confirmModal.isOpen}
          loading={confirmModal.loading}
          onConfirm={handleConfirmAction}
          onCancel={closeConfirm}
          {...getConfirmConfig()}
        />
      </div>
    </OwnerSidebar>
  );
}
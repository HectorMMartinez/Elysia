import React, { useState, useEffect, useMemo } from "react";
import OwnerSidebar from "../../components/layout/OwnerSidebar";
import {
  obtenerTurnos,
  crearTurno,
  editarTurno,
  eliminarTurno,
  obtenerEmpleadosConTurnos,
  asociarEmpleadoATurno,
  desasociarEmpleadoDeTurno,
} from "../../services/shiftService";
import { TurnoFormModal } from "../../components/shift/TurnoFormModal";
import { AsociarTurnoModal } from "../../components/shift/AsociarTurnoModal";
import ConfirmActionModalEmpleado from "../../components/common/ConfirmActionModalEmpleado";
import {
  FiPlus,
  FiSearch,
  FiRefreshCw,
  FiAlertCircle,
  FiEdit2,
  FiTrash2,
  FiClock,
  FiUsers,
  FiLink,
  FiCalendar,
} from "react-icons/fi";

export default function TurnosPage() {
  // ===================== STATE TURNOS =====================
  const [turnos, setTurnos] = useState([]);
  const [loadingTurnos, setLoadingTurnos] = useState(true);
  const [errorTurnos, setErrorTurnos] = useState(null);

  // ===================== STATE ASOCIACIONES =====================
  const [asociaciones, setAsociaciones] = useState([]);
  const [loadingAsociaciones, setLoadingAsociaciones] = useState(true);
  const [errorAsociaciones, setErrorAsociaciones] = useState(null);

  // ===================== FILTROS =====================
  const [searchTurnos, setSearchTurnos] = useState("");
  const [searchAsociaciones, setSearchAsociaciones] = useState("");

  // ===================== MODALES =====================
  const [isTurnoModalOpen, setIsTurnoModalOpen] = useState(false);
  const [editingTurno, setEditingTurno] = useState(null);
  const [formSubmittingTurno, setFormSubmittingTurno] = useState(false);

  const [isAsociarModalOpen, setIsAsociarModalOpen] = useState(false);
  const [formSubmittingAsociar, setFormSubmittingAsociar] = useState(false);

  const [confirmModal, setConfirmModal] = useState({
    isOpen: false,
    type: null,
    item: null,
    loading: false,
  });

  // ===================== FETCH =====================
  const fetchTurnos = async () => {
    try {
      setLoadingTurnos(true);
      setErrorTurnos(null);
      const data = await obtenerTurnos();
      setTurnos(Array.isArray(data) ? data : []);
    } catch (err) {
      setErrorTurnos(err.message || "No se pudieron cargar los turnos.");
    } finally {
      setLoadingTurnos(false);
    }
  };

  const fetchAsociaciones = async () => {
    try {
      setLoadingAsociaciones(true);
      setErrorAsociaciones(null);
      const data = await obtenerEmpleadosConTurnos();
      setAsociaciones(Array.isArray(data) ? data : []);
    } catch (err) {
      setErrorAsociaciones(err.message || "No se pudieron cargar las asignaciones.");
    } finally {
      setLoadingAsociaciones(false);
    }
  };

  useEffect(() => {
    fetchTurnos();
    fetchAsociaciones();
  }, []);

  // ===================== FILTRADOS =====================
  const filteredTurnos = useMemo(() => {
    return turnos.filter((t) =>
      t.name?.toLowerCase().includes(searchTurnos.toLowerCase())
    );
  }, [turnos, searchTurnos]);

  const filteredAsociaciones = useMemo(() => {
    return asociaciones.filter(
      (a) =>
        a.nombreEmpleado?.toLowerCase().includes(searchAsociaciones.toLowerCase()) ||
        a.nombreShift?.toLowerCase().includes(searchAsociaciones.toLowerCase())
    );
  }, [asociaciones, searchAsociaciones]);

  // ===================== HANDLERS TURNOS =====================
  const handleOpenCreateTurno = () => {
    setEditingTurno(null);
    setIsTurnoModalOpen(true);
  };

  const handleOpenEditTurno = (turno) => {
    setEditingTurno(turno);
    setIsTurnoModalOpen(true);
  };

  const handleSubmitTurno = async (formData) => {
    try {
      setFormSubmittingTurno(true);
      if (editingTurno) {
        await editarTurno(editingTurno.id, formData);
      } else {
        await crearTurno(formData);
      }
      setIsTurnoModalOpen(false);
      fetchTurnos();
    } catch (err) {
      throw err;
    } finally {
      setFormSubmittingTurno(false);
    }
  };

  // ===================== HANDLERS ASOCIAR =====================
  const handleSubmitAsociar = async (formData) => {
    try {
      setFormSubmittingAsociar(true);
      await asociarEmpleadoATurno(formData);
      setIsAsociarModalOpen(false);
      fetchAsociaciones();
    } catch (err) {
      throw err;
    } finally {
      setFormSubmittingAsociar(false);
    }
  };

  // ===================== CONFIRMACIÓN =====================
  const openConfirm = (type, item) => {
    setConfirmModal({ isOpen: true, type, item, loading: false });
  };

  const closeConfirm = () => {
    setConfirmModal({ isOpen: false, type: null, item: null, loading: false });
  };

  const handleConfirmAction = async () => {
    const { type, item } = confirmModal;
    if (!item) return;

    try {
      setConfirmModal((prev) => ({ ...prev, loading: true }));

      if (type === "deleteTurno") {
        await eliminarTurno(item.id);
        fetchTurnos();
      } else if (type === "desasociar") {
        await desasociarEmpleadoDeTurno(item.id);
        fetchAsociaciones();
      }

      closeConfirm();
    } catch (err) {
      alert(err.message || "Ocurrió un error.");
      setConfirmModal((prev) => ({ ...prev, loading: false }));
    }
  };

  const getConfirmConfig = () => {
    const { type, item } = confirmModal;
    if (!item) return {};

    if (type === "deleteTurno") {
      return {
        title: "Eliminar Turno",
        subtitle: "Acción irreversible",
        message: `¿Estás seguro de eliminar el turno "${item.name}"? Esta acción no se puede deshacer.`,
        confirmText: "Eliminar",
        confirmColor: "bg-red-600 hover:bg-red-700",
      };
    }

    if (type === "desasociar") {
      return {
        title: "Desasignar Turno",
        subtitle: "Quitar asignación",
        message: `¿Deseas quitar el turno "${item.nombreShift}" a ${item.nombreEmpleado} el día ${item.workDate}?`,
        confirmText: "Desasignar",
        confirmColor: "bg-orange-600 hover:bg-orange-700",
      };
    }

    return {};
  };

  // ===================== HELPERS =====================
  const formatTime = (time) => {
    if (!time) return "—";
    return String(time).slice(0, 5);
  };

  const formatDate = (dateStr) => {
    if (!dateStr) return "—";
    return new Date(dateStr).toLocaleDateString("es-DO", {
      day: "2-digit",
      month: "short",
      year: "numeric",
    });
  };

  // ===================== RENDER =====================
  return (
    <OwnerSidebar>
      <div className="h-screen overflow-y-auto bg-gradient-to-br from-slate-50 via-white to-slate-100/80">
        <div className="p-6 lg:p-8 max-w-[1500px] mx-auto space-y-8 pb-20">
          
          {/* ===================== HEADER PRINCIPAL ===================== */}
          <div className="relative overflow-hidden rounded-3xl bg-gradient-to-r from-violet-600 via-purple-600 to-indigo-600 p-8 text-white shadow-xl shadow-purple-200/50">
            <div className="absolute inset-0 bg-[url('data:image/svg+xml;base64,PHN2ZyB3aWR0aD0iNjAiIGhlaWdodD0iNjAiIHZpZXdCb3g9IjAgMCA2MCA2MCIgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIj48ZyBmaWxsPSJub25lIiBmaWxsLXJ1bGU9ImV2ZW5vZGQiPjxnIGZpbGw9IiNmZmYiIGZpbGwtb3BhY2l0eT0iMC4wNSI+PHBhdGggZD0iTTM2IDM0djItSDI0di0yaDEyek0zNiAyNHYySDI0di0yaDEyeiIvPjwvZz48L2c+PC9zdmc+')] opacity-30"></div>
            <div className="relative z-10 flex flex-col sm:flex-row justify-between items-start sm:items-center gap-4">
              <div>
                <h1 className="text-3xl font-bold tracking-tight">Gestión de Turnos</h1>
                <p className="text-purple-100 mt-1.5 text-sm">
                  Administra los horarios y asigna a tu equipo de forma eficiente
                </p>
              </div>
              <div className="flex items-center gap-3 text-sm">
                <div className="flex items-center gap-2 bg-white/15 backdrop-blur-sm px-4 py-2 rounded-xl">
                  <FiClock size={16} />
                  <span className="font-medium">{turnos.length} turnos</span>
                </div>
                <div className="flex items-center gap-2 bg-white/15 backdrop-blur-sm px-4 py-2 rounded-xl">
                  <FiUsers size={16} />
                  <span className="font-medium">{asociaciones.length} asignaciones</span>
                </div>
              </div>
            </div>
          </div>

          {/* ===================== SECCIÓN 1: TURNOS ===================== */}
          <section className="space-y-5">
            <div className="flex flex-col sm:flex-row justify-between items-start sm:items-center gap-4">
              <div className="flex items-center gap-3">
                <div className="w-11 h-11 rounded-2xl bg-gradient-to-br from-violet-500 to-purple-600 flex items-center justify-center shadow-lg shadow-purple-200">
                  <FiClock className="text-white" size={20} />
                </div>
                <div>
                  <h2 className="text-xl font-bold text-slate-800">Turnos</h2>
                  <p className="text-sm text-slate-500">Define los horarios de trabajo disponibles</p>
                </div>
              </div>
              <button
                onClick={handleOpenCreateTurno}
                className="group flex items-center gap-2 px-5 py-2.5 bg-gradient-to-r from-violet-600 to-purple-600 text-white font-semibold text-sm rounded-xl hover:from-violet-700 hover:to-purple-700 shadow-lg shadow-purple-200/60 transition-all hover:shadow-purple-300/70 hover:-translate-y-0.5"
              >
                <FiPlus size={18} className="group-hover:rotate-90 transition-transform duration-300" />
                <span>Nuevo Turno</span>
              </button>
            </div>

            <div className="bg-white rounded-3xl border border-slate-200/60 shadow-sm shadow-slate-200/50 overflow-hidden">
              {/* Toolbar */}
              <div className="px-6 py-4 border-b border-slate-100 flex flex-col sm:flex-row justify-between gap-4 bg-slate-50/50">
                <div className="relative flex-1 max-w-sm">
                  <FiSearch className="absolute left-3.5 top-1/2 -translate-y-1/2 text-slate-400" size={17} />
                  <input
                    type="text"
                    placeholder="Buscar turno por nombre..."
                    value={searchTurnos}
                    onChange={(e) => setSearchTurnos(e.target.value)}
                    className="w-full pl-10 pr-4 py-2.5 rounded-xl border border-slate-200 bg-white text-sm focus:outline-none focus:ring-2 focus:ring-violet-500/20 focus:border-violet-400 transition"
                  />
                </div>
                <button
                  onClick={fetchTurnos}
                  className="flex items-center gap-2 px-4 py-2.5 rounded-xl border border-slate-200 text-slate-600 font-medium text-sm hover:bg-white hover:border-slate-300 transition-all"
                >
                  <FiRefreshCw size={15} />
                  Actualizar
                </button>
              </div>

              {/* Content */}
              <div className="p-1">
                {loadingTurnos ? (
                  <div className="py-20 text-center">
                    <div className="inline-flex items-center justify-center w-12 h-12 rounded-full bg-violet-50 mb-4">
                      <FiRefreshCw className="w-6 h-6 text-violet-600 animate-spin" />
                    </div>
                    <p className="text-slate-500 font-medium">Cargando turnos...</p>
                  </div>
                ) : errorTurnos ? (
                  <div className="py-16 text-center space-y-4">
                    <div className="inline-flex items-center justify-center w-14 h-14 rounded-full bg-red-50 text-red-500">
                      <FiAlertCircle size={28} />
                    </div>
                    <p className="text-slate-800 font-semibold">{errorTurnos}</p>
                    <button
                      onClick={fetchTurnos}
                      className="px-5 py-2.5 bg-violet-600 text-white text-sm font-semibold rounded-xl hover:bg-violet-700 transition"
                    >
                      Reintentar
                    </button>
                  </div>
                ) : filteredTurnos.length === 0 ? (
                  <div className="py-16 text-center">
                    <div className="inline-flex items-center justify-center w-14 h-14 rounded-full bg-slate-100 text-slate-400 mb-4">
                      <FiClock size={26} />
                    </div>
                    <p className="text-slate-500 font-medium">No hay turnos registrados</p>
                    <p className="text-sm text-slate-400 mt-1">Crea tu primer turno para comenzar</p>
                  </div>
                ) : (
                  <div className="overflow-x-auto">
                    <table className="w-full text-left">
                      <thead>
                        <tr className="text-[11px] font-semibold text-slate-400 uppercase tracking-wider">
                          <th className="px-6 py-3.5">Nombre del turno</th>
                          <th className="px-6 py-3.5">Horario</th>
                          <th className="px-6 py-3.5 text-right">Acciones</th>
                        </tr>
                      </thead>
                      <tbody className="divide-y divide-slate-100">
                        {filteredTurnos.map((turno) => (
                          <tr
                            key={turno.id}
                            className="group hover:bg-violet-50/40 transition-colors"
                          >
                            <td className="px-6 py-4">
                              <div className="flex items-center gap-3">
                                <div className="w-9 h-9 rounded-xl bg-gradient-to-br from-violet-100 to-purple-100 flex items-center justify-center text-violet-600">
                                  <FiClock size={16} />
                                </div>
                                <span className="font-semibold text-slate-800">
                                  {turno.name}
                                </span>
                              </div>
                            </td>
                            <td className="px-6 py-4">
                              <div className="inline-flex items-center gap-2 px-3 py-1.5 rounded-lg bg-slate-100 text-sm font-medium text-slate-700">
                                <span>{formatTime(turno.startTime)}</span>
                                <span className="text-slate-400">→</span>
                                <span>{formatTime(turno.endTime)}</span>
                              </div>
                            </td>
                            <td className="px-6 py-4">
                              <div className="flex items-center justify-end gap-1 opacity-70 group-hover:opacity-100 transition-opacity">
                                <button
                                  onClick={() => handleOpenEditTurno(turno)}
                                  className="p-2 text-slate-500 hover:text-violet-600 hover:bg-violet-100 rounded-lg transition"
                                  title="Editar"
                                >
                                  <FiEdit2 size={16} />
                                </button>
                                <button
                                  onClick={() => openConfirm("deleteTurno", turno)}
                                  className="p-2 text-slate-500 hover:text-red-600 hover:bg-red-50 rounded-lg transition"
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
            </div>
          </section>

          {/* ===================== SECCIÓN 2: ASIGNACIONES ===================== */}
          <section className="space-y-5">
            <div className="flex flex-col sm:flex-row justify-between items-start sm:items-center gap-4">
              <div className="flex items-center gap-3">
                <div className="w-11 h-11 rounded-2xl bg-gradient-to-br from-emerald-500 to-teal-600 flex items-center justify-center shadow-lg shadow-emerald-200">
                  <FiUsers className="text-white" size={20} />
                </div>
                <div>
                  <h2 className="text-xl font-bold text-slate-800">Asignaciones</h2>
                  <p className="text-sm text-slate-500">
                    Asigna empleados activos a turnos específicos
                  </p>
                </div>
              </div>
              <button
                onClick={() => setIsAsociarModalOpen(true)}
                className="group flex items-center gap-2 px-5 py-2.5 bg-gradient-to-r from-emerald-600 to-teal-600 text-white font-semibold text-sm rounded-xl hover:from-emerald-700 hover:to-teal-700 shadow-lg shadow-emerald-200/60 transition-all hover:shadow-emerald-300/70 hover:-translate-y-0.5"
              >
                <FiLink size={18} />
                <span>Asignar Empleado</span>
              </button>
            </div>

            <div className="bg-white rounded-3xl border border-slate-200/60 shadow-sm shadow-slate-200/50 overflow-hidden">
              {/* Toolbar */}
              <div className="px-6 py-4 border-b border-slate-100 flex flex-col sm:flex-row justify-between gap-4 bg-slate-50/50">
                <div className="relative flex-1 max-w-sm">
                  <FiSearch className="absolute left-3.5 top-1/2 -translate-y-1/2 text-slate-400" size={17} />
                  <input
                    type="text"
                    placeholder="Buscar por empleado o turno..."
                    value={searchAsociaciones}
                    onChange={(e) => setSearchAsociaciones(e.target.value)}
                    className="w-full pl-10 pr-4 py-2.5 rounded-xl border border-slate-200 bg-white text-sm focus:outline-none focus:ring-2 focus:ring-emerald-500/20 focus:border-emerald-400 transition"
                  />
                </div>
                <button
                  onClick={fetchAsociaciones}
                  className="flex items-center gap-2 px-4 py-2.5 rounded-xl border border-slate-200 text-slate-600 font-medium text-sm hover:bg-white hover:border-slate-300 transition-all"
                >
                  <FiRefreshCw size={15} />
                  Actualizar
                </button>
              </div>

              {/* Content */}
              <div className="p-1">
                {loadingAsociaciones ? (
                  <div className="py-20 text-center">
                    <div className="inline-flex items-center justify-center w-12 h-12 rounded-full bg-emerald-50 mb-4">
                      <FiRefreshCw className="w-6 h-6 text-emerald-600 animate-spin" />
                    </div>
                    <p className="text-slate-500 font-medium">Cargando asignaciones...</p>
                  </div>
                ) : errorAsociaciones ? (
                  <div className="py-16 text-center space-y-4">
                    <div className="inline-flex items-center justify-center w-14 h-14 rounded-full bg-red-50 text-red-500">
                      <FiAlertCircle size={28} />
                    </div>
                    <p className="text-slate-800 font-semibold">{errorAsociaciones}</p>
                    <button
                      onClick={fetchAsociaciones}
                      className="px-5 py-2.5 bg-emerald-600 text-white text-sm font-semibold rounded-xl hover:bg-emerald-700 transition"
                    >
                      Reintentar
                    </button>
                  </div>
                ) : filteredAsociaciones.length === 0 ? (
                  <div className="py-16 text-center">
                    <div className="inline-flex items-center justify-center w-14 h-14 rounded-full bg-slate-100 text-slate-400 mb-4">
                      <FiUsers size={26} />
                    </div>
                    <p className="text-slate-500 font-medium">No hay asignaciones registradas</p>
                    <p className="text-sm text-slate-400 mt-1">
                      Asigna empleados a turnos para empezar a planificar
                    </p>
                  </div>
                ) : (
                  <div className="overflow-x-auto">
                    <table className="w-full text-left">
                      <thead>
                        <tr className="text-[11px] font-semibold text-slate-400 uppercase tracking-wider">
                          <th className="px-6 py-3.5">Empleado</th>
                          <th className="px-6 py-3.5">Turno</th>
                          <th className="px-6 py-3.5">Fecha</th>
                          <th className="px-6 py-3.5 text-right">Acciones</th>
                        </tr>
                      </thead>
                      <tbody className="divide-y divide-slate-100">
                        {filteredAsociaciones.map((asoc) => (
                          <tr
                            key={asoc.id}
                            className="group hover:bg-emerald-50/40 transition-colors"
                          >
                            <td className="px-6 py-4">
                              <div className="flex items-center gap-3">
                                <div className="w-9 h-9 rounded-full bg-gradient-to-br from-emerald-100 to-teal-100 flex items-center justify-center text-emerald-700 font-semibold text-sm">
                                  {asoc.nombreEmpleado?.charAt(0)?.toUpperCase() || "?"}
                                </div>
                                <span className="font-semibold text-slate-800">
                                  {asoc.nombreEmpleado}
                                </span>
                              </div>
                            </td>
                            <td className="px-6 py-4">
                              <span className="inline-flex items-center gap-1.5 px-3 py-1 rounded-lg bg-violet-50 text-violet-700 text-sm font-medium">
                                <FiClock size={13} />
                                {asoc.nombreShift}
                              </span>
                            </td>
                            <td className="px-6 py-4">
                              <div className="flex items-center gap-2 text-sm text-slate-600">
                                <FiCalendar size={14} className="text-slate-400" />
                                {formatDate(asoc.workDate)}
                              </div>
                            </td>
                            <td className="px-6 py-4">
                              <div className="flex items-center justify-end opacity-70 group-hover:opacity-100 transition-opacity">
                                <button
                                  onClick={() => openConfirm("desasociar", asoc)}
                                  className="p-2 text-slate-500 hover:text-orange-600 hover:bg-orange-50 rounded-lg transition"
                                  title="Desasignar"
                                >
                                  <FiLink size={16} />
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
            </div>
          </section>
        </div>
      </div>

      {/* ===================== MODALES ===================== */}
      <TurnoFormModal
        isOpen={isTurnoModalOpen}
        onClose={() => setIsTurnoModalOpen(false)}
        onSubmit={handleSubmitTurno}
        initialData={editingTurno}
        isLoading={formSubmittingTurno}
      />

      <AsociarTurnoModal
        isOpen={isAsociarModalOpen}
        onClose={() => setIsAsociarModalOpen(false)}
        onSubmit={handleSubmitAsociar}
        isLoading={formSubmittingAsociar}
      />

      <ConfirmActionModalEmpleado
        isOpen={confirmModal.isOpen}
        loading={confirmModal.loading}
        onConfirm={handleConfirmAction}
        onCancel={closeConfirm}
        {...getConfirmConfig()}
      />
    </OwnerSidebar>
  );
}
import React, { useState, useEffect, useMemo } from "react";
import OwnerSidebar from "../../components/layout/OwnerSidebar";
import { mesaService } from "../../services/mesaService";
import { MesaFormModal } from "../../components/tables/MesaFormModal";
import { ConfirmModal } from "../../components/common/ConfirmModal";
import { 
  FiPlus, FiSearch, FiRefreshCw, FiUsers, FiAlertCircle, 
  FiCheckCircle, FiClock, FiEdit2, FiTrash2 
} from "react-icons/fi";

const API_BASE_URL = "https://localhost:7108"; // URL de tu backend .NET

export const TablesPage = () => {
  const [mesas, setMesas] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [searchTerm, setSearchTerm] = useState("");
  const [selectedEstado, setSelectedEstado] = useState("Todos");
  
  // Estados para el Modal Formulario (Crear/Editar)
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [editingMesa, setEditingMesa] = useState(null);
  const [formSubmitting, setFormSubmitting] = useState(false);

  // Estados para el Modal de Confirmación de Eliminación
  const [deleteModalOpen, setDeleteModalOpen] = useState(false);
  const [tableToDelete, setTableToDelete] = useState(null);
  const [isDeleting, setIsDeleting] = useState(false);

  // Helper para construir la URL completa de la imagen
  const getImageUrl = (imagen) => {
    if (!imagen) return null;
    if (imagen.startsWith("http://") || imagen.startsWith("https://") || imagen.startsWith("data:")) {
      return imagen;
    }
    const cleanPath = imagen.startsWith("/") ? imagen : `/${imagen}`;
    return `${API_BASE_URL}${cleanPath}`;
  };

  // Cargar datos desde la API
  const fetchMesas = async () => {
    try {
      setLoading(true);
      setError(null);
      const data = await mesaService.getAll();
      setMesas(Array.isArray(data) ? data : []);
    } catch (err) {
      console.error("Error al cargar mesas:", err);
      setError("No pudimos cargar la lista de mesas.");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchMesas();
  }, []);

  // Cálculo de KPIs superiores
  const stats = useMemo(() => {
    return {
      total: mesas.length,
      disponibles: mesas.filter(m => m.estado?.toLowerCase() === "disponible").length,
      ocupadas: mesas.filter(m => m.estado?.toLowerCase() === "ocupada").length,
      reservadas: mesas.filter(m => m.estado?.toLowerCase() === "reservada").length,
    };
  }, [mesas]);

  // Filtrado dinámico
  const filteredMesas = useMemo(() => {
    return mesas.filter(m => {
      const matchSearch = m.nombre?.toLowerCase().includes(searchTerm.toLowerCase()) ||
                          m.descripcion?.toLowerCase().includes(searchTerm.toLowerCase());
      
      const matchEstado = selectedEstado === "Todos" || 
                          m.estado?.toLowerCase() === selectedEstado.toLowerCase();

      return matchSearch && matchEstado;
    });
  }, [mesas, searchTerm, selectedEstado]);

  // Manejo de Modal Crear/Editar
  const handleOpenCreateModal = () => {
    setEditingMesa(null);
    setIsModalOpen(true);
  };

  const handleOpenEditModal = (mesa) => {
    setEditingMesa(mesa);
    setIsModalOpen(true);
  };

  // Abre el modal personalizado de eliminación
  const handleOpenDeleteModal = (mesa) => {
    setTableToDelete(mesa);
    setDeleteModalOpen(true);
  };

  // Ejecuta la eliminación tras la confirmación en el modal
  const handleConfirmDelete = async () => {
    if (!tableToDelete) return;
    
    try {
      setIsDeleting(true);
      await mesaService.delete(tableToDelete.id);
      setDeleteModalOpen(false);
      setTableToDelete(null);
      fetchMesas();
    } catch (err) {
      console.error("Error al eliminar la mesa:", err);
      alert("No se pudo eliminar la mesa.");
    } finally {
      setIsDeleting(false);
    }
  };

  const handleFormSubmit = async (formData) => {
    try {
      setFormSubmitting(true);
      if (editingMesa) {
        await mesaService.update(editingMesa.id, formData);
      } else {
        await mesaService.create(formData);
      }
      setIsModalOpen(false);
      fetchMesas();
    } catch (err) {
      console.error("Error guardando mesa:", err);
      alert("Ocurrió un error al guardar los datos de la mesa.");
    } finally {
      setFormSubmitting(false);
    }
  };

  const getStatusBadge = (estado) => {
    switch (estado?.toLowerCase()) {
      case "disponible":
        return <span className="px-3 py-1 bg-emerald-100 text-emerald-700 text-xs font-bold rounded-full">Disponible</span>;
      case "ocupada":
        return <span className="px-3 py-1 bg-amber-100 text-amber-700 text-xs font-bold rounded-full">Ocupada</span>;
      case "reservada":
        return <span className="px-3 py-1 bg-blue-100 text-blue-700 text-xs font-bold rounded-full">Reservada</span>;
      default:
        return <span className="px-3 py-1 bg-gray-100 text-gray-700 text-xs font-bold rounded-full">{estado || "N/A"}</span>;
    }
  };

  return (
    <OwnerSidebar>
      <div className="p-8 max-w-[1600px] mx-auto space-y-8 bg-slate-50/50 min-h-screen">
        
        {/* Header */}
        <div className="flex flex-col sm:flex-row justify-between items-start sm:items-center gap-4">
          <div>
            <h1 className="text-3xl font-bold text-slate-800">Mesas</h1>
            <p className="text-slate-500 mt-1">Gestiona la distribución, disponibilidad y estado de las mesas.</p>
          </div>
          <button
            onClick={handleOpenCreateModal}
            className="flex items-center gap-2 px-5 py-3 bg-purple-600 text-white font-semibold text-sm rounded-xl hover:bg-purple-700 shadow-lg shadow-purple-200 transition-all"
          >
            <FiPlus size={18} />
            <span>Agregar mesa</span>
          </button>
        </div>

        {/* Tarjetas de Métricas */}
        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-5">
          <div className="bg-white p-5 rounded-2xl border border-gray-100 shadow-sm flex items-center justify-between">
            <div>
              <p className="text-xs font-medium text-slate-400">Total de mesas</p>
              <h3 className="text-2xl font-extrabold text-slate-800 mt-1">{stats.total}</h3>
            </div>
            <div className="p-3 bg-purple-50 text-purple-600 rounded-xl">
              <FiUsers size={24} />
            </div>
          </div>

          <div className="bg-white p-5 rounded-2xl border border-gray-100 shadow-sm flex items-center justify-between">
            <div>
              <p className="text-xs font-medium text-slate-400">Disponibles</p>
              <h3 className="text-2xl font-extrabold text-slate-800 mt-1">{stats.disponibles}</h3>
            </div>
            <div className="p-3 bg-emerald-50 text-emerald-600 rounded-xl">
              <FiCheckCircle size={24} />
            </div>
          </div>

          <div className="bg-white p-5 rounded-2xl border border-gray-100 shadow-sm flex items-center justify-between">
            <div>
              <p className="text-xs font-medium text-slate-400">Ocupadas</p>
              <h3 className="text-2xl font-extrabold text-slate-800 mt-1">{stats.ocupadas}</h3>
            </div>
            <div className="p-3 bg-amber-50 text-amber-600 rounded-xl">
              <FiClock size={24} />
            </div>
          </div>

          <div className="bg-white p-5 rounded-2xl border border-gray-100 shadow-sm flex items-center justify-between">
            <div>
              <p className="text-xs font-medium text-slate-400">Reservadas</p>
              <h3 className="text-2xl font-extrabold text-slate-800 mt-1">{stats.reservadas}</h3>
            </div>
            <div className="p-3 bg-blue-50 text-blue-600 rounded-xl">
              <FiAlertCircle size={24} />
            </div>
          </div>
        </div>

        {/* Buscador y Listado Grid */}
        <div className="bg-white rounded-3xl border border-gray-100 shadow-sm p-6 space-y-6">
          
          {/* Controles de Filtro */}
          <div className="flex flex-col sm:flex-row justify-between gap-4">
            <div className="relative flex-1 max-w-md">
              <FiSearch className="absolute left-4 top-3.5 text-gray-400" size={18} />
              <input
                type="text"
                placeholder="Buscar por nombre o descripción..."
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
                <option value="Disponible">Disponible</option>
                <option value="Ocupada">Ocupada</option>
                <option value="Reservada">Reservada</option>
              </select>

              <button
                onClick={fetchMesas}
                className="flex items-center gap-2 px-4 py-2.5 rounded-xl border border-gray-200 text-gray-600 font-semibold text-sm hover:bg-gray-50 transition-colors"
              >
                <FiRefreshCw size={16} />
                <span>Actualizar</span>
              </button>
            </div>
          </div>

          {/* Grid de Mesas o Loader */}
          {loading ? (
            <div className="py-20 text-center space-y-3">
              <FiRefreshCw className="w-8 h-8 text-purple-600 animate-spin mx-auto" />
              <p className="text-gray-500 font-semibold">Cargando mesas...</p>
            </div>
          ) : error ? (
            <div className="py-16 text-center space-y-4">
              <div className="p-4 bg-red-50 text-red-500 rounded-full w-fit mx-auto">
                <FiAlertCircle size={32} />
              </div>
              <p className="text-gray-800 font-bold">{error}</p>
              <button
                onClick={fetchMesas}
                className="px-6 py-2.5 bg-purple-600 text-white text-sm font-semibold rounded-xl hover:bg-purple-700 shadow-md"
              >
                Intentar de nuevo
              </button>
            </div>
          ) : filteredMesas.length === 0 ? (
            <div className="py-16 text-center text-gray-400 font-medium">
              No se encontraron mesas registradas.
            </div>
          ) : (
            <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6">
              {filteredMesas.map((mesa) => {
                const imgSource = getImageUrl(mesa.imagen);
                return (
                  <div
                    key={mesa.id}
                    className="bg-white rounded-2xl border border-gray-100 overflow-hidden shadow-sm hover:shadow-md transition-shadow group flex flex-col justify-between"
                  >
                    <div>
                      <div className="h-40 bg-slate-100 relative overflow-hidden">
                        {imgSource ? (
                          <img
                            src={imgSource}
                            alt={mesa.nombre}
                            className="w-full h-full object-cover group-hover:scale-105 transition-transform duration-300"
                            onError={(e) => {
                              e.target.style.display = 'none';
                            }}
                          />
                        ) : (
                          <div className="w-full h-full flex items-center justify-center text-slate-300 font-semibold">
                            Sin imagen
                          </div>
                        )}
                        <div className="absolute top-3 right-3">
                          {getStatusBadge(mesa.estado)}
                        </div>
                      </div>

                      <div className="p-5 space-y-2">
                        <h4 className="text-lg font-bold text-gray-800">{mesa.nombre}</h4>
                        <p className="text-xs text-gray-500 line-clamp-2">{mesa.descripcion}</p>
                        <div className="pt-2 flex items-center gap-2 text-xs font-semibold text-gray-600">
                          <FiUsers className="text-purple-600" />
                          <span>Capacidad: {mesa.capacidad} personas</span>
                        </div>
                      </div>
                    </div>

                    <div className="p-4 border-t border-gray-50 bg-slate-50/50 flex justify-end gap-2">
                      <button
                        onClick={() => handleOpenEditModal(mesa)}
                        className="p-2 text-gray-500 hover:text-purple-600 hover:bg-purple-50 rounded-lg transition-colors"
                        title="Editar mesa"
                      >
                        <FiEdit2 size={16} />
                      </button>
                      <button
                        onClick={() => handleOpenDeleteModal(mesa)}
                        className="p-2 text-gray-500 hover:text-red-600 hover:bg-red-50 rounded-lg transition-colors"
                        title="Eliminar mesa"
                      >
                        <FiTrash2 size={16} />
                      </button>
                    </div>
                  </div>
                );
              })}
            </div>
          )}

        </div>

        {/* Modal Formulario (Crear/Editar) */}
        <MesaFormModal
          isOpen={isModalOpen}
          onClose={() => setIsModalOpen(false)}
          onSubmit={handleFormSubmit}
          initialData={editingMesa}
          isLoading={formSubmitting}
        />

        {/* Modal de Confirmación para Eliminar */}
        <ConfirmModal
          isOpen={deleteModalOpen}
          onClose={() => setDeleteModalOpen(false)}
          onConfirm={handleConfirmDelete}
          item={tableToDelete ? {
            nombre: tableToDelete.nombre,
            descripcion: tableToDelete.descripcion,
            imagen: getImageUrl(tableToDelete.imagen)
          } : null}
          isLoading={isDeleting}
        />

      </div>
    </OwnerSidebar>
  );
};
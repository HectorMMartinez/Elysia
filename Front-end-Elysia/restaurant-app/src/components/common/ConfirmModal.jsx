import React from "react";
import { FiTrash2, FiX, FiAlertTriangle } from "react-icons/fi";

export const ConfirmModal = ({ isOpen, onClose, onConfirm, item, isLoading }) => {
  if (!isOpen) return null;

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-slate-900/50 backdrop-blur-sm p-4 animate-in fade-in duration-200">
      <div className="bg-white rounded-3xl max-w-md w-full shadow-2xl overflow-hidden border border-slate-100">
        
        {/* Encabezado con ícono de bote de basura y botón de cerrar */}
        <div className="p-6 border-b border-slate-100 flex items-start justify-between">
          <div className="flex items-center gap-4">
            <div className="p-3 bg-red-100/80 text-red-600 rounded-2xl shrink-0">
              <FiTrash2 size={24} />
            </div>
            <div>
              <h3 className="text-xl font-bold text-slate-800">Eliminar mesa</h3>
              <p className="text-xs text-slate-400 mt-0.5">Esta acción no se puede deshacer.</p>
            </div>
          </div>
          <button 
            onClick={onClose} 
            disabled={isLoading}
            className="text-slate-400 hover:text-slate-600 transition-colors p-1 rounded-lg"
          >
            <FiX size={20} />
          </button>
        </div>

        {/* Creador del cuerpo */}
        <div className="p-6 space-y-4">
          <p className="text-sm font-medium text-slate-600">
            ¿Estás seguro de que deseas eliminar la siguiente mesa?
          </p>

          {/* Tarjeta de vista previa de la mesa */}
          {item && (
            <div className="flex items-center gap-3 p-3 bg-slate-50 border border-slate-100 rounded-2xl">
              {item.imagen ? (
                <img 
                  src={item.imagen} 
                  alt={item.nombre} 
                  className="w-14 h-14 rounded-xl object-cover shrink-0" 
                />
              ) : (
                <div className="w-14 h-14 bg-slate-200 rounded-xl flex items-center justify-center text-xs font-bold text-slate-400 shrink-0">
                  Sin foto
                </div>
              )}
              <div className="truncate">
                <h4 className="font-bold text-slate-800 text-base truncate">{item.nombre}</h4>
                <p className="text-xs text-slate-400 truncate">{item.descripcion || "Sin descripción"}</p>
              </div>
            </div>
          )}

          {/* Nota de advertencia sencilla orientada al usuario */}
          <div className="p-3.5 bg-amber-50/80 border border-amber-200/60 rounded-2xl flex items-start gap-3">
            <FiAlertTriangle className="text-amber-600 shrink-0 mt-0.5" size={18} />
            <p className="text-xs font-medium text-amber-800 leading-relaxed">
              Si esta mesa tiene órdenes activas o reservas asociadas, no se podrá eliminar hasta que se completen.
            </p>
          </div>
        </div>

        {/* Acciones del pie de página */}
        <div className="p-6 pt-2 flex items-center justify-end gap-3 bg-white">
          <button
            type="button"
            onClick={onClose}
            disabled={isLoading}
            className="px-5 py-2.5 rounded-xl border border-slate-200 text-slate-700 text-sm font-bold hover:bg-slate-50 transition-colors disabled:opacity-50"
          >
            Cancelar
          </button>
          <button
            type="button"
            onClick={onConfirm}
            disabled={isLoading}
            className="flex items-center gap-2 px-5 py-2.5 rounded-xl bg-red-600 text-white text-sm font-bold hover:bg-red-700 shadow-md shadow-red-200 transition-all disabled:opacity-50"
          >
            <FiTrash2 size={16} />
            <span>{isLoading ? "Eliminando..." : "Eliminar mesa"}</span>
          </button>
        </div>

      </div>
    </div>
  );
};
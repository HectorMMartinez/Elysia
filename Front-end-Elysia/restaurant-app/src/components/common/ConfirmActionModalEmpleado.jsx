// components/common/ConfirmActionModalEmpleado.jsx
import React from "react";
import { FiAlertTriangle, FiX } from "react-icons/fi";

export default function ConfirmActionModalEmpleado({
  isOpen,
  loading = false,
  title,
  subtitle,
  message,
  confirmText = "Confirmar",
  confirmColor = "bg-red-600 hover:bg-red-700",
  onConfirm,
  onCancel,
}) {
  if (!isOpen) return null;

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center p-4 bg-black/50 backdrop-blur-sm">
      <div className="bg-white rounded-2xl shadow-2xl w-full max-w-md overflow-hidden">
        <div className="p-6">
          <div className="flex items-start gap-4">
            <div className="p-3 bg-amber-50 text-amber-600 rounded-xl">
              <FiAlertTriangle size={24} />
            </div>
            <div className="flex-1">
              <h3 className="text-lg font-bold text-slate-800">{title}</h3>
              {subtitle && (
                <p className="text-sm text-slate-500 mt-0.5">{subtitle}</p>
              )}
              <p className="text-sm text-slate-600 mt-3 leading-relaxed">
                {message}
              </p>
            </div>
            <button
              onClick={onCancel}
              disabled={loading}
              className="p-1 text-gray-400 hover:text-gray-600"
            >
              <FiX size={18} />
            </button>
          </div>
        </div>

        <div className="flex gap-3 p-6 pt-0">
          <button
            onClick={onCancel}
            disabled={loading}
            className="flex-1 px-4 py-2.5 rounded-xl border border-gray-200 text-slate-600 font-semibold text-sm hover:bg-gray-50 transition disabled:opacity-50"
          >
            Cancelar
          </button>
          <button
            onClick={onConfirm}
            disabled={loading}
            className={`flex-1 px-4 py-2.5 rounded-xl text-white font-semibold text-sm transition disabled:opacity-50 flex items-center justify-center gap-2 ${confirmColor}`}
          >
            {loading ? (
              <>
                <span className="w-4 h-4 border-2 border-white/30 border-t-white rounded-full animate-spin" />
                Procesando...
              </>
            ) : (
              confirmText
            )}
          </button>
        </div>
      </div>
    </div>
  );
}
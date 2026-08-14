import React, { useState, useEffect } from "react";
import { useForm } from "react-hook-form";
import { FiX, FiUpload, FiGrid } from "react-icons/fi";

export const MesaFormModal = ({ isOpen, onClose, onSubmit, initialData = null, isLoading = false }) => {
  const [imagePreview, setImagePreview] = useState(null);

  const {
    register,
    handleSubmit,
    reset,
    watch,
    formState: { errors }
  } = useForm({
    defaultValues: {
      nombre: "",
      descripcion: "",
      capacidad: 2,
      estado: "Disponible"
    }
  });

  const selectedImage = watch("imagen");

  // Rellenar o limpiar formulario al abrir/cerrar o cambiar datos
  useEffect(() => {
    if (initialData) {
      reset({
        nombre: initialData.nombre || "",
        descripcion: initialData.descripcion || "",
        capacidad: initialData.capacidad || 2,
        estado: initialData.estado || "Disponible"
      });
      setImagePreview(initialData.imagen || null);
    } else {
      reset({
        nombre: "",
        descripcion: "",
        capacidad: 2,
        estado: "Disponible"
      });
      setImagePreview(null);
    }
  }, [initialData, reset, isOpen]);

  // Previsualización de la imagen cargada localmente
  useEffect(() => {
    if (selectedImage && selectedImage[0] instanceof File) {
      const reader = new FileReader();
      reader.onloadend = () => {
        setImagePreview(reader.result);
      };
      reader.readAsDataURL(selectedImage[0]);
    }
  }, [selectedImage]);

  if (!isOpen) return null;

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-slate-900/40 backdrop-blur-sm p-4 overflow-y-auto">
      <div className="bg-white rounded-2xl shadow-xl w-full max-w-2xl overflow-hidden my-8 transform transition-all">
        
        {/* Encabezado */}
        <div className="flex items-center justify-between p-6 border-b border-gray-100">
          <div>
            <h3 className="text-xl font-bold text-gray-800">
              {initialData ? "Editar mesa" : "Agregar mesa"}
            </h3>
            <p className="text-sm text-gray-500 mt-1">
              {initialData ? "Modifica los parámetros de la mesa seleccionada." : "Completa los datos para registrar una nueva mesa."}
            </p>
          </div>
          <button
            type="button"
            onClick={onClose}
            className="p-2 text-gray-400 hover:text-gray-600 rounded-lg hover:bg-gray-100 transition-colors"
          >
            <FiX size={20} />
          </button>
        </div>

        {/* Formulario */}
        <form onSubmit={handleSubmit(onSubmit)} className="p-6 space-y-5">
          <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
            
            {/* Nombre */}
            <div>
              <label className="block text-xs font-semibold text-gray-700 uppercase mb-1">
                Nombre / Número de Mesa <span className="text-red-500">*</span>
              </label>
              <input
                type="text"
                placeholder="Ej. Mesa 01 - Terraza"
                className={`w-full px-4 py-2.5 rounded-xl border text-sm focus:outline-none focus:ring-2 focus:ring-purple-500/20 ${
                  errors.nombre ? "border-red-400 bg-red-50/30" : "border-gray-200"
                }`}
                {...register("nombre", { required: "El nombre de la mesa es requerido" })}
              />
              {errors.nombre && (
                <span className="text-xs text-red-500 mt-1 block">{errors.nombre.message}</span>
              )}
            </div>

            {/* Capacidad */}
            <div>
              <label className="block text-xs font-semibold text-gray-700 uppercase mb-1">
                Capacidad (Personas)
              </label>
              <input
                type="number"
                min="1"
                placeholder="2"
                className="w-full px-4 py-2.5 rounded-xl border border-gray-200 text-sm focus:outline-none focus:ring-2 focus:ring-purple-500/20"
                {...register("capacidad", { valueAsNumber: true })}
              />
            </div>

            {/* Estado */}
            <div>
              <label className="block text-xs font-semibold text-gray-700 uppercase mb-1">
                Estado <span className="text-red-500">*</span>
              </label>
              <select
                className="w-full px-4 py-2.5 rounded-xl border border-gray-200 text-sm focus:outline-none focus:ring-2 focus:ring-purple-500/20 bg-white"
                {...register("estado", { required: true })}
              >
                <option value="Disponible">Disponible</option>
                <option value="Ocupada">Ocupada</option>
                <option value="Reservada">Reservada</option>
                <option value="Mantenimiento">Mantenimiento</option>
              </select>
            </div>

            {/* Descripción */}
            <div className="md:col-span-2">
              <label className="block text-xs font-semibold text-gray-700 uppercase mb-1">
                Descripción <span className="text-red-500">*</span>
              </label>
              <textarea
                rows="3"
                placeholder="Describe la ubicación o detalles de la mesa..."
                className={`w-full px-4 py-2.5 rounded-xl border text-sm focus:outline-none focus:ring-2 focus:ring-purple-500/20 resize-none ${
                  errors.descripcion ? "border-red-400 bg-red-50/30" : "border-gray-200"
                }`}
                {...register("descripcion", { required: "La descripción es requerida" })}
              ></textarea>
              {errors.descripcion && (
                <span className="text-xs text-red-500 mt-1 block">{errors.descripcion.message}</span>
              )}
            </div>

            {/* Subida de Imagen */}
            <div className="md:col-span-2">
              <label className="block text-xs font-semibold text-gray-700 uppercase mb-1">
                Imagen de la Mesa
              </label>
              <div className="flex gap-4 items-center">
                <label className="flex-1 flex flex-col items-center justify-center border-2 border-dashed border-gray-200 rounded-2xl p-6 cursor-pointer hover:border-purple-500 hover:bg-purple-50/30 transition-all">
                  <FiUpload className="w-8 h-8 text-purple-600 mb-2" />
                  <span className="text-sm font-semibold text-gray-700">Seleccionar imagen</span>
                  <span className="text-xs text-gray-400 mt-0.5">PNG, JPG o WEBP. Máximo 5 MB.</span>
                  <input
                    type="file"
                    accept="image/*"
                    className="hidden"
                    {...register("imagen")}
                  />
                </label>

                {/* Previsualización */}
                <div className="w-32 h-28 rounded-2xl border border-gray-100 bg-slate-50 flex items-center justify-center overflow-hidden flex-shrink-0">
                  {imagePreview ? (
                    <img src={imagePreview} alt="Vista previa" className="w-full h-full object-cover" />
                  ) : (
                    <div className="text-center p-2 text-gray-400">
                      <FiGrid className="w-6 h-6 mx-auto mb-1 opacity-50" />
                      <span className="text-[10px]">Sin imagen</span>
                    </div>
                  )}
                </div>
              </div>
            </div>

          </div>

          {/* Acciones */}
          <div className="flex justify-end gap-3 pt-4 border-t border-gray-100">
            <button
              type="button"
              onClick={onClose}
              className="px-5 py-2.5 rounded-xl border border-gray-200 text-gray-600 font-semibold text-sm hover:bg-gray-50 transition-colors"
            >
              Cancelar
            </button>
            <button
              type="submit"
              disabled={isLoading}
              className="px-6 py-2.5 rounded-xl bg-purple-600 text-white font-semibold text-sm hover:bg-purple-700 disabled:opacity-50 transition-all shadow-md shadow-purple-200"
            >
              {isLoading ? "Guardando..." : initialData ? "Actualizar mesa" : "Guardar mesa"}
            </button>
          </div>
        </form>

      </div>
    </div>
  );
};
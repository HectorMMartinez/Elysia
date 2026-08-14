import { useState, useEffect } from "react";
import { obtenerPlatos } from "../../services/platoService";
import { crearMenu, editarMenu } from "../../services/menuService";

const MenuFormModal = ({ isOpen, onClose, onSuccess, menuEdicion = null }) => {
  const [formData, setFormData] = useState({
    nombre: "",
    descripcion: "",
    estado: "Disponible",
    isPrincipal: false,
  });

  const [platosDisponibles, setPlatosDisponibles] = useState([]);
  const [platosSeleccionados, setPlatosSeleccionados] = useState([]);
  const [loading, setLoading] = useState(false);
  const [loadingPlatos, setLoadingPlatos] = useState(false);
  const [error, setError] = useState("");

  // Cargar lista general de platos al abrir
  useEffect(() => {
    if (isOpen) {
      cargarPlatos();
    }
  }, [isOpen]);

  // Cargar datos en modo edición
  useEffect(() => {
    if (menuEdicion && isOpen) {
      const valorPrincipal =
        menuEdicion.isPrincipal ?? menuEdicion.esPrincipal;

      const esPrincipalBooleano =
        valorPrincipal === true ||
        valorPrincipal === "true" ||
        valorPrincipal === 1 ||
        valorPrincipal === "1";

      setFormData({
        nombre: menuEdicion.nombreMenu || menuEdicion.nombre || "",
        descripcion:
          menuEdicion.descripcionMenu || menuEdicion.descripcion || "",
        estado: menuEdicion.menuEstado || menuEdicion.estado || "Disponible",
        isPrincipal: esPrincipalBooleano,
      });

      // Extraer IDs asegurando conversión numérica estricta
      const platosOriginales =
        menuEdicion.platosDtos || menuEdicion.platos || menuEdicion.mostrarPlatosDtos || [];

      const idsUnicos = Array.from(
        new Set(
          platosOriginales
            .map((p) => Number(p.idPlato || p.platoId || p.id))
            .filter((id) => !isNaN(id) && id > 0)
        )
      );

      setPlatosSeleccionados(idsUnicos);
    } else if (!menuEdicion && isOpen) {
      resetForm();
    }
  }, [menuEdicion, isOpen]);

  const cargarPlatos = async () => {
    try {
      setLoadingPlatos(true);
      const data = await obtenerPlatos();
      setPlatosDisponibles(Array.isArray(data) ? data : []);
    } catch (err) {
      console.error("Error al obtener platos:", err);
    } finally {
      setLoadingPlatos(false);
    }
  };

  const resetForm = () => {
    setFormData({
      nombre: "",
      descripcion: "",
      estado: "Disponible",
      isPrincipal: false,
    });
    setPlatosSeleccionados([]);
    setError("");
  };

  const handleChange = (e) => {
    const { name, value, type, checked } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]: type === "checkbox" ? checked : value,
    }));
  };

  const handleTogglePlato = (platoId) => {
    const numId = Number(platoId);
    if (isNaN(numId) || numId <= 0) return;

    setPlatosSeleccionados((prev) =>
      prev.includes(numId)
        ? prev.filter((id) => id !== numId)
        : [...prev, numId]
    );
  };

  // Helper para extraer el precio numérico independientemente de la propiedad de la API
  const obtenerPrecioPlato = (plato) => {
    const rawPrice =
      plato.precio ??
      plato.precioPlato ??
      plato.costo ??
      plato.precioUnitario ??
      0;
    const val = parseFloat(rawPrice);
    return isNaN(val) ? 0 : val;
  };

  // Combinación inteligente: Si el plato no está aún en platosDisponibles, busca en menuEdicion.platosDtos
  const obtenerListaCompletaPlatos = () => {
    const mapaPlatos = new Map();

    // 1. Agregar platos del catálogo general
    platosDisponibles.forEach((p) => {
      const id = Number(p.idPlato || p.platoId || p.id);
      if (id > 0) mapaPlatos.set(id, p);
    });

    // 2. Agregar platos que ya estaban asociados al menú si no estaban en el catálogo general
    if (menuEdicion) {
      const platosEdicion =
        menuEdicion.platosDtos || menuEdicion.platos || menuEdicion.mostrarPlatosDtos || [];
      platosEdicion.forEach((p) => {
        const id = Number(p.idPlato || p.platoId || p.id);
        if (id > 0 && !mapaPlatos.has(id)) {
          mapaPlatos.set(id, p);
        }
      });
    }

    return Array.from(mapaPlatos.values());
  };

  const todosLosPlatos = obtenerListaCompletaPlatos();

  // Cálculo en tiempo real de la suma acumulada de los platos marcados
  const precioTotalSumado = todosLosPlatos
    .filter((plato) => {
      const id = Number(plato.idPlato || plato.platoId || plato.id);
      return platosSeleccionados.includes(id);
    })
    .reduce((acc, plato) => acc + obtenerPrecioPlato(plato), 0);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError("");

    if (!formData.nombre.trim()) {
      setError("El nombre del menú es obligatorio.");
      return;
    }

    if (!formData.descripcion.trim()) {
      setError("La descripción es obligatoria.");
      return;
    }

    try {
      setLoading(true);

      const idsLimpios = platosSeleccionados
        .map((id) => Number(id))
        .filter((id) => !isNaN(id) && id > 0);

      // Enviamos la suma total absoluta exacta recalculada desde cero ($700.00)
      const datosEnvio = {
        nombre: formData.nombre.trim(),
        nombreMenu: formData.nombre.trim(),
        descripcion: formData.descripcion.trim(),
        descripcionMenu: formData.descripcion.trim(),
        estado: formData.estado,
        menuEstado: formData.estado,
        isPrincipal: Boolean(formData.isPrincipal),
        esPrincipal: Boolean(formData.isPrincipal),
        precio: precioTotalSumado,
        precioTotal: precioTotalSumado,
        platoIds: idsLimpios,
        platosIds: idsLimpios,
      };

      if (menuEdicion) {
        const menuId =
          menuEdicion.menuId || menuEdicion.id || menuEdicion.idMenu;
        await editarMenu(menuId, datosEnvio);
      } else {
        await crearMenu(datosEnvio);
      }

      onSuccess();
      onClose();
    } catch (err) {
      setError(err.message || "Ocurrió un error al guardar el menú.");
    } finally {
      setLoading(false);
    }
  };

  if (!isOpen) return null;

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/50 backdrop-blur-sm p-4 overflow-y-auto">
      <div className="bg-white rounded-2xl shadow-xl w-full max-w-2xl overflow-hidden my-8">
        <div className="px-6 py-4 border-b border-slate-100 flex justify-between items-center bg-slate-50/50">
          <h3 className="text-lg font-bold text-slate-800">
            {menuEdicion ? "Editar Menú" : "Nuevo Menú"}
          </h3>
          <button
            onClick={onClose}
            type="button"
            className="text-slate-400 hover:text-slate-600 transition-colors"
          >
            ✕
          </button>
        </div>

        <form onSubmit={handleSubmit} className="p-6 space-y-4">
          {error && (
            <div className="p-3 text-sm text-red-600 bg-red-50 rounded-lg">
              {error}
            </div>
          )}

          <div className="space-y-4">
            <div>
              <label className="block text-sm font-medium text-slate-700 mb-1">
                Nombre del Menú
              </label>
              <input
                type="text"
                name="nombre"
                value={formData.nombre}
                onChange={handleChange}
                placeholder="Ej. Menú Ejecutivo, Menú del Día"
                className="w-full px-4 py-2 rounded-xl border border-slate-200 focus:ring-2 focus:ring-violet-500 focus:border-transparent transition-all outline-none"
              />
            </div>

            <div>
              <label className="block text-sm font-medium text-slate-700 mb-1">
                Descripción
              </label>
              <textarea
                name="descripcion"
                rows="2"
                value={formData.descripcion}
                onChange={handleChange}
                placeholder="Descripción del menú..."
                className="w-full px-4 py-2 rounded-xl border border-slate-200 focus:ring-2 focus:ring-violet-500 focus:border-transparent transition-all outline-none resize-none"
              />
            </div>

            <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
              <div>
                <label className="block text-sm font-medium text-slate-700 mb-1">
                  Estado
                </label>
                <select
                  name="estado"
                  value={formData.estado}
                  onChange={handleChange}
                  className="w-full px-4 py-2 rounded-xl border border-slate-200 focus:ring-2 focus:ring-violet-500 outline-none"
                >
                  <option value="Disponible">Disponible</option>
                  <option value="NoDisponible">No Disponible</option>
                </select>
              </div>

              <div className="flex items-center mt-6">
                <label className="relative inline-flex items-center cursor-pointer">
                  <input
                    type="checkbox"
                    name="isPrincipal"
                    checked={formData.isPrincipal}
                    onChange={handleChange}
                    className="sr-only peer"
                  />
                  <div className="w-11 h-6 bg-slate-200 peer-focus:outline-none rounded-full peer peer-checked:after:translate-x-full peer-checked:after:border-white after:content-[''] after:absolute after:top-[2px] after:left-[2px] after:bg-white after:border-slate-300 after:border after:rounded-full after:h-5 after:w-5 after:transition-all peer-checked:bg-emerald-500"></div>
                  <span className="ml-3 text-sm font-medium text-slate-700">
                    Menú Principal
                  </span>
                </label>
              </div>
            </div>

            {/* Selección de Platos */}
            <div className="pt-2">
              <div className="flex items-center justify-between mb-2">
                <label className="block text-sm font-medium text-slate-700">
                  Seleccionar Platos para este Menú
                </label>
                {/* Total recalculado en tiempo real */}
                <span className="text-sm font-bold text-violet-600 bg-violet-50 px-2.5 py-1 rounded-lg border border-violet-100">
                  Total: ${precioTotalSumado.toFixed(2)}
                </span>
              </div>

              {loadingPlatos ? (
                <div className="py-4 text-center text-sm text-slate-400">
                  Cargando catálogo de platos...
                </div>
              ) : todosLosPlatos.length === 0 ? (
                <div className="py-4 text-center text-sm text-slate-400 bg-slate-50 rounded-xl">
                  No hay platos disponibles para asignar.
                </div>
              ) : (
                <div className="max-h-48 overflow-y-auto border border-slate-200 rounded-xl p-3 space-y-2 bg-slate-50/50">
                  {todosLosPlatos.map((plato) => {
                    const platoId = Number(
                      plato.idPlato || plato.platoId || plato.id
                    );
                    const isChecked = platosSeleccionados.includes(platoId);
                    const nombrePlato = plato.nombrePlato || plato.nombre;
                    const precioPlato = obtenerPrecioPlato(plato);

                    return (
                      <label
                        key={platoId}
                        className={`flex items-center justify-between p-2.5 rounded-lg cursor-pointer transition-colors ${
                          isChecked
                            ? "bg-violet-50 border border-violet-200"
                            : "hover:bg-slate-100 bg-white border border-slate-100"
                        }`}
                      >
                        <div className="flex items-center space-x-3">
                          <input
                            type="checkbox"
                            checked={isChecked}
                            onChange={() => handleTogglePlato(platoId)}
                            className="w-4 h-4 text-violet-600 rounded border-slate-300 focus:ring-violet-500"
                          />
                          <span className="text-sm font-medium text-slate-800">
                            {nombrePlato}
                          </span>
                        </div>
                        <span className="text-xs font-bold text-slate-600">
                          ${precioPlato.toFixed(2)}
                        </span>
                      </label>
                    );
                  })}
                </div>
              )}
            </div>
          </div>

          <div className="pt-4 border-t border-slate-100 flex justify-end space-x-3">
            <button
              type="button"
              onClick={onClose}
              className="px-4 py-2 text-sm font-medium text-slate-600 hover:bg-slate-100 rounded-xl transition-colors"
            >
              Cancelar
            </button>
            <button
              type="submit"
              disabled={loading}
              className="px-5 py-2 text-sm font-medium text-white bg-emerald-600 hover:bg-emerald-700 rounded-xl transition-colors disabled:opacity-50 flex items-center space-x-2 shadow-sm"
            >
              {loading ? "Guardando..." : "Guardar Menú"}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default MenuFormModal;
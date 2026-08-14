import React, { useState } from "react";

const MenuCard = ({ menu, onEdit, onDelete }) => {
  const [mostrarPlatos, setMostrarPlatos] = useState(true);

  // 1. Extraemos los platos vinculados al menú
  const rawPlatos = menu.platosDtos || menu.platos || menu.mostrarPlatosDtos || [];

  // Base URL para imágenes servidas desde la API .NET
  const API_URL = "https://localhost:7108/";

  // Helper para resolver la ruta de la imagen de forma limpia
  const getImagenUrl = (plato) => {
    if (!plato) return null;
    const ruta =
      plato.imagenUrl ||
      plato.imagen ||
      plato.fotoUrl ||
      plato.foto ||
      plato.rutaImagen ||
      plato.urlImagen;

    if (!ruta) return null;
    if (ruta.startsWith("http://") || ruta.startsWith("https://")) return ruta;
    return `${API_URL}${ruta.replace(/^\//, "")}`;
  };

  // 2. CORRECCIÓN DE DUPLICADOS:
  // Filtramos el arreglo para asegurar que solo exista un plato por ID único
  const platos = rawPlatos.filter((plato, index, self) => {
    const platoId = plato.platoId || plato.id || plato.idPlato;
    if (!platoId) return true; // Si no hay ID, se conserva por índice
    return index === self.findIndex((p) => (p.platoId || p.id || p.idPlato) === platoId);
  });

  // 3. Calculamos la suma exacta de los platos únicos
  const sumaPlatosActuales = platos.reduce((acc, plato) => {
    const precioRaw =
      plato.precio ??
      plato.precioPlato ??
      plato.costo ??
      plato.precioUnitario ??
      0;

    const valorNumerico = parseFloat(precioRaw);
    return acc + (isNaN(valorNumerico) ? 0 : valorNumerico);
  }, 0);

  // REGLA DE PRECIO: Suma de platos o precio directo del menú
  const precioFinal =
    platos.length > 0
      ? sumaPlatosActuales
      : parseFloat(menu.precio || menu.precioTotal || 0) || 0;

  const esPrincipal =
    menu.isPrincipal === true ||
    menu.esPrincipal === true ||
    menu.isPrincipal === "true" ||
    menu.esPrincipal === "true" ||
    menu.isPrincipal === 1 ||
    menu.esPrincipal === 1;

  const estado = menu.menuEstado || menu.estado || "Disponible";

  return (
    <div className="bg-white rounded-3xl p-6 shadow-sm border border-slate-100 flex flex-col gap-5 hover:shadow-md transition-all duration-200">
      
      {/* ENCABEZADO UNIFICADO: Sin imagen gigante superior para evitar jerarquías desiguales */}
      <div className="flex flex-col sm:flex-row items-start sm:items-center justify-between gap-4 border-b border-slate-100 pb-4">
        <div>
          <div className="flex items-center gap-3 flex-wrap">
            <h3 className="text-2xl font-extrabold text-slate-900 tracking-tight">
              {menu.nombreMenu || menu.nombre}
            </h3>

            {esPrincipal && (
              <span className="px-3 py-1 text-xs font-bold rounded-lg bg-amber-50 text-amber-600 border border-amber-100">
                Principal
              </span>
            )}

            <span
              className={`px-3 py-1 text-xs font-bold rounded-lg border ${
                estado === "Disponible"
                  ? "bg-emerald-50 text-emerald-600 border-emerald-100"
                  : "bg-slate-100 text-slate-500 border-slate-200"
              }`}
            >
              {estado}
            </span>
          </div>

          <p className="text-sm text-slate-500 mt-1 font-normal leading-relaxed">
            {menu.descripcionMenu || menu.descripcion || "Comida"}
          </p>
        </div>

        {/* Precio total y botones de acción */}
        <div className="flex items-center gap-4 self-end sm:self-center">
          <div className="text-right">
            <span className="text-2xl font-black text-slate-900 tracking-tight">
              ${precioFinal.toFixed(2)}
            </span>
            <span className="block text-[11px] font-semibold text-slate-400">
              Total del menú
            </span>
          </div>

          <div className="flex items-center gap-2 border-l border-slate-200 pl-4">
            <button
              onClick={() => onEdit(menu)}
              title="Editar menú"
              className="w-10 h-10 rounded-xl border border-slate-200 flex items-center justify-center text-slate-500 hover:bg-violet-50 hover:text-violet-600 hover:border-violet-200 transition-all"
            >
              ✏️
            </button>
            <button
              onClick={() => onDelete(menu)}
              title="Eliminar menú"
              className="w-10 h-10 rounded-xl border border-slate-200 flex items-center justify-center text-slate-500 hover:bg-rose-50 hover:text-rose-600 hover:border-rose-200 transition-all"
            >
              🗑️
            </button>
          </div>
        </div>
      </div>

      {/* SECCIÓN DE PLATOS: Mismo tamaño simétrico para todos los platos */}
      <div>
        <div className="flex items-center justify-between mb-3">
          <button
            type="button"
            onClick={() => setMostrarPlatos(!mostrarPlatos)}
            className="flex items-center gap-2 text-xs font-extrabold uppercase tracking-wider text-slate-400 hover:text-violet-700 transition-colors"
          >
            <span>PLATOS INCLUIDOS ({platos.length})</span>
            <span className="text-xs">{mostrarPlatos ? "▲" : "▼"}</span>
          </button>
        </div>

        {mostrarPlatos && (
          <div>
            {platos.length > 0 ? (
              <div className="grid grid-cols-1 md:grid-cols-2 gap-3">
                {platos.map((plato, idx) => {
                  const idPlato = plato.platoId || plato.id || plato.idPlato || idx;
                  const imgPlato = getImagenUrl(plato);
                  const nombrePlato = plato.nombrePlato || plato.nombre || "Plato";
                  const descPlato = plato.descripcionPlato || plato.descripcion || "";
                  const precioPlato = parseFloat(
                    plato.precio ?? plato.precioPlato ?? plato.costo ?? plato.precioUnitario ?? 0
                  );

                  return (
                    <div
                      key={idPlato}
                      className="flex items-center justify-between gap-3 bg-slate-50 border border-slate-100 rounded-2xl p-3.5 hover:border-slate-200 transition-all"
                    >
                      <div className="flex items-center gap-3.5 min-w-0">
                        {/* Contenedor de Imagen de tamaño estándar estricto (w-16 h-16) */}
                        <div className="w-16 h-16 rounded-xl bg-slate-200 overflow-hidden shrink-0 flex items-center justify-center border border-slate-200/60 relative">
                          {imgPlato ? (
                            <img
                              src={imgPlato}
                              alt={nombrePlato}
                              className="w-full h-full object-cover"
                              onError={(e) => {
                                e.target.style.display = "none";
                              }}
                            />
                          ) : (
                            <span className="text-xl">🍲</span>
                          )}
                        </div>

                        {/* Nombre y descripción */}
                        <div className="min-w-0 flex-1">
                          <h4 className="text-sm font-bold text-slate-800 truncate">
                            {nombrePlato}
                          </h4>
                          <p
                            className="text-xs text-slate-400 truncate"
                            title={descPlato}
                          >
                            {descPlato || "Sin descripción"}
                          </p>
                        </div>
                      </div>

                      {/* Precio del plato individual */}
                      <div className="text-right shrink-0">
                        <span className="text-sm font-extrabold text-slate-800">
                          ${isNaN(precioPlato) ? "0.00" : precioPlato.toFixed(2)}
                        </span>
                      </div>
                    </div>
                  );
                })}
              </div>
            ) : (
              <div className="bg-slate-50 rounded-2xl p-4 text-center text-xs text-slate-400 border border-dashed border-slate-200">
                Este menú no tiene platos asignados.
              </div>
            )}
          </div>
        )}
      </div>

    </div>
  );
};

export default MenuCard;
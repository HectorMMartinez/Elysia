import { useState, useEffect } from "react";
import {
  obtenerMenusConPlatos,
  eliminarMenu,
} from "../../services/menuService";
import MenuFormModal from "../../components/menu/MenuFormModal";
import { ConfirmModal } from "../../components/common/ConfirmModal";
import DashboardLayout from "../../components/layout/OwnerSidebar";
import MenuCard from "../../components/menu/MenuCard";

const MenuPage = () => {
  const [menus, setMenus] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const [busqueda, setBusqueda] = useState("");
  const [filtroEstado, setFiltroEstado] = useState("TODOS");

  // INTEGRACIÓN: Estados para la Paginación
  const [paginaActual, setPaginaActual] = useState(1);
  const menusPorPagina = 5; // Puedes cambiar la cantidad de menús por página aquí

  // Estados para modales
  const [isFormModalOpen, setIsFormModalOpen] = useState(false);
  const [menuEditar, setMenuEditar] = useState(null);

  const [isDeleteModalOpen, setIsDeleteModalOpen] = useState(false);
  const [menuAEliminar, setMenuAEliminar] = useState(null);
  const [deleting, setDeleting] = useState(false);

  useEffect(() => {
    cargarMenus();
  }, []);

  // INTEGRACIÓN: Resetear a la página 1 cuando cambia la búsqueda o el filtro
  useEffect(() => {
    setPaginaActual(1);
  }, [busqueda, filtroEstado]);

  const cargarMenus = async () => {
    try {
      setLoading(true);
      setError("");
      const res = await obtenerMenusConPlatos();
      const listaMenus =
        res?.mostrarMenuConPlatosDtos || (Array.isArray(res) ? res : []);
      setMenus(listaMenus);
    } catch (err) {
      setError(err.message || "Error al cargar los menús.");
    } finally {
      setLoading(false);
    }
  };

  const handleCrear = () => {
    setMenuEditar(null);
    setIsFormModalOpen(true);
  };

  const handleEditar = (menu) => {
    setMenuEditar(menu);
    setIsFormModalOpen(true);
  };

  const handleConfirmarEliminar = (menu) => {
    setMenuAEliminar(menu);
    setIsDeleteModalOpen(true);
  };

  const handleEliminar = async () => {
    if (!menuAEliminar) return;
    try {
      setDeleting(true);
      const menuId = menuAEliminar.menuId || menuAEliminar.id;
      await eliminarMenu(menuId);
      setIsDeleteModalOpen(false);
      setMenuAEliminar(null);
      await cargarMenus();
    } catch (err) {
      alert(err.message || "No se pudo eliminar el menú.");
    } finally {
      setDeleting(false);
    }
  };

  const verificarEsPrincipal = (m) => {
    const val = m?.isPrincipal ?? m?.esPrincipal ?? false;
    return val === true || val === "true" || val === 1 || val === "1";
  };

  // 1. Primero filtramos la lista completa
  const menusFiltrados = menus.filter((menu) => {
    const nombre = menu.nombreMenu || menu.nombre || "";
    const descripcion = menu.descripcionMenu || menu.descripcion || "";
    const estado = menu.menuEstado || menu.estado || "";

    const coincideBusqueda =
      nombre.toLowerCase().includes(busqueda.toLowerCase()) ||
      descripcion.toLowerCase().includes(busqueda.toLowerCase());

    if (filtroEstado === "TODOS") return coincideBusqueda;
    return coincideBusqueda && estado === filtroEstado;
  });

  // INTEGRACIÓN: 2. Luego aplicamos la lógica de paginación sobre la lista filtrada
  const totalMenusFiltrados = menusFiltrados.length;
  const totalPaginas = Math.ceil(totalMenusFiltrados / menusPorPagina);
  
  // Cálculo de índices para recortar el array
  const indiceUltimoMenu = paginaActual * menusPorPagina;
  const indicePrimerMenu = indiceUltimoMenu - menusPorPagina;
  
  // Array recortado que se va a renderizar en la página actual
  const menusPaginados = menusFiltrados.slice(indicePrimerMenu, indiceUltimoMenu);

  // KPIs (se calculan sobre la lista completa sin paginar)
  const totalMenus = menus.length;
  const disponibles = menus.filter(
    (m) => (m.menuEstado || m.estado) === "Disponible"
  ).length;
  const noDisponibles = menus.filter(
    (m) => (m.menuEstado || m.estado) === "NoDisponible"
  ).length;
  const principales = menus.filter(verificarEsPrincipal).length;

  const contenidoPagina = (
    <div className="p-8 space-y-7 max-w-[1700px] mx-auto bg-slate-50 min-h-screen">
      
      {/* Header Pulido */}
      <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-5">
        <div>
          <h1 className="text-4xl font-black text-slate-950 tracking-tighter">
            Menús
          </h1>
          <p className="text-base text-slate-600 mt-1.5 font-normal">
            Consulta los menús, sus platos asignados y su estado operativo.
          </p>
        </div>
        <button
          onClick={handleCrear}
          className="px-6 py-3 bg-violet-700 hover:bg-violet-800 text-white text-base font-bold rounded-2xl shadow-sm transition-all flex items-center justify-center space-x-2.5 active:scale-[0.98]"
        >
          <span className="text-xl">+</span>
          <span>Agregar menú</span>
        </button>
      </div>

      {/* Tarjetas KPI */}
      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-6">
        {/* KPI Total */}
        <div className="bg-white p-6 rounded-3xl border border-slate-100/80 shadow-sm flex items-center justify-between">
          <div>
            <p className="text-sm font-semibold text-slate-500 tracking-tight">Total de menús</p>
            <h3 className="text-4xl font-extrabold text-slate-950 mt-1.5 tracking-tighter">
              {totalMenus}
            </h3>
          </div>
          <div className="w-16 h-16 bg-violet-100/60 text-violet-700 rounded-3xl flex items-center justify-center text-3xl">
            🍽️
          </div>
        </div>
        {/* KPI Disponibles */}
        <div className="bg-white p-6 rounded-3xl border border-slate-100/80 shadow-sm flex items-center justify-between">
          <div>
            <p className="text-sm font-semibold text-slate-500 tracking-tight">Disponibles</p>
            <h3 className="text-4xl font-extrabold text-slate-950 mt-1.5 tracking-tighter">
              {disponibles}
            </h3>
          </div>
          <div className="w-16 h-16 bg-emerald-100/60 text-emerald-600 rounded-3xl flex items-center justify-center text-3xl">
            ✓
          </div>
        </div>
        {/* KPI No Disponibles */}
        <div className="bg-white p-6 rounded-3xl border border-slate-100/80 shadow-sm flex items-center justify-between">
          <div>
            <p className="text-sm font-semibold text-slate-500 tracking-tight">No disponibles</p>
            <h3 className="text-4xl font-extrabold text-slate-950 mt-1.5 tracking-tighter">
              {noDisponibles}
            </h3>
          </div>
          <div className="w-16 h-16 bg-red-100/60 text-red-600 rounded-3xl flex items-center justify-center text-3xl">
            ✕
          </div>
        </div>
        {/* KPI Principales */}
        <div className="bg-white p-6 rounded-3xl border border-slate-100/80 shadow-sm flex items-center justify-between">
          <div>
            <p className="text-sm font-semibold text-slate-500 tracking-tight">Menús Principales</p>
            <h3 className="text-4xl font-extrabold text-slate-950 mt-1.5 tracking-tighter">
              {principales}
            </h3>
          </div>
          <div className="w-16 h-16 bg-amber-100/60 text-amber-600 rounded-3xl flex items-center justify-center text-3xl">
            ⭐
          </div>
        </div>
      </div>

      {/* CAMBIO: Se envolvió la barra de filtros en un contenedor blanco pulido para alinearla visualmente */}
      <div className="bg-white rounded-3xl border border-slate-100/80 shadow-sm p-7">
        {/* Barra de Filtros */}
        <div className="flex flex-col md:flex-row gap-5 justify-between items-center">
          {/* Input de Búsqueda */}
          <div className="relative w-full md:w-[480px]">
            <span className="absolute inset-y-0 left-0 pl-4 flex items-center pointer-events-none text-slate-400 text-lg">
              🔍
            </span>
            <input
              type="text"
              placeholder="Buscar por nombre, descripción..."
              value={busqueda}
              onChange={(e) => setBusqueda(e.target.value)}
              className="w-full pl-12 pr-5 py-3.5 bg-slate-50 border border-slate-200 rounded-2xl text-base font-medium text-slate-800 placeholder:text-slate-400 outline-none focus:bg-white focus:ring-2 focus:ring-violet-300 transition-all focus:border-violet-400"
            />
          </div>

          <div className="flex flex-col sm:flex-row items-center gap-4 w-full md:w-auto">
            {/* Select de Estado */}
            <select
              value={filtroEstado}
              onChange={(e) => setFiltroEstado(e.target.value)}
              className="w-full sm:w-auto px-5 py-3.5 bg-slate-50 border border-slate-200 rounded-2xl text-base font-medium text-slate-800 outline-none focus:bg-white focus:ring-2 focus:ring-violet-300 transition-all appearance-none cursor-pointer focus:border-violet-400"
            >
              <option value="TODOS">Todos los estados</option>
              <option value="Disponible">Disponibles</option>
              <option value="NoDisponible">No Disponibles</option>
            </select>

            {/* Botón Actualizar */}
            <button
              onClick={cargarMenus}
              className="w-full sm:w-auto px-6 py-3.5 bg-white border border-slate-200 hover:bg-slate-50 text-slate-800 text-base font-semibold rounded-2xl transition-all flex items-center justify-center space-x-2.5 active:scale-[0.98]"
            >
              <span className="text-lg">🔄</span>
              <span>Actualizar</span>
            </button>
          </div>
        </div>
      </div>

      {/* Contenedor de la Lista y Paginación */}
      <div className="bg-white rounded-3xl border border-slate-100/80 shadow-sm p-7 space-y-7">
        {/* Estado de Lista Pulido */}
        {loading ? (
          <div className="py-28 text-center flex flex-col items-center justify-center gap-4 text-slate-500 font-medium bg-slate-50 rounded-3xl border border-slate-100">
            <div className="w-10 h-10 border-4 border-slate-200 border-t-violet-600 rounded-full animate-spin"></div>
            Cargando menús...
          </div>
        ) : error ? (
          <div className="p-6 text-center text-red-700 bg-red-50 rounded-3xl text-base font-semibold border border-red-100 flex items-center justify-center gap-3">
            <span className="text-2xl">⚠️</span> {error}
          </div>
        ) : totalMenusFiltrados === 0 ? (
          <div className="py-32 text-center flex flex-col items-center justify-center gap-4 bg-slate-50 rounded-3xl border border-slate-100/80">
            <div className="w-20 h-20 bg-violet-100/60 text-violet-600 rounded-3xl flex items-center justify-center text-4xl">
              🍴
            </div>
            <h3 className="text-2xl font-bold text-slate-900 tracking-tight mt-2">
              Todavía no hay menús
            </h3>
            <p className="text-base text-slate-500 max-w-sm">
              Cuando registres menús o cambies tus filtros, aparecerán organizados en esta sección.
            </p>
          </div>
        ) : (
          /* Renderizado con MenuCard en formato lista vertical con GAP */
          <div className="flex flex-col gap-6 pt-2">
            {/* INTEGRACIÓN: Mapeamos el array recortado 'menusPaginados' */}
            {menusPaginados.map((menu) => {
              const menuId = menu.menuId || menu.id;
              return (
                <MenuCard
                  key={menuId}
                  menu={menu}
                  onEdit={handleEditar}
                  onDelete={handleConfirmarEliminar}
                />
              );
            })}
          </div>
        )}

        {/* INTEGRACIÓN: Componente visual de Paginación al estilo del Boceto */}
        {!loading && totalPaginas > 1 && (
          <div className="flex justify-center items-center gap-2 pt-8 border-t border-slate-100/80">
            {/* Números de página */}
            {Array.from({ length: totalPaginas }, (_, index) => {
              const numeroPagina = index + 1;
              return (
                <button
                  key={numeroPagina}
                  onClick={() => setPaginaActual(numeroPagina)}
                  className={`w-11 h-11 rounded-xl font-bold transition-all ${
                    paginaActual === numeroPagina
                      ? "bg-violet-600 text-white shadow-md shadow-violet-200"
                      : "bg-white text-slate-600 hover:bg-slate-100 border border-slate-200"
                  }`}
                >
                  {numeroPagina}
                </button>
              );
            })}

            {/* Botón Siguiente (>) */}
            <button
              disabled={paginaActual === totalPaginas}
              onClick={() => setPaginaActual((prev) => Math.min(prev + 1, totalPaginas))}
              className="w-11 h-11 rounded-xl bg-white border border-slate-200 text-slate-600 font-bold hover:bg-slate-100 disabled:opacity-40"
            >
              ›
            </button>
          </div>
        )}
      </div>

      {/* Modales se mantienen igual */}
      <MenuFormModal
        isOpen={isFormModalOpen}
        onClose={() => setIsFormModalOpen(false)}
        onSuccess={cargarMenus}
        menuEdicion={menuEditar}
      />

      <ConfirmModal
        isOpen={isDeleteModalOpen}
        onClose={() => setIsDeleteModalOpen(false)}
        onConfirm={handleEliminar}
        title="Eliminar Menú"
        message={`¿Estás seguro de que deseas eliminar el menú "${
          menuAEliminar?.nombreMenu || menuAEliminar?.nombre || ""
        }"?`}
        loading={deleting}
      />
    </div>
  );

  return DashboardLayout ? (
    <DashboardLayout>{contenidoPagina}</DashboardLayout>
  ) : (
    contenidoPagina
  );
};

export default MenuPage;
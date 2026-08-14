import { useEffect, useMemo, useState } from "react";
import {
  FaBoxOpen,
  FaCheckCircle,
  FaEdit,
  FaExclamationTriangle,
  FaPlus,
  FaSearch,
  FaSyncAlt,
  FaTimesCircle,
  FaTrash,
  FaUtensils,
} from "react-icons/fa";

import OwnerSidebar from "../../components/layout/OwnerSidebar";
import DishModal from "../../components/dishes/DishModal";
import DeleteDishModal from "../../components/dishes/DeleteDishModal";
import {
  obtenerCategoriasPlato,
  obtenerPlatos,
  obtenerProductosParaIngredientes,
} from "../../services/platoService";
import { getProductImageUrl } from "../../utils/imageHelper";

const FILTERS = {
  ALL: "all",
};

const normalizarTexto = (value) =>
  String(value ?? "").trim().toLowerCase();

const estadoPlato = (estado) => {
  const value = String(estado ?? "").toLowerCase();

  if (estado === 1 || value === "1" || value === "disponible") {
    return {
      label: "Disponible",
      className: "bg-green-100 text-green-700",
      key: "disponible",
    };
  }

  if (estado === 2 || value === "2" || value === "agotado") {
    return {
      label: "Agotado",
      className: "bg-red-100 text-red-700",
      key: "agotado",
    };
  }

  return {
    label: "Sin estado",
    className: "bg-slate-200 text-slate-600",
    key: "unknown",
  };
};

const esErrorSinPlatos = (message) =>
  /no hay platos|no se encontraron platos|no se encontraron plato/i.test(
    message
  );

export default function PlatosPage() {
  const [platos, setPlatos] = useState([]);
  const [categorias, setCategorias] = useState([]);
  const [productos, setProductos] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const [search, setSearch] = useState("");
  const [categoryFilter, setCategoryFilter] = useState(FILTERS.ALL);
  const [dishModalOpen, setDishModalOpen] = useState(false);
  const [dishModalMode, setDishModalMode] = useState("create");
  const [selectedDish, setSelectedDish] = useState(null);
  const [deleteModalOpen, setDeleteModalOpen] = useState(false);
  const [deleteDish, setDeleteDish] = useState(null);

  const cargarDatos = async () => {
    try {
      setLoading(true);
      setError("");

      const categoriasData = await obtenerCategoriasPlato();
      setCategorias(Array.isArray(categoriasData) ? categoriasData : []);

      const productosData = await obtenerProductosParaIngredientes();
      setProductos(Array.isArray(productosData) ? productosData : []);

      try {
        const platosData = await obtenerPlatos();
        setPlatos(Array.isArray(platosData) ? platosData : []);
      } catch (platosError) {
        if (esErrorSinPlatos(platosError.message)) {
          setPlatos([]);
          return;
        }

        throw platosError;
      }
    } catch (err) {
      setError(err.message || "No fue posible cargar los platos.");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    queueMicrotask(cargarDatos);
  }, []);

  const openCreateModal = () => {
    setSelectedDish(null);
    setDishModalMode("create");
    setDishModalOpen(true);
  };

  const openEditModal = (plato) => {
    setSelectedDish(plato);
    setDishModalMode("edit");
    setDishModalOpen(true);
  };

  const closeDishModal = () => {
    setDishModalOpen(false);
    setSelectedDish(null);
  };

  const openDeleteModal = (plato) => {
    setDeleteDish(plato);
    setDeleteModalOpen(true);
  };

  const closeDeleteModal = () => {
    setDeleteModalOpen(false);
    setDeleteDish(null);
  };

  const categoriasPorId = useMemo(() => {
    return categorias.reduce((acc, categoria) => {
      acc[String(categoria.id)] = categoria.nombre;
      return acc;
    }, {});
  }, [categorias]);

  const summary = useMemo(() => {
    return {
      total: platos.length,
      disponibles: platos.filter(
        (plato) => estadoPlato(plato.estado).key === "disponible"
      ).length,
      agotados: platos.filter(
        (plato) => estadoPlato(plato.estado).key === "agotado"
      ).length,
      categorias: new Set(
        platos.map((plato) => plato.categoriaId).filter(Boolean)
      ).size,
    };
  }, [platos]);

  const filteredPlatos = useMemo(() => {
    const term = normalizarTexto(search);

    return platos.filter((plato) => {
      const categoriaNombre =
        plato.nombreCategoria ||
        categoriasPorId[String(plato.categoriaId)] ||
        "";

      const matchesSearch =
        !term ||
        normalizarTexto(plato.nombre).includes(term) ||
        normalizarTexto(plato.descripcion).includes(term) ||
        normalizarTexto(plato.codigo).includes(term) ||
        normalizarTexto(categoriaNombre).includes(term);

      if (!matchesSearch) return false;

      if (categoryFilter !== FILTERS.ALL) {
        return String(plato.categoriaId) === String(categoryFilter);
      }

      return true;
    });
  }, [platos, categoriasPorId, search, categoryFilter]);

  return (
    <OwnerSidebar>
      <div className="p-4 md:p-8">
        <header className="mb-8 flex flex-col gap-5 md:flex-row md:items-center md:justify-between">
          <div>
            <h1 className="text-3xl font-bold text-slate-800 md:text-4xl">
              Platos
            </h1>
            <p className="mt-2 text-slate-500">
              Consulta los platos, sus categorías y su estado operativo.
            </p>
          </div>

          <button
            type="button"
            onClick={openCreateModal}
            className="inline-flex items-center justify-center gap-2 rounded-xl bg-violet-600 px-5 py-3 font-semibold text-white transition hover:bg-violet-700"
          >
            <FaPlus />
            Agregar plato
          </button>
        </header>

        <section className="mb-8 grid gap-4 sm:grid-cols-2 xl:grid-cols-4">
          <SummaryCard
            title="Total de platos"
            value={summary.total}
            icon={<FaUtensils />}
            className="bg-blue-100 text-blue-600"
          />
          <SummaryCard
            title="Disponibles"
            value={summary.disponibles}
            icon={<FaCheckCircle />}
            className="bg-green-100 text-green-600"
          />
          <SummaryCard
            title="Agotados"
            value={summary.agotados}
            icon={<FaTimesCircle />}
            className="bg-red-100 text-red-600"
          />
          <SummaryCard
            title="Categorías usadas"
            value={summary.categorias}
            icon={<FaBoxOpen />}
            className="bg-amber-100 text-amber-600"
          />
        </section>

        <section className="overflow-hidden rounded-2xl bg-white shadow-sm">
          <div className="border-b border-slate-200 p-5 md:p-6">
            <div className="flex flex-col gap-4 lg:flex-row lg:items-center lg:justify-between">
              <div className="relative w-full lg:max-w-md">
                <FaSearch className="absolute left-4 top-1/2 -translate-y-1/2 text-slate-400" />
                <input
                  type="search"
                  value={search}
                  onChange={(event) => setSearch(event.target.value)}
                  placeholder="Buscar por nombre, descripción, código o categoría..."
                  className="w-full rounded-xl border border-slate-300 py-3 pl-11 pr-4 outline-none focus:border-violet-500 focus:ring-2 focus:ring-violet-100"
                />
              </div>

              <div className="flex flex-col gap-3 sm:flex-row">
                <select
                  value={categoryFilter}
                  onChange={(event) =>
                    setCategoryFilter(event.target.value)
                  }
                  className="rounded-xl border border-slate-300 bg-white px-4 py-3 outline-none focus:border-violet-500"
                >
                  <option value={FILTERS.ALL}>Todas las categorías</option>
                  {categorias.map((categoria) => (
                    <option key={categoria.id} value={categoria.id}>
                      {categoria.nombre}
                    </option>
                  ))}
                </select>

                <button
                  type="button"
                  onClick={cargarDatos}
                  disabled={loading}
                  className="inline-flex items-center justify-center gap-2 rounded-xl border border-slate-300 px-4 py-3 font-semibold text-slate-700 hover:bg-slate-50 disabled:opacity-60"
                >
                  <FaSyncAlt className={loading ? "animate-spin" : ""} />
                  Actualizar
                </button>
              </div>
            </div>
          </div>

          {loading && (
            <StateMessage
              icon={<FaSyncAlt className="animate-spin" />}
              title="Cargando platos..."
              text="Consultando los platos registrados."
            />
          )}

          {!loading && error && (
            <ErrorState message={error} onRetry={cargarDatos} />
          )}

          {!loading && !error && platos.length === 0 && (
            <StateMessage
              icon={<FaUtensils />}
              title="Todavía no hay platos"
              text="Cuando registres platos, aparecerán en esta lista."
            />
          )}

          {!loading &&
            !error &&
            platos.length > 0 &&
            filteredPlatos.length === 0 && (
              <StateMessage
                icon={<FaSearch />}
                title="No encontramos resultados"
                text="Prueba otra búsqueda o cambia la categoría."
              />
            )}

          {!loading && !error && filteredPlatos.length > 0 && (
            <>
              <div className="overflow-x-auto">
                <table className="min-w-full divide-y divide-slate-200">
                  <thead className="bg-slate-50">
                    <tr>
                      {[
                        "Plato",
                        "Categoría",
                        "Precio",
                        "Ingredientes",
                        "Estado",
                        "Acciones",
                      ].map((title) => (
                        <th
                          key={title}
                          className="px-5 py-4 text-left text-xs font-bold uppercase tracking-wide text-slate-500"
                        >
                          {title}
                        </th>
                      ))}
                    </tr>
                  </thead>

                  <tbody className="divide-y divide-slate-200">
                    {filteredPlatos.map((plato) => {
                      const status = estadoPlato(plato.estado);
                      const categoriaNombre =
                        plato.nombreCategoria ||
                        categoriasPorId[String(plato.categoriaId)] ||
                        "Sin categoría";
                      const ingredientes = Array.isArray(
                        plato.listDataProducto
                      )
                        ? plato.listDataProducto
                        : [];

                      return (
                        <tr key={plato.id} className="hover:bg-slate-50">
                          <td className="px-5 py-4">
                            <div className="flex min-w-[280px] items-center gap-3">
                              <div className="flex h-14 w-14 shrink-0 items-center justify-center overflow-hidden rounded-xl bg-slate-100">
                                {plato.imagen ? (
                                  <img
                                    src={getProductImageUrl(plato.imagen)}
                                    alt={plato.nombre}
                                    className="h-full w-full object-cover"
                                    onError={(event) => {
                                      event.currentTarget.style.display =
                                        "none";
                                    }}
                                  />
                                ) : (
                                  <FaUtensils className="text-slate-400" />
                                )}
                              </div>

                              <div>
                                <p className="font-semibold text-slate-800">
                                  {plato.nombre}
                                </p>
                                <p
                                  className="mt-1 max-w-[300px] truncate text-sm text-slate-500"
                                  title={plato.descripcion}
                                >
                                  {plato.descripcion}
                                </p>
                                {plato.codigo && (
                                  <p className="mt-1 text-xs font-semibold uppercase text-slate-400">
                                    {plato.codigo}
                                  </p>
                                )}
                              </div>
                            </div>
                          </td>

                          <td className="px-5 py-4 text-sm font-medium text-slate-700">
                            {categoriaNombre}
                          </td>

                          <td className="px-5 py-4 font-semibold text-slate-800">
                            {formatCurrency(plato.precio)}
                          </td>

                          <td className="px-5 py-4 text-sm text-slate-600">
                            {ingredientes.length > 0 ? (
                              <span>
                                {ingredientes.length} ingrediente
                                {ingredientes.length === 1 ? "" : "s"}
                              </span>
                            ) : (
                              <span className="text-slate-400">
                                Sin ingredientes
                              </span>
                            )}
                          </td>

                          <td className="px-5 py-4">
                            <span
                              className={`rounded-full px-3 py-1 text-xs font-semibold ${status.className}`}
                            >
                              {status.label}
                            </span>
                          </td>

                          <td className="px-5 py-4">
                            <div className="flex min-w-[96px] gap-2">
                              <ActionButton
                                title="Editar plato"
                                className="text-blue-600 hover:bg-blue-50"
                                onClick={() => openEditModal(plato)}
                              >
                                <FaEdit />
                              </ActionButton>
                              <ActionButton
                                title="Eliminar plato"
                                className="text-red-600 hover:bg-red-50"
                                onClick={() => openDeleteModal(plato)}
                              >
                                <FaTrash />
                              </ActionButton>
                            </div>
                          </td>
                        </tr>
                      );
                    })}
                  </tbody>
                </table>
              </div>

              <footer className="border-t border-slate-200 px-5 py-4 text-sm text-slate-500">
                Mostrando{" "}
                <strong className="text-slate-700">
                  {filteredPlatos.length}
                </strong>{" "}
                de{" "}
                <strong className="text-slate-700">
                  {platos.length}
                </strong>{" "}
                platos.
              </footer>
            </>
          )}
        </section>

        <DishModal
          open={dishModalOpen}
          mode={dishModalMode}
          plato={selectedDish}
          categorias={categorias}
          productos={productos}
          onClose={closeDishModal}
          onSuccess={cargarDatos}
        />

        <DeleteDishModal
          open={deleteModalOpen}
          plato={deleteDish}
          onClose={closeDeleteModal}
          onSuccess={cargarDatos}
        />
      </div>
    </OwnerSidebar>
  );
}

function formatCurrency(value) {
  const number = Number(value ?? 0);

  return new Intl.NumberFormat("es-DO", {
    style: "currency",
    currency: "DOP",
    maximumFractionDigits: 2,
  }).format(Number.isFinite(number) ? number : 0);
}

function SummaryCard({ title, value, icon, className }) {
  return (
    <article className="rounded-2xl bg-white p-5 shadow-sm">
      <div className="flex items-center justify-between">
        <div>
          <p className="text-sm font-medium text-slate-500">{title}</p>
          <p className="mt-2 text-3xl font-bold text-slate-800">
            {value}
          </p>
        </div>
        <div
          className={`flex h-14 w-14 items-center justify-center rounded-2xl text-2xl ${className}`}
        >
          {icon}
        </div>
      </div>
    </article>
  );
}

function ActionButton({
  children,
  title,
  className,
  onClick,
  disabled = false,
}) {
  return (
    <button
      type="button"
      title={title}
      aria-label={title}
      onClick={onClick}
      disabled={disabled}
      className={`flex h-9 w-9 items-center justify-center rounded-lg transition disabled:cursor-not-allowed ${className}`}
    >
      {children}
    </button>
  );
}

function StateMessage({ icon, title, text }) {
  return (
    <div className="flex min-h-[320px] flex-col items-center justify-center px-6 py-16 text-center">
      <div className="flex h-16 w-16 items-center justify-center rounded-2xl bg-blue-100 text-3xl text-blue-600">
        {icon}
      </div>
      <h2 className="mt-5 text-xl font-bold text-slate-800">{title}</h2>
      <p className="mt-2 text-slate-500">{text}</p>
    </div>
  );
}

function ErrorState({ message, onRetry }) {
  return (
    <div className="flex min-h-[320px] flex-col items-center justify-center px-6 py-16 text-center">
      <div className="flex h-16 w-16 items-center justify-center rounded-2xl bg-red-100 text-3xl text-red-600">
        <FaExclamationTriangle />
      </div>
      <h2 className="mt-5 text-xl font-bold text-slate-800">
        No pudimos cargar los platos
      </h2>
      <p className="mt-2 max-w-xl text-slate-500">{message}</p>
      <button
        type="button"
        onClick={onRetry}
        className="mt-6 inline-flex items-center gap-2 rounded-xl bg-violet-600 px-5 py-3 font-semibold text-white hover:bg-violet-700"
      >
        <FaSyncAlt />
        Intentar de nuevo
      </button>
    </div>
  );
}

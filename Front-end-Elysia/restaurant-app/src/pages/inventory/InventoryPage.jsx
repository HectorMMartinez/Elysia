import { useEffect, useMemo, useState } from "react";
import {
  FaArrowDown,
  FaArrowUp,
  FaBoxOpen,
  FaCheckCircle,
  FaEdit,
  FaExclamationTriangle,
  FaPlus,
  FaSearch,
  FaSyncAlt,
  FaTimesCircle,
  FaTrash,
} from "react-icons/fa";

import OwnerSidebar from "../../components/layout/OwnerSidebar";
import { obtenerProductos } from "../../services/productoService";
import ProductModal from "../../components/inventory/ProductModal";
import InventoryMovementModal from "../../components/inventory/InventoryMovementModal";
import DeleteProductModal from "../../components/inventory/DeleteProductModal";
import { getProductImageUrl } from "../../utils/imageHelper";
const FILTERS = {
  ALL: "all",
  ACTIVE: "active",
  INACTIVE: "inactive",
  LOW: "low",
  OUT: "out",
};

const stockStatus = (product) => {
  const current = Number(product.stockActual ?? 0);
  const minimum = Number(product.stockMinimo ?? 0);

  if (current <= 0) return { label: "Agotado", className: "bg-red-100 text-red-700" };
  if (current <= minimum) return { label: "Stock bajo", className: "bg-amber-100 text-amber-700" };
  return { label: "Disponible", className: "bg-green-100 text-green-700" };
};



export default function InventoryPage() {
  const [productos, setProductos] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const [search, setSearch] = useState("");
  const [filter, setFilter] = useState(FILTERS.ALL);
  const [productModalOpen, setProductModalOpen] = useState(false);
  const [productModalMode, setProductModalMode] = useState("create");
  const [selectedProduct, setSelectedProduct] = useState(null);
  const [movementModalOpen, setMovementModalOpen] = useState(false);
  const [movementType, setMovementType] = useState("entry");
  const [movementProduct, setMovementProduct] = useState(null);
  const [deleteModalOpen, setDeleteModalOpen] = useState(false);
  const [deleteProduct, setDeleteProduct] = useState(null);

  const cargarProductos = async () => {
    try {
      setLoading(true);
      setError("");
      const data = await obtenerProductos();
      setProductos(Array.isArray(data) ? data : []);
    } catch (err) {
      setError(err.message || "No fue posible cargar los productos.");
    } finally {
      setLoading(false);
    }
  };

  const openDeleteModal = (product) => {
      setDeleteProduct(product);
      setDeleteModalOpen(true);
  };

  const closeDeleteModal = () => {
      setDeleteModalOpen(false);
      setDeleteProduct(null);
  };

  useEffect(() => {
    cargarProductos();
  }, []);

  const summary = useMemo(() => ({
    total: productos.length,
    active: productos.filter((p) => p.activo).length,
    low: productos.filter((p) => Number(p.stockActual) > 0 && Number(p.stockActual) <= Number(p.stockMinimo)).length,
    out: productos.filter((p) => Number(p.stockActual) <= 0).length,
  }), [productos]);

  const filteredProducts = useMemo(() => {
    const term = search.trim().toLowerCase();

    return productos.filter((product) => {
      const matchesSearch =
        !term ||
        String(product.nombre ?? "").toLowerCase().includes(term) ||
        String(product.descripcion ?? "").toLowerCase().includes(term);

      if (!matchesSearch) return false;

      const current = Number(product.stockActual ?? 0);
      const minimum = Number(product.stockMinimo ?? 0);

      if (filter === FILTERS.ACTIVE) return product.activo === true;
      if (filter === FILTERS.INACTIVE) return product.activo === false;
      if (filter === FILTERS.LOW) return current > 0 && current <= minimum;
      if (filter === FILTERS.OUT) return current <= 0;
      return true;
    });
  }, [productos, search, filter]);

  const openCreateModal = () => {
    setSelectedProduct(null);
    setProductModalMode("create");
    setProductModalOpen(true);
  };

  const openEditModal = (product) => {
    setSelectedProduct(product);
    setProductModalMode("edit");
    setProductModalOpen(true);
  };

  const closeProductModal = () => {
    setProductModalOpen(false);
    setSelectedProduct(null);
  };

  const openMovementModal = (product, type) => {
    setMovementProduct(product);
    setMovementType(type);
    setMovementModalOpen(true);
  };

  const closeMovementModal = () => {
    setMovementModalOpen(false);
    setMovementProduct(null);
  };

  const formatDate = (value) => {
    if (!value) return "Sin fecha";

    const date = new Date(value);

    if (Number.isNaN(date.getTime())) {
      return "Sin fecha";
    }

    return new Intl.DateTimeFormat("es-DO", {
      day: "2-digit",
      month: "short",
      year: "numeric",
    }).format(date);
  };

  return (
    <div className="min-h-screen bg-slate-100 flex">
      <OwnerSidebar />

      <main className="flex-1 overflow-x-hidden p-4 md:p-8">
        <header className="mb-8 flex flex-col gap-5 md:flex-row md:items-center md:justify-between">
          <div>
            <h1 className="text-3xl font-bold text-slate-800 md:text-4xl">Inventario</h1>
            <p className="mt-2 text-slate-500">
              Gestiona los productos, existencias y movimientos del restaurante.
            </p>
          </div>

          <button
            type="button"
            onClick={openCreateModal}
            className="inline-flex items-center justify-center gap-2 rounded-xl bg-violet-600 px-5 py-3 font-semibold text-white transition hover:bg-violet-700"
          >
            <FaPlus />
            Agregar producto
          </button>
        </header>

        <section className="mb-8 grid gap-4 sm:grid-cols-2 xl:grid-cols-4">
          <SummaryCard title="Total de productos" value={summary.total} icon={<FaBoxOpen />} className="bg-blue-100 text-blue-600" />
          <SummaryCard title="Productos activos" value={summary.active} icon={<FaCheckCircle />} className="bg-green-100 text-green-600" />
          <SummaryCard title="Stock bajo" value={summary.low} icon={<FaExclamationTriangle />} className="bg-amber-100 text-amber-600" />
          <SummaryCard title="Productos agotados" value={summary.out} icon={<FaTimesCircle />} className="bg-red-100 text-red-600" />
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
                  placeholder="Buscar por nombre o descripción..."
                  className="w-full rounded-xl border border-slate-300 py-3 pl-11 pr-4 outline-none focus:border-violet-500 focus:ring-2 focus:ring-violet-100"
                />
              </div>

              <div className="flex flex-col gap-3 sm:flex-row">
                <select
                  value={filter}
                  onChange={(event) => setFilter(event.target.value)}
                  className="rounded-xl border border-slate-300 bg-white px-4 py-3 outline-none focus:border-violet-500"
                >
                  <option value={FILTERS.ALL}>Todos los productos</option>
                  <option value={FILTERS.ACTIVE}>Activos</option>
                  <option value={FILTERS.INACTIVE}>Inactivos</option>
                  <option value={FILTERS.LOW}>Stock bajo</option>
                  <option value={FILTERS.OUT}>Agotados</option>
                </select>

                <button
                  type="button"
                  onClick={cargarProductos}
                  disabled={loading}
                  className="inline-flex items-center justify-center gap-2 rounded-xl border border-slate-300 px-4 py-3 font-semibold text-slate-700 hover:bg-slate-50 disabled:opacity-60"
                >
                  <FaSyncAlt className={loading ? "animate-spin" : ""} />
                  Actualizar
                </button>
              </div>
            </div>
          </div>

          {loading && <StateMessage icon={<FaSyncAlt className="animate-spin" />} title="Cargando inventario..." text="Consultando los productos registrados." />}
          {!loading && error && <ErrorState message={error} onRetry={cargarProductos} />}
          {!loading && !error && productos.length === 0 && <StateMessage icon={<FaBoxOpen />} title="Todavía no hay productos" text="Agrega el primer producto para comenzar." />}
          {!loading && !error && productos.length > 0 && filteredProducts.length === 0 && <StateMessage icon={<FaSearch />} title="No encontramos resultados" text="Prueba otra búsqueda o cambia el filtro." />}

          {!loading && !error && filteredProducts.length > 0 && (
            <>
              <div className="overflow-x-auto">
                <table className="min-w-full divide-y divide-slate-200">
                  <thead className="bg-slate-50">
                    <tr>
                      {["Producto", "Unidad", "Stock actual", "Stock mínimo", "Inventario", "Estado", "Fecha", "Acciones"].map((title) => (
                        <th key={title} className="px-5 py-4 text-left text-xs font-bold uppercase tracking-wide text-slate-500">
                          {title}
                        </th>
                      ))}
                    </tr>
                  </thead>

                  <tbody className="divide-y divide-slate-200">
                    {filteredProducts.map((product) => {
                      const status = stockStatus(product);

                      return (
                        <tr key={product.id} className="hover:bg-slate-50">
                          <td className="px-5 py-4">
                            <div className="flex min-w-[240px] items-center gap-3">
                              <div className="flex h-12 w-12 shrink-0 items-center justify-center overflow-hidden rounded-xl bg-slate-100">
                                {product.imagen ? (
                                  <img
                                    src={getProductImageUrl(product.imagen)}
                                    alt={product.nombre}
                                    className="h-full w-full object-cover"
                                    onError={(event) => {
                                      event.currentTarget.style.display = "none";
                                    }}
                                  />
                                ) : (
                                  <FaBoxOpen className="text-slate-400" />
                                )}
                              </div>
                              <div>
                                <p className="font-semibold text-slate-800">{product.nombre}</p>
                                <p className="mt-1 max-w-[260px] truncate text-sm text-slate-500" title={product.descripcion}>
                                  {product.descripcion}
                                </p>
                              </div>
                            </div>
                          </td>
                          <td className="px-5 py-4 text-sm text-slate-600">{product.unidadMedida}</td>
                          <td className="px-5 py-4 font-semibold text-slate-800">{product.stockActual}</td>
                          <td className="px-5 py-4 text-slate-600">{product.stockMinimo}</td>
                          <td className="px-5 py-4">
                            <span className={`rounded-full px-3 py-1 text-xs font-semibold ${status.className}`}>{status.label}</span>
                          </td>
                          <td className="px-5 py-4">
                            <span className={`rounded-full px-3 py-1 text-xs font-semibold ${product.activo ? "bg-green-100 text-green-700" : "bg-slate-200 text-slate-600"}`}>
                              {product.activo ? "Activo" : "Inactivo"}
                            </span>
                          </td>
                          <td className="px-5 py-4 text-sm text-slate-500">{formatDate(product.fechaCreacion)}</td>
                          <td className="px-5 py-4">
                            <div className="flex min-w-[190px] gap-2">
                              <ActionButton
                                title="Registrar entrada"
                                className="text-green-600 hover:bg-green-50"
                                onClick={() => openMovementModal(product, "entry")}
                              >
                                <FaArrowDown />
                              </ActionButton>
                              <ActionButton
                                title="Registrar salida"
                                className="text-amber-600 hover:bg-amber-50"
                                onClick={() => openMovementModal(product, "exit")}
                              >
                                <FaArrowUp />
                              </ActionButton>
                              <ActionButton
                                title="Editar producto"
                                className="text-blue-600 hover:bg-blue-50"
                                onClick={() => openEditModal(product)}
                              >
                                <FaEdit />
                              </ActionButton>
                              <ActionButton
                                title="Eliminar producto"
                                className="text-red-600 hover:bg-red-50"
                                onClick={() => openDeleteModal(product)}
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
                Mostrando <strong className="text-slate-700">{filteredProducts.length}</strong> de{" "}
                <strong className="text-slate-700">{productos.length}</strong> productos.
              </footer>
            </>
          )}
        </section>
      </main>

      <ProductModal
        open={productModalOpen}
        mode={productModalMode}
        product={selectedProduct}
        onClose={closeProductModal}
        onSuccess={cargarProductos}
      />

      <InventoryMovementModal
        open={movementModalOpen}
        type={movementType}
        product={movementProduct}
        onClose={closeMovementModal}
        onSuccess={cargarProductos}
      />

      <DeleteProductModal
          open={deleteModalOpen}
          product={deleteProduct}
          onClose={closeDeleteModal}
          onSuccess={cargarProductos}
      />
    </div>
  );
}

function SummaryCard({ title, value, icon, className }) {
  return (
    <article className="rounded-2xl bg-white p-5 shadow-sm">
      <div className="flex items-center justify-between">
        <div>
          <p className="text-sm font-medium text-slate-500">{title}</p>
          <p className="mt-2 text-3xl font-bold text-slate-800">{value}</p>
        </div>
        <div className={`flex h-14 w-14 items-center justify-center rounded-2xl text-2xl ${className}`}>{icon}</div>
      </div>
    </article>
  );
}

function ActionButton({ children, title, className, onClick }) {
  return (
    <button
      type="button"
      title={title}
      aria-label={title}
      onClick={onClick}
      className={`flex h-9 w-9 items-center justify-center rounded-lg transition ${className}`}
    >
      {children}
    </button>
  );
}

function StateMessage({ icon, title, text }) {
  return (
    <div className="flex min-h-[320px] flex-col items-center justify-center px-6 py-16 text-center">
      <div className="flex h-16 w-16 items-center justify-center rounded-2xl bg-blue-100 text-3xl text-blue-600">{icon}</div>
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
      <h2 className="mt-5 text-xl font-bold text-slate-800">No pudimos cargar el inventario</h2>
      <p className="mt-2 max-w-xl text-slate-500">{message}</p>
      <button type="button" onClick={onRetry} className="mt-6 inline-flex items-center gap-2 rounded-xl bg-violet-600 px-5 py-3 font-semibold text-white hover:bg-violet-700">
        <FaSyncAlt />
        Intentar de nuevo
      </button>
    </div>
  );
}
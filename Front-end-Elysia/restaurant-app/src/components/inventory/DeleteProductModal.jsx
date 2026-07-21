import { useEffect, useState } from "react";
import {
  FaExclamationTriangle,
  FaSpinner,
  FaTimes,
  FaTrash,
} from "react-icons/fa";
import { getProductImageUrl } from "../../utils/imageHelper";
import { eliminarProducto } from "../../services/productoService";

export default function DeleteProductModal({
  open,
  product,
  onClose,
  onSuccess,
}) {
  const [deleting, setDeleting] = useState(false);
  const [error, setError] = useState("");

  useEffect(() => {
    if (open) {
      setDeleting(false);
      setError("");
    }
  }, [open, product]);

  useEffect(() => {
    if (!open) return undefined;

    const handleKeyDown = (event) => {
      if (event.key === "Escape" && !deleting) {
        onClose();
      }
    };

    document.addEventListener("keydown", handleKeyDown);

    return () => {
      document.removeEventListener("keydown", handleKeyDown);
    };
  }, [open, deleting, onClose]);

  if (!open || !product) return null;
    const API_BASE_URL =
    import.meta.env.VITE_API_BASE_URL || "https://localhost:7108";

    const getProductImageUrl = (image) => {
    if (!image) return "";

    if (
        image.startsWith("http://") ||
        image.startsWith("https://") ||
        image.startsWith("blob:") ||
        image.startsWith("data:")
    ) {
        return image;
    }

    const normalizedPath = image.startsWith("/") ? image : `/${image}`;

    return `${API_BASE_URL}${normalizedPath}`;
    };
  const handleClose = () => {
    if (deleting) return;

    setError("");
    onClose();
  };

  const handleDelete = async () => {
    if (!product?.id || deleting) return;

    try {
      setDeleting(true);
      setError("");

      await eliminarProducto(product.id);

      if (onSuccess) {
        await onSuccess();
      }

      onClose();
    } catch (err) {
      console.error("Error al eliminar el producto:", err);

      const backendMessage =
        err?.response?.data?.message ||
        err?.response?.data ||
        err?.message;

      if (typeof backendMessage === "string" && backendMessage.trim()) {
        setError(backendMessage);
      } else {
        setError(
          "No fue posible eliminar el producto. Puede que tenga movimientos de inventario o esté asociado a un plato."
        );
      }
    } finally {
      setDeleting(false);
    }
  };

  return (
    <div
      className="fixed inset-0 z-50 flex items-center justify-center bg-slate-950/50 p-4 backdrop-blur-sm"
      role="dialog"
      aria-modal="true"
      aria-labelledby="delete-product-title"
      onMouseDown={(event) => {
        if (event.target === event.currentTarget) {
          handleClose();
        }
      }}
    >
      <div className="w-full max-w-md overflow-hidden rounded-2xl bg-white shadow-2xl">
        <header className="flex items-start justify-between border-b border-slate-200 p-5">
          <div className="flex items-center gap-3">
            <div className="flex h-12 w-12 shrink-0 items-center justify-center rounded-xl bg-red-100 text-xl text-red-600">
              <FaTrash />
            </div>

            <div>
              <h2
                id="delete-product-title"
                className="text-xl font-bold text-slate-800"
              >
                Eliminar producto
              </h2>

              <p className="mt-1 text-sm text-slate-500">
                Esta acción no se puede deshacer.
              </p>
            </div>
          </div>

          <button
            type="button"
            onClick={handleClose}
            disabled={deleting}
            aria-label="Cerrar modal"
            className="flex h-9 w-9 items-center justify-center rounded-lg text-slate-400 transition hover:bg-slate-100 hover:text-slate-700 disabled:cursor-not-allowed disabled:opacity-50"
          >
            <FaTimes />
          </button>
        </header>

        <div className="p-5">
          <p className="text-slate-600">
            ¿Estás seguro de que deseas eliminar el siguiente producto?
          </p>

          <div className="mt-4 flex items-center gap-3 rounded-xl border border-slate-200 bg-slate-50 p-4">
            <div className="flex h-14 w-14 shrink-0 items-center justify-center overflow-hidden rounded-xl bg-slate-200">
              {product.imagen ? (
                <img
                  src={getProductImageUrl(product.imagen)}
                  alt={product.nombre}
                  className="h-full w-full object-cover"
                />
              ) : (
                <FaTrash className="text-slate-400" />
              )}
            </div>

            <div className="min-w-0">
              <p className="truncate font-semibold text-slate-800">
                {product.nombre}
              </p>

              <p className="mt-1 text-sm text-slate-500">
                Stock actual: {product.stockActual ?? 0}{" "}
                {product.unidadMedida ?? ""}
              </p>
            </div>
          </div>

          <div className="mt-4 flex items-start gap-3 rounded-xl bg-amber-50 p-4 text-sm text-amber-800">
            <FaExclamationTriangle className="mt-0.5 shrink-0" />

            <p>
              También se eliminará la imagen del producto. Si el producto tiene
              movimientos de inventario o está asociado a un plato, es posible
              que el sistema no permita eliminarlo.
            </p>
          </div>

          {error && (
            <div
              role="alert"
              className="mt-4 rounded-xl border border-red-200 bg-red-50 p-4 text-sm text-red-700"
            >
              {error}
            </div>
          )}
        </div>

        <footer className="flex flex-col-reverse gap-3 border-t border-slate-200 bg-slate-50 p-5 sm:flex-row sm:justify-end">
          <button
            type="button"
            onClick={handleClose}
            disabled={deleting}
            className="rounded-xl border border-slate-300 bg-white px-5 py-3 font-semibold text-slate-700 transition hover:bg-slate-100 disabled:cursor-not-allowed disabled:opacity-60"
          >
            Cancelar
          </button>

          <button
            type="button"
            onClick={handleDelete}
            disabled={deleting}
            className="inline-flex items-center justify-center gap-2 rounded-xl bg-red-600 px-5 py-3 font-semibold text-white transition hover:bg-red-700 disabled:cursor-not-allowed disabled:opacity-60"
          >
            {deleting ? (
              <>
                <FaSpinner className="animate-spin" />
                Eliminando...
              </>
            ) : (
              <>
                <FaTrash />
                Eliminar producto
              </>
            )}
          </button>
        </footer>
      </div>
    </div>
  );
}
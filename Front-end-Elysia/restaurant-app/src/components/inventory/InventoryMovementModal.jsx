import { useEffect, useMemo, useState } from "react";
import {
  FaArrowDown,
  FaArrowUp,
  FaBoxOpen,
  FaSpinner,
  FaTimes,
} from "react-icons/fa";

import { getProductImageUrl } from "../../utils/imageHelper";

import {
  registrarEntrada,
  registrarSalida,
} from "../../services/productoService";

const FORM_INICIAL = {
  cantidad: "",
};

export default function InventoryMovementModal({
  open,
  type = "entry",
  product = null,
  onClose,
  onSuccess,
}) {
  const esEntrada = type === "entry";

  const [form, setForm] = useState(FORM_INICIAL);
  const [error, setError] = useState("");
  const [saving, setSaving] = useState(false);

  useEffect(() => {
    if (!open) return;

    setForm(FORM_INICIAL);
    setError("");
  }, [open, type, product]);

  const stockActual = Number(product?.stockActual ?? 0);

  const cantidadNumerica = Number(form.cantidad || 0);

  const stockResultante = useMemo(() => {
    if (!product || cantidadNumerica <= 0) {
      return stockActual;
    }

    return esEntrada
      ? stockActual + cantidadNumerica
      : stockActual - cantidadNumerica;
  }, [
    product,
    stockActual,
    cantidadNumerica,
    esEntrada,
  ]);

  if (!open || !product) {
    return null;
  }

  const titulo = esEntrada
    ? "Registrar entrada"
    : "Registrar salida";

  const descripcion = esEntrada
    ? "Agrega existencias al inventario del producto."
    : "Descuenta existencias del inventario del producto.";

  const validar = () => {
    if (!Number.isFinite(cantidadNumerica) || cantidadNumerica <= 0) {
      return "La cantidad debe ser mayor que cero.";
    }

    if (!esEntrada && cantidadNumerica > stockActual) {
      return `No puedes retirar más de ${stockActual} ${product.unidadMedida ?? ""}.`;
    }

    return "";
  };

  const guardarMovimiento = async (event) => {
    event.preventDefault();

    const mensajeValidacion = validar();

    if (mensajeValidacion) {
      setError(mensajeValidacion);
      return;
    }

    try {
      setSaving(true);
      setError("");

      const movimiento = {
        productoId: product.id,
        cantidad: cantidadNumerica,
      };

      if (esEntrada) {
        await registrarEntrada(movimiento);
      } else {
        await registrarSalida(movimiento);
      }

      await onSuccess?.();
      onClose?.();
    } catch (err) {
      setError(
        err.message ||
          "No fue posible registrar el movimiento."
      );
    } finally {
      setSaving(false);
    }
  };

  return (
    <div
      className="fixed inset-0 z-50 flex items-center justify-center bg-slate-950/50 p-4"
      onMouseDown={(event) => {
        if (event.target === event.currentTarget && !saving) {
          onClose?.();
        }
      }}
    >
      <div className="w-full max-w-lg overflow-hidden rounded-2xl bg-white shadow-2xl">
        <div className="flex items-start justify-between border-b border-slate-200 px-6 py-5">
          <div className="flex items-start gap-4">
            <div
              className={`flex h-12 w-12 shrink-0 items-center justify-center rounded-2xl text-xl ${
                esEntrada
                  ? "bg-green-100 text-green-700"
                  : "bg-amber-100 text-amber-700"
              }`}
            >
              {esEntrada ? <FaArrowDown /> : <FaArrowUp />}
            </div>

            <div>
              <h2 className="text-2xl font-bold text-slate-800">
                {titulo}
              </h2>

              <p className="mt-1 text-sm text-slate-500">
                {descripcion}
              </p>
            </div>
          </div>

          <button
            type="button"
            onClick={onClose}
            disabled={saving}
            className="flex h-10 w-10 shrink-0 items-center justify-center rounded-xl text-slate-500 transition hover:bg-slate-100 disabled:opacity-50"
            aria-label="Cerrar modal"
          >
            <FaTimes />
          </button>
        </div>

        <form onSubmit={guardarMovimiento} className="p-6">
          {error && (
            <div className="mb-5 rounded-xl border border-red-200 bg-red-50 px-4 py-3 text-sm font-medium text-red-700">
              {error}
            </div>
          )}

          <div className="mb-5 flex items-center gap-4 rounded-2xl bg-slate-50 p-4">
            <div className="flex h-14 w-14 shrink-0 items-center justify-center overflow-hidden rounded-xl bg-white text-slate-400">
              {product.imagen ? (
                <img
                    src={getProductImageUrl(product.imagen)}
                    alt={product.nombre}
                    className="h-full w-full object-cover"
                />
              ) : (
                <FaBoxOpen className="text-2xl" />
              )}
            </div>

            <div className="min-w-0">
              <p className="truncate font-bold text-slate-800">
                {product.nombre}
              </p>

              <p className="mt-1 text-sm text-slate-500">
                Stock actual:{" "}
                <span className="font-semibold text-slate-700">
                  {stockActual} {product.unidadMedida}
                </span>
              </p>
            </div>
          </div>

          <div>
            <label
              htmlFor="cantidadMovimiento"
              className="mb-2 block text-sm font-semibold text-slate-700"
            >
              Cantidad
            </label>

            <div className="relative">
              <input
                id="cantidadMovimiento"
                type="number"
                min="0.01"
                step="0.01"
                value={form.cantidad}
                onChange={(event) => {
                  setForm({ cantidad: event.target.value });
                  setError("");
                }}
                placeholder="0"
                disabled={saving}
                autoFocus
                className="w-full rounded-xl border border-slate-300 px-4 py-3 pr-32 text-slate-700 outline-none transition focus:border-violet-500 focus:ring-2 focus:ring-violet-100 disabled:bg-slate-100"
              />

              <span className="pointer-events-none absolute inset-y-0 right-4 flex items-center text-sm font-medium text-slate-500">
                {product.unidadMedida}
              </span>
            </div>
          </div>

          <div
            className={`mt-5 rounded-2xl border p-4 ${
              stockResultante < 0
                ? "border-red-200 bg-red-50"
                : esEntrada
                  ? "border-green-200 bg-green-50"
                  : "border-amber-200 bg-amber-50"
            }`}
          >
            <p className="text-sm font-medium text-slate-600">
              Stock después del movimiento
            </p>

            <p
              className={`mt-1 text-2xl font-bold ${
                stockResultante < 0
                  ? "text-red-700"
                  : esEntrada
                    ? "text-green-700"
                    : "text-amber-700"
              }`}
            >
              {stockResultante} {product.unidadMedida}
            </p>
          </div>

          {!esEntrada && stockActual <= 0 && (
            <p className="mt-4 text-sm font-medium text-red-600">
              Este producto está agotado y no admite salidas.
            </p>
          )}

          <div className="mt-7 flex flex-col-reverse gap-3 border-t border-slate-200 pt-5 sm:flex-row sm:justify-end">
            <button
              type="button"
              onClick={onClose}
              disabled={saving}
              className="rounded-xl border border-slate-300 px-5 py-3 font-semibold text-slate-700 transition hover:bg-slate-50 disabled:opacity-50"
            >
              Cancelar
            </button>

            <button
              type="submit"
              disabled={saving || (!esEntrada && stockActual <= 0)}
              className={`inline-flex items-center justify-center gap-2 rounded-xl px-5 py-3 font-semibold text-white transition disabled:cursor-not-allowed disabled:opacity-60 ${
                esEntrada
                  ? "bg-green-600 hover:bg-green-700"
                  : "bg-amber-600 hover:bg-amber-700"
              }`}
            >
              {saving && <FaSpinner className="animate-spin" />}

              {saving
                ? "Registrando..."
                : esEntrada
                  ? "Registrar entrada"
                  : "Registrar salida"}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}
import { useEffect, useMemo, useState } from "react";
import {
  FaBoxOpen,
  FaCloudUploadAlt,
  FaSpinner,
  FaTimes,
} from "react-icons/fa";

import {
  crearProducto,
  editarProducto,
} from "../../services/productoService";

import {
  UNIDADES_MEDIDA,
  VALORES_UNIDADES_MEDIDA,
} from "../../constants/inventoryUnits";

import { getProductImageUrl } from "../../utils/imageHelper";

const FORM_INICIAL = {
  nombre: "",
  descripcion: "",
  unidadMedida: "",
  stockActual: "",
  stockMinimo: "",
  activo: true,
  imagen: null,
};


const UNIDADES_ANTIGUAS = {
  Unidad: "ud",
  Pieza: "pz",
  Par: "par",
  Docena: "doc",
  "Media docena": "mdoc",
  Centena: "cen",
  Paquete: "paq",
  Caja: "caja",
  Saco: "saco",
  Funda: "fund",
  Bolsa: "bols",
  Bandeja: "band",
  Cubeta: "cube",
  Rollo: "roll",
  Lata: "lata",
  Botella: "bot",
  Frasco: "fras",
  Tarro: "tarr",
  Miligramo: "mg",
  Gramo: "g",
  Kilogramo: "kg",
  Onza: "oz",
  Lb: "lb",
  Libra: "lb",
  Quintal: "qq",
  Tonelada: "t",
  Mililitro: "ml",
  Centilitro: "cl",
  Litro: "l",
  Galón: "gal",
  Galon: "gal",
  Cuarto: "qt",
  Pinta: "pt",
  "Onza líquida": "floz",
  "Onza liquida": "floz",
  Taza: "taza",
  "Media taza": "mtaz",
  Cucharada: "cda",
  Cucharadita: "cdta",
  Gota: "gota",
  Milímetro: "mm",
  Milimetro: "mm",
  Centímetro: "cm",
  Centimetro: "cm",
  Metro: "m",
  Porción: "porc",
  Porcion: "porc",
  Ración: "rac",
  Racion: "rac",
  Servicio: "serv",
  Pizca: "pizc",
  Manojo: "manj",
  Diente: "dien",
  Rodaja: "roda",
  Rebanada: "reba",
  Filete: "file",
};

const normalizarUnidadMedida = (unidad) => {
  const valor = String(unidad ?? "").trim();

  if (!valor) return "";
  if (VALORES_UNIDADES_MEDIDA.includes(valor)) return valor;

  const coincidencia = VALORES_UNIDADES_MEDIDA.find(
    (item) => item.toLowerCase() === valor.toLowerCase()
  );

  return coincidencia ?? UNIDADES_ANTIGUAS[valor] ?? "";
};

export default function ProductModal({
  open,
  mode = "create",
  product = null,
  onClose,
  onSuccess,
}) {
  const esEdicion = mode === "edit";

  const [form, setForm] = useState(FORM_INICIAL);
  const [preview, setPreview] = useState("");
  const [error, setError] = useState("");
  const [saving, setSaving] = useState(false);

  useEffect(() => {
    if (!open) return;

    if (esEdicion && product) {
      setForm({
        nombre: product.nombre ?? "",
        descripcion: product.descripcion ?? "",
        unidadMedida: normalizarUnidadMedida(product.unidadMedida),
        stockActual: product.stockActual ?? "",
        stockMinimo: product.stockMinimo ?? "",
        activo: Boolean(product.activo),
        imagen: null,
      });

      setPreview(getProductImageUrl(product.imagen));
    } else {
      setForm(FORM_INICIAL);
      setPreview("");
    }

    setError("");
  }, [open, esEdicion, product]);

  useEffect(() => {
    return () => {
      if (preview?.startsWith("blob:")) {
        URL.revokeObjectURL(preview);
      }
    };
  }, [preview]);

  const titulo = useMemo(
    () => (esEdicion ? "Editar producto" : "Agregar producto"),
    [esEdicion]
  );

  if (!open) return null;

  const actualizarCampo = (event) => {
    const { name, value, type, checked } = event.target;

    setForm((actual) => ({
      ...actual,
      [name]: type === "checkbox" ? checked : value,
    }));

    setError("");
  };

  const seleccionarImagen = (event) => {
    const archivo = event.target.files?.[0] ?? null;

    if (!archivo) return;

    if (!archivo.type.startsWith("image/")) {
      setError("El archivo seleccionado debe ser una imagen.");
      return;
    }

    if (archivo.size > 5 * 1024 * 1024) {
      setError("La imagen no puede superar los 5 MB.");
      return;
    }

    if (preview?.startsWith("blob:")) {
      URL.revokeObjectURL(preview);
    }

    setForm((actual) => ({
      ...actual,
      imagen: archivo,
    }));

    setPreview(URL.createObjectURL(archivo));
    setError("");
  };

  const validarFormulario = () => {
    if (!form.nombre.trim()) return "El nombre es obligatorio.";
    if (!form.descripcion.trim()) return "La descripción es obligatoria.";
    if (!form.unidadMedida) return "Debes seleccionar una unidad de medida.";

    if (form.unidadMedida.length > 4) {
      return "La unidad de medida no puede superar los 4 caracteres.";
    }

    if (Number(form.stockActual) <= 0) {
      return "El stock actual debe ser mayor que cero.";
    }

    if (Number(form.stockMinimo) <= 0) {
      return "El stock mínimo debe ser mayor que cero.";
    }

    if (Number(form.stockMinimo) > Number(form.stockActual)) {
      return "El stock mínimo no puede ser mayor que el stock actual.";
    }

    if (!esEdicion && !(form.imagen instanceof File)) {
      return "Debes seleccionar una imagen.";
    }

    return "";
  };

  const guardarProducto = async (event) => {
    event.preventDefault();

    const mensajeValidacion = validarFormulario();

    if (mensajeValidacion) {
      setError(mensajeValidacion);
      return;
    }

    try {
      setSaving(true);
      setError("");

      const datos = {
        nombre: form.nombre.trim(),
        descripcion: form.descripcion.trim(),
        unidadMedida: form.unidadMedida,
        stockActual: Number(form.stockActual),
        stockMinimo: Number(form.stockMinimo),
        activo: Boolean(form.activo),
        imagen: form.imagen,
      };

      if (esEdicion) {
        await editarProducto(product.id, datos);
      } else {
        await crearProducto(datos);
      }

      await onSuccess?.();
      onClose?.();
    } catch (err) {
      setError(err.message || "No fue posible guardar el producto.");
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
      <div className="max-h-[92vh] w-full max-w-3xl overflow-y-auto rounded-2xl bg-white shadow-2xl">
        <div className="sticky top-0 z-10 flex items-center justify-between border-b border-slate-200 bg-white px-6 py-5">
          <div>
            <h2 className="text-2xl font-bold text-slate-800">
              {titulo}
            </h2>

            <p className="mt-1 text-sm text-slate-500">
              {esEdicion
                ? "Actualiza la información del producto seleccionado."
                : "Completa los datos para registrar un producto."}
            </p>
          </div>

          <button
            type="button"
            onClick={onClose}
            disabled={saving}
            className="flex h-10 w-10 items-center justify-center rounded-xl text-slate-500 transition hover:bg-slate-100 disabled:opacity-50"
            aria-label="Cerrar modal"
          >
            <FaTimes />
          </button>
        </div>

        <form onSubmit={guardarProducto} className="p-6">
          {error && (
            <div className="mb-5 rounded-xl border border-red-200 bg-red-50 px-4 py-3 text-sm font-medium text-red-700">
              {error}
            </div>
          )}

          <div className="grid gap-5 md:grid-cols-2">
            <FormField
              label="Nombre"
              name="nombre"
              value={form.nombre}
              onChange={actualizarCampo}
              placeholder="Ej. Arroz premium"
              disabled={saving}
            />

            <UnidadMedidaField
              value={form.unidadMedida}
              onChange={actualizarCampo}
              disabled={saving}
            />


            <div className="md:col-span-2">
              <label
                htmlFor="descripcion"
                className="mb-2 block text-sm font-semibold text-slate-700"
              >
                Descripción
              </label>

              <textarea
                id="descripcion"
                name="descripcion"
                value={form.descripcion}
                onChange={actualizarCampo}
                rows={4}
                placeholder="Describe brevemente el producto."
                disabled={saving}
                className="w-full resize-none rounded-xl border border-slate-300 px-4 py-3 text-slate-700 outline-none transition focus:border-violet-500 focus:ring-2 focus:ring-violet-100 disabled:bg-slate-100"
              />
            </div>

            <FormField
              label="Stock actual"
              name="stockActual"
              type="number"
              min="0.01"
              step="0.01"
              value={form.stockActual}
              onChange={actualizarCampo}
              placeholder="0"
              disabled={saving}
            />

            <FormField
              label="Stock mínimo"
              name="stockMinimo"
              type="number"
              min="0.01"
              step="0.01"
              value={form.stockMinimo}
              onChange={actualizarCampo}
              placeholder="0"
              disabled={saving}
            />

            <div className="md:col-span-2">
              <p className="mb-2 text-sm font-semibold text-slate-700">
                Imagen {esEdicion ? "(opcional)" : ""}
              </p>

              <div className="grid gap-4 md:grid-cols-[1fr_180px]">
                <label className="flex min-h-36 cursor-pointer flex-col items-center justify-center rounded-2xl border-2 border-dashed border-slate-300 px-5 py-6 text-center transition hover:border-violet-400 hover:bg-violet-50">
                  <FaCloudUploadAlt className="text-4xl text-violet-500" />
                  <span className="mt-3 font-semibold text-slate-700">
                    Seleccionar imagen
                  </span>
                  <span className="mt-1 text-sm text-slate-500">
                    PNG, JPG o WEBP. Máximo 5 MB.
                  </span>

                  <input
                    type="file"
                    accept="image/*"
                    onChange={seleccionarImagen}
                    disabled={saving}
                    className="hidden"
                  />
                </label>

                <div className="flex min-h-36 items-center justify-center overflow-hidden rounded-2xl bg-slate-100">
                  {preview ? (
                    <img
                      src={preview}
                      alt="Vista previa"
                      className="h-full max-h-44 w-full object-cover"
                    />
                  ) : (
                    <div className="text-center text-slate-400">
                      <FaBoxOpen className="mx-auto text-4xl" />
                      <p className="mt-2 text-sm">Sin imagen</p>
                    </div>
                  )}
                </div>
              </div>
            </div>

            {esEdicion && (
              <div className="md:col-span-2">
                <label className="flex cursor-pointer items-center justify-between rounded-2xl border border-slate-200 p-4">
                  <div>
                    <p className="font-semibold text-slate-800">
                      Producto activo
                    </p>
                    <p className="mt-1 text-sm text-slate-500">
                      Los productos inactivos permanecen registrados.
                    </p>
                  </div>

                  <input
                    type="checkbox"
                    name="activo"
                    checked={form.activo}
                    onChange={actualizarCampo}
                    disabled={saving}
                    className="h-5 w-5 accent-violet-600"
                  />
                </label>
              </div>
            )}
          </div>

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
              disabled={saving}
              className="inline-flex items-center justify-center gap-2 rounded-xl bg-violet-600 px-5 py-3 font-semibold text-white transition hover:bg-violet-700 disabled:cursor-not-allowed disabled:opacity-60"
            >
              {saving && <FaSpinner className="animate-spin" />}
              {saving
                ? "Guardando..."
                : esEdicion
                  ? "Guardar cambios"
                  : "Guardar producto"}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}

function UnidadMedidaField({ value, onChange, disabled }) {
  return (
    <div>
      <label
        htmlFor="unidadMedida"
        className="mb-2 block text-sm font-semibold text-slate-700"
      >
        Unidad de medida
      </label>

      <select
        id="unidadMedida"
        name="unidadMedida"
        value={value}
        onChange={onChange}
        disabled={disabled}
        className="w-full rounded-xl border border-slate-300 bg-white px-4 py-3 text-slate-700 outline-none transition focus:border-violet-500 focus:ring-2 focus:ring-violet-100 disabled:bg-slate-100"
      >
        <option value="">Selecciona una unidad</option>

        {UNIDADES_MEDIDA.map((grupo) => (
          <optgroup key={grupo.grupo} label={grupo.grupo}>
            {grupo.opciones.map((opcion) => (
              <option key={opcion.valor} value={opcion.valor}>
                {opcion.etiqueta}
              </option>
            ))}
          </optgroup>
        ))}

      </select>
    </div>
  );
}

function FormField({
  label,
  name,
  type = "text",
  value,
  onChange,
  placeholder,
  disabled,
  min,
  step,
}) {
  return (
    <div>
      <label
        htmlFor={name}
        className="mb-2 block text-sm font-semibold text-slate-700"
      >
        {label}
      </label>

      <input
        id={name}
        name={name}
        type={type}
        min={min}
        step={step}
        value={value}
        onChange={onChange}
        placeholder={placeholder}
        disabled={disabled}
        className="w-full rounded-xl border border-slate-300 px-4 py-3 text-slate-700 outline-none transition focus:border-violet-500 focus:ring-2 focus:ring-violet-100 disabled:bg-slate-100"
      />
    </div>
  );
}
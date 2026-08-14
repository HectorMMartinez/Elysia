import { useEffect, useMemo, useState } from "react";
import {
  FaCloudUploadAlt,
  FaPlus,
  FaSpinner,
  FaTimes,
  FaTrash,
  FaUtensils,
} from "react-icons/fa";

import { crearPlato, editarPlato } from "../../services/platoService";
import { getProductImageUrl } from "../../utils/imageHelper";

const FORM_INICIAL = {
  nombre: "",
  descripcion: "",
  precio: "",
  categoriaId: "",
  estado: "1",
  imagen: null,
  ingredientes: [],
};

const INGREDIENTE_INICIAL = {
  productoId: "",
  cantidad: "",
};

export default function DishModal({
  open,
  mode = "create",
  plato = null,
  categorias = [],
  productos = [],
  onClose,
  onSuccess,
}) {
  const esEdicion = mode === "edit";

  const [form, setForm] = useState(FORM_INICIAL);
  const [ingredientForm, setIngredientForm] = useState(
    INGREDIENTE_INICIAL
  );
  const [preview, setPreview] = useState("");
  const [error, setError] = useState("");
  const [saving, setSaving] = useState(false);

  const productosPorId = useMemo(() => {
    return productos.reduce((acc, producto) => {
      acc[String(producto.id)] = producto;
      return acc;
    }, {});
  }, [productos]);

  useEffect(() => {
    if (!open) return undefined;

    queueMicrotask(() => {
      if (esEdicion && plato) {
        const ingredientes = Array.isArray(plato.listDataProducto)
          ? plato.listDataProducto
              .map((ingrediente) => {
                const productoId =
                  ingrediente.productoId ?? ingrediente.id;
                const producto = productosPorId[String(productoId)];

                if (!productoId || !producto) return null;

                return {
                  productoId: Number(productoId),
                  nombre:
                    producto.nombre ||
                    ingrediente.nombreProducto ||
                    "",
                  unidadMedida: producto.unidadMedida || "",
                  cantidad:
                    ingrediente.cantidaProducto ??
                    ingrediente.cantidad ??
                    "",
                };
              })
              .filter(Boolean)
          : [];

        setForm({
          nombre: plato.nombre ?? "",
          descripcion: plato.descripcion ?? "",
          precio: plato.precio ?? "",
          categoriaId: plato.categoriaId ?? "",
          estado: normalizarEstado(plato.estado),
          imagen: null,
          ingredientes,
        });

        setPreview(getProductImageUrl(plato.imagen));
      } else {
        setForm(FORM_INICIAL);
        setPreview("");
      }

      setIngredientForm(INGREDIENTE_INICIAL);
      setError("");
      setSaving(false);
    });

    return undefined;
  }, [open, esEdicion, plato, productosPorId]);

  useEffect(() => {
    return () => {
      if (preview?.startsWith("blob:")) {
        URL.revokeObjectURL(preview);
      }
    };
  }, [preview]);

  useEffect(() => {
    if (!open) return undefined;

    const handleKeyDown = (event) => {
      if (event.key === "Escape" && !saving) {
        onClose?.();
      }
    };

    document.addEventListener("keydown", handleKeyDown);

    return () => {
      document.removeEventListener("keydown", handleKeyDown);
    };
  }, [open, saving, onClose]);

  if (!open) return null;

  const titulo = esEdicion ? "Editar plato" : "Agregar plato";

  const productosDisponibles = productos.filter((producto) => {
    return !form.ingredientes.some(
      (ingrediente) =>
        String(ingrediente.productoId) === String(producto.id)
    );
  });

  const actualizarCampo = (event) => {
    const { name, value } = event.target;

    setForm((actual) => ({
      ...actual,
      [name]: value,
    }));

    setError("");
  };

  const actualizarIngrediente = (event) => {
    const { name, value } = event.target;

    setIngredientForm((actual) => ({
      ...actual,
      [name]: value,
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

  const agregarIngrediente = () => {
    const productoId = Number(ingredientForm.productoId);
    const cantidad = Number(ingredientForm.cantidad);
    const producto = productosPorId[String(productoId)];

    if (!producto) {
      setError("Debes seleccionar un producto del inventario.");
      return;
    }

    if (!Number.isFinite(cantidad) || cantidad <= 0) {
      setError("La cantidad del ingrediente debe ser mayor que cero.");
      return;
    }

    const duplicado = form.ingredientes.some(
      (ingrediente) => ingrediente.productoId === productoId
    );

    if (duplicado) {
      setError("Ese ingrediente ya fue seleccionado.");
      return;
    }

    setForm((actual) => ({
      ...actual,
      ingredientes: [
        ...actual.ingredientes,
        {
          productoId,
          nombre: producto.nombre,
          unidadMedida: producto.unidadMedida,
          cantidad,
        },
      ],
    }));
    setIngredientForm(INGREDIENTE_INICIAL);
    setError("");
  };

  const eliminarIngrediente = (productoId) => {
    setForm((actual) => ({
      ...actual,
      ingredientes: actual.ingredientes.filter(
        (ingrediente) => ingrediente.productoId !== productoId
      ),
    }));
    setError("");
  };

  const actualizarCantidadSeleccionada = (productoId, value) => {
    setForm((actual) => ({
      ...actual,
      ingredientes: actual.ingredientes.map((ingrediente) =>
        ingrediente.productoId === productoId
          ? { ...ingrediente, cantidad: value }
          : ingrediente
      ),
    }));
    setError("");
  };

  const validarFormulario = () => {
    if (!form.nombre.trim()) return "El nombre es obligatorio.";
    if (!form.descripcion.trim()) return "La descripcion es obligatoria.";

    if (Number(form.precio) <= 0) {
      return "El precio debe ser mayor que cero.";
    }

    if (Number(form.categoriaId) <= 0) {
      return "Debes seleccionar una categoria.";
    }

    if (!["1", "2"].includes(String(form.estado))) {
      return "Debes seleccionar un estado valido.";
    }

    if (!esEdicion && !(form.imagen instanceof File)) {
      return "Debes seleccionar una imagen.";
    }

    if (form.ingredientes.length === 0) {
      return "Debes agregar al menos un ingrediente.";
    }

    const ingredienteInvalido = form.ingredientes.some(
      (ingrediente) => Number(ingrediente.cantidad) <= 0
    );

    if (ingredienteInvalido) {
      return "Cada ingrediente debe tener una cantidad mayor que cero.";
    }

    return "";
  };

  const guardarPlato = async (event) => {
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
        precio: Number(form.precio),
        categoriaId: Number(form.categoriaId),
        estado: Number(form.estado),
        imagen: form.imagen,
        ingredientes: form.ingredientes.map((ingrediente) => ({
          productoId: ingrediente.productoId,
          cantidad: Number(ingrediente.cantidad),
        })),
      };

      if (esEdicion) {
        await editarPlato(plato.id, datos);
      } else {
        await crearPlato(datos);
      }

      await onSuccess?.();
      onClose?.();
    } catch (err) {
      setError(err.message || "No fue posible guardar el plato.");
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
      <div className="max-h-[92vh] w-full max-w-4xl overflow-y-auto rounded-2xl bg-white shadow-2xl">
        <div className="sticky top-0 z-10 flex items-center justify-between border-b border-slate-200 bg-white px-6 py-5">
          <div>
            <h2 className="text-2xl font-bold text-slate-800">
              {titulo}
            </h2>
            <p className="mt-1 text-sm text-slate-500">
              Define la informacion del plato y sus ingredientes.
            </p>
          </div>

          <button
            type="button"
            onClick={() => {
              if (!saving) {
                onClose?.();
              }
            }}
            disabled={saving}
            className="flex h-10 w-10 items-center justify-center rounded-xl text-slate-500 transition hover:bg-slate-100 disabled:opacity-50"
            aria-label="Cerrar modal"
          >
            <FaTimes />
          </button>
        </div>

        <form onSubmit={guardarPlato} className="p-6">
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
              placeholder="Ej. Pasta primavera"
              disabled={saving}
            />

            <FormField
              label="Precio"
              name="precio"
              type="number"
              min="0.01"
              step="0.01"
              value={form.precio}
              onChange={actualizarCampo}
              placeholder="0.00"
              disabled={saving}
            />

            <SelectField
              label="Categoria"
              name="categoriaId"
              value={form.categoriaId}
              onChange={actualizarCampo}
              disabled={saving}
            >
              <option value="">Selecciona una categoria</option>
              {categorias.map((categoria) => (
                <option key={categoria.id} value={categoria.id}>
                  {categoria.nombre}
                </option>
              ))}
            </SelectField>

            <SelectField
              label="Estado"
              name="estado"
              value={form.estado}
              onChange={actualizarCampo}
              disabled={saving}
            >
              <option value="1">Disponible</option>
              <option value="2">Agotado</option>
            </SelectField>

            <div className="md:col-span-2">
              <label
                htmlFor="descripcion"
                className="mb-2 block text-sm font-semibold text-slate-700"
              >
                Descripcion
              </label>
              <textarea
                id="descripcion"
                name="descripcion"
                value={form.descripcion}
                onChange={actualizarCampo}
                rows={4}
                placeholder="Describe brevemente el plato."
                disabled={saving}
                className="w-full resize-none rounded-xl border border-slate-300 px-4 py-3 text-slate-700 outline-none transition focus:border-violet-500 focus:ring-2 focus:ring-violet-100 disabled:bg-slate-100"
              />
            </div>

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
                    PNG, JPG o WEBP. Maximo 5 MB.
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
                      <FaUtensils className="mx-auto text-4xl" />
                      <p className="mt-2 text-sm">Sin imagen</p>
                    </div>
                  )}
                </div>
              </div>
            </div>
          </div>

          <section className="mt-7 border-t border-slate-200 pt-6">
            <div className="mb-4 flex flex-col gap-1">
              <h3 className="text-lg font-bold text-slate-800">
                Ingredientes
              </h3>
              <p className="text-sm text-slate-500">
                Selecciona productos del inventario y la cantidad por
                unidad del plato.
              </p>
            </div>

            <div className="grid gap-3 md:grid-cols-[1fr_150px_auto]">
              <SelectField
                label="Producto"
                name="productoId"
                value={ingredientForm.productoId}
                onChange={actualizarIngrediente}
                disabled={saving || productosDisponibles.length === 0}
              >
                <option value="">Selecciona un producto</option>
                {productosDisponibles.map((producto) => (
                  <option key={producto.id} value={producto.id}>
                    {producto.nombre} ({producto.unidadMedida})
                  </option>
                ))}
              </SelectField>

              <FormField
                label="Cantidad"
                name="cantidad"
                type="number"
                min="0.01"
                step="0.01"
                value={ingredientForm.cantidad}
                onChange={actualizarIngrediente}
                placeholder="0"
                disabled={saving}
              />

              <div className="flex items-end">
                <button
                  type="button"
                  onClick={agregarIngrediente}
                  disabled={saving || productosDisponibles.length === 0}
                  className="inline-flex h-12 w-full items-center justify-center gap-2 rounded-xl bg-slate-800 px-4 font-semibold text-white transition hover:bg-slate-900 disabled:cursor-not-allowed disabled:opacity-50 md:w-auto"
                >
                  <FaPlus />
                  Agregar
                </button>
              </div>
            </div>

            <div className="mt-5 overflow-hidden rounded-2xl border border-slate-200">
              {form.ingredientes.length === 0 ? (
                <div className="flex min-h-32 flex-col items-center justify-center px-5 py-8 text-center text-slate-500">
                  <FaUtensils className="text-3xl text-slate-300" />
                  <p className="mt-3 text-sm">
                    Aun no hay ingredientes seleccionados.
                  </p>
                </div>
              ) : (
                <div className="overflow-x-auto">
                  <table className="min-w-full divide-y divide-slate-200">
                    <thead className="bg-slate-50">
                      <tr>
                        {[
                          "Ingrediente",
                          "Unidad",
                          "Cantidad",
                          "Acciones",
                        ].map((title) => (
                          <th
                            key={title}
                            className="px-4 py-3 text-left text-xs font-bold uppercase tracking-wide text-slate-500"
                          >
                            {title}
                          </th>
                        ))}
                      </tr>
                    </thead>
                    <tbody className="divide-y divide-slate-200">
                      {form.ingredientes.map((ingrediente) => (
                        <tr key={ingrediente.productoId}>
                          <td className="px-4 py-3 font-semibold text-slate-800">
                            {ingrediente.nombre}
                          </td>
                          <td className="px-4 py-3 text-sm text-slate-600">
                            {ingrediente.unidadMedida}
                          </td>
                          <td className="px-4 py-3">
                            <input
                              type="number"
                              min="0.01"
                              step="0.01"
                              value={ingrediente.cantidad}
                              onChange={(event) =>
                                actualizarCantidadSeleccionada(
                                  ingrediente.productoId,
                                  event.target.value
                                )
                              }
                              disabled={saving}
                              className="w-32 rounded-lg border border-slate-300 px-3 py-2 text-slate-700 outline-none focus:border-violet-500 focus:ring-2 focus:ring-violet-100 disabled:bg-slate-100"
                            />
                          </td>
                          <td className="px-4 py-3">
                            <button
                              type="button"
                              onClick={() =>
                                eliminarIngrediente(
                                  ingrediente.productoId
                                )
                              }
                              disabled={saving}
                              className="flex h-9 w-9 items-center justify-center rounded-lg text-red-600 transition hover:bg-red-50 disabled:opacity-50"
                              aria-label="Eliminar ingrediente"
                              title="Eliminar ingrediente"
                            >
                              <FaTrash />
                            </button>
                          </td>
                        </tr>
                      ))}
                    </tbody>
                  </table>
                </div>
              )}
            </div>
          </section>

          <div className="mt-7 flex flex-col-reverse gap-3 border-t border-slate-200 pt-5 sm:flex-row sm:justify-end">
            <button
              type="button"
              onClick={() => {
                if (!saving) {
                  onClose?.();
                }
              }}
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
                  : "Guardar plato"}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}

function normalizarEstado(estado) {
  const value = String(estado ?? "").toLowerCase();

  if (estado === 2 || value === "2" || value === "agotado") {
    return "2";
  }

  return "1";
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

function SelectField({
  label,
  name,
  value,
  onChange,
  disabled,
  children,
}) {
  return (
    <div>
      <label
        htmlFor={name}
        className="mb-2 block text-sm font-semibold text-slate-700"
      >
        {label}
      </label>
      <select
        id={name}
        name={name}
        value={value}
        onChange={onChange}
        disabled={disabled}
        className="w-full rounded-xl border border-slate-300 bg-white px-4 py-3 text-slate-700 outline-none transition focus:border-violet-500 focus:ring-2 focus:ring-violet-100 disabled:bg-slate-100"
      >
        {children}
      </select>
    </div>
  );
}

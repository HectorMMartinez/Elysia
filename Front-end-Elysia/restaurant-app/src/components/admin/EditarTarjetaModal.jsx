import { useEffect, useState } from "react";
import { FaTimes, FaCreditCard, FaLock } from "react-icons/fa";

const TIPOS_TARJETA = [
  { value: 0, label: "Visa" },
  { value: 1, label: "Mastercard" },
  { value: 2, label: "American Express" },
  { value: 3, label: "Otro" },
];


const normalizarAnioVencimiento = (anio) => {
  const numero = Number(anio);

  if (numero >= 0 && numero <= 99) {
    return 2000 + numero;
  }

  return numero;
};

export default function EditarTarjetaModal({
  tarjeta,
  loading,
  onConfirm,
  onCancel,
}) {
  const [form, setForm] = useState({
    nombreTitular: "",
    numeroTarjeta: "",
    cvv: "",
    mesVencimiento: 1,
    anioVencimiento: 2026,
    tipo: 0,
  });

  const [errors, setErrors] = useState({});

  useEffect(() => {
    if (tarjeta) {
      setForm({
        nombreTitular: tarjeta.nombreTitular || "",
        numeroTarjeta: tarjeta.numeroTarjeta || "",
        cvv: tarjeta.cvv || "",

        mesVencimiento:
          tarjeta.mesVencimiento || 1,

        anioVencimiento:
          normalizarAnioVencimiento(
            tarjeta.anioVencimiento
          ) || 2026,

        tipo:
          tarjeta.tipo ?? 0,
      });

      setErrors({});
    }
  }, [tarjeta]);


  const handleChange = (e) => {
    const { name, value } = e.target;

    setForm((prev) => ({
      ...prev,

      [name]:
        name === "mesVencimiento" ||
        name === "anioVencimiento" ||
        name === "tipo"
          ? Number(value)
          : value,
    }));
  };

  const validate = () => {
    const newErrors = {};

    if (!form.nombreTitular.trim()) {
      newErrors.nombreTitular =
        "El nombre del titular es obligatorio";
    }

    if (!form.numeroTarjeta.trim()) {
      newErrors.numeroTarjeta =
        "El número de tarjeta es obligatorio";
    } else if (
      !/^\d{13,19}$/.test(
        form.numeroTarjeta.replace(/\s/g, "")
      )
    ) {
      newErrors.numeroTarjeta =
        "Número de tarjeta inválido";
    }

    if (!form.cvv.trim()) {
      newErrors.cvv =
        "El CVV es obligatorio";
    } else if (!/^\d{3,4}$/.test(form.cvv)) {
      newErrors.cvv =
        "CVV inválido";
    }

    if (
      form.mesVencimiento < 1 ||
      form.mesVencimiento > 12
    ) {
      newErrors.mesVencimiento =
        "Mes inválido";
    }

    if (
      form.anioVencimiento < 2026 ||
      form.anioVencimiento > 2050
    ) {
      newErrors.anioVencimiento =
        "Año inválido";
    }

    setErrors(newErrors);

    return Object.keys(newErrors).length === 0;
  };

  const handleSubmit = (e) => {
    e.preventDefault();

    if (!validate()) {
      return;
    }

    onConfirm({
      nombreTitular:
        form.nombreTitular.trim(),

      numeroTarjeta:
        form.numeroTarjeta.replace(/\s/g, ""),

      cvv:
        form.cvv,

      mesVencimiento:
        form.mesVencimiento,

      anioVencimiento:
        form.anioVencimiento,

      tipo:
        form.tipo,
    });
  };

  if (!tarjeta) {
    return null;
  }

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center p-4 bg-black/50 backdrop-blur-sm">

      <div className="bg-white rounded-2xl shadow-2xl w-full max-w-lg overflow-hidden">

        {/* =====================================================
            HEADER
        ====================================================== */}

        <div className="flex items-center justify-between px-6 py-4 border-b border-slate-100">

          <div className="flex items-center gap-3">

            <div className="w-10 h-10 rounded-xl bg-violet-100 flex items-center justify-center">

              <FaCreditCard className="text-violet-600" />

            </div>

            <div>

              <h3 className="font-semibold text-slate-800">
                Editar tarjeta
              </h3>

              <p className="text-xs text-slate-500">
                {tarjeta.nombreRestaurante}
              </p>

            </div>

          </div>

          <button
            type="button"
            onClick={onCancel}
            disabled={loading}
            className="p-2 rounded-lg text-slate-400 hover:bg-slate-100 hover:text-slate-600 transition disabled:opacity-50"
          >
            <FaTimes />
          </button>

        </div>


        {/* =====================================================
            BODY
        ====================================================== */}

        <form
          onSubmit={handleSubmit}
          className="p-6 space-y-4"
        >

          {/* =================================================
              NOMBRE TITULAR
          ================================================= */}

          <div>

            <label className="block text-sm font-medium text-slate-700 mb-1.5">
              Nombre del titular
            </label>

            <input
              type="text"
              name="nombreTitular"
              value={form.nombreTitular}
              onChange={handleChange}
              disabled={loading}
              className={`w-full px-4 py-2.5 border rounded-xl outline-none focus:ring-2 focus:ring-violet-500/30 focus:border-violet-500 transition ${
                errors.nombreTitular
                  ? "border-red-300"
                  : "border-slate-200"
              }`}
              placeholder="Nombre completo del titular"
            />

            {errors.nombreTitular && (
              <p className="text-xs text-red-500 mt-1">
                {errors.nombreTitular}
              </p>
            )}

          </div>


          {/* =================================================
              NÚMERO TARJETA
          ================================================= */}

          <div>

            <label className="block text-sm font-medium text-slate-700 mb-1.5">
              Número de tarjeta
            </label>

            <input
              type="text"
              name="numeroTarjeta"
              value={form.numeroTarjeta}
              onChange={handleChange}
              disabled={loading}
              maxLength={19}
              className={`w-full px-4 py-2.5 border rounded-xl outline-none focus:ring-2 focus:ring-violet-500/30 focus:border-violet-500 transition font-mono ${
                errors.numeroTarjeta
                  ? "border-red-300"
                  : "border-slate-200"
              }`}
              placeholder="4111111111111111"
            />

            {errors.numeroTarjeta && (
              <p className="text-xs text-red-500 mt-1">
                {errors.numeroTarjeta}
              </p>
            )}

          </div>


          {/* =================================================
              CVV + TIPO
          ================================================= */}

          <div className="grid grid-cols-2 gap-4">

            {/* CVV */}

            <div>

              <label className="block text-sm font-medium text-slate-700 mb-1.5">
                CVV
              </label>

              <div className="relative">

                <input
                  type="password"
                  name="cvv"
                  value={form.cvv}
                  onChange={handleChange}
                  disabled={loading}
                  maxLength={4}
                  className={`w-full px-4 py-2.5 border rounded-xl outline-none focus:ring-2 focus:ring-violet-500/30 focus:border-violet-500 transition font-mono ${
                    errors.cvv
                      ? "border-red-300"
                      : "border-slate-200"
                  }`}
                  placeholder="•••"
                />

                <FaLock className="absolute right-3 top-1/2 -translate-y-1/2 text-slate-300 text-sm" />

              </div>

              {errors.cvv && (
                <p className="text-xs text-red-500 mt-1">
                  {errors.cvv}
                </p>
              )}

            </div>


            {/* TIPO */}

            <div>

              <label className="block text-sm font-medium text-slate-700 mb-1.5">
                Tipo
              </label>

              <select
                name="tipo"
                value={form.tipo}
                onChange={handleChange}
                disabled={loading}
                className="w-full px-4 py-2.5 border border-slate-200 rounded-xl outline-none focus:ring-2 focus:ring-violet-500/30 focus:border-violet-500 bg-white transition"
              >

                {TIPOS_TARJETA.map((t) => (
                  <option
                    key={t.value}
                    value={t.value}
                  >
                    {t.label}
                  </option>
                ))}

              </select>

            </div>

          </div>


          {/* =================================================
              MES Y AÑO
          ================================================= */}

          <div className="grid grid-cols-2 gap-4">

            {/* MES */}

            <div>

              <label className="block text-sm font-medium text-slate-700 mb-1.5">
                Mes
              </label>

              <select
                name="mesVencimiento"
                value={form.mesVencimiento}
                onChange={handleChange}
                disabled={loading}
                className={`w-full px-4 py-2.5 border rounded-xl outline-none focus:ring-2 focus:ring-violet-500/30 focus:border-violet-500 bg-white transition ${
                  errors.mesVencimiento
                    ? "border-red-300"
                    : "border-slate-200"
                }`}
              >

                {Array.from(
                  { length: 12 },
                  (_, i) => i + 1
                ).map((m) => (

                  <option
                    key={m}
                    value={m}
                  >
                    {String(m).padStart(2, "0")}
                  </option>

                ))}

              </select>

              {errors.mesVencimiento && (
                <p className="text-xs text-red-500 mt-1">
                  {errors.mesVencimiento}
                </p>
              )}

            </div>


            {/* AÑO */}

            <div>

              <label className="block text-sm font-medium text-slate-700 mb-1.5">
                Año
              </label>

              <select
                name="anioVencimiento"
                value={form.anioVencimiento}
                onChange={handleChange}
                disabled={loading}
                className={`w-full px-4 py-2.5 border rounded-xl outline-none focus:ring-2 focus:ring-violet-500/30 focus:border-violet-500 bg-white transition ${
                  errors.anioVencimiento
                    ? "border-red-300"
                    : "border-slate-200"
                }`}
              >

                {Array.from(
                  { length: 25 },
                  (_, i) => 2026 + i
                ).map((y) => (

                  <option
                    key={y}
                    value={y}
                  >
                    {y}
                  </option>

                ))}

              </select>

              {errors.anioVencimiento && (
                <p className="text-xs text-red-500 mt-1">
                  {errors.anioVencimiento}
                </p>
              )}

            </div>

          </div>


          {/* =================================================
              ACTIONS
          ================================================= */}

          <div className="flex gap-3 pt-2">

            <button
              type="button"
              onClick={onCancel}
              disabled={loading}
              className="flex-1 px-4 py-2.5 rounded-xl border border-slate-200 text-slate-600 font-medium hover:bg-slate-50 transition disabled:opacity-50"
            >
              Cancelar
            </button>

            <button
              type="submit"
              disabled={loading}
              className="flex-1 px-4 py-2.5 rounded-xl bg-violet-600 text-white font-medium hover:bg-violet-700 transition disabled:opacity-50 flex items-center justify-center gap-2"
            >

              {loading ? (
                <>
                  <div className="w-4 h-4 border-2 border-white/30 border-t-white rounded-full animate-spin" />

                  Guardando...
                </>
              ) : (
                "Guardar cambios"
              )}

            </button>

          </div>

        </form>

      </div>

    </div>
  );
}
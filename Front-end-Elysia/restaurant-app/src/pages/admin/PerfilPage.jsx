import { useEffect, useState } from "react";
import {
  FaUser,
  FaEnvelope,
  FaPhone,
  FaLock,
  FaCamera,
  FaSave,
  FaSpinner,
  FaTimes,
  FaExclamationTriangle,
  FaCheckCircle,
  FaShieldAlt,
  FaIdBadge,
  FaUserCircle,
} from "react-icons/fa";

import managerAccountService from "../../services/managerAccountService";
import AdminSidebar from "../../components/layout/AdminSidebar";

export default function ProfilePage() {
  const [perfil, setPerfil] = useState(null);

  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState("");
  const API_URL = "https://localhost:7108";
  const [confirmModal, setConfirmModal] = useState(false);

  const [form, setForm] = useState({
    name: "",
    lastName: "",
    email: "",
    userName: "",
    password: "",
    phone: "",
    profileImage: null,
  });

  const [previewImage, setPreviewImage] = useState(null);

  const cargarPerfil = async () => {
    try {
      setLoading(true);
      setError("");

      const response = await managerAccountService.getPerfil();

      const data = response?.data ?? response;

      if (!data) {
        setError("No se pudo obtener la información del perfil.");
        return;
      }

      setPerfil(data);

      setForm({
        name: data.name || "",
        lastName: data.lastName || "",
        email: data.email || "",
        userName: data.userName || "",
        password: "",
        phone: data.phone || "",
        profileImage: null,
      });

      setPreviewImage(data.profileImage || null);
    } catch (err) {
      console.error("Error al cargar perfil:", err);

      setError(obtenerMensajeError(err));
       
    } finally {
      setLoading(false);
    }
  };

  const obtenerMensajeError = (err) => {
  const data = err?.response?.data;

  if (!data) {
    return "No fue posible actualizar el perfil.";
  }

  if (data.errors) {
    const mensajes = Object.entries(data.errors)
      .flatMap(([campo, errores]) =>
        Array.isArray(errores)
          ? errores.map((mensaje) => `${campo}: ${mensaje}`)
          : []
      );

    if (mensajes.length > 0) {
      return mensajes.join(" ");
    }
  }

  if (typeof data.message === "string") {
    return data.message;
  }

  if (typeof data.title === "string") {
    return data.title;
  }

  if (typeof data === "string") {
    return data;
  }

  return "No fue posible actualizar el perfil.";
};




  useEffect(() => {
    cargarPerfil();
  }, []);


  const handleChange = (e) => {
    const { name, value } = e.target;

    setForm((prev) => ({
      ...prev,
      [name]: value,
    }));

    if (error) {
      setError("");
    }
  };


  const handleImageChange = (e) => {
    const file = e.target.files?.[0];

    if (!file) return;

    if (!file.type.startsWith("image/")) {
      setError("El archivo seleccionado debe ser una imagen.");
      return;
    }

    if (file.size > 5 * 1024 * 1024) {
      setError("La imagen no puede superar los 5 MB.");
      return;
    }

    setForm((prev) => ({
      ...prev,
      profileImage: file,
    }));

    setPreviewImage(URL.createObjectURL(file));
    setError("");
  };


  const validarFormulario = () => {
    if (!form.name.trim()) {
      setError("El nombre es obligatorio.");
      return false;
    }

    if (!form.lastName.trim()) {
      setError("El apellido es obligatorio.");
      return false;
    }

    if (!form.email.trim()) {
      setError("El correo electrónico es obligatorio.");
      return false;
    }

    if (!form.userName.trim()) {
      setError("El nombre de usuario es obligatorio.");
      return false;
    }

    if (
      form.password.trim() &&
      form.password.trim().length < 6
    ) {
      setError(
        "La nueva contraseña debe tener al menos 6 caracteres."
      );
      return false;
    }

    return true;
  };


  const handleSubmit = (e) => {
    e.preventDefault();

    setError("");

    if (!validarFormulario()) return;

    setConfirmModal(true);
  };

  
  const cancelarEdicion = () => {
    if (saving) return;

    setConfirmModal(false);
  };



  const confirmarEdicion = async () => {
    try {
      setSaving(true);
      setError("");

      const formData = new FormData();

      formData.append("Name", form.name.trim());
      formData.append("LastName", form.lastName.trim());
      formData.append("Email", form.email.trim());
      formData.append("UserName", form.userName.trim());

      if (form.password.trim()) {
        formData.append("Password", form.password.trim());
      }

      if (form.phone.trim()) {
        formData.append("Phone", form.phone.trim());
      }

      if (form.profileImage) {
        formData.append("ProfileImage", form.profileImage);
      }

      await managerAccountService.editarPerfil(formData);

      setConfirmModal(false);

      await cargarPerfil();
    } catch (err) {
      console.error("Error al editar perfil:", err);

      setError(obtenerMensajeError(err));
    } finally {
      setSaving(false);
    }
  };


  if (loading) {
    return (
      <div className="min-h-screen bg-slate-950">

        {/* SIDEBAR FIJO */}
        <div className="fixed left-0 top-0 z-50 h-screen">
          <AdminSidebar />
        </div>

        {/* CONTENIDO */}
        <main className="lg:ml-64 min-h-screen flex items-center justify-center">

          <div className="flex flex-col items-center gap-4">

            <div className="w-14 h-14 rounded-2xl bg-violet-500/10 border border-violet-500/20 flex items-center justify-center">

              <FaSpinner className="text-violet-400 text-xl animate-spin" />

            </div>

            <div className="text-center">

              <p className="text-white font-semibold">
                Cargando perfil
              </p>

              <p className="text-slate-500 text-sm mt-1">
                Estamos preparando tu información...
              </p>

            </div>

          </div>

        </main>
      </div>
    );
  }

  

  return (
    <div className="min-h-screen bg-slate-950 text-slate-200">

      {/* ========================================================
          SIDEBAR FIJO
      ======================================================== */}

      <div className="fixed left-0 top-0 z-50 h-screen">
        <AdminSidebar />
      </div>

      {/* ========================================================
          CONTENIDO
      ======================================================== */}

      <main className="lg:ml-64 min-h-screen">

        {/* Fondo decorativo */}

        <div className="fixed inset-0 pointer-events-none overflow-hidden">

          <div className="absolute -top-40 -right-40 w-96 h-96 bg-violet-600/10 rounded-full blur-3xl" />

          <div className="absolute top-1/2 -left-40 w-96 h-96 bg-indigo-600/5 rounded-full blur-3xl" />

        </div>

        <div className="relative px-4 sm:px-6 lg:px-10 py-6 lg:py-10 max-w-7xl mx-auto">

          {/* ====================================================
              HEADER
          ==================================================== */}

          <div className="mb-8">

            <div className="flex flex-col md:flex-row md:items-center md:justify-between gap-5">

              <div>

                <div className="flex items-center gap-2 text-violet-400 text-sm font-medium mb-2">

                  <FaUserCircle />

                  <span>Cuenta administrativa</span>

                </div>

                <h1 className="text-3xl lg:text-4xl font-bold text-white tracking-tight">
                  Mi perfil
                </h1>

                <p className="text-slate-400 mt-2 max-w-xl">
                  Administra tu información personal, credenciales
                  y configuración de seguridad.
                </p>

              </div>

              <div className="flex items-center gap-3 px-4 py-3 rounded-2xl bg-white/[0.04] border border-white/[0.08]">

                <div className="w-10 h-10 rounded-xl bg-emerald-500/10 border border-emerald-500/20 flex items-center justify-center">

                  <FaShieldAlt className="text-emerald-400" />

                </div>

                <div>

                  <p className="text-xs text-slate-500">
                    Estado de cuenta
                  </p>

                  <p className="text-sm font-semibold text-emerald-400">
                    Cuenta protegida
                  </p>

                </div>

              </div>

            </div>

          </div>

          {/* ====================================================
              ERROR
          ==================================================== */}

          {error && (
            <div className="mb-6 rounded-2xl border border-red-500/20 bg-red-500/5 px-5 py-4 flex items-start gap-4">

              <div className="w-10 h-10 rounded-xl bg-red-500/10 flex items-center justify-center shrink-0">

                <FaExclamationTriangle className="text-red-400" />

              </div>

              <div>

                <p className="text-sm font-semibold text-red-300">
                  Ocurrió un problema
                </p>

                <p className="text-sm text-red-400/80 mt-1">
                  {error}
                </p>

              </div>

            </div>
          )}

          {/* ====================================================
              PERFIL
          ==================================================== */}

          <div className="relative overflow-hidden rounded-3xl border border-white/[0.08] bg-gradient-to-br from-violet-600/20 via-slate-900 to-slate-950 shadow-2xl mb-6">

            <div className="absolute -right-20 -top-20 w-72 h-72 rounded-full bg-violet-500/10 blur-3xl" />

            <div className="relative p-6 lg:p-8">

              <div className="flex flex-col sm:flex-row items-center sm:items-start gap-6">

                {/* FOTO */}

                <div className="relative shrink-0">

                  <div className="p-1 rounded-full bg-gradient-to-br from-violet-400 to-indigo-600">

                    {previewImage ? (
                      <img
                        src={`${API_URL}/${previewImage}`}
                        alt="Perfil"
                        className="w-28 h-28 rounded-full object-cover border-4 border-slate-950"
                      />
                    ) : (
                      <div className="w-28 h-28 rounded-full bg-slate-900 flex items-center justify-center border-4 border-slate-950">

                        <FaUser className="text-violet-400 text-4xl" />

                      </div>
                    )}

                  </div>

                  <label
                    htmlFor="profileImage"
                    className="absolute bottom-1 right-1 w-10 h-10 rounded-xl bg-violet-600 border-4 border-slate-950 text-white flex items-center justify-center cursor-pointer hover:bg-violet-500 transition shadow-lg"
                  >
                    <FaCamera className="text-sm" />
                  </label>

                  <input
                    id="profileImage"
                    type="file"
                    accept="image/*"
                    onChange={handleImageChange}
                    className="hidden"
                    disabled={saving}
                  />

                </div>

                {/* INFORMACIÓN */}

                <div className="text-center sm:text-left flex-1">

                  <p className="text-sm text-violet-300 font-medium">
                    Perfil de administrador
                  </p>

                  <h2 className="text-2xl font-bold text-white mt-1">
                    {form.name} {form.lastName}
                  </h2>

                  <p className="text-slate-400 mt-1">
                    @{form.userName}
                  </p>

                  <div className="flex flex-wrap justify-center sm:justify-start gap-2 mt-4">

                    <span className="inline-flex items-center gap-2 px-3 py-1.5 rounded-lg bg-white/[0.05] border border-white/[0.08] text-xs text-slate-300">

                      <FaEnvelope className="text-violet-400" />

                      {form.email}

                    </span>

                    {form.phone && (
                      <span className="inline-flex items-center gap-2 px-3 py-1.5 rounded-lg bg-white/[0.05] border border-white/[0.08] text-xs text-slate-300">

                        <FaPhone className="text-violet-400" />

                        {form.phone}

                      </span>
                    )}

                  </div>

                </div>

              </div>

            </div>

          </div>

          {/* ====================================================
              FORMULARIO
          ==================================================== */}

          <form onSubmit={handleSubmit}>

            <div className="grid grid-cols-1 xl:grid-cols-3 gap-6">

              {/* INFORMACIÓN PERSONAL */}

              <section className="xl:col-span-2 rounded-3xl border border-white/[0.08] bg-slate-900/80 backdrop-blur-sm overflow-hidden shadow-xl">

                <div className="px-6 py-5 border-b border-white/[0.07]">

                  <div className="flex items-center gap-3">

                    <div className="w-10 h-10 rounded-xl bg-violet-500/10 border border-violet-500/20 flex items-center justify-center">

                      <FaIdBadge className="text-violet-400" />

                    </div>

                    <div>

                      <h2 className="font-semibold text-white">
                        Información personal
                      </h2>

                      <p className="text-xs text-slate-500 mt-0.5">
                        Datos básicos de tu cuenta
                      </p>

                    </div>

                  </div>

                </div>

                <div className="p-6">

                  <div className="grid grid-cols-1 md:grid-cols-2 gap-5">

                    {/* NOMBRE */}

                    <div>

                      <label className="block text-sm font-medium text-slate-300 mb-2">
                        Nombre
                      </label>

                      <div className="relative">

                        <FaUser className="absolute left-4 top-1/2 -translate-y-1/2 text-slate-500" />

                        <input
                          type="text"
                          name="name"
                          value={form.name}
                          onChange={handleChange}
                          disabled={saving}
                          className="w-full pl-11 pr-4 py-3.5 rounded-xl bg-slate-950/70 border border-white/[0.08] text-white outline-none focus:border-violet-500 focus:ring-4 focus:ring-violet-500/10 transition"
                        />

                      </div>

                    </div>

                    {/* APELLIDO */}

                    <div>

                      <label className="block text-sm font-medium text-slate-300 mb-2">
                        Apellido
                      </label>

                      <div className="relative">

                        <FaUser className="absolute left-4 top-1/2 -translate-y-1/2 text-slate-500" />

                        <input
                          type="text"
                          name="lastName"
                          value={form.lastName}
                          onChange={handleChange}
                          disabled={saving}
                          className="w-full pl-11 pr-4 py-3.5 rounded-xl bg-slate-950/70 border border-white/[0.08] text-white outline-none focus:border-violet-500 focus:ring-4 focus:ring-violet-500/10 transition"
                        />

                      </div>

                    </div>

                    {/* EMAIL */}

                    <div>

                      <label className="block text-sm font-medium text-slate-300 mb-2">
                        Correo electrónico
                      </label>

                      <div className="relative">

                        <FaEnvelope className="absolute left-4 top-1/2 -translate-y-1/2 text-slate-500" />

                        <input
                          type="email"
                          name="email"
                          value={form.email}
                          onChange={handleChange}
                          disabled={saving}
                          className="w-full pl-11 pr-4 py-3.5 rounded-xl bg-slate-950/70 border border-white/[0.08] text-white outline-none focus:border-violet-500 focus:ring-4 focus:ring-violet-500/10 transition"
                        />

                      </div>

                    </div>

                    {/* USERNAME */}

                    <div>

                      <label className="block text-sm font-medium text-slate-300 mb-2">
                        Nombre de usuario
                      </label>

                      <div className="relative">

                        <FaUserCircle className="absolute left-4 top-1/2 -translate-y-1/2 text-slate-500" />

                        <input
                          type="text"
                          name="userName"
                          value={form.userName}
                          onChange={handleChange}
                          disabled={saving}
                          className="w-full pl-11 pr-4 py-3.5 rounded-xl bg-slate-950/70 border border-white/[0.08] text-white outline-none focus:border-violet-500 focus:ring-4 focus:ring-violet-500/10 transition"
                        />

                      </div>

                    </div>

                    {/* TELÉFONO */}

                    <div className="md:col-span-2">

                      <label className="block text-sm font-medium text-slate-300 mb-2">
                        Teléfono
                      </label>

                      <div className="relative">

                        <FaPhone className="absolute left-4 top-1/2 -translate-y-1/2 text-slate-500" />

                        <input
                          type="text"
                          name="phone"
                          value={form.phone}
                          onChange={handleChange}
                          disabled={saving}
                          placeholder="Ingresa tu número de teléfono"
                          className="w-full pl-11 pr-4 py-3.5 rounded-xl bg-slate-950/70 border border-white/[0.08] text-white placeholder-slate-600 outline-none focus:border-violet-500 focus:ring-4 focus:ring-violet-500/10 transition"
                        />

                      </div>

                    </div>

                  </div>

                </div>

              </section>

              {/* SEGURIDAD */}

              <section className="rounded-3xl border border-white/[0.08] bg-slate-900/80 backdrop-blur-sm overflow-hidden shadow-xl h-fit">

                <div className="px-6 py-5 border-b border-white/[0.07]">

                  <div className="flex items-center gap-3">

                    <div className="w-10 h-10 rounded-xl bg-amber-500/10 border border-amber-500/20 flex items-center justify-center">

                      <FaShieldAlt className="text-amber-400" />

                    </div>

                    <div>

                      <h2 className="font-semibold text-white">
                        Seguridad
                      </h2>

                      <p className="text-xs text-slate-500 mt-0.5">
                        Protege tu cuenta
                      </p>

                    </div>

                  </div>

                </div>

                <div className="p-6">

                  <div className="rounded-2xl bg-amber-500/5 border border-amber-500/10 p-4 mb-5">

                    <div className="flex gap-3">

                      <FaLock className="text-amber-400 mt-0.5 shrink-0" />

                      <p className="text-xs leading-relaxed text-slate-400">
                        Si no deseas modificar tu contraseña,
                        simplemente deja este campo vacío.
                      </p>

                    </div>

                  </div>

                  <label className="block text-sm font-medium text-slate-300 mb-2">
                    Nueva contraseña
                  </label>

                  <div className="relative">

                    <FaLock className="absolute left-4 top-1/2 -translate-y-1/2 text-slate-500" />

                    <input
                      type="password"
                      name="password"
                      value={form.password}
                      onChange={handleChange}
                      disabled={saving}
                      placeholder="••••••••"
                      className="w-full pl-11 pr-4 py-3.5 rounded-xl bg-slate-950/70 border border-white/[0.08] text-white placeholder-slate-600 outline-none focus:border-violet-500 focus:ring-4 focus:ring-violet-500/10 transition"
                    />

                  </div>

                  <p className="text-xs text-slate-600 mt-2">
                    Mínimo 6 caracteres.
                  </p>

                </div>

              </section>

            </div>

            {/* ==================================================
                GUARDAR
            ================================================== */}

            <div className="mt-6 rounded-3xl border border-white/[0.08] bg-slate-900/70 p-5 flex flex-col sm:flex-row items-center justify-between gap-4">

              <div className="flex items-center gap-3">

                <div className="w-10 h-10 rounded-xl bg-emerald-500/10 border border-emerald-500/20 flex items-center justify-center">

                  <FaCheckCircle className="text-emerald-400" />

                </div>

                <div>

                  <p className="text-sm font-medium text-white">
                    ¿Terminaste de editar?
                  </p>

                  <p className="text-xs text-slate-500 mt-0.5">
                    Revisa los datos antes de guardar.
                  </p>

                </div>

              </div>

              <button
                type="submit"
                disabled={saving}
                className="w-full sm:w-auto min-w-[190px] flex items-center justify-center gap-2 px-6 py-3.5 rounded-xl bg-gradient-to-r from-violet-600 to-indigo-600 text-white font-semibold shadow-lg shadow-violet-600/20 hover:from-violet-500 hover:to-indigo-500 transition-all duration-200 disabled:opacity-50 disabled:cursor-not-allowed"
              >

                {saving ? (
                  <>
                    <FaSpinner className="animate-spin" />
                    Guardando...
                  </>
                ) : (
                  <>
                    <FaSave />
                    Guardar cambios
                  </>
                )}

              </button>

            </div>

          </form>

        </div>

      </main>

      {/* ========================================================
          MODAL
      ======================================================== */}

      {confirmModal && (
        <div className="fixed inset-0 z-[100] flex items-center justify-center p-4">

          <div
            className="absolute inset-0 bg-slate-950/80 backdrop-blur-md"
            onClick={cancelarEdicion}
          />

          <div className="relative w-full max-w-md bg-slate-900 border border-white/[0.1] rounded-3xl shadow-2xl overflow-hidden">

            {/* HEADER */}

            <div className="px-6 py-5 border-b border-white/[0.07] flex items-center justify-between">

              <div className="flex items-center gap-3">

                <div className="w-11 h-11 rounded-xl bg-violet-500/10 border border-violet-500/20 flex items-center justify-center">

                  <FaCheckCircle className="text-violet-400 text-lg" />

                </div>

                <div>

                  <h3 className="text-lg font-semibold text-white">
                    Confirmar cambios
                  </h3>

                  <p className="text-xs text-slate-500 mt-0.5">
                    Actualización de perfil
                  </p>

                </div>

              </div>

              <button
                type="button"
                onClick={cancelarEdicion}
                disabled={saving}
                className="p-2 rounded-xl text-slate-500 hover:bg-white/[0.05] hover:text-white transition disabled:opacity-50"
              >
                <FaTimes />
              </button>

            </div>

            {/* CONTENIDO */}

            <div className="px-6 py-6">

              <p className="text-slate-400 leading-relaxed text-sm">
                ¿Estás seguro de que deseas guardar los cambios
                realizados en tu perfil?
              </p>

              <div className="mt-5 rounded-2xl bg-slate-950/70 border border-white/[0.07] p-4">

                <div className="flex items-center gap-3">

                  {previewImage ? (
                    <img
                      src={previewImage}
                      alt="Vista previa"
                      className="w-12 h-12 rounded-full object-cover border-2 border-slate-700"
                    />
                  ) : (
                    <div className="w-12 h-12 rounded-full bg-violet-500/10 border border-violet-500/20 flex items-center justify-center">

                      <FaUser className="text-violet-400" />

                    </div>
                  )}

                  <div className="min-w-0">

                    <p className="font-semibold text-white truncate">
                      {form.name} {form.lastName}
                    </p>

                    <p className="text-sm text-slate-500 truncate">
                      @{form.userName}
                    </p>

                  </div>

                </div>

              </div>

              <div className="mt-4 flex items-start gap-2 text-xs text-slate-600">

                <FaShieldAlt className="mt-0.5 shrink-0" />

                <span>
                  Los cambios se aplicarán a tu cuenta inmediatamente.
                </span>

              </div>

            </div>

            {/* ACCIONES */}

            <div className="px-6 py-4 bg-slate-950/50 border-t border-white/[0.07] flex gap-3">

              <button
                type="button"
                onClick={cancelarEdicion}
                disabled={saving}
                className="flex-1 px-4 py-3 rounded-xl border border-white/[0.08] bg-white/[0.03] text-slate-300 font-medium hover:bg-white/[0.06] transition disabled:opacity-50"
              >
                Cancelar
              </button>

              <button
                type="button"
                onClick={confirmarEdicion}
                disabled={saving}
                className="flex-1 px-4 py-3 rounded-xl bg-gradient-to-r from-violet-600 to-indigo-600 text-white font-semibold hover:from-violet-500 hover:to-indigo-500 transition flex items-center justify-center gap-2 disabled:opacity-50"
              >

                {saving ? (
                  <>
                    <FaSpinner className="animate-spin" />
                    Guardando...
                  </>
                ) : (
                  <>
                    <FaSave />
                    Confirmar
                  </>
                )}

              </button>

            </div>

          </div>

        </div>
      )}

    </div>
  );
}
import { useState, useEffect, useRef } from "react";
import {
  FaRobot,
  FaChartLine,
  FaUtensils,
  FaComments,
  FaSpinner,
  FaTimes,
  FaExclamationTriangle,
  FaCheckCircle,
  FaLightbulb,
  FaArrowRight,
  FaPaperPlane,
  FaUser,
  FaStore,
  FaBoxOpen,
  FaCalendarCheck,
  FaUsers,
  FaClipboardList,
  FaStar,
  FaExclamationCircle,
  FaTrashAlt,
  FaPlusCircle,
} from "react-icons/fa";
import centerIAService from "../../services/centerIAService";
import managerAccountService from "../../services/managerAccountService";
import OwnerSidebar from "../../components/layout/OwnerSidebar"; // ajusta la ruta si es necesario



function ConfirmModal({ open, title, description, onConfirm, onCancel, loading }) {
  if (!open) return null;

  return (
    <div className="fixed inset-0 z-[100] flex items-center justify-center p-4">
      <div
        className="absolute inset-0 bg-slate-950/80 backdrop-blur-md"
        onClick={loading ? undefined : onCancel}
      />
      <div className="relative w-full max-w-md bg-slate-900 border border-white/[0.1] rounded-3xl shadow-2xl overflow-hidden">
        <div className="px-6 py-5 border-b border-white/[0.07] flex items-center justify-between">
          <div className="flex items-center gap-3">
            <div className="w-11 h-11 rounded-xl bg-violet-500/10 border border-violet-500/20 flex items-center justify-center">
              <FaCheckCircle className="text-violet-400 text-lg" />
            </div>
            <div>
              <h3 className="text-lg font-semibold text-white">{title}</h3>
              <p className="text-xs text-slate-500 mt-0.5">Confirmación requerida</p>
            </div>
          </div>
          <button
            type="button"
            onClick={onCancel}
            disabled={loading}
            className="p-2 rounded-xl text-slate-500 hover:bg-white/[0.05] hover:text-white transition disabled:opacity-50"
          >
            <FaTimes />
          </button>
        </div>

        <div className="px-6 py-6">
          <p className="text-slate-400 leading-relaxed text-sm">{description}</p>
        </div>

        <div className="px-6 py-4 bg-slate-950/50 border-t border-white/[0.07] flex gap-3">
          <button
            type="button"
            onClick={onCancel}
            disabled={loading}
            className="flex-1 px-4 py-3 rounded-xl border border-white/[0.08] bg-white/[0.03] text-slate-300 font-medium hover:bg-white/[0.06] transition disabled:opacity-50"
          >
            Cancelar
          </button>
          <button
            type="button"
            onClick={onConfirm}
            disabled={loading}
            className="flex-1 px-4 py-3 rounded-xl bg-gradient-to-r from-violet-600 to-indigo-600 text-white font-semibold hover:from-violet-500 hover:to-indigo-500 transition flex items-center justify-center gap-2 disabled:opacity-50"
          >
            {loading ? (
              <>
                <FaSpinner className="animate-spin" />
                Procesando...
              </>
            ) : (
              <>
                <FaCheckCircle />
                Confirmar
              </>
            )}
          </button>
        </div>
      </div>
    </div>
  );
}


function AnalisisView({ data, onClose }) {
  const secciones = [
    { key: "analisisVentas", titulo: "Ventas", icon: FaChartLine, color: "emerald" },
    { key: "analisisPedidos", titulo: "Pedidos", icon: FaClipboardList, color: "blue" },
    { key: "analisisInventario", titulo: "Inventario", icon: FaBoxOpen, color: "amber" },
    { key: "analisisMenu", titulo: "Menú", icon: FaUtensils, color: "violet" },
    { key: "analisisReservas", titulo: "Reservas", icon: FaCalendarCheck, color: "cyan" },
    { key: "analisisEmpleados", titulo: "Empleados", icon: FaUsers, color: "rose" },
  ];

  const colorMap = {
    emerald: "bg-emerald-500/10 border-emerald-500/20 text-emerald-400",
    blue: "bg-blue-500/10 border-blue-500/20 text-blue-400",
    amber: "bg-amber-500/10 border-amber-500/20 text-amber-400",
    violet: "bg-violet-500/10 border-violet-500/20 text-violet-400",
    cyan: "bg-cyan-500/10 border-cyan-500/20 text-cyan-400",
    rose: "bg-rose-500/10 border-rose-500/20 text-rose-400",
  };

  return (
    <div className="space-y-6">
      {/* Header */}
      <div className="flex items-start justify-between gap-4">
        <div>
          <div className="flex items-center gap-2 text-violet-400 text-sm font-medium mb-1">
            <FaChartLine />
            <span>Análisis inteligente</span>
          </div>
          <h2 className="text-2xl font-bold text-white">Análisis del restaurante</h2>
          <p className="text-slate-400 text-sm mt-1">Últimos 30 días · Generado por CenterIA</p>
        </div>
        <button
          onClick={onClose}
          className="p-2.5 rounded-xl border border-white/[0.08] text-slate-400 hover:text-white hover:bg-white/[0.05] transition"
        >
          <FaTimes />
        </button>
      </div>

      {/* Resumen ejecutivo */}
      <div className="rounded-2xl border border-violet-500/20 bg-gradient-to-br from-violet-600/15 to-slate-900/80 p-6">
        <div className="flex items-center gap-3 mb-4">
          <div className="w-10 h-10 rounded-xl bg-violet-500/20 border border-violet-500/30 flex items-center justify-center">
            <FaLightbulb className="text-violet-400" />
          </div>
          <h3 className="font-semibold text-white text-lg">Resumen ejecutivo</h3>
        </div>
        <p className="text-slate-300 leading-relaxed text-[15px]">{data.resumen}</p>
      </div>

      {/* Secciones de análisis */}
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-5">
        {secciones.map(({ key, titulo, icon: Icon, color }) => (
          <div
            key={key}
            className="rounded-2xl border border-white/[0.08] bg-slate-900/70 p-5 hover:border-white/[0.12] transition"
          >
            <div className="flex items-center gap-3 mb-3">
              <div className={`w-9 h-9 rounded-xl border flex items-center justify-center ${colorMap[color]}`}>
                <Icon className="text-sm" />
              </div>
              <h4 className="font-semibold text-white">{titulo}</h4>
            </div>
            <p className="text-slate-400 text-sm leading-relaxed">{data[key]}</p>
          </div>
        ))}
      </div>

      {/* Recomendaciones */}
      <div className="rounded-2xl border border-emerald-500/20 bg-emerald-500/5 p-6">
        <div className="flex items-center gap-3 mb-5">
          <div className="w-10 h-10 rounded-xl bg-emerald-500/15 border border-emerald-500/25 flex items-center justify-center">
            <FaCheckCircle className="text-emerald-400" />
          </div>
          <h3 className="font-semibold text-white text-lg">Recomendaciones de acción</h3>
        </div>
        <ul className="space-y-3">
          {data.recomendaciones?.map((rec, i) => (
            <li key={i} className="flex gap-3 items-start">
              <span className="mt-1.5 w-1.5 h-1.5 rounded-full bg-emerald-400 shrink-0" />
              <p className="text-slate-300 text-sm leading-relaxed">{rec}</p>
            </li>
          ))}
        </ul>
      </div>
    </div>
  );
}


function OptimizacionView({ data, onClose }) {
  const bloques = [
    {
      key: "platosPromocionar",
      titulo: "Platos a promocionar",
      icon: FaStar,
      color: "emerald",
      descripcion: "Alto rendimiento · Recomendados para impulsar",
    },
    {
      key: "platosRevisarPrecio",
      titulo: "Revisar precio",
      icon: FaExclamationCircle,
      color: "amber",
      descripcion: "Baja rotación · Posible ajuste de precio",
    },
    {
      key: "platosRetirar",
      titulo: "Candidatos a retirar",
      icon: FaTrashAlt,
      color: "rose",
      descripcion: "Bajo rendimiento · Evaluar eliminación",
    },
    {
      key: "nuevasSugerencias",
      titulo: "Sugerencias estratégicas",
      icon: FaPlusCircle,
      color: "violet",
      descripcion: "Acciones recomendadas por CenterIA",
    },
  ];

  const colorMap = {
    emerald: {
      border: "border-emerald-500/20",
      bg: "bg-emerald-500/5",
      icon: "bg-emerald-500/15 border-emerald-500/25 text-emerald-400",
      badge: "bg-emerald-500/15 text-emerald-300",
    },
    amber: {
      border: "border-amber-500/20",
      bg: "bg-amber-500/5",
      icon: "bg-amber-500/15 border-amber-500/25 text-amber-400",
      badge: "bg-amber-500/15 text-amber-300",
    },
    rose: {
      border: "border-rose-500/20",
      bg: "bg-rose-500/5",
      icon: "bg-rose-500/15 border-rose-500/25 text-rose-400",
      badge: "bg-rose-500/15 text-rose-300",
    },
    violet: {
      border: "border-violet-500/20",
      bg: "bg-violet-500/5",
      icon: "bg-violet-500/15 border-violet-500/25 text-violet-400",
      badge: "bg-violet-500/15 text-violet-300",
    },
  };

  return (
    <div className="space-y-6">
      {/* Header */}
      <div className="flex items-start justify-between gap-4">
        <div>
          <div className="flex items-center gap-2 text-violet-400 text-sm font-medium mb-1">
            <FaUtensils />
            <span>Optimización de carta</span>
          </div>
          <h2 className="text-2xl font-bold text-white">Optimización de menú</h2>
          <p className="text-slate-400 text-sm mt-1">Análisis de rotación y rentabilidad</p>
        </div>
        <button
          onClick={onClose}
          className="p-2.5 rounded-xl border border-white/[0.08] text-slate-400 hover:text-white hover:bg-white/[0.05] transition"
        >
          <FaTimes />
        </button>
      </div>

      {/* Resumen */}
      <div className="rounded-2xl border border-violet-500/20 bg-gradient-to-br from-violet-600/15 to-slate-900/80 p-6">
        <div className="flex items-center gap-3 mb-4">
          <div className="w-10 h-10 rounded-xl bg-violet-500/20 border border-violet-500/30 flex items-center justify-center">
            <FaLightbulb className="text-violet-400" />
          </div>
          <h3 className="font-semibold text-white text-lg">Resumen general</h3>
        </div>
        <p className="text-slate-300 leading-relaxed text-[15px]">{data.resumen}</p>
      </div>

      {/* Bloques */}
      <div className="space-y-5">
        {bloques.map(({ key, titulo, icon: Icon, color, descripcion }) => {
          const items = data[key] || [];
          const c = colorMap[color];

          return (
            <div key={key} className={`rounded-2xl border ${c.border} ${c.bg} p-6`}>
              <div className="flex items-start gap-4 mb-5">
                <div className={`w-11 h-11 rounded-xl border flex items-center justify-center shrink-0 ${c.icon}`}>
                  <Icon />
                </div>
                <div>
                  <h3 className="font-semibold text-white text-lg">{titulo}</h3>
                  <p className="text-slate-500 text-sm mt-0.5">{descripcion}</p>
                </div>
              </div>

              {items.length === 0 ? (
                <p className="text-slate-500 text-sm">No hay elementos en esta categoría.</p>
              ) : (
                <ul className="space-y-4">
                  {items.map((item, i) => (
                    <li
                      key={i}
                      className="flex gap-3 p-4 rounded-xl bg-slate-950/50 border border-white/[0.05]"
                    >
                      <span className={`mt-0.5 px-2 py-0.5 rounded-md text-xs font-medium shrink-0 ${c.badge}`}>
                        {i + 1}
                      </span>
                      <p className="text-slate-300 text-sm leading-relaxed">{item}</p>
                    </li>
                  ))}
                </ul>
              )}
            </div>
          );
        })}
      </div>
    </div>
  );
}


function ChatAsistente({ nombreUsuario, onClose }) {
  const [mensajes, setMensajes] = useState([]);
  const [input, setInput] = useState("");
  const [enviando, setEnviando] = useState(false);
  const bottomRef = useRef(null);
  const inputRef = useRef(null);

  useEffect(() => {
    setMensajes([
      {
        rol: "assistant",
        contenido: `Hola! ${nombreUsuario}… ¿en qué puedo ayudarte?`,
        esSaludo: true,
      },
    ]);
    setTimeout(() => inputRef.current?.focus(), 100);
  }, [nombreUsuario]);

  useEffect(() => {
    bottomRef.current?.scrollIntoView({ behavior: "smooth" });
  }, [mensajes]);

  const enviarMensaje = async () => {
    const texto = input.trim();
    if (!texto || enviando) return;

    const mensajeUsuario = { rol: "user", contenido: texto };
    const nuevos = [...mensajes, mensajeUsuario];
    setMensajes(nuevos);
    setInput("");
    setEnviando(true);

    try {
      const historialParaBackend = nuevos
        .filter((m) => !m.esSaludo)
        .map((m) => ({ rol: m.rol, contenido: m.contenido }));

      const historialSinUltimo = historialParaBackend.slice(0, -1);
      const data = await centerIAService.consultarAsistente(texto, historialSinUltimo);

      setMensajes((prev) => [
        ...prev,
        { rol: "assistant", contenido: data.respuesta },
      ]);
    } catch (error) {
      setMensajes((prev) => [
        ...prev,
        {
          rol: "assistant",
          contenido: "Lo siento, ocurrió un error al procesar tu pregunta. Intenta de nuevo.",
          esError: true,
        },
      ]);
    } finally {
      setEnviando(false);
      setTimeout(() => inputRef.current?.focus(), 50);
    }
  };

  const handleKeyDown = (e) => {
    if (e.key === "Enter" && !e.shiftKey) {
      e.preventDefault();
      enviarMensaje();
    }
  };

  return (
    <div className="flex flex-col h-[min(70vh,640px)] rounded-2xl border border-white/[0.08] bg-slate-900/80 overflow-hidden">
      {/* Header del chat */}
      <div className="px-5 py-4 border-b border-white/[0.07] flex items-center justify-between bg-slate-950/40">
        <div className="flex items-center gap-3">
          <div className="w-10 h-10 rounded-xl bg-violet-500/15 border border-violet-500/25 flex items-center justify-center">
            <FaRobot className="text-violet-400" />
          </div>
          <div>
            <h3 className="font-semibold text-white">Asistente Elysia</h3>
            <p className="text-xs text-slate-500">Consultor inteligente del restaurante</p>
          </div>
        </div>
        <button
          onClick={onClose}
          className="p-2 rounded-xl text-slate-500 hover:text-white hover:bg-white/[0.05] transition"
        >
          <FaTimes />
        </button>
      </div>

      {/* Mensajes */}
      <div className="flex-1 overflow-y-auto px-5 py-5 space-y-4">
        {mensajes.map((msg, i) => (
          <div
            key={i}
            className={`flex ${msg.rol === "user" ? "justify-end" : "justify-start"}`}
          >
            <div
              className={`max-w-[85%] rounded-2xl px-4 py-3 ${
                msg.rol === "user"
                  ? "bg-violet-600 text-white rounded-br-md"
                  : msg.esError
                  ? "bg-red-500/10 border border-red-500/20 text-red-200 rounded-bl-md"
                  : "bg-slate-800 text-slate-200 rounded-bl-md"
              }`}
            >
              <p className="text-[11px] font-semibold mb-1 opacity-60">
                {msg.rol === "user" ? nombreUsuario : "Bot"}
              </p>
              <p className="text-sm leading-relaxed whitespace-pre-wrap">{msg.contenido}</p>
            </div>
          </div>
        ))}

        {enviando && (
          <div className="flex justify-start">
            <div className="bg-slate-800 rounded-2xl rounded-bl-md px-4 py-3 flex items-center gap-2">
              <FaSpinner className="animate-spin text-violet-400 text-sm" />
              <span className="text-sm text-slate-400">Bot está pensando...</span>
            </div>
          </div>
        )}
        <div ref={bottomRef} />
      </div>

      {/* Input */}
      <div className="px-4 py-4 border-t border-white/[0.07] bg-slate-950/40">
        <div className="flex gap-3">
          <input
            ref={inputRef}
            type="text"
            value={input}
            onChange={(e) => setInput(e.target.value)}
            onKeyDown={handleKeyDown}
            disabled={enviando}
            placeholder="Escribe tu pregunta sobre el restaurante..."
            className="flex-1 px-4 py-3 rounded-xl bg-slate-950/70 border border-white/[0.08] text-white placeholder-slate-600 outline-none focus:border-violet-500 focus:ring-4 focus:ring-violet-500/10 transition disabled:opacity-50"
          />
          <button
            onClick={enviarMensaje}
            disabled={!input.trim() || enviando}
            className="px-4 py-3 rounded-xl bg-gradient-to-r from-violet-600 to-indigo-600 text-white hover:from-violet-500 hover:to-indigo-500 transition disabled:opacity-40 disabled:cursor-not-allowed flex items-center justify-center"
          >
            {enviando ? <FaSpinner className="animate-spin" /> : <FaPaperPlane />}
          </button>
        </div>
      </div>
    </div>
  );
}


export default function CenterIAPage() {
  const [nombreUsuario, setNombreUsuario] = useState("Usuario");
  const [loadingPerfil, setLoadingPerfil] = useState(true);

  // Estados de confirmación
  const [confirmModal, setConfirmModal] = useState(null); // "analisis" | "optimizacion" | "chat"
  const [loadingAccion, setLoadingAccion] = useState(false);

  // Resultados
  const [vistaActiva, setVistaActiva] = useState(null); // "analisis" | "optimizacion" | "chat"
  const [dataAnalisis, setDataAnalisis] = useState(null);
  const [dataOptimizacion, setDataOptimizacion] = useState(null);
  const [error, setError] = useState("");

  // Cargar userName
  useEffect(() => {
    const cargarPerfil = async () => {
      try {
        const response = await managerAccountService.getPerfil();
        const data = response?.data ?? response;
        setNombreUsuario(data?.userName || data?.name || "Usuario");
      } catch {
        setNombreUsuario("Usuario");
      } finally {
        setLoadingPerfil(false);
      }
    };
    cargarPerfil();
  }, []);

  const abrirConfirmacion = (tipo) => {
    setError("");
    setConfirmModal(tipo);
  };

  const cerrarConfirmacion = () => {
    if (loadingAccion) return;
    setConfirmModal(null);
  };

  const ejecutarAccion = async () => {
    if (!confirmModal) return;
    setLoadingAccion(true);
    setError("");

    try {
      if (confirmModal === "analisis") {
        const data = await centerIAService.obtenerAnalisisRestaurante();
        setDataAnalisis(data);
        setVistaActiva("analisis");
      } else if (confirmModal === "optimizacion") {
        const data = await centerIAService.obtenerOptimizacionMenu();
        setDataOptimizacion(data);
        setVistaActiva("optimizacion");
      } else if (confirmModal === "chat") {
        setVistaActiva("chat");
      }
      setConfirmModal(null);
    } catch (err) {
      setError(err.message || "Ocurrió un error al procesar la solicitud.");
      setConfirmModal(null);
    } finally {
      setLoadingAccion(false);
    }
  };

  const cerrarVista = () => {
    setVistaActiva(null);
    setDataAnalisis(null);
    setDataOptimizacion(null);
  };

  const configModal = {
    analisis: {
      title: "Generar análisis del restaurante",
      description:
        "Se generará un análisis completo de ventas, pedidos, inventario, menú, reservas y empleados de los últimos 30 días. ¿Deseas continuar?",
    },
    optimizacion: {
      title: "Optimizar menú",
      description:
        "Se analizará el rendimiento de tus platos y se generarán recomendaciones de promoción, revisión de precios y posibles retiros. ¿Deseas continuar?",
    },
    chat: {
      title: "Abrir asistente inteligente",
      description:
        "Se abrirá el chat con el Asistente de Elysia. Podrás hacer preguntas sobre tu restaurante y recibir respuestas basadas en tus datos reales. ¿Deseas continuar?",
    },
  };

  const acciones = [
    {
      id: "analisis",
      titulo: "Análisis del restaurante",
      descripcion: "Resumen completo de operaciones, ventas, inventario y más.",
      icon: FaChartLine,
      gradient: "from-violet-600 to-indigo-600",
      shadow: "shadow-violet-600/20",
    },
    {
      id: "optimizacion",
      titulo: "Optimización de menú",
      descripcion: "Identifica platos estrella, candidatos a retirar y oportunidades.",
      icon: FaUtensils,
      gradient: "from-emerald-600 to-teal-600",
      shadow: "shadow-emerald-600/20",
    },
    {
      id: "chat",
      titulo: "Asistente inteligente",
      descripcion: "Consulta en lenguaje natural sobre cualquier aspecto de tu negocio.",
      icon: FaComments,
      gradient: "from-amber-500 to-orange-600",
      shadow: "shadow-amber-500/20",
    },
  ];

  return (
    <div className="min-h-screen bg-slate-950 text-slate-200">
      {/* SIDEBAR */}
      <div className="fixed left-0 top-0 z-50 h-screen">
        <OwnerSidebar/>
      </div>

      <main className="lg:ml-64 min-h-screen">
        {/* Fondo decorativo */}
        <div className="fixed inset-0 pointer-events-none overflow-hidden">
          <div className="absolute -top-40 -right-40 w-96 h-96 bg-violet-600/10 rounded-full blur-3xl" />
          <div className="absolute top-1/2 -left-40 w-96 h-96 bg-indigo-600/5 rounded-full blur-3xl" />
        </div>

        <div className="relative px-4 sm:px-6 lg:px-10 py-6 lg:py-10 max-w-7xl mx-auto">
          {/* HEADER */}
          <div className="mb-10">
            <div className="flex items-center gap-2 text-violet-400 text-sm font-medium mb-2">
              <FaRobot />
              <span>CenterIA · Inteligencia artificial</span>
            </div>
            <h1 className="text-3xl lg:text-4xl font-bold text-white tracking-tight">
              Centro de Inteligencia
            </h1>
            <p className="text-slate-400 mt-2 max-w-2xl">
              Obtén análisis profundos, optimiza tu menú y consulta al asistente inteligente
              basado en los datos reales de tu restaurante.
            </p>
          </div>

          {/* ERROR */}
          {error && (
            <div className="mb-6 rounded-2xl border border-red-500/20 bg-red-500/5 px-5 py-4 flex items-start gap-4">
              <div className="w-10 h-10 rounded-xl bg-red-500/10 flex items-center justify-center shrink-0">
                <FaExclamationTriangle className="text-red-400" />
              </div>
              <div>
                <p className="text-sm font-semibold text-red-300">Ocurrió un problema</p>
                <p className="text-sm text-red-400/80 mt-1">{error}</p>
              </div>
            </div>
          )}

          {/* CONTENIDO PRINCIPAL */}
          {!vistaActiva ? (
            /* ========== SELECTOR DE ACCIONES ========== */
            <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
              {acciones.map((accion) => {
                const Icon = accion.icon;
                return (
                  <button
                    key={accion.id}
                    onClick={() => abrirConfirmacion(accion.id)}
                    className="group relative text-left rounded-3xl border border-white/[0.08] bg-slate-900/70 p-6 hover:border-white/[0.15] hover:bg-slate-900 transition-all duration-300 overflow-hidden"
                  >
                    <div className="absolute inset-0 bg-gradient-to-br from-white/[0.02] to-transparent opacity-0 group-hover:opacity-100 transition" />
                    <div className="relative">
                      <div
                        className={`w-14 h-14 rounded-2xl bg-gradient-to-br ${accion.gradient} flex items-center justify-center mb-5 shadow-lg ${accion.shadow}`}
                      >
                        <Icon className="text-white text-xl" />
                      </div>
                      <h3 className="text-lg font-semibold text-white mb-2">{accion.titulo}</h3>
                      <p className="text-sm text-slate-400 leading-relaxed mb-5">
                        {accion.descripcion}
                      </p>
                      <div className="flex items-center gap-2 text-sm font-medium text-violet-400 group-hover:gap-3 transition-all">
                        <span>Iniciar</span>
                        <FaArrowRight className="text-xs" />
                      </div>
                    </div>
                  </button>
                );
              })}
            </div>
          ) : (
            /* ========== VISTAS DE RESULTADO ========== */
            <div className="rounded-3xl border border-white/[0.08] bg-slate-900/50 p-6 lg:p-8">
              {vistaActiva === "analisis" && dataAnalisis && (
                <AnalisisView data={dataAnalisis} onClose={cerrarVista} />
              )}
              {vistaActiva === "optimizacion" && dataOptimizacion && (
                <OptimizacionView data={dataOptimizacion} onClose={cerrarVista} />
              )}
              {vistaActiva === "chat" && (
                <ChatAsistente
                  nombreUsuario={nombreUsuario}
                  onClose={cerrarVista}
                />
              )}
            </div>
          )}
        </div>
      </main>

      {/* MODAL DE CONFIRMACIÓN */}
      {confirmModal && (
        <ConfirmModal
          open={!!confirmModal}
          title={configModal[confirmModal].title}
          description={configModal[confirmModal].description}
          onConfirm={ejecutarAccion}
          onCancel={cerrarConfirmacion}
          loading={loadingAccion}
        />
      )}
    </div>
  );
}
import { useState } from "react";
import { useSearchParams, useNavigate } from "react-router-dom";
import authService from "../../services/authService";

export default function ResetPasswordPage() {
  const [searchParams] = useSearchParams();
  const navigate = useNavigate();

  const [password, setPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [message, setMessage] = useState("");
  const [loading, setLoading] = useState(false);

  const userId = searchParams.get("userId");
  const token = searchParams.get("token");

  const handleSubmit = async (e) => {
    e.preventDefault();

    // Validación de coincidencia
    if (password !== confirmPassword) {
      setMessage("Las contraseñas no coinciden");
      return;
    }

    if (password.length < 6) {
      setMessage("La contraseña debe tener al menos 6 caracteres");
      return;
    }

    setLoading(true);
    setMessage("");

    const res = await authService.resetPassword({
      id: userId,
      token,
      password,
    });

    setLoading(false);

    if (res.success) {
      setMessage("Contraseña cambiada correctamente");

      setTimeout(() => {
        navigate("/");
      }, 2000);
    } else {
      setMessage(res.message);
    }
  };

  return (
    <div className="min-h-screen flex items-center justify-center bg-gradient-to-br from-slate-50 via-violet-50/40 to-slate-100 px-4">
      <div className="w-full max-w-md">
        {/* Card */}
        <div className="bg-white/80 backdrop-blur-sm border border-slate-200/80 rounded-2xl shadow-xl shadow-slate-200/50 p-8 sm:p-10">
          
          {/* Header */}
          <div className="text-center mb-8">
            <div className="mx-auto mb-5 flex h-14 w-14 items-center justify-center rounded-2xl bg-violet-100 text-violet-600">
              {/* Key icon */}
              <svg
                xmlns="http://www.w3.org/2000/svg"
                className="h-7 w-7"
                fill="none"
                viewBox="0 0 24 24"
                stroke="currentColor"
                strokeWidth={1.8}
              >
                <path
                  strokeLinecap="round"
                  strokeLinejoin="round"
                  d="M15.75 5.25a3 3 0 013 3m3 0a6 6 0 01-7.029 5.912c-.563-.097-1.159.026-1.563.43L10.5 17.25H8.25v2.25H6v2.25H2.25v-2.818c0-.597.237-1.17.659-1.591l6.499-6.499c.404-.404.527-1 .43-1.563A6 6 0 1121.75 8.25z"
                />
              </svg>
            </div>

            <h1 className="text-2xl font-semibold tracking-tight text-slate-900">
              Nueva contraseña
            </h1>
            <p className="mt-2 text-sm text-slate-500">
              Crea una contraseña segura para tu cuenta.
            </p>
          </div>

          {/* Form */}
          <form onSubmit={handleSubmit} className="space-y-5">
            {/* Nueva contraseña */}
            <div>
              <label
                htmlFor="password"
                className="mb-1.5 block text-sm font-medium text-slate-700"
              >
                Nueva contraseña
              </label>
              <input
                id="password"
                type="password"
                required
                autoComplete="new-password"
                placeholder="••••••••"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                className="w-full rounded-xl border border-slate-200 bg-white px-4 py-3 text-slate-900 placeholder:text-slate-400 
                           transition-all duration-200
                           focus:border-violet-500 focus:outline-none focus:ring-4 focus:ring-violet-500/10
                           disabled:opacity-60"
                disabled={loading}
              />
            </div>

            {/* Confirmar contraseña */}
            <div>
              <label
                htmlFor="confirmPassword"
                className="mb-1.5 block text-sm font-medium text-slate-700"
              >
                Confirmar contraseña
              </label>
              <input
                id="confirmPassword"
                type="password"
                required
                autoComplete="new-password"
                placeholder="••••••••"
                value={confirmPassword}
                onChange={(e) => setConfirmPassword(e.target.value)}
                className={`w-full rounded-xl border bg-white px-4 py-3 text-slate-900 placeholder:text-slate-400 
                           transition-all duration-200
                           focus:outline-none focus:ring-4
                           disabled:opacity-60
                           ${
                             confirmPassword && password !== confirmPassword
                               ? "border-rose-300 focus:border-rose-500 focus:ring-rose-500/10"
                               : "border-slate-200 focus:border-violet-500 focus:ring-violet-500/10"
                           }`}
                disabled={loading}
              />
              {confirmPassword && password !== confirmPassword && (
                <p className="mt-1.5 text-xs text-rose-600">
                  Las contraseñas no coinciden
                </p>
              )}
            </div>

            <button
              type="submit"
              disabled={loading || (confirmPassword && password !== confirmPassword)}
              className="relative flex w-full items-center justify-center gap-2 rounded-xl bg-violet-600 px-4 py-3.5 
                         text-sm font-semibold text-white shadow-sm
                         transition-all duration-200
                         hover:bg-violet-700 hover:shadow-md
                         focus:outline-none focus:ring-4 focus:ring-violet-500/20
                         disabled:cursor-not-allowed disabled:opacity-70"
            >
              {loading ? (
                <>
                  <svg
                    className="h-4 w-4 animate-spin"
                    viewBox="0 0 24 24"
                    fill="none"
                  >
                    <circle
                      className="opacity-25"
                      cx="12"
                      cy="12"
                      r="10"
                      stroke="currentColor"
                      strokeWidth="4"
                    />
                    <path
                      className="opacity-75"
                      fill="currentColor"
                      d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"
                    />
                  </svg>
                  Procesando...
                </>
              ) : (
                "Cambiar contraseña"
              )}
            </button>
          </form>

          {/* Message */}
          {message && (
            <div
              className={`mt-6 rounded-xl px-4 py-3 text-center text-sm ${
                message.includes("correctamente")
                  ? "bg-emerald-50 text-emerald-700 border border-emerald-100"
                  : "bg-rose-50 text-rose-700 border border-rose-100"
              }`}
            >
              {message}
            </div>
          )}
        </div>
      </div>
    </div>
  );
}
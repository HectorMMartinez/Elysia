import { useState } from "react";
import authService from "../../services/authService";

export default function ForgotPasswordPage() {

  const [email, setEmail] = useState("");
  const [message, setMessage] = useState("");
  const [loading, setLoading] = useState(false);

  const handleSubmit = async (e) => {
    e.preventDefault();

    setLoading(true);

    const res = await authService.forgotPassword({ email });

    setLoading(false);

    if (res.success) {
      setMessage("Revisa tu correo para restablecer tu contraseña.");
    } else {
      setMessage(res.message);
    }
  };

  return (
    <div className="min-h-screen flex items-center justify-center bg-slate-100">
      <div className="bg-white p-8 rounded-xl shadow w-full max-w-md">

        <h1 className="text-xl font-bold mb-4">
          Recuperar contraseña
        </h1>

        <form onSubmit={handleSubmit} className="space-y-4">

          <input
            className="w-full border p-3 rounded"
            type="email"
            placeholder="Tu correo"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
          />

          <button
            className="w-full bg-violet-600 text-white p-3 rounded"
            disabled={loading}
          >
            {loading ? "Enviando..." : "Enviar enlace"}
          </button>

        </form>

        {message && (
          <p className="mt-4 text-center text-sm text-slate-600">
            {message}
          </p>
        )}

      </div>
    </div>
  );
}
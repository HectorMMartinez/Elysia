import { useEffect, useState } from "react";
import { useSearchParams, useNavigate } from "react-router-dom";
import authService from "../../services/authService";

export default function ResetPasswordPage() {

  const [searchParams] = useSearchParams();
  const navigate = useNavigate();

  const [password, setPassword] = useState("");
  const [message, setMessage] = useState("");
  const [loading, setLoading] = useState(false);

  const userId = searchParams.get("userId");
  const token = searchParams.get("token");

  const handleSubmit = async (e) => {
    e.preventDefault();

    setLoading(true);

    const res = await authService.resetPassword({
      id: userId,
      token,
      password
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
    <div className="min-h-screen flex items-center justify-center bg-slate-100">

      <div className="bg-white p-8 rounded-xl shadow w-full max-w-md">

        <h1 className="text-xl font-bold mb-4">
          Nueva contraseña
        </h1>

        <form onSubmit={handleSubmit} className="space-y-4">

          <input
            className="w-full border p-3 rounded"
            type="password"
            placeholder="Nueva contraseña"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
          />

          <button
            className="w-full bg-violet-600 text-white p-3 rounded"
            disabled={loading}
          >
            {loading ? "Procesando..." : "Cambiar contraseña"}
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
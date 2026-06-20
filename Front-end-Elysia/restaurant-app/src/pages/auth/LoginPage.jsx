import { Link } from "react-router-dom";

export default function LoginPage() {
  return (
    <div className="min-h-screen bg-slate-100 flex items-center justify-center px-4">
      
      <div className="w-full max-w-md bg-white rounded-2xl shadow-xl p-8">

        {/* Logo */}
        <div className="text-center mb-8">

          <div className="w-20 h-20 mx-auto rounded-full bg-violet-600 flex items-center justify-center text-white text-3xl font-bold">
            E
          </div>

          <h1 className="mt-4 text-3xl font-bold text-slate-800">
            Elysia
          </h1>

          <p className="text-slate-500 mt-2">
            Sistema de Gestión para Restaurantes
          </p>

        </div>

        {/* Formulario */}
        <form className="space-y-5">

          <div>
            <label className="block text-sm font-medium text-slate-700 mb-2">
              Correo Electrónico
            </label>

            <input
              type="email"
              placeholder="correo@ejemplo.com"
              className="w-full border border-slate-300 rounded-lg px-4 py-3 focus:outline-none focus:ring-2 focus:ring-violet-500"
            />
          </div>

          <div>
            <label className="block text-sm font-medium text-slate-700 mb-2">
              Contraseña
            </label>

            <input
              type="password"
              placeholder="********"
              className="w-full border border-slate-300 rounded-lg px-4 py-3 focus:outline-none focus:ring-2 focus:ring-violet-500"
            />
          </div>

          <button
            type="button"
            className="w-full bg-violet-600 text-white py-3 rounded-lg font-semibold hover:bg-violet-700 transition"
          >
            Iniciar Sesión
          </button>

        </form>

        {/* Navegación */}
        <div className="mt-6 flex flex-col gap-3">

          <Link
            to="/register"
            className="text-center border border-violet-600 text-violet-600 py-3 rounded-lg font-medium hover:bg-violet-50 transition"
          >
            Registrarse
          </Link>

          <Link
            to="/forgot-password"
            className="text-center text-slate-600 hover:text-violet-600"
          >
            ¿Olvidaste tu contraseña?
          </Link>

        </div>

      </div>

    </div>
  );
}
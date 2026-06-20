import { useState } from "react";
import { Link } from "react-router-dom";

import {
  FaUser,
  FaEnvelope,
  FaLock,
  FaPhone,
  FaIdCard,
  FaCamera,
} from "react-icons/fa";


export default function RegisterPage() {
  const [step, setStep] = useState(1);

  return (
    <div className="min-h-screen bg-slate-100 py-10 px-4">
      <div className="max-w-5xl mx-auto bg-white rounded-2xl shadow-xl p-8">

        {/* Header */}
        <div className="text-center mb-10">
          <div className="w-20 h-20 mx-auto rounded-full bg-violet-600 flex items-center justify-center text-white text-3xl font-bold">
            E
          </div>

          <h1 className="mt-4 text-3xl font-bold text-slate-800">
            Registro de Propietario
          </h1>

          <p className="text-slate-500">
            Complete la información para crear su cuenta
          </p>
        </div>

        {/* Stepper */}

        <div className="flex justify-center items-center mb-10">

          <div
            className={`w-12 h-12 rounded-full flex items-center justify-center text-white font-bold ${
              step >= 1 ? "bg-violet-600" : "bg-slate-300"
            }`}
          >
            1
          </div>

          <div className="w-24 h-1 bg-slate-300"></div>

          <div
            className={`w-12 h-12 rounded-full flex items-center justify-center text-white font-bold ${
              step >= 2 ? "bg-violet-600" : "bg-slate-300"
            }`}
          >
            2
          </div>

          <div className="w-24 h-1 bg-slate-300"></div>

          <div
            className={`w-12 h-12 rounded-full flex items-center justify-center text-white font-bold ${
              step >= 3 ? "bg-violet-600" : "bg-slate-300"
            }`}
          >
            3
          </div>
        </div>

        {/* Paso 1 */}

        {step === 1 && (
            <div>

    {/* Tarjeta encabezado */}

    <div className="mb-8 bg-gradient-to-r from-violet-600 to-purple-700 rounded-2xl p-6 text-white shadow-lg">
      <h2 className="text-3xl font-bold">
        Información Personal
      </h2>

      <p className="mt-2 text-violet-100">
        Complete los datos del propietario para crear su cuenta en Elysia.
      </p>
    </div>

    {/* Foto Perfil */}

    <div className="mb-8">

      <label className="block text-sm font-semibold text-slate-700 mb-3">
        Foto de Perfil
      </label>

      <div className="border-2 border-dashed border-violet-300 rounded-2xl p-8 bg-violet-50 hover:bg-violet-100 transition cursor-pointer">

        <div className="flex flex-col items-center">

          <div className="w-20 h-20 rounded-full bg-violet-600 flex items-center justify-center text-white text-3xl mb-4">
            <FaCamera />
          </div>

          <h3 className="font-semibold text-slate-700">
            Subir fotografía
          </h3>

          <p className="text-slate-500 text-sm mt-1">
            PNG, JPG o JPEG
          </p>

          <input
            type="file"
            className="mt-4"
          />

        </div>

      </div>

    </div>

    {/* Formulario */}

    <div className="bg-white border border-slate-200 rounded-2xl p-8 shadow-sm">

      <div className="grid grid-cols-1 md:grid-cols-2 gap-6">

        <div>
          <label className="block mb-2 font-medium">
            Nombre
          </label>

          <div className="relative">
            <FaUser className="absolute left-4 top-4 text-slate-400" />

            <input
              type="text"
              placeholder="Kelvin"
              className="w-full pl-12 pr-4 py-3 border rounded-xl focus:ring-2 focus:ring-violet-500 focus:outline-none"
            />
          </div>
        </div>

        <div>
          <label className="block mb-2 font-medium">
            Apellido
          </label>

          <div className="relative">
            <FaUser className="absolute left-4 top-4 text-slate-400" />

            <input
              type="text"
              placeholder="Díaz"
              className="w-full pl-12 pr-4 py-3 border rounded-xl focus:ring-2 focus:ring-violet-500 focus:outline-none"
            />
          </div>
        </div>

        <div>
          <label className="block mb-2 font-medium">
            Usuario
          </label>

          <div className="relative">
            <FaUser className="absolute left-4 top-4 text-slate-400" />

            <input
              type="text"
              placeholder="kelvindiaz"
              className="w-full pl-12 pr-4 py-3 border rounded-xl focus:ring-2 focus:ring-violet-500 focus:outline-none"
            />
          </div>
        </div>

        <div>
          <label className="block mb-2 font-medium">
            Correo
          </label>

          <div className="relative">
            <FaEnvelope className="absolute left-4 top-4 text-slate-400" />

            <input
              type="email"
              placeholder="correo@email.com"
              className="w-full pl-12 pr-4 py-3 border rounded-xl focus:ring-2 focus:ring-violet-500 focus:outline-none"
            />
          </div>
        </div>

        <div>
          <label className="block mb-2 font-medium">
            Contraseña
          </label>

          <div className="relative">
            <FaLock className="absolute left-4 top-4 text-slate-400" />

            <input
              type="password"
              placeholder="********"
              className="w-full pl-12 pr-4 py-3 border rounded-xl focus:ring-2 focus:ring-violet-500 focus:outline-none"
            />
          </div>
        </div>

        <div>
          <label className="block mb-2 font-medium">
            Confirmar Contraseña
          </label>

          <div className="relative">
            <FaLock className="absolute left-4 top-4 text-slate-400" />

            <input
              type="password"
              placeholder="********"
              className="w-full pl-12 pr-4 py-3 border rounded-xl focus:ring-2 focus:ring-violet-500 focus:outline-none"
            />
          </div>
        </div>

        <div>
          <label className="block mb-2 font-medium">
            Teléfono
          </label>

          <div className="relative">
            <FaPhone className="absolute left-4 top-4 text-slate-400" />

            <input
              type="text"
              placeholder="8091234567"
              className="w-full pl-12 pr-4 py-3 border rounded-xl focus:ring-2 focus:ring-violet-500 focus:outline-none"
            />
          </div>
        </div>

        <div>
          <label className="block mb-2 font-medium">
            Cédula
          </label>

          <div className="relative">
            <FaIdCard className="absolute left-4 top-4 text-slate-400" />

            <input
              type="text"
              placeholder="00112345678"
              className="w-full pl-12 pr-4 py-3 border rounded-xl focus:ring-2 focus:ring-violet-500 focus:outline-none"
            />
          </div>
        </div>

      </div>

    </div>

  </div>
        )}

        {/* Paso 2 */}

        {step === 2 && (
          <div>

    {/* Encabezado */}

    <div className="mb-8 bg-gradient-to-r from-amber-500 to-orange-600 rounded-2xl p-6 text-white shadow-lg">
      <h2 className="text-3xl font-bold">
        Información del Restaurante
      </h2>

      <p className="mt-2 text-orange-100">
        Registre la información principal de su negocio.
      </p>
    </div>

    {/* Logo */}

    <div className="mb-8">

      <label className="block text-sm font-semibold text-slate-700 mb-3">
        Logo del Restaurante
      </label>

      <div className="border-2 border-dashed border-orange-300 rounded-2xl p-8 bg-orange-50 hover:bg-orange-100 transition cursor-pointer">

        <div className="flex flex-col items-center">

          <div className="w-20 h-20 rounded-full bg-orange-500 flex items-center justify-center text-white text-3xl mb-4">
            🏪
          </div>

          <h3 className="font-semibold text-slate-700">
            Subir Logo
          </h3>

          <p className="text-slate-500 text-sm mt-1">
            PNG, JPG o JPEG
          </p>

          <input
            type="file"
            className="mt-4"
          />

        </div>

      </div>

    </div>

    {/* Formulario */}

    <div className="bg-white border border-slate-200 rounded-2xl p-8 shadow-sm">

      <div className="grid grid-cols-1 md:grid-cols-2 gap-6">

        {/* Nombre Restaurante */}

        <div>
          <label className="block mb-2 font-medium">
            Nombre Restaurante
          </label>

          <input
            type="text"
            placeholder="Elysia Restaurant"
            className="w-full px-4 py-3 border rounded-xl focus:ring-2 focus:ring-orange-500 focus:outline-none"
          />
        </div>

        {/* Especialidad */}

        <div>
          <label className="block mb-2 font-medium">
            Especialidad
          </label>

          <input
            type="text"
            placeholder="Comida Dominicana"
            className="w-full px-4 py-3 border rounded-xl focus:ring-2 focus:ring-orange-500 focus:outline-none"
          />
        </div>

        {/* Teléfono */}

        <div>
          <label className="block mb-2 font-medium">
            Teléfono Restaurante
          </label>

          <input
            type="text"
            placeholder="8091234567"
            className="w-full px-4 py-3 border rounded-xl focus:ring-2 focus:ring-orange-500 focus:outline-none"
          />
        </div>

        {/* RNC */}

        <div>
          <label className="block mb-2 font-medium">
            RNC
          </label>

          <input
            type="text"
            placeholder="123456789"
            className="w-full px-4 py-3 border rounded-xl focus:ring-2 focus:ring-orange-500 focus:outline-none"
          />
        </div>

        {/* Hora Apertura */}

        <div>
          <label className="block mb-2 font-medium">
            Hora Apertura
          </label>

          <input
            type="time"
            className="w-full px-4 py-3 border rounded-xl focus:ring-2 focus:ring-orange-500 focus:outline-none"
          />
        </div>

        {/* Hora Cierre */}

        <div>
          <label className="block mb-2 font-medium">
            Hora Cierre
          </label>

          <input
            type="time"
            className="w-full px-4 py-3 border rounded-xl focus:ring-2 focus:ring-orange-500 focus:outline-none"
          />
        </div>

      </div>

      {/* Dirección */}

      <div className="mt-6">

        <label className="block mb-2 font-medium">
          Dirección
        </label>

        <textarea
          rows="4"
          placeholder="Ingrese la dirección completa del restaurante"
          className="w-full px-4 py-3 border rounded-xl focus:ring-2 focus:ring-orange-500 focus:outline-none resize-none"
        ></textarea>

      </div>

    </div>

  </div>
        )}

        {/* Paso 3 */}

        {step === 3 && (
           <div>

    {/* Header */}

    <div className="mb-8 bg-gradient-to-r from-emerald-500 to-green-600 rounded-2xl p-6 text-white shadow-lg">
      <h2 className="text-3xl font-bold">
        Plan y Método de Pago
      </h2>

      <p className="mt-2 text-green-100">
        Seleccione el plan ideal para su restaurante.
      </p>
    </div>

    {/* Planes */}

    <h3 className="text-xl font-bold text-slate-800 mb-4">
      Seleccione un Plan
    </h3>

    <div className="grid md:grid-cols-2 gap-6 mb-8">

      {/* Plan Simple */}

      <div className="border-2 border-slate-200 hover:border-violet-500 rounded-2xl p-6 bg-white shadow-sm hover:shadow-lg transition cursor-pointer">

        <div className="flex justify-between items-center mb-4">
          <h4 className="text-xl font-bold">
            Plan Simple
          </h4>

          <span className="bg-violet-100 text-violet-700 px-3 py-1 rounded-full text-sm font-medium">
            Básico
          </span>
        </div>

        <div className="text-4xl font-bold text-violet-600 mb-4">
          RD$1,500
        </div>

        <div className="text-slate-600 text-sm leading-6">
          Diseñado para pequeños restaurantes que necesitan administrar
          operaciones esenciales desde una sola plataforma.
        </div>
      </div>

      {/* Plan Premium */}

      <div className="border-2 border-amber-300 hover:border-amber-500 rounded-2xl p-6 bg-gradient-to-br from-amber-50 to-orange-50 shadow-md hover:shadow-xl transition cursor-pointer">

        <div className="flex justify-between items-center mb-4">
          <h4 className="text-xl font-bold">
            Plan Premium
          </h4>

          <span className="bg-amber-500 text-white px-3 py-1 rounded-full text-sm font-medium">
            Recomendado
          </span>
        </div>

        <div className="text-4xl font-bold text-amber-600 mb-4">
          RD$3,500
        </div>

        <div className="text-slate-600 text-sm leading-6">
          Incluye reportes avanzados, estadísticas, análisis de ventas,
          alertas inteligentes y recomendaciones automáticas.
        </div>
      </div>

    </div>

    {/* Términos */}

    <div className="mb-8 bg-slate-50 border border-slate-200 rounded-xl p-4">

      <label className="flex items-center gap-3 cursor-pointer">

        <input
          type="checkbox"
          className="w-5 h-5"
        />

        <span className="text-slate-700">
          Acepto los términos y condiciones de Elysia.
        </span>

      </label>

    </div>

    {/* Tarjeta */}

    <div className="bg-white border border-slate-200 rounded-2xl p-8 shadow-sm">

      <h3 className="text-xl font-bold text-slate-800 mb-6">
        Información de Pago
      </h3>

      <div className="grid grid-cols-1 md:grid-cols-2 gap-6">

        <div className="md:col-span-2">
          <label className="block mb-2 font-medium">
            Nombre del Titular
          </label>

          <input
            type="text"
            placeholder="Kelvin Díaz"
            className="w-full px-4 py-3 border rounded-xl focus:ring-2 focus:ring-green-500 focus:outline-none"
          />
        </div>

        <div className="md:col-span-2">
          <label className="block mb-2 font-medium">
            Número de Tarjeta
          </label>

          <input
            type="text"
            placeholder="1234 5678 9012 3456"
            className="w-full px-4 py-3 border rounded-xl focus:ring-2 focus:ring-green-500 focus:outline-none"
          />
        </div>

        <div>
          <label className="block mb-2 font-medium">
            Tipo de Tarjeta
          </label>

          <select className="w-full px-4 py-3 border rounded-xl focus:ring-2 focus:ring-green-500 focus:outline-none">
            <option>Seleccione</option>
            <option>Visa</option>
            <option>MasterCard</option>
            <option>American Express</option>
          </select>
        </div>

        <div>
          <label className="block mb-2 font-medium">
            CVV
          </label>

          <input
            type="text"
            placeholder="123"
            className="w-full px-4 py-3 border rounded-xl focus:ring-2 focus:ring-green-500 focus:outline-none"
          />
        </div>

        <div>
          <label className="block mb-2 font-medium">
            Mes Vencimiento
          </label>

          <input
            type="number"
            placeholder="12"
            className="w-full px-4 py-3 border rounded-xl focus:ring-2 focus:ring-green-500 focus:outline-none"
          />
        </div>

        <div>
          <label className="block mb-2 font-medium">
            Año Vencimiento
          </label>

          <input
            type="number"
            placeholder="2028"
            className="w-full px-4 py-3 border rounded-xl focus:ring-2 focus:ring-green-500 focus:outline-none"
          />
        </div>

      </div>

    </div>

  </div>
        )}

        {/* Botones */}

        <div className="flex justify-between mt-10">

          {step > 1 ? (
            <button
              onClick={() => setStep(step - 1)}
              className="px-6 py-3 border border-slate-300 rounded-lg"
            >
              Anterior
            </button>
          ) : (
            <Link
              to="/"
              className="px-6 py-3 border border-slate-300 rounded-lg"
            >
              Volver
            </Link>
          )}

          {step < 3 && (
            <button
              onClick={() => setStep(step + 1)}
              className="px-6 py-3 bg-violet-600 text-white rounded-lg hover:bg-violet-700"
            >
              Siguiente
            </button>
          )}

          {step === 3 && (
            <button
              className="px-6 py-3 bg-green-600 text-white rounded-lg"
            >
              Finalizar Registro
            </button>
          )}
        </div>
      </div>
    </div>
  );
}
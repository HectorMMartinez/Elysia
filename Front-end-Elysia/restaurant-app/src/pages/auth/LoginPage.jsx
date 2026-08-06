import { useState } from "react";
import { useForm } from "react-hook-form";
import { Link, useLocation } from "react-router-dom";

import FormInput from "../../components/forms/FormInput";
import FormButton from "../../components/forms/FormButton";
import FormAlert from "../../components/forms/FormAlert";
import authService from "../../services/authService";
import { loginValidation } from "../../validations/loginValidation";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../../context/AuthContext";

export default function LoginPage() {

  const location = useLocation();
  const navigate = useNavigate();
  const { login } = useAuth();

  const {
    register,
    handleSubmit,
    formState: { errors }
  } = useForm();

  const [loading, setLoading] = useState(false);

 
  const [alert, setAlert] = useState({
    show: Boolean(location.state?.message),
    type: "success", // "success" | "error"
    message: location.state?.message || ""
  });

  const onSubmit = async (data) => {
  setLoading(true);

  try {
   

    const res = await authService.login(data); 
   
    if (res.success) {
      login(res.data); 
      navigate("/dashboard"); 
     
    } else {
      setAlert({
        show: true,
        type: "error",
        message: res.message
      });
    }

  } catch (error) {

    setAlert({
      show: true,
      type: "error",
      message: "Error inesperado"
    });
  }

  setLoading(false);
};


  return (
    <div className="min-h-screen bg-slate-100 flex items-center justify-center px-4">

      <div className="w-full max-w-md bg-white rounded-2xl shadow-xl p-8">

        {/* LOGO */}
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

        {/* 🔥 ALERTA BONITA */}
        {alert.show && (
          <div
            className={`mb-5 p-3 rounded-lg text-sm flex justify-between items-start gap-3 border
              ${
                alert.type === "success"
                  ? "bg-green-100 text-green-700 border-green-200"
                  : "bg-red-100 text-red-700 border-red-200"
              }
            `}
          >
            <span>{alert.message}</span>

            <button
              onClick={() =>
                setAlert({ ...alert, show: false })
              }
              className="font-bold opacity-70 hover:opacity-100"
            >
              ✕
            </button>
          </div>
        )}

        {/* FORM */}
        <form onSubmit={handleSubmit(onSubmit)} className="space-y-5">

          <FormAlert />

          <FormInput
            label="Correo Electrónico"
            name="email"
            type="email"
            placeholder="correo@ejemplo.com"
            register={register}
            rules={loginValidation.email}
            error={errors.email}
          />

          <FormInput
            label="Contraseña"
            name="password"
            type="password"
            placeholder="********"
            register={register}
            rules={loginValidation.password}
            error={errors.password}
          />

          <FormButton loading={loading}>
            Iniciar Sesión
          </FormButton>

        </form>

        {/* LINKS */}
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

//  provandogit
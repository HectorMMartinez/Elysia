import { useEffect, useState } from "react";
import { useNavigate, useSearchParams } from "react-router-dom";

import { FaCheckCircle, FaTimesCircle, FaSpinner } from "react-icons/fa";

import authService from "../../services/authService";

export default function ConfirmAccountPage() {

    const navigate = useNavigate();
    const [searchParams] = useSearchParams();

    const [loading, setLoading] = useState(true);
    const [success, setSuccess] = useState(false);
    const [message, setMessage] = useState("");
    useEffect(() => {

    let timer;

    const confirmAccount = async () => {

        const userId = searchParams.get("userId");
        const token = searchParams.get("token");

        if (!userId || !token) {

            setLoading(false);
            setSuccess(false);
            setMessage("El enlace de confirmación es inválido.");
            return;

        }

        const result = await authService.confirmAccount({
            userId,
            token
        });

        setLoading(false);

        if (result.success) {

            setSuccess(true);
            setMessage(result.data.message);

            timer = setTimeout(() => {

                navigate("/");

            }, 3000);

        } else {

            setSuccess(false);
            setMessage(result.message);

        }

    };

    confirmAccount();

    return () => {

        if (timer) {

            clearTimeout(timer);

        }

    };

}, [navigate, searchParams]);

    return (

        <div className="min-h-screen bg-slate-100 flex items-center justify-center px-4">

            <div className="bg-white rounded-2xl shadow-xl w-full max-w-md p-10 text-center">

                <div className="w-20 h-20 rounded-full bg-violet-600 mx-auto flex items-center justify-center text-white text-3xl font-bold mb-6">
                    E
                </div>

                {loading && (

                    <>
                        <FaSpinner className="text-6xl text-violet-600 animate-spin mx-auto mb-6"/>

                        <h2 className="text-2xl font-bold mb-2">

                            Confirmando cuenta...

                        </h2>

                        <p className="text-slate-500">

                            Espere un momento.

                        </p>
                    </>

                )}

                {!loading && success && (

                    <>
                        <FaCheckCircle className="text-6xl text-green-500 mx-auto mb-6"/>

                        <h2 className="text-2xl font-bold text-green-600 mb-3">

                            Cuenta confirmada

                        </h2>

                        <p className="text-slate-600">

                            {message}

                        </p>

                        <p className="mt-4 text-sm text-slate-500">

                            Será redireccionado al inicio de sesión...

                        </p>

                        <button
                         onClick={() => navigate("/")}
                         className="mt-8 w-full bg-green-600 hover:bg-green-700 text-white py-3 rounded-xl transition">
                          Ir al Login
                       </button>
                       </>

                )}

                {!loading && !success && (

                    <>
                        <FaTimesCircle className="text-6xl text-red-500 mx-auto mb-6"/>

                        <h2 className="text-2xl font-bold text-red-600 mb-3">

                            No fue posible confirmar la cuenta

                        </h2>

                        <p className="text-slate-600">

                            {message}

                        </p>

                        <button
                            onClick={() => navigate("/")}
                            className="mt-8 w-full bg-violet-600 hover:bg-violet-700 text-white py-3 rounded-xl transition"
                        >
                            Ir al Login
                        </button>

                    </>

                )}

            </div>

        </div>

    );

}
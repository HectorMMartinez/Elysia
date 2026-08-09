import { useEffect, useState } from "react";
import {
  FaTimes,
  FaUser,
  FaEnvelope,
  FaPhone,
  FaIdCard,
  FaStore,
  FaMapMarkerAlt,
  FaClock,
} from "react-icons/fa";

import managerAccountService from "../../services/managerAccountService";
import UserStatusBadge from "./UserStatusBadge";

export default function UserDetailModal({
  user,
  onClose,
}) {
  const [userDetail, setUserDetail] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    if (!user?.id) {
      return;
    }

    const cargarDetalle = async () => {
      try {
        setLoading(true);
        setError(null);

        const data = await managerAccountService.getById(user.id);

        setUserDetail(data);
      } catch (error) {
        console.error(
          "Error al obtener detalle del usuario:",
          error
        );

        const message =
          error.response?.data ||
          error.message ||
          "No fue posible obtener los datos del usuario.";

        setError(
          typeof message === "string"
            ? message
            : "No fue posible obtener los datos del usuario."
        );
      } finally {
        setLoading(false);
      }
    };

    cargarDetalle();
  }, [user]);

  if (!user) {
    return null;
  }

  return (
    <div
      className="fixed inset-0 z-50 flex items-center justify-center bg-black/50 p-4"
      onMouseDown={(e) => {
        if (e.target === e.currentTarget) {
          onClose();
        }
      }}
    >
      <div className="bg-white w-full max-w-3xl max-h-[90vh] overflow-y-auto rounded-2xl shadow-2xl">

        {/* HEADER */}

        <div className="flex items-center justify-between px-6 py-5 border-b border-slate-100">
          <div>
            <h2 className="text-xl font-bold text-slate-800">
              Detalles del usuario
            </h2>

            <p className="text-sm text-slate-500 mt-1">
              Información de la cuenta
            </p>
          </div>

          <button
            type="button"
            onClick={onClose}
            className="w-9 h-9 flex items-center justify-center rounded-lg text-slate-400 hover:bg-slate-100 hover:text-slate-700 transition"
          >
            <FaTimes />
          </button>
        </div>

        {/* CONTENIDO */}

        <div className="p-6">

          {/* LOADING */}

          {loading && (
            <div className="py-12 flex flex-col items-center justify-center">
              <div className="w-9 h-9 border-4 border-violet-200 border-t-violet-600 rounded-full animate-spin" />

              <p className="text-slate-500 mt-4">
                Cargando información...
              </p>
            </div>
          )}

          {/* ERROR */}

          {error && !loading && (
            <div className="bg-red-50 border border-red-200 rounded-xl p-5">
              <p className="font-semibold text-red-700">
                No se pudo cargar la información
              </p>

              <p className="text-sm text-red-600 mt-1">
                {error}
              </p>
            </div>
          )}

          {/* INFORMACIÓN */}

          {userDetail && !loading && !error && (
            <div className="space-y-6">

              {/* PERFIL */}

              <div className="flex items-center gap-5 bg-slate-50 rounded-2xl p-5">

                <img
                  src={
                    userDetail.profileImage ||
                    "https://cdn-icons-png.flaticon.com/512/149/149071.png"
                  }
                  alt={`${userDetail.name} ${userDetail.lastName}`}
                  className="w-20 h-20 rounded-full object-cover border-4 border-white shadow"
                  onError={(e) => {
                    e.currentTarget.src =
                      "https://cdn-icons-png.flaticon.com/512/149/149071.png";
                  }}
                />

                <div className="flex-1">
                  <h3 className="text-2xl font-bold text-slate-800">
                    {userDetail.name} {userDetail.lastName}
                  </h3>

                  <p className="text-slate-500">
                    @{userDetail.userName}
                  </p>

                  <div className="mt-2">
                    <UserStatusBadge
                      isActive={userDetail.isActive}
                    />
                  </div>
                </div>

              </div>

              {/* INFORMACIÓN PERSONAL */}

              <section>
                <h3 className="text-lg font-bold text-slate-800 mb-4">
                  Información personal
                </h3>

                <div className="grid sm:grid-cols-2 gap-4">

                  <InfoItem
                    icon={<FaUser />}
                    label="Nombre completo"
                    value={`${userDetail.name} ${userDetail.lastName}`}
                  />

                  <InfoItem
                    icon={<FaIdCard />}
                    label="Cédula"
                    value={userDetail.idCard}
                  />

                  <InfoItem
                    icon={<FaEnvelope />}
                    label="Correo electrónico"
                    value={userDetail.email}
                  />

                  <InfoItem
                    icon={<FaPhone />}
                    label="Teléfono"
                    value={userDetail.phone || "No registrado"}
                  />

                  <InfoItem
                    icon={<FaUser />}
                    label="Nombre de usuario"
                    value={`@${userDetail.userName}`}
                  />

                  <InfoItem
                    icon={<FaUser />}
                    label="Rol"
                    value={userDetail.role}
                  />

                </div>
              </section>

              {/* RESTAURANTE
                  Solo para propietarios
              */}

              {userDetail.role?.toLowerCase() === "propietario" && (
                <section>
                  <h3 className="text-lg font-bold text-slate-800 mb-4">
                    Información del restaurante
                  </h3>

                  <div className="grid sm:grid-cols-2 gap-4">

                    <InfoItem
                      icon={<FaStore />}
                      label="Restaurante"
                      value={
                        userDetail.nombreRestaurante ||
                        "No registrado"
                      }
                    />

                    <InfoItem
                      icon={<FaIdCard />}
                      label="RNC"
                      value={
                        userDetail.rnc ||
                        "No registrado"
                      }
                    />

                    <InfoItem
                      icon={<FaMapMarkerAlt />}
                      label="Dirección"
                      value={
                        userDetail.direccionRestaurante ||
                        "No registrada"
                      }
                    />

                    <InfoItem
                      icon={<FaPhone />}
                      label="Teléfono del restaurante"
                      value={
                        userDetail.phoneRestaurante ||
                        "No registrado"
                      }
                    />

                    <InfoItem
                      icon={<FaClock />}
                      label="Hora de apertura"
                      value={
                        userDetail.horaApertura ||
                        "No registrada"
                      }
                    />

                    <InfoItem
                      icon={<FaClock />}
                      label="Hora de cierre"
                      value={
                        userDetail.horaCierre ||
                        "No registrada"
                      }
                    />

                  </div>
                </section>
              )}

            </div>
          )}

        </div>

        {/* FOOTER */}

        <div className="px-6 py-4 border-t border-slate-100 flex justify-end">
          <button
            type="button"
            onClick={onClose}
            className="px-5 py-2.5 rounded-xl bg-slate-800 text-white font-medium hover:bg-slate-700 transition"
          >
            Cerrar
          </button>
        </div>

      </div>
    </div>
  );
}

function InfoItem({
  icon,
  label,
  value,
}) {
  return (
    <div className="border border-slate-100 rounded-xl p-4">

      <div className="flex items-center gap-2 text-slate-400 mb-2">
        {icon}

        <span className="text-xs font-semibold uppercase">
          {label}
        </span>
      </div>

      <p className="text-sm font-medium text-slate-700 break-words">
        {value || "No disponible"}
      </p>

    </div>
  );
}
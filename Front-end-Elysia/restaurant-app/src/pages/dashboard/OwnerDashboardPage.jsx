import {
  FaCalendarCheck,
  FaChair,
  FaDollarSign,
  FaBoxOpen,
} from "react-icons/fa";

import OwnerSidebar from "../../components/layout/OwnerSidebar";

export default function OwnerDashboardPage() {
  return (
    <div className="min-h-screen bg-slate-100 flex">
      <OwnerSidebar />

      <main className="flex-1 p-8">
        <div className="mb-8">
          <h2 className="text-4xl font-bold text-slate-800">
            Panel del Propietario
          </h2>

          <p className="text-slate-500 mt-2">
            Bienvenido a Elysia Restaurant Management
          </p>
        </div>

        <div className="grid md:grid-cols-2 xl:grid-cols-4 gap-6 mb-8">
          <div className="bg-white rounded-2xl p-6 shadow-sm">
            <div className="flex justify-between items-center">
              <div>
                <p className="text-slate-500">
                  Reservas Hoy
                </p>

                <h3 className="text-3xl font-bold mt-2">
                  15
                </h3>
              </div>

              <FaCalendarCheck
                size={40}
                className="text-violet-500"
              />
            </div>
          </div>

          <div className="bg-white rounded-2xl p-6 shadow-sm">
            <div className="flex justify-between items-center">
              <div>
                <p className="text-slate-500">
                  Mesas Ocupadas
                </p>

                <h3 className="text-3xl font-bold mt-2">
                  8
                </h3>
              </div>

              <FaChair
                size={40}
                className="text-amber-500"
              />
            </div>
          </div>

          <div className="bg-white rounded-2xl p-6 shadow-sm">
            <div className="flex justify-between items-center">
              <div>
                <p className="text-slate-500">
                  Ventas del Día
                </p>

                <h3 className="text-3xl font-bold mt-2">
                  RD$25,000
                </h3>
              </div>

              <FaDollarSign
                size={40}
                className="text-green-500"
              />
            </div>
          </div>

          <div className="bg-white rounded-2xl p-6 shadow-sm">
            <div className="flex justify-between items-center">
              <div>
                <p className="text-slate-500">
                  Inventario
                </p>

                <h3 className="text-3xl font-bold mt-2">
                  125
                </h3>
              </div>

              <FaBoxOpen
                size={40}
                className="text-blue-500"
              />
            </div>
          </div>
        </div>

        <div className="grid lg:grid-cols-3 gap-6">
          <div className="lg:col-span-2 bg-white rounded-2xl shadow-sm p-6">
            <h3 className="text-xl font-bold mb-6">
              Actividad Reciente
            </h3>

            <div className="space-y-4">
              <div className="border-l-4 border-green-500 pl-4 py-2">
                Nueva reserva registrada para las 7:00 PM.
              </div>

              <div className="border-l-4 border-violet-500 pl-4 py-2">
                Pedido completado correctamente.
              </div>

              <div className="border-l-4 border-amber-500 pl-4 py-2">
                Mesa #12 liberada.
              </div>

              <div className="border-l-4 border-blue-500 pl-4 py-2">
                Inventario actualizado.
              </div>
            </div>
          </div>

          <div className="bg-gradient-to-br from-violet-600 to-indigo-700 rounded-2xl text-white p-6 shadow-lg">
            <h3 className="text-2xl font-bold">
              Plan Premium
            </h3>

            <p className="mt-3 text-violet-100">
              Tu membresía está activa.
            </p>

            <div className="mt-8">
              <p className="text-violet-200">
                Próxima renovación
              </p>

              <h4 className="text-xl font-bold mt-2">
                20 Julio 2026
              </h4>
            </div>

            <div className="mt-8">
              <p className="text-violet-200">
                Estado
              </p>

              <span className="inline-block mt-2 bg-green-500 px-4 py-2 rounded-full">
                Activo
              </span>
            </div>
          </div>
        </div>
      </main>
    </div>
  );
}
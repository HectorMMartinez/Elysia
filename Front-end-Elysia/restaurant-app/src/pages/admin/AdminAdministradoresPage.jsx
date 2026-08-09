import { useEffect, useMemo, useState } from "react";

import {
  FaUsers,
  FaSearch,
  FaFilter,
  FaSyncAlt,
  FaExclamationTriangle,
} from "react-icons/fa";

import AdminSidebar from "../../components/layout/AdminSidebar";
import UserTable from "../../components/admin/UserTable";
import UserDetailModal from "../../components/admin/UserDetailModal";
import ConfirmActionModal from "../../components/admin/ConfirmActionModal";

import managerAccountService from "../../services/managerAccountService";

export default function AdminAdministradoresPage() {

  const [users, setUsers] = useState([]);
  const [loading, setLoading] = useState(true);
  const [actionLoading, setActionLoading] = useState(false);
  const [error, setError] = useState(null);

  const [search, setSearch] = useState("");
  const [statusFilter, setStatusFilter] = useState("todos");

  const [selectedUser, setSelectedUser] = useState(null);

  const [confirmModal, setConfirmModal] = useState({
    isOpen: false,
    action: null,
    user: null,
  });


  // ============================================================
  // CARGAR ADMINISTRADORES
  // ============================================================

  const cargarAdministradores = async () => {

    try {

      setLoading(true);
      setError(null);

      const data =
        await managerAccountService.getAllAdmins();

      setUsers(
        Array.isArray(data)
          ? data
          : []
      );

    } catch (error) {

      console.error(
        "Error al cargar administradores:",
        error
      );

      const message =
        error.response?.data ||
        error.message ||
        "No fue posible cargar los administradores.";

      setError(
        typeof message === "string"
          ? message
          : "No fue posible cargar los administradores."
      );

    } finally {

      setLoading(false);

    }

  };


  // ============================================================
  // CARGA INICIAL
  // ============================================================

  useEffect(() => {

    cargarAdministradores();

  }, []);


  // ============================================================
  // FILTRADO
  // ============================================================

  const filteredUsers = useMemo(() => {

    const searchText =
      search.toLowerCase().trim();

    return users.filter((user) => {

      const fullName =
        `${user.name || ""} ${user.lastName || ""}`
          .toLowerCase();

      const username =
        (user.userName || "")
          .toLowerCase();

      const email =
        (user.email || "")
          .toLowerCase();

      const matchesSearch =
        !searchText ||
        fullName.includes(searchText) ||
        username.includes(searchText) ||
        email.includes(searchText);

      const matchesStatus =
        statusFilter === "todos" ||
        (
          statusFilter === "activos" &&
          user.isActive
        ) ||
        (
          statusFilter === "inactivos" &&
          !user.isActive
        );

      return (
        matchesSearch &&
        matchesStatus
      );

    });

  }, [
    users,
    search,
    statusFilter,
  ]);


  // ============================================================
  // ESTADÍSTICAS
  // ============================================================

  const totalUsers =
    users.length;

  const activeUsers =
    users.filter(
      (user) => user.isActive
    ).length;

  const inactiveUsers =
    users.filter(
      (user) => !user.isActive
    ).length;


  // ============================================================
  // VER DETALLE
  // ============================================================

  const handleView = (user) => {

    setSelectedUser(user);

  };


  const handleCloseDetails = () => {

    setSelectedUser(null);

  };


  // ============================================================
  // ACTIVAR
  // ============================================================

  const handleActivate = (user) => {

    setConfirmModal({
      isOpen: true,
      action: "activate",
      user: user,
    });

  };


  // ============================================================
  // INACTIVAR
  // ============================================================

  const handleDeactivate = (user) => {

    setConfirmModal({
      isOpen: true,
      action: "deactivate",
      user: user,
    });

  };


  // ============================================================
  // CANCELAR ACCIÓN
  // ============================================================

  const handleCancelAction = () => {

    if (actionLoading) {

      return;

    }

    setConfirmModal({
      isOpen: false,
      action: null,
      user: null,
    });

  };


  // ============================================================
  // CONFIRMAR ACTIVAR / INACTIVAR
  // ============================================================

  const handleConfirmAction = async () => {

    const {
      action,
      user,
    } = confirmModal;

    if (!user || !action) {

      return;

    }

    try {

      setActionLoading(true);
      setError(null);


      // ACTIVAR

      if (action === "activate") {

        await managerAccountService.activar(
          user.id
        );

      }


      // INACTIVAR

      if (action === "deactivate") {

        await managerAccountService.inactivar(
          user.id
        );

      }


      // CERRAR MODAL

      setConfirmModal({
        isOpen: false,
        action: null,
        user: null,
      });


      // CERRAR DETALLE

      setSelectedUser(null);


      // RECARGAR LISTADO

      await cargarAdministradores();

    } catch (error) {

      console.error(
        `Error al ${
          action === "activate"
            ? "activar"
            : "inactivar"
        } administrador:`,
        error
      );

      const message =
        error.response?.data ||
        error.message ||
        `No fue posible ${
          action === "activate"
            ? "activar"
            : "inactivar"
        } el usuario.`;

      setError(
        typeof message === "string"
          ? message
          : `No fue posible ${
              action === "activate"
                ? "activar"
                : "inactivar"
            } el usuario.`
      );

    } finally {

      setActionLoading(false);

    }

  };


  // ============================================================
  // RENDER
  // ============================================================

  return (

    <div className="min-h-screen bg-slate-50 flex">


      {/* =====================================================
          SIDEBAR
      ====================================================== */}

      <AdminSidebar />


      {/* =====================================================
          CONTENIDO
      ====================================================== */}

      <main className="flex-1 p-8">


        {/* =================================================
            HEADER
        ================================================= */}

        <div className="mb-8 flex items-start justify-between gap-4 flex-wrap">

          <div>

            <div className="flex items-center gap-3">

              <div className="w-12 h-12 rounded-xl bg-violet-100 flex items-center justify-center">

                <FaUsers className="text-violet-600 text-xl" />

              </div>


              <div>

                <h1 className="text-3xl font-bold text-slate-800">

                  Administradores

                </h1>


                <p className="text-slate-500 mt-1">

                  Gestiona las cuentas de administradores de Elysia.

                </p>

              </div>

            </div>

          </div>


          {/* ACTUALIZAR */}

          <button
            type="button"
            onClick={cargarAdministradores}
            disabled={
              loading ||
              actionLoading
            }
            className="flex items-center gap-2 px-4 py-2.5 bg-white border border-slate-200 rounded-xl text-slate-600 font-medium hover:bg-slate-50 transition disabled:opacity-50 disabled:cursor-not-allowed"
          >

            <FaSyncAlt
              className={
                loading
                  ? "animate-spin"
                  : ""
              }
            />

            Actualizar

          </button>

        </div>


        {/* =================================================
            ERROR
        ================================================= */}

        {error && (

          <div className="mb-6 bg-red-50 border border-red-200 rounded-xl p-4 flex items-start gap-3">

            <FaExclamationTriangle className="text-red-500 mt-0.5" />

            <div>

              <p className="font-semibold text-red-700">

                Ocurrió un problema

              </p>

              <p className="text-sm text-red-600 mt-1">

                {error}

              </p>

            </div>

          </div>

        )}


        {/* =================================================
            ESTADÍSTICAS
        ================================================= */}

        <div className="grid grid-cols-1 sm:grid-cols-3 gap-5 mb-6">


          {/* TOTAL */}

          <div className="bg-white rounded-2xl border border-slate-100 shadow-sm p-5">

            <p className="text-sm text-slate-500">

              Total administradores

            </p>

            <p className="text-3xl font-bold text-slate-800 mt-2">

              {totalUsers}

            </p>

          </div>


          {/* ACTIVOS */}

          <div className="bg-white rounded-2xl border border-slate-100 shadow-sm p-5">

            <p className="text-sm text-slate-500">

              Administradores activos

            </p>

            <p className="text-3xl font-bold text-green-600 mt-2">

              {activeUsers}

            </p>

          </div>


          {/* INACTIVOS */}

          <div className="bg-white rounded-2xl border border-slate-100 shadow-sm p-5">

            <p className="text-sm text-slate-500">

              Administradores inactivos

            </p>

            <p className="text-3xl font-bold text-red-500 mt-2">

              {inactiveUsers}

            </p>

          </div>

        </div>


        {/* =================================================
            BUSCADOR / FILTRO
        ================================================= */}

        <div className="bg-white rounded-2xl border border-slate-100 shadow-sm p-5 mb-6">

          <div className="flex flex-col lg:flex-row gap-4">


            {/* BUSCADOR */}

            <div className="relative flex-1">

              <FaSearch className="absolute left-4 top-1/2 -translate-y-1/2 text-slate-400" />

              <input
                type="text"
                value={search}
                onChange={(e) =>
                  setSearch(e.target.value)
                }
                placeholder="Buscar por nombre, usuario o email..."
                className="w-full pl-11 pr-4 py-3 border border-slate-200 rounded-xl outline-none focus:ring-2 focus:ring-violet-500 focus:border-violet-500 text-slate-700"
              />

            </div>


            {/* FILTRO */}

            <div className="relative lg:w-56">

              <FaFilter className="absolute left-4 top-1/2 -translate-y-1/2 text-slate-400 pointer-events-none" />

              <select
                value={statusFilter}
                onChange={(e) =>
                  setStatusFilter(
                    e.target.value
                  )
                }
                className="w-full appearance-none pl-11 pr-4 py-3 border border-slate-200 rounded-xl outline-none focus:ring-2 focus:ring-violet-500 focus:border-violet-500 text-slate-700 bg-white"
              >

                <option value="todos">

                  Todos los estados

                </option>

                <option value="activos">

                  Activos

                </option>

                <option value="inactivos">

                  Inactivos

                </option>

              </select>

            </div>

          </div>


          {/* RESULTADOS */}

          <div className="mt-4 text-sm text-slate-500">

            Mostrando{" "}

            <span className="font-semibold text-slate-700">

              {filteredUsers.length}

            </span>{" "}

            de{" "}

            <span className="font-semibold text-slate-700">

              {totalUsers}

            </span>{" "}

            administradores.

          </div>

        </div>


        {/* =================================================
            TABLA
        ================================================= */}

        <UserTable
          users={filteredUsers}
          loading={loading}
          onView={handleView}
          onActivate={handleActivate}
          onDeactivate={handleDeactivate}
        />

      </main>


      {/* =====================================================
          DETALLE
      ====================================================== */}

      {selectedUser && (

        <UserDetailModal
          user={selectedUser}
          onClose={handleCloseDetails}
        />

      )}


      {/* =====================================================
          CONFIRMACIÓN
      ====================================================== */}

      {confirmModal.isOpen && (

        <ConfirmActionModal
          isOpen={confirmModal.isOpen}
          action={confirmModal.action}
          user={confirmModal.user}
          loading={actionLoading}
          onConfirm={handleConfirmAction}
          onCancel={handleCancelAction}
        />

      )}


      {/* =====================================================
          LOADING DE ACCIÓN
      ====================================================== */}

      {actionLoading && (

        <div className="fixed bottom-6 right-6 bg-slate-800 text-white px-5 py-3 rounded-xl shadow-lg flex items-center gap-3 z-50">

          <div className="w-5 h-5 border-2 border-white/30 border-t-white rounded-full animate-spin" />

          <span className="text-sm font-medium">

            Actualizando administrador...

          </span>

        </div>

      )}

    </div>

  );

}
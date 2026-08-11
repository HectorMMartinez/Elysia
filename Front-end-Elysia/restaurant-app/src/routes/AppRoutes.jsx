import { BrowserRouter, Routes, Route } from "react-router-dom";

// Auth
import LoginPage from "../pages/auth/LoginPage";
import RegisterPage from "../pages/auth/RegisterPage";
import ForgotPasswordPage from "../pages/auth/ForgotPasswordPage";
import ResetPasswordPage from "../pages/auth/ResetPasswordPage";
import ConfirmAccountPage from "../pages/auth/ConfirmAccountPage";

// Propietario
import OwnerDashboardPage from "../pages/dashboard/OwnerDashboardPage";
import InventoryPage from "../pages/inventory/InventoryPage";
import PlatosPage from "../pages/dishes/PlatosPage";
import { TablesPage } from "../pages/tables/TablesPage";
import MenuPage from "../pages/menu/MenuPage";
// import ReservasPage from "../pages/reservas/ReservasPage";
// import PedidosPage from "../pages/pedidos/PedidosPage";
// import TurnosPage from "../pages/turnos/TurnosPage";
// import EmpleadosPage from "../pages/empleados/EmpleadosPage";
// import CentralInteligenciaPage from "../pages/central/CentralInteligenciaPage";
// import OwnerPerfilPage from "../pages/perfil/OwnerPerfilPage";

// Admin
import AdminDashboardPage from "../pages/admin/AdminDashboardPage";
import PropietariosPage from "../pages/admin/AdminPropietariosPage";
import AdminAdministradoresPage from "../pages/admin/AdminAdministradoresPage";
import MembresiasPage from "../pages/admin/MembresiasPage";
import TarjetasPage from "../pages/admin/TarjetasPage";
import PerfilPage from "../pages/admin/PerfilPage";


import PrivateRoute from "./PrivateRoute";

export default function AppRoutes() {
  return (
    <BrowserRouter>
      <Routes>
        {/* ========== RUTAS PÚBLICAS ========== */}
        <Route path="/" element={<LoginPage />} />
        <Route path="/register" element={<RegisterPage />} />
        <Route path="/forgot-password" element={<ForgotPasswordPage />} />
        <Route path="/reset-password" element={<ResetPasswordPage />} />
        <Route path="/confirm-account" element={<ConfirmAccountPage />} />

        {/* ========== RUTAS PROPIETARIO ========== */}
        <Route
          path="/dashboard"
          element={
            <PrivateRoute>
              <OwnerDashboardPage />
            </PrivateRoute>
          }
        />

        <Route
          path="/inventario"
          element={
            <PrivateRoute>
              <InventoryPage />
            </PrivateRoute>
          }
        />

        <Route
          path="/platos"
          element={
            <PrivateRoute>
              <PlatosPage />
            </PrivateRoute>
          }
        />

        <Route
          path="/mesas"
          element={
            <PrivateRoute>
              <TablesPage />
            </PrivateRoute>
          }
        />

        <Route
          path="/menu"
          element={
            <PrivateRoute>
              <MenuPage />
            </PrivateRoute>
          }
        />

        {/* Descomenta cuando tengas las páginas listas */}
        {/* 
        <Route path="/reservas" element={<PrivateRoute><ReservasPage /></PrivateRoute>} />
        <Route path="/pedidos" element={<PrivateRoute><PedidosPage /></PrivateRoute>} />
        <Route path="/turnos" element={<PrivateRoute><TurnosPage /></PrivateRoute>} />
        <Route path="/empleados" element={<PrivateRoute><EmpleadosPage /></PrivateRoute>} />
        <Route path="/central-inteligencia" element={<PrivateRoute><CentralInteligenciaPage /></PrivateRoute>} />
        <Route path="/perfil" element={<PrivateRoute><OwnerPerfilPage /></PrivateRoute>} />
        */}

        {/* ========== RUTAS ADMIN ========== */}
        <Route
          path="/admin/dashboard"
          element={
            <PrivateRoute>
              <AdminDashboardPage />
            </PrivateRoute>
          }
        />

     <Route
      path="/admin/propietarios"
      element={
       <PrivateRoute>
         <PropietariosPage />
       </PrivateRoute>
      }/>

      <Route
       path="/admin/administradores"
       element={
       <PrivateRoute>
         <AdminAdministradoresPage />
       </PrivateRoute>
      } 
      />

      <Route
       path="/admin/membresias"
       element={
       <PrivateRoute>
        <MembresiasPage />
      </PrivateRoute>
      }/>

      <Route
       path="/admin/tarjetas"
       element={
       <PrivateRoute>
        <TarjetasPage />
      </PrivateRoute>
      }/>

       <Route
       path="/admin/perfil"
       element={
       <PrivateRoute>
        <PerfilPage/>
      </PrivateRoute>
      }/>
      
      </Routes>
    </BrowserRouter>
  );
}
import { useEffect, useState } from "react";
import { FaArrowLeft, FaBoxOpen } from "react-icons/fa";
import OwnerSidebar from "../../components/layout/OwnerSidebar";
import { obtenerProductos } from "../../services/productoService";

export default function InventoryPage() {
  const [productos, setProductos] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  useEffect(() => {
    cargarProductos();
  }, []);

  const cargarProductos = async () => {
    try {
      setLoading(true);
      setError("");

      const data = await obtenerProductos();

      setProductos(Array.isArray(data) ? data : []);
    } catch (error) {
      console.error("Error al cargar productos:", error);

      const mensaje =
        error.response?.data ||
        "No fue posible cargar los productos del inventario.";

      setError(
        typeof mensaje === "string"
          ? mensaje
          : "No fue posible cargar los productos."
      );
    } finally {
      setLoading(false);
    }
  };

return (
  <div className="min-h-screen bg-slate-100 flex">
    <OwnerSidebar />

    <main className="flex-1 p-8">
      <div className="flex items-center justify-between mb-8">
        <div>
          <h1 className="text-4xl font-bold text-slate-800">
            Inventario
          </h1>

          <p className="text-slate-500 mt-2">
            Consulta los productos registrados en el sistema.
          </p>
        </div>

        <div className="bg-blue-100 text-blue-600 p-4 rounded-2xl">
          <FaBoxOpen size={32} />
        </div>
      </div>

      {/* Aquí mantienes loading, error, tabla y estado vacío */}
    </main>
  </div>
);
}
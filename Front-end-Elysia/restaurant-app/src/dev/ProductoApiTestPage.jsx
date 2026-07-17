import { useState } from "react";
import {
  obtenerProductos,
  obtenerProductoPorId,
  crearProducto,
  editarProducto,
  eliminarProducto,
  registrarEntrada,
  registrarSalida,
} from "../services/productoService";

export default function ProductoApiTestPage() {
  const [resultado, setResultado] = useState(null);
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);
  const [imagen, setImagen] = useState(null);

  const ejecutarPrueba = async (nombre, funcion) => {
    try {
      setLoading(true);
      setError("");
      setResultado(null);

      const data = await funcion();

      console.log(`${nombre}:`, data);

      setResultado({
        prueba: nombre,
        data,
      });
    } catch (error) {
      console.error(`${nombre}:`, error);

      setError(error.message);
    } finally {
      setLoading(false);
    }
  };

  const probarObtenerTodos = () => {
    ejecutarPrueba("Obtener todos los productos", () =>
      obtenerProductos()
    );
  };

  const probarObtenerPorId = () => {
    ejecutarPrueba("Obtener producto por ID", () =>
      obtenerProductoPorId(3)
    );
  };

  const probarCrear = () => {
    if (!imagen) {
      setError("Selecciona una imagen antes de crear el producto.");
      return;
    }

    ejecutarPrueba("Crear producto", () =>
      crearProducto({
        nombre: "Azúcar",
        descripcion: "Paquete de azúcar de prueba",
        unidadMedida: "Lb",
        stockActual: 10,
        stockMinimo: 3,
        imagen,
      })
    );
  };

  const probarEditar = () => {
  ejecutarPrueba("Editar producto", () =>
    editarProducto(3, {
      nombre: "Azúcar morena",
      descripcion: "Paquete de azúcar actualizado",
      unidadMedida: "Lb",
      stockActual: 15,
      stockMinimo: 5,
      activo: true,
      imagen,
    })
  );
};

const probarEntrada = () => {
  ejecutarPrueba("Registrar entrada", () =>
    registrarEntrada({
      productoId: 3,
      cantidad: 5,
    })
  );
};


  const probarSalida = () => {
  ejecutarPrueba("Registrar salida", () =>
    registrarSalida({
      productoId: 3,
      cantidad: 2,
    })
  );
};


const probarEliminar = () => {
  ejecutarPrueba("Eliminar producto", () =>
    eliminarProducto(3)
  );
};
  return (
    <div className="min-h-screen bg-slate-100 p-8">
      <div className="max-w-4xl mx-auto bg-white rounded-2xl shadow-sm p-8">
        <h1 className="text-3xl font-bold text-slate-800">
          Pruebas API de Productos
        </h1>

        <p className="text-slate-500 mt-2">
          Página temporal para validar los endpoints del módulo de inventario.
        </p>

        <div className="mt-6">
          <label className="block font-medium mb-2">
            Imagen para crear o editar
          </label>

          <input
            type="file"
            accept="image/*"
            onChange={(event) =>
              setImagen(event.target.files?.[0] || null)
            }
          />
        </div>

        <div className="grid md:grid-cols-2 gap-4 mt-8">
          <button
            type="button"
            onClick={probarObtenerTodos}
            className="bg-violet-600 text-white px-4 py-3 rounded-xl"
          >
            Obtener todos
          </button>

          <button
            type="button"
            onClick={probarObtenerPorId}
            className="bg-blue-600 text-white px-4 py-3 rounded-xl"
          >
            Obtener ID 3
          </button>

          <button
            type="button"
            onClick={probarCrear}
            className="bg-green-600 text-white px-4 py-3 rounded-xl"
          >
            Crear producto
          </button>

          <button
            type="button"
            onClick={probarEditar}
            className="bg-amber-600 text-white px-4 py-3 rounded-xl"
          >
            Editar producto 3
          </button>

          <button
            type="button"
            onClick={probarEntrada}
            className="bg-cyan-600 text-white px-4 py-3 rounded-xl"
          >
            Entrada +5
          </button>

          <button
            type="button"
            onClick={probarSalida}
            className="bg-orange-600 text-white px-4 py-3 rounded-xl"
          >
            Salida -2
          </button>

          <button
            type="button"
            onClick={probarEliminar}
            className="bg-red-600 text-white px-4 py-3 rounded-xl md:col-span-2"
          >
            Eliminar producto 3
          </button>
        </div>

        {loading && (
          <p className="mt-6 text-violet-600">
            Ejecutando prueba...
          </p>
        )}

        {error && (
          <div className="mt-6 bg-red-50 border border-red-200 p-4 rounded-xl">
            <strong>Error:</strong> {error}
          </div>
        )}

        {resultado && (
          <div className="mt-6">
            <h2 className="font-bold text-lg">
              {resultado.prueba}
            </h2>

            <pre className="mt-3 bg-slate-900 text-green-300 p-4 rounded-xl overflow-auto text-sm">
              {JSON.stringify(resultado.data, null, 2)}
            </pre>
          </div>
        )}
      </div>
    </div>
  );
}
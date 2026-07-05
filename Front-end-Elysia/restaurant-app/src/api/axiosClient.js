import axios from "axios";
import { storage } from "../utils/storage";

const axiosClient = axios.create({
  baseURL: import.meta.env.VITE_API_URL,
  headers: {
    "Content-Type": "application/json",
  },
});

// Interceptor para agregar el JWT automáticamente
axiosClient.interceptors.request.use(
  (config) => {

    // NO enviar token al endpoint de planes
    if (!config.url.includes("/Plans/") && !config.url.includes("/Account/")) {

      const auth = storage.getAuth();

      if (auth?.token) {
        config.headers.Authorization = `Bearer ${auth.token}`;
      }
    }

    return config;
  }
);

export default axiosClient;
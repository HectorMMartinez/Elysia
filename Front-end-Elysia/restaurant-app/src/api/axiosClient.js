import axios from "axios";
import { storage } from "../utils/storage";

const axiosClient = axios.create({
  baseURL: import.meta.env.VITE_API_URL,
});

axiosClient.interceptors.request.use(
  (config) => {
    const esFormData = config.data instanceof FormData;

    if (esFormData) {
      delete config.headers["Content-Type"];
    } else {
      config.headers["Content-Type"] = "application/json";
    }

    const esEndpointPublico =
      config.url?.includes("/Plans/") ||
      config.url?.includes("/Account/");

    if (!esEndpointPublico) {
      const auth = storage.getAuth();

      if (auth?.token) {
        config.headers.Authorization =
          `Bearer ${auth.token}`;
      }
    }

    return config;
  },
  (error) => Promise.reject(error)
);

export default axiosClient;
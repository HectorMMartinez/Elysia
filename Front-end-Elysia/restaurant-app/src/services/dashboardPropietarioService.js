import axiosClient from "../api/axiosClient";
import ENDPOINTS from "../api/endpoints";
import { handleApiError } from "../utils/apiError";

const dashboardService = {
  async getPanel() {
    try {
      const response = await axiosClient.get(ENDPOINTS.DASHBOARD.GET_PANEL);
      return {
        success: true,
        data: response.data,
      };
    } catch (error) {
      return handleApiError(error);
    }
  },

  async cambiarPlan(usuarioId) {
    try {
      const response = await axiosClient.put(
        `${ENDPOINTS.PLAN.CAMBIAR_PLAN}/${usuarioId}`
      );
      return {
        success: true,
        data: response.data,
      };
    } catch (error) {
      return handleApiError(error);
    }
  },
};

export default dashboardService;
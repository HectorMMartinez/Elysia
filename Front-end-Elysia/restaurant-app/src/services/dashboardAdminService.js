import axiosClient from "../api/axiosClient";
import ENDPOINTS from "../api/endpoints";
import { handleApiError } from "../utils/apiError";

const dashboardAdminService = {
  async getPanel() {
    try {
      const response = await axiosClient.get(
        ENDPOINTS.DASHBOARD_ADMIN.GET_PANEL
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

export default dashboardAdminService;
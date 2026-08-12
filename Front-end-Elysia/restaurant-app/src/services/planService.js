import axiosClient from "../api/axiosClient";
import ENDPOINTS from "../api/endpoints";

const planService = {

    async getAll() {

        const response = await axiosClient.get(
            ENDPOINTS.PLAN.GET_ALL
        );

        return response.data;
    },
    async cambiarPlanASimple(userId) {
    const response = await axiosClient.put(
      ENDPOINTS.PLAN.CAMBIAR_PLAN_A_SIMPLE(userId)
    );
    return response.data;
  },

};

export default planService;


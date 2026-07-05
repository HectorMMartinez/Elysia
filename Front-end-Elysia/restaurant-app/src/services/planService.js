import axiosClient from "../api/axiosClient";
import ENDPOINTS from "../api/endpoints";

const planService = {

    async getAll() {

        const response = await axiosClient.get(
            ENDPOINTS.PLAN.GET_ALL
        );

        return response.data;
    }

};

export default planService;


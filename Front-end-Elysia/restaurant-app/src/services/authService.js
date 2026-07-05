import axiosClient from "../api/axiosClient";
import ENDPOINTS from "../api/endpoints";
import { handleApiError } from "../utils/apiError";

const authService = {

    
    async login(data) {

    try {

        const response = await axiosClient.post(
            ENDPOINTS.AUTH.LOGIN,
            data
        );

        return {

            success: true,
            data: response.data

        };

    }
    catch (error) {

        return handleApiError(error);

    }
},

  async register(formData) {
    try {
      const response = await axiosClient.post(
        ENDPOINTS.AUTH.REGISTER,
        formData,
        {
          headers: {
            "Content-Type": "multipart/form-data"
          }
        }
      );

      return {
        success: true,
        data: response.data
      };
    } catch (error) {
      return handleApiError(error);
    }
  },
  

async confirmAccount(data) {

    try {

        const response = await axiosClient.post(

            ENDPOINTS.AUTH.CONFIRM_ACCOUNT,
            data

        );

        return {

            success: true,
            data: response.data

        };

    }
    catch (error) {

        return handleApiError(error);

    }

},




  
    async forgotPassword(data) {

    try {

        await axiosClient.post(
            ENDPOINTS.AUTH.FORGOT_PASSWORD,
            data
        );

        return {

            success: true

        };

    }
    catch (error) {

        return handleApiError(error);

    }

},


  async resetPassword(data) {

    try {

        await axiosClient.post(
            ENDPOINTS.AUTH.RESET_PASSWORD,
            data
        );

        return {

            success: true

        };

    }
    catch (error) {

        return handleApiError(error);

    }

},


};

export default authService;
export function handleApiError(error) {

    if (error.response) {

        const data = error.response.data;

        let message = "Ocurrió un error.";

        if (typeof data === "string") {

            message = data;

        }
        else if (data?.message) {

            message = data.message;

        }
        else if (data?.title) {

            message = data.title;

        }
        else if (data?.errors) {

            message = Object.values(data.errors)
                .flat()
                .join("\n");

        }

        return {
            success: false,
            status: error.response.status,
            message
        };
    }

    if (error.request) {

        return {
            success: false,
            status: 0,
            message: "No fue posible conectar con el servidor."
        };
    }

    return {
        success: false,
        status: 0,
        message: error.message || "Ocurrió un error inesperado."
    };
}
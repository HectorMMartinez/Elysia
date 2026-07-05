export default function FormButton({

    children,

    loading = false,

    type = "submit"

}) {

    return (

        <button

            type={type}

            disabled={loading}

            className="

                w-full
                rounded-lg
                bg-violet-600
                py-3
                font-semibold
                text-white
                transition

                hover:bg-violet-700

                disabled:opacity-70

                disabled:cursor-not-allowed

            "

        >

            {

                loading

                    ? "Cargando..."

                    : children

            }

        </button>

    );

}
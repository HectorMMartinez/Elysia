export default function FormInput({
    label,
    name,
    type = "text",
    placeholder = "",
    register,
    rules = {},
    error,
    disabled = false,
    accept,
    className = ""
}) {

    const inputClass = `

        w-full
        rounded-lg
        border
        px-4
        py-3
        outline-none
        transition

        ${error
            ? "border-red-500 focus:ring-2 focus:ring-red-500"
            : "border-slate-300 focus:ring-2 focus:ring-violet-500"}

        ${disabled ? "bg-slate-100 cursor-not-allowed" : ""}

        ${className}

    `;

    return (

        <div className="space-y-2">

            <label className="block text-sm font-medium text-slate-700">

                {label}

            </label>

            <input

                type={type}

                placeholder={placeholder}

                disabled={disabled}

                accept={accept}

                {...register(name, rules)}

                className={inputClass}

            />

            {

                error && (

                    <p className="text-sm text-red-500">

                        {error.message}

                    </p>

                )

            }

        </div>

    );

}
import FormInput from "./FormInput";

export default function FormRenderer({
  fields,
  register,
  errors,
  disabled = false,
}) {
  return (
    <div className="space-y-5">
      {fields.map((field) => (
        <FormInput
          key={field.name}
          label={field.label}
          name={field.name}
          type={field.type}
          placeholder={field.placeholder}
          register={register}
          error={errors?.[field.name]}
          disabled={disabled}
        />
      ))}
    </div>
  );
}
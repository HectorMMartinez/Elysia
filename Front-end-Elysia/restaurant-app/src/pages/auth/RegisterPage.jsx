// IMPORTS

import { useState, useEffect } from "react";
import { Link, useNavigate } from "react-router-dom";
import { useForm } from "react-hook-form";

import {
  FaUser,
  FaEnvelope,
  FaLock,
  FaPhone,
  FaIdCard,
  FaCamera,
} from "react-icons/fa";

import FormInput from "../../components/forms/FormInput";
import FormButton from "../../components/forms/FormButton";
import FormAlert from "../../components/forms/FormAlert";

import authService from "../../services/authService";
import planService from "../../services/planService";

import { registerValidation } from "../../validations/registerValidation";



export default function RegisterPage() {
  const [step, setStep] = useState(1);
  const [loading, setLoading] = useState(false);

  const [apiError, setApiError] = useState("");
  const [plans, setPlans] = useState([]);

  const [loadingPlans, setLoadingPlans] = useState(false);
  const [selectedPlanId, setSelectedPlanId] = useState(null);
  const navigate = useNavigate();

  const {
  register,
  handleSubmit,
  watch,
  trigger,
  setValue,
  formState: { errors },
} = useForm({
  mode: "onBlur",
  defaultValues: {
    // ===== PASO 1 =====
    name: "",
    lastName: "",
    username: "",
    email: "",
    password: "",
    confirmPassword: "",
    phone: "",
    cardId: "",
    profileImage: null,

    // ===== PASO 2 =====
    restaurantName: "",
    specialty: "",
    restaurantPhone: "",
    rnc: "",
    openingTime: "",
    closingTime: "",
    address: "",
    restaurantLogo: null,

    // ===== PASO 3 =====
    termsAccepted: false,

    cardholderName: "",
    cardNumber: "",
    cardType: "Visa",
    cvv: "",
    expiryMonth: "",
    expiryYear: "",
  },
});

  const password = watch("password");
  const termsAccepted = watch("termsAccepted");

  useEffect(() => {

    const loadPlans = async () => {

        try {

            setLoadingPlans(true);

            const plans = await planService.getAll();

            setPlans(plans);

        } catch (error) {

            setApiError("No se pudieron cargar los planes.");

        } finally {

            setLoadingPlans(false);

        }

    };

    loadPlans();

}, []);



  const validateStep1 = async () => {

    return await trigger([
        "name",
        "lastName",
        "username",
        "email",
        "password",
        "confirmPassword",
        "phone",
        "cardId"
    ]);

};

const validateStep2 = async () => {

    return await trigger([
        "restaurantName",
        "specialty",
        "restaurantPhone",
        "rnc",
        "openingTime",
        "closingTime",
        "address"
    ]);

};


const validateStep3 = async () => {

    setApiError("");

    if (!selectedPlanId) {

        setApiError("Debe seleccionar un plan.");

        return false;

    }

    const isValid = await trigger([
        "termsAccepted",
        "cardholderName",
        "cardNumber",
        "cardType",
        "cvv",
        "expiryMonth",
        "expiryYear"
    ]);

    return isValid;

};


 const handleNext = async ()=>{

    setApiError("");

    if(step===1){

        if(await validateStep1()){

            setStep(2);

        }

        return;

    }

    if(step===2){

        if(await validateStep2()){

            setStep(3);

        }

    }

};

const handlePrevious = ()=>{

    setApiError("");

    setStep(step-1);

};


const onSubmit = async(data)=>{

    if(!(await validateStep3())) return;

    setLoading(true);

    setApiError("");

    try{

       const formData = new FormData();

    // ===== USER =====
    formData.append("name", data.name);
    formData.append("lastName", data.lastName);
    formData.append("username", data.username);
    formData.append("email", data.email);
    formData.append("password", data.password);
    formData.append("phone", data.phone);
    formData.append("idCard", data.cardId);

    if (data.profileImage) {
      formData.append("profileImage", data.profileImage);
    }

    // ===== RESTAURANT =====
    formData.append("nombreRestaurante", data.restaurantName);
    formData.append("especialidad", data.specialty);
    formData.append("phoneRestaurante", data.restaurantPhone);
    formData.append("rnc", data.rnc);
    formData.append("horaApertura", data.openingTime);
    formData.append("horaCierre", data.closingTime);
    formData.append("direccionRestaurante", data.address);
          
     if (data.restaurantLogo) {
      formData.append("logoRestaurante", data.restaurantLogo);
    }


    // ===== PLAN =====
    formData.append("planId", selectedPlanId);

    // ===== PAYMENT =====
    formData.append("nombreTitular", data.cardholderName);
    formData.append("numeroTarjeta", data.cardNumber);
    formData.append("tipo", data.cardType);
    formData.append("cvv", data.cvv);
    formData.append("mesVencimiento", data.expiryMonth);
    formData.append("anioVencimiento", data.expiryYear);

    const res = await authService.register(formData);

    if (res.success) {
        navigate("/", {
        state: {
            message: "Registro exitoso. Revisa tu correo (incluida la carpeta de spam) para confirmar tu cuenta."
        }
    });
    } else {
      setApiError(res.message || "Error en registro");
    }


    }catch(error){

        setApiError("Ha ocurrido un error.");

    }finally{

        setLoading(false);

    }

};




  return (
    <div className="min-h-screen bg-slate-100 py-10 px-4">
      <div className="max-w-5xl mx-auto bg-white rounded-2xl shadow-xl p-8">

        {/* Header */}
        <div className="text-center mb-10">
          <div className="w-20 h-20 mx-auto rounded-full bg-violet-600 flex items-center justify-center text-white text-3xl font-bold">
            E
          </div>

          <h1 className="mt-4 text-3xl font-bold text-slate-800">
            Registro de Propietario
          </h1>

          <p className="text-slate-500">
            Complete la información para crear su cuenta
          </p>
        </div>

        {/* Stepper */}
        <div className="flex justify-center items-center mb-10">

          <div
            className={`w-12 h-12 rounded-full flex items-center justify-center text-white font-bold ${
              step >= 1 ? "bg-violet-600" : "bg-slate-300"
            }`}
          >
            1
          </div>

          <div className="w-24 h-1 bg-slate-300"></div>

          <div
            className={`w-12 h-12 rounded-full flex items-center justify-center text-white font-bold ${
              step >= 2 ? "bg-violet-600" : "bg-slate-300"
            }`}
          >
            2
          </div>

          <div className="w-24 h-1 bg-slate-300"></div>

          <div
            className={`w-12 h-12 rounded-full flex items-center justify-center text-white font-bold ${
              step >= 3 ? "bg-violet-600" : "bg-slate-300"
            }`}
          >
            3
          </div>
        </div>
        {apiError && (
         <div className="mb-6">
             <FormAlert message={apiError} />
         </div>
        )}

        {/* Paso 1 */}

        {step === 1 && (
            <div>

    {/* Tarjeta encabezado */}

    <div className="mb-8 bg-gradient-to-r from-violet-600 to-purple-700 rounded-2xl p-6 text-white shadow-lg">
      <h2 className="text-3xl font-bold">
        Información Personal
      </h2>

      <p className="mt-2 text-violet-100">
        Complete los datos del propietario para crear su cuenta en Elysia.
      </p>
    </div>

    {/* Foto Perfil */}

    <div className="mb-8">

      <label className="block text-sm font-semibold text-slate-700 mb-3">
        Foto de Perfil
      </label>

      <div className="border-2 border-dashed border-violet-300 rounded-2xl p-8 bg-violet-50 hover:bg-violet-100 transition cursor-pointer">

        <div className="flex flex-col items-center">

          <div className="w-20 h-20 rounded-full bg-violet-600 flex items-center justify-center text-white text-3xl mb-4">
            <FaCamera />
          </div>

          <h3 className="font-semibold text-slate-700">
            Subir fotografía
          </h3>

          <p className="text-slate-500 text-sm mt-1">
            PNG, JPG o JPEG
          </p>

          <input
          type="file"
          accept=".jpg,.jpeg,.png"
          onChange={(e) => {
          setValue(
          "profileImage",
           e.target.files[0],
           {
            shouldValidate: true
           });
          }}
          className="mt-4"/>

           {errors.profileImage && (
          <p className="text-red-500 text-sm mt-1">
              {errors.profileImage.message}
          </p>
          )}
        </div>

      </div>

    </div>

    {/* Formulario */}

    <div className="bg-white border border-slate-200 rounded-2xl p-8 shadow-sm">

      <div className="grid grid-cols-1 md:grid-cols-2 gap-6">

        <div>
          <label className="block mb-2 font-medium">
            Nombre
          </label>

          <div className="relative">
            <FaUser className="absolute left-4 top-4 text-slate-400" />

           <input
          type="text"
          placeholder="Kelvin"
          {...register("name", registerValidation.name)}
          className={`w-full pl-12 pr-4 py-3 border rounded-xl focus:ring-2 focus:ring-violet-500 focus:outline-none ${
           errors.name
            ? "border-red-500"
            : "border-slate-300" }`}/>

          {errors.name && (
          <p className="text-red-500 text-sm mt-1">
              {errors.name.message}
          </p>
          )}
          </div>
        </div>

        <div>
          <label className="block mb-2 font-medium">
            Apellido
          </label>

          <div className="relative">
            <FaUser className="absolute left-4 top-4 text-slate-400" />

            <input
              type="text"
              placeholder="Díaz"
              {...register("lastName", registerValidation.lastName)}
              className={`w-full pl-12 pr-4 py-3 border rounded-xl focus:ring-2 focus:ring-violet-500 focus:outline-none 
                ${errors.lastName ? "border-red-500" : "border-slate-300" }`}/>

            {errors.lastName && (
              <p className="text-red-500 text-sm mt-1">
                {errors.lastName.message}
              </p>
            )}
          </div>
        </div>

         
        <div>
          <label className="block mb-2 font-medium">
            Usuario
          </label>

          <div className="relative">
            <FaUser className="absolute left-4 top-4 text-slate-400" />

            <input
              type="text"
              placeholder="kelvindiaz"
              {...register("username", registerValidation.username)}
              className={`w-full pl-12 pr-4 py-3 border rounded-xl focus:ring-2 focus:ring-violet-500 focus:outline-none ${
                errors.username ? "border-red-500" : "border-slate-300"
              }`}
            />
            {errors.username && (
              <p className="text-red-500 text-sm mt-1">
                {errors.username.message}
              </p>
            )}
          </div>
        </div>
         


        <div>
          <label className="block mb-2 font-medium">
            Correo
          </label>

          <div className="relative">
            <FaEnvelope className="absolute left-4 top-4 text-slate-400" />

            <input
              type="email"
              placeholder="correo@email.com"
              {...register("email", registerValidation.email)}
              className={`w-full pl-12 pr-4 py-3 border rounded-xl focus:ring-2 focus:ring-violet-500 focus:outline-none ${
                errors.email ? "border-red-500" : "border-slate-300"
              }`}
            />
            {errors.email && (
              <p className="text-red-500 text-sm mt-1">
                {errors.email.message}
              </p>
            )}
          </div>
        </div>
            

        <div>
          <label className="block mb-2 font-medium">
            Contraseña
          </label>

          <div className="relative">
            <FaLock className="absolute left-4 top-4 text-slate-400" />

            <input
              type="password"
              placeholder="********"
              {...register("password", registerValidation.password)}
              className={`w-full pl-12 pr-4 py-3 border rounded-xl focus:ring-2 focus:ring-violet-500 focus:outline-none ${
              errors.password? "border-red-500": "border-slate-300"}`}/>

              {errors.password && (
                  <p className="text-red-500 text-sm mt-1">
              {errors.password.message}</p>)}
          </div>
        </div>


        <div>
          <label className="block mb-2 font-medium">
            Confirmar Contraseña
          </label>

          <div className="relative">
            <FaLock className="absolute left-4 top-4 text-slate-400" />

            <input type="password"
                  placeholder="********"
                   {...register("confirmPassword", {
                    required: "Debe confirmar la contraseña",
                   validate: (value) =>
                   value === watch("password") || "Las contraseñas no coinciden",})}
                   className={`w-full pl-12 pr-4 py-3 border rounded-xl focus:ring-2 focus:ring-violet-500 focus:outline-none ${
                   errors.confirmPassword
                    ? "border-red-500"
                    : "border-slate-300"
                    }`}/>

                  {errors.confirmPassword && (
                     <p className="text-red-500 text-sm mt-1">
                   {errors.confirmPassword.message}
                   </p>)}
          </div>
        </div>


        <div>
          <label className="block mb-2 font-medium">
            Teléfono
          </label>

          <div className="relative">
            <FaPhone className="absolute left-4 top-4 text-slate-400" />
              <input
               type="text"
               placeholder="8091234567"
               {...register("phone", registerValidation.phone)}
               className={`w-full pl-12 pr-4 py-3 border rounded-xl focus:ring-2 focus:ring-violet-500 focus:outline-none ${
               errors.phone
               ? "border-red-500"
               : "border-slate-300"}`}/>

               {errors.phone && (
                       <p className="text-red-500 text-sm mt-1">
               {errors.phone.message}</p>)}
          </div>
        </div>


        <div>
          <label className="block mb-2 font-medium">
            Cédula
          </label>

          <div className="relative">
            <FaIdCard className="absolute left-4 top-4 text-slate-400" />

            <input
             type="text"
             placeholder="00112345678"
             {...register("cardId", registerValidation.cardId)}
             className={`w-full pl-12 pr-4 py-3 border rounded-xl focus:ring-2 focus:ring-violet-500 focus:outline-none ${
             errors.cardId
             ? "border-red-500"
             : "border-slate-300"}`}/>

             {errors.cardId && (
                   <p className="text-red-500 text-sm mt-1">
             {errors.cardId.message}</p>
            )}
          </div>
        </div>
      </div>
    </div>  
  </div>
        )}

        {/* Paso 2 */}

        {step === 2 && (
          <div>

    {/* Encabezado */}

    <div className="mb-8 bg-gradient-to-r from-amber-500 to-orange-600 rounded-2xl p-6 text-white shadow-lg">
      <h2 className="text-3xl font-bold">
        Información del Restaurante
      </h2>

      <p className="mt-2 text-orange-100">
        Registre la información principal de su negocio.
      </p>
    </div>

    {/* Logo */}

    <div className="mb-8">

      <label className="block text-sm font-semibold text-slate-700 mb-3">
        Logo del Restaurante
      </label>

      <div className="border-2 border-dashed border-orange-300 rounded-2xl p-8 bg-orange-50 hover:bg-orange-100 transition cursor-pointer">

        <div className="flex flex-col items-center">

          <div className="w-20 h-20 rounded-full bg-orange-500 flex items-center justify-center text-white text-3xl mb-4">
            🏪
          </div>

          <h3 className="font-semibold text-slate-700">
            Subir Logo
          </h3>

          <p className="text-slate-500 text-sm mt-1">
            PNG, JPG o JPEG
          </p>

          <input
          type="file"
          accept=".jpg,.jpeg,.png"
          onChange={(e) => {
          setValue(
          "restaurantLogo",
          e.target.files[0],
           {
             shouldValidate: true
           });}}

          className="mt-4"/>
          
           {errors.restaurantLogo && (
                   <p className="text-red-500 text-sm mt-1">
             {errors.restaurantLogo.message}</p>
           )}
          
        </div>

      </div>

    </div>

    {/* Formulario */}

    <div className="bg-white border border-slate-200 rounded-2xl p-8 shadow-sm">

      <div className="grid grid-cols-1 md:grid-cols-2 gap-6">

        {/* Nombre Restaurante */}

         <div>
         <label className="block mb-2 font-medium">
                Nombre Restaurante
         </label>

        <input
        type="text"
        placeholder="Elysia Restaurant"
        {...register("restaurantName", registerValidation.restaurantName)}
        className={`w-full px-4 py-3 border rounded-xl focus:ring-2 focus:ring-orange-500 focus:outline-none ${
        errors.restaurantName
        ? "border-red-500"
        : "border-slate-300"}`}/>

       {errors.restaurantName && (
        <p className="text-red-500 text-sm mt-1">
            {errors.restaurantName.message}
        </p>)}
       </div>

        {/* Especialidad */}

        <div>
       <label className="block mb-2 font-medium">
           Especialidad
       </label>

      <input
        type="text"
        placeholder="Comida Dominicana"
        {...register("specialty", registerValidation.specialty)}
        className={`w-full px-4 py-3 border rounded-xl focus:ring-2 focus:ring-orange-500 focus:outline-none ${
        errors.specialty
         ? "border-red-500"
         : "border-slate-300" }`}/>

        {errors.specialty && (
        <p className="text-red-500 text-sm mt-1">
         {errors.specialty.message} </p>
         )}</div>

        {/* Teléfono */}

         <div>
        <label className="block mb-2 font-medium">
             Teléfono Restaurante
        </label>

         <input
          type="text"
          placeholder="8091234567"
          {...register("restaurantPhone", registerValidation.restaurantPhone)}
          className={`w-full px-4 py-3 border rounded-xl focus:ring-2 focus:ring-orange-500 focus:outline-none ${
          errors.restaurantPhone
          ? "border-red-500"
          : "border-slate-300"}`}/>

          {errors.restaurantPhone && (
          <p className="text-red-500 text-sm mt-1">
          {errors.restaurantPhone.message}
         </p>)}</div>


        {/* RNC */}

        <div>
         <label className="block mb-2 font-medium">
         RNC
       </label>

      <input
        type="text"
        placeholder="123456789"
        {...register("rnc", registerValidation.rnc)}
        className={`w-full px-4 py-3 border rounded-xl focus:ring-2 focus:ring-orange-500 focus:outline-none ${
        errors.rnc
        ? "border-red-500"
        : "border-slate-300"}`}/>

       {errors.rnc && (
       <p className="text-red-500 text-sm mt-1">
       {errors.rnc.message}
       </p>)}</div>

        {/* Hora Apertura */}

        <div>
        <label className="block mb-2 font-medium">
          Hora Apertura
       </label> 

      <input
        type="time"
        {...register("openingTime", registerValidation.openingTime)}
        className={`w-full px-4 py-3 border rounded-xl focus:ring-2 focus:ring-orange-500 focus:outline-none ${
        errors.openingTime
        ? "border-red-500"
        : "border-slate-300"
        }`}/>

       {errors.openingTime && (
          <p className="text-red-500 text-sm mt-1">
       {errors.openingTime.message}</p>)}
      </div>
        {/* Hora Cierre */}

        <div>
        <label className="block mb-2 font-medium">
          Hora Cierre
         </label>

      <input
       type="time"
       {...register("closingTime", registerValidation.closingTime)}
       className={`w-full px-4 py-3 border rounded-xl focus:ring-2 focus:ring-orange-500 focus:outline-none ${
       errors.closingTime
       ? "border-red-500"
       : "border-slate-300"}`}/>

       {errors.closingTime && (
       <p className="text-red-500 text-sm mt-1">
       {errors.closingTime.message}
      </p> )}
      </div>
      </div>

      {/* Dirección */}

       <div className="mt-6">

      <label className="block mb-2 font-medium">
          Dirección
      </label>

    <textarea
     rows="4"
     placeholder="Ingrese la dirección completa del restaurante"
     {...register("address", registerValidation.address)}
     className={`w-full px-4 py-3 border rounded-xl resize-none focus:ring-2 focus:ring-orange-500 focus:outline-none ${
      errors.address
        ? "border-red-500"
        : "border-slate-300" }`}/>

    {errors.address && (
     <p className="text-red-500 text-sm mt-1">
       {errors.address.message}
     </p>
   )}
</div>
    </div>
  </div>
        )}

        {/* Paso 3 */}

        {step === 3 && (
           <div>

    {/* Header */}

    <div className="mb-8 bg-gradient-to-r from-emerald-500 to-green-600 rounded-2xl p-6 text-white shadow-lg">
      <h2 className="text-3xl font-bold">
        Plan y Método de Pago
      </h2>

      <p className="mt-2 text-green-100">
        Seleccione el plan ideal para su restaurante.
      </p>
    </div>

    {/* Planes */}
    <h3 className="text-xl font-bold text-slate-800 mb-4">
  Seleccione un Plan
</h3>

{loadingPlans ? (
  <div className="text-center py-10">
    Cargando planes...
  </div>
) : (
  <div className="grid grid-cols-1 md:grid-cols-2 gap-6 mb-8">
    {plans.map((plan) => (
      <div
        key={plan.id}
        onClick={() => {
          setSelectedPlanId(plan.id);
          setApiError("");
        }}
        className={`
          cursor-pointer rounded-2xl p-6 transition border-2 shadow-md
          bg-gradient-to-br
          ${
            selectedPlanId === plan.id
              ? "from-violet-600/10 to-purple-600/10 border-violet-600"
              : "from-slate-50 to-white border-slate-200 hover:border-violet-500"
          }
        `}
      >
        <div className="flex justify-between items-center mb-4">
          <h4 className="text-xl font-bold">
            {plan.nombre}
          </h4>

          <span
            className={`
              px-3 py-1 rounded-full text-sm font-medium
              ${
                selectedPlanId === plan.id
                  ? "bg-violet-600 text-white"
                  : "bg-slate-100 text-slate-700"
              }
            `}
          >
            {selectedPlanId === plan.id
              ? "Seleccionado"
              : "Seleccionar"}
          </span>
        </div>

        <div className="text-4xl font-bold text-violet-600 mb-4">
          RD$ {Number(plan.precioMensual).toLocaleString()}
        </div>

        <p className="text-slate-600 leading-6">
          {plan.descripcion}
        </p>
      </div>
    ))}
  </div>
)}   
    
    {/* Términos */}
<div className="mb-8 bg-slate-50 border border-slate-200 rounded-xl p-4">

  <label className="flex items-center gap-3 cursor-pointer">

    <input
      type="checkbox"
      {...register(
        "termsAccepted",
        registerValidation.termsAccepted
      )}
      className="w-5 h-5"
    />

    <span className="text-slate-700">
      Acepto los términos y condiciones de Elysia.
    </span>

  </label>

  {errors.termsAccepted && (
    <p className="text-red-500 text-sm mt-2">
      {errors.termsAccepted.message}
    </p>
  )}
</div>

     

    {/* Tarjeta */}

    <div className="bg-white border border-slate-200 rounded-2xl p-8 shadow-sm">

      <h3 className="text-xl font-bold text-slate-800 mb-6">
        Información de Pago
      </h3>

      <div className="grid grid-cols-1 md:grid-cols-2 gap-6">

        <div className="md:col-span-2">

        <label className="block mb-2 font-medium">
          Nombre del Titular
        </label>

        <input
          type="text"
          placeholder="Kelvin Díaz"
          {...register("cardholderName", registerValidation.cardholderName)}
          className={`w-full px-4 py-3 border rounded-xl focus:ring-2 focus:ring-green-500 focus:outline-none ${
           errors.cardholderName
           ? "border-red-500"
           : "border-slate-300"}`}/>

          {errors.cardholderName && (
          <p className="text-red-500 text-sm mt-1">
          {errors.cardholderName.message}</p>)}
       </div>

        <div className="md:col-span-2">
          <label className="block mb-2 font-medium">
            Número de Tarjeta
          </label>

        <input
           type="text"
            placeholder="1234567890123456"
            {...register("cardNumber", registerValidation.cardNumber)}
            className={`w-full px-4 py-3 border rounded-xl focus:ring-2 focus:ring-green-500 focus:outline-none ${
            errors.cardNumber
            ? "border-red-500"
            : "border-slate-300"}`}/>

        {errors.cardNumber && (
        <p className="text-red-500 text-sm mt-1">
        {errors.cardNumber.message}</p>)} 
        </div>

        <div>
          <label className="block mb-2 font-medium">
            Tipo de Tarjeta
          </label>

          <select

    {...register("cardType", registerValidation.cardType)}

    className={`w-full px-4 py-3 border rounded-xl focus:ring-2 focus:ring-green-500 focus:outline-none ${
        errors.cardType
            ? "border-red-500"
            : "border-slate-300"
    }`}

>

    <option value="">Seleccione</option>
    <option value="Visa">Visa</option>
    <option value="MasterCard">MasterCard</option>
    <option value="American Express">
        American Express
    </option>

</select>

{errors.cardType && (

<p className="text-red-500 text-sm mt-1">

    {errors.cardType.message}

</p>

)} 
        </div>

        <div>
          <label className="block mb-2 font-medium">
            CVV
          </label>

          <input
    type="text"
    placeholder="123"
    {...register("cvv", registerValidation.cvv)}
    className={`w-full px-4 py-3 border rounded-xl focus:ring-2 focus:ring-green-500 focus:outline-none ${
        errors.cvv
            ? "border-red-500"
            : "border-slate-300"
    }`}
/>

{errors.cvv && (

<p className="text-red-500 text-sm mt-1">

    {errors.cvv.message}

</p>

)}
        </div>

        <div>
          <label className="block mb-2 font-medium">
            Mes Vencimiento
          </label>

           <input
    type="text"
    placeholder="01"
    {...register("expiryMonth", registerValidation.expiryMonth)}
    className={`w-full px-4 py-3 border rounded-xl focus:ring-2 focus:ring-green-500 focus:outline-none ${
        errors.expiryMonth
            ? "border-red-500"
            : "border-slate-300"
    }`}
/>

{errors.expiryMonth && (

<p className="text-red-500 text-sm mt-1">

    {errors.expiryMonth.message}

</p>

)}
        </div>

        <div>
          <label className="block mb-2 font-medium">
            Año Vencimiento
          </label>

          <input
    type="text"
    placeholder="2028"
    {...register("expiryYear", registerValidation.expiryYear)}
    className={`w-full px-4 py-3 border rounded-xl focus:ring-2 focus:ring-green-500 focus:outline-none ${
        errors.expiryYear
            ? "border-red-500"
            : "border-slate-300"
    }`}
/>

{errors.expiryYear && (

<p className="text-red-500 text-sm mt-1">

    {errors.expiryYear.message}

</p>

)}
        </div>

      </div>

    </div>

  </div>
        )}

        {/* Botones */}

        <div className="flex justify-between mt-10">

          {step > 1 ? (
            <button
              onClick={() => setStep(step - 1)}
              className="px-6 py-3 border border-slate-300 rounded-lg"
            >
              Anterior
            </button>
          ) : (
            <Link
              to="/"
              className="px-6 py-3 border border-slate-300 rounded-lg"
            >
              Volver
            </Link>
          )}

          {step < 3 && (
            <button
              onClick={handleNext}
              type="button"
               className="px-6 py-3 bg-violet-600 text-white rounded-lg hover:bg-violet-700"
             >
              Siguiente
            </button>
          )}

          {step === 3 && (
            <button
            onClick={handleSubmit(onSubmit)}
            type="button"
            className="px-6 py-3 bg-green-600 text-white rounded-lg"
           >
           Finalizar Registro
           </button>
          )}
        </div>
      </div>
    </div>
  );
}


using Elysia.Core.Application.Dtos.Email;
using Elysia.Core.Application.Dtos.User;
using Elysia.Core.Application.Interfaces;
using Elysia.Core.Domain.Settings;
using Elysia.Infraestructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Elysia.Infraestructure.Identity.Services
{
    public class AccountServices : IAccountServices
    {



        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly IEmailServices emailService;
        private readonly JwtSettings _jwtSettings;




        public AccountServices(UserManager<AppUser> _userManager, SignInManager<AppUser> _signInManager, IEmailServices _emailService, IOptions<JwtSettings> JwtSettings)
        {
            emailService = _emailService;
            userManager = _userManager;
            signInManager = _signInManager;
            _jwtSettings = JwtSettings.Value;

        }



        public async Task<LoginResponseDto> Authenticate(LoginDto dto)
        {

            var responseDto = new LoginResponseDto() { Name = "", LastName = "", Errors = [], Token = "", Rol = "" };



            if (string.IsNullOrWhiteSpace(dto.Password))
            {

                responseDto.HasError = true;
                responseDto.Errors!.Add($"You should put the password");
                return responseDto;
            }




            if (string.IsNullOrWhiteSpace(dto.Email))
            {

                responseDto.HasError = true;
                responseDto.Errors!.Add($"You should put the Email");
                return responseDto;
            }



            var user = await userManager.FindByEmailAsync(dto.Email);

            if (user == null)
            {
                responseDto.HasError = true;
                responseDto.Errors!.Add($"There is not account registered with this Email: {dto.Email}");
                return responseDto;

            }





            if (!user!.EmailConfirmed || !user.IsActive)
            {
                responseDto.HasError = true;
                responseDto.Errors!.Add($"This account {dto.Email} is not active, you shoul check your email");
                return responseDto;

            }



            var result = await signInManager.PasswordSignInAsync(user.UserName ?? "", dto.Password, false, true);


            if (!result.Succeeded)
            {

                responseDto.HasError = true;
                responseDto.Errors!.Add($"These credentials are invalid for this user: {user.Email}");
                return responseDto;

            }


            JwtSecurityToken jwtSecurityToken = await GenerateJwtToken(user);



            var rolesList = await userManager.GetRolesAsync(user);


            responseDto.Name = user.Name;
            responseDto.LastName = user.LastName;
            responseDto.Rol = rolesList.FirstOrDefault()!;
            responseDto.UsuarioId = user.Id;
            responseDto.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);


            return responseDto;

        }





        public async Task<RegisterResponseDto?> RegisterUser(SaveUserRequestDto? saveUser)
        {

            var response = new RegisterResponseDto() { Roles = [], Name = "", Email = "", Id = "", LastName = "", UserName = "", Errors = [], Message = "" };


            if (saveUser == null)
            {
                return null;

            }


            if (string.IsNullOrWhiteSpace(saveUser.Email)
             || string.IsNullOrWhiteSpace(saveUser.Password)
             || string.IsNullOrWhiteSpace(saveUser.UserName)
             || string.IsNullOrWhiteSpace(saveUser.LastName)
             || string.IsNullOrWhiteSpace(saveUser.Name)
             || string.IsNullOrWhiteSpace(saveUser.Phone)
             || string.IsNullOrWhiteSpace(saveUser.NombreRestaurante)
             || string.IsNullOrWhiteSpace(saveUser.IdCard)
             || string.IsNullOrWhiteSpace(saveUser.Role)
             || string.IsNullOrWhiteSpace(saveUser.Especialidad)
             || string.IsNullOrWhiteSpace(saveUser.PhoneRestaurante)
             || string.IsNullOrWhiteSpace(saveUser.HoraApertura.ToString())
             || string.IsNullOrWhiteSpace(saveUser.HoraCierre.ToString())
             || string.IsNullOrWhiteSpace(saveUser.DireccionRestaurante)
             || string.IsNullOrWhiteSpace(saveUser.RNC))
            {
                return null;
            }


            var userWithSomeUserName = await userManager.FindByNameAsync(saveUser.UserName);
            if (userWithSomeUserName != null)
            {
                response.HasError = true;
                response.Errors!.Add($"this userName: {saveUser.UserName} is already taken.");
                return response;
            }

            var userWithSomeEmail = await userManager.FindByEmailAsync(saveUser.Email);
            if (userWithSomeEmail != null)
            {
                response.HasError = true;
                response.Errors!.Add($"This email: {saveUser.Email} is already taken.");
                return response;

            }



            if (saveUser.Phone.Trim().Length != 10 || saveUser.PhoneRestaurante.Trim().Length != 10)
            {
                response.HasError = true;
                response.Errors.Add("Los numeros de telefonos tienen que tener exactamente 10 digitos sin guiones");
                return response;

            }




            if (saveUser.IdCard.Trim().Length != 11)
            {
                response.HasError = true;
                response.Errors.Add("La cedula o idcard debe tener exactamente 11 digitos sin guiones");
                return response;

            }



            if (saveUser.RNC.Trim().Length != 9)
            {
                response.HasError = true;
                response.Errors.Add("El RNC debe tener exactamente 9 digitos sin guiones");
                return response;

            }


            var validateInformationUser = await ValidateInformationUserExist(saveUser.Phone, saveUser.IdCard);
            if (validateInformationUser != null && validateInformationUser!.Count > 0)
            {
                response.HasError = true;
                response.Errors.AddRange(validateInformationUser);
                return response;
            }




            var validateInformationRestaurant = await ValidateInformationRestauranteExist(saveUser.PhoneRestaurante, saveUser.NombreRestaurante, saveUser.RNC, saveUser.DireccionRestaurante);
            if (validateInformationRestaurant != null && validateInformationRestaurant.Count > 0)
            {
                response.HasError = true;
                response.Errors.AddRange(validateInformationRestaurant);
                return response;
            }



            AppUser User = new AppUser()
            {

                Name = saveUser.Name,
                LastName = saveUser.LastName,
                Email = saveUser.Email,
                PhoneNumber = saveUser.Phone,
                UserName = saveUser.UserName,
                EmailConfirmed = false,
                IsActive = false,
                LogoRestaurante = saveUser.LogoRestaurante ?? "",
                NombreRestaurante = saveUser.NombreRestaurante,
                DireccionRestaurante = saveUser.DireccionRestaurante,
                HoraApertura = saveUser.HoraApertura,
                HoraCierre = saveUser.HoraCierre,
                IdCard = saveUser.IdCard,
                PhoneRestaurante = saveUser.PhoneRestaurante,
                PhoneNumberConfirmed = true,
                ProfileImage = saveUser.ProfileImage ?? "",
                RNC = saveUser.RNC ?? "",
                Especialidad = saveUser.Especialidad

            };



            var result = await userManager.CreateAsync(User, saveUser.Password);

            if (result.Succeeded)
            {



                await userManager.AddToRoleAsync(User, saveUser.Role);
                string token = await GetVerificationEmailToken(User);

                var confirmLink =
                   $"http://localhost:5173/confirm-account?userId={User.Id}&token={token}";


                var templatePath = Path.Combine(
                       AppContext.BaseDirectory, "wwwroot",
                      "Template",
                      "ConfirmAccountPage.html");


                var html = await File.ReadAllTextAsync(templatePath);

                html = html.Replace("{{USERNAME}}", saveUser.Name);
                html = html.Replace("{{CONFIRM_LINK}}", confirmLink);


                await emailService.SendAsync(new EmailDto()
                {
                    To = saveUser.Email,
                    Subject = "Confirmacion de cuenta",
                    HtmlBody = html

                });



                var CurrentrolesList = await userManager.GetRolesAsync(User);

                response.Id = User.Id;
                response.Name = User.Name;
                response.UserName = User.UserName ?? "";
                response.LastName = User.LastName;
                response.Email = User.Email ?? "";
                response.IsVerified = User.EmailConfirmed;
                response.Roles = CurrentrolesList.ToList();
                response.Message = "Please Check your email, for verification your account";

            }
            else
            {

                response.HasError = true;
                response.Errors!.AddRange(result.Errors.Select(s => s.Description).ToList());
                return response;

            }


            return response;
        }





        public async Task<EditResponseDto?> EditUser(SaveUserRequestDto? saveUser, bool? IsCreated = false)
        {


            bool IsNotCreated = IsCreated ?? false;
            var response = new EditResponseDto() { Name = "", Email = "", Id = "", LastName = "", UserName = "", Errors = [] };



            if (saveUser == null)
            {
                return null;

            }


            var userWithSomeUserName = await userManager.Users.FirstOrDefaultAsync(u => u.UserName == saveUser.UserName && u.Id != saveUser.Id);
            if (userWithSomeUserName != null)
            {
                response.HasError = true;
                response.Errors!.Add($"this userName: {saveUser.UserName} is already taken.");
                return response;
            }

            var userWithSomeEmail = await userManager.Users.FirstOrDefaultAsync(u => u.Email == saveUser.Email && u.Id != saveUser.Id);
            if (userWithSomeEmail != null)
            {
                response.HasError = true;
                response.Errors!.Add($"This email: {saveUser.Email} is already taken.");
                return response;

            }


            var user = await userManager.FindByIdAsync(saveUser.Id);
            if (user == null)
            {
                response.HasError = true;
                response.Errors!.Add("There is not account registered with this user");
                return response;
            }


            if (saveUser.Phone!.Trim().Length != 10 || saveUser.PhoneRestaurante.Trim().Length != 10)
            {
                response.HasError = true;
                response.Errors.Add("Los numeros de telefonos tienen que tener exactamente 10 digitos sin guiones");
                return response;

            }




            if (saveUser.IdCard.Trim().Length != 11)
            {
                response.HasError = true;
                response.Errors.Add("La cedula o idcard debe tener exactamente 11 digitos sin guiones");
                return response;

            }



            if (saveUser.RNC.Trim().Length != 9)
            {
                response.HasError = true;
                response.Errors.Add("El RNC debe tener exactamente 9 digitos sin guiones");
                return response;

            }


            var validateInformationUser = await ValidateInformationUserExist(saveUser.Phone, saveUser.IdCard, true, user.Id);
            if (validateInformationUser != null && validateInformationUser!.Count > 0)
            {
                response.HasError = true;
                response.Errors.AddRange(validateInformationUser);
                return response;
            }




            var validateRestaurantInformation = await ValidateInformationRestauranteExist(saveUser.PhoneRestaurante, saveUser.NombreRestaurante, saveUser.RNC, saveUser.DireccionRestaurante, true, user.Id);
            if (validateRestaurantInformation != null && validateRestaurantInformation!.Count > 0)
            {
                response.HasError = true;
                response.Errors.AddRange(validateRestaurantInformation);
                return response;
            }



            user.Name = !string.IsNullOrWhiteSpace(saveUser.Name) ? saveUser.Name : user.Name;
            user.LastName = !string.IsNullOrWhiteSpace(saveUser.LastName) ? saveUser.LastName : user.LastName;
            user.PhoneNumber = !string.IsNullOrWhiteSpace(saveUser.Phone) ? saveUser.Phone : user.PhoneNumber;
            user.UserName = !string.IsNullOrWhiteSpace(saveUser.UserName) ? saveUser.UserName : user.UserName;

            user.ProfileImage = !string.IsNullOrWhiteSpace(saveUser.ProfileImage)
                ? saveUser.ProfileImage
                : user.ProfileImage;

            user.DireccionRestaurante = !string.IsNullOrWhiteSpace(saveUser.DireccionRestaurante)
                ? saveUser.DireccionRestaurante
                : user.DireccionRestaurante;

            user.PhoneRestaurante = !string.IsNullOrWhiteSpace(saveUser.PhoneRestaurante)
                ? saveUser.PhoneRestaurante
                : user.PhoneRestaurante;

            user.LogoRestaurante = !string.IsNullOrWhiteSpace(saveUser.LogoRestaurante)
                ? saveUser.LogoRestaurante
                : user.LogoRestaurante;

            user.NombreRestaurante = !string.IsNullOrWhiteSpace(saveUser.NombreRestaurante)
                ? saveUser.NombreRestaurante
                : user.NombreRestaurante;

            user.IdCard = !string.IsNullOrWhiteSpace(saveUser.IdCard)
                ? saveUser.IdCard
                : user.IdCard;

            user.RNC = !string.IsNullOrWhiteSpace(saveUser.RNC)
                ? saveUser.RNC
                : user.RNC;

         
            user.HoraApertura = !string.IsNullOrEmpty(saveUser.HoraApertura.ToString())
                ? saveUser.HoraApertura
                : user.HoraApertura;

            user.HoraCierre = !string.IsNullOrEmpty(saveUser.HoraCierre.ToString())
                ? saveUser.HoraCierre
                : user.HoraCierre;

            if (!IsCreated!.Value)
            {
                user.EmailConfirmed = user.Email == saveUser.Email;
            }

            user.Email = !string.IsNullOrWhiteSpace(saveUser.Email)
                ? saveUser.Email
                : user.Email;





            var result = await userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                var rolesList = await userManager.GetRolesAsync(user);
                await userManager.RemoveFromRolesAsync(user, rolesList);
                await userManager.AddToRoleAsync(user, saveUser.Role);

                if (!user.EmailConfirmed && !IsNotCreated)
                {

                    user.IsActive = false;
                    string token = await GetVerificationEmailToken(user);

                    var confirmLink =
                       $"http://localhost:5173/confirm-account?userId={user.Id}&token={token}";


                    var templatePath = Path.Combine(
                           AppContext.BaseDirectory, "wwwroot",
                          "EmailTemplates",
                          "ConfirmYourAccount.html");


                    var html = await File.ReadAllTextAsync(templatePath);

                    html = html.Replace("{{USERNAME}}", saveUser.Name);
                    html = html.Replace("{{CONFIRM_LINK}}", confirmLink);


                    await emailService.SendAsync(new EmailDto()
                    {
                        To = saveUser.Email,
                        Subject = "Confirmacion de cuenta",
                        HtmlBody = html

                    });

                }



                if (!string.IsNullOrWhiteSpace(saveUser.Password) && !IsNotCreated)
                {

                    var token = await userManager.GeneratePasswordResetTokenAsync(user);
                    var resultChange = await userManager.ResetPasswordAsync(user, token, saveUser.Password);

                    if (resultChange != null && !resultChange.Succeeded)
                    {

                        response.HasError = true;
                        response.Errors.AddRange(resultChange.Errors.Select(s => s.Description).ToList());
                        return response;

                    }

                }


                var CurrentrolesList = await userManager.GetRolesAsync(user);

                response.Id = user.Id;
                response.Name = user.Name;
                response.UserName = user.UserName ?? "";
                response.LastName = user.LastName;
                response.Email = user.Email ?? "";
                response.IsActive = user.EmailConfirmed;
                response.Roles = CurrentrolesList.ToList();

                return response;
            }
            else
            {

                response.HasError = true;
                response.Errors!.AddRange(result.Errors.Select(s => s.Description).ToList());
                return response;

            }


        }




        public async Task<UserResponseDto> DeleteAsync(string id)
        {
            var response = new UserResponseDto() { Errors = [], HasError = false };

            if (string.IsNullOrEmpty(id))
            {
                response.HasError = true;
                response.Errors.Add("You should put the user id");
                return response;


            }


            var user = await userManager.FindByIdAsync(id);


            if (user == null)
            {

                response.HasError = true;
                response.Errors!.Add($"There is not Account registered with this user");
                return response;

            }


            await userManager.DeleteAsync(user!);

            return response;
        }


        //hay que hacer un metodo para habilitar o inhabilitar cuentas de usuarios

        //activar usuario
        public async Task<UserResponseDto> ActivarUser(string usuarioId)
        {
            var response = new UserResponseDto() { Errors = [], Message = "", HasError = false };
            var user = await userManager.FindByIdAsync(usuarioId);

            if (user != null)
            {

                user.IsActive = true;
                user.EmailConfirmed = true;
                response.HasError = false;
                response.Message = "Usuario activado correctamente";
                return response;
            }

            response.HasError = true;
            response.Errors.Add("Ucurrio un error al intentar activar el usuario, no se pudo encontrar el usuario indicado");
            return response;

        }



        //desativar usuario
        public async Task<UserResponseDto> InhativarUser(string usuarioId)
        {
            var response = new UserResponseDto() { Errors = [], Message = "", HasError = false };
            var user = await userManager.FindByIdAsync(usuarioId);

            if (user != null)
            {

                user.IsActive = false;
                response.HasError = false;
                response.Message = "Usuario desativado correctamente";
                return response;
            }

            response.HasError = true;
            response.Errors.Add("Ucurrio un error al intentar desativar el usuario, no se pudo encontrar el usuario indicado");
            return response;

        }


        public async Task<List<UserDto>> GetAllUser(bool? IsActive = true)
        {

            var users = userManager.Users;


            if (users == null)
            {
                return new List<UserDto>();
            }


            List<UserDto> ListUserDto = [];
            if (IsActive!.Value && IsActive != null)
            {
                users = users.Where(u => u.EmailConfirmed);

            }
            else
            {
                users = users.Where(u => !u.EmailConfirmed);
            }


            var ListUser = await users.ToListAsync();

            foreach (var item in ListUser)
            {

                var rolesList = await userManager.GetRolesAsync(item);


                ListUserDto.Add(new UserDto()
                {

                    Id = item.Id,
                    Name = item.Name,
                    UserName = item.UserName ?? "",
                    LastName = item.LastName,
                    Email = item.Email ?? "",
                    IsActive = item.EmailConfirmed,
                    NombreRestaurante = item.NombreRestaurante,
                    RNC = item.RNC,
                    LogoRestaurante = item.LogoRestaurante,
                    DireccionRestaurante = item.DireccionRestaurante,
                    HoraApertura = item.HoraApertura.Value,
                    HoraCierre = item.HoraCierre.Value,
                    PhoneRestaurante = item.PhoneRestaurante,
                    IdCard = item.IdCard,
                    Phone = item.PhoneNumber ?? "",
                    ProfileImage = item.ProfileImage,
                    Role = rolesList.FirstOrDefault() ?? ""

                });



            }

            return ListUserDto;


        }



        public async Task<UserDto?> GetUserByEmail(string gmail)
        {

            var user = await userManager.FindByEmailAsync(gmail);


            if (user == null)
            {
                return null;

            }


            var rolesList = await userManager.GetRolesAsync(user);

            var userDto = new UserDto()
            {

                Id = user.Id,
                Name = user.Name,
                UserName = user.UserName ?? "",
                LastName = user.LastName,
                Email = user.Email ?? "",
                IsActive = user.EmailConfirmed,
                NombreRestaurante = user.NombreRestaurante,
                RNC = user.RNC,
                LogoRestaurante = user.LogoRestaurante,
                DireccionRestaurante = user.DireccionRestaurante,
                HoraApertura = user.HoraApertura.Value,
                HoraCierre = user.HoraCierre.Value,
                PhoneRestaurante = user.PhoneRestaurante,
                IdCard = user.IdCard,
                Phone = user.PhoneNumber ?? "",
                ProfileImage = user.ProfileImage,
                Role = rolesList.FirstOrDefault() ?? ""

            };


            return userDto;
        }



        public async Task<UserDto?> GetUserById(string id)
        {

            var user = await userManager.FindByIdAsync(id);


            if (user == null)
            {
                return null;

            }


            var rolesList = await userManager.GetRolesAsync(user);

            var userDto = new UserDto()
            {

                Id = user.Id,
                Name = user.Name,
                UserName = user.UserName ?? "",
                LastName = user.LastName,
                Email = user.Email ?? "",
                IsActive = user.EmailConfirmed,
                NombreRestaurante = user.NombreRestaurante,
                RNC = user.RNC,
                LogoRestaurante = user.LogoRestaurante,
                DireccionRestaurante = user.DireccionRestaurante,
                HoraApertura = user.HoraApertura.Value,
                HoraCierre = user.HoraCierre.Value,
                PhoneRestaurante = user.PhoneRestaurante,
                IdCard = user.IdCard,
                Phone = user.PhoneNumber ?? "",
                ProfileImage = user.ProfileImage,
                Role = rolesList.FirstOrDefault() ?? ""


            };


            return userDto;
        }




        public async Task<UserDto?> GetUserByUserName(string userName)
        {

            var user = await userManager.FindByNameAsync(userName);


            if (user == null)
            {
                return null;

            }

            var rolesList = await userManager.GetRolesAsync(user);

            var userDto = new UserDto()
            {
                Id = user.Id,
                Name = user.Name,
                UserName = user.UserName ?? "",
                LastName = user.LastName,
                Email = user.Email ?? "",
                IsActive = user.EmailConfirmed,
                NombreRestaurante = user.NombreRestaurante,
                RNC = user.RNC,
                LogoRestaurante = user.LogoRestaurante,
                DireccionRestaurante = user.DireccionRestaurante,
                HoraApertura = user.HoraApertura.Value,
                HoraCierre = user.HoraCierre.Value,
                PhoneRestaurante = user.PhoneRestaurante,
                IdCard = user.IdCard,
                Phone = user.PhoneNumber ?? "",
                ProfileImage = user.ProfileImage,
                Role = rolesList.FirstOrDefault() ?? ""

            };


            return userDto;
        }



        public async Task<UserResponseDto?> RessetPassowrd(RessetPasswordRequestDto? request)
        {

            var response = new UserResponseDto() { Errors = [], HasError = false };


            if (request == null)
            {
                return null;
            }


            if (string.IsNullOrWhiteSpace(request.Id)
               || string.IsNullOrWhiteSpace(request.Token)
               || string.IsNullOrWhiteSpace(request.Password))
            {
                return null;
            }




            var user = await userManager.FindByIdAsync(request.Id);

            if (user == null)
            {
                response.HasError = true;
                response.Errors!.Add($"there is not account registered with this user");
                return response;

            }


            var token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Token));
            var result = await userManager.ResetPasswordAsync(user, token, request.Password);
            if (!result.Succeeded)
            {
                response.HasError = true;
                response.Errors!.Add($"An error ocurred while resset password");
                return response;

            }


            user.EmailConfirmed = true;
            user.IsActive = true;
            await userManager.UpdateAsync(user);

            return response;
        }




        public async Task<UserResponseDto> ForgotPasswordAsync(ForgotPasswordRequestDto request)
        {

            var response = new UserResponseDto() { Errors = [], HasError = false };


            if (string.IsNullOrEmpty(request.Email))
            {
                response.HasError = true;
                response.Errors.Add("You should put de userName");
                return response;

            }


            var user = await userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                response.HasError = true;
                response.Errors!.Add($"there is not account registered with this Email {request.Email}");
                return response;

            }



            var ressetToken = await GetRessetPassworToken(user);
            user.EmailConfirmed = false;
            user.IsActive = false;


            var result = await userManager.UpdateAsync(user);


            var encodeToken = Uri.EscapeDataString(ressetToken);
            var confirmLink =
               $"http://localhost:5173/reset-password?userId={user.Id}&token={encodeToken}";


            var templatePath = Path.Combine(
                   AppContext.BaseDirectory, "wwwroot",
                  "Template",
                  "ChangePassword.html");


            var html = await File.ReadAllTextAsync(templatePath);

            html = html.Replace("{{USERNAME}}", request.Email);
            html = html.Replace("{{CONFIRM_LINK}}", confirmLink);


            await emailService.SendAsync(new EmailDto()
            {
                To = user.Email!,
                Subject = "Resetear la contraseña",
                HtmlBody = html

            });

            if (result.Succeeded)
            {
                response.Message = "Please Check your email, for reseet your password";
            }

            return response;
        }








        public async Task<ConfirmResponseDto?> confirmAccounAsync(ConfirmRequestDto? dto)
        {
            var response = new ConfirmResponseDto() { HasError = false, Message = "" };

            if (dto == null)
            {
                return new ConfirmResponseDto
                {
                    HasError = true,
                    Message = "Invalid confirmation request."
                };
            }


            if (string.IsNullOrEmpty(dto.UserId)
                || string.IsNullOrEmpty(dto.Token)
               )
            {

                return new ConfirmResponseDto
                {
                    HasError = true,
                    Message = "Invalid confirmation request."
                };

            }



            var user = await userManager.FindByIdAsync(dto.UserId);

            if (user == null)
            {

                response.HasError = true;
                response.Message = "There is not account registered with this user";
                return response;

            }


            dto.Token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(dto.Token));
            var result = await userManager.ConfirmEmailAsync(user, dto.Token);
            if (result.Succeeded)
            {
                response.HasError = false;
                response.Message = $"Account confirmed for {user.Email}. you can now use the app";
                user.IsActive = true;
                await userManager.UpdateAsync(user);
                return response;

            }
            else
            {

                response.HasError = true;
                response.Message = $"An error ocurred when while confirming this email {user.Email}, Please verificate your token";
                return response;

            }


        }











        #region private Method
        private async Task<List<string>> ValidateInformationRestauranteExist(string telefono, string nombre, string rnc, string direccion, bool? isEditMode = false, string? usuarioId = null)
        {
            var ListMessage = new List<string>();
            var restaurantes = await userManager.Users.ToListAsync();


            if (isEditMode!.Value)
            {
                foreach (var user in restaurantes)
                {
                    if (user.Id != usuarioId)
                    {

                        if (user.PhoneRestaurante == telefono) ListMessage.Add("El telefono del restaurante ya esta registrado, favor verificar");

                        if (user.NombreRestaurante == nombre) ListMessage.Add("El nombre del restaurante ya esta registrado, favor verificar");

                        if (user.RNC == rnc) ListMessage.Add("El RNC del restaurante ya esta registrado, favor verificar");


                        if (user.DireccionRestaurante == direccion) ListMessage.Add("La direccion del restaurante ya esta registrada, favor verificar");
                    }

                }


                return ListMessage;
            }
            {

                foreach (var user in restaurantes)
                {
                    if (user.PhoneRestaurante == telefono) ListMessage.Add("El telefono del restaurante ya esta registrado, favor verificar");

                    if (user.NombreRestaurante == nombre) ListMessage.Add("El nombre del restaurante ya esta registrado, favor verificar");

                    if (user.RNC == rnc) ListMessage.Add("El RNC del restaurante ya esta registrado, favor verificar");


                    if (user.DireccionRestaurante == direccion) ListMessage.Add("La direccion del restaurante ya esta registrada, favor verificar");

                }



                return ListMessage;
            }

        }




        private async Task<List<string>> ValidateInformationUserExist(string telefono, string idCard, bool? isEditMode = false, string? usuarioId = null)
        {
            var ListMessage = new List<string>();
            var restaurantes = await userManager.Users.ToListAsync();


            if (isEditMode!.Value)
            {
                foreach (var user in restaurantes)
                {
                    if (user.Id != usuarioId)
                    {

                        if (user.PhoneNumber == telefono) ListMessage.Add("El telefono del usuario ya esta registrado, favor verificar");

                        if (user.IdCard == idCard) ListMessage.Add("La cedula del usuario ya esta registrado, favor verificar");

                    }
                }


                return ListMessage;
            }
            {

                foreach (var user in restaurantes)
                {
                    if (user.PhoneNumber == telefono) ListMessage.Add("El telefono del usuario ya esta registrado, favor verificar");

                    if (user.IdCard == idCard) ListMessage.Add("la cedula del usuario  ya esta registrado, favor verificar");

                }


                return ListMessage;
            }

        }





        private async Task<string> GetVerificationEmailToken(AppUser user)
        {

            string Token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            Token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(Token));

            return Token;

        }


        private async Task<string> GetRessetPassworToken(AppUser user)
        {

            string Token = await userManager.GeneratePasswordResetTokenAsync(user);
            Token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(Token));
            return Token;

        }



        public async Task<JwtSecurityToken> GenerateJwtToken(AppUser user)
        {


            var userClaims = await userManager.GetClaimsAsync(user);
            var roles = await userManager.GetRolesAsync(user);



            var rolesClaims = new List<Claim>();

            foreach (var role in roles)
            {
                rolesClaims.Add(new Claim(ClaimTypes.Role, role));
            }


            var claims = new[]
            {

                new Claim(JwtRegisteredClaimNames.Sub,user.UserName ?? ""),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email,user.Email ?? ""),
                new Claim("UId",user.Id),

            }.Union(userClaims).Union(rolesClaims);


            var symmectriSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var signinCredentials = new SigningCredentials(symmectriSecurityKey, SecurityAlgorithms.HmacSha256);


            var JwtSecuritytoken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signinCredentials);


            return JwtSecuritytoken;

        }
        #endregion













    }
}

using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using wca.compras.domain.Dtos;
using wca.compras.domain.Email;
using wca.compras.domain.Entities;
using wca.compras.domain.Interfaces;
using wca.compras.domain.Interfaces.Repositories;
using wca.compras.domain.Interfaces.Services;
using wca.compras.domain.Security;
using BC = BCrypt.Net.BCrypt;

namespace wca.compras.services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IRepository<Usuario> _usuarioRepository;
        private readonly IRepository<ResetPassword> _resetPasswordRepository;
        private readonly IPerfilRepository _perfilRepository;
        private readonly IEmailService _emailService;
        private readonly SigningConfiguration _signingConfiguration;
        private readonly TokenConfiguration _tokenConfiguration;
        private readonly IMapper _mapper;

        public AuthenticationService(IRepository<Usuario> repository,
                                     IRepository<ResetPassword> resetPasswordRepository,
                                     IPerfilRepository perfilRepository,
                                     IEmailService emailService,
                                     SigningConfiguration signingConfiguration,
                                     TokenConfiguration tokenConfiguration,
                                     IMapper mapper)
        {
            _usuarioRepository = repository;
            _resetPasswordRepository = resetPasswordRepository;
            _perfilRepository = perfilRepository;
            _emailService = emailService;
            _signingConfiguration = signingConfiguration;
            _tokenConfiguration = tokenConfiguration;
            _mapper = mapper;
        }

        

        public async Task<LoginResponse> Authenticate(LoginRequest login)
        {
            try
            {
                var authUser = await _usuarioRepository.GetAsync(u => u.Email == login.Email && u.Ativo == true);
                if (authUser == null || !BC.Verify(login.Password, authUser.Password))
                {
                    return new LoginResponse(false, "Falha na autenticação!", "", "", "", "", "", null);
                }

                var perfilUser = await _perfilRepository.GetWithPermissoesByIdAsync(authUser.PerfilId);
                if (perfilUser == null || perfilUser.Ativo == false)
                {
                    return new LoginResponse(false, "Falha na autenticação!", "", "", "", "", "", null);
                }

                ClaimsIdentity identity = new ClaimsIdentity(
                    new GenericIdentity(authUser.Email),
                    new[] {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.UniqueName, authUser.Id.ToString())
                    }
                );

                DateTime createDate = DateTime.Now;
                DateTime expirationDate = createDate + TimeSpan.FromSeconds(_tokenConfiguration.Seconds);

                var handler = new JwtSecurityTokenHandler();
                var token = CreateToken(identity, createDate, expirationDate, handler);
                return await SuccessObject(createDate, expirationDate, token, authUser, _mapper.Map<PerfilPermissoesDto>(perfilUser));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Authenticate.Error: " + ex.ToString());
                throw new Exception(ex.ToString());
            }
            
        }

        public async Task ForgotPassword(ForgotPasswordRequest forgotPasswordRequest, string urlOrigin)
        {
            try
            {
                var usuario = await _usuarioRepository.GetAsync(u => u.Email == forgotPasswordRequest.Email);
                if (usuario == null)
                {
                    throw new Exception($"E-mail {forgotPasswordRequest.Email} não esta cadastrado!");
                }

                var resetPassword = new ResetPassword()
                {
                    UsuarioId = usuario.Id,
                    Token = randomTokenString()
                };
                await _resetPasswordRepository.CreateAsync(resetPassword);

                

                var link = $"{urlOrigin}/recuperar-senha/{resetPassword.Token}";

                string mensagem = $"<p>Olá {usuario.Nome} </p>";
                mensagem += $"Clique <a href='{link}'>aqui</a> para alterar sua senha!";

                var message = new Message(new string[] { usuario.Email }, "Alterar senha de acesso", mensagem);

                _emailService.SendEmail(message);

            }
            catch (Exception ex)
            {
                Console.WriteLine("ForgotPassword.Error: " + ex.ToString());
                throw new Exception(ex.ToString());
            }
            
        }

        public async Task ResetPassword(ResetPasswordRequest model)
        {
            try
            {
                var resetPassData = await _resetPasswordRepository.GetAsync(r => r.Token == model.Token);
                if (resetPassData == null || resetPassData.Ativo == false)
                {
                    throw new Exception("Token inválido");
                }

                var usuario = await _usuarioRepository.GetAsync(resetPassData.UsuarioId);
                usuario.Password = BC.HashPassword(model.Password);
                resetPassData.DataRevogacao = DateTimeOffset.UtcNow;
                
                await _usuarioRepository.UpdateAsync(usuario);
                await _resetPasswordRepository.UpdateAsync(resetPassData);
            }
            catch (Exception ex)
            {

                Console.WriteLine("ResetPassword.Error: " + ex.ToString());
                throw new Exception(ex.ToString());
            }
        }

        private string randomTokenString()
        {
            var randomBytes = new byte[40];
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetBytes(randomBytes);
            }
            return BitConverter.ToString(randomBytes).Replace("-", "");
        }

        private string CreateToken(ClaimsIdentity identity, DateTime createDate, DateTime expirationDate, JwtSecurityTokenHandler handler)
        {
            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _tokenConfiguration.Issuer,
                Audience = _tokenConfiguration.Audience,
                SigningCredentials = _signingConfiguration.SigningCredentials,
                Subject = identity,
                NotBefore = createDate,
                Expires = expirationDate
            });
            var token = handler.WriteToken(securityToken);
            return token;
        }

        private async Task<LoginResponse> SuccessObject(DateTime createDate, DateTime expirationDate, string token, Usuario usuario, PerfilPermissoesDto perfil)
        {
            return new LoginResponse(
                Authenticated: true,
                "Usuário autenticado com sucesso!",
                createDate.ToString("yyyy-MM-dd HH:mm:ss"),
                expirationDate.ToString("yyyy-MM-dd HH:mm:ss"),
                token,
                usuario.Id,
                usuario.Nome,
                perfil
            );
        }
    }
}

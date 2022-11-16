using wca.compras.domain.Dtos;
using wca.compras.domain.Entities;
using wca.compras.domain.Interfaces;
using wca.compras.domain.Interfaces.Services;
using wca.compras.domain.Security;

namespace wca.compras.services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IRepository<Usuario> _repository;
        private readonly SigningConfiguration _signingConfiguration;
        private readonly TokenConfiguration _tokenConfiguration;

        public AuthenticationService(IRepository<Usuario> repository,
                                     SigningConfiguration signingConfiguration,
                                     TokenConfiguration tokenConfiguration)
        {
            _repository = repository;
            _signingConfiguration = signingConfiguration;
            _tokenConfiguration = tokenConfiguration;
        }
        public Task<LoginResponse> Authenticate(LoginRequest login)
        {
            throw new NotImplementedException();
        }

        public Task ForgotPassword(ForgotPasswordRequest email, string urlOrigin)
        {
            throw new NotImplementedException();
        }

        public Task ResetPassword(ResetPasswordRequest resetPassword)
        {
            throw new NotImplementedException();
        }
    }
}

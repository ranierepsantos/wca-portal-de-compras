using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wca.compras.domain.Dtos;

namespace wca.compras.domain.Interfaces.Services
{
    public interface IAuthenticationService
    {
        public Task<LoginResponse> Authenticate(LoginRequest login);

        public Task<bool> ForgotPassword(ForgotPasswordRequest email, string urlOrigin);

        public Task ResetPassword(ResetPasswordRequest resetPassword);
    }
}

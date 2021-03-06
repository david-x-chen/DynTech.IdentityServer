﻿using System.Threading.Tasks;

namespace Vanda.IdentityServer.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}

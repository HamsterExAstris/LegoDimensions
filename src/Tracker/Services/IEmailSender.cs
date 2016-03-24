﻿using System.Threading.Tasks;

namespace ShatteredTemple.LegoDimensions.Tracker.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}

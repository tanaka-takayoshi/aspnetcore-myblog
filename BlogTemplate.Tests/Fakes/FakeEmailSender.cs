using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using My_Blog.Services;

namespace My_Blog.Tests.Fakes
{
    class FakeEmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            throw new NotImplementedException();
        }
    }
}

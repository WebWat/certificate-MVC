using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Text;

namespace ApplicationCore.Services
{
    public class UrlGenerator : IUrlGenerator
    {
        public string Generate()
        {
            return WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(Guid.NewGuid().ToString() + Guid.NewGuid().ToString()));
        }
    }
}

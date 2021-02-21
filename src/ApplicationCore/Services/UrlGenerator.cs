using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Text;

namespace ApplicationCore.Services
{
    /// <summary>
    /// Generates a unique url for the user
    /// </summary>
    public class UrlGenerator : IUrlGenerator
    {
        public string Generate()
        {
            return WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(Guid.NewGuid().ToString()))[0..15];
        }
    }
}

﻿using RightOnBoard.JwtAuthTokenServer.Service.Interfaces;
using System;
using System.Security.Cryptography;
using System.Text;

namespace RightOnBoard.JwtAuthTokenServer.Service.Services
{
    public class SecurityService : ISecurityService
    {
        public string GetSha256Hash(string input)
        {
            using (var hashAlgorithm = new SHA256CryptoServiceProvider())
            {
                var byteValue = Encoding.UTF8.GetBytes(input);
                var byteHash = hashAlgorithm.ComputeHash(byteValue);
                return Convert.ToBase64String(byteHash);
            }
        }
    }
}

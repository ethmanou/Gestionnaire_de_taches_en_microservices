
using Front.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Front.Services
{
    public class RegisterService
    {
        private readonly HttpClient _httpClient;

        // Utilisez un constructeur avec une injection de dépendances pour initialiser HttpClient
        public RegisterService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Exemple d'une méthode pour enregistrer un utilisateur (décommentez si nécessaire)
        public UserDTO RegisterUser(string username, string password)
        {
            return new UserDTO
            {
                Id = 0,
                Email = "test@test.fr",
                Name = username,
            };
        }
    }
}



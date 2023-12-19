using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Authorization;
using Jaunts.Core.Models.AppSettings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Jaunts.Core.Api;
/// <summary>
/// Extension methods for working with Jwt bearer tokens
/// </summary>
public static class JwtTokenExtensionMethods
{

    /// <summary>
    /// Generates a Jwt bearer token containing the users username
    /// </summary>
    /// <param name="user">The users details</param>
    /// <returns></returns>
    public static string GenerateJwtToken(this ApplicationUser user, int permissionsValue)
    {
        var jwtConfiguration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(@"appsettings.json", false, false).Build();


        // Set our tokens claims
        var claims = new[]
        {
                // Unique ID for this token
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),

                // The username using the Identity name so it fills out the HttpContext.User.Identity.Name value
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserName),

                // Add user Id so that UserManager.GetUserAsync can find the user based on Id
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                //
                new Claim(CustomClaimTypes.Permissions, permissionsValue.ToString())
            };

        // Create the credentials used to generate the token
        var credentials = new SigningCredentials(
            // Get the secret key from configuration
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfiguration.GetSection("Jwt:SecretKey").Value)),
            // Use HS256 algorithm
            SecurityAlgorithms.HmacSha256);

        // Generate the Jwt Token
        var token = new JwtSecurityToken(
            issuer: jwtConfiguration.GetSection("Jwt:Issuer").Value,
            audience: jwtConfiguration.GetSection("Jwt:Audience").Value,
            claims: claims,
            signingCredentials: credentials,
            // Expire if not used for 3 months
            expires: DateTime.Now.AddMonths(3)
            );

        // Return the generated token
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}


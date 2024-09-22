
using Microsoft.EntityFrameworkCore;
//
using qlsv.Models;
using qlsv.Helpers;

namespace qlsv.Data;

public class InitDbContext
{
    // Initialize database for Identity
    public static void Initialize(
        IServiceProvider serviceProvider
    ) {
        using (var context = new IdentityDbContext(
            serviceProvider.GetRequiredService<DbContextOptions<IdentityDbContext>>()))
        {
            context.Database.EnsureCreated();

            // Create user root account
            if (!context.Users.Any())
            {
                var user = new UserCustom
                {
                    UserName = "root",
                    Email = "root@v.com",
                    EmailConfirmed = true,
                    ProfilePicture = new byte[] { 0 }, 
                    FirstName = "Ly Tran",
                    LastName = "Vinh",
                    Address = "HN",
                    Phone = "088888888"
                };

                var password = "123";
                var passwordHash = "i1CelkDpmAmgU08yFCskzfda4mWOI12kwgW571+2OiY="; // SecurityHelper.Hash(password);
                user.PasswordHash = passwordHash;

                context.Users.Add(user);
                context.SaveChanges();
            }
        }
    }
}
using DesafioBackEnd.Domain.Contracts.Persistence;
using DesafioBackEnd.Domain.Entities;
using DesafioBackEnd.Domain.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace DesafioBackEnd.Infrastructure.Persistence.Data.Seeds;

public static class UserSeed
{
    public static void Run(IServiceProvider service)
    {
        IRoleRepository roleRepository = service.GetRequiredService<IRoleRepository>();
        IUserRepository userRepository = service.GetRequiredService<IUserRepository>();

        List<Role> roles = roleRepository.All();

        var userEmail = "admin@desafiobackend.com";
        var adminUser = userRepository.FindByEmail(userEmail);

        if (adminUser is null)
        {
            adminUser = new User()
            {
                Id = Guid.NewGuid(),
                Name = "Administrator",
                Email = userEmail,
                RoleId = roles
                    .First(r => r.Name == RoleEnum.Administrator)
                    .Id,
            };

            userRepository.Add(adminUser);
        }
    }
}

using Core.Packages.Application.Features.Roles.Commands.Create;
using Core.Packages.Application.Features.UseRoles.Commands.Create;
using Core.Packages.Application.Shared.Result;
using Core.Packages.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Core.Packages.Application.Features.Auth.Register.Commands
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, IDataResult<int>>
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IMediator _mediator;
        public RegisterCommandHandler(UserManager<User> userManager, RoleManager<Role> roleManager, IMediator mediator)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mediator = mediator;
        }

        public async Task<IDataResult<int>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {

            User? existingUser = await _userManager.FindByEmailAsync(request.Email);
            string baseRoleName = "System Admin";

            if (existingUser != null)
            {
                return new ErrorDataResult<int>("User already exists");
            }
            else if (request.Password != request.ConfirmPassword)
            {
                return new ErrorDataResult<int>("The Passwords do not match, please check.");
            }

            User user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                IdentityNo = request.IdentityNo,
                Address = request.Address,
                Email = request.Email,
                UserName = request.Email
            };
            user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, request.Password);
            IdentityResult result = await _userManager.CreateAsync(user, request.Password);
            await _mediator.Send(new CreateRoleCommand { Name = baseRoleName });
            await _mediator.Send(new CreateUserRoleCommand { User = user, RoleName = baseRoleName });
            return new SuccessDataResult<int>(user.Id, "User created successfully");
        }
    }
}

using Core.Packages.Application.Shared.Result;
using Core.Packages.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Core.Packages.Application.Features.UseRoles.Commands.Create
{
    public class CreateUserRoleCommandHandler : IRequestHandler<CreateUserRoleCommand, IResult>
    {
        private readonly UserManager<User> _userManager;

        public CreateUserRoleCommandHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IResult> Handle(CreateUserRoleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _userManager.AddToRoleAsync(request.User, request.RoleName);
                return new SuccessResult("User role created successfully");
            }
            catch (Exception ex)
            {
                return new ErrorResult(ex.Message);
            }

        }
    }
}

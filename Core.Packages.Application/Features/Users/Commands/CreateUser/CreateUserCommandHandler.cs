using Core.Packages.Domain.Entities;
using Core.Packages.Domain.Repositories.EntityFrameworkCore;
using Core.Packages.Domain.Result;
using Core.Packages.Domain.ToolKit.Security;
using Core.Packages.Domain.UnitOfWork;
using MediatR;

namespace Core.Packages.Application.Features.Users.Commands.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, IResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;

        public CreateUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
        }

        public async Task<IResult> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                User user = new User
                {
                    Email = request.User.Email,
                    PasswordHash = _passwordHasher.HashPassword(request.User.PasswordHash),
                    FirstName = request.User.FirstName,
                    LastName = request.User.LastName,
                    CreatedBy = 1,
                };

                await _userRepository.AddAsync(user,cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _unitOfWork.CommitTransactionAsync(cancellationToken);
                return new SuccessResult();
            }
            catch (Exception)
            {

                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                return new ErrorResult("An error occurred while creating the user.");
            }
        }
    }
}

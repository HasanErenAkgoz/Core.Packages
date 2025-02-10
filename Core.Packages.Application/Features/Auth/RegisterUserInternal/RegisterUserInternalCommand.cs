using Business.Constants;
using Core.Packages.Domain.Entities;
using Core.Packages.Domain.Repositories.EntityFrameworkCore;
using Core.Packages.Domain.Result;
using Core.Packages.Domain.ToolKit.Security;
using Core.Packages.Domain.UnitOfWork;
using MediatR;

namespace Core.Packages.Application.Features.Auth.RegisterUserInternal
{
    public class RegisterUserInternalCommand : IRequest<IResult>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
    }

    public class RegisterUserInternalCommandHandler : IRequestHandler<RegisterUserInternalCommand, IResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;
        public RegisterUserInternalCommandHandler(IUnitOfWork unitOfWork, IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<IResult> Handle(RegisterUserInternalCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var isThereAnyUser = await _userRepository.GetAsync(u => u.Email == request.Email,cancellationToken);

                if (isThereAnyUser != null)
                {
                    return new ErrorResult(Messages.NameAlreadyExist);
                }

                await _userRepository.AddAsync(new User
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    PasswordHash = _passwordHasher.HashPassword(request.PasswordHash)
                }, cancellationToken);

                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _unitOfWork.CommitTransactionAsync(cancellationToken);
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            }

            throw new NotImplementedException();
        }
    }
}

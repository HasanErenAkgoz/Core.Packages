using AutoMapper;
using Core.Packages.Application.Common.Messages;
using Core.Packages.Application.Shared.Result;
using Core.Packages.Domain.Repositories.EntityFrameworkCore;
using MediatR;

namespace Core.Packages.Application.Features.Translate.Commands.Create
{
    public class CreateTranslateCommandHandler : IRequestHandler<CreateTranslateCommand, IResult>
    {
        private readonly ITranslationRepository _translationRepository;
        private readonly IMapper _mapper;

        public CreateTranslateCommandHandler(ITranslationRepository translationRepository, IMapper mapper)
        {
            _translationRepository = translationRepository;
            _mapper = mapper;
        }

        public async Task<IResult> Handle(CreateTranslateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                bool isExistTranslate = _translationRepository.Any(p => p.Key == request.Key && p.Language == request.Language);
                if (isExistTranslate)
                    return new ErrorResult(Messages.Exists);

                var translation = _mapper.Map<Domain.Entities.Translation>(request);
                await _translationRepository.AddAsync(translation, cancellationToken);

                return new SuccessResult(Messages.Added);
            }
            catch (Exception ex)
            {
                return new ErrorResult(ex.Message);

            }
            throw new NotImplementedException();
        }
    }
}

using Core.Packages.Application.Interfaces;

namespace Core.Packages.Infrastructure.Services;

public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.UtcNow;
} 
namespace Core.Packages.Domain.Security.TwoFactorAuth;

public class VerificationWindow
{
    public long PastTimeSteps { get; }
    public long FutureTimeSteps { get; }

    public VerificationWindow(long pastTimeSteps = 1, long futureTimeSteps = 1)
    {
        if (pastTimeSteps < 0)
            throw new ArgumentOutOfRangeException(nameof(pastTimeSteps), "Past time steps cannot be negative.");
        if (futureTimeSteps < 0)
            throw new ArgumentOutOfRangeException(nameof(futureTimeSteps), "Future time steps cannot be negative.");

        PastTimeSteps = pastTimeSteps;
        FutureTimeSteps = futureTimeSteps;
    }
} 
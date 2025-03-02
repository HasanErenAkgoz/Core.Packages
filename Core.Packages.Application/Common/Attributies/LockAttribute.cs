namespace Core.Packages.Application.Common.Attributies
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class LockAttribute : Attribute
    {
        public string LockKey { get; }
        public int LockSeconds { get; }
        public bool WaitForUnlock { get; }

        public LockAttribute(string lockKey, int lockSeconds = 10, bool waitForUnlock = false)
        {
            LockKey = lockKey;
            LockSeconds = lockSeconds;
            WaitForUnlock = waitForUnlock;
        }
    }
}

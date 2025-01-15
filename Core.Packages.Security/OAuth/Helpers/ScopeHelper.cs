namespace Core.Packages.Security.OAuth.Helpers;

// Scope işlemleri için yardımcı sınıf
public static class ScopeHelper
{
    // Scope'ları doğrulama
    public static bool ValidateScope(string allowedScopes, string requestedScope)
    {
        if (string.IsNullOrEmpty(requestedScope))
            return true;

        var allowedScopeList = allowedScopes.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var requestedScopeList = requestedScope.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        return requestedScopeList.All(scope => allowedScopeList.Contains(scope));
    }

    // Scope'ları normalize etme
    public static string NormalizeScope(string scope)
    {
        if (string.IsNullOrEmpty(scope))
            return string.Empty;

        var scopes = scope.Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Distinct()
            .OrderBy(s => s);

        return string.Join(" ", scopes);
    }

    // Scope'ları birleştirme
    public static string CombineScopes(params string[] scopes)
    {
        var combinedScopes = scopes
            .Where(s => !string.IsNullOrEmpty(s))
            .SelectMany(s => s.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            .Distinct()
            .OrderBy(s => s);

        return string.Join(" ", combinedScopes);
    }
} 
using System;

namespace Hybel.HealthSystem
{
    internal static class DelegateExtensions
    {
        public static bool HasListeners(this Delegate @delegate) => !((@delegate?.GetInvocationList().Length ?? 0) == 0);
    }
}

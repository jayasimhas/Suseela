using System;

namespace Informa.Library.Utilities.Extensions
{
    public static class BooleanExtensions
    {
        public static bool Not(this bool _this) => !_this;

        public static void Then(this bool condition, Action action)
        {
            if (condition) { action(); };
        }
    }
}

using Microsoft.Extensions.Localization;

namespace LudusAppoint.Infrastructure.Localization
{
    /// <summary>
    /// A composite localizer that tries primary localizer first and falls back to secondary when resource is not found.
    /// </summary>
    public class FallbackStringLocalizer : IStringLocalizer
    {
        private readonly IStringLocalizer _primary;
        private readonly IStringLocalizer _fallback;

        public FallbackStringLocalizer(IStringLocalizer primary, IStringLocalizer fallback)
        {
            _primary = primary ?? throw new ArgumentNullException(nameof(primary));
            _fallback = fallback ?? throw new ArgumentNullException(nameof(fallback));
        }

        public LocalizedString this[string name]
        {
            get
            {
                var primaryValue = _primary[name];
                return primaryValue.ResourceNotFound ? _fallback[name] : primaryValue;
            }
        }

        public LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                var primaryValue = _primary[name, arguments];
                return primaryValue.ResourceNotFound ? _fallback[name, arguments] : primaryValue;
            }
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            // Primary should take precedence; merge uniquely by name.
            var primaryList = _primary.GetAllStrings(includeParentCultures).ToList();
            var fallbackList = _fallback.GetAllStrings(includeParentCultures);

            var names = new HashSet<string>(primaryList.Select(s => s.Name));
            foreach (var s in fallbackList)
            {
                if (!names.Contains(s.Name))
                    primaryList.Add(s);
            }
            return primaryList;
        }
    }
}
using System;
using System.Linq;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace LudusAppoint.Infrastructure.Localization
{
    /// <summary>
    /// Wraps the ResourceManager-based factory and returns a FallbackStringLocalizer
    /// which falls back from tenant-specific resource base names to shared ones.
    /// It detects the "Views" segment anywhere in the baseName and strips the tenant segment after it.
    /// </summary>
    public class FallbackStringLocalizerFactory : IStringLocalizerFactory
    {
        private readonly ResourceManagerStringLocalizerFactory _innerFactory;
        private readonly ILogger<FallbackStringLocalizerFactory>? _logger;

        public FallbackStringLocalizerFactory(ResourceManagerStringLocalizerFactory innerFactory, ILogger<FallbackStringLocalizerFactory>? logger = null)
        {
            _innerFactory = innerFactory ?? throw new ArgumentNullException(nameof(innerFactory));
            _logger = logger;
        }

        public IStringLocalizer Create(Type resourceSource)
        {
            // For type-based localizers, just use the inner factory directly
            return _innerFactory.Create(resourceSource);
        }

        public IStringLocalizer Create(string baseName, string location)
        {
            try
            {
                _logger?.LogDebug("Creating localizer for baseName='{baseName}', location='{location}'", baseName, location);

                if (string.IsNullOrEmpty(baseName))
                    return _innerFactory.Create(baseName, location);

                // Split into segments and find the "Views" segment (case-insensitive)
                var parts = baseName.Split('.');
                var viewsIndex = Array.FindIndex(parts, p => string.Equals(p, "Views", StringComparison.OrdinalIgnoreCase));

                if (viewsIndex >= 0 && parts.Length > viewsIndex + 2)
                {
                    // Compose fallback by skipping the tenant segment (the segment after "Views")
                    var fallbackParts = parts.Take(viewsIndex + 1).Concat(parts.Skip(viewsIndex + 2));
                    var fallbackBaseName = string.Join(".", fallbackParts);

                    _logger?.LogDebug("Detected Views segment at index {index}. Primary='{primary}', Fallback='{fallback}'",
                                      viewsIndex, baseName, fallbackBaseName);

                    var primary = _innerFactory.Create(baseName, location);
                    var fallback = _innerFactory.Create(fallbackBaseName, location);
                    return new FallbackStringLocalizer(primary, fallback);
                }

                // No tenant-ish Views pattern detected — return inner factory result
                return _innerFactory.Create(baseName, location);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error creating fallback localizer for baseName='{baseName}'", baseName);
                // Fallback to inner factory in case of errors
                return _innerFactory.Create(baseName, location);
            }
        }
    }
}
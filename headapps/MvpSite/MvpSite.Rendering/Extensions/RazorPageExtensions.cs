using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace MvpSite.Rendering.Extensions;

public static class RazorPageExtensions
{
    public static HtmlPrefixSwitcher SwitchHtmlPrefix<T>(this RazorPage<T> page, string prefix)
    {
        return new HtmlPrefixSwitcher(page.ViewData.TemplateInfo, prefix);
    }

    public sealed class HtmlPrefixSwitcher : IDisposable
    {
        private readonly TemplateInfo? _templateInfo;

        private readonly string? _original;

        public HtmlPrefixSwitcher(TemplateInfo? templateInfo, string prefix)
        {
            _templateInfo = templateInfo;
            if (_templateInfo != null)
            {
                _original = _templateInfo.HtmlFieldPrefix;
                _templateInfo.HtmlFieldPrefix = prefix;
            }
        }

        public void Dispose()
        {
            if (_templateInfo != null)
            {
                _templateInfo.HtmlFieldPrefix = _original;
            }
        }
    }
}
using Microsoft.AspNetCore.Components;

namespace MyGeoApplication.Pages;

public static class CommonFragments
{
    public static readonly RenderFragment EmptyGridContent = _ =>
    {
        // <p style="color: lightgrey; font-size: 24px; text-align: center; margin: 2rem;">No records to display</p>
        _.OpenElement(0, "p");
        _.AddAttribute(1, "style", "color: lightgrey; font-size: 24px; text-align: center; margin: 2rem;");
        _.AddContent(2, "Nothing to display");
        _.CloseElement();
    };
}
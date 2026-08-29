using System.Xml.Linq;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Renty.Web.TagHelpers
{
    [HtmlTargetElement("icon")]
    public class IconTagHelper : TagHelper
    {
        private readonly IWebHostEnvironment _env;

        public IconTagHelper(IWebHostEnvironment env)
        {
            _env = env;
        }

        public string Name { get; set; } = string.Empty;

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var path = Path.Combine(_env.WebRootPath, "icons", $"{Name}.svg");
            var raw = await File.ReadAllTextAsync(path);
            var svg = XDocument.Parse(raw).Root!;

            output.TagName = "svg";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.SetAttribute("viewBox", svg.Attribute("viewBox")?.Value);
            output.Attributes.SetAttribute("xmlns", "http://www.w3.org/2000/svg");
            output.Attributes.SetAttribute("aria-hidden", "true");
            output.Attributes.SetAttribute("fill", "none");
            output.Attributes.SetAttribute("width", "1em");
            output.Attributes.SetAttribute("height", "1em");
            output.Content.SetHtmlContent(string.Concat(svg.Nodes()));
        }
    }
}

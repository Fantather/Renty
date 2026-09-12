using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Renty.Web.TagHelpers
{
    [HtmlTargetElement("input-field")]
    public class InputFieldTagHelper : TagHelper
    {
        private readonly IHtmlGenerator _htmlGenerator;
        public InputFieldTagHelper (IHtmlGenerator htmlGenerator)
        {
            _htmlGenerator = htmlGenerator;
        }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; } = default!;

        [HtmlAttributeName("asp-for")]
        public ModelExpression For { get; set; } = default!;

        public string Type { get; set; } = "text";
        public string? Placeholder { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.AddClass("input-field", HtmlEncoder.Default);

            var inputAttributes = new Dictionary<string, object>
            {
                ["class"] = "input-field__input",
                ["type"] = Type,
                ["placeholder"] = Placeholder ?? For.Metadata.DisplayName ?? For.Name,
            };
            TagBuilder input = _htmlGenerator.GenerateTextBox(ViewContext, For.ModelExplorer, For.Name, For.Model, format: null, htmlAttributes: inputAttributes);
            output.Content.AppendHtml(input);

            TagBuilder? validationMessage = _htmlGenerator.GenerateValidationMessage(ViewContext, For.ModelExplorer, For.Name, message: null, tag: null, htmlAttributes: new Dictionary<string, object> { ["class"] = "input-field__error" });
            if(validationMessage != null)
            {
                output.Content.AppendHtml(validationMessage);
            }
        }
    }
}

using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Renty.Web.TagHelpers
{
    /// <summary>
    /// Поле с автокомплитом: скрытый input (несёт реальное значение и валидацию через
    /// asp-for) + видимый текстовый input для поиска (не привязан к модели) +
    /// пустой контейнер под результаты, который заполняет shared/search-field.js.
    /// </summary>
    [HtmlTargetElement("search-field")]
    public class SearchFieldTagHelper : TagHelper
    {
        private readonly IHtmlGenerator _htmlGenerator;

        public SearchFieldTagHelper(IHtmlGenerator htmlGenerator)
        {
            _htmlGenerator = htmlGenerator;
        }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; } = default!;

        [HtmlAttributeName("asp-for")]
        public ModelExpression For { get; set; } = default!;

        public string? Placeholder { get; set; }
        public string SearchUrl { get; set; } = string.Empty;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.AddClass("search-field", HtmlEncoder.Default);
            output.Attributes.SetAttribute("data-search-url", SearchUrl);

            TagBuilder hidden = _htmlGenerator.GenerateHidden(ViewContext, For.ModelExplorer, For.Name,
                value: For.Model, useViewData: false, htmlAttributes: null);
            output.Content.AppendHtml(hidden);

            var visibleInputAttributes = new Dictionary<string, object>
            {
                ["type"] = "text",
                ["id"] = For.Name + "_search",
                ["class"] = "input-field__input",
                ["autocomplete"] = "off",
                ["placeholder"] = Placeholder ?? For.Metadata.DisplayName ?? For.Name,
            };
            var visibleInput = new TagBuilder("input");
            visibleInput.TagRenderMode = TagRenderMode.SelfClosing;
            foreach (var attribute in visibleInputAttributes)
            {
                visibleInput.Attributes[attribute.Key] = attribute.Value.ToString();
            }
            output.Content.AppendHtml(visibleInput);

            var resultsPanel = new TagBuilder("div");
            resultsPanel.Attributes["id"] = For.Name + "_results";
            resultsPanel.Attributes["hidden"] = "hidden";
            resultsPanel.AddCssClass("popover-panel");
            resultsPanel.AddCssClass("search-field__results");
            output.Content.AppendHtml(resultsPanel);

            TagBuilder? validationMessage = _htmlGenerator.GenerateValidationMessage(ViewContext, For.ModelExplorer, For.Name,
                message: null, tag: null, htmlAttributes: new Dictionary<string, object> { ["class"] = "input-field__error" });
            if (validationMessage != null)
            {
                output.Content.AppendHtml(validationMessage);
            }
        }
    }
}

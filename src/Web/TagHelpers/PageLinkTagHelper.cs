﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Web.ViewModels;

namespace Web.TagHelpers;

public class PageLinkTagHelper : TagHelper
{
    private readonly IUrlHelperFactory urlHelperFactory;

    [ViewContext]
    [HtmlAttributeNotBound]
    public ViewContext ViewContext { get; set; }
    public PageViewModel PageModel { get; set; }
    public string PageController { get; set; } = string.Empty;
    public string PageAction { get; set; } = string.Empty;
    public string UniqueUrl { get; set; } = string.Empty;

    public PageLinkTagHelper(IUrlHelperFactory helperFactory)
    {
        urlHelperFactory = helperFactory;
    }


    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        IUrlHelper urlHelper = urlHelperFactory.GetUrlHelper(ViewContext);
        output.TagName = "div";
        output.Attributes.Add("style", "display: flex; justify-content: center; margin-top: 1em;");

        TagBuilder tag = new("ul");
        tag.AddCssClass("pagination");

        TagBuilder currentItem = CreateTag(PageModel.PageNumber, urlHelper);

        if (PageModel.FirstPage)
        {
            TagBuilder prevItem = CreateFirstPage(urlHelper);
            tag.InnerHtml.AppendHtml(prevItem);
        }

        if (PageModel.HasPreviousPage)
        {
            if (PageModel.HasFollowingPreviousPage)
            {
                TagBuilder prevItem2 = CreateTag(PageModel.PageNumber - 2, urlHelper);
                tag.InnerHtml.AppendHtml(prevItem2);
            }

            TagBuilder prevItem1 = CreateTag(PageModel.PageNumber - 1, urlHelper);
            tag.InnerHtml.AppendHtml(prevItem1);
        }

        tag.InnerHtml.AppendHtml(currentItem);

        if (PageModel.HasNextPage)
        {
            TagBuilder nextItem1 = CreateTag(PageModel.PageNumber + 1, urlHelper);
            tag.InnerHtml.AppendHtml(nextItem1);

            if (PageModel.HasFollowingNextPage)
            {
                TagBuilder nextItem2 = CreateTag(PageModel.PageNumber + 2, urlHelper);
                tag.InnerHtml.AppendHtml(nextItem2);
            }
        }

        if (PageModel.LastPage)
        {
            TagBuilder prevItem = CreateLastPage(urlHelper);
            tag.InnerHtml.AppendHtml(prevItem);
        }

        output.Content.AppendHtml(tag);
    }


    TagBuilder CreateFirstPage(IUrlHelper urlHelper)
    {
        TagBuilder item = new("li");
        item.AddCssClass("page-item");

        TagBuilder link = new("a");
        link.AddCssClass("page-link");

        if (UniqueUrl != null)
            link.Attributes["href"] = urlHelper.Action(PageAction, PageController) + "?page=1";
        else
            link.Attributes["href"] = urlHelper.Action(PageAction, PageController, new { page = 1 });

        link.InnerHtml.Append("«");
        item.InnerHtml.AppendHtml(link);

        return item;
    }


    TagBuilder CreateLastPage(IUrlHelper urlHelper)
    {
        TagBuilder item = new("li");
        item.AddCssClass("page-item");

        TagBuilder link = new("a");
        link.AddCssClass("page-link");

        link.Attributes["href"] = UniqueUrl != null ? urlHelper.Action(PageAction, PageController) + $"?page={PageModel.TotalPages}" :
                                                      urlHelper.Action(PageAction, PageController, new { page = PageModel.TotalPages });

        link.InnerHtml.Append("»");
        item.InnerHtml.AppendHtml(link);

        return item;
    }


    TagBuilder CreateTag(int pageNumber, IUrlHelper urlHelper)
    {
        TagBuilder item = new("li");
        item.AddCssClass("page-item");

        TagBuilder link = new("a");
        link.AddCssClass("page-link");

        if (pageNumber == PageModel.PageNumber)
        {
            item.AddCssClass("active");
        }
        else
        {
            link.Attributes["href"] = UniqueUrl != null ? urlHelper.Action(PageAction, PageController) + $"?page={PageModel.TotalPages}" :
                                                          urlHelper.Action(PageAction, PageController, new { page = PageModel.TotalPages });
        }

        link.InnerHtml.Append(pageNumber.ToString());
        item.InnerHtml.AppendHtml(link);

        return item;
    }
}

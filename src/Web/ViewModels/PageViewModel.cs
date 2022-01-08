using System;

namespace Web.ViewModels;

public class PageViewModel
{
    public int PageNumber { get; private set; }
    public int TotalPages { get; private set; }

    public PageViewModel(int count, int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;

        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
    }

    public bool HasPreviousPage
    {
        get
        {
            return PageNumber > 1;
        }
    }

    public bool HasNextPage
    {
        get
        {
            return PageNumber < TotalPages;
        }
    }

    public bool HasFollowingPreviousPage
    {
        get
        {
            return PageNumber > 2;
        }
    }

    public bool HasFollowingNextPage
    {
        get
        {
            return PageNumber < TotalPages - 1;
        }
    }

    public bool FirstPage
    {
        get
        {
            return PageNumber >= 4;
        }
    }

    public bool LastPage
    {
        get
        {
            return PageNumber < TotalPages - 2;
        }
    }
}
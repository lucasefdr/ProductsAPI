﻿namespace ProductsAPI.Pagination;

public class PaginationParameters
{
    public int PageNumber { get; set; } = 1;
    private int _pageSize = 10;
    const int maxPageSize = 25;

    public int PageSize
    {
        get
        {
            return _pageSize;
        }
        set
        {
            _pageSize = (value > maxPageSize) ? maxPageSize : value;
        }
    }
}

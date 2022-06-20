namespace Application.Common.Models;

public abstract class PaginationQuery
{
    private int pageNumber;
    private int pageSize;

    public int PageNumber
    {
        get => GetOrSetPageNumber(pageNumber);
        set => pageNumber = GetOrSetPageNumber(value);
    }

    public int PageSize
    {
        get => GetOrSetPageSize(pageSize);
        set => pageSize = GetOrSetPageSize(value);
    }

    public int Skip()
    {
        return PageSize * (PageNumber - 1);
    }

    public int Take()
    {
        return PageSize;
    }


    private int GetOrSetPageNumber(int pageNumber)
    {
        if (pageNumber <= 0)
            pageNumber = 1;

        return pageNumber;
    }

    private int GetOrSetPageSize(int pageSize)
    {
        if (pageSize <= 0 || pageSize > 100)
            pageSize = 10;

        return pageSize;
    }
}

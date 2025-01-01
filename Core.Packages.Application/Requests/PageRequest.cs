namespace Core.Packages.Application.Requests;

public class PageRequest
{
    public int PageIndex { get; set; }
    public int PageSize { get; set; }

    public PageRequest()
    {
        PageIndex = 0;
        PageSize = 10;
    }

    public PageRequest(int pageIndex, int pageSize)
    {
        PageIndex = pageIndex < 0 ? 0 : pageIndex;
        PageSize = pageSize > 100 ? 100 : pageSize;
    }
} 
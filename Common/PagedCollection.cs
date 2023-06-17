using asp.net_core_api_template.Models;

namespace asp.net_core_api_template.Common;

public class PagedCollection<TResponse>
{
    public int Limit { get; set; }
    public int Offset { get; set; }
    public bool Finished { get; set; }
    public ICollection<TResponse> Items { get; set; }

    public PagedCollection(ICollection<TResponse> items, int limit, int offset, bool finished)
    {
        Limit = limit;
        Offset = offset;
        Items = items;
        Finished = finished;
    }
}

public abstract class PagedCollection<TModel, TResponse> where TModel: IModel<TResponse>
{
    private static readonly int LIMIT_DEFAULT = 10;
    private static readonly int OFFSET_DEFAULT = 0;
    public static PagedCollection<TResponse> ToPagedCollection(IQueryable<TModel> items, int? limit, int? offset)
    {
        int _limit = limit?? LIMIT_DEFAULT;
        if (limit < 0)
        {
            _limit = -1;
        }
        
        int _offset = offset?? OFFSET_DEFAULT;
        if (_offset < 0)
        {
            _offset = 0;
        }
        
        // if (items.Count() <= offset)
        // {
        //     return new PagedCollection<TResponse>(new List<TResponse>(), _limit, _offset, true);
        // }
        
        items = items.Skip(_offset);
        if (_limit > 0)
        {
            items = items.Take(_limit);
        }
        var responses = items
            .Select(e => e.Map())
            .ToList();

        return new PagedCollection<TResponse>(responses, _limit, _offset, responses.Count < _limit);
    }
}
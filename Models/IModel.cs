

namespace asp.net_core_api_template.Models;

public interface IModel<TResponse>
{
    TResponse Map();
}
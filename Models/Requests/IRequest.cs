namespace asp.net_core_api_template.Models.Requests;

public interface IRequest<TModel>
{
    void Validate();
    TModel ToModel();
}
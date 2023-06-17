namespace asp.net_core_api_template.Infrastructure
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class IncludeInMobileAppApi : Attribute { }
    
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class IncludeInWebappApi : Attribute { }
}
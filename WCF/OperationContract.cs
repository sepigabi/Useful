public interface IFooService
{
    [OperationContract]
    [WebInvoke(Method = "POST",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.Bare,
        UriTemplate = "/leadUpdates")]
    object LeadUpdates(Lead lead);
}

[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
public class FooService : IFooService
{
    public object LeadUpdates(Lead lead)
    {
        //TODO Implement
    }
}

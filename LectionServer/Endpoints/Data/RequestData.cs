using System.Reflection;
using System.Security.Authentication;

namespace LectionServer.Endpoints.Data;

public record RequestData(Guid UserId)
{
    public static ValueTask<RequestData?> BindAsync(HttpContext context, ParameterInfo parameter)
    {
        if(!Guid.TryParse(context.User.Identities.First().Claims.First(x => x.Type == "id").Value, out var userId))
            throw new AuthenticationException("Can not verify a session");
        
        return ValueTask.FromResult<RequestData?>(
            new RequestData(userId)
        );
    }
}
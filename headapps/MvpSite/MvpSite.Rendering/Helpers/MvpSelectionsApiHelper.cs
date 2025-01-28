using System.Net;
using Mvp.Selections.Client;
using Mvp.Selections.Client.Models;
using Mvp.Selections.Domain;

namespace MvpSite.Rendering.Helpers;

public class MvpSelectionsApiHelper(MvpSelectionsApiClient client)
{
    public async Task<bool> IsCurrentUserAnAdmin()
    {
        bool isAdmin = false;
        if (await client.IsAuthenticatedAsync())
        {
            Response<User> currentUserResponse = await client.GetCurrentUserAsync();
            if (currentUserResponse is { StatusCode: HttpStatusCode.OK, Result: not null })
            {
                isAdmin = currentUserResponse.Result.HasRight(Right.Admin);
            }
        }

        return isAdmin;
    }
}
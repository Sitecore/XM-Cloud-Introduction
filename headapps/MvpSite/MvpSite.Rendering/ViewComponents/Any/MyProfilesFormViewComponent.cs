using System.Net;
using Microsoft.AspNetCore.Mvc;
using Mvp.Selections.Client;
using Mvp.Selections.Client.Models;
using Mvp.Selections.Domain;
using MvpSite.Rendering.Models.Any;
using Sitecore.AspNetCore.SDK.RenderingEngine.Binding;

namespace MvpSite.Rendering.ViewComponents.Any;

[ViewComponent(Name = ViewComponentName)]
public class MyProfilesFormViewComponent(IViewModelBinder modelBinder, MvpSelectionsApiClient client)
    : BaseViewComponent(modelBinder, client)
{
    public const string ViewComponentName = "AnyMyProfilesForm";

    public override async Task<IViewComponentResult> InvokeAsync()
    {
        IViewComponentResult result;
        MyProfilesFormModel model = await ModelBinder.Bind<MyProfilesFormModel>(ViewContext);
        if (!model.IsEditing)
        {
            Response<User> userResponse = await Client.GetCurrentUserAsync();
            if (userResponse is { StatusCode: HttpStatusCode.OK, Result: not null })
            {
                User user = userResponse.Result;
                model.Links.AddRange(user.Links);
                if (model.RemoveProfileLinkId != null)
                {
                    model.RemoveProfileLink =
                        model.Links.FirstOrDefault(l => l.Id == model.RemoveProfileLinkId);
                }

                if (model is { RemoveProfileLinkId: not null, RemoveConfirmed: false })
                {
                    result = View("Confirm", model);
                }
                else if (model is { RemoveProfileLinkId: not null, RemoveConfirmed: true })
                {
                    Response<bool> removeResponse = await Client.RemoveProfileLinkAsync(user.Id, model.RemoveProfileLinkId.Value);
                    if (removeResponse.Result && model.RemoveProfileLink != null)
                    {
                        model.Links.Remove(model.RemoveProfileLink);
                        ModelState.Clear();
                        result = View(model);
                    }
                    else
                    {
                        model.ErrorMessages.Add(removeResponse.Message);
                        result = View("~/Views/Shared/_Error.cshtml", model);
                    }
                }
                else if (model.IsEdit && ModelState.IsValid)
                {
                    ProfileLink newProfileLink = new(Guid.Empty)
                    {
                        Name = model.Name!,
                        Uri = model.Link!,
                        Type = model.Type
                    };
                    Response<ProfileLink> newResponse = await Client.AddProfileLinkAsync(user.Id, newProfileLink);
                    if (newResponse is { StatusCode: HttpStatusCode.Created, Result: not null })
                    {
                        model.Links.Add(newResponse.Result);
                        ModelState.Clear();
                        result = View(model);
                    }
                    else
                    {
                        model.ErrorMessages.Add(newResponse.Message);
                        result = View("~/Views/Shared/_Error.cshtml", model);
                    }
                }
                else if (!model.IsEdit)
                {
                    ModelState.Clear();
                    result = View(model);
                }
                else
                {
                    result = View(model);
                }
            }
            else
            {
                model.ErrorMessages.Add(userResponse.Message);
                result = View("~/Views/Shared/_Error.cshtml", model);
            }
        }
        else
        {
            result = View("ExperienceEditor", model);
        }

        return result;
    }
}
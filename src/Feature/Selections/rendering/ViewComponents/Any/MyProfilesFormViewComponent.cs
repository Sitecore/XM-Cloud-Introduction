using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mvp.Feature.Selections.Models.Any;
using Mvp.Selections.Client;
using Mvp.Selections.Client.Models;
using Mvp.Selections.Domain;
using Sitecore.AspNet.RenderingEngine.Binding;

namespace Mvp.Feature.Selections.ViewComponents.Any
{
    [ViewComponent(Name = ViewComponentName)]
    public class MyProfilesFormViewComponent : BaseViewComponent
    {
        public const string ViewComponentName = "AnyMyProfilesForm";

        public MyProfilesFormViewComponent(IViewModelBinder modelBinder, MvpSelectionsApiClient client)
            : base(modelBinder, client)
        {
        }

        public override async Task<IViewComponentResult> InvokeAsync()
        {
            IViewComponentResult result;
            MyProfilesFormModel model = await ModelBinder.Bind<MyProfilesFormModel>(ViewContext);
            if (!model.IsEditing)
            {
                Response<User> userResponse = await Client.GetCurrentUserAsync();
                if (userResponse.StatusCode == HttpStatusCode.OK && userResponse.Result != null)
                {
                    User user = userResponse.Result;
                    model.Links.AddRange(user.Links);
                    if (model.RemoveProfileLinkId != null)
                    {
                        model.RemoveProfileLink =
                            model.Links.FirstOrDefault(l => l.Id == model.RemoveProfileLinkId);
                    }

                    if (model.RemoveProfileLinkId != null && !model.RemoveConfirmed)
                    {
                        result = View("Confirm", model);
                    }
                    else if (model.RemoveProfileLinkId != null && model.RemoveConfirmed)
                    {
                        Response<bool> removeResponse = await Client.RemoveProfileLinkAsync(user.Id, model.RemoveProfileLinkId.Value);
                        if (removeResponse.Result)
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
                        ProfileLink newProfileLink = new (Guid.Empty)
                        {
                            Name = model.Name,
                            Uri = model.Link,
                            Type = model.Type
                        };
                        Response<ProfileLink> newResponse = await Client.AddProfileLinkAsync(user.Id, newProfileLink);
                        if (newResponse.StatusCode == HttpStatusCode.OK && newResponse.Result != null)
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
}

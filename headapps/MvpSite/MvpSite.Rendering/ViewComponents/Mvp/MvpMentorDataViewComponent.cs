using System.Net;
using Microsoft.AspNetCore.Mvc;
using Mvp.Selections.Api.Model;
using Mvp.Selections.Client;
using Mvp.Selections.Client.Models;
using Mvp.Selections.Domain;
using MvpSite.Rendering.Models.Mvp;
using Sitecore.AspNetCore.SDK.RenderingEngine.Binding;

namespace MvpSite.Rendering.ViewComponents.Mvp;

[ViewComponent(Name = ViewComponentName)]
public class MvpMentorDataViewComponent(IViewModelBinder modelBinder, MvpSelectionsApiClient client)
    : BaseViewComponent(modelBinder, client)
{
    public const string ViewComponentName = "MvpMentorData";

    public override async Task<IViewComponentResult> InvokeAsync()
    {
        IViewComponentResult result;
        MentorDataModel model = await ModelBinder.Bind<MentorDataModel>(ViewContext);
        if (!model.IsEditing)
        {
            if (model.IsEdit && ModelState.IsValid)
            {
                model.IsMvp = true;
                Tuple<Guid, Mentor?> mentor = await LoadCurrentMentorAsync();
                if (mentor.Item2 != null && model.IsMentor)
                {
                    Response<Mentor> updateResponse = await Client.UpdateMentorAsync(new Mentor
                    {
                        Id = mentor.Item1,
                        IsOpenToNewMentees = model.IsOpenToNewMentees,
                        Description = model.MentorDescription ?? string.Empty
                    });
                    if (updateResponse.StatusCode == HttpStatusCode.OK)
                    {
                        ModelState.Clear();
                        result = View("Updated", model);
                    }
                    else
                    {
                        model.ErrorMessages.Add(updateResponse.Message);
                        result = View("~/Views/Shared/_Error.cshtml", model);
                    }
                }
                else if (mentor.Item2 == null && model.IsMentor)
                {
                    Response<Mentor> createResponse = await Client.AddMentorAsync(new Mentor
                    {
                        Id = mentor.Item1,
                        IsOpenToNewMentees = model.IsOpenToNewMentees,
                        Description = model.MentorDescription ?? string.Empty
                    });
                    if (createResponse.StatusCode == HttpStatusCode.Created)
                    {
                        ModelState.Clear();
                        result = View("Updated", model);
                    }
                    else
                    {
                        model.ErrorMessages.Add(createResponse.Message);
                        result = View("~/Views/Shared/_Error.cshtml", model);
                    }
                }
                else if (mentor.Item2 != null && !model.IsMentor)
                {
                    Response<bool> removeResponse = await Client.RemoveMentorAsync(mentor.Item1);
                    if (removeResponse.StatusCode == HttpStatusCode.NoContent)
                    {
                        ModelState.Clear();
                        result = View("Updated", model);
                    }
                    else
                    {
                        model.ErrorMessages.Add(removeResponse.Message);
                        result = View("~/Views/Shared/_Error.cshtml", model);
                    }
                }
                else
                {
                    result = View(model);
                }
            }
            else if (model.IsEdit && !ModelState.IsValid)
            {
                model.IsMvp = true;
                result = View(model);
            }
            else
            {
                await LoadMentorProfile(model);
                result = View(model);
            }
        }
        else
        {
            result = View("ExperienceEditor", model);
        }

        return result;
    }

    private async Task LoadMentorProfile(MentorDataModel model)
    {
        User? currentUser = await LoadCurrentUserAsync();
        if (currentUser != null)
        {
            Mentor? mentor = await LoadMentorAsync(currentUser.Id);
            if (mentor != null)
            {
                model.IsMentor = true;
                model.IsOpenToNewMentees = mentor.IsOpenToNewMentees;
                model.MentorDescription = mentor.Description;
                model.IsMvp = true;
            }
            else
            {
                MvpProfile? profile = await LoadMvpProfileAsync(currentUser.Id);
                if (profile != null)
                {
                    model.IsMvp = true;
                }
            }
        }
    }

    private async Task<Tuple<Guid, Mentor?>> LoadCurrentMentorAsync()
    {
        Mentor? mentor = null;
        User? currentUser = await LoadCurrentUserAsync();
        if (currentUser != null)
        {
            mentor = await LoadMentorAsync(currentUser.Id);
        }

        return new Tuple<Guid, Mentor?>(currentUser?.Id ?? Guid.Empty, mentor);
    }

    private async Task<Mentor?> LoadMentorAsync(Guid id)
    {
        Mentor? result = null;
        Response<Mentor> mentorResponse = await Client.GetMentorAsync(id);
        if (mentorResponse.StatusCode == HttpStatusCode.OK)
        {
            result = mentorResponse.Result;
        }

        return result;
    }

    private async Task<MvpProfile?> LoadMvpProfileAsync(Guid id)
    {
        MvpProfile? result = null;
        Response<MvpProfile> profileResponse = await Client.GetMvpProfileAsync(id);
        if (profileResponse is { StatusCode: HttpStatusCode.OK, Result: not null })
        {
            result = profileResponse.Result;
        }

        return result;
    }

    private async Task<User?> LoadCurrentUserAsync()
    {
        User? result = null;
        Response<User> userResponse = await Client.GetCurrentUserAsync();
        if (userResponse is { StatusCode: HttpStatusCode.OK, Result: not null })
        {
            result = userResponse.Result;
        }

        return result;
    }
}
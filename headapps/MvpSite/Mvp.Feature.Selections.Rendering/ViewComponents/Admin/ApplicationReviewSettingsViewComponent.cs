using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mvp.Feature.Selections.Models.Admin;
using Mvp.Selections.Client;
using Mvp.Selections.Client.Models;
using Mvp.Selections.Client.Models.Request;
using Mvp.Selections.Domain;
using Mvp.Selections.Domain.Roles;
using Sitecore.AspNetCore.SDK.RenderingEngine.Binding;

namespace Mvp.Feature.Selections.ViewComponents.Admin
{
    [ViewComponent(Name = ViewComponentName)]
    public class ApplicationReviewSettingsViewComponent(IViewModelBinder modelBinder, MvpSelectionsApiClient client)
        : BaseViewComponent(modelBinder, client)
    {
        public const string ViewComponentName = "AdminApplicationReviewSettings";

        public override async Task<IViewComponentResult> InvokeAsync()
        {
            ApplicationReviewSettingsModel model = await ModelBinder.Bind<ApplicationReviewSettingsModel>(ViewContext);
            if (model.IsEditing)
            {
                GenerateFakeDataForEdit(model);
            }
            else
            {
                if (model.AddReviewerUserEmails.Count > 0)
                {
                    await AddReviewers(model);
                }
                else if (model.RemoveReviewerUserId.HasValue)
                {
                    await RemoveReviewer(model);
                }

                await LoadData(model);
            }

            return model.ErrorMessages.Count > 0 ? View("~/Views/Shared/_Error.cshtml", model) : View(model);
        }

        private static void GenerateFakeDataForEdit(ApplicationReviewSettingsModel model)
        {
            model.Application = new Application(Guid.NewGuid())
            {
                Applicant = new User(Guid.NewGuid())
                {
                    Name = "Lorem Ipsum"
                },
                Selection = new Selection(Guid.NewGuid())
                {
                    Year = (short)DateTime.Now.Year
                },
                MvpType = new MvpType(1)
                {
                    Name = "Dolor"
                },
                Country = new Country(1)
                {
                    Name = "Amed"
                }
            };
        }

        private async Task LoadData(ApplicationReviewSettingsModel model)
        {
            Task<Response<IList<User>>> usersResponseTask = Client.GetUsersForApplicationReview(model.Id);
            Task<Response<Application>> applicationResponseTask = Client.GetApplicationAsync(model.Id);
            List<Task> tasks = [usersResponseTask, applicationResponseTask];
            while (tasks.Count > 0)
            {
                Task finished = await Task.WhenAny(tasks);
                if (finished == usersResponseTask)
                {
                    Response<IList<User>> usersResponse = await (Task<Response<IList<User>>>)finished;
                    if (usersResponse.StatusCode == HttpStatusCode.OK && usersResponse.Result != null)
                    {
                        model.Reviewers.AddRange(usersResponse.Result);
                    }
                    else
                    {
                        model.ErrorMessages.Add(usersResponse.Message);
                    }
                }
                else if (finished == applicationResponseTask)
                {
                    Response<Application> applicationResponse = await (Task<Response<Application>>)finished;
                    if (applicationResponse.StatusCode == HttpStatusCode.OK && applicationResponse.Result != null)
                    {
                        model.Application = applicationResponse.Result;
                    }
                    else
                    {
                        model.ErrorMessages.Add(applicationResponse.Message);
                    }
                }
                else
                {
                    await finished;
                }

                tasks.Remove(finished);
            }
        }

        private async Task AddReviewers(ApplicationReviewSettingsModel model)
        {
            Dictionary<Task<Response<IList<User>>>, string> tasks = new(model.AddReviewerUserEmails.Count);
            foreach (string email in model.AddReviewerUserEmails)
            {
                tasks.Add(Client.GetUsersAsync(email: email), email);
            }

            while (tasks.Count > 0)
            {
                Task<Response<IList<User>>> finished = await Task.WhenAny(tasks.Keys);
                Response<IList<User>> response = await finished;
                if (response.StatusCode == HttpStatusCode.OK && response.Result != null)
                {
                    string email = tasks[finished];
                    if (response.Result.Count == 1)
                    {
                        SelectionRole role = null;
                        Response<IList<SelectionRole>> selectionRolesResponse = await Client.GetSelectionRolesAsync(applicationId: model.Id);
                        if (selectionRolesResponse.StatusCode == HttpStatusCode.OK && selectionRolesResponse.Result?.Count >= 1)
                        {
                            role = selectionRolesResponse.Result.First();
                        }
                        else if (selectionRolesResponse.StatusCode == HttpStatusCode.OK && selectionRolesResponse.Result?.Count == 0)
                        {
                            Response<SelectionRole> createRoleResponse =
                                await Client.AddSelectionRoleAsync(new SelectionRole(Guid.Empty)
                                {
                                    Name = $"Application {model.Id}",
                                    ApplicationId = model.Id
                                });
                            if (createRoleResponse.StatusCode == HttpStatusCode.Created && createRoleResponse.Result != null)
                            {
                                role = createRoleResponse.Result;
                            }
                            else
                            {
                                model.ErrorMessages.Add(createRoleResponse.Message);
                            }
                        }
                        else
                        {
                            model.ErrorMessages.Add(selectionRolesResponse.Message);
                        }

                        if (role != null)
                        {
                            Response<AssignUserToRole> assignResponse = await Client.AssignUserToRoleAsync(role.Id, response.Result[0].Id);
                            if (assignResponse.StatusCode != HttpStatusCode.Created)
                            {
                                model.ErrorMessages.Add(assignResponse.Message);
                            }
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, $"No or multiple results for email '{email}'. This assignment can only be made through Role Management.");
                    }
                }
                else
                {
                    model.ErrorMessages.Add(response.Message);
                }

                tasks.Remove(finished);
            }

            model.AddReviewerUserEmails.Clear();
        }

        private async Task RemoveReviewer(ApplicationReviewSettingsModel model)
        {
            Response<IList<SelectionRole>> selectionRolesResponse = await Client.GetSelectionRolesAsync(applicationId: model.Id);
            if (selectionRolesResponse.StatusCode == HttpStatusCode.OK && selectionRolesResponse.Result != null)
            {
                List<Task> removeTasks = new(selectionRolesResponse.Result.Count);
                foreach (SelectionRole role in selectionRolesResponse.Result)
                {
                    removeTasks.Add(Client.RemoveUserFromRoleAsync(role.Id, model.RemoveReviewerUserId!.Value));
                }

                await Task.WhenAll(removeTasks);
                model.RemoveReviewerUserId = null;
            }
            else
            {
                model.ErrorMessages.Add(selectionRolesResponse.Message);
            }
        }
    }
}

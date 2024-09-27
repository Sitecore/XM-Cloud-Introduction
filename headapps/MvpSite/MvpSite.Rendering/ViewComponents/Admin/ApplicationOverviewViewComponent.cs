using System.Net;
using Microsoft.AspNetCore.Mvc;
using Mvp.Selections.Client;
using Mvp.Selections.Client.Models;
using Mvp.Selections.Domain;
using MvpSite.Rendering.Models.Admin;
using Sitecore.AspNetCore.SDK.RenderingEngine.Binding;

namespace MvpSite.Rendering.ViewComponents.Admin;

[ViewComponent(Name = ViewComponentName)]
public class ApplicationOverviewViewComponent(IViewModelBinder modelBinder, MvpSelectionsApiClient client)
    : BaseViewComponent(modelBinder, client)
{
    public const string ViewComponentName = "AdminApplicationsOverview";

    public override async Task<IViewComponentResult> InvokeAsync()
    {
        IViewComponentResult result;
        ApplicationsOverviewModel model = await ModelBinder.Bind<ApplicationsOverviewModel>(ViewContext);
        if (model.IsEditing)
        {
            GenerateFakeDataForEdit(model);
            result = View(model);
        }
        else if (model is { RemoveApplicationId: not null, RemoveConfirmed: false })
        {
            Response<Application> applicationResponse = await Client.GetApplicationAsync(model.RemoveApplicationId.Value);
            if (applicationResponse is { StatusCode: HttpStatusCode.OK, Result: not null })
            {
                model.RemoveApplication = applicationResponse.Result;
                result = View("Confirm", model);
            }
            else
            {
                model.ErrorMessages.Add(applicationResponse.Message);
                result = View("~/Views/Shared/_Error.cshtml", model);
            }
        }
        else if (model is { RemoveApplicationId: not null, RemoveConfirmed: true })
        {
            Response<bool> removeApplicationResponse = await Client.RemoveApplicationAsync(model.RemoveApplicationId.Value);
            if (removeApplicationResponse.Result)
            {
                Task.WaitAll(LoadApplications(model), LoadFilterValues(model));
                result = View(model);
            }
            else
            {
                model.ErrorMessages.Add(removeApplicationResponse.Message);
                result = View("~/Views/Shared/_Error.cshtml", model);
            }
        }
        else
        {
            Task.WaitAll(LoadApplications(model), LoadFilterValues(model));
            result = View(model);
        }

        return result;
    }

    private static void GenerateFakeDataForEdit(ApplicationsOverviewModel model)
    {
        model.List.Add(new Application(Guid.NewGuid())
        {
            Selection = new Selection(Guid.NewGuid())
            {
                Year = (short)DateTime.Today.Year
            },
            Country = new Country(1)
            {
                Name = "Country A"
            },
            Applicant = new User(Guid.NewGuid())
            {
                Name = "John Doe"
            },
            Status = ApplicationStatus.Open,
            ModifiedOn = DateTime.Now
        });
        model.List.Add(new Application(Guid.NewGuid())
        {
            Selection = new Selection(Guid.NewGuid())
            {
                Year = (short)DateTime.Today.Year
            },
            Country = new Country(2)
            {
                Name = "Country B"
            },
            Applicant = new User(Guid.NewGuid())
            {
                Name = "Jane Doe"
            },
            Status = ApplicationStatus.Submitted,
            ModifiedOn = DateTime.Today.AddDays(-1)
        });
    }

    private async Task LoadApplications(ApplicationsOverviewModel model)
    {
        Response<IList<Application>> response = await Client.GetApplicationsAsync(null, model.Filter?.ApplicantName, model.Filter?.SelectionId, model.Filter?.CountryId, model.Filter?.Status, model.Page, model.PageSize);
        if (response is { StatusCode: HttpStatusCode.OK, Result: not null })
        {
            model.List.AddRange(response.Result);
        }
    }

    private async Task LoadFilterValues(ApplicationsOverviewModel model)
    {
        Task<Response<IList<Country>>> countriesResponseTask = Client.GetCountriesAsync(1, short.MaxValue);
        Task<Response<IList<Selection>>> selectionResponseTask = Client.GetSelectionsAsync(1, short.MaxValue);
        List<Task> tasks = [countriesResponseTask, selectionResponseTask];
        while (tasks.Count > 0)
        {
            Task finished = await Task.WhenAny(tasks);
            tasks.Remove(finished);
            if (finished == countriesResponseTask)
            {
                Response<IList<Country>> countriesResponse = await (Task<Response<IList<Country>>>)finished;
                if (countriesResponse is { StatusCode: HttpStatusCode.OK, Result: not null })
                {
                    model.Countries.AddRange(countriesResponse.Result);
                }
            }
            else if (finished == selectionResponseTask)
            {
                Response<IList<Selection>> selectionResponse = await (Task<Response<IList<Selection>>>)finished;
                if (selectionResponse is { StatusCode: HttpStatusCode.OK, Result: not null })
                {
                    model.Selections.AddRange(selectionResponse.Result);
                }
            }
            else
            {
                await finished;
                tasks.Remove(finished);
            }
        }
    }
}
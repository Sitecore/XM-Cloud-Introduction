using System.Net;
using Microsoft.AspNetCore.Mvc;
using Mvp.Selections.Api.Model;
using Mvp.Selections.Client;
using Mvp.Selections.Client.Models;
using Mvp.Selections.Domain;
using Mvp.Selections.Domain.Comments;
using MvpSite.Rendering.Models.Admin;
using Sitecore.AspNetCore.SDK.RenderingEngine.Binding;

namespace MvpSite.Rendering.ViewComponents.Admin;

[ViewComponent(Name = ViewComponentName)]
public class ScoreCardDetailViewComponent(IViewModelBinder modelBinder, MvpSelectionsApiClient client)
    : BaseViewComponent(modelBinder, client)
{
    public const string ViewComponentName = "AdminScoreCardDetail";

    public override async Task<IViewComponentResult> InvokeAsync()
    {
        IViewComponentResult result;
        ScoreCardDetailModel model = await ModelBinder.Bind<ScoreCardDetailModel>(ViewContext);
        if (model.IsEditing)
        {
            GenerateFakeDataForEdit(model);
            result = View(model);
        }
        else
        {
            if (model.ApplicationId != Guid.Empty)
            {
                await Task.WhenAll(
                    LoadApplication(model),
                    LoadReviews(model),
                    LoadComments(model));
                await Task.WhenAll(
                    LoadScoreCategories(model),
                    LoadMvpProfile(model),
                    LoadPriorYearsComments(model));
            }

            result = model.ErrorMessages.Count > 0
                ? View("~/Views/Shared/_Error.cshtml", model)
                : View(model);
        }

        return result;
    }

    // ReSharper disable StringLiteralTypo
    private static void GenerateFakeDataForEdit(ScoreCardDetailModel model)
    {
        Random rnd = new();
        Application application = new(Guid.NewGuid())
        {
            Applicant = new User(Guid.NewGuid())
            {
                Name = "Lorem Ipsum"
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

        Score goodScore = new(Guid.NewGuid())
        {
            Name = "Good",
            Value = 10
        };
        Score badScore = new(Guid.NewGuid())
        {
            Name = "Bad",
            Value = -10
        };

        ScoreCategory topCategory1 = new(Guid.NewGuid())
        {
            Name = "Pulvinar",
            Weight = (decimal)rnd.NextDouble()
        };
        ScoreCategory topCategory2 = new(Guid.NewGuid())
        {
            Name = "Aliquam",
            Weight = (decimal)rnd.NextDouble()
        };
        ScoreCategory subCategory1 = new(Guid.NewGuid())
        {
            Name = "Imper",
            Weight = (decimal)rnd.NextDouble(),
            ParentCategory = topCategory1,
            ScoreOptions = [goodScore, badScore]
        };
        ScoreCategory subCategory2 = new(Guid.NewGuid())
        {
            Name = "Proin",
            Weight = (decimal)rnd.NextDouble(),
            ParentCategory = topCategory1,
            ScoreOptions = [goodScore, badScore]
        };
        ScoreCategory subCategory3 = new(Guid.NewGuid())
        {
            Name = "Turpis",
            Weight = (decimal)rnd.NextDouble(),
            ParentCategory = topCategory1,
            ScoreOptions = [goodScore, badScore]
        };
        ScoreCategory subCategory4 = new(Guid.NewGuid())
        {
            Name = "Congue",
            Weight = (decimal)rnd.NextDouble(),
            ParentCategory = topCategory1,
            ScoreOptions = [goodScore, badScore]
        };
        topCategory1.SubCategories.Add(subCategory1);
        topCategory1.SubCategories.Add(subCategory2);
        topCategory2.SubCategories.Add(subCategory3);
        topCategory2.SubCategories.Add(subCategory4);
        model.ScoreCategories.Add(topCategory1);
        model.ScoreCategories.Add(topCategory2);

        goodScore.ScoreCategories.Add(subCategory1);
        goodScore.ScoreCategories.Add(subCategory2);
        goodScore.ScoreCategories.Add(subCategory3);
        goodScore.ScoreCategories.Add(subCategory4);
        badScore.ScoreCategories.Add(subCategory1);
        badScore.ScoreCategories.Add(subCategory2);
        badScore.ScoreCategories.Add(subCategory3);
        badScore.ScoreCategories.Add(subCategory4);

        model.Application = application;
        model.Reviews.Add(new Review(Guid.NewGuid())
        {
            Application = application,
            Comment = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Etiam eget fringilla sapien. Aliquam eget lorem ac magna vestibulum blandit porttitor eu lorem. Suspendisse sollicitudin purus quam. Donec ornare elementum justo, dignissim gravida lectus ultrices non. Sed cursus sodales dolor vel accumsan. Aliquam erat volutpat. Nullam sed erat mattis, luctus est vel, accumsan justo. Phasellus sed lacus turpis. Phasellus ultricies diam ac est congue vulputate.",
            Reviewer = new User(Guid.NewGuid())
            {
                Name = "Pretium Nulla"
            },
            CategoryScores =
            [
                new()
                {
                    ScoreCategoryId = subCategory1.Id,
                    ScoreId = goodScore.Id
                },
                new()
                {
                    ScoreCategoryId = subCategory2.Id,
                    ScoreId = goodScore.Id
                },
                new()
                {
                    ScoreCategoryId = subCategory3.Id,
                    ScoreId = goodScore.Id
                },
                new()
                {
                    ScoreCategoryId = subCategory4.Id,
                    ScoreId = badScore.Id
                }
            ]
        });
        model.Reviews.Add(new Review(Guid.NewGuid())
        {
            Application = application,
            Comment = "Maecenas posuere pretium lorem at aliquet. Nulla ut imperdiet quam. Proin id lobortis eros, condimentum rhoncus nisl. Suspendisse commodo quam nisl, sit amet ultrices orci tempor ac. Cras et tincidunt enim. Aliquam erat volutpat. Proin congue ligula ut lacus mollis, mollis ornare lacus tincidunt. Vestibulum ornare rutrum aliquam. Donec sit amet neque eget quam iaculis porta. Duis fermentum ullamcorper facilisis. Donec facilisis ultricies neque, at accumsan libero. Aliquam massa urna, condimentum ac massa vel, feugiat interdum ex. Cras suscipit pellentesque metus, id luctus metus tincidunt eu. Praesent at nulla mollis, dictum neque eget, maximus ipsum. Interdum et malesuada fames ac ante ipsum primis in faucibus. Praesent volutpat erat eget pulvinar congue.",
            Reviewer = new User(Guid.NewGuid())
            {
                Name = "Urna At"
            },
            CategoryScores =
            [
                new()
                {
                    ScoreCategoryId = subCategory1.Id,
                    ScoreId = goodScore.Id
                },
                new()
                {
                    ScoreCategoryId = subCategory2.Id,
                    ScoreId = badScore.Id
                },
                new()
                {
                    ScoreCategoryId = subCategory3.Id,
                    ScoreId = badScore.Id
                },
                new()
                {
                    ScoreCategoryId = subCategory4.Id,
                    ScoreId = badScore.Id
                }
            ]
        });
    }

    // ReSharper restore StringLiteralTypo
    private async Task LoadScoreCategories(ScoreCardDetailModel model)
    {
        if (model.Application != null)
        {
            Response<IList<ScoreCategory>> scoreCategoriesResponse = await Client.GetScoreCategoriesAsync(model.Application.Selection.Id, model.Application.MvpType.Id);
            if (scoreCategoriesResponse is { StatusCode: HttpStatusCode.OK, Result: not null })
            {
                model.ScoreCategories.AddRange(scoreCategoriesResponse.Result);
            }
            else
            {
                model.ErrorMessages.Add(scoreCategoriesResponse.Message);
            }
        }
    }

    private async Task LoadReviews(ScoreCardDetailModel model)
    {
        Response<IList<Review>> reviewsResponse = await Client.GetReviewsAsync(model.ApplicationId, 1, short.MaxValue);
        if (reviewsResponse is { StatusCode: HttpStatusCode.OK, Result: not null })
        {
            model.Reviews.AddRange(reviewsResponse.Result);
        }
        else
        {
            model.ErrorMessages.Add(reviewsResponse.Message);
        }
    }

    private async Task LoadApplication(ScoreCardDetailModel model)
    {
        Response<Application> applicationResponse = await Client.GetApplicationAsync(model.ApplicationId);
        if (applicationResponse is { StatusCode: HttpStatusCode.OK, Result: not null })
        {
            model.Application = applicationResponse.Result;
        }
        else
        {
            model.ErrorMessages.Add(applicationResponse.Message);
        }
    }

    private async Task LoadComments(ScoreCardDetailModel model)
    {
        Response<IList<ApplicationComment>> getResponse = await Client.GetApplicationCommentsAsync(model.ApplicationId);
        if (getResponse is { StatusCode: HttpStatusCode.OK, Result: not null })
        {
            model.Comments.AddRange(getResponse.Result);
        }
        else
        {
            model.ErrorMessages.Add(getResponse.Message);
        }
    }

    private async Task LoadMvpProfile(ScoreCardDetailModel model)
    {
        if (model.Application != null)
        {
            Response<MvpProfile> mvpProfileResponse = await Client.GetMvpProfileAsync(model.Application.Applicant.Id);
            if (mvpProfileResponse is { StatusCode: HttpStatusCode.OK, Result: not null })
            {
                model.MvpProfile = mvpProfileResponse.Result;
            }
            else if (mvpProfileResponse.StatusCode != HttpStatusCode.NotFound)
            {
                model.ErrorMessages.Add(mvpProfileResponse.Message);
            }
        }
    }

    private async Task LoadPriorYearsComments(ScoreCardDetailModel model)
    {
        if (model.Application != null)
        {
            Response<IList<Application>> applicationsResponse = await Client.GetApplicationsAsync(model.Application.Applicant.Id);
            if (applicationsResponse is { StatusCode: HttpStatusCode.OK, Result: not null })
            {
                foreach (Application application in applicationsResponse.Result.Where(a => a.Selection.Year < model.Application.Selection.Year))
                {
                    Response<IList<ApplicationComment>> commentsResponse = await Client.GetApplicationCommentsAsync(application.Id);
                    if (commentsResponse is { StatusCode: HttpStatusCode.OK, Result: not null })
                    {
                        if (commentsResponse.Result.Count > 0)
                        {
                            model.PriorYearsComments.Add(application.Selection.Year, commentsResponse.Result);
                        }
                    }
                    else
                    {
                        model.ErrorMessages.Add(commentsResponse.Message);
                    }
                }
            }
            else
            {
                model.ErrorMessages.Add(applicationsResponse.Message);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mvp.Feature.Selections.Models.Admin;
using Mvp.Selections.Client;
using Mvp.Selections.Client.Models;
using Mvp.Selections.Domain;
using Mvp.Selections.Domain.Comments;
using Sitecore.AspNetCore.SDK.RenderingEngine.Binding;

namespace Mvp.Feature.Selections.ViewComponents.Admin
{
    [ViewComponent(Name = ViewComponentName)]
    public class AwardViewComponent : BaseViewComponent
    {
        public const string ViewComponentName = "AdminAward";

        public AwardViewComponent(IViewModelBinder modelBinder, MvpSelectionsApiClient client)
            : base(modelBinder, client)
        {
        }

        public override async Task<IViewComponentResult> InvokeAsync()
        {
            IViewComponentResult result;
            AwardModel model = await ModelBinder.Bind<AwardModel>(ViewContext);
            if (model.IsEditing)
            {
                GenerateFakeDataForEdit(model);
                result = View(model);
            }
            else
            {
                if (model.IsRemove)
                {
                    await LoadTitle(model);
                    if (model.IsConfirmed)
                    {
                        await RemoveTitle(model);
                    }
                }
                else
                {
                    await Task.WhenAll(
                        LoadApplication(model),
                        LoadMvpTypes(model),
                        LoadComments(model));
                    if (model.IsConfirmed)
                    {
                        await AwardTitle(model);
                    }
                }

                result = model.ErrorMessages.Count > 0
                    ? View("~/Views/Shared/_Error.cshtml", model)
                    : model.IsConfirmed ? View("Success", model) : View(model);
            }

            return result;
        }

        private static void GenerateFakeDataForEdit(AwardModel model)
        {
            MvpType loremMvp = new (1)
            {
                Name = "Lorem"
            };

            MvpType ipsumMvp = new (2)
            {
                Name = "Ipsum"
            };

            Application dolorApplication = new (Guid.NewGuid())
            {
                Applicant = new User(Guid.NewGuid())
                {
                    Name = "Dolor Amed"
                },
                Country = new Country(1)
                {
                    Name = "Consequetur"
                },
                Selection = new Selection(Guid.NewGuid())
                {
                    Year = (short)DateTime.Now.AddYears(15).Year
                },
                MvpType = loremMvp,
                Status = ApplicationStatus.Submitted
            };

            model.Title = new (Guid.NewGuid())
            {
                Warning = "Donec varius, leo eget iaculis placerat, sapien orci iaculis nulla, ut facilisis eros arcu a ex.",
                MvpType = loremMvp,
                Application = dolorApplication
            };

            model.Application = dolorApplication;

            model.MvpTypes.Add(loremMvp);
            model.MvpTypes.Add(ipsumMvp);

            model.Comments.Add(new ApplicationComment(Guid.NewGuid())
            {
                Application = dolorApplication,
                User = new User(Guid.NewGuid())
                {
                    Name = "Commentor 1"
                },
                Value = "Vestibulum tincidunt elementum nisl, et pulvinar justo ultricies ac. Nullam egestas luctus vehicula. Donec elementum dapibus magna, a laoreet velit rutrum ac.",
                ModifiedOn = DateTime.Now.AddDays(-2)
            });
            model.Comments.Add(new ApplicationComment(Guid.NewGuid())
            {
                Application = dolorApplication,
                User = new User(Guid.NewGuid())
                {
                    Name = "Commentor 2"
                },
                Value = "Proin et eros efficitur, ornare lorem et, tempus erat. Pellentesque vitae tempor elit. Sed volutpat diam volutpat nulla vestibulum tincidunt. Aliquam a est a neque varius mattis a id sapien. Morbi ex libero, feugiat in leo in, vehicula sodales lacus.",
                ModifiedOn = DateTime.Now.AddDays(-5)
            });
            model.Comments.Add(new ApplicationComment(Guid.NewGuid())
            {
                Application = dolorApplication,
                User = new User(Guid.NewGuid())
                {
                    Name = "Commentor 1"
                },
                Value = "Donec arcu neque, aliquam eu mi et, mattis accumsan magna. Praesent dui ante, tincidunt rutrum congue ac, facilisis sit amet nisi. In hac habitasse platea dictumst.",
                ModifiedOn = DateTime.Now.AddDays(-9)
            });
        }

        private async Task AwardTitle(AwardModel model)
        {
            Title newTitle = new (Guid.Empty)
            {
                MvpType = model.MvpTypes.Single(t => t.Id == model.MvpTypeId),
                Application = model.Application,
                Warning = model.Warning
            };
            Response<Title> addResponse = await Client.AddTitleAsync(newTitle);
            if (addResponse.StatusCode == HttpStatusCode.Created && addResponse.Result != null)
            {
                model.Title = addResponse.Result;
            }
            else
            {
                model.ErrorMessages.Add(addResponse.Message);
            }
        }

        private async Task RemoveTitle(AwardModel model)
        {
            Response<bool> removeResponse = await Client.RemoveTitleAsync(model.TitleId);
            if (removeResponse.StatusCode != HttpStatusCode.NoContent)
            {
                model.ErrorMessages.Add(removeResponse.Message);
            }
        }

        private async Task LoadMvpTypes(AwardModel model)
        {
            Response<IList<MvpType>> mvpTypesResponse = await Client.GetMvpTypesAsync(1, short.MaxValue);
            if (mvpTypesResponse.StatusCode == HttpStatusCode.OK && mvpTypesResponse.Result != null)
            {
                model.MvpTypes.AddRange(mvpTypesResponse.Result);
            }
            else
            {
                model.ErrorMessages.Add(mvpTypesResponse.Message);
            }
        }

        private async Task LoadApplication(AwardModel model)
        {
            Response<Application> applicationResponse = await Client.GetApplicationAsync(model.ApplicationId);
            if (applicationResponse.StatusCode == HttpStatusCode.OK && applicationResponse.Result != null)
            {
                model.Application = applicationResponse.Result;
            }
            else
            {
                model.ErrorMessages.Add(applicationResponse.Message);
            }
        }

        private async Task LoadTitle(AwardModel model)
        {
            Response<Title> titleResponse = await Client.GetTitleAsync(model.TitleId);
            if (titleResponse.StatusCode == HttpStatusCode.OK && titleResponse.Result != null)
            {
                model.Title = titleResponse.Result;
            }
            else
            {
                model.ErrorMessages.Add(titleResponse.Message);
            }
        }

        private async Task LoadComments(AwardModel model)
        {
            Response<IList<ApplicationComment>> getResponse = await Client.GetApplicationCommentsAsync(model.ApplicationId);
            if (getResponse.StatusCode == HttpStatusCode.OK && getResponse.Result != null)
            {
                model.Comments.AddRange(getResponse.Result);
            }
            else
            {
                model.ErrorMessages.Add(getResponse.Message);
            }
        }
    }
}

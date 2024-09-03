using System;
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
    public class ApplicationCommentViewComponent : BaseViewComponent
    {
        public const string ViewComponentName = "AdminApplicationComment";

        public ApplicationCommentViewComponent(IViewModelBinder modelBinder, MvpSelectionsApiClient client)
            : base(modelBinder, client)
        {
        }

        public override async Task<IViewComponentResult> InvokeAsync()
        {
            ApplicationCommentModel model = await ModelBinder.Bind<ApplicationCommentModel>(ViewContext);
            if (!string.IsNullOrWhiteSpace(model.Comment))
            {
                await PostComment(model);
                model.Comment = null;
                ModelState.Clear();
            }

            return model.ErrorMessages.Count > 0 ? View("~/Views/Shared/_Error.cshtml", model) : View(model);
        }

        private async Task PostComment(ApplicationCommentModel model)
        {
            Response<User> getUserResponse = await Client.GetCurrentUserAsync();
            if (getUserResponse.StatusCode == HttpStatusCode.OK && getUserResponse.Result != null)
            {
                ApplicationComment newComment = new (Guid.Empty)
                {
                    Application = new Application(model.ApplicationId),
                    User = getUserResponse.Result,
                    Value = model.Comment
                };
                Response<ApplicationComment> addResponse = await Client.AddApplicationCommentAsync(model.ApplicationId, newComment);
                if (addResponse.StatusCode != HttpStatusCode.Created)
                {
                    model.ErrorMessages.Add(addResponse.Message);
                }
            }
            else
            {
                model.ErrorMessages.Add(getUserResponse.Message);
            }
        }
    }
}

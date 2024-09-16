using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Mvp.Feature.People.Models.Profile;
using Mvp.Selections.Client;
using Sitecore.AspNetCore.SDK.RenderingEngine.Binding;
using System.Threading.Tasks;
using Mvp.Selections.Api.Model;
using Mvp.Selections.Client.Models;
using Mvp.Selections.Domain;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Mvp.Feature.People.Configuration;
using Mvp.Feature.People.Extensions;

namespace Mvp.Feature.People.ViewComponents
{
    [ViewComponent(Name = ViewComponentName)]
    public class ProfileViewComponent(IViewModelBinder modelBinder, MvpSelectionsApiClient client, IMemoryCache cache, IOptions<MvpPeopleOptions> options)
        : ViewComponent
    {
        public const string ViewComponentName = "Profile";

        private const string ProfileCacheKeyPrefix = "MvpProfile_";

        private readonly MvpPeopleOptions _options = options.Value;

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ProfileViewModel model = await modelBinder.Bind<ProfileViewModel>(ViewContext);
            if (model.IsEditing)
            {
                GenerateFakeDataForEdit(model);
            }
            else
            {
                await LoadProfile(model);
            }

            return model.ErrorMessages.Count > 0 ? View("~/Views/Shared/Components/Profile/Error.cshtml", model) :
                model.Mvp != null ? View(model) : View("~/Views/Shared/Components/Profile/NotFound.cshtml", model);
        }

        private static void GenerateFakeDataForEdit(ProfileViewModel model)
        {
            model.Id = Guid.NewGuid();
            model.Mvp = new MvpProfile
            {
                Name = "Lorem Ipsum",
                Country = new Country(1)
                {
                    Name = "Dolor"
                },
                ProfileLinks = [
                    new ProfileLink(Guid.NewGuid())
                    {
                        Name = "Sitecore X",
                        Type = ProfileLinkType.Twitter,
                        Uri = new Uri("https://twitter.com/Sitecore")
                    },
                    new ProfileLink(Guid.NewGuid())
                    {
                        Name = "Sitecore Blog",
                        Type = ProfileLinkType.Blog,
                        Uri = new Uri("https://www.sitecore.com/knowledge-center/blog")
                    },
                    new ProfileLink(Guid.NewGuid())
                    {
                        Name = "Sitecore YouTube",
                        Type = ProfileLinkType.Youtube,
                        Uri = new Uri("https://www.youtube.com/c/sitecore")
                    },
                    new ProfileLink(Guid.NewGuid())
                    {
                        Name = "Sitecore LinkedIn",
                        Type = ProfileLinkType.LinkedIn,
                        Uri = new Uri("https://linkedin.com/company/sitecore")
                    },
                    new ProfileLink(Guid.NewGuid())
                    {
                        Name = "Sitecore Community",
                        Type = ProfileLinkType.Community,
                        Uri = new Uri("https://community.sitecore.com/community")
                    },
                    new ProfileLink(Guid.NewGuid())
                    {
                        Name = "Sitecore GitHub",
                        Type = ProfileLinkType.Github,
                        Uri = new Uri("https://github.com/Sitecore")
                    },
                    new ProfileLink(Guid.NewGuid())
                    {
                        Name = "Sitecore StackExchange",
                        Type = ProfileLinkType.StackExchange,
                        Uri = new Uri("https://sitecore.stackexchange.com/")
                    },
                    new ProfileLink(Guid.NewGuid())
                    {
                        Name = "Sitecore Other",
                        Type = ProfileLinkType.Other,
                        Uri = new Uri("https://www.sitecore.com/")
                    }
                ],
                PublicContributions = [
                    new Contribution(Guid.NewGuid())
                    {
                        Name = "Blah Blah Blah",
                        Type = ContributionType.BlogPost,
                        Date = DateTime.Today.AddDays(-14),
                        IsPublic = true,
                        Description = "The most amazing fake blogpost!",
                        Uri = new Uri("https://www.sitecore.com"),
                        RelatedProducts = [new Product(1){ Name = "MVP Website" }]
                    }
                ],
                Titles = [
                    new Title(Guid.NewGuid())
                    {
                        MvpType = new MvpType(1)
                        {
                            Name = "Fake Technology"
                        },
                        Application = new Application(Guid.NewGuid())
                        {
                            Selection = new Selection(Guid.NewGuid())
                            {
                                Year = (short)(DateTime.Today.Year - 1)
                            },
                            Country = new Country(1)
                            {
                                Name = "Amet"
                            }
                        }
                    }
                ]
            };
        }

        private async Task LoadProfile(ProfileViewModel model)
        {
            if (model.Id.HasValue)
            {
                model.Mvp = await GetProfileById(model.Id.Value, model.ErrorMessages);
            }
            else if (HttpContext.Request.Path.HasValue)
            {
                string? lastSegment = HttpContext.Request.Path.Value.Split('/', StringSplitOptions.RemoveEmptyEntries)
                    .LastOrDefault();
                if (!string.IsNullOrWhiteSpace(lastSegment))
                {
                    model.Mvp = await GetProfileByName(lastSegment.DecodeSpaces(), model.ErrorMessages);
                }
            }
        }

        private async Task<MvpProfile?> GetProfileByName(string name, List<string> errorMessages)
        {
            MvpProfile? result = null;
            string cacheKey = $"{ProfileCacheKeyPrefix}{name}_id";
            if (cache.TryGetValue(cacheKey, out Guid? id) && id.HasValue)
            {
                result = await GetProfileById(id.Value, errorMessages);
            }
            else
            {
                Response<SearchResult<MvpProfile>> response = await client.SearchMvpProfileAsync(name);
                if (response is { StatusCode: HttpStatusCode.OK, Result.Results.Count: 1 })
                {
                    id = response.Result.Results.Single().Id;
                    result = await GetProfileById(id.Value, errorMessages);
                    cache.Set(cacheKey, id, TimeSpan.FromSeconds(_options.ProfileCachedSeconds));
                }
                else if (response is { StatusCode: HttpStatusCode.OK, Result.Results.Count: > 1 })
                {
                    errorMessages.Add($"Found '{response.Result.Results.Count}' results for '{name}', unable to display. Please access by ID instead.");
                }
                else
                {
                    errorMessages.Add(response.Message);
                }
            }

            return result;
        }

        private async Task<MvpProfile?> GetProfileById(Guid id, List<string> errorMessages)
        {
            MvpProfile? result = null;
            string cacheKey = $"{ProfileCacheKeyPrefix}{id:N}";
            if (cache.TryGetValue(cacheKey, out MvpProfile? profile))
            {
                result = profile;
            }
            else
            {
                Response<MvpProfile> response = await client.GetMvpProfileAsync(id);
                if (response is { StatusCode: HttpStatusCode.OK, Result: not null })
                {
                    result = response.Result;
                    cache.Set(cacheKey, response.Result, TimeSpan.FromSeconds(_options.ProfileCachedSeconds));
                }
                else
                {
                    errorMessages.Add(response.Message);
                }
            }

            return result;
        }
    }
}

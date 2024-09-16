﻿using System;
using Mvp.Selections.Domain;
using Sitecore.AspNetCore.SDK.LayoutService.Client.Response.Model.Fields;

namespace Mvp.Feature.Selections.Models.Admin
{
    public class UsersOverviewModel : ListModel<User>
    {
        public TextField TitleLabel { get; set; }

        public TextField IdentifierTableHeader { get; set; }

        public TextField NameTableHeader { get; set; }

        public TextField EmailTableHeader { get; set; }

        public TextField CountryTableHeader { get; set; }

        public HyperLinkField EditLink { get; set; }

        public Guid? RemoveUserId { get; set; }

        public User RemoveUser { get; set; }

        public TextField ConfirmMessageLabelFormat { get; set; }

        public TextField ConfirmLabel { get; set; }

        public bool RemoveConfirmed { get; set; } = false;
    }
}

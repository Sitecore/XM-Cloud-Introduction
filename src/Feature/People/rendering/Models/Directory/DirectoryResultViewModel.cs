using System;

namespace Mvp.Feature.People.Models.Directory
{
    public class DirectoryResultViewModel
    {
        public string Name { get; set; }

        public string Type { get; set; }

        public string Country { get; set; }

        public Uri Image { get; set; }

        public string Year { get; set; }

        public Uri ProfileUri { get; set; }
    }
}

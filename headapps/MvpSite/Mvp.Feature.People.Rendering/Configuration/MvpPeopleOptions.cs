namespace Mvp.Feature.People.Configuration
{
    public class MvpPeopleOptions
    {
        public const string MvpPeople = "MvpPeople";

        public int ProfileCachedSeconds { get; set; } = 300;

        public int SearchCachedSeconds { get; set; } = 300;
    }
}

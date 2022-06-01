using GraphQL.Client.Abstractions;

namespace Mvp.Feature.People.GraphQL
{
    public interface IGraphQLClientFactory
    {
        public IGraphQLClient CreateGraphQlClient();
    }
}

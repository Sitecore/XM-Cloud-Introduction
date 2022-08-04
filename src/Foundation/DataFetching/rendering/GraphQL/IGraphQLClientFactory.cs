using GraphQL.Client.Abstractions;

namespace Mvp.Foundation.DataFetching.GraphQL
{
    public interface IGraphQLClientFactory
    {
        public IGraphQLClient CreateGraphQlClient();
    }
}

namespace Mvp.Foundation.DataFetching.GraphQL
{
    public interface IGraphQLRequestBuilder
    {
        public GraphQLHttpRequestWithHeaders BuildRequest(string query, object variables);
    }
}

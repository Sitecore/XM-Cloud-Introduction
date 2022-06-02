namespace Mvp.Feature.People.GraphQL
{
    public interface IGraphQLRequestBuilder
    {
        public GraphQLHttpRequestWithHeaders BuildRequest(string endCursor);
    }
}

import { DictionaryService, GraphQLDictionaryService } from '@sitecore-jss/sitecore-jss-nextjs';
import clientFactory from 'lib/graphql-client-factory';

/**
 * Factory responsible for creating a DictionaryService instance
 */
export class DictionaryServiceFactory {
  /**
   * @param {string} siteName site name
   * @returns {DictionaryService} service instance
   */
  create(siteName: string): DictionaryService {
    return new GraphQLDictionaryService({
      siteName,
      clientFactory,
      /*
        GraphQL endpoint may reach its rate limit with the amount of requests it receives and throw a rate limit error.
        GraphQL Dictionary and Layout Services can handle rate limit errors from server and attempt a retry on requests.
        For this, specify the number of 'retries' the GraphQL client will attempt.
        By default it is set to 3. You can disable it by configuring it to 0 for this service.
      */
      retries: (process.env.GRAPH_QL_SERVICE_RETRIES &&
        parseInt(process.env.GRAPH_QL_SERVICE_RETRIES, 10)) as number,
      useSiteQuery: true,
    });
  }
}

/** DictionaryServiceFactory singleton */
export const dictionaryServiceFactory = new DictionaryServiceFactory();

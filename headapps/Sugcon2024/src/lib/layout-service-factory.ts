import {
  LayoutService,
  GraphQLLayoutService
} from '@sitecore-jss/sitecore-jss-nextjs';
import clientFactory from 'lib/graphql-client-factory';

/**
 * Factory responsible for creating a LayoutService instance
 */
export class LayoutServiceFactory {
  /**
   * @param {string} siteName site name
   * @returns {LayoutService} service instance
   */
  create(siteName: string): LayoutService {
    return new GraphQLLayoutService({
 siteName, 
  clientFactory, 
   retries: (process.env.GRAPH_QL_SERVICE_RETRIES && 
     parseInt(process.env.GRAPH_QL_SERVICE_RETRIES, 10)) as number, 
 });
  }
}

/** LayoutServiceFactory singleton */
export const layoutServiceFactory = new LayoutServiceFactory();

import { SitecoreClient } from '@sitecore-content-sdk/nextjs/client';
import scConfig from 'sitecore.config';

const client = new SitecoreClient({
  ...scConfig,
});

export default client;

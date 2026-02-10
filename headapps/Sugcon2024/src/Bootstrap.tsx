'use client';
import { useEffect, JSX } from 'react';
import { CloudSDK } from '@sitecore-cloudsdk/core/browser';
import '@sitecore-cloudsdk/events/browser';
import config from 'sitecore.config';

const Bootstrap = ({
  siteName,
  isPreviewMode,
}: {
  siteName: string;
  isPreviewMode: boolean;
}): JSX.Element | null => {
  useEffect(() => {
    if (process.env.NODE_ENV === 'development') {
      console.debug('Browser Events SDK is not initialized in development environment');
      return;
    }

    if (isPreviewMode) {
      console.debug('Browser Events SDK is not initialized in edit and preview modes');
      return;
    }

    if (config.api.edge?.clientContextId) {
      CloudSDK({
        sitecoreEdgeUrl: config.api.edge.edgeUrl,
        sitecoreEdgeContextId: config.api.edge.clientContextId,
        siteName: siteName || config.defaultSite,
        enableBrowserCookie: true,
        cookieDomain: window.location.hostname.replace(/^www\./, ''),
      })
        .addEvents()
        .initialize();
    } else {
      console.error('Client Edge API settings missing from configuration');
    }
  }, [siteName, isPreviewMode]);

  return null;
};

export default Bootstrap;

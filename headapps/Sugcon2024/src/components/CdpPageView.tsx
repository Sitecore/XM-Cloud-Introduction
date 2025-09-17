import {
  CdpHelper,
  LayoutServicePageState,
  SiteInfo,
  useSitecoreContext,
} from '@sitecore-jss/sitecore-jss-nextjs';
import { useCallback, useEffect } from 'react';
import config from 'temp/config';
import { init } from '@sitecore/engage';
import { siteResolver } from 'lib/site-resolver';

/**
 * This is the CDP page view component.
 * It uses the Sitecore Engage SDK to enable page view events on the client-side.
 * See Sitecore Engage SDK documentation for details.
 * https://www.npmjs.com/package/@sitecore/engage
 */
const resolvePointOfSale = (site: SiteInfo, language: string): string => {
  return `${site.name}_${language}`;
};

const CdpPageView = (): JSX.Element => {
  const {
    sitecoreContext: { pageState, route, variantId, site },
  } = useSitecoreContext();

  /**
   * Creates a page view event using the Sitecore Engage SDK.
   */
  const createPageView = useCallback(
    async (page: string, language: string, site: SiteInfo, pageVariantId: string) => {
      const pointOfSale = resolvePointOfSale(site, language);
      const engage = await init({
        clientKey: process.env.NEXT_PUBLIC_CDP_CLIENT_KEY || '',
        targetURL: process.env.NEXT_PUBLIC_CDP_TARGET_URL || '',
        cookieDomain: window.location.hostname.replace(/^www\./, ''),
        forceServerCookieMode: false,
      });
      engage.pageView({
        channel: 'WEB',
        currency: 'USD',
        pointOfSale,
        page,
        pageVariantId,
        language,
      });
    },
    [] // Empty dependency array, so it will only be created once.
  );

  /**
   * Determines if the page view events should be turned off.
   * IMPORTANT: You should implement based on your cookie consent management solution of choice.
   * By default it is disabled in development mode
   */
  const disabled = () => {
    return (
      !process.env.NEXT_PUBLIC_CDP_CLIENT_KEY ||
      !process.env.NEXT_PUBLIC_CDP_TARGET_URL ||
      !process.env.NEXT_PUBLIC_PERSONALIZE_SCOPE ||
      process.env.NODE_ENV === 'development'
    );
  };

  useEffect(() => {
    // Do not create events in editing or preview mode or if missing route data
    if (pageState !== LayoutServicePageState.Normal || !route?.itemId) {
      return;
    }
    // Do not create events if disabled (e.g. we don't have consent)
    if (disabled()) {
      return;
    }

    const siteInfo = siteResolver.getByName(site?.name || config.sitecoreSiteName);
    const language = route.itemLanguage || config.defaultLanguage;
    const scope = process.env.NEXT_PUBLIC_PERSONALIZE_SCOPE;

    const pageVariantId = CdpHelper.getPageVariantId(
      route.itemId,
      language,
      variantId as string,
      scope
    );
    createPageView(route.name, language, siteInfo, pageVariantId);
  }, [pageState, route, variantId, site, createPageView]);

  return <></>;
};

export default CdpPageView;

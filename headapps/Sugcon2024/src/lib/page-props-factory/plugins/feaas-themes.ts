import { SitecorePageProps } from 'lib/page-props';
import { getDesignLibraryStylesheetLinks } from '@sitecore-jss/sitecore-jss-nextjs';
import { Plugin } from '..';

class FEeaSThemesPlugin implements Plugin {
  order = 2;

  async exec(props: SitecorePageProps) {
    const contextId = process.env.SITECORE_EDGE_CONTEXT_ID;
    if (!contextId) {
      throw new Error('Environment variable SITECORE_EDGE_CONTEXT_ID is required but not set.');
    }
    // Collect FEAAS themes
    props.headLinks.push(...getDesignLibraryStylesheetLinks(props.layoutData, contextId));

    return props;
  }
}

export const feaasThemesPlugin = new FEeaSThemesPlugin();

import { SitecorePageProps } from 'lib/page-props';
import { getDesignLibraryStylesheetLinks } from '@sitecore-jss/sitecore-jss-nextjs';
import { Plugin } from '..';

class FEeaSThemesPlugin implements Plugin {
  order = 2;

  async exec(props: SitecorePageProps) {
    // Collect FEAAS themes
    props.headLinks.push(
      ...getDesignLibraryStylesheetLinks(
        props.layoutData,
        process.env.SITECORE_EDGE_CONTEXT_ID || ''
      )
    );

    return props;
  }
}

export const feaasThemesPlugin = new FEeaSThemesPlugin();

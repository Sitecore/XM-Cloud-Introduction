import { JSX } from "react";
import { Field, Page, ImageField, AppPlaceholder, DesignLibraryApp } from "@sitecore-content-sdk/nextjs";
import Scripts from "src/Scripts";
import SitecoreStyles from "components/content-sdk/SitecoreStyles";
import componentMap from ".sitecore/component-map";
import Head from "next/head";
import { GoogleAnalytics } from '@next/third-parties/google';
import { Header } from "components/templates/header/Header";
import { HeaderMeta } from "components/templates/header/HeaderMeta";
import { Footer } from "components/templates/footer/Footer";

interface LayoutProps {
  page: Page;
}

export interface RouteFields {
  [key: string]: unknown;
  Title?: Field;
  MetaDescription?: Field;
  MetaKeywords?: Field;
  OGTitle?: Field;
  OGDescription?: Field;
  OGType?: Field;
  OGImage?: ImageField;
}

const Layout = ({ page }: LayoutProps): JSX.Element => {
  const { layout, mode } = page;
  const { route } = layout.sitecore;
  const fields = route?.fields as RouteFields;
  const mainClassPageEditing = mode.isEditing ? "editing-mode" : "prod-mode";

  return (
    <>
      <Scripts />
      <SitecoreStyles layoutData={layout} />
      <Head>
        <title>{fields?.Title?.value?.toString() || 'Page'}</title>

        <HeaderMeta fields={fields} />

        <link rel="icon" href={`/favicon.ico`} />
      </Head>

      {process.env.NEXT_PUBLIC_GTAG && <GoogleAnalytics gaId={process.env.NEXT_PUBLIC_GTAG} />}

      <div className={mainClassPageEditing}>
        {mode.isDesignLibrary ? (
          route && (
            <DesignLibraryApp
              page={page}
              rendering={route}
              componentMap={componentMap}
              loadServerImportMap={() => import(".sitecore/import-map.server")}
            />
          )
        ) : (
          <>
            {/* root placeholder for the app, which we add components to using route data */}
            <Header page={page}
              componentMap={componentMap}
              route={route} />
            <main>
              <div id="content">
                {route && (
                  <AppPlaceholder
                    page={page}
                    componentMap={componentMap}
                    name="headless-main"
                    rendering={route}
                  />
                )}
              </div>
            </main>
            <Footer page={page}
              componentMap={componentMap}
              route={route} />
          </>
        )}
      </div>
    </>
  );
};

export default Layout;

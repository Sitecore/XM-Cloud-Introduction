'use client';

import { JSX } from "react";
import { Field, Page, ImageField, AppPlaceholder, DesignLibraryApp } from "@sitecore-content-sdk/nextjs";
import Scripts from "src/Scripts";
import SitecoreStyles from "components/content-sdk/SitecoreStyles";
import componentMap from ".sitecore/component-map";
import Head from "next/head";
import { usePathname } from "next/navigation";
import { ChakraProvider } from "@chakra-ui/react";
import theme from './Theme';
import { GoogleAnalytics } from '@next/third-parties/google';
import { Header } from "components/templates/header/Header";
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

  const origin = typeof window !== 'undefined' && window.location.origin ? window.location.origin : '';

  const pathname = usePathname();
  const canonicalUrl = (origin + (pathname === '/' ? '' : pathname)).split('?')[0];


  const mainClassPageEditing = mode.isEditing ? "editing-mode" : "prod-mode";

  return (
    <>
      <Scripts />
      <SitecoreStyles layoutData={layout} />
      <Head>
        <title>{fields?.Title?.value?.toString() || 'Page'}</title>

        <meta
          key="metadesc"
          name="description"
          content={fields?.MetaDescription?.value?.toString()}
        />
        <meta
          key="metakeywords"
          name="keywords"
          content={fields?.MetaKeywords?.value?.toString()}
        />
        <meta
          key="metaviewport"
          name="viewport"
          content="width=device-width, height=device-height"
        />
        <link rel="canonical" href={canonicalUrl} />
        <meta property="og:title" content={fields?.OGTitle?.value?.toString()} />
        <meta property="og:url" content={canonicalUrl} />
        <meta property="og:image" content={fields?.OGImage?.value?.src?.toString()} />
        <meta property="og:type" content={fields?.OGType?.value?.toString()} />
        <meta property="og:description" content={fields?.OGDescription?.value?.toString()} />

        <link rel="icon" href={`/favicon.ico`} />
      </Head>

      {process.env.NEXT_PUBLIC_GTAG && <GoogleAnalytics gaId={process.env.NEXT_PUBLIC_GTAG} />}

      <div className={mainClassPageEditing}>
        <ChakraProvider theme={theme}>
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
        </ChakraProvider>
      </div>
    </>
  );
};

export default Layout;

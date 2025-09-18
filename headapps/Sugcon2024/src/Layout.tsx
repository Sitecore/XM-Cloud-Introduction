/**
 * This Layout is needed for Starter Kit.
 */
import React from 'react';
import Head from 'next/head';
import { useRouter } from 'next/router';
import {
  Placeholder,
  LayoutServiceData,
  Field,
  HTMLLink,
  ImageField,
} from '@sitecore-jss/sitecore-jss-nextjs';
import { Default as Header } from 'template/Header';
import { Default as Footer } from 'template/Footer';

import Scripts from 'src/Scripts';
import { ChakraProvider } from '@chakra-ui/react';
import theme from './Theme';
import { GoogleAnalytics } from '@next/third-parties/google';

interface LayoutProps {
  layoutData: LayoutServiceData;
  headLinks: HTMLLink[];
}

interface RouteFields {
  [key: string]: unknown;
  Title?: Field;
  MetaDescription?: Field;
  MetaKeywords?: Field;
  OGTitle?: Field;
  OGDescription?: Field;
  OGType?: Field;
  OGImage?: ImageField;
}

const Layout = ({ layoutData, headLinks }: LayoutProps): JSX.Element => {
  const { route } = layoutData.sitecore;
  const fields = route?.fields as RouteFields;
  const origin =
    typeof window !== 'undefined' && window.location.origin ? window.location.origin : '';
  const router = useRouter();
  const canonicalUrl = (origin + (router.asPath === '/' ? '' : router.asPath)).split('?')[0];

  return (
    <>
      <Scripts />
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

        {headLinks.map((headLink) => (
          <link rel={headLink.rel} key={headLink.href} href={headLink.href} />
        ))}
      </Head>

      {process.env.NEXT_PUBLIC_GTAG && <GoogleAnalytics gaId={process.env.NEXT_PUBLIC_GTAG} />}

      <ChakraProvider theme={theme}>
        {/* root placeholder for the app, which we add components to using route data */}
        <Header route={route} />
        <main>
          <div id="content">{route && <Placeholder name="headless-main" rendering={route} />}</div>
        </main>
        <Footer route={route} />
      </ChakraProvider>
    </>
  );
};

export default Layout;

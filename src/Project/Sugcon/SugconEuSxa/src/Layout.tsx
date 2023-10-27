/**
 * This Layout is needed for Starter Kit.
 */
import React from 'react';
import Head from 'next/head';
import {
  Placeholder,
  getPublicUrl,
  LayoutServiceData,
  Field,
  ImageField,
} from '@sitecore-jss/sitecore-jss-nextjs';
import Scripts from 'src/Scripts';

// Prefix public assets with a public URL to enable compatibility with Sitecore Experience Editor.
// If you're not supporting the Experience Editor, you can remove this.
const publicUrl = getPublicUrl();

interface LayoutProps {
  layoutData: LayoutServiceData;
}

interface RouteFields {
  [key: string]: unknown;
  Title?: Field;
  MetaDescription?: Field;
  MetaKeywords?: Field;
  OgTitle?: Field;
  OgDescription?: Field;
  OgImage?: ImageField;
  OgType?: Field;
}

const Layout = ({ layoutData }: LayoutProps): JSX.Element => {
  const { route } = layoutData.sitecore;
  const fields = route?.fields as RouteFields;
  const isPageEditing = layoutData.sitecore.context.pageEditing;
  const mainClassPageEditing = isPageEditing ? 'editing-mode' : 'prod-mode';

  return (
    <>
      <Scripts />
      <Head>
        <title>
          {fields?.Title?.value
            ? 'SUGCON Europe - ' + fields.Title.value.toString()
            : 'SUGCON Europe'}
        </title>
        <link rel="icon" href={`${publicUrl}/favicon.ico`} />
        {/* Meta Tags */}
        {fields?.MetaDescription?.value ? (
          <meta name="description" content={`${fields?.MetaDescription?.value}`} />
        ) : (
          ''
        )}

        {fields?.MetaKeywords?.value ? (
          <meta name="keywords" content={`${fields?.MetaKeywords?.value}`} />
        ) : (
          ''
        )}
        {/* Open Graph Tags */}
        {fields?.OgTitle?.value ? (
          <meta name="title" property="og:title" content={`${fields?.OgTitle?.value}`} />
        ) : (
          ''
        )}

        {fields?.OgDescription?.value ? (
          <meta property="og:description" content={`${fields?.OgDescription?.value}`} />
        ) : (
          ''
        )}

        {fields?.OgImage?.value?.src ? (
          <meta name="image" property="og:image" content={`${fields?.OgImage?.value?.src}`} />
        ) : (
          ''
        )}

        {fields?.OgType?.value ? (
          <meta property="og:type" content={`${fields?.OgType?.value}`} />
        ) : (
          ''
        )}
      </Head>

      {/* root placeholder for the app, which we add components to using route data */}
      <div className={mainClassPageEditing}>
        <header>
          <div id="header" className="container-fluid">
            <div className="row">
              {route && <Placeholder name="headless-header" rendering={route} />}
            </div>
          </div>
        </header>
        <main>
          <div id="content" className="container-fluid">
            <div className="row">
              {route && <Placeholder name="headless-main" rendering={route} />}
            </div>
          </div>
        </main>
        <footer>
          <div id="footer" className="container-fluid">
            <div className="row">
              {route && <Placeholder name="headless-footer" rendering={route} />}
            </div>
          </div>
        </footer>
      </div>
    </>
  );
};

export default Layout;

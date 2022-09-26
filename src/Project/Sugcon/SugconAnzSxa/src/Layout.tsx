/**
 * This Layout needs for SXA example.
 */
import React from 'react';
import Head from 'next/head';
import { Placeholder, getPublicUrl, LayoutServiceData } from '@sitecore-jss/sitecore-jss-nextjs';

// Prefix public assets with a public URL to enable compatibility with Sitecore Experience Editor.
// If you're not supporting the Experience Editor, you can remove this.
const publicUrl = getPublicUrl();

interface LayoutProps {
  layoutData: LayoutServiceData;
}

const Layout = ({ layoutData }: LayoutProps): JSX.Element => {
  const { route } = layoutData.sitecore;

  return (
    <>
      <Head>
        <title>
          {route?.fields?.Title?.value
            ? 'SUGCON ANZ - ' + route?.fields?.Title?.value
            : 'SUGCON ANZ'}
        </title>
        <link rel="icon" href={`${publicUrl}/favicon.ico`} />

        {/* Meta Tags */}
        {route?.fields?.MetaDescription?.value
          ? <meta name="description" content={`${route?.fields?.MetaDescription?.value}`}/>
          : ''}

        {route?.fields?.MetaKeywords?.value
          ? <meta name="keywords" content={`${route?.fields?.MetaKeywords?.value}`}/>
          : ''}
        
        {/* Open Graph Tags */}
        {route?.fields?.OgTitle?.value
          ? <meta property="og:title" content={`${route?.fields?.OgTitle?.value}`}/>
          : ''}

        {route?.fields?.OgDescription?.value
          ? <meta property="og:description" content={`${route?.fields?.OgDescription?.value}`}/>
          : ''}

        {route?.fields?.OgImage?.value?.src
          ? <meta property="og:Image" content={`${route?.fields?.OgImage?.value?.src}`}/>
          : ''}

        {route?.fields?.OgType?.value
          ? <meta property="og:Type" content={`${route?.fields?.OgType?.value}`}/>
          : ''}
      </Head>

      {/* root placeholder for the app, which we add components to using route data */}
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
    </>
  );
};

export default Layout;

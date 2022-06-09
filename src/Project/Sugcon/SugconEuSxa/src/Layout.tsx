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
        <title>{route?.fields?.pageTitle?.value || 'Page'}</title>
        <link rel="icon" href={`${publicUrl}/favicon.ico`} />
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

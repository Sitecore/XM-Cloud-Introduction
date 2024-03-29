import type { AppProps } from 'next/app';
import { I18nProvider } from 'next-localization';
import { SitecorePageProps } from 'lib/page-props';
import { SpeedInsights } from '@vercel/speed-insights/next';

import 'assets/main.scss';

function App({ Component, pageProps }: AppProps<SitecorePageProps>): JSX.Element {
  const { dictionary, ...rest } = pageProps;

  // Vercel Speed Insights - Turned on and off through env variable.
  const isSpeedInsightsEnabled =
    process.env.NEXT_PUBLIC_ENABLE_SPEED_INSIGHTS === 'true';

  return (
    <>
      {/*
       Use the next-localization (w/ rosetta) library to provide our translation dictionary to the app.
       Note Next.js does not (currently) provide anything for translation, only i18n routing.
       If your app is not multilingual, next-localization and references to it can be removed.
      */}
      <I18nProvider lngDict={dictionary} locale={pageProps.locale}>
        <Component {...rest} />
      </I18nProvider>

      {isSpeedInsightsEnabled ? <SpeedInsights /> : <></>}
    </>
  );
}

export default App;

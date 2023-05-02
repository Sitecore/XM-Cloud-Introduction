import { SitecorePageProps } from 'lib/page-props';
import { sitecorePagePropsFactory } from 'lib/page-props-factory';
import { GetStaticProps } from 'next';

import SitecorePage from './[[...path]]';

export const getStaticProps: GetStaticProps = async (context) => {
  let props = { notFound: false };
  if (!process.env.DISABLE_SSG_FETCH) {
    props = await sitecorePagePropsFactory.create({
      ...context,
      params: { ...context.params, path: '/NotFound' },
    });
  }

  return {
    props,
    revalidate: 5,
  };
};

export default function Custom404Page({
  layoutData,
  componentProps,
  notFound,
  site,
  locale,
  dictionary,
}: SitecorePageProps): JSX.Element {
  if (!process.env.DISABLE_SSG_FETCH) {
    return (
      <SitecorePage
        notFound={notFound}
        layoutData={layoutData}
        componentProps={componentProps}
        dictionary={dictionary}
        locale={locale}
        site={site}
      />
    );
  } else {
    return <div />;
  }
}

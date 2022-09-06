import { SitecorePageProps } from 'lib/page-props';
import { sitecorePagePropsFactory } from 'lib/page-props-factory';
import { GetStaticProps } from 'next';

import SitecorePage from './[[...path]]';

export const getStaticProps: GetStaticProps = async (context) => {
  let props = { notFound: false };
  props = await sitecorePagePropsFactory.create({
    ...context,
    params: { ...context.params, path: '/NotFound' },
  });

  return {
    props,
    revalidate: 5,
  };
};

export default function Custom404Page({
  layoutData,
  componentProps,
}: SitecorePageProps): JSX.Element {
  return (
    <SitecorePage
      notFound={true}
      layoutData={layoutData}
      componentProps={componentProps}
      dictionary={{}}
      locale=""
    />
  );
}

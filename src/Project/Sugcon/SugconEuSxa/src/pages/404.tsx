import { SitecorePageProps } from 'lib/page-props';
import { sitecorePagePropsFactory } from 'lib/page-props-factory';
import { GetStaticProps } from 'next';

import SitecorePage from './[[...path]]';

export const getStaticProps: GetStaticProps = async (context) => {
  let props = { notFound: false };
  props = await sitecorePagePropsFactory.create({
    ...context,
    params: { ...context.params, path: '/404' },
  });

  return {
    props,
    revalidate: 5,
    notFound: props.notFound, // Returns custom 404 page with a status code of 404 when true
  };
};

export default function Custom404Page({
  notFound,
  layoutData,
  componentProps,
}: SitecorePageProps): JSX.Element {
  return (
    <SitecorePage
      notFound={notFound}
      layoutData={layoutData}
      componentProps={componentProps}
      dictionary={{}}
      locale=""
    />
  );
}

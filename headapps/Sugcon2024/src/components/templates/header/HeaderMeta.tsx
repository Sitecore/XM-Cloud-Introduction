'use client';
import { usePathname } from 'next/navigation';
import { JSX } from 'react';
import { RouteFields } from 'src/Layout';

interface HeaderMetaProps {
  fields: RouteFields | undefined;
}

export const HeaderMeta = ({ fields }: HeaderMetaProps): JSX.Element => {
  
  const pathname = usePathname();
  const canonicalUrl = (origin + (pathname === '/' ? '' : pathname)).split('?')[0];
  
  return (
    <>
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
    </>
  );
};

export default HeaderMeta;


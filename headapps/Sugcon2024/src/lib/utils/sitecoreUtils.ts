import { TextField } from '@sitecore-jss/sitecore-jss-nextjs';

interface SitecoreLinkField {
  value?: {
    href?: string;
    text?: string;
    anchor?: string;
  };
}

interface SitecoreImageField {
  value?: {
    src?: string;
  };
}

export function isSitecoreLinkFieldPopulated(field?: SitecoreLinkField): boolean {
  return Boolean(field?.value?.href && (field?.value?.text || field?.value?.anchor));
}

export function isSitecoreTextFieldPopulated(field: TextField): boolean {
  return Boolean(field?.value);
}

export function isSitecoreImageFieldPopulated(field: SitecoreImageField): boolean {
  return Boolean(field?.value?.src);
}

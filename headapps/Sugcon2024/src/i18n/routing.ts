import { defineRouting } from 'next-intl/routing';
import sitecoreConfig from 'sitecore.config';

export const routing = defineRouting({
  // A list of all locales that are supported
  locales: [sitecoreConfig.defaultLanguage],

  // Used when no locale matches
  defaultLocale: sitecoreConfig.defaultLanguage,

  // No prefix is added for the default locale ("as-needed").
  // For other configuration options, refer to the next-intl documentation:
  // https://next-intl.dev/docs/routing/configuration
  localePrefix: 'as-needed',
});

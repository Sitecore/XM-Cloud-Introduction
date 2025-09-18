import type { StorybookConfig } from '@storybook/nextjs';
import path from 'path';

const config: StorybookConfig = {
  // If you DO have MDX stories, keep the MDX line; otherwise comment it out.
  stories: [
    // '../src/**/*.mdx',
    '../src/**/*.stories.@(js|jsx|mjs|ts|tsx)',
  ],
  addons: [
    '@storybook/addon-links',
    '@storybook/addon-essentials',
    '@storybook/addon-onboarding',
    '@storybook/addon-interactions',
  ],
  framework: {
    name: '@storybook/nextjs',
    options: {},
  },
  docs: {
    autodocs: 'tag',
  },
  // `public/images` is already inside `public`, so one entry is enough
  staticDirs: ['../public'],

  webpackFinal: async (cfg) => {
    // Storybook's webpack config defaults to production mode, which causes
    // minification and dead code elimination. This interferes with some of
    // our conditional code that relies on process.env.NODE_ENV checks.
    (cfg as any).cache = false; // cache: false

   
    cfg.resolve = cfg.resolve || {};
    cfg.resolve.alias = cfg.resolve.alias || {};
    Object.assign(cfg.resolve.alias, {
      '@sass': path.resolve(__dirname, '../src/assets/sass'),
      '@fontawesome': path.join(__dirname, '../node_modules', 'font-awesome'),
    });

    return cfg;
  },
};

export default config;

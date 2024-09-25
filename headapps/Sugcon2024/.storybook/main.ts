import type { StorybookConfig } from '@storybook/nextjs';
import path from 'path';

const config: StorybookConfig = {
  stories: ['../src/**/*.mdx', '../src/**/*.stories.@(js|jsx|mjs|ts|tsx)'],
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
  staticDirs: ['../public', '../public/images'],
  webpackFinal: async (config) => {
    // Ensure the existence of config.resolve and config.resolve.alias
    config.resolve = config.resolve || {};
    config.resolve.alias = config.resolve.alias || {};

    Object.assign(config.resolve.alias, {
      '@sass': path.resolve(__dirname, '../src/assets/sass'),
      '@fontawesome': path.join(__dirname, '../node_modules', 'font-awesome'),
    });

    return config;
  },
};
export default config;

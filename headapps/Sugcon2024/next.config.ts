import type { NextConfig } from 'next';
import path from 'path';
import createNextIntlPlugin from 'next-intl/plugin';

const nextConfig: NextConfig = {
  // Allow specifying a distinct distDir when concurrently running app in a container
  distDir: process.env.NEXTJS_DIST_DIR || '.next',
  
  // Enable React Strict Mode
  reactStrictMode: true,

  // Disable the X-Powered-By header. Follows security best practices.
  poweredByHeader: false,

  // Security headers including Content Security Policy
  headers: async () => {
    return [
      {
        source: '/(.*)',
        headers: [
          {
            key: 'Content-Security-Policy',
            value: [
              "default-src 'self'",
              "script-src 'self' 'unsafe-inline' 'unsafe-eval' https://www.googletagmanager.com https://www.google-analytics.com https://edge.sitecorecloud.io https://edge-platform.sitecorecloud.io https://*.sitecorecloud.io https://sessionize.com https://*.sessionize.com https://cdn.sessionize.com https://assets.sessionize.com",
              "style-src 'self' 'unsafe-inline' data: https://fonts.googleapis.com https://cdnjs.cloudflare.com https://sessionize.com https://*.sessionize.com https://sessionize.blob.core.windows.net",
              "img-src 'self' data: blob: https://edge.sitecorecloud.io https://*.sitecorecloud.io https://*.sugcon.events https://www.googletagmanager.com https://www.google-analytics.com https://i.ytimg.com https://*.ytimg.com https://sessionize.com https://*.sessionize.com",
              "font-src 'self' data: https://fonts.gstatic.com https://cdnjs.cloudflare.com https://sessionize.com https://*.sessionize.com",
              "connect-src 'self' https://www.google-analytics.com https://www.googletagmanager.com https://edge.sitecorecloud.io https://edge-platform.sitecorecloud.io https://*.sitecorecloud.io https://sessionize.com https://*.sessionize.com",
              "frame-src 'self' https://www.youtube.com https://www.youtube-nocookie.com https://youtube.com https://*.youtube.com https://sessionize.com https://*.sessionize.com",
              "frame-ancestors 'self' https://*.sitecorecloud.io https://pages.sitecorecloud.io",
              "media-src 'self' https://*.youtube.com https://*.googlevideo.com https://edge.sitecorecloud.io https://*.sitecorecloud.io https://*.sugcon.events",
              "worker-src 'self' blob:",
              "base-uri 'self'",
              "form-action 'self'",
              "object-src 'none'",
            ].join('; '),
          },
          {
            key: 'X-Content-Type-Options',
            value: 'nosniff',
          },
          {
            key: 'X-Frame-Options',
            value: 'SAMEORIGIN',
          },
          {
            key: 'Referrer-Policy',
            value: 'strict-origin-when-cross-origin',
          },
          {
            key: 'Permissions-Policy',
            value: 'camera=(), microphone=(), geolocation=()',
          },
        ],
      },
    ];
  },

  // use this configuration to ensure that only images from the whitelisted domains
  // can be served from the Next.js Image Optimization API
  // see https://nextjs.org/docs/app/api-reference/components/image#remotepatterns
  images: {
    remotePatterns: [
      {
        protocol: 'https',
        hostname: 'edge*.**',
        port: '',
      },
      {
        protocol: 'https',
        hostname: 'xmc-*.**',
        port: '',
      },

      {
        protocol: 'https',
        hostname: '**.sugcon.events',
        port: '',
      },
    ],
  },
  
  // use this configuration to serve the sitemap.xml and robots.txt files from the API route handlers
  rewrites: async () => {
    return [
      {
        source: '/sitemap:id([\\w-]{0,}).xml',
        destination: '/api/sitemap',
        locale: false,
      },
      {
        source: '/robots.txt',
        destination: '/api/robots',
        locale: false,
      },
    ];
  },

  sassOptions: {
    includePaths: [
      path.join(__dirname, 'src/assets'),
      path.join(__dirname, 'node_modules'),
    ],
    silenceDeprecations: ['legacy-js-api'],
  },
};

const withNextIntl = createNextIntlPlugin();
export default withNextIntl(nextConfig);

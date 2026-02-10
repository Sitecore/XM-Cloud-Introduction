# GitHub Copilot Instructions for Sitecore Content SDK Next.js App Router Project

## Project Purpose and Tech Stack

This is a **Sitecore Content SDK** application built with **Next.js App Router** and **TypeScript**. The project follows Sitecore best practices for XM Cloud development and leverages the latest Next.js App Router features for improved performance and developer experience.

### Key Technologies
- **Next.js App Router** - React framework with Server Components and modern routing
- **Sitecore Content SDK** - Official SDK for Sitecore XM Cloud integration
- **TypeScript** - Type-safe JavaScript development
- **Sitecore XM Cloud** - Headless CMS platform
- **React Server Components** - Server-side rendering for better performance
- **next-intl** - Internationalization support

## Coding Standards

### TypeScript Standards
- Use **strict mode** in tsconfig.json
- Prefer type assertions over `any`: `value as ContentItem`
- Use discriminated unions for complex state management
- Enable strict null checks and strict function types

### Naming Conventions
- **Variables/Functions**: camelCase (`getUserData()`, `isLoading`, `currentUser`)
- **Components**: PascalCase (`SitecoreComponent`, `PageLayout`, `ContentBlock`)
- **Constants**: UPPER_SNAKE_CASE (`API_ENDPOINT`, `DEFAULT_TIMEOUT`)
- **Directories**: kebab-case (`src/components`, `src/api-clients`)
- **Types/Interfaces**: PascalCase (`ContentItem`, `LayoutProps`, `SitecoreConfig`)

### Modular Layout (App Router)
```
src/
  app/                 # App Router pages and layouts
  components/          # UI components (React)
  lib/                 # Configuration and utilities
  i18n/                # Internationalization setup
  types/               # TypeScript type definitions
  hooks/               # Custom React hooks
```

## Library Usage

### @sitecore-content-sdk
- Use `SitecoreClient` for content fetching
- Implement proper error handling with try/catch blocks
- Cache API responses using React Query or SWR
- Handle content preview vs. published content scenarios

```typescript
import { SitecoreClient } from '@sitecore-content-sdk/nextjs/client';
import scConfig from 'sitecore.config';

const client = new SitecoreClient({
  ...scConfig,
});
```

### React App Router Patterns
- Use **Server Components** for data fetching and static content (default)
- Use **Client Components** for interactivity (use 'use client' directive)
- Implement proper error boundaries with error.tsx
- Use loading.tsx for loading states
- Leverage layout.tsx for shared page structure

### Sitecore Field Components
- Always use Sitecore field components: `<Text>`, `<RichText>`, `<Image>`
- Validate field existence before rendering
- Handle empty/null fields gracefully
- Prefer Sitecore field components over manual rendering

```typescript
// Good: Using Sitecore field components
<Text field={fields?.title} tag="h1" />
<RichText field={fields?.content} />
<Image field={fields?.backgroundImage} />

// Avoid: Manual field value extraction unless necessary
```

## Example Patterns and Prompts

### Server Component Development
```typescript
// Server Component example (default in App Router)
import { SitecoreClient } from '@sitecore-content-sdk/nextjs/client';
import scConfig from 'sitecore.config';

const client = new SitecoreClient({
  ...scConfig,
});

export default async function SitecorePage({ params }: { params: { path: string[] } }) {
  try {
    const pageData = await client.getPage(params.path.join('/'));
    return <SitecoreLayout layoutData={pageData?.layout} />;
  } catch (error) {
    return <div>Content not found</div>;
  }
}
```

### Client Component Integration

Interactive Sitecore Components:

- Use 'use client' directive when needed
- Keep client components focused on interactivity
- Pass server-fetched data as props
- Handle hydration mismatches carefully

```typescript
'use client';

interface InteractiveSitecoreComponentProps {
  fields: {
    title: Field;
    content: Field;
  };
}

export default function InteractiveSitecoreComponent({
  fields,
}: InteractiveSitecoreComponentProps) {
  // Client-side interactivity here
  return (
    <div>
      <Text field={fields?.title} tag="h2" />
      <RichText field={fields?.content} />
    </div>
  );
}
```

### Component Development
```typescript
// Component props interface
interface HeroProps {
  fields: {
    title: Field;
    subtitle: Field;
    backgroundImage: Field;
  };
}

export default function Hero({ fields }: HeroProps) {
  return (
    <div>
      <Text field={fields?.title} tag="h1" />
      <Text field={fields?.subtitle} tag="p" />
      <Image field={fields?.backgroundImage} />
    </div>
  );
}
```

### Error Handling

API Calls:

- Always wrap in try/catch blocks
- Throw custom errors with context: `SitecoreFetchError`, `ConfigurationError`
- Handle edge cases with guard clauses

```typescript
async function fetchPageData(path: string): Promise<Page | null> {
  if (!path) {
    throw new Error('Page path is required');
  }

  try {
    const pageData = await client.getPage(path);
    return pageData;
  } catch (error) {
    throw new SitecoreFetchError(`Failed to fetch page data for ${path}`, error);
  }
}
```

### Configuration
```typescript
// sitecore.config.ts
import { defineConfig } from '@sitecore-content-sdk/nextjs/config';

export default defineConfig({
  api: {
    edge: {
      contextId: process.env.SITECORE_EDGE_CONTEXT_ID || '',
      clientContextId: process.env.NEXT_PUBLIC_SITECORE_EDGE_CONTEXT_ID,
      edgeUrl: process.env.SITECORE_EDGE_URL || 'https://edge-platform.sitecorecloud.io',
    },
    local: {
      apiKey: process.env.SITECORE_API_KEY || '',
      apiHost: process.env.SITECORE_API_HOST || '',
    },
  },
  defaultSite: process.env.NEXT_PUBLIC_DEFAULT_SITE_NAME || 'default',
  defaultLanguage: process.env.NEXT_PUBLIC_DEFAULT_LANGUAGE || 'en',
  editingSecret: process.env.SITECORE_EDITING_SECRET,
});
```

### Internationalization

Multi-language Support:

- Configure next-intl for language routing
- Handle Sitecore language contexts
- Implement language switching
- Use proper locale-based data fetching

```typescript
// Language-aware data fetching
import { getTranslations } from 'next-intl/server';

export default async function LocalizedPage() {
  const t = await getTranslations('common');
  // Fetch Sitecore content for current locale
}
```

## Development Workflow

1. **Install dependencies**: `npm install`
2. **Configure environment**: Copy `.env.example` to `.env.local`
3. **Start development**: `npm run dev`
4. **Build for production**: `npm run build`

## App Router Best Practices

### Server vs Client Components
- Use Server Components for Sitecore content rendering (default)
- Use Client Components for user interactions
- Minimize client-side JavaScript
- Leverage server-side data fetching

### Routing and Layouts
- Use layout.tsx for shared page structure
- Implement loading.tsx for loading states
- Create error.tsx for error boundaries
- Use page.tsx for route content
- Use [...path] for Sitecore catch-all routes

### Performance Optimization
- Leverage Server Components for better performance
- Use streaming for improved loading experience
- Implement proper caching strategies
- Optimize images with Next.js Image component

## Best Practices

### Performance
- Optimize images using Next.js Image component
- Implement proper loading states
- Cache expensive operations appropriately
- Consider server-side rendering implications
- Lazy-load non-critical modules
- Use Server Components for better performance

### Security
- Sanitize user inputs before processing
- Validate data at application boundaries
- Use HTTPS for all Sitecore connections
- Never expose sensitive configuration in client-side code
- Escape content when rendering to prevent XSS

### Code Quality
- Follow DRY principle - extract common functionality
- Use SOLID principles for maintainable code
- Write self-documenting code with clear intent
- Implement proper error boundaries
- Test behavior, not implementation details

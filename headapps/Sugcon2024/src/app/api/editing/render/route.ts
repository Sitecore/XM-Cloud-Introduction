import { createEditingRenderRouteHandlers } from "@sitecore-content-sdk/nextjs/route-handler";

/**
 * API route to handler Sitecore Editor rendeing.
 * When using custom server URL, it should match the rendering host from your Sitecore configuration,
 * (see the settings item under /sitecore/content/<your/site/path>/Settings/Site Grouping).
 *
 * The route handler will:
 *  1. Extract data about the route we need to render from the Sitecore Editor GET request
 *  2. Enable Next.js Draft Mode
 *  3. Pass preview data as query string parameters, alongside required headers and cookies to an internal editing request
 *  4. Return the rendered HTML for editing mode
 */

export const { GET, POST, OPTIONS } = createEditingRenderRouteHandlers({});

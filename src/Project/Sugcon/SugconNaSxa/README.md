# SUGCON EU Site
This is a headless Next.js site that uses the following technologies:
- Sitecore XM Cloud (Content Management)
- Sitecore JSS and Headless SXA (headless SDK)
- Next.js (rendering host)
- Vercel (hosting)

## Prerequisites for this Next.js site are:
- [NodeJS 16 LTS](https://nodejs.org/en/download/) (or greater)

## Environment variables needed for deployment
- `FETCH_WITH`: Use the value 'GraphQL' to force the use of the GraphQL endpoint.
- `GRAPH_QL_ENDPOINT`: This is your environment Edge GraphQL endpoint. In production, it is likely https://edge.sitecorecloud.io/api/graphql/v1
- `SITECORE_API_KEY`: This is an Edge service token that you generate against your environment. [TODO: Link to the docs]

## Sitecore JSS Next.js Sample Docs
* [Documentation](https://doc.sitecore.com/xp/en/developers/hd/latest/sitecore-headless-development/sitecore-javascript-rendering-sdk--jss--for-next-js.html)
import { ComponentRendering, Field } from '@sitecore-jss/sitecore-jss-nextjs';

export type ComponentData = {
  rendering: ComponentRendering;
  fields: {
    Title: Field<string>;
    SessionizeURL: Field<string>;
  };
};

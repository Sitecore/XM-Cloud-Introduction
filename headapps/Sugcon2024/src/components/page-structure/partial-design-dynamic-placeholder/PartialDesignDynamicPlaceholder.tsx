import React, { JSX } from 'react';
import { ComponentProps } from 'lib/component-props';
import componentMap from '.sitecore/component-map';
import { AppPlaceholder } from "@sitecore-content-sdk/nextjs";

const PartialDesignDynamicPlaceholder = (
  props: ComponentProps
): JSX.Element => {
  return (
    <AppPlaceholder
      name={props.rendering?.params?.sig || ""}
      rendering={props.rendering}
      page={props.page}
      componentMap={componentMap}
    />
  );
};

export default PartialDesignDynamicPlaceholder;

import { Flex, FlexProps } from '@chakra-ui/react';
import { layoutFlexStyles } from 'components/templates/LayoutConstants';

// Wrapper for Chakra UI Flex component which will apply our Theme properties for a Container...
// i.e: max-width, padding-block, padding-inline, etc...
export const LayoutFlex = (props: FlexProps) => {
  return <Flex {...layoutFlexStyles} {...props} />;
};

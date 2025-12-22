import { Flex, FlexProps, useStyleConfig } from '@chakra-ui/react';

// Wrapper for Chakra UI Flex component which will apply our Theme properties for a Container...
// i.e: max-width, padding-block, padding-inline, etc...
export const LayoutFlex = (props: FlexProps) => {
  const styles = useStyleConfig('LayoutFlex', props);

  return <Flex {...(styles as FlexProps)} {...props} />;
};

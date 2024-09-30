import { Flex } from '@chakra-ui/react';
import { Placeholder, RouteData } from '@sitecore-jss/sitecore-jss-nextjs';

import React from 'react';
import { HeaderHeights } from './LayoutConstants';
import { LayoutFlex } from './LayoutFlex';

interface HeaderDefaultProps {
  route: RouteData | null;
}

export const Default = ({ route }: HeaderDefaultProps): JSX.Element => {
  return (
    <Flex
      as="header"
      width="full"
      pos="sticky"
      top="0"
      zIndex="banner"
      bg="white"
      shadow="xl"
      minH={{ base: HeaderHeights.Mobile, lg: HeaderHeights.Desktop }}
    >
      <LayoutFlex my="0" align="center" flexGrow="1">
        {route && <Placeholder name="headless-header" rendering={route} />}
      </LayoutFlex>
    </Flex>
  );
};

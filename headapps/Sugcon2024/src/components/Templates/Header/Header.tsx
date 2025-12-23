import React, { JSX } from 'react';
import { Flex } from '@chakra-ui/react';
import { AppPlaceholder, RouteData, Page, ComponentMap } from '@sitecore-content-sdk/nextjs';
import { HeaderHeights } from '../LayoutConstants';
import { LayoutFlex } from '../layout-flex/LayoutFlex';

interface HeaderDefaultProps {
  route: RouteData | null;
  page: Page;
  componentMap: ComponentMap
}

export const Header = ({ route, page, componentMap }: HeaderDefaultProps): JSX.Element => {
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
        {route && <AppPlaceholder
                      page={page}
                      componentMap={componentMap}
                      name="headless-header"
                      rendering={route}
                    />}
      </LayoutFlex>
    </Flex>
  );
};

export default Header;
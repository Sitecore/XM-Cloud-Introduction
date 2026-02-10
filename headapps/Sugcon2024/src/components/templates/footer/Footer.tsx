import { AppPlaceholder, RouteData, Page, ComponentMap } from '@sitecore-content-sdk/nextjs';
import React, { JSX } from 'react';
import { Flex } from '@chakra-ui/react';

interface FooterDefaultProps {
  route: RouteData | null;
  page: Page;
  componentMap: ComponentMap
}

export const Footer = ({ route, page, componentMap }: FooterDefaultProps): JSX.Element => {
  return (
    <Flex
      as="footer"
      align="center"
      justify="center"
      wrap="wrap"
      flexGrow="1"
      bgColor="black"
      pb="30px"
      gap="30px"
    >
      { route && <AppPlaceholder
                      page={page}
                      componentMap={componentMap}
                      name="headless-footer"
                      rendering={route}
                    /> }
    </Flex>
  );
};

export default Footer;
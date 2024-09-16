import { Placeholder, RouteData } from '@sitecore-jss/sitecore-jss-nextjs';
import React from 'react';
import { Flex } from '@chakra-ui/react';

interface FooterDefaultProps {
  route: RouteData | null;
}

export const Default = ({ route }: FooterDefaultProps): JSX.Element => {
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
      {route && <Placeholder name="headless-footer" rendering={route} />}
    </Flex>
  );
};

import React from 'react';
import { Box, Heading, Text, Flex, Link } from '@chakra-ui/react';
import { LinkField, TextField, useSitecoreContext } from '@sitecore-jss/sitecore-jss-nextjs';
import { ComponentParams, ComponentRendering } from '@sitecore-jss/sitecore-jss-nextjs';

interface Fields {
  data: {
    datasource: {
      url: {
        path: string;
        siteName: string;
      };
      field: {
        jsonValue: {
          value: string;
          editable: string;
        };
      };
    };
    contextItem: {
      url: {
        path: string;
        siteName: string;
      };
      field: {
        jsonValue: {
          value: string;
          editable: string;
        };
      };
    };
  };
}

type PageTitleProps = {
  rendering: ComponentRendering & { params: ComponentParams };
  params: { [key: string]: string };
  fields: Fields;
};

export const Default = (props: PageTitleProps): JSX.Element => {
  //const id = props.params.RenderingIdentifier;
  const datasource = props.fields?.data?.datasource || props.fields?.data?.contextItem;
  const { sitecoreContext } = useSitecoreContext();

  const text: TextField = {
    value: datasource?.field?.jsonValue?.value,
    editable: datasource?.field?.jsonValue?.editable,
  };
  const link: LinkField = {
    value: {
      href: datasource?.url?.path,
      title: datasource?.field?.jsonValue?.value,
      editable: true,
    },
  };
  if (sitecoreContext.pageState !== 'normal') {
    link.value.querystring = `sc_site=${datasource?.url?.siteName}`;
    if (!text.value) {
      text.value = 'Title field';
      link.value.href = '#';
    }
  }

  return (
    <Flex
      direction={{ base: 'column', md: 'row' }}
      alignItems="center"
      bg="#f0f0f0"
      w="100vw"
      boxShadow="-20px 19px 40px 0px rgba(0, 0, 0, 0.2) inset"
      maxHeight="400px"
    >
      <Flex
        direction="column"
        margin="0 auto" // Center the content box
        p={5}
        flexGrow={1}
        minWidth="50%"
      >
        <Box width="auto" alignSelf="end" maxWidth="620px">
          <Heading as="h1" fontSize="30px" fontWeight="bold" mb="33px">
            <>
              {sitecoreContext.pageState === 'edit' ? (
                <Text>{text.value}</Text>
              ) : (
                <Link href={link.value.href} isExternal={link.value.target == '_blank'}>
                  <Text>{text.value}</Text>
                </Link>
              )}
            </>
          </Heading>
        </Box>
      </Flex>
      <Box flex="1" position="relative" minWidth="50%" maxHeight="400px">
        {' '}
      </Box>
    </Flex>
  );
};

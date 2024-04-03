import React from 'react';
import { Box, Heading } from '@chakra-ui/react';
import { LinkField, Text, TextField, useSitecoreContext } from '@sitecore-jss/sitecore-jss-nextjs';
import bg from '../../assets/images/SUGCON-hero-artwork.jpg';
import { LayoutFlex } from 'components/Templates/LayoutFlex';

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
    <Box
      width="100%"
      background="linear-gradient(#eb1f1f 40% , #2B317B)"
      backgroundImage={bg.src}
      backgroundPosition="center"
      backgroundSize="cover"
      backgroundRepeat="no-repeat"
      color="white"
    >
      <LayoutFlex>
        <Heading as="h1" fontSize="44px" fontWeight="normal">
          <Text field={text} />
        </Heading>
      </LayoutFlex>
    </Box>
  );
};

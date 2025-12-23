import React, { JSX}  from 'react';
import {
  Field,
  Link as JssLink,
  LinkField,
  Text as JssText,
} from '@sitecore-content-sdk/nextjs';
import { Button, Flex, Heading, Text } from '@chakra-ui/react';
import { LayoutFlex } from 'components/page-structure/layout-flex/LayoutFlex';

interface Fields {
  Title: Field<string>;
  Text: Field<string>;
  CallToAction: LinkField;
}

type ActionBannerProps = {
  params: { [key: string]: string };
  fields: Fields;
};

const ActionBannerDefaultComponent = (props: ActionBannerProps): JSX.Element => (
  <div className={`component promo ${props?.params?.styles}`}>
    <div className="component-content">
      <span className="is-empty-hint">Agenda</span>
    </div>
  </div>
);

export const Default = (props: ActionBannerProps): JSX.Element => {
  const id = props?.params?.RenderingIdentifier || undefined;

  if (props?.fields) {
    return (
      <Flex bgColor="sugcon.red.100" color="white" className={props?.params?.styles} id={id}>
        <LayoutFlex
          justify="space-between"
          flexDir={{ base: 'column', lg: 'row' }}
          align={{ base: 'flex-start', lg: 'center' }}
          gap={{ lg: '75px' }}
        >
          <Heading as="h2" size="lg" mb="0">
            <JssText field={props?.fields?.Title} />
          </Heading>

          <Text fontSize="lg" mb={{ base: '10px', lg: '0' }}>
            <JssText field={props?.fields?.Text} />
          </Text>

          <Button as={JssLink} variant="secondary" field={props?.fields?.CallToAction}>
            {props?.fields?.CallToAction?.value?.text}
          </Button>
        </LayoutFlex>
      </Flex>
    );
  }

  return <ActionBannerDefaultComponent {...props} />;
};

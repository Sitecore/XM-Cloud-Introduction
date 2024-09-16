import { Meta, StoryObj } from '@storybook/react';
import { Default as HeaderComponent } from 'template/Header';
import { Field, RouteData } from '@sitecore-jss/sitecore-jss-nextjs';
import { GenericFieldValue, Item } from '@sitecore-jss/sitecore-jss/types/layout/models';
import { PrimaryNavigation } from '../components/Basic Components/Navigation.stories';
import { Logo } from '../components/Basic Components/Logo.stories';

const meta = {
  title: 'Templates/Header',
  component: HeaderComponent,
  parameters: {
    layout: 'fullscreen',
  },
  tags: ['autodocs'],
  argTypes: {},
} satisfies Meta<typeof HeaderComponent>;

export default meta;

type Story = StoryObj<typeof meta>;

const route = {
  name: 'Home',
  displayName: 'Home',
  placeholders: {
    'headless-header': [
      {
        uid: '159680fb-a886-4c78-9311-7a66bc849cfd',
        componentName: 'PartialDesignDynamicPlaceholder',
        dataSource: '',
        params: {
          sid: '{2F04937C-9094-4872-B5C5-157B2815BDF3}',
          ph: 'headless-header',
          sig: 'sxa-header',
        },
        placeholders: {
          'sxa-header': [
            {
              uid: 'c123a7f7-7187-464c-82ab-bc343cd151f2',
              componentName: 'Logo',
              dataSource: '{20C73871-7E77-4F6E-9D35-C518AB120F44}',
              ...Logo.args,
              fields: {
                ...Logo.args.fields,
                ImageCaption: null,
              },
            },
            {
              ...PrimaryNavigation.args.rendering,
            },
          ],
        },
      },
    ],
  },
};

export const Header: Story = {
  name: 'Header',
  args: {
    route: route as unknown as RouteData<Record<string, Field<GenericFieldValue> | Item | Item[]>>,
  },
};

import { Meta, StoryObj } from '@storybook/react';
import { HeroHomepage } from '../../../components/Basic Components/Hero';
import { SitecoreContext } from '@sitecore-jss/sitecore-jss-react';

const meta = {
  title: 'Components/Hero',
  component: HeroHomepage,
  parameters: {
    layout: 'centered',
  },
  tags: ['autodocs'],
  decorators: [
    (Story) => (
      // Assuming you might need to provide a mock context value
      <SitecoreContext componentFactory={() => null}>
        <Story />
      </SitecoreContext>
    ),
  ],
  argTypes: {},
} satisfies Meta<typeof HeroHomepage>;
export default meta;
type Story = StoryObj<typeof meta>;
export const Homepage: Story = {
  name: 'Hero Homepage',
  args: {
    params: {
      styles: '',
    },
    fields: {
      Headline: {
        value: 'An event from the community for the community',
      },
      EventDate: {
        value: 'October 5-6th',
      },
      Text: {
        value:
          'Get ready to experience the ultimate Sitecore conference of the year at SUGCON North America in Minneapolis, Minnesota.',
      },
      CallToAction: {
        value: {
          href: 'https://www.sitecore.com',
          anchor: 'Register now',
        },
      },
      Image: {
        value: {
          src: 'https://picsum.photos/1000/566',
        },
      },
    },
  },
};

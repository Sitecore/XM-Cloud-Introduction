import { Meta, StoryObj } from '@storybook/react';
import { Default as SponsorListing } from 'src/components/Sponsors/SponsorListing';

const meta = {
  title: 'Components/SponsorListing',
  component: SponsorListing,
  parameters: {
    layout: 'centered',
  },
  tags: ['autodocs'],
  argTypes: {},
} satisfies Meta<typeof SponsorListing>;

export default meta;
type Story = StoryObj<typeof meta>;

export const Default: Story = {
  name: 'Default',
  args: {
    params: {
      styles: '',
    },
    fields: {
      Title: {
        value: 'Diamond Sponsors',
      },
      Sponsors: [
        {
          fields: {
            SponsorName: {
              value: 'Sitecore',
            },
            SponsorLogo: {
              value: {
                src: 'https://placehold.co/450x200/png',
              },
            },
            SponsorBio: {
              value:
                'Sitecore is the global leader in digital experience management software that combines content management, commerce, and customer insights.',
            },
            SponsorURL: {
              value: {
                href: 'https://www.sitecore.com',
                anchor: 'Learn more',
              },
            },
          },
        },
        {
          fields: {
            SponsorName: {
              value: 'Microsoft',
            },
            SponsorLogo: {
              value: {
                src: 'https://placehold.co/450x200/png',
              },
            },
            SponsorBio: {
              value:
                'Microsoft is the global leader in digital experience management software that combines content management, commerce, and customer insights.',
            },
            SponsorURL: {
              value: {
                href: 'https://www.microsoft.com',
                anchor: 'Learn more',
              },
            },
          },
        },
        {
          fields: {
            SponsorName: {
              value: 'Salesforce',
            },
            SponsorURL: {
              value: {
                href: 'https://www.salesforce.com',
              },
            },
            SponsorBio: {
              value:
                'Salesforce is the global leader in digital experience management software that combines content management, commerce, and customer insights.',
            },
            SponsorLogo: {
              value: {
                src: 'https://placehold.co/450x200/png',
              },
            },
          },
        },
      ],
    },
  },
};

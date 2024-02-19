import { Meta, StoryObj } from '@storybook/react';
import { Default as PeopleGrid } from '../../../components/List Components/PeopleGrid';
import '../../../assets/sass/components/people-grid/index.scss';
const meta = {
  title: 'Components/PeopleGrid',
  component: PeopleGrid,
  parameters: {
    layout: 'centered',
  },
  tags: ['autodocs'],
  argTypes: {},
} satisfies Meta<typeof PeopleGrid>;
export default meta;
type Story = StoryObj<typeof meta>;
export const Teaser: Story = {
  name: 'People Grid',
  args: {
    params: {
      Columns: '4',
      DisplaySocialLinks: '1',
      Styles: 'red-linear-gradient',
    },
    fields: {
      People: [
        {
          id: 'd6f5b3e8-1c83-44b2-b381-ccb74d1bc6c2',
          url: 'http://cm/Data/People/Eric-Sanner',
          name: 'Eric Sanner',
          displayName: 'Eric Sanner',
          fields: {
            Twitter: {
              value: {
                href: 'https://blogs.perficient.com/author/esanner',
                text: 'Blogs',
                linktype: 'external',
                url: 'https://blogs.perficient.com/author/esanner',
                anchor: '',
                target: '',
              },
            },
            Company: {
              value: 'Perficient',
            },
            Name: {
              value: 'Eric Sanner',
            },
            JobRole: {
              value: 'Solution Architect',
            },
            Biography: {
              value: 'This is a test',
            },
            Linkedin: {
              value: {
                href: 'https://www.linkedin.com/in/ericsanner/',
                text: 'Linkedin',
                linktype: 'external',
                url: 'https://www.linkedin.com/in/ericsanner/',
                anchor: '',
                target: '',
              },
            },
            Image: {
              value: {
                src: '../../../../images/EricSanner.png',
                alt: '',
                width: '400',
                height: '400',
              },
            },
          },
        },
        {
          id: '43f0cff6-cd6f-4799-9b97-2acb0452fec7',
          url: 'http://cm/Data/People/Chet-Potvin',
          name: 'Chet Potvin',
          displayName: 'Chet Potvin',
          fields: {
            Twitter: {
              value: {
                href: '',
              },
            },
            Company: {
              value: 'Merkle',
            },
            Name: {
              value: 'Chet Potvin',
            },
            JobRole: {
              value: 'Technical Architect',
            },
            Biography: {
              value: '',
            },
            Linkedin: {
              value: {
                href: '',
              },
            },
            Image: {
              value: {
                src: '../../../../images/ChetPotvin.png',
                alt: '',
                width: '512',
                height: '512',
              },
            },
          },
        },
        {
          id: '5312c099-92b6-41ac-9b5a-5a6166c2ba5d',
          url: 'http://cm/Data/People/Dave-Ambrose',
          name: 'Dave Ambrose',
          displayName: 'Dave Ambrose',
          fields: {
            Twitter: {
              value: {
                href: '',
              },
            },
            Company: {
              value: 'Perficient',
            },
            Name: {
              value: 'Dave Ambrose',
            },
            JobRole: {
              value: 'Solutions Architect',
            },
            Biography: {
              value: '',
            },
            Linkedin: {
              value: {
                href: '',
              },
            },
            Image: {
              value: {
                src: '../../../../images/DaveAmbrose.jpg',
                alt: '',
                width: '512',
                height: '512',
              },
            },
          },
        },
        {
          id: '7448c4dd-f5a0-4af8-90e3-0cd9946b71ef',
          url: 'http://cm/Data/People/Joshua-Hoover',
          name: 'Joshua Hoover',
          displayName: 'Joshua Hoover',
          fields: {
            Twitter: {
              value: {
                href: '',
              },
            },
            Company: {
              value: 'Perficient',
            },
            Name: {
              value: 'Joshua Hover',
            },
            JobRole: {
              value: 'Director',
            },
            Biography: {
              value: '',
            },
            Linkedin: {
              value: {
                href: '',
              },
            },
            Image: {
              value: {
                src: '../../../../images/JoshuaHover.jpg',
                alt: '',
                width: '512',
                height: '512',
              },
            },
          },
        },
      ],
      Headline: {
        value: 'Featured Speakers',
      },
    },
  },
};

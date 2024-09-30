import { Meta, StoryObj } from '@storybook/react';
import { Default as LogoComponent } from '../../../components/Basic Components/Logo';
const meta = {
  title: 'Basic Components/Logo',
  component: LogoComponent,
  parameters: {
    layout: 'centered',
  },
  tags: ['autodocs'],
  argTypes: {},
} satisfies Meta<typeof LogoComponent>;
export default meta;
type Story = StoryObj<typeof meta>;
export const Logo: Story = {
  name: 'Logo',
  args: {
    params: {
      styles: '',
    },
    fields: {
      Image: {
        value: {
          src: 'https://edge.sitecorecloud.io/sitecoresaa94c3-xmcloudintr2ef7-production-9f57/media/Project/Sugcon2024/shared/Logos/SUGCON-Logo-2023-Europe-Horz-RGB.png?h=58&iar=0&w=200',
          alt: 'Logo',
          width: '200',
          height: '50',
        },
      },
      TargetUrl: {
        value: {
          href: 'https://logoipsum.com/',
          anchor: 'Home',
        },
      },
      ImageCaption: {
        value: 'A circular logo with red, yellow and blue stripes',
      },
    },
  },
};

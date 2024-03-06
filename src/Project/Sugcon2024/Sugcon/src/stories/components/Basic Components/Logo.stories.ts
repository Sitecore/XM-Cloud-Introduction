import { Meta, StoryObj } from '@storybook/react';
import { Default as LogoComponent } from '../../../components/Basic Components/Logo';
const meta = {
  title: 'Components/Logo',
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
          src: 'https://img.logoipsum.com/300.svg',
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
      }
    },
  },
};

import { Meta, StoryObj } from '@storybook/react';
import { Default as Person } from '../../../components/Basic Components/Person';

const meta = {
    title: 'Basic Components/Person',
    component: Person,
    tags: ['autodocs'],
    argTypes: {},
} satisfies Meta<typeof Person>;

export default meta;

type Story = StoryObj<typeof meta>;

export const PersonStory: Story = {
    name: 'Person',
    args: {
        params: {
            styles: '',
        },
        fields: {
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
            Twitter: {
                value: {
                    href: 'https://www.linkedin.com/in/ericsanner/',
                    text: 'X',
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
        }
    }
};

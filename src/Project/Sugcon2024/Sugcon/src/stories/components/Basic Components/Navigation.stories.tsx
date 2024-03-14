import { Meta, StoryObj } from '@storybook/react';
import { Default as Navigation } from '../../../components/Navigation';
import { SitecoreContext } from '@sitecore-jss/sitecore-jss-react';

const meta = {
  title: 'Basic Components/Navigation',
  component: Navigation,
  parameters: {
    layout: 'fullscreen',
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
} satisfies Meta<typeof Navigation>;

export default meta;

type Story = StoryObj<typeof meta>;

export const PrimaryNavigation: Story = {
  name: 'Primary Navigation',
  args: {
    rendering: {
      uid: '101b1558-808e-4d52-9ce5-6781dd2fc046',
      componentName: 'Navigation',
      dataSource: '',
      params: {
        FieldNames: 'Default',
        LevelFrom: '{1BB88840-5FB3-4353-AD8D-81136F6FF75A}',
        LevelTo: '{A59325BB-5A27-46F9-8110-9D499715F3BE}',
        Styles: 'navigation-horizontal',
      },
      fields: [
        {
          Id: '9dc1fb0c-996c-4130-bc07-d5773bf2e824',
          Styles: ['level1', 'submenu', 'item0', 'odd', 'first'],
          Href: '/Sessssions',
          Querystring: '',
          NavigationTitle: {
            value: 'Sessions',
            editable: 'Sessions',
          },
          Children: [
            {
              Id: 'ffaa7f0d-2fb9-4c57-8846-5a55c7969fbf',
              Styles: ['level2', 'item0', 'odd', 'first'],
              Href: '/Sessions/Session-1',
              Querystring: '',
              NavigationTitle: {
                value: 'Session 1',
                editable: 'Session 1',
              },
            },
            {
              Id: 'bed50e98-2c1a-402e-a8ac-329015cc4b43',
              Styles: ['level2', 'item1', 'even', 'last'],
              Href: '/Sessions/Session-2',
              Querystring: '',
              NavigationTitle: {
                value: 'Session 2',
                editable: 'Session 2',
              },
            },
          ],
        },
        {
          Id: '20434b37-8963-41c7-88bb-0d63af1854d1',
          Styles: ['level1', 'item1', 'even'],
          Href: '/Agenda',
          Querystring: '',
          NavigationTitle: {
            value: 'Agenda',
            editable: 'Agenda',
          },
        },
        {
          Id: 'f9718ba4-5378-43c1-89cd-537301b81f1b',
          Styles: ['level1', 'item2', 'odd'],
          Href: '/Speakers',
          Querystring: '',
          NavigationTitle: {
            value: 'Speakers',
            editable: 'Speakers',
          },
        },
        {
          Id: '0fbd1460-52d5-467a-accd-f955d448ae1d',
          Styles: ['level1', 'item3', 'even'],
          Href: '/Trainings',
          Querystring: '',
          NavigationTitle: {
            value: 'Trainings',
            editable: 'Trainings',
          },
        },
        {
          Id: '96b9ccc8-50fe-4493-8a2c-62807499262d',
          Styles: ['level1', 'item4', 'odd'],
          Href: '/Sponsors',
          Querystring: '',
          NavigationTitle: {
            value: 'Sponsors',
            editable: 'Sponsors',
          },
        },
        {
          Id: 'e5525fb5-fa3d-4eeb-a354-a30d01d79296',
          Styles: ['level1', 'item5', 'even', 'last'],
          Href: '/Organizers',
          Querystring: '',
          NavigationTitle: {
            value: 'Organizers',
            editable: 'Organizers',
          },
        },
      ],
    },
    fields: {
      // eslint-disable-next-line @typescript-eslint/ban-ts-comment
      // @ts-ignore
      '0': {
        Id: '9dc1fb0c-996c-4130-bc07-d5773bf2e824',
        Styles: ['level1', 'submenu', 'item0', 'odd', 'first'],
        Href: '/Sessions',
        Querystring: '',
        NavigationTitle: {
          value: 'Sessions',
          editable: 'Sessions',
        },
        Children: [
          {
            Id: 'ffaa7f0d-2fb9-4c57-8846-5a55c7969fbf',
            Styles: ['level2', 'item0', 'odd', 'first'],
            Href: '/Sessions/Session-1',
            Querystring: '',
            NavigationTitle: {
              value: 'Session 1',
              editable: 'Session 1',
            },
          },
          {
            Id: 'bed50e98-2c1a-402e-a8ac-329015cc4b43',
            Styles: ['level2', 'item1', 'even', 'last'],
            Href: '/Sessions/Session-2',
            Querystring: '',
            NavigationTitle: {
              value: 'Session 2',
              editable: 'Session 2',
            },
          },
        ],
      },
      '1': {
        Id: '20434b37-8963-41c7-88bb-0d63af1854d1',
        Styles: ['level1', 'item1', 'even'],
        Href: '/Agenda',
        Querystring: '',
        NavigationTitle: {
          value: 'Agenda',
          editable: 'Agenda',
        },
      },
      '2': {
        Id: 'f9718ba4-5378-43c1-89cd-537301b81f1b',
        Styles: ['level1', 'item2', 'odd'],
        Href: '/Speakers',
        Querystring: '',
        NavigationTitle: {
          value: 'Speakers',
          editable: 'Speakers',
        },
      },
      '3': {
        Id: '0fbd1460-52d5-467a-accd-f955d448ae1d',
        Styles: ['level1', 'item3', 'even'],
        Href: '/Trainings',
        Querystring: '',
        NavigationTitle: {
          value: 'Trainings',
          editable: 'Trainings',
        },
      },
      '4': {
        Id: '96b9ccc8-50fe-4493-8a2c-62807499262d',
        Styles: ['level1', 'item4', 'odd'],
        Href: '/Sponsors',
        Querystring: '',
        NavigationTitle: {
          value: 'Sponsors',
          editable: 'Sponsors',
        },
      },
      '5': {
        Id: 'e5525fb5-fa3d-4eeb-a354-a30d01d79296',
        Styles: ['level1', 'item5', 'even', 'last'],
        Href: '/Organizers',
        Querystring: '',
        NavigationTitle: {
          value: 'Organizers',
          editable: 'Organizers',
        },
      },
    },
    params: {
      FieldNames: 'Default',
      LevelFrom: '{1BB88840-5FB3-4353-AD8D-81136F6FF75A}',
      LevelTo: '{A59325BB-5A27-46F9-8110-9D499715F3BE}',
      Styles: '',
      styles: '',
    },
  },
};

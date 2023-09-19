import * as FEAAS from '@sitecore-feaas/clientside/react';
import { useEffect, useState } from 'react';

export default function ExampleClientsideComponent(props: {
  firstName: string;
  lastName?: string;
  telephone?: string;
  bold?: boolean;
  children?: JSX.Element[];
}) {
  const [counter, setCounter] = useState(0);

  useEffect(() => {
    const interval = setInterval(() => {
      setCounter((c) => c + 1);
    }, 1000);
    return () => clearInterval(interval);
  }, []);

  return (
    <>
      <h2>Clientside</h2>
      <dl style={props.bold ? { fontWeight: 'bold' } : {}}>
        <dt>Description</dt>
        <dd>Interactive UI</dd>
        <dt>Rendered on</dt>
        <dd>Clientside</dd>
        <dt>Data</dt>
        <dd>
          {props.firstName} {props.lastName} / {props.telephone}
        </dd>
        <dt>Clientside hook</dt>
        <dd>
          <var>{counter}</var>s elapsed
        </dd>
        {props.children && props.children.length != 0 && (
          <>
            <dt>Children</dt>
            <dd>{props.children}</dd>
          </>
        )}
      </dl>
    </>
  );
}

FEAAS.registerComponent(ExampleClientsideComponent, {
  name: 'clientside-only',
  title: 'Clientside-only component',
  description: 'Description of my example component',
  thumbnail:
    'https://mss-p-006-delivery.stylelabs.cloud/api/public/content/3997aaa0d8be4eb789f3b1541bd95c58',
  group: 'Examples',
  required: ['firstName'],
  properties: {
    firstName: {
      type: 'string',
      title: 'First name',
    },
    lastName: {
      type: 'string',
      title: 'Last name',
    },
    telephone: {
      type: 'number',
      title: 'Telephone',
      minLength: 10,
    },
    bold: {
      type: 'boolean',
      title: 'Show text in bold weight',
    },
  },
  ui: {
    firstName: {
      'ui:autofocus': true,
      'ui:emptyValue': '',
      'ui:placeholder': 'Write your first name',
    },
    bold: {
      'ui:widget': 'radio',
    },
  },
});

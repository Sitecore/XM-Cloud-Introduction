import { Text, Field, withDatasourceCheck } from '@sitecore-jss/sitecore-jss-nextjs';
import { ComponentProps } from 'lib/component-props';

type SessionProps = ComponentProps & {
  fields: {
    heading: Field<string>;
  };
};

const Session = (props: SessionProps): JSX.Element => (
  <div>
    <p>Session Component</p>
    <Text field={props.fields.heading} />
  </div>
);

export default withDatasourceCheck()<SessionProps>(Session);

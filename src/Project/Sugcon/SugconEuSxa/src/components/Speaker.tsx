import { Text, Field, withDatasourceCheck } from '@sitecore-jss/sitecore-jss-nextjs';
import { ComponentProps } from 'lib/component-props';

type SpeakerProps = ComponentProps & {
  fields: {
    heading: Field<string>;
  };
};

const Speaker = (props: SpeakerProps): JSX.Element => (
  <div>
    <p>Speaker Component</p>
    <Text field={props.fields.heading} />
  </div>
);

export default withDatasourceCheck()<SpeakerProps>(Speaker);

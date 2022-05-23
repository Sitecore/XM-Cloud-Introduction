import { Text, Field, withDatasourceCheck } from '@sitecore-jss/sitecore-jss-nextjs';
import { ComponentProps } from 'lib/component-props';

type CTAProps = ComponentProps & {
  fields: {
    heading: Field<string>;
  };
};

const CTA = (props: CTAProps): JSX.Element => (
  <div>
    <p>CTA Component</p>
    <Text field={props.fields.heading} />
  </div>
);

export default withDatasourceCheck()<CTAProps>(CTA);

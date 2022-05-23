import { Text, Field,ImageField, withDatasourceCheck } from '@sitecore-jss/sitecore-jss-nextjs';
import { ComponentProps } from 'lib/component-props';

type CTAProps = ComponentProps & {
  fields: {
    Heading: Field<string>;
    Text: Field<string>;
    Link: Field<string>;
    Image: ImageField;
  };
};

const CTA = (props: CTAProps): JSX.Element => (
  <div>
    <p>CTA Component</p>
    <Text field={props.fields.Heading} />
  </div>
);

export default withDatasourceCheck()<CTAProps>(CTA);

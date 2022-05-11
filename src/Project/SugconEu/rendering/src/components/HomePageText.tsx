import { Text, Field, withDatasourceCheck, RichText } from '@sitecore-jss/sitecore-jss-nextjs';
import { ComponentProps } from 'lib/component-props';

type HomePageTextProps = ComponentProps & {
  fields: {
    heading: Field<string>;
    body: Field<string>;
    footer: Field<string>;
  };
};

const HomePageText = ({ fields }: HomePageTextProps): JSX.Element => (
  <div className="container-fluid container-md mx-auto w-md-100">
    <div className="pt-5">
      <p className="h2 w-75 text-left">
        <Text field={fields.heading} />
      </p>
      <p className="w-100 text-left lead">
        <RichText field={fields.body} />
      </p>
      <p className="h3 w-100 text-left text-secondary">
        <Text field={fields.footer} />
      </p>
    </div>
  </div>
);

export default withDatasourceCheck()<HomePageTextProps>(HomePageText);

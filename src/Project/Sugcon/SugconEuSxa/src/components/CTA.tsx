import {
  Text,
  RichText,
  Field,
  Link,
  Image,
  LinkField,
  ImageField,
} from '@sitecore-jss/sitecore-jss-nextjs';

interface Fields {
  Headline: Field<string>;
  Text: Field<string>;
  Link: LinkField;
  Image: ImageField;
}

type CTAProps = {
  params: { [key: string]: string };
  fields: Fields;
};

const CTADefaultComponent = (props: CTAProps): JSX.Element => (
  <div className={`component cta ${props.params.styles}`}>
    <div className="component-content">
      <span className="is-empty-hint">CTA Component</span>
    </div>
  </div>
);

export const Default = (props: CTAProps): JSX.Element => {
  if (props.fields) {
    return (
      <div className={`component cta ${props.params.styles}`}>
        <div className="component-content">
          <h2 className="h2">
            <Text field={props.fields.Headline} />
          </h2>
          <RichText field={props.fields.Text} />
          <Image field={props.fields.Image} className="img-fluid" />
          <Link field={props.fields.Link} />
        </div>
      </div>
    );
  }

  return <CTADefaultComponent {...props} />;
};

//export default withDatasourceCheck()<CTAProps>(CTA);

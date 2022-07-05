import {
  Text,
  RichText,
  Field,
  LinkField,
  Link,
  ImageField,
  Image,
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

const CTA = (props: CTAProps): JSX.Element => (
  <div className={`component cta ${props.params.styles}`}>
    <div className="component-content">
      <span className="is-empty-hint">CTA Default view</span>
    </div>
  </div>
);

export const Default = (props: CTAProps): JSX.Element => {
  if (props.fields) {
    return (
      <div className="container component mt-5">
        <div className="d-flex flex-row">
          <div className="col-12 col-md-5 p-5 align-self-stretch bg-light">
            <h3>
              <Text field={props.fields.Headline} />
            </h3>
            <RichText field={props.fields.Text} />
            <Link field={props.fields.Link}></Link>
          </div>
          <div className="align-self-stretch">
            <Image field={props.fields.Image} />
          </div>
        </div>
      </div>
    );
  }
  return <CTA {...props} />;
};

export const Test = (props: CTAProps): JSX.Element => {
  if (props.fields) {
    return (
      <div>
        <p>CTA Component</p>
        <Text field={props.fields.Headline} />
      </div>
    );
  }
  return <CTA {...props} />;
};

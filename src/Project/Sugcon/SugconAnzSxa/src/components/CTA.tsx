import { Text, Field, LinkField, Link, Image, ImageField } from '@sitecore-jss/sitecore-jss-nextjs';

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
      <div className="container component">
        <div className="row justify-content-end cta">
          <div className="col-12 col-lg-7">
            <div className="ctaText">
              <h3>
                <Text field={props.fields.Headline} />
              </h3>
              <p>
                <Text field={props.fields.Text} />
              </p>
              <Link field={props.fields.Link}></Link>
            </div>
          </div>
          <div className="floated ctaImage" style={{ opacity: 1, top: 0 }}>
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

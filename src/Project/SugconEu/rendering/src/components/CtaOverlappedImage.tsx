import { Text, Field, LinkField,Link,Image,ImageField, withDatasourceCheck } from '@sitecore-jss/sitecore-jss-nextjs';
import { RichTextField } from '@sitecore-jss/sitecore-jss-react';
import { ComponentProps } from 'lib/component-props';

type CtaOverlappedImageProps = ComponentProps & {
  fields: {
    Headline: Field<string>;
    Text: RichTextField;
    Link:LinkField;
    BackgroundImage:ImageField;
  };
};

const CtaOverlappedImage = (props: CtaOverlappedImageProps): JSX.Element => (
  <div>
    <p>CtaOverlappedImage Component</p>
    <Text field={props?.fields?.Headline} />
    <Link field={props?.fields?.Link}></Link>
    <Text field={props?.fields?.Text}/>
    <Image field={props?.fields?.BackgroundImage}/>
  </div>
);

export default withDatasourceCheck()<CtaOverlappedImageProps>(CtaOverlappedImage);

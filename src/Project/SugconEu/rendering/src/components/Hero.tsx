import {
  Text,
  Field,
  ImageField,
  Image,
  withDatasourceCheck,
} from '@sitecore-jss/sitecore-jss-nextjs';
import { RichTextField } from '@sitecore-jss/sitecore-jss-react';
import { ComponentProps } from 'lib/component-props';

type HeroProps = ComponentProps & {
  fields: {
    Text: RichTextField;
    Label: Field<string>;
    BackgroundImage: ImageField;
  };
};

const Hero = (props: HeroProps): JSX.Element => (
  <div>
    <p>Hero Component</p>
    <Text field={props?.fields?.Text} />
    <Text field={props?.fields?.Label} />
    <Image field={props?.fields?.BackgroundImage} />
  </div>
);

export default withDatasourceCheck()<HeroProps>(Hero);

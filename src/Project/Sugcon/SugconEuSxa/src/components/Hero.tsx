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

const Hero = ({ fields }: HeroProps): JSX.Element => (
  <div className="row" id="box-search">
    <div className="thumbnail text-center">
      <Image media={fields.BackgroundImage} className="img-fluid"></Image>
      <div className="caption d-none d-md-block">
        <p className="h1 py-4">
          <Text className="contentTitle" field={fields.Text} />
        </p>
      </div>
    </div>
  </div>
);

export default withDatasourceCheck()<HeroProps>(Hero);

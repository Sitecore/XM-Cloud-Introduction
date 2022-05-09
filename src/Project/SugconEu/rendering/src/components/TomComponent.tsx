import {
  withDatasourceCheck,
  ImageField,
  Image,
  Field,
  Text,
} from '@sitecore-jss/sitecore-jss-nextjs';
import { ComponentProps } from 'lib/component-props';

type TomComponentProps = ComponentProps & {
  fields: {
    image: ImageField;
    heading: Field<string>;
  };
};

const TomComponent = ({ fields }: TomComponentProps): JSX.Element => (
  <div className="row" id="box-search">
    <div className="thumbnail text-center">
      <Image media={fields.image} className="img-fluid"></Image>
      <div className="caption d-none d-md-block">
        <p className="h1 py-4">
          <Text className="contentTitle" field={fields.heading} />
        </p>
      </div>
    </div>
  </div>
);

export default withDatasourceCheck()<TomComponentProps>(TomComponent);

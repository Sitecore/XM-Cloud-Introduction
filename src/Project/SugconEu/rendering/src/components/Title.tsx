import { Text, Field, withDatasourceCheck } from '@sitecore-jss/sitecore-jss-nextjs';
import { ComponentProps } from 'lib/component-props';

type TitleProps = ComponentProps & {
  fields: {
    Title: Field<string>;
  };
};

const Title = (props: TitleProps): JSX.Element => (
  <div className="row title">
    <div className="col-12">
      <h1>
        <Text field={props?.fields?.Title} />
      </h1>
    </div>
  </div>
);

export default withDatasourceCheck()<TitleProps>(Title);

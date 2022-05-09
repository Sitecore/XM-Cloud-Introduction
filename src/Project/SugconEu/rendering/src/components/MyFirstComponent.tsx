import { Text, Field, ImageField,LinkField, Link, Image, withDatasourceCheck } from '@sitecore-jss/sitecore-jss-nextjs';
import { ComponentProps } from 'lib/component-props';

type MyFirstComponentProps = ComponentProps & {
  fields: {
    headline: Field<string>;
    image:ImageField;
    link: LinkField;

  };
};

const MyFirstComponent = (props: MyFirstComponentProps): JSX.Element => (
  <div>
    <p>My First Component</p>
    <Image field={props?.fields?.image}/>
    <Text tag="h2" field={props?.fields?.headline} />
    <Link field={props?.fields?.link}></Link>
  </div>
);

export default withDatasourceCheck()<MyFirstComponentProps>(MyFirstComponent);

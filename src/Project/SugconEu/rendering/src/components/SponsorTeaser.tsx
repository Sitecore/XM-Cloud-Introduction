import { Text, Field,ImageField,Image, withDatasourceCheck } from '@sitecore-jss/sitecore-jss-nextjs';
import { ComponentProps } from 'lib/component-props';

type SponsorTeaserProps = ComponentProps & {
  fields: {
    Label: Field<string>;
    SponsorName: Field<string>;
    SponsorUrl: Field<string>;
    SponsorLogo: ImageField;

  };
};

const SponsorTeaser = (props: SponsorTeaserProps): JSX.Element => (
  <div className='row'>
    <p>SponsorTeaser Component</p>
    <div className='col-12 col-md6'>
      <Text field={props?.fields?.Label} />
      <Text field={props?.fields?.SponsorName} />
      <Text field={props?.fields?.SponsorUrl} />
      <Image field={props?.fields?.SponsorLogo} />
    </div>
    <div className='col-12 col-md6'>
      <Image field={props?.fields?.SponsorLogo} />
    </div>
  </div>
);

export default withDatasourceCheck()<SponsorTeaserProps>(SponsorTeaser);

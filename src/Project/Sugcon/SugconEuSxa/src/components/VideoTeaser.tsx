import { Text, Field, withDatasourceCheck } from '@sitecore-jss/sitecore-jss-nextjs';
import { ComponentProps } from 'lib/component-props';

type VideoTeaserProps = ComponentProps & {
  fields: {
    heading: Field<string>;
  };
};

const VideoTeaser = (props: VideoTeaserProps): JSX.Element => (
  <div>
    <p>VideoTeaser Component</p>
    <Text field={props.fields.heading} />
  </div>
);

export default withDatasourceCheck()<VideoTeaserProps>(VideoTeaser);

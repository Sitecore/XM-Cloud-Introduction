import { Text, Field, withDatasourceCheck } from '@sitecore-jss/sitecore-jss-nextjs';
import { ComponentProps } from 'lib/component-props';

type VideoTeaserProps = ComponentProps & {
  fields: {
    Title: Field<string>;
    YoutubeVideoId: Field<string>;
  };
};

const VideoTeaser = (props: VideoTeaserProps): JSX.Element => (
  <div>
    <p>VideoTeaser Component</p>
    <Text field={props.fields.Title} />
    <Text field={props.fields.YoutubeVideoId} />
  </div>
);

export default withDatasourceCheck()<VideoTeaserProps>(VideoTeaser);

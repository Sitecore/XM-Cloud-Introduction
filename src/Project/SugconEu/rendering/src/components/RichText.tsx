import {Field, RichText } from '@sitecore-jss/sitecore-jss-nextjs';

type MyComponentProps = {
  fields: {
    
    Text: Field<string>  
  };
}

const MyComponent = (props: MyComponentProps): JSX.Element => (
  <div className='row richtext'>
    <div className ='col-12'>
      <RichText field={props.fields.Text} />
    </div>
  </div>
);

export default MyComponent;
import { Text, Field, withDatasourceCheck } from '@sitecore-jss/sitecore-jss-nextjs';
import { ComponentProps } from 'lib/component-props';

type FooterProps = ComponentProps & {
  fields: {
    heading: Field<string>;
  };
};

const Footer = (props: FooterProps): JSX.Element => (
  <footer className="text-center text-lg-start bg-secondary">
    <section className="d-flex justify-content-center justify-content-lg-between p-2 mt-5 border-bottom">
      <div className="me-5 d-none d-lg-block text-muted">
        <span className="text-light">I declare this copyrited!</span>
      </div>
      <div>
        <a href="" className="me-4 text-reset">
          <i className="text-light">YouTube</i>
        </a>
        <a href="" className="me-4 text-reset">
          <i className="text-light"> Twitter</i>
        </a>
      </div>
    </section>
  </footer>
);

export default withDatasourceCheck()<FooterProps>(Footer);

import Link from 'next/link';
import { getPublicUrl } from '@sitecore-jss/sitecore-jss-nextjs';

// Prefix public assets with a public URL to enable compatibility with Sitecore Experience Editor.
// If you're not supporting the Experience Editor, you can remove this.
const publicUrl = getPublicUrl();

const Navigation = (): JSX.Element => {
  return (
    <div className="d-flex flex-column flex-md-row align-items-center p-3 px-md-4 mb-3 bg-white border-bottom">
      <h5 className="my-0 mr-md-auto font-weight-normal">
        <Link href="/">
          <a className="text-dark">
            <img src={`${publicUrl}/sc_logo.svg`} alt="Sitecore" />
          </a>
        </Link>
      </h5>
      <nav className="my-2 my-md-0 mr-md-3 d-none d-md-block">
        <Link href="/agenda">
          <a className="p-2 text-dark h5 hidden-md">Agenda</a>
        </Link>
        <Link href="/graphql">
          <a className="p-2 text-dark h5">Speakers</a>
        </Link>
        <Link href="/graphql">
          <a className="p-2 text-dark h5">Organizers</a>
        </Link>
        <Link href="/graphql">
          <a className="p-2 text-dark h5">Past Events</a>
        </Link>
      </nav>
      <nav className="my-2 my-md-0 mr-md-3 d-xs-block d-sm-block d-md-none ">
        <Link href="/agenda">
          <a className="p-2 text-dark h5 hidden-md">Agenda</a>
        </Link>
        <Link href="/graphql">
          <a className="p-2 text-dark h5">Speakers</a>
        </Link>
        <Link href="/graphql">
          <a className="p-2 text-dark h5">Organizers</a>
        </Link>
        <Link href="/graphql">
          <a className="p-2 text-dark h5">Past Events</a>
        </Link>
      </nav>
    </div>
  );
};

export default Navigation;

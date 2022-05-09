const Footer = (): JSX.Element => {
  return (
    <footer className="text-center text-lg-start bg-secondary">
      <section className="d-flex justify-content-center justify-content-between px-5 py-2 mt-5 border-bottom">
        <div className=" d-block text-muted">
          <span className="text-light">I declare this copyrited!</span>
        </div>
        <div>
          <a href="" className="text-light">
            YouTube
          </a>{' '}
          <a href="" className="text-light">
            Twitter
          </a>
        </div>
      </section>
    </footer>
  );
};

export default Footer;

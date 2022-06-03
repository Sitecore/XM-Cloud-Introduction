import React from 'react';
import { useEffect, useState } from 'react';

import { withDatasourceCheck } from '@sitecore-jss/sitecore-jss-nextjs';

const Agenda = (): JSX.Element => (
  <div>
    <p>Agenda Component</p>
  </div>
);

export const Default = (): JSX.Element => {
  const [htmlContent, setHtmlContent] = useState('');
  useEffect(() => {
    async function getHtmlFromSessionize() {
      const html = await (
        await fetch('https://sessionize.com/api/v2/1wt79jwg/view/GridSmart')
      ).text();
      setHtmlContent(html);
    }
    getHtmlFromSessionize();
  }, []);

  return (
    <div className="container component">
      <h1>2022 Agenda</h1>
      <div dangerouslySetInnerHTML={{ __html: htmlContent }} />
    </div>
  );
};

export default withDatasourceCheck()(Agenda);

import { Text, Field, withDatasourceCheck } from '@sitecore-jss/sitecore-jss-nextjs';
import { ComponentProps } from 'lib/component-props';
import { useEffect, useState } from 'react';

type AgendaProps = ComponentProps & {
  fields: {
    
  };
};

const Agenda = (props: AgendaProps): JSX.Element => {
  const htmlContent = '';
  const [token, setToken] = useState('');
  useEffect(() => {
    async function getToken() {
      const token = await (
        await fetch('https://sessionize.com/api/v2/1wt79jwg/view/GridSmart')
      ).text();
      setToken(token);
    }
    getToken();
  }, []);
  console.log(htmlContent);
  return (
    <>
      <div id="agenda" className="w-100 h-100">
        <div dangerouslySetInnerHTML={{ __html: token }} />
      </div>
    </>
  );

};

export default withDatasourceCheck()<AgendaProps>(Agenda);

import type { NextApiRequest, NextApiResponse } from 'next';

const robotsApi = async (_req: NextApiRequest, res: NextApiResponse): Promise<void> => {
  // Ensure response is text/html

  return res.status(200).send('OK');
};

export default robotsApi;

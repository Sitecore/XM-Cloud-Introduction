export const fetchSessionizeData = (url: string): Promise<string> =>
  fetch(url).then((response) => response.text());

export const urlSerializer = (
  url: string,
  params: Record<string, string> = {}
) => {
  if (url.startsWith("/")) {
    url = url.slice(1);
  }

  if (url.endsWith("/")) {
    url = url.slice(0, -1);
  }

  const urlParams = new URLSearchParams();

  for (const key in Object.keys(params)) {
    urlParams.append(key, params[key]);
  }

  return urlParams.toString().length > 0 ? `${url}?${urlParams}` : url;
};

import { api } from './api';

export async function list (url, body) {
  url += '?' + new URLSearchParams(body).toString();
  const response = await api('GET', url);
  const { status, data, message } = response;
  if (status !== false) {
    const { list, paginator } = data;
    return {
      'status': status,
      'list': list,
      'paginator': paginator
    };
  } else {
    return {
      'status': false,
      'message': message ?? 'Cant catch server status'
    };
  }
}

export async function item (url) {
  const response = await api('GET', url);
  const { status, data, message } = response;
  return {
    status,
    data,
    message
  }
}

export async function update (method, url, values) {
  // body: JSON.stringify(values)
  const response = await api(method, url, values);
  const { status, message, data } = response;
  return {
    status,
    message,
    data
  }
}

export async function drop (url, values) {
  values = values || {};
  const response = await api('DELETE', url, values);
  const { status, message } = response;
  return {
    status,
    message
  };
}

export async function getRaw (url) {
  return await api('GET', url);
}

export async function postRaw (url, values) {
  return await api('POST', url, values);
}

export async function putRaw (url, values) {
  return await api('PUT', url, values);
}

export async function deleteRaw (url, values) {
  return await api('DELETE', url, values);
}

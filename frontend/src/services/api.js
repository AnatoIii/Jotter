import axios from 'axios';

axios.interceptors.response.use(response => {
  return response;
}, error => {
  const errorString = error.toString();
  if(errorString.indexOf('code 403') !== -1) {
    window.location.href = '/403';
  }
  else if(errorString.indexOf('code 401') !== -1) {
    window.location.href = '/login';
  }
  else {
    console.log('API INTERCEPT ERROR', error);
    return error;
  }
});

export async function api(method, url, data) {
  try {
    const token = localStorage.getItem('token');
    const headers = token !== false ? {
      Authorization: `Bearer ${token}`
    } : {};

    data = data ?? {};

    const response = await axios({
      method, url, data, headers
    });

    if(response.status >= 200 && response.status < 300) {
      return response.data;
    } else {
      // add 401 not authorized response
      // auto-logout
      console.log('RESPONSE BAD STATUS', response);
      return {
        status: false,
        message: 'Сервер ответил некорректным статусом'
      };
    }
  } catch (e) {
    console.log('API RESPONSE ERROR', e);
    return {
      status: false,
      message: 'Ошибка на стороне сервера. Подробности в консоли'
    };
  }
}

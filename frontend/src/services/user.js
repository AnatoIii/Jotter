import decode from "jwt-decode";
import { api } from './api';

const getToken = () => {
  return localStorage.getItem('token');
};

const setToken = (token) => {
  localStorage.setItem('token', token);
  console.log(localStorage);
};

const deleteToken = () => {
  localStorage.removeItem('token');
};

const isTokenExpired = (token) => {
  try {
    const decoded = decode(token);
    return (decoded.exp < Date.now() / 1000);
  }
  catch (err) {
    return false;
  }
};

const loggedIn = () => {
  const token = getToken();
  return !!token && !isTokenExpired(token);
};

export async function currentUser () {
  // if (loggedIn) {
  //   const token = getToken();
  //   if (!token) {
  //     return false;
  //   }
  //   try {
  //     const response = await api('get', '/api/current-user');
  //     const { status, data } = response;
  //     if (status !== false) {
  //       return data;
  //     } else {
  //       logout();
  //       return false;
  //     }
  //   } catch (e) {
  //     logout();
  //     return false;
  //   }
  // } else {
  //   return false;
  // }
}

export async function login (params) {
  try {
    const response = await api('post', '/login', params);
    console.log(response);
    const { error, isSuccessful, responseResult } = response;
    
    if (isSuccessful) {
      setToken(responseResult.accessToken);
      return {
        successful: true
      };
    } else {
      return {
        successful: false,
        message: error
      };
    }
  } catch (e) {
    return {
      successful: false,
      message: e
    };
  }
}

export async function register (params) {
  try {
    const response = await api('post', '/register', params);
    const { error, isSuccessful, responseResult } = response;
    if (isSuccessful) {
      setToken(responseResult);
      return {
        successful: true
      };
    } else {
      return {
        successful: false,
        message: error
      };
    }
  } catch (e) {
    return {
      successful: false,
      message: e
    };
  }
}

export function logout () {
  deleteToken();
}

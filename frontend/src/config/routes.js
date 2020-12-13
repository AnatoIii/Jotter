export default [
  // home
  {
    path: '/',
    module: 'home',
    title: 'Main page',
  },

  // auth pages
  {
    path: '/login',
    module: 'user/login',
    title: 'Log in',
  },
  {
    path: '/register',
    module: 'user/register',
    title: 'Register'
  },
  // system
  {
    path: '/404',
    module: '404',
    title: '404 - Page not found'
  },
  {
    path: '/403',
    module: '403',
    title: '403 - Access restricted'
  },
];

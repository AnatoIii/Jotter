import React, { Fragment } from 'react'
import { withRouter, Redirect } from 'react-router-dom'
import { connect } from 'react-redux'
import { compose } from "redux";
import NProgress from 'nprogress'
import { Helmet } from 'react-helmet'
import Loader from '../components/Loader'
import LoginLayout from 'layouts/Login'
import MainLayout from 'layouts/Main'
import SystemLayout from 'layouts/System'
import RegisterLayout from 'layouts/Register'

const Layouts = {
  login: LoginLayout,
  main: MainLayout,
  system: SystemLayout,
  register: RegisterLayout
};

class LayoutLoader extends React.PureComponent {
  previousPath = '';

  componentDidUpdate(prevProps) {
    const { location } = this.props;
    const { prevLocation } = prevProps;
    if (location !== prevLocation) {
      window.scrollTo(0, 0)
    }
  }

  render() {
    const {
      children,
      location: { pathname, search },
      user,
    } = this.props;

    // NProgress Management
    const currentPath = pathname + search;
    if (currentPath !== this.previousPath) {
      NProgress.start()
    }

    setTimeout(() => {
      NProgress.done();
      this.previousPath = currentPath
    }, 300);

    // Layout Rendering
    const getLayout = () => {
      if(pathname === '/login') {
        return 'login'
      }
      if(pathname === '/register') {
        return 'register'
      }

      if(pathname === '/403' || pathname === '/404') {
        return 'system'
      }

      return 'main';
    };

    const Container = Layouts[getLayout()];
    const isUserAuthorized = user.authorized;
    const isUserLoading = user.loading;

    const isLoginLayout = getLayout() === 'login';
    const isMainLayout = getLayout() === 'main';

    const BootstrappedLayout = () => {

       // admin rules
       if (isMainLayout && !isUserLoading && !isUserAuthorized) {
         return <Redirect to="/login" />
       }

      // redirect to admin
      if (isLoginLayout && isUserAuthorized) {
        return <Redirect to="/" />
      }

      // show loader when user in check authorization process, not authorized yet and not on login pages
      if (isUserLoading && !isUserAuthorized && !isLoginLayout) {
        return <Loader />
      }

      // in other case render previously set layout
      return <Container>{children}</Container>
    };

    return (
      <Fragment>
        <Helmet titleTemplate="Jotter | %s" title="Main page" />
        {BootstrappedLayout()}
      </Fragment>
    )
  }
}
export default compose(withRouter, connect(({ user }) => ({ user })))(LayoutLoader)

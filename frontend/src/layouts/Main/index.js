import React from 'react'
import { BackTop, Layout } from 'antd'
import { connect } from 'react-redux'
import { withRouter } from 'react-router-dom'
import {BreadcrumbsItem} from 'react-breadcrumbs-dynamic'

import classNames from 'classnames'
import TopBar from 'components/TopBar'
import Settings from 'components/Settings'

import 'assets/global.scss'
import {compose} from "redux";

const mapStateToProps = ({ settings }) => ({
  isBorderless: settings.isBorderless,
  isSquaredBorders: settings.isSquaredBorders,
  isFixedWidth: settings.isFixedWidth,
  isMenuShadow: settings.isMenuShadow,
});

class MainLayout extends React.PureComponent {
  render() {
    const {
      children,
      isBorderless,
      isSquaredBorders,
      isFixedWidth,
      isMenuShadow
    } = this.props;

    /* if you need to hide breadcrumbs on 1st level
    const { history } = this.props;
    { history.location.pathname !== '/' && (
      <BreadcrumbsItem to='/'>Главная</BreadcrumbsItem>
    )} */

    return (
      <Layout
        className={classNames({
          settings__borderLess: isBorderless,
          settings__squaredBorders: isSquaredBorders,
          settings__fixedWidth: isFixedWidth,
          settings__menuShadow: isMenuShadow,
          settings__menuTop: '',
        })}
      >
        <BreadcrumbsItem to='/'>Main page</BreadcrumbsItem>
        <BackTop />
        <Settings />
        <Layout>
          <Layout.Header>
            <TopBar />
          </Layout.Header>
          <Layout.Content style={{ height: '100%', position: 'relative' }}>
            <div className="utils__content" style={{padding: 10, maxWidth:'140rem'}}>{children}</div>
          </Layout.Content>
        </Layout>
      </Layout>
    )
  }
}

export default compose(withRouter, connect(mapStateToProps))(MainLayout)


import React from 'react'
import { connect } from 'react-redux'
import { BrowserRouter, Route, Switch } from 'react-router-dom'
import Loadable from 'react-loadable'
import loadRoutes from 'config/routes';
import loadModules from 'config/modules';
import Loader from 'components/Loader'
import NotFoundPage from 'pages/404'
import LayoutLoader from './layoutLoader'

const loadable = loader => (
  Loadable({
    loader,
    delay: false,
    loading: () => <Loader />,
  })
);

const flattenItems = (items, key) =>
  items.reduce((flattenedItems, item) => {
    flattenedItems.push(item);
    if (Array.isArray(item[key])) {
      return flattenedItems.concat(flattenItems(item[key], key))
    }
    return flattenedItems
  }, []);

class Router extends React.Component {
  render() {
    const { history } = this.props;
    const userRoles = this.props.user.role;
    const moduleRoutes = [...loadModules, ...loadRoutes];
    const flattenModuleRoutes = flattenItems(moduleRoutes, 'children');

    const routes = flattenModuleRoutes.filter(i => i.module).map((item)=> {
      let granted = true;
      if(item.roles) {
        granted = false;
        if(userRoles && userRoles.length > 0) {
          userRoles.forEach(userRole => {
            if(item.roles.includes(userRole)) {
              granted = true;
            }
          });
        }
      }

      const loadableModule = (granted !== true) ? import('../pages/403') : import('../pages/' + item.module);
      const path = (item.path) ?? '/' + item.module + '/:action?/:id?';
      return {
        path,
        component: loadable(() => loadableModule),
        exact: true,
      };
    });

    return (
      <BrowserRouter history={history}>
        <LayoutLoader>
          <Switch>
            {routes.map(route => (
              <Route
                path={route.path}
                component={route.component}
                key={route.path}
                exact={route.exact}
              />
            ))}
            <Route component={NotFoundPage} />
          </Switch>
        </LayoutLoader>
      </BrowserRouter>
    )
  }
}

export default connect(
  ({ user }) => ({ user })
)(Router)

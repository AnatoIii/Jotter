import React from 'react'
import ReactDOM from 'react-dom'
import { Provider } from 'react-redux'
import thunk from 'redux-thunk'
import { routerMiddleware } from 'connected-react-router'
import { createStore, applyMiddleware, compose } from 'redux'
import createSagaMiddleware from 'redux-saga'
import { createBrowserHistory } from 'history'
import reducers from 'redux/reducers'
import sagas from 'redux/sagas'
import {ThroughProvider} from 'react-through'

import { ConfigProvider } from 'antd';
import ru_RU from 'antd/es/locale/ru_RU';

import Router from 'system/router'
import * as serviceWorker from './system/serviceWorker'
import 'antd/dist/antd.css';
import './colorizer.css'
import axios from 'axios'

const history = createBrowserHistory();
const sagaMiddleware = createSagaMiddleware();
const routeMiddleware = routerMiddleware(history);
const middlewares = [thunk, sagaMiddleware, routeMiddleware];
const store = createStore(reducers(history), compose(applyMiddleware(...middlewares)));
sagaMiddleware.run(sagas);

ReactDOM.render(
  <ThroughProvider>
    <ConfigProvider locale={ru_RU}>
      <Provider store={store}>
        <Router history={history} />
      </Provider>
    </ConfigProvider>
  </ThroughProvider>,
  document.getElementById('root'),
);

axios.defaults.baseURL = 'http://localhost:5000';

serviceWorker.register();
export { store, history }

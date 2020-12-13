import {all, takeEvery, put, call} from 'redux-saga/effects'
import {currentUser, logout} from 'services/user'
import actions from './actions'

export function* LOAD_CURRENT_ACCOUNT() {
  yield put({
    type: 'user/SET_STATE',
    payload: {
      loading: true,
    },
  });

  const response = yield call(currentUser);
  if(response) {
    const {guid, chatId, username} = response;
    yield put({
      type: 'user/SET_STATE',
      payload: {
        username,
        guid,
        chatId,
        authorized: true,
      },
    })
  } else {
    // reset state
    yield all([
      RESET_STATE_ACCOUNT()
    ])
  }

  yield put({
    type: 'user/SET_STATE',
    payload: {
      loading: false,
    },
  })
}

export function* LOGOUT() {
  yield call(logout);
  yield all([
    RESET_STATE_ACCOUNT()
  ])
}

function* RESET_STATE_ACCOUNT() {
  yield put({
    type: 'user/SET_STATE',
    payload: {
      username : '',
      guid: '',
      chatId: '',
      authorized: false,
      loading: false,
    },
  })
}

export default function* rootSaga() {
  yield all([
    takeEvery(actions.LOAD_CURRENT_ACCOUNT, LOAD_CURRENT_ACCOUNT),
    takeEvery(actions.LOGOUT, LOGOUT),
    LOAD_CURRENT_ACCOUNT(), // run once on app load to check user auth
  ])
}

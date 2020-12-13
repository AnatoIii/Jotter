import actions from './actions'
const initialState = {
  username : '',
  email : '',
  firstName : '',
  lastName : '',
  middleName : '',
  phone : '',
  internalPhone : '',
  role : '',
  code: '',
  authorized: false,
  loading: false,
};

export default function userReducer(state = initialState, action) {
  switch (action.type) {
    case actions.SET_STATE:
      return { ...state, ...action.payload };
    default:
      return state
  }
}

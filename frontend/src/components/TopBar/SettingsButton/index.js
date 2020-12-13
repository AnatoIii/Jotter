import React from 'react'
import {withRouter} from "react-router";
import {connect} from "react-redux";
import {compose} from "redux";
import style from './style.module.scss'

const mapStateToProps = ({settings}) => ({
  isSettingsOpen: settings.isSettingsOpen,
})

class SettingsButton extends React.Component {

  settingsMenu = () => {
    const {dispatch, isSettingsOpen} = this.props;
    dispatch({
      type: 'settings/CHANGE_SETTING',
      payload: {
        setting: 'isSettingsOpen',
        value: !isSettingsOpen,
      },
    });
  }

  render() {
    return (
      <div className={style.bitcoinPrice}>
        <a href="!#" onClick={this.settingsMenu}><i className="icmn icmn-cog utils__spin-delayed--pseudo-selector" size="large" /></a>
      </div>
    )
  }
}

export default compose(withRouter, connect(mapStateToProps))(SettingsButton)

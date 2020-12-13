import React from 'react'
import { connect } from 'react-redux'
import { Scrollbars } from 'react-custom-scrollbars'
import { Switch } from 'antd'
import styles from './style.module.scss'

const mapStateToProps = ({ settings }) => ({ settings })

class Settings extends React.Component {
  changeSetting = (setting, value) => {
    const { dispatch } = this.props
    dispatch({
      type: 'settings/CHANGE_SETTING',
      payload: {
        setting,
        value,
      },
    })
  }

  closeSettings = () => {
    const { dispatch } = this.props
    dispatch({
      type: 'settings/CHANGE_SETTING',
      payload: {
        setting: 'isSettingsOpen',
        value: false,
      },
    })
  }

  render() {
    const {
      settings: {
        isLightTheme,
        isSettingsOpen,
        isMenuCollapsed,
        isBorderless,
        isMenuShadow,
        isSquaredBorders,
        isFixedWidth,
      },
    } = this.props

    return (
      <div
        className={isSettingsOpen ? `${styles.settings} ${styles.settingsOpened}` : styles.settings}
      >
        <Scrollbars style={{ height: '100vh' }}>
          <div className={styles.container}>
            <div className={styles.title}>
              Настройки отображения
              <button
                className={`${styles.close} fa fa-times`}
                onClick={this.closeSettings}
                type="button"
              />
            </div>
            <div className={styles.item}>
              <Switch
                checked={isMenuCollapsed}
                onChange={value => {
                  this.changeSetting('isMenuCollapsed', value)
                }}
              />
              <span className={styles.itemLabel}>Мобильное меню</span>
            </div>
            <div className={styles.item}>
              <Switch
                checked={isMenuShadow}
                onChange={value => {
                  this.changeSetting('isMenuShadow', value)
                }}
              />
              <span className={styles.itemLabel}>Тень меню</span>
            </div>
            <div className={styles.item}>
              <Switch
                checked={isLightTheme}
                onChange={value => {
                  this.changeSetting('isLightTheme', value)
                }}
              />
              <span className={styles.itemLabel}>Светлая тема</span>
            </div>
            <div className={styles.item}>
              <Switch
                checked={isBorderless}
                onChange={value => {
                  this.changeSetting('isBorderless', value)
                }}
              />
              <span className={styles.itemLabel}>Границы таблиц</span>
            </div>
            <div className={styles.item}>
              <Switch
                checked={isSquaredBorders}
                onChange={value => {
                  this.changeSetting('isSquaredBorders', value)
                }}
              />
              <span className={styles.itemLabel}>Прямые границы</span>
            </div>
            <div className={styles.item}>
              <Switch
                checked={isFixedWidth}
                onChange={value => {
                  this.changeSetting('isFixedWidth', value)
                }}
              />
              <span className={styles.itemLabel}>Фикс. ширина</span>
            </div>
          </div>
        </Scrollbars>
      </div>
    )
  }
}

export default connect(
  (mapStateToProps)
)(Settings)

import React from 'react'
import BreadcrumbsWrapper from './BreadcrumbsWrapper'

import Notificator from './Notificator'
import ProfileMenu from './ProfileMenu'
import styles from './style.module.scss'

class TopBar extends React.Component {
  render() {
    return (
      <div className={styles.topbar}>
        <div className="mr-auto">
          <BreadcrumbsWrapper />
        </div>
        <div className="mr-4">
          <Notificator />
        </div>
        <ProfileMenu />
      </div>
    )
  }
}
export default TopBar

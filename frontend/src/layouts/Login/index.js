import React from 'react'
import { Layout } from 'antd'
import { withRouter } from 'react-router-dom'

import 'assets/global.scss'
import styles from './style.module.scss'

class LoginLayout extends React.PureComponent {

  render() {
    const { children } = this.props

    return (
      <Layout>
        <Layout.Content>
          <div className={`${styles.layout} ${styles.light}`}>
            <div className={styles.content}>{children}</div>
          </div>
        </Layout.Content>
      </Layout>
    )
  }
}

export default withRouter(LoginLayout)

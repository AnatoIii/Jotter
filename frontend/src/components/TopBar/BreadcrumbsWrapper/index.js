import React from 'react'
import {Link} from 'react-router-dom'
import {Breadcrumbs} from 'react-breadcrumbs-dynamic'

import styles from './style.module.scss'

class BreadcrumbsWrapper extends React.Component {
  render() {
    return (
      <div className={styles.breadcrumbs}>
        <div className={styles.path}>
          <Breadcrumbs
            item={Link}
            finalItem={'b'}
            finalProps={{
              style: {color: '#111'}
            }}
            separator={<span style={{paddingLeft:5,paddingRight:5}}>·</span>}
          />
        </div>
      </div>)
  }
}

export default BreadcrumbsWrapper;

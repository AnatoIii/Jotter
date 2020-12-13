import React from 'react'
import styles from './style.module.scss'

const Footer = () => (
  <div className={styles.footer}>
    <div className={styles.inner}>
      <div className={styles.bottom}>
        <div className="row">
          <div className="col-sm-12">
            <div className={styles.copyright}>
              <span>
                © 2020{' '}
                <a href="https://cds.spb.ru" target="_blank" rel="noopener noreferrer">
                  CDS
                </a>
                <br />
                All rights reserved
              </span>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
)

export default Footer

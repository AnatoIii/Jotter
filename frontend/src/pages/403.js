import React, { PureComponent } from 'react'
import { withRouter  } from 'react-router-dom'
import { Helmet } from 'react-helmet'
import { Link } from 'react-router-dom'

class Forbidden extends PureComponent {

  componentDidMount() {
    const { pathname } = this.props.location;
    if(pathname !== '/403') {
      this.props.history.push('/403');
    }
  }

  render() {
    return (
      <div
        style={{
          minHeight: 'calc(100vh - 500px)',
          display: 'flex',
          justifyContent: 'center',
          alignItems: 'center',
        }}
      >
        <Helmet title="403" />
        <div
          style={{
            maxWidth: '560px',
            backgroundColor: '#fff',
            padding: '80px 30px',
            margin: '100px auto',
            borderRadius: '10px',
            flex: '1',
          }}
        >
          <div
            style={{
              maxWidth: '400px',
              margin: '0 auto',
            }}
          >
            <h1 className="font-size-36 mb-2">Access not permitted</h1>
            <p className="mb-3">You have not enough permissions to access this</p>
            <h1 className="font-size-80 mb-4 font-weight-bold">403 —</h1>
            <Link to="/" className="btn">
              &larr; Back to Main page
            </Link>
          </div>
        </div>
      </div>
    )
  }
}

export default withRouter(Forbidden)

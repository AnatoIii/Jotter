import React from 'react'
import {Form, Input, Button, notification} from 'antd'
import {Helmet} from 'react-helmet'
import {register} from 'services/user';
import styles from './style.module.scss'
import {connect} from "react-redux";
import {LoginOutlined} from "@ant-design/icons";
import background from '../../../assets/images/login.jpg';

class Login extends React.Component {
  formRef = React.createRef();

  state = {
    loading: false,
  };

  onFinish = async (data) => {
    this.setState({ loading: true});
    const result = await register(data);
    this.setState({ loading: false});
    
    if(result.successful !== false) {
      const {dispatch} = this.props;
      dispatch({
        type: 'user/LOAD_CURRENT_ACCOUNT',
      });
      window.location = '/';
    } else {
      notification.warning({
        message: 'Log in',
        description: result.message ?? 'Cannot log in',
      })
    }
  };

  loginLink = () => {
    window.location.replace("/login");
  }

  render() {
    const {loading} = this.state;
    return (
      <div className={styles.loginMain}>
        <div className={styles.loginImage}>
          <img src={background} alt="background" />
        </div>
        <Helmet title="Log in"/>
        <div className={styles.block}>
          <h3>Register</h3>
          <Form layout="vertical" ref={this.formRef} onFinish={this.onFinish}>
          <Form.Item
              name="email"
              label="Email"
              rules={[
                {
                  required: true,
                  message: 'Please input your email!',
                },
              ]}
            >
              <Input />
            </Form.Item>

            <Form.Item
              name="name"
              label="Name"
              rules={[
                {
                  required: true,
                  message: 'Please input your username!',
                },
              ]}
            >
              <Input />
            </Form.Item>

            <Form.Item
              name="password"
              label="Password"
              rules={[
                {
                  required: true,
                  message: 'Please input your password!',
                },
              ]}
              hasFeedback
            >
              <Input.Password />
            </Form.Item>

            <Form.Item
              name="confirm"
              label="Confirm Password"
              dependencies={['password']}
              hasFeedback
              rules={[
                {
                  required: true,
                  message: 'Please confirm your password!',
                },
                ({ getFieldValue }) => ({
                  validator(value) {
                    console.log(getFieldValue('password'), value)
                    if (!value || getFieldValue('password') === getFieldValue('confirm')) {
                      return Promise.resolve();
                    }
                    return Promise.reject('The two passwords that you entered do not match!');
                  },
                }),
              ]}
            >
              <Input.Password />
            </Form.Item>
            <div className="form-actions">
              <Button
                loading={loading}
                type="primary"
                className="width-150 mr-4"
                htmlType="submit"
                id="register_button"
              >
                Register
              </Button>
              <Button
                type="primary"
                className="width-150 mr-4"
                htmlType="button"
                onClick={() => this.loginLink()}
              >
                Back to log in
              </Button>
            </div>
          </Form>
        </div>
      </div>
    )
  }
}

export default connect(
  ({ user }) => ({ user })
)(Login)

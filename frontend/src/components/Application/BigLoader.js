import React from 'react'
import {LoadingOutlined} from "@ant-design/icons";
import {Spin} from "antd";

const BigLoader = () => {
  const loadingIcon = <LoadingOutlined style={{ fontSize: 54 }} spin />;
  return (
    <div style={{padding:50, textAlign:'center'}}>
      <Spin indicator={loadingIcon} />
    </div>
  );
};
export default BigLoader;

import React from 'react';
import { Container } from 'reactstrap';
import { NavMenu } from './NavMenu';

const Layout = (props) => {
  const displayName = Layout.name;

  return (
    <div>
      <NavMenu />
      <Container tag="main">
        {props.children}
      </Container>
    </div>
  );
}

export default Layout;
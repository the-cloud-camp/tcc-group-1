import React from 'react';
import {
  BrowserRouter as Router,
  Route,
  Switch
} from "react-router-dom";
import Login from './pages/Login';
import PawnRegister from './pages/pawn-register/PawnRegister';
import Main from './pages/Main';
import Home from './pages/Home'

function App() {
  return (
    <Router>
      <Switch>
        <Route path='/login'>
          <Login />
        </Route>
        <Route path='/main'>
          <Main />
          </Route>
        <Route path='/home'>
          <Home />
        </Route>
        <Route path='/pawn-register'>
          <PawnRegister />
        </Route>
      </Switch>
    </Router>
  );
}

export default App;

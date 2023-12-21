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
import CustomerService from './pages/customer-service/CustomerService';
import PawnSearch from './pages/pawn-search/Pawn-search';


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
        <Route path='/customer-service'>
          <CustomerService />
        </Route>
        <Route path='/pawn-info'>
          <PawnSearch />
        </Route>
      </Switch>
    </Router>
  );
}

export default App;

import logo from './logo.svg';
import { lazy } from 'react';
import './App.css';
import SignInPage from './views/Customer/SignInPage/SignInPage';
import PreRegister from './views/Customer/PreRegisterPage/PreRegister';
import AppRoutes from './routes/AppRoutes';

const About = lazy(() => import("./About"));


function App() {
  

  return (
    <div className="App">
         <AppRoutes />
    </div>
  );
}

export default App;

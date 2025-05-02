import React from "react";
import ReactDOM from "react-dom/client";
import "./index.css";
import App from "./App";
import "./assets/Styles/Mainbtn.css";
import { BrowserRouter } from "react-router-dom";
import { DeliveryProvider } from "./contexts/DeliveryContext";
import { ConfirmProvider } from "./contexts/ConfirmContext";
import { AuthProvider } from "./contexts/AuthContext";
import AlertProvider from "./contexts/AlertContext";

const root = ReactDOM.createRoot(document.getElementById("root"));
root.render(
  <AuthProvider>
    <BrowserRouter>
      <ConfirmProvider>
        <DeliveryProvider>
          <AlertProvider>
            <App />
          </AlertProvider>
        </DeliveryProvider>
      </ConfirmProvider>
    </BrowserRouter>
  </AuthProvider>
);

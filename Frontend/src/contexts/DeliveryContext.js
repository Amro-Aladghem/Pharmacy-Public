import { createContext, useContext } from "react";
import { useState } from "react";

export const DeliveryContext = createContext(null);

export const DeliveryProvider = ({ children }) => {
  const [open, setOpen] = useState(false);

  return (
    <DeliveryContext.Provider value={{ open, setOpen }}>
      {children}
    </DeliveryContext.Provider>
  );
};

export const useDelvieryCheck = () => {
  const openObj = useContext(DeliveryContext);

  return openObj;
};

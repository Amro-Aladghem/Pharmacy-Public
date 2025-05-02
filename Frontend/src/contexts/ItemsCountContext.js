import { createContext, useState, useEffect } from "react";
import { useAuth } from "./AuthContext";

export const ItemsCountContext = createContext(null);

export const ItemsCountProvider = ({ children }) => {
  const [itemsCount, setItemsCount] = useState(0);
  const { authInfo } = useAuth();

  function handelItemsCountChange(count, isFirstTime) {
    setItemsCount(count);

    if (!isFirstTime) {
      const cart = JSON.parse(localStorage.getItem("cart"));
      cart.numberOfItems = count;
      localStorage.setItem("cart", JSON.stringify(cart));
    }
  }

  useEffect(() => {
    if (!authInfo.loggedIn) {
      return;
    }

    const cart = JSON.parse(localStorage.getItem("cart"));

    const count = !cart ? 0 : cart.numberOfItems;

    setItemsCount(count);
  }, [authInfo]);

  return (
    <ItemsCountContext.Provider
      value={{ itemsCount, setItemsCount: handelItemsCountChange }}
    >
      {children}
    </ItemsCountContext.Provider>
  );
};

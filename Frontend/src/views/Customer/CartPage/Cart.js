import "./Cart.css";
import LocalMallIcon from "@mui/icons-material/LocalMall";
import { Box } from "@mui/material";
import Typography from "@mui/joy/Typography";
import CartItem from "../../../components/CartItem/CartItem";
import PaymentSummery from "./PaymentSummery";
import Grid from "@mui/material/Grid2";
import { ShowCustomerCart } from "../../../services/orderServices";
import { useEffect, useState, useContext } from "react";
import { useNavigate } from "react-router-dom";
import { CircularProgress } from "@mui/material";
import { ItemsCountContext } from "../../../contexts/ItemsCountContext";
import { useAuth } from "../../../contexts/AuthContext";
import LoginAlarm from "../../../components/LoginAlarm/LoginAlarm";

export function Cart() {
  const navigate = useNavigate();

  const { itemsCount, setItemsCount } = useContext(ItemsCountContext);
  const { authInfo } = useAuth();

  const [loading, setLoading] = useState(true);
  const [cartItems, setCartItems] = useState([]);
  const [result, setResult] = useState({
    isSuccess: false,
    message: "!سلة مشرتياتك فارغة ",
  });
  const [cartInfo, setCartInfo] = useState({
    cartId: -1,
    totalPrice: 0,
    deliveryFees: 0,
    subPrice: 0,
    serviceFees: 0,
  });

  async function fetchData(CustomerId, cartId) {
    const response = await ShowCustomerCart(cartId, CustomerId);

    setLoading(false);

    if (!response.IsSuccess) {
      setResult({ ...result, isSuccess: false });
      return;
    }

    if (!response.result) {
      setResult({ ...result, isSuccess: true, message: "سلة مشترياتك فارغة " });

      return;
    }

    if (response.result.deliveryFees == 0) {
      navigate("/cart/check-delivery");
      return;
    }

    setResult({ ...result, isSuccess: true });
    setCartItems(response.result.cartItmes);

    const responseResult = response.result;

    setCartInfo({
      ...cartInfo,
      cartId: responseResult.cartId,
      totalPrice: responseResult.totalPrice,
      deliveryFees: responseResult.deliveryFees,
      subPrice: responseResult.subPrice,
      serviceFees: responseResult.serviceFees,
    });
  }

  function handelChangeItems(updatedObj, action = "update") {
    let cartItemsAfterUpdate = [];

    if (action == "update") {
      cartItemsAfterUpdate = cartItems.map((Item) => {
        if (Item.itemId == updatedObj.itemId)
          return {
            ...Item,
            price: updatedObj.price,
            quantity: updatedObj.quantity,
          };

        return Item;
      });
    } else {
      cartItemsAfterUpdate = cartItems.filter((Item) => {
        if (Item.itemId == updatedObj.itemId) return false;

        return true;
      });
    }

    setCartItems(cartItemsAfterUpdate);

    const newSubPrice = cartInfo.subPrice + updatedObj.subPriceDiff;
    const newTotalPrice = cartInfo.totalPrice + updatedObj.subPriceDiff;

    setCartInfo({
      ...cartInfo,
      subPrice: newSubPrice,
      totalPrice: newTotalPrice,
    });

    setItemsCount(cartItemsAfterUpdate.length);
  }

  useEffect(() => {
    const cart = JSON.parse(localStorage.getItem("cart"));

    if (authInfo.loggedIn && cart) {
      fetchData(authInfo.customerId, cart.cartId);
      return;
    }

    setLoading(false);
  }, [authInfo]);

  const CartItems = cartItems.map((Item, index) => {
    return (
      <CartItem
        key={index}
        itemId={Item.itemId}
        quantity={Item.quantity}
        price={Item.price}
        name={Item.product.phProductName}
        image={Item.product.imageURL}
        priceForOne={Item.product.price}
        handelItemChange={handelChangeItems}
        stoke={Item.product.stoke}
      />
    );
  });

  return (
    <>
      <Box
        sx={{
          width: "100%",
          display: "flex",
          justifyContent: "center",
          gap: "13px",
          marginTop: "5px",
        }}
      >
        <Typography
          level="h1"
          sx={{
            backgroundColor: "aqua",
            borderRadius: "8px",
          }}
        >
          سلة مشترياتك
        </Typography>
        <LocalMallIcon
          sx={{
            fontSize: "50px",
            color: "rgb(255, 86, 114)",
          }}
        />
      </Box>

      <Box
        sx={{
          display: "flex",
          justifyContent: "center",
          flexDirection: "column",
          alignItems: "center",
        }}
      >
        {loading ? (
          <>
            <CircularProgress size={40} />
          </>
        ) : !authInfo.loggedIn ? (
          <LoginAlarm />
        ) : !result.isSuccess || !itemsCount ? (
          <>
            <h2>{result.message}</h2>
            <img
              src="/empty-cart.png"
              width={"270px"}
              height={"270px"}
              alt=""
            />
          </>
        ) : (
          <>
            <Grid
              container
              spacing={{ md: 10, xs: 3 }}
              sx={{
                display: "flex",
                width: "98vw",
                justifyContent: "center",
                direction: "rtl",
                height: "auto",
              }}
            >
              <Grid
                size={{ md: 7, xs: 12 }}
                sx={{ display: "flex", justifyContent: "center" }}
              >
                <Box
                  sx={{
                    width: { md: "50vw", xs: "100vw" },
                    maxHeight: "100%",
                    borderRadius: "8px",
                  }}
                >
                  {CartItems}
                </Box>
              </Grid>

              <Grid
                size={{ md: 5, xs: 12 }}
                sx={{ display: "flex", justifyContent: "center" }}
              >
                <PaymentSummery
                  subFees={cartInfo.subPrice}
                  totalFees={cartInfo.totalPrice}
                  serviceFees={cartInfo.serviceFees}
                  deliveryFees={cartInfo.deliveryFees}
                />
              </Grid>
            </Grid>

            <Box
              sx={{
                display: "flex",
                justifyContent: "center",
                marginTop: "35px",
              }}
            >
              <button
                className="main-btn"
                style={{ width: "fit-content" }}
                onClick={() => {
                  navigate("/cart/checkout");
                }}
              >
                استكمال عملية الشراء
                <div className="arrow-wrapper">
                  <div className="arrow"></div>
                </div>
              </button>
            </Box>
          </>
        )}
      </Box>
    </>
  );
}

export default Cart;

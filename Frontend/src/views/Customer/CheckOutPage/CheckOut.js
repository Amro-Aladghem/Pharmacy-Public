import "./CheckOut.css";
import { Box } from "@mui/material";
import PaymentMethodes from "./PaymentMethodes";
import Typography from "@mui/joy/Typography";
import { CircularProgress } from "@mui/material";
import CheckLocation from "./CheckLocation";
import { useState, useEffect } from "react";
import PaymentSummerySection from "./PaymentSummerySection";
import LoginAlarm from "../../../components/LoginAlarm/LoginAlarm";
import {
  handleOnDeliveryCheckOut,
  handleCheckOutViaWhatsApp,
} from "../../../services/orderServices";
import { useAuth } from "../../../contexts/AuthContext";
import { useAlert } from "../../../contexts/AlertContext";

export function CheckOut() {
  const { authInfo } = useAuth();
  const { setAlert } = useAlert();

  const [loading, setLoading] = useState(false);
  const [checkOutResult, setCheckOutResult] = useState(true);
  const [paymentMethode, setpaymentMethode] = useState(4);
  const [initLaunch, setInitLaunch] = useState({
    cartExists: false,
    initLoading: true,
  });

  const [verfiyLocation, setVerfiyLocation] = useState({
    loading: true,
    isChanged: false,
    result: false,
  });

  async function handleError() {
    setLoading(false);
    setAlert({
      type: "error",
      message: "فشل اضافة الطلب الرجاء اعادة المحاولة !",
      open: true,
    });
  }

  async function handelCheckOutProcess() {
    setLoading(true);

    if (paymentMethode == 4) {
      handleOnDeliveryCheckOut(paymentMethode, handleError);
      return;
    } else {
      handleCheckOutViaWhatsApp(
        authInfo.customerId,
        paymentMethode,
        handleError
      );
    }
  }

  useEffect(() => {
    const cart = localStorage.getItem("cart");

    const ItemsCount = !cart ? 0 : JSON.parse(cart).numberOfItems;

    setInitLaunch((prev) => {
      return { ...prev, initLoading: false };
    });

    if (ItemsCount != 0) {
      setInitLaunch((prev) => {
        return { ...prev, cartExists: true };
      });

      return;
    }
  }, []);

  return (
    <>
      <Box
        sx={{
          width: "100%",
          display: "flex",
          justifyContent: "center",
          alignItems: "center",
          direction: "rtl",
          flexDirection: "column",
        }}
      >
        {initLaunch.initLoading ? (
          <CircularProgress size={40} />
        ) : !authInfo.loggedIn ? (
          <LoginAlarm />
        ) : !initLaunch.cartExists ? (
          <>
            <h2>{"سلة مشترياتك فارغة"}</h2>
            <img
              src="/empty-cart.png"
              width={"270px"}
              height={"270px"}
              alt=""
            />
          </>
        ) : (
          <Box
            sx={{
              backgroundColor: "rgba(156, 153, 153, 0.353)",
              borderRadius: "8px",
              display: "flex",
              flexDirection: "column",
              justifyContent: "center",
              alignItems: "center",
              padding: "10px 10px",
              gap: "20px",
              width: { md: "370px", xs: "350px" },
              marginTop: "10px",
            }}
          >
            <Typography
              level="h1"
              sx={{
                backgroundColor: "rgb(255, 86, 114)",
                borderRadius: "8px",
                padding: "5px 5px",
              }}
            >
              اتمام عملية شراء الطلب
            </Typography>

            <CheckLocation
              verfiyLocation={verfiyLocation}
              setVerfiyLocation={setVerfiyLocation}
            />

            <Typography
              level="title-lg"
              sx={{
                backgroundColor: "aqua",
                borderRadius: "8px",
                padding: "5px 5px",
              }}
            >
              طرق الدفع المتاحة
            </Typography>

            <PaymentMethodes
              paymentMethode={paymentMethode}
              setpaymentMethode={setpaymentMethode}
            />

            <PaymentSummerySection />

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
                disabled={
                  verfiyLocation.loading || verfiyLocation.isChanged || loading
                }
                onClick={() => handelCheckOutProcess()}
              >
                {verfiyLocation.loading || loading
                  ? "الرجاء الأنتظار..."
                  : "تأكيد الطلب"}

                <div className="arrow-wrapper">
                  <div className="arrow"></div>
                </div>
              </button>
            </Box>
          </Box>
        )}
      </Box>
    </>
  );
}

export default CheckOut;

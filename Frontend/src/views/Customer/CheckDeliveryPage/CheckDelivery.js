import { useAuth } from "../../../contexts/AuthContext";
import { useDelvieryCheck } from "../../../contexts/DeliveryContext";
import DeliverFeesDialog from "../../../components/Dialogs/DeliveryFeesDialog/DeliveryFeesDialog";
import { useState, useEffect, useContext } from "react";
import { Box } from "@mui/material";
import Typography from "@mui/joy/Typography";
import DeliveryDiningIcon from "@mui/icons-material/DeliveryDining";
import { useNavigate } from "react-router-dom";
import { ItemsCountContext } from "../../../contexts/ItemsCountContext";
import LoginAlarm from "../../../components/LoginAlarm/LoginAlarm";

export function CheckDelivery() {
  const { authInfo } = useAuth();
  const { setOpen } = useDelvieryCheck();
  const { itemsCount, setItemsCount } = useContext(ItemsCountContext);
  const navigate = useNavigate();

  function handleDeliveryStatus(isDeliveried) {
    if (!isDeliveried) {
      const cartId = JSON.parse(localStorage.getItem("cart")).cartId;

      const cartAfterClear = {
        cartId: cartId,
        pharmacyId: null,
        numberOfItems: 0,
      };

      localStorage.setItem("cart", JSON.stringify(cartAfterClear));
      setItemsCount(0, true);

      navigate("/");
      return;
    }

    navigate("/cart");
  }

  useEffect(() => {}, [authInfo]);

  return (
    <Box
      sx={{
        width: "100%",
        display: "flex",
        alignItems: "center",
        direction: "rtl",
        flexDirection: "column",
      }}
    >
      {!authInfo.loggedIn ? (
        <LoginAlarm />
      ) : itemsCount == 0 ? (
        <>
          <p>سلة مشترياتك فارغة لا داعي للتحقق</p>
          <img src="/empty-cart.png" width={"250px"} height={"250px"} alt="" />
        </>
      ) : (
        <>
          <Box
            sx={{
              backgroundColor: "rgba(128, 128, 128, 0.355)",
              padding: "8px 8px",
              borderRadius: "12px",
              width: { xs: "300px", md: "380px" },
              marginTop: "10%",
              textAlign: "center",
            }}
          >
            <Typography
              level="h1"
              sx={{
                backgroundColor: "aqua",
                borderRadius: "12px",
                padding: "3px 3px",
              }}
            >
              رسوم التوصيل
            </Typography>

            <Box
              sx={{
                display: "flex",
                flexDirection: "column",
                gap: "10px",
                alignItems: "center",
              }}
            >
              <DeliveryDiningIcon fontSize="large" />
              <Typography level="title-lg">
                *يبدو أنك لم تقم بالتحقق من رسوم التوصيل عند وضعك المنتج ف السلة
                ولكن يمكنك الأن من خلال الزر أدناه
              </Typography>
              <Typography
                level="title-md"
                sx={{
                  backgroundColor: "rgb(255, 86, 114)",
                  marginTop: "20px",
                  borderRadius: "8px",
                }}
              >
                بعد التحقق ستتمكن من استكمال عملية الشراء😊
              </Typography>
              <button
                className="main-btn"
                style={{ width: "fit-content" }}
                onClick={() => {
                  setOpen(true);
                }}
              >
                التحقق من التوصيل
                <div className="arrow-wrapper">
                  <div className="arrow"></div>
                </div>
              </button>
            </Box>
          </Box>
          <DeliverFeesDialog
            isCartPage={true}
            handleDeliveryStatus={handleDeliveryStatus}
          />
        </>
      )}
    </Box>
  );
}

export default CheckDelivery;

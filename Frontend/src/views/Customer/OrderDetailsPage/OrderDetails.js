import "./OrderDetails.css";
import OrderStepper from "../../../components/Prograssbars/OrderStepper/OrderStepper";
import OrderDetailsHeader from "./Componants/OrderDetailsHeader";
import OrderInfo from "./Componants/OrderInfo";
import PharmacyInfo from "./Componants/PharmacyInfo";
import OrderProducts from "./Componants/OrderProducts";
import { useState, useEffect } from "react";
import { useParams } from "react-router-dom";
import { useAuth } from "../../../contexts/AuthContext";
import { GetOrderStatusAndDetails } from "../../../services/orderServices";
import LoginAlarm from "../../../components/LoginAlarm/LoginAlarm";
import { Box } from "@mui/material";
import { CircularProgress } from "@mui/material";
import CancelOrder from "./Componants/CancelOrder";
import DeclinedOrder from "./Componants/DeslinedOrder";

export function OrderDetails() {
  const { orderId } = useParams();
  const { authInfo } = useAuth();

  const [orderInfo, setOrderInfo] = useState({});
  const [orderStatus, setOrderStatus] = useState({});
  const [loading, setLoading] = useState(true);
  const [result, setResult] = useState(false);

  async function fetchData() {
    setLoading(true);

    const response = await GetOrderStatusAndDetails(
      orderId,
      authInfo.customerId
    );

    setLoading(false);

    if (!response.IsSuccess) {
      setResult(false);
      return;
    }

    setOrderInfo(response.result);
    setOrderStatus(response.result.status);
    setResult(true);
  }

  const handelStatusChange = (newStatus) => {
    setOrderStatus(newStatus);
  };

  useEffect(() => {
    if (!authInfo.loggedIn) {
      setLoading(false);
      return;
    }

    fetchData();
  }, [authInfo]);

  return (
    <>
      {loading ? (
        <Box
          sx={{
            display: "flex",
            justifyContent: "center",
            flexDirection: "column",
            alignItems: "center",
          }}
        >
          <CircularProgress size={40} />
        </Box>
      ) : !authInfo.loggedIn ? (
        <Box
          sx={{
            display: "flex",
            justifyContent: "center",
            flexDirection: "column",
            alignItems: "center",
          }}
        >
          <LoginAlarm />
        </Box>
      ) : !result ? (
        <Box
          sx={{
            display: "flex",
            justifyContent: "center",
            flexDirection: "column",
            alignItems: "center",
          }}
        >
          <p>فشل تحميل الطلب!</p>
        </Box>
      ) : orderStatus.id == 5 ? (
        <CancelOrder
          paymentMethodeId={orderInfo.paymentMethodeId}
          orderId={orderInfo.id}
        />
      ) : orderStatus.id == 6 ? (
        <DeclinedOrder
          paymentMethodeId={orderInfo.paymentMethodeId}
          orderId={orderInfo}
        />
      ) : (
        <>
          <OrderDetailsHeader
            statusId={orderStatus.id}
            handelStatusChange={handelStatusChange}
          />

          <OrderStepper statusId={orderStatus.id} />

          <OrderInfo
            orderId={orderInfo.id}
            statusName={orderStatus.arabicName}
            date={orderInfo.dateOfOrder.split("T")[0]}
          />

          <PharmacyInfo
            phName={orderInfo.pharmacy.arabicName}
            phone={orderInfo.pharmacy.phone}
            phId={orderInfo.pharmacy.pharmacyId}
          />

          <OrderProducts
            items={orderInfo.products}
            total={orderInfo.totalPrice}
            subPrice={orderInfo.subPrice}
            serviceFees={orderInfo.serviceFees}
            deliveryFees={orderInfo.deliveryFees}
          />
        </>
      )}
    </>
  );
}

export default OrderDetails;

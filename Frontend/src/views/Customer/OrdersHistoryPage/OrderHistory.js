import OrderHistoryCard from "../../../components/Card/History/OrderHistoryCard";
import Grid from "@mui/material/Grid2";
import { useState, useEffect } from "react";
import { GetOrdersHistoryForTheCustomer } from "../../../services/orderServices";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../../../contexts/AuthContext";
import LoginAlarm from "../../../components/LoginAlarm/LoginAlarm";

export function OrderHistory() {
  const navigate = useNavigate();

  const [result, setResult] = useState(true);
  const [loading, setLoading] = useState(true);
  const [historyOrders, setHistoryOrders] = useState([]);

  const { authInfo } = useAuth();

  async function fetchData(CustomerId) {
    const response = await GetOrdersHistoryForTheCustomer(CustomerId);

    setLoading(false);

    if (!response.IsSuccess || !response.result) {
      setResult(false);
      return;
    }

    setResult(true);

    setHistoryOrders([...historyOrders, ...response.result]);
  }

  const Orders = historyOrders.map((Order) => {
    return (
      <Grid
        size={{ xs: 12, sm: 6, md: 4 }}
        sx={{ display: "flex", justifyContent: "center" }}
      >
        <OrderHistoryCard
          key={Order.id}
          OrderId={Order.id}
          PharmacyId={Order.pharmacyId}
          PharamcyName={Order.pharmacyName}
          PharmacyImage={Order.phImageURL}
          Date={Order.date.split("T")[0]}
          StatusName={Order.status.arabicName}
          StatusId={Order.status.id}
        />
      </Grid>
    );
  });

  useEffect(() => {
    if (!authInfo.loggedIn) {
      return;
    }

    fetchData(authInfo.customerId);
  }, [authInfo]);

  return (
    <>
      <div
        style={{ direction: "rtl", display: "flex", justifyContent: "center" }}
      >
        <h1>جميع طلباتك السابقة </h1>
      </div>

      <Grid
        container
        sx={{
          display: "flex",
          justifyContent: "center",
          alignItems: "center",
          marginTop: "20px",
        }}
      >
        {!authInfo.loggedIn ? (
          <LoginAlarm />
        ) : !result ? (
          <p>لا يوجد لديك اي طلبات سابقةأو انه حدث خطأ </p>
        ) : (
          Orders
        )}
      </Grid>
    </>
  );
}

export default OrderHistory;

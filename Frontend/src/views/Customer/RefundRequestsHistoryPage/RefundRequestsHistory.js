import { GetRefundRequestHistory } from "../../../services/requestServices";
import { useState, useEffect } from "react";
import { useAuth } from "../../../contexts/AuthContext";
import LoginAlarm from "../../../components/LoginAlarm/LoginAlarm";
import RefundRequestCard from "./RefundRequestCard";
import Grid from "@mui/material/Grid2";
import { CircularProgress } from "@mui/material";
import { Box } from "@mui/material";

export function RefundRequestsHistory() {
  const { authInfo } = useAuth();
  const [result, setResult] = useState(true);
  const [loading, setLoading] = useState(true);
  const [requests, setRequests] = useState([]);

  async function fetchData() {
    const response = await GetRefundRequestHistory(authInfo.customerId);

    setLoading(false);

    if (!response.IsSuccess) {
      setResult(false);
      return;
    }

    setRequests(response.result);
  }

  const requestsCards = requests.map((request, index) => {
    return (
      <RefundRequestCard
        key={index}
        refundId={request.id}
        reffrenceId={request.refferenceId}
        date={request.dateAndTimeOfRequest.split("T")[0]}
        statusName={request.status.arabicName}
      />
    );
  });

  useEffect(() => {
    if (!authInfo.loggedIn) {
      return;
    }

    fetchData();
  }, [authInfo]);

  return (
    <Box
      container
      sx={{
        display: "flex",
        justifyContent: "center",
        width: "100%",
        flexDirection: "column",
        alignItems: "center",
        marginTop: "15px",
      }}
    >
      {!authInfo.loggedIn ? (
        <LoginAlarm />
      ) : loading ? (
        <CircularProgress size={40} />
      ) : !result ? (
        <p>فشل التحميل , الرجاء اعادة تحميل الصفحة</p>
      ) : requests.length == 0 ? (
        <p>لا يوجد لديك أية طلبات استرجاع</p>
      ) : (
        <Grid
          container
          sx={{ display: "flex", justifyContent: "center", width: "100%" }}
          spacing={10}
        >
          {requestsCards}
        </Grid>
      )}
    </Box>
  );
}

export default RefundRequestsHistory;

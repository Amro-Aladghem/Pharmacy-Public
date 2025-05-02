import { MeetingHistoryCard } from "../../../components/Card/History/MeetingHistoryCard";
import { GetMeetingReqHistoryForCustomer } from "../../../services/requestServices";
import { useAuth } from "../../../contexts/AuthContext";
import { useState, useEffect } from "react";
import LoginAlarm from "../../../components/LoginAlarm/LoginAlarm";
import { CircularProgress } from "@mui/material";
import Grid from "@mui/material/Grid2";

export function MeetingHistory() {
  const [result, setResult] = useState(true);
  const [loading, setLoading] = useState(true);
  const [historyRequests, setHistoryRequests] = useState([]);

  const { authInfo } = useAuth();

  async function fetchData(customerId) {
    const response = await GetMeetingReqHistoryForCustomer(customerId);

    setLoading(false);

    if (!response.IsSuccess) {
      setResult(false);
      return;
    }

    setHistoryRequests(response.result);
  }

  const Requests = historyRequests.map((Request, index) => {
    return (
      <Grid
        size={{ xs: 12, sm: 6, md: 4 }}
        sx={{ display: "flex", justifyContent: "center" }}
      >
        <MeetingHistoryCard
          key={index}
          RequestId={Request.requsetId}
          PharamcyName={Request.pharmacyName}
          PharmacyImage={Request.pharmacyImageURL}
          StatusName={Request.statusArabicName}
          Date={Request.date.split("T")[0]}
          StatusId={Request.statusId}
        />
      </Grid>
    );
  });

  useEffect(() => {
    if (!authInfo.loggedIn) {
      setLoading(false);
      return;
    }

    fetchData(authInfo.customerId);
  }, [authInfo]);

  return (
    <>
      <Grid
        container
        sx={{
          display: "flex",
          justifyContent: "center",
          alignItems: "center",
          marginTop: "20px",
        }}
      >
        {loading ? (
          <CircularProgress size={40} />
        ) : !authInfo.loggedIn ? (
          <LoginAlarm />
        ) : !result ? (
          <p>لا يوجد لديك اي طلبات سابقةأو انه حدث خطأ </p>
        ) : (
          Requests
        )}
      </Grid>
    </>
  );
}

export default MeetingHistory;

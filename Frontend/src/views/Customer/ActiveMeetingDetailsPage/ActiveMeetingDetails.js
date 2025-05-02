import MeetingDetailsHeader from "./Componants/MeetingDetailsHeader";
import MeetingDetailsPrice from "./Componants/MeetingDetailsPrice";
import MeetingRequestInfo from "./Componants/MeetingRequestInfo";
import MeetingStatus from "./Componants/MeetingStatus";
import PharmacyInfo from "./Componants/MeetingPharmacyInfo";
import { GetMeetingRequestInfo } from "../../../services/requestServices";
import { useParams } from "react-router-dom";
import { useState, useEffect } from "react";
import { useAuth } from "../../../contexts/AuthContext";
import { Box, CircularProgress } from "@mui/material";
import LoginAlarm from "../../../components/LoginAlarm/LoginAlarm";
import CancelMeetingRequest from "./Componants/CancelMeeting";
import DeslinedMeeting from "./Componants/DeslinedMeeting";

export function AcitveMeetingDetails() {
  const { requestId } = useParams();
  const { authInfo } = useAuth();

  const [requestInfo, setRequestInfo] = useState({});
  const [requestStatus, setRequestStatus] = useState({});
  const [loading, setLoading] = useState(true);
  const [result, setResult] = useState(false);

  async function fetchData() {
    const response = await GetMeetingRequestInfo(
      requestId,
      authInfo.customerId
    );

    setLoading(false);

    if (!response.IsSuccess) {
      return;
    }

    setRequestInfo(response.result);

    setRequestStatus(response.result.meetingReqStatus);

    setResult(true);
  }

  async function handelStatusChange(newStatus) {
    setRequestStatus(newStatus);
  }

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
            width: "100%",
            flexDirection: "column",
            justifyContent: "center",
          }}
        >
          <CircularProgress size={40} sx={{ color: "inherit" }} />
        </Box>
      ) : !authInfo.loggedIn ? (
        <Box
          sx={{
            display: "flex",
            justifyContent: "center",
            width: "100%",
            flexDirection: "column",
            alignItems: "center",
          }}
        >
          <LoginAlarm />
        </Box>
      ) : !result ? (
        <Box sx={{ display: "flex", justifyContent: "center", width: "100%" }}>
          <p>فشل التحميل معلومات الحجز أو أنها منتهية الصلاحية</p>
        </Box>
      ) : requestStatus.id == 4 ? (
        <DeslinedMeeting requestId={requestInfo.requestId} />
      ) : requestStatus.id == 5 ? (
        <CancelMeetingRequest requestId={requestInfo.requestId} />
      ) : (
        <>
          <MeetingDetailsHeader
            statusId={requestStatus.id}
            handelStatusChange={handelStatusChange}
          />
          <MeetingStatus
            statusId={requestStatus.id}
            meetingURL={requestStatus.meetingURL}
          />

          <MeetingRequestInfo
            requestId={requestInfo.requestId}
            date={requestInfo.dateOfRequest.split("T")[0]}
            status={requestStatus.arabicName}
          />

          <PharmacyInfo
            pharmacyId={requestInfo.pharmacy.pharmacyId}
            pharmacyName={requestInfo.pharmacy.arabicName}
          />

          <MeetingDetailsPrice price={requestInfo.price} />
        </>
      )}
    </>
  );
}

export default AcitveMeetingDetails;

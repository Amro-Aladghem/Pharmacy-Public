import { Box } from "@mui/material";
import Typography from "@mui/joy/Typography";
import Button from "@mui/joy/Button";
import { GetMostActiveRequestStatusForCustomer } from "../../../../services/requestServices";
import { useAuth } from "../../../../contexts/AuthContext";
import { CancelRequestMeetingByCustomer } from "../../../../services/requestServices";
import { useParams } from "react-router-dom";
import { useAlert } from "../../../../contexts/AlertContext";

export function MeetingDetailsHeader({ statusId, handelStatusChange }) {
  const { authInfo } = useAuth();
  const { requestId } = useParams();
  const { setAlert } = useAlert();

  async function handleReffreshClick() {
    const response = await GetMostActiveRequestStatusForCustomer(
      authInfo.customerId
    );

    if (!response.IsSuccess) {
      setAlert({
        type: "error",
        message: "فشل تحديث الصفحة الرجاء اعادة المحاولة لاحقا!",
        open: true,
      });
      return;
    }

    handelStatusChange(response.result);
  }

  async function handleCancelRequestMeeting() {
    const response = await CancelRequestMeetingByCustomer(
      authInfo.customerId,
      requestId
    );

    if (!response.IsSuccess) {
      setAlert({
        type: "error",
        message: "فشل الغاء الحجز الرجاء اعادة المحاولة",
        open: true,
      });
      return;
    }

    window.location.href = "/";
  }

  return (
    <Box
      sx={{
        direction: "rtl",
        display: "flex",
        marginTop: "15px",
        padding: "0px 10px",
        flexDirection: { xs: "column", md: "row" },
      }}
    >
      <Typography level="h3" sx={{ marginBottom: "8px" }}>
        تفاصيل حالة المكالمة المحجوزة
      </Typography>

      <Box
        sx={{
          display: "flex",
          marginRight: "auto",
          gap: "10px",
          marginLeft: { xs: "0px", md: "5px" },
        }}
      >
        {statusId != 6 ? (
          <Button
            sx={{
              padding: "5px 5px",
              backgroundColor: "aqua",
              fontSize: "16px",
              borderRadius: "8px",
              color: "black",
              ":hover": {
                backgroundColor: "black",
                color: "white",
              },
            }}
            onClick={() => {
              handleReffreshClick();
            }}
          >
            تحديث الصفحة
          </Button>
        ) : null}

        {statusId <= 2 ? (
          <Button
            sx={{
              padding: "5px 5px",
              backgroundColor: "rgb(255, 86, 114)",
              fontSize: "16px",
              borderRadius: "8px",
              ":hover": {
                backgroundColor: "rgb(255, 86, 114)",
                color: "white",
              },
            }}
            onClick={() => {
              handleCancelRequestMeeting();
            }}
          >
            الغاء الحجز
          </Button>
        ) : null}
      </Box>
    </Box>
  );
}

export default MeetingDetailsHeader;

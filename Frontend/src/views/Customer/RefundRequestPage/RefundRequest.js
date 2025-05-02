import Typography from "@mui/joy/Typography";
import { Box, TextareaAutosize } from "@mui/material";
import { useAuth } from "../../../contexts/AuthContext";
import LoginAlarm from "../../../components/LoginAlarm/LoginAlarm";
import { useLocation, useParams, useNavigate } from "react-router-dom";
import { useState, useEffect } from "react";
import { AddRefundRequestForMeetingRequest } from "../../../services/requestServices";
import { AddRefundRequestForTheOrder } from "../../../services/requestServices";
import { useAlert } from "../../../contexts/AlertContext";

export function RefundRequest() {
  const location = useLocation();
  const { type } = useParams();
  const { authInfo } = useAuth();
  const { setAlert } = useAlert();

  const navigate = useNavigate();

  const [loading, setLoading] = useState(false);
  const [result, setResult] = useState({ isDone: false, message: "" });
  const [additionalMessage, setAdditionalMessage] = useState("");
  const [reffernceId, setReffrenceId] = useState(0);

  async function HandledRefundRequest() {
    if (!reffernceId) return;

    setLoading(true);

    const Request = {
      CustomerId: authInfo.customerId,
      TypeName: type,
      RefferenceId: reffernceId,
      AdditionalInformation: additionalMessage,
    };

    const response = await (type == "order"
      ? AddRefundRequestForTheOrder(Request)
      : AddRefundRequestForMeetingRequest(Request));

    setLoading(false);

    if (!response.IsSuccess) {
      setResult({ ...result, isDone: false, message: response.error });
      return;
    }

    setAlert({
      type: "success",
      message: "تم التقديم بنجاح سيتم الرد عليك في أقرب وقت !",
    });
    navigate("/");
  }

  useEffect(() => {
    const params = new URLSearchParams(location.search);
    const reffernceId = params.get("id");

    if (reffernceId) setReffrenceId(reffernceId);
  }, [authInfo, location]);

  return (
    <Box
      sx={{
        display: "flex",
        flexDirection: "column",
        width: "100%",
        direction: "rtl",
        alignItems: "center",
      }}
    >
      {!authInfo.loggedIn ? (
        <LoginAlarm />
      ) : (
        <Box
          sx={{
            display: "flex",
            alignItems: "center",
            flexDirection: "column",
            gap: "25px",
            marginTop: "10px",
          }}
        >
          <Typography
            level="h1"
            sx={{
              marginBottom: "15px",
            }}
          >
            طلب استرداد المال
          </Typography>
          <Typography level="body-lg">
            رقم الطلب المراد التقديم له : #{reffernceId}
          </Typography>
          <TextareaAutosize
            minRows={8}
            value={additionalMessage}
            placeholder="(اختياري) أية ملاحظات اضافية "
            onChange={(event) => setAdditionalMessage(event.target.value)}
            style={{
              resize: "none",
              width: "250px",
              backgroundColor: "aqua",
              borderRadius: "8px",
              padding: "6px 6px",
            }}
          />
          <button
            className="main-btn"
            style={{ width: "fit-content" }}
            onClick={() => {
              HandledRefundRequest();
            }}
            disabled={loading}
          >
            تقديم طلب الأسترجاع
            <div className="arrow-wrapper">
              <div className="arrow"></div>
            </div>
          </button>
        </Box>
      )}
    </Box>
  );
}

export default RefundRequest;

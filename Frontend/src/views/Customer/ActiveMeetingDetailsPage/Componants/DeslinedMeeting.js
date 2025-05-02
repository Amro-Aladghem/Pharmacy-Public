import { Box } from "@mui/material";
import Typography from "@mui/joy/Typography";
import { useNavigate } from "react-router-dom";

export function DeslinedMeeting({ requestId }) {
  const navigate = useNavigate();

  return (
    <Box
      sx={{
        display: "flex",
        justifyContent: "center",
        flexDirection: "column",
        alignItems: "center",
        marginTop: "10px",
      }}
    >
      <Typography level="h4">لقد تم رفض هذا الحجز يبدو بسبب الضغط</Typography>
      <img src="/DeclinedOrder.png" alt="" width={"250px"} height={"250px"} />

      {
        <button
          className="main-btn"
          style={{ width: "fit-content" }}
          onClick={() => {
            navigate(`refund-request/meeting?id=${requestId}`);
          }}
        >
          استرجاع الأموال
          <div className="arrow-wrapper">
            <div className="arrow"></div>
          </div>
        </button>
      }
    </Box>
  );
}

export default DeslinedMeeting;

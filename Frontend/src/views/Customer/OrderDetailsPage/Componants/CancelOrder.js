import { Box } from "@mui/material";
import Typography from "@mui/joy/Typography";
import { useNavigate } from "react-router-dom";

export function CancelOrder({ paymentMethodeId, orderId }) {
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
      <Typography level="h4">لقد تم الغاء هذا الطلب</Typography>
      <img src="/CancelledOrder.png" alt="" width={"250px"} height={"250px"} />

      {paymentMethodeId != 4 ? (
        <button
          className="main-btn"
          style={{ width: "fit-content" }}
          onClick={() => {
            navigate(`/refund-request/order?id=${orderId}`);
          }}
        >
          استرجاع الأموال
          <div className="arrow-wrapper">
            <div className="arrow"></div>
          </div>
        </button>
      ) : null}
    </Box>
  );
}

export default CancelOrder;

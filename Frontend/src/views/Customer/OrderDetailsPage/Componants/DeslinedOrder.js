import { Box } from "@mui/material";
import Typography from "@mui/joy/Typography";
import { useNavigate } from "react-router-dom";

export function DeclinedOrder({ paymentMethodeId, orderId }) {
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
      <Typography level="h4">
        تم رفض طلبك من الصيدلية , قد يكون بسبب الضغظ
      </Typography>
      <img src="/DeclinedOrder.png" alt="" width={"250px"} height={"250px"} />

      {paymentMethodeId !== 4 ? (
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

export default DeclinedOrder;

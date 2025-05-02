import { Box } from "@mui/material";
import Typography from "@mui/joy/Typography";
import Button from "@mui/joy/Button";
import { GetOrderRealTimeStatus } from "../../../../services/orderServices";
import { useNavigate, useParams } from "react-router-dom";
import { CancelOrderByCustomer } from "../../../../services/orderServices";
import { useConfirm } from "../../../../contexts/ConfirmContext";
import { useAuth } from "../../../../contexts/AuthContext";
import { useAlert } from "../../../../contexts/AlertContext";

export function OrderDetailsHeader({ statusId, handelStatusChange }) {
  const { orderId } = useParams();
  const confirm = useConfirm();
  const { setAlert } = useAlert();

  const { authInfo } = useAuth();
  const navigate = useNavigate();

  async function handleReffreshClick() {
    const response = await GetOrderRealTimeStatus(orderId);

    if (!response.IsSuccess) {
      setAlert({ type: "error", message: "فشل تحديث الحالة ", open: true });
      return;
    }

    setAlert({ type: "success", message: "تم تحديث الحالة ", open: true });
    handelStatusChange(response.result);
  }

  async function handleCancelClick() {
    const result = await confirm("هل أنت متأكد من الغاء الطلب؟");

    if (!result) {
      return;
    }

    const response = await CancelOrderByCustomer(orderId, authInfo.customerId);

    if (!response.IsSuccess) {
      setAlert({
        type: "error",
        message:
          "فشل الغاء الطلب , الرجاء اعادة المحاولة أو التواصل مع الدعم الفني!",
        open: true,
      });

      return;
    }

    navigate("/");
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
      <Typography level="h3" sx={{ marginTop: "8px" }}>
        تفاصيل حالة الطلب
      </Typography>

      <Box
        sx={{
          display: "flex",
          marginRight: "auto",
          gap: "10px",
          marginLeft: { xs: "0px", md: "5px" },
        }}
      >
        {statusId <= 4 ? (
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
            onClick={() => handleReffreshClick()}
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
            onClick={() => handleCancelClick()}
          >
            الغاء الطلب
          </Button>
        ) : null}
      </Box>
    </Box>
  );
}

export default OrderDetailsHeader;

import Button from "@mui/material/Button";
import Dialog from "@mui/material/Dialog";
import DialogActions from "@mui/material/DialogActions";
import DialogContent from "@mui/material/DialogContent";
import DialogContentText from "@mui/material/DialogContentText";
import DialogTitle from "@mui/material/DialogTitle";
import { useContext } from "react";
import { DeliveryContext } from "../../../contexts/DeliveryContext";
import { CircularProgress } from "@mui/material";
import { GetDeliveryFees } from "../../../services/pharmacyServices";
import { useLocation } from "react-router-dom";
import { useState, useEffect } from "react";

export function DeliverFeesDialog({
  handleDeliveryStatus,
  isCartPage = false,
}) {
  const location = useLocation();

  const { open, setOpen } = useContext(DeliveryContext);
  const [loading, setLoading] = useState(false);
  const [result, setResult] = useState({
    isSucess: true,
    errorMessage: "",
    isHasDelivery: true,
    Fees: 0,
  });
  const [checkInfo, setCheckInfo] = useState({
    isChecked: false,
    pharmacyId: 0,
    customerId: 0,
  });

  useEffect(() => {
    let pharmacyId = null;

    if (isCartPage) {
      pharmacyId = JSON.parse(localStorage.getItem("cart")).pharmacyId;
    } else {
      const params = new URLSearchParams(location.search);
      pharmacyId = Number(params.get("pharmacy"));
    }

    const customer = JSON.parse(sessionStorage.getItem("customer"));

    if (!pharmacyId || !customer) {
      setResult({
        ...result,
        isSucess: false,
        errorMessage: "حدث خطأ ما ,يجب تسجيل الدخول",
      });
      return;
    }

    setCheckInfo({
      ...checkInfo,
      pharmacyId: pharmacyId,
      customerId: customer.customerId,
    });
  }, [location.search]);

  async function fetchData() {
    setLoading(true);
    setCheckInfo({ ...checkInfo, isChecked: true });

    const response = await GetDeliveryFees(
      checkInfo.pharmacyId,
      checkInfo.customerId
    );

    setLoading(false);

    if (!response.IsSuccess) {
      setResult({ ...result, isSucess: false, errorMessage: response.error });
      return;
    }

    setResult({
      ...result,
      isHasDelivery: response.result.isDelivered,
      Fees: response.result.fees,
    });

    if (isCartPage) handleDeliveryStatus(response.result.isDelivered);
  }

  return (
    <>
      <Dialog
        open={open}
        onClose={() => setOpen(!open)}
        aria-labelledby="alert-dialog-title"
        aria-describedby="alert-dialog-description"
        dir="rtl"
      >
        <DialogTitle id="alert-dialog-title">
          {"تحقق من رسوم التوصيل "}
        </DialogTitle>
        <DialogContent>
          <DialogContentText
            id="alert-dialog-description"
            sx={{ color: "black", backgroundColor: "rgb(255, 86, 114)" }}
          >
            {!checkInfo.isChecked ? (
              "تحقق اذا كانت هذه الصيدلية تتوفر لديها خدمة التوصيل لموقعك,تعرف على مقدار رسوم التوصيل"
            ) : loading ? (
              <CircularProgress size={40} />
            ) : !result.isSucess ? (
              <p>{result.errorMessage}</p>
            ) : !result.isHasDelivery ? (
              "هذه الصيدلية لا توصل الى منطقتك"
            ) : (
              `\nرسم التوصيل الى منطقتك هي:${result.Fees}jd`
            )}
          </DialogContentText>
        </DialogContent>
        <DialogActions>
          <Button disabled={checkInfo.isChecked} onClick={() => fetchData()}>
            تحقق
          </Button>
        </DialogActions>
      </Dialog>
    </>
  );
}

export default DeliverFeesDialog;

import "./ProductPage.css";
import Grid from "@mui/material/Grid2";
import LocalMallIcon from "@mui/icons-material/LocalMall";
import { Box } from "@mui/material";
import { useState } from "react";
import { ItemsCountContext } from "../../../contexts/ItemsCountContext";
import { useContext } from "react";
import { useAuth } from "../../../contexts/AuthContext";
import Typography from "@mui/joy/Typography";
import ProductionQuantityLimitsIcon from "@mui/icons-material/ProductionQuantityLimits";
import { useConfirm } from "../../../contexts/ConfirmContext";
import { HandleAddItemToCart } from "../../../services/orderServices";
import { useAlert } from "../../../contexts/AlertContext";

export function ProductInfo({ info, pharmacyId }) {
  const [counter, setCounter] = useState(1);
  const { authInfo } = useAuth();
  const { setAlert } = useAlert();
  const confirm = useConfirm();

  const { setItemsCount, itemsCount } = useContext(ItemsCountContext);

  async function handelAddToCartClicked() {
    if (!authInfo.loggedIn) {
      setAlert({ type: "error", message: "يجب تسجيل الدخول أولا", open: true });
      return;
    }

    const cart = JSON.parse(localStorage.getItem("cart"));

    const newCartItem = {
      PhProductId: info.productId,
      PharmacyId: pharmacyId,
      Quantity: counter,
    };

    let IsPharmacyChanged = false;

    if (cart) {
      if (cart.pharmacyId != undefined) {
        IsPharmacyChanged = cart.pharmacyId != pharmacyId;

        if (IsPharmacyChanged) {
          const result = await confirm(
            "هذا الدواء من صيدلية اخرى غير التي  في سلة مشترياتك , هل تريد تغيير الصيدلية ؟"
          );

          if (!result) return;
        }
      }
    }

    const numberOfItems = await HandleAddItemToCart(
      newCartItem,
      authInfo.customerId,
      cart,
      IsPharmacyChanged
    );

    setItemsCount(numberOfItems, !cart);
  }

  return (
    <>
      <Grid
        size={{ md: 6, xs: 12 }}
        sx={{ display: "flex", justifyContent: "center" }}
      >
        <img id="product-img" alt="productImage" src={info.ImageURL} />
      </Grid>

      <Grid
        size={{ md: 6, sm: 12 }}
        sx={{
          display: "flex",
          flexDirection: "column",
          justifyContent: "end",
          columnGap: "3",
          marginTop: { md: 4 },
        }}
      >
        <Typography level="h1">{info.Name} </Typography>

        {itemsCount == 0 ? (
          <Box
            id="check-ph-alarm"
            sx={{
              display: "flex",
              justifyContent: "start",
              alignItems: "center",
            }}
          >
            <ProductionQuantityLimitsIcon
              fontSize="medium"
              sx={{ color: "red", display: "inline" }}
            />
            <Typography
              level="body-sm"
              sx={{ display: "inline", color: "red" }}
            >
              اسحب للاسفل للتحقق من رسوم التوصيل أو توفر الخدمة
            </Typography>
          </Box>
        ) : null}

        <h2>
          السعر:
          <span style={{ backgroundColor: "rgb(255, 86, 114)" }}>
            {info.Price}jd
          </span>
        </h2>

        <div id="btns-container">
          <div>
            <button
              className="add-tocart-btn"
              onClick={() => handelAddToCartClicked()}
            >
              <LocalMallIcon />
              اضف للسلة
            </button>
          </div>
          <div id="add-sub-btn" className="card__counter">
            <button
              className="card__btn"
              onClick={() => {
                setCounter((prev) => {
                  if (prev == 0) return 0;

                  return prev - 1;
                });
              }}
            >
              -
            </button>

            <div className="card__counter-score">{counter}</div>

            <button
              className="card__btn card__btn-plus"
              onClick={() => {
                setCounter((prev) => {
                  if (prev == info.Stoke) return prev;

                  return prev + 1;
                });
              }}
            >
              +
            </button>
          </div>
        </div>

        <Box display="flex" justifyContent="start" gap={2}>
          <h3>
            شكل المنتج: <span className="secondry-bg">{info.TypeName}</span>
          </h3>

          <h3>
            سعة الدواء: <span className="secondry-bg"> {info.Qauntity}</span>
          </h3>
        </Box>

        <Box display="flex" justifyContent="start" gap={1}>
          <h4>
            تاريخ الأنتاج:
            <span className="secondry-bg">
              {info.ProducedDate.split("T")[0]}
            </span>
          </h4>

          <h4>
            تاريخ الأنتهاء:
            <span className="secondry-bg">{info.EndDate.split("T")[0]}</span>
          </h4>
        </Box>

        <h3>
          الكمية المتوفرة:
          <span className="secondry-bg">{info.Stoke} علبة</span>
        </h3>
      </Grid>
    </>
  );
}

export default ProductInfo;

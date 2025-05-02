import "./ProductPage.css";
import Grid from "@mui/material/Grid2";
import ProductInfo from "./ProductInfo";
import ProductSubInfo from "./ProductSubInfo";
import { GetProductFullInfo } from "../../../services/phProductsServices";
import { useState, useEffect } from "react";
import { useLocation, useParams } from "react-router-dom";
import DeliverFeesDialog from "../../../components/Dialogs/DeliveryFeesDialog/DeliveryFeesDialog";
import { CircularProgress } from "@mui/material";

let pharmacyId = null;

export function ProductPage() {
  const { productId } = useParams();
  const location = useLocation();

  const [result, setResult] = useState(true);
  const [loading, setLoading] = useState(true);
  const [info, setInfo] = useState({
    productId: null,
    Name: null,
    ImageURL: null,
    ProducedDate: "",
    EndDate: "",
    Price: null,
    Qauntity: null,
    Stoke: null,
    TypeName: null,
  });

  const [pharmacyInfo, setPharmacyInfo] = useState({
    pharmacyId: null,
    Name: null,
    Image: null,
    IsHasDelivery: false,
  });

  async function fetchData(pharmacyId) {
    setLoading(true);
    const response = await GetProductFullInfo(productId, pharmacyId);

    setLoading(false);

    if (!response.IsSuccess) {
      setResult(false);
      return;
    }

    const result = response.result;

    setInfo({
      ...info,
      productId: result.phProductId,
      Name: result.name,
      ImageURL: result.imageURL,
      ProducedDate: result.producedDate,
      EndDate: result.endDate,
      Price: result.price,
      Qauntity: result.medicalQuantity,
      Stoke: result.stoke,
      TypeName: result.medicalType.medicalTypeNameArabic,
    });

    setPharmacyInfo({
      ...pharmacyInfo,
      pharmacyId: result.pharmacy.pharmacyId,
      Name: result.pharmacy.name,
      Image: result.pharmacy.imageURL,
      IsHasDelivery: result.pharmacy.isHasDelivery,
    });
  }

  useEffect(() => {
    const params = new URLSearchParams(location.search);

    pharmacyId = Number(params.get("pharmacy"));

    if (!pharmacyId || !productId) {
      setResult(false);
      return;
    }

    fetchData(pharmacyId);
  }, [location.search, productId]);

  return (
    <Grid
      container
      spacing={{ md: 5, xs: 0 }}
      sx={{
        display: "flex",
        justifyContent: "center",
        alignItems: "center",
        direction: "rtl",
        marginTop: 1,
      }}
    >
      {loading ? (
        <CircularProgress size={40} />
      ) : !result ? (
        <p>فشل تحميل معلومات المنتج</p>
      ) : (
        <>
          <ProductInfo info={info} pharmacyId={pharmacyInfo.pharmacyId} />

          <ProductSubInfo info={pharmacyInfo} />

          <DeliverFeesDialog />
        </>
      )}
    </Grid>
  );
}

export default ProductPage;

import Box from "@mui/material/Box";
import "./PharmacyProfile.css";
import { useNavigate, useParams } from "react-router-dom";
import Grid from "@mui/material/Grid2";
import { useState, useEffect } from "react";
import { GetPharmacyProfileInfo } from "../../../services/pharmacyServices";
import { useAuth } from "../../../contexts/AuthContext";
import LoginAlarm from "../../../components/LoginAlarm/LoginAlarm";
import DeliveryCard from "./Componants/DeliveryCard";
import MeetingRequestCard from "./Componants/MeetingRequestCard";
import MessagesCard from "./Componants/MessagesCard";
import PharmacyProducts from "./Componants/PharmacyProducts";
import PharmacyProfileHeader from "./Componants/PharmacyProfileHeader";

export function PharmacyProfile() {
  const { pharmacyId } = useParams();

  const { authInfo } = useAuth();

  const [result, setResult] = useState(true);
  const [loading, setLoading] = useState(true);
  const [info, setInfo] = useState({
    pharmacyId: -1,
    pharmacyName: "",
    about: "",
    callPrice: 0,
    isHasDelivery: false,
    governorate: "",
    imageURL: "",
  });

  async function fetchPharmacyData() {
    const response = await GetPharmacyProfileInfo(pharmacyId);

    setLoading(false);
    if (!response.IsSuccess) {
      setResult(false);
      return;
    }

    setInfo({
      ...info,
      pharmacyId: response.result.pharmacyId,
      pharmacyName: response.result.arabicName,
      about: response.result.about,
      callPrice: response.result.vedioCallPrice,
      governorate: response.result.governateName,
      imageURL: response.result.imageURL,
      isHasDelivery: response.result.isHasDelivery,
    });
  }

  useEffect(() => {
    fetchPharmacyData();
    window.scrollTo(0, 0);
  }, [pharmacyId, authInfo]);

  return (
    <div style={{ direction: "rtl" }}>
      <PharmacyProfileHeader info={info} loading={loading} result={result} />
      <Grid
        container
        spacing={2}
        sx={{ display: "flex", justifyContent: "center", marginBottom: "10px" }}
      >
        {!authInfo.loggedIn ? (
          <Box
            sx={{
              display: "flex",
              flexDirection: "column",
              alignItems: "center",
            }}
          >
            <p>لكي تستخدم الخدمات</p>
            <LoginAlarm />
          </Box>
        ) : (
          <>
            {/* Options */}

            <MeetingRequestCard info={info} />
            <MessagesCard />
            <DeliveryCard info={info} />
          </>
        )}
      </Grid>

      <div style={{ textAlign: "center" }}>
        <h2>منتجات صيديلة {info.pharmacyName || ""}</h2>
      </div>

      <PharmacyProducts />
    </div>
  );
}

export default PharmacyProfile;

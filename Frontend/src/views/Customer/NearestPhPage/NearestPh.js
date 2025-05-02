import { GetNearerPharmaciesFromCustomer } from "../../../services/pharmacyServices";
import { useState, useEffect } from "react";
import { CircularProgress } from "@mui/material";
import Grid from "@mui/material/Grid2";
import PharmacyCard from "../../../components/Card/PharmacyCard/PharmacyCard";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../../../contexts/AuthContext";
import LoginAlarm from "../../../components/LoginAlarm/LoginAlarm";

export function NearestPh() {
  const [loading, setLoading] = useState(true);
  const [result, setResult] = useState(true);
  const [pharmacies, setPharmacies] = useState([]);

  const { authInfo } = useAuth();

  const navigate = useNavigate();

  useEffect(() => {
    if (authInfo.loggedIn) {
      fetchData(authInfo.customerId);
    }
  }, [authInfo]);

  async function fetchData(CustomerId) {
    const response = await GetNearerPharmaciesFromCustomer(CustomerId);

    setLoading(false);

    if (!response.IsSuccess) {
      setResult(false);
      return;
    }

    setResult(true);
    setPharmacies(response.result);
  }

  const pharmaciesCards = pharmacies.map((Pharmacy) => {
    return (
      <Grid
        size={{ xs: 6, md: 2.4, sm: 4 }}
        sx={{ display: "flex", justifyContent: "center" }}
      >
        <PharmacyCard
          key={Pharmacy.pharmacyId}
          Id={Pharmacy.pharmacyId}
          Name={Pharmacy.arabicName}
          GovernateName={Pharmacy.governateName}
          ImageURL={Pharmacy.imageURL}
          RegionName={Pharmacy.regionName}
        />
      </Grid>
    );
  });

  return (
    <>
      {!authInfo.loggedIn ? (
        <div
          style={{
            display: "flex",
            flexDirection: "column",
            alignItems: "center",
            justifyContent: "center",
          }}
        >
          <LoginAlarm />
        </div>
      ) : (
        <Grid
          container
          spacing={1}
          sx={{
            my: 4,
            direction: "rtl",
            display: "flex",
            justifyContent: "center",
            alignItems: "center",
          }}
        >
          {loading ? (
            <CircularProgress size={40} />
          ) : !result ? (
            <p>لم يتم العثور على اي صيدليات قريبة منك</p>
          ) : (
            <>
              <Grid
                size={12}
                sx={{ display: "flex", justifyContent: "center" }}
              >
                <h2>صيدليات تبعد عنك أقل من 3 كم</h2>
              </Grid>
              {pharmaciesCards}
            </>
          )}
        </Grid>
      )}
    </>
  );
}

export default NearestPh;

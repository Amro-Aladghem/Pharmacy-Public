import "./Pharmacies.css";
import { useLocation } from "react-router-dom";
import Grid from "@mui/material/Grid2";
import CircularProgress from "@mui/material/CircularProgress";
import InfiniteScroll from "react-infinite-scroll-component";
import { useState, useEffect } from "react";
import { GetPharmaciesList } from "../../../services/pharmacyServices";
import PharmacyCard from "../../../components/Card/PharmacyCard/PharmacyCard";
import RegionsMenue from "../../../components/RegionsMenu/RegionsMenu";

export function Pharmacies() {
  const location = useLocation();

  const [paginated, setPaginated] = useState({
    LastPharmacyId: 0,
    Limit: 10,
    IsRowsCountCalculated: false,
    GovernorateId: null,
    RegionId: null,
  });
  const [pharmacies, setPharmacies] = useState([]);
  const [loading, setLoading] = useState(true);
  const [result, setResult] = useState(true);

  useEffect(() => {
    const params = new URLSearchParams(location.search);
    let GovernorateId = Number(params.get("Governorate"));
    let Regionid = Number(params.get("Region"));

    fetchData(true, GovernorateId, Regionid);
  }, [location.search]);

  const fetchData = async (
    isFirstTime = false,
    GovernorateId = 0,
    RegionId
  ) => {
    const Paginated = {
      LastPharmacyId: isFirstTime ? 0 : paginated.LastPharmacyId,
      Limit: paginated.Limit,
      IsRowsCountCalculated: paginated.IsRowsCountCalculated,
      GovernorateId: isFirstTime ? GovernorateId : paginated.GovernorateId,
      RegionId: isFirstTime ? RegionId : paginated.RegionId,
    };

    if (isFirstTime) {
      setPharmacies([]);
    }

    const responseResult = await GetPharmaciesList(Paginated);

    setLoading(false);

    if (!responseResult.IsSuccess) {
      setResult(false);
      return;
    }

    setPharmacies((prev) => [...prev, ...responseResult.result.pharmacies]);

    setResult(true);

    setPaginated((prev) => {
      return {
        ...prev,
        LastPharmacyId: responseResult.result.lastPharmacyId,
        ...(isFirstTime && {
          GovernorateId: GovernorateId,
          RegionId: RegionId,
          IsRowsCountCalculated: true,
        }),
      };
    });
  };

  const PharmaciesCards = pharmacies.map((Pharmacy) => {
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
      <InfiniteScroll
        dataLength={pharmacies.length}
        next={fetchData}
        hasMore={result}
      >
        <Grid
          container
          spacing={1}
          sx={{
            my: 4,
            direction: "rtl",
            display: "flex",
            justifyContent: "center",
          }}
        >
          <RegionsMenue />
          {PharmaciesCards}
          {!result ? <p>لا يوجد المزيد من الصيدليات في هذه المحافظة</p> : null}

          {loading ? (
            <CircularProgress size={40} color="rgb(255, 86, 114)" />
          ) : null}
        </Grid>
      </InfiniteScroll>
    </>
  );
}

export default Pharmacies;

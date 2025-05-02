import ArrowBackIcon from "@mui/icons-material/ArrowBack";
import { IconButton } from "@mui/material";
import { useNavigate } from "react-router-dom";
import MainLoader from "../../../../components/MainLoader";
import { useState, useEffect } from "react";
import PharmacyCard from "../../../../components/Card/PharmacyCard/PharmacyCard";
import { GetPharmaciesList } from "../../../../services/pharmacyServices";

export function RandomPharmacies() {
  const [Loading, setLoading] = useState(true);
  const [cards, setCards] = useState([]);
  const [result, setResult] = useState(true);
  const navigate = useNavigate();

  useEffect(() => {
    fetchData();
  }, []);

  const fetchData = async () => {
    const Paginated = {
      Limit: 5,
      LastPharmacyId: 0,
      IsRowsCountCalculated: false,
      GovernorateId: 1,
    };

    const responseResult = await GetPharmaciesList(Paginated);

    if (!responseResult.IsSuccess) {
      setLoading(false);
      setResult(false);
      return;
    }

    setCards(responseResult.result.pharmacies);

    setLoading(false);
  };

  const PharmaciesCards = cards.map((Pharmacy) => {
    return (
      <PharmacyCard
        key={Pharmacy.pharmacyId}
        Id={Pharmacy.pharmacyId}
        ImageURL={Pharmacy.imageURL}
        Name={"صيدلية  " + Pharmacy.arabicName}
        GovernateName={Pharmacy.governateName}
        RegionName={Pharmacy.regionName}
      />
    );
  });

  return (
    <div id="main-popular-container" sx={{ gap: { xs: 4, md: 16 } }}>
      {Loading ? (
        <MainLoader />
      ) : result ? (
        <>
          {PharmaciesCards}
          <IconButton
            sx={{
              alignSelf: "center",
              flexShrink: "0",
            }}
          >
            <ArrowBackIcon />
          </IconButton>
        </>
      ) : (
        <p>فشل التحميل</p>
      )}
    </div>
  );
}

export default RandomPharmacies;

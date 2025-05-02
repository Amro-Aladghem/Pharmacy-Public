import { useState, useEffect } from "react";
import { Box } from "@mui/material";
import "./RegionsMenu.css";
import Typography from "@mui/joy/Typography";
import { useLocation, useNavigate, useSearchParams } from "react-router-dom";
import { GetAllRegionsForTheGov } from "../../services/requestServices";

export function RegionsMenue() {
  const location = useLocation();
  const navigate = useNavigate();

  const [searchParams, setSearchParams] = useSearchParams();
  const [region, setRegion] = useState(0);
  const [regions, setRegions] = useState([]);
  const [loading, setLoading] = useState(false);
  const [currentGovId, setCurrentGovId] = useState(1);

  async function GetRegions() {
    if (regions.length != 0) return;

    const params = new URLSearchParams(location.search);
    const GovId = Number(params.get("Governorate"));

    if (!GovId) return;

    setLoading(true);

    const response = await GetAllRegionsForTheGov(GovId);

    setLoading(false);

    if (!response.IsSuccess) {
      setRegions([]);
      return;
    }

    setRegions(response.result);
  }

  function AddRegionQueryParam(id) {
    if (id == 0) return;

    const newSearchParams = new URLSearchParams(searchParams);
    newSearchParams.set("Region", id);
    setSearchParams(newSearchParams);
  }

  const regionsOptions = regions.map((region) => {
    return <option value={region.regionID}>{region.regionName}</option>;
  });

  useEffect(() => {
    const param = new URLSearchParams(location.search);
    const GoveId = param.get("Governorate");

    if (GoveId != currentGovId) setRegions([]);

    setCurrentGovId(GoveId);
  }, [location.search]);

  return (
    <Box
      sx={{
        width: "100%",
        display: "flex",
        justifyContent: "center",
        alignItems: "center",
      }}
    >
      <Typography
        sx={{ display: { xs: "none", md: "block" } }}
        level="title-sm"
      >
        اسم المنطقة:
      </Typography>
      <select
        value={region}
        id="regions-menu"
        onChange={(event) => {
          setRegion(event.target.value);
          if (regions.length !== 0) AddRegionQueryParam(event.target.value);
        }}
        onClick={() => {
          GetRegions();
        }}
      >
        <option value={0}>اختر المنطقة</option>
        {loading ? <option> جاري التحميل ...</option> : null}
        {regionsOptions}
      </select>
    </Box>
  );
}

export default RegionsMenue;

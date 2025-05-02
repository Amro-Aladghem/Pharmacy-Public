import "./GovernoratesMenu.css";
import { Box } from "@mui/material";
import Typography from "@mui/joy/Typography";
import { useSearchParams } from "react-router-dom";

const Governorates = [
  { id: 1, name: "عمان" },
  { id: 3, name: "اربد" },
  { id: 2, name: "الزرقاء" },
  { id: 8, name: "مأدبا" },
  { id: 7, name: "البلقاء" },
  { id: 5, name: "جرش" },
  { id: 6, name: "المفرق" },
  { id: 11, name: "معان" },
  { id: 10, name: "الطفيلة" },
  { id: 12, name: "العقبة" },
  { id: 9, name: "الكرك" },
  { id: 4, name: "عجلون" },
];

const GovernoratesList = Governorates.map((Governorate, index) => {
  return (
    <option key={index} value={Governorate.id}>
      {Governorate.name}
    </option>
  );
});

export function GovernoratesMenu() {
  const [searchParams, setSearchParams] = useSearchParams();

  function AddRegionQueryParam(id) {
    if (id == 0) return;

    const newSearchParams = new URLSearchParams(searchParams);
    newSearchParams.set("Governorate", id);
    setSearchParams(newSearchParams);
  }

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
        اسم المحافظة:
      </Typography>
      <select
        id="Governorates-menu"
        onChange={(event) => {
          AddRegionQueryParam(event.target.value);
        }}
      >
        <option value={0}>اختر العاصمة</option>
        {GovernoratesList}
      </select>
    </Box>
  );
}

export default GovernoratesMenu;

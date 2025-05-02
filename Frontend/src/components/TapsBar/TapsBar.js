import Tabs from "@mui/material/Tabs";
import Tab, { tabsClasses } from "@mui/material/Tab";
import Box from "@mui/material/Box";
import { useState, useEffect } from "react";
import { Outlet, useLocation, useSearchParams } from "react-router-dom";
import { useNavigate } from "react-router-dom";

const tabsData = [
  { id: 1, name: "مسكنات الألم" },
  { id: 2, name: "مضادات حيوية" },
  { id: 3, name: "مضادات الفيروسات" },
  { id: 4, name: "مضادات الفطريات" },
  { id: 5, name: "مضادات الحساسية" },
  { id: 6, name: "مضادات الالتهابات" },
  { id: 7, name: "الفيتامينات والمكملات الغذائية" },
  { id: 8, name: "أدوية العيون" },
  { id: 9, name: "أدوية الجهاز الهضمي" },
  { id: 10, name: "أدوية الجهاز التنفسي" },
  { id: 11, name: "أدوية القلب والأوعية الدموية" },
  { id: 12, name: "أدوية السكري" },
  { id: 13, name: "أدوية الأعصاب" },
  { id: 14, name: "أدوية الأمراض الجلدية" },
  { id: 15, name: "مستحضرات التجميل والعناية بالبشرة" },
  { id: 16, name: "أدوية المسالك البولية" },
  { id: 17, name: "أدوية الهرمونات والغدد الصماء" },
  { id: 18, name: "أدوية الصحة الجنسية" },
];

export function TapsBar() {
  const location = useLocation();
  const navigate = useNavigate();
  const [searchParams, setSearchParams] = useSearchParams();

  const [value, setValue] = useState(1);
  const [isFirstTime, setIsFirstTime] = useState(true);

  const handleChange = (event, newValue) => {
    const newSearchParams = new URLSearchParams(searchParams);
    newSearchParams.set("category", newValue);
    setSearchParams(newSearchParams);
  };

  useEffect(() => {
    const params = new URLSearchParams(location.search);

    let categoryId = Number(params.get("category"));

    categoryId = categoryId || 1;

    setValue(categoryId);
    setIsFirstTime(false);
  }, [location.search]);

  return (
    <>
      <Box sx={{ bgcolor: "background.paper", direction: "rtl" }}>
        <Tabs
          value={value}
          onChange={handleChange}
          variant="scrollable"
          scrollButtons="on"
          allowScrollButtonsMobile
          aria-label=""
          sx={{
            "& .MuiTab-root": { color: "black" },
            "& .MuiTab-root.Mui-selected": { color: "rgb(255, 86, 114)" },
          }}
          TabIndicatorProps={{
            style: { backgroundColor: "rgb(255, 86, 114)", height: "2px" },
          }}
        >
          {tabsData.map((tap, index) => {
            return <Tab key={index} value={tap.id} label={tap.name} />;
          })}
        </Tabs>
      </Box>

      <Outlet />
    </>
  );
}

export default TapsBar;

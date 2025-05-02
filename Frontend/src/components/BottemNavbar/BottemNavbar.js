import Box from "@mui/material/Box";
import BottomNavigation from "@mui/material/BottomNavigation";
import BottomNavigationAction from "@mui/material/BottomNavigationAction";
import { useState } from "react";
import LocalMallIcon from "@mui/icons-material/LocalMall";
import VideoCallIcon from "@mui/icons-material/VideoCall";
import Person2Icon from "@mui/icons-material/Person2";
import HomeIcon from "@mui/icons-material/Home";
import VaccinesIcon from "@mui/icons-material/Vaccines";
import { useNavigate, useLocation } from "react-router-dom";
import Badge from "@mui/material/Badge";
import { useContext, useEffect } from "react";
import { ItemsCountContext } from "../../contexts/ItemsCountContext";

const pages = [
  {
    id: 1,
    label: "استشارة",
    icon: <VideoCallIcon />,
    path: "/pharmacies/have-meeting",
  },
  { id: 2, label: "حسابي", icon: <Person2Icon />, path: "/customer/profile" },
  { id: 3, label: "الرئيسية", icon: <HomeIcon />, path: "/" },
  {
    id: 4,
    label: "الصيدليات",
    icon: <VaccinesIcon />,
    path: "/pharmacies",
  },
];

export function BottemNavbar() {
  const [value, setValue] = useState(3);
  const location = useLocation();

  const navigate = useNavigate();
  const { itemsCount } = useContext(ItemsCountContext);

  function handleLocationChange() {
    const currentPath = location.pathname;

    const currentPage = pages.find((page) => page.path === currentPath);

    if (!currentPage) {
      setValue(pages.length + 2);
      return;
    }

    setValue(currentPage.id);
  }

  useEffect(() => {
    handleLocationChange();
  }, [location]);

  return (
    <Box
      sx={{
        width: "100%",
        position: "fixed",
        bottom: 0,
        left: 0,
        zIndex: 1000,
        display: { xs: "block", md: "none", sm: "block" },
      }}
    >
      <BottomNavigation
        showLabels
        value={value}
        onChange={(event, newValue) => {
          setValue(newValue);
        }}
      >
        <BottomNavigationAction
          label="الحقيبة"
          icon={
            <Badge badgeContent={itemsCount} color="secondary">
              <LocalMallIcon />
            </Badge>
          }
          onClick={() => navigate("/cart")}
          sx={{
            color: "gray",
            "&.Mui-selected": {
              color: "rgb(255, 86, 114)",
            },
          }}
        />

        {pages.map((page, index) => (
          <BottomNavigationAction
            key={index}
            label={page.label}
            icon={page.icon}
            onClick={() => navigate(page.path)}
            sx={{
              color: "gray",
              "&.Mui-selected": {
                color: "rgb(255, 86, 114)",
              },
            }}
          />
        ))}
      </BottomNavigation>
    </Box>
  );
}

export default BottemNavbar;

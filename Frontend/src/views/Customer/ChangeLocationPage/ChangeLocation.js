import MapComponent from "../../../components/MapComponent";
import Typography from "@mui/joy/Typography";
import { useState, useEffect } from "react";
import { EditLocation } from "../../../services/customerServices";
import { useAuth } from "../../../contexts/AuthContext";
import LoginAlarm from "../../../components/LoginAlarm/LoginAlarm";
import { useNavigate } from "react-router-dom";
import { Box } from "@mui/material";
import { useAlert } from "../../../contexts/AlertContext";

export function ChangeLocation() {
  const [Info, setInfo] = useState({
    latitude: 0,
    longitude: 0,
    isChanged: false,
  });

  const { authInfo } = useAuth();
  const { setAlert } = useAlert();
  const navigate = useNavigate();

  async function hanldeChangeLocation() {
    const response = await EditLocation(authInfo.customerId, {
      Latitude: Info.latitude,
      Longitude: Info.longitude,
    });

    if (!response.IsSuccess) {
      setAlert({
        type: "error",
        open: true,
        message: "فشل تغيير الموقع!",
      });
      return;
    }

    setAlert({
      type: "success",
      open: true,
      message: "تم تغيير الموقع!",
    });
    navigate("/");
  }

  useEffect(() => {
    if (Info.isChanged) {
      hanldeChangeLocation();
    }
  }, [Info.isChanged, authInfo]);

  return (
    <>
      <Box
        sx={{
          display: "flex",
          justifyContent: "center",
          alignItems: "center",
          width: "100%",
          flexDirection: "column",
        }}
      >
        {!authInfo.loggedIn ? (
          <LoginAlarm />
        ) : (
          <>
            <Typography level="h1">تحديد موقعك الحالي</Typography>
            <MapComponent Info={Info} handler={{ setInfo }} />
          </>
        )}
      </Box>
    </>
  );
}

export default ChangeLocation;

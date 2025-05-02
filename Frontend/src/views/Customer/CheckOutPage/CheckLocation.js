import { Box, responsiveFontSizes } from "@mui/material";
import FmdGoodIcon from "@mui/icons-material/FmdGood";
import { CircularProgress } from "@mui/material";
import Typography from "@mui/joy/Typography";
import { VerifyCustomerLocationAtCheckOut } from "../../../services/orderServices";
import { useEffect } from "react";
import { Link } from "react-router-dom";
import { useAuth } from "../../../contexts/AuthContext";

async function GetCoordinate() {
  const coordinate = {
    latitude: 0,
    longitude: 0,
  };

  if (navigator.geolocation) {
    return new Promise((resolve, reject) => {
      navigator.geolocation.getCurrentPosition(
        function (position) {
          coordinate.latitude = position.coords.latitude;
          coordinate.longitude = position.coords.longitude;

          resolve(coordinate);
        },
        function (error) {
          reject(null);
        }
      );
    });
  } else {
    return null;
  }
}

export function CheckLocation({ setVerfiyLocation, verfiyLocation }) {
  const { authInfo } = useAuth();

  async function Verfiy(CustomerId, Longitude, Latitude) {
    const resposne = await VerifyCustomerLocationAtCheckOut(
      CustomerId,
      Longitude,
      Latitude
    );

    if (!resposne.IsSuccess) {
      setVerfiyLocation({
        ...verfiyLocation,
        loading: false,
        result: true,
        isChanged: false,
      });
      return;
    }

    setVerfiyLocation({
      ...verfiyLocation,
      loading: false,
      result: true,
      isChanged: resposne.isChanged,
    });
  }

  async function handelVerfiyProcess(CustomerId) {
    const coordinate = await GetCoordinate();

    if (!coordinate) {
      setVerfiyLocation({
        ...verfiyLocation,
        loading: false,
        result: false,
        isChanged: false,
      });
    }

    Verfiy(CustomerId, coordinate.longitude, coordinate.latitude);
  }

  useEffect(() => {
    handelVerfiyProcess(authInfo.customerId);
  }, []);

  return (
    <Box
      sx={{
        width: "100%",
        display: "flex",
        flexDirection: "column",
      }}
    >
      <FmdGoodIcon />
      <Typography
        level="body-md"
        sx={{
          display: "inline",
        }}
      >
        يتم مقارنة موقعك الحالي بالموقع المحفوظ:
      </Typography>

      {verfiyLocation.loading ? (
        <CircularProgress size={25} sx={{ color: "rgb(255, 86, 114)" }} />
      ) : verfiyLocation.isChanged ? (
        <>
          <Typography
            level="title-lg"
            sx={{
              backgroundColor: "rgb(255, 86, 114)",
              color: "black",
              width: "fit-content",
            }}
          >
            لقد تغير موقعك عن المحفوظ
          </Typography>
          <Link to={"/customer/location/edit"} underline="hover">
            اضغط هنا لتغيير الموقع المحفوظ
          </Link>
        </>
      ) : (
        <>
          <Typography
            level="title-md"
            sx={{
              backgroundColor: "rgb(255, 86, 114)",
              color: "black",
              width: "fit-content",
            }}
          >
            تم التحقق نفس الموقع✅
          </Typography>
          {!verfiyLocation.result ? (
            <Link underline="hover">اضغط هنا لتغيير الموقع المحفوظ</Link>
          ) : null}
        </>
      )}
    </Box>
  );
}

export default CheckLocation;

import Card from "@mui/joy/Card";
import CardContent from "@mui/joy/CardContent";
import AspectRatio from "@mui/joy/AspectRatio";
import Typography from "@mui/joy/Typography";
import { useNavigate } from "react-router-dom";
import PlaceIcon from "@mui/icons-material/Place";
import { Link } from "react-router-dom";
import StoreIcon from "@mui/icons-material/Store";
import { Box } from "@mui/material";

export function PharmacyCard({
  Id,
  Name,
  GovernateName,
  ImageURL,
  RegionName,
}) {
  const navigate = useNavigate();

  return (
    <Link to={`/pharmacies/${Id}`}>
      <Card
        size="sm"
        onClick={() => navigate(`/pharmacy/${Id}`)}
        sx={{
          width: { xs: "160px", md: "170px" },
          direction: "rtl",
          alignSelf: "stretch",
          height: "90%",
        }}
      >
        <AspectRatio minHeight="160px" maxHeight="260px">
          <img src={ImageURL} alt="" />
        </AspectRatio>
        <CardContent orientation="vertical">
          <Box
            id="card-Ph-name-container"
            sx={{
              display: "flex",
              justifyContent: "start",
            }}
          >
            <StoreIcon sx={{ color: "rgb(255, 86, 114)", display: "inline" }} />
            <Typography
              sx={{
                fontSize: "sm",
                fontWeight: "500",
                display: "inline",
                color: "black",
              }}
              level="body-sm"
            >
              {Name}
            </Typography>
          </Box>

          <Box sx={{ display: "flex", alignItems: "center" }}>
            <PlaceIcon
              fontSize="sm"
              sx={{ color: "rgb(255, 86, 114)", display: "inline" }}
            />
            <Typography sx={{ display: "inline" }} level="body-sm">
              {`${GovernateName},${RegionName}`}
            </Typography>
          </Box>
        </CardContent>
      </Card>
    </Link>
  );
}

export default PharmacyCard;

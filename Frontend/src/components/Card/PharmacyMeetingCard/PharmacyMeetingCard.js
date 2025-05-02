import AspectRatio from "@mui/joy/AspectRatio";
import Link from "@mui/joy/Link";
import Card from "@mui/joy/Card";
import CardContent from "@mui/joy/CardContent";
import Typography from "@mui/joy/Typography";
import { Box } from "@mui/material";
import { useNavigate } from "react-router-dom";

export function PharmacyMeetingCard({
  pharmacyId,
  pharmacyName,
  image,
  price,
  governateName,
}) {
  const navigate = useNavigate();

  return (
    <Card
      id="pharmacy-card"
      variant="outlined"
      orientation="horizontal"
      sx={{
        width: { md: "85%", xs: "90%" },
        margin: "14px 0px",
        "&:hover": {
          boxShadow: "md",
          borderColor: "neutral.outlinedHoverBorder",
        },
        direction: "rtl",
      }}
    >
      <AspectRatio ratio="1" sx={{ width: 90 }}>
        <img src={image} alt="product-Image" />
      </AspectRatio>

      <CardContent>
        <Box style={{ display: "flex", alignItems: "center" }}>
          <Box>
            <Typography level="title-lg" id="card-description">
              {pharmacyName}
            </Typography>
            <Typography
              level="body-sm"
              aria-describedby="card-description"
              sx={{ mb: 0.1 }}
            >
              {governateName}
            </Typography>

            <Typography
              level="title-lg"
              sx={{
                backgroundColor: "aqua",
                borderRadius: "8px",
                width: "fit-content",
                padding: "2px 2px",
              }}
            >
              سعر الأستشارة : {price}jd
            </Typography>
          </Box>

          <button
            className="main-btn"
            style={{ width: "fit-content", marginRight: "auto" }}
            onClick={() => {
              navigate(`/pharmacies/${pharmacyId}/request/meeting/info`);
            }}
          >
            حجز
            <div className="arrow-wrapper">
              <div className="arrow"></div>
            </div>
          </button>
        </Box>
      </CardContent>
    </Card>
  );
}

export default PharmacyMeetingCard;

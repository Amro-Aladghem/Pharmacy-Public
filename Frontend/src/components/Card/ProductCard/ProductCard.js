import Card from "@mui/joy/Card";
import CardContent from "@mui/joy/CardContent";
import AspectRatio from "@mui/joy/AspectRatio";
import Typography from "@mui/joy/Typography";
import { useNavigate } from "react-router-dom";
import PlaceIcon from "@mui/icons-material/Place";
import "./ProductCard.css";
import { Link } from "react-router-dom";
import StoreIcon from "@mui/icons-material/Store";
import { Box } from "@mui/material";

export function ProductCard({
  Id,
  Image,
  PharmacyName,
  Name,
  Price,
  PharmacyId,
}) {
  const navigate = useNavigate();

  return (
    <Link to={`/products/${Id}?pharmacy=${PharmacyId}`}>
      <Card
        size="sm"
        sx={{ width: { xs: "140px", md: "170px" }, height: "90%" }}
      >
        <AspectRatio minHeight="160px" maxHeight="260px">
          <img src={Image} alt="" />
        </AspectRatio>
        <CardContent orientation="vertical">
          <Typography
            sx={{
              fontSize: "sm",
              fontWeight: "md",
              color: "black",
              display: "-webkit-box",
              WebkitLineClamp: 3,
              WebkitBoxOrient: "vertical",
              overflow: "hidden",
              textOverflow: "ellipsis",
              minHeight: "3.6em",
            }}
            level="body-sm"
          >
            {Name}
          </Typography>
          <div id="card-Ph-name-container">
            <StoreIcon sx={{ color: "rgb(255, 86, 114)" }} />
            <Typography level="body-sm" sx={{ display: "inline" }}>
              {PharmacyName}
            </Typography>
          </div>
          <Typography
            sx={{
              backgroundColor: "aqua",
            }}
            level="title-sm"
          >
            {Price}jd
          </Typography>
        </CardContent>
      </Card>
    </Link>
  );
}

export default ProductCard;

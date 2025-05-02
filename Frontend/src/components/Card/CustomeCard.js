import Card from "@mui/joy/Card";
import CardContent from "@mui/joy/CardContent";
import AspectRatio from "@mui/joy/AspectRatio";
import Typography from "@mui/joy/Typography";
import { Button } from "@mui/material";
import { useNavigate } from "react-router-dom";
import Grid from "@mui/material/Grid2";
import "../../assets/Styles/Mainbtn.css";

export function CustomeCard({ image, body, page }) {
  const navigate = useNavigate();

  return (
    <Grid
      size={{ xs: 6, md: 4 }}
      sx={{ display: "flex", justifyContent: "center" }}
    >
      <Card sx={{ width: { xs: "140px", sm: "320px", md: "320px" } }}>
        <AspectRatio minHeight="120px" maxHeight="200px">
          <img src={image} alt="" />
        </AspectRatio>
        <CardContent orientation="vertical">
          <div>
            <Typography
              sx={{ fontSize: { xs: "md", md: "lg" }, fontWeight: "lg" }}
            >
              {body}
            </Typography>
          </div>
          <Button
            className="main-btn"
            size="md"
            sx={{
              ml: "auto",
              alignSelf: "center",
              fontWeight: 600,
              backgroundColor: "rgb(8, 176, 176)",
              color: "white",
            }}
            onClick={() => navigate(page)}
          >
            الذهاب
            <div className="arrow-wrapper">
              <div className="arrow"></div>
            </div>
          </Button>
        </CardContent>
      </Card>
    </Grid>
  );
}

export default CustomeCard;

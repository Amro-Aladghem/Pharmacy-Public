import Card from "@mui/joy/Card";
import Grid from "@mui/material/Grid2";
import Typography from "@mui/joy/Typography";
import AspectRatio from "@mui/joy/AspectRatio";
import CardContent from "@mui/joy/CardContent";
import { useNavigate } from "react-router-dom";

export function MessagesCard() {
  const navigate = useNavigate();
  return (
    <Grid
      size={{ xs: 12, sm: 6, md: 4 }}
      sx={{ display: "flex", justifyContent: "center" }}
    >
      <Card sx={{ width: 320 }}>
        <div>
          <Typography level="title-lg">أرسل رسالة للصيدلي</Typography>
          <Typography level="body-sm">الرسائل مجانية بالكامل</Typography>
        </div>
        <AspectRatio minHeight="120px" maxHeight="200px">
          <img
            src="/PhMessages.png"
            loading="lazy"
            alt=""
            width={"320px"}
            height={"120px"}
          />
        </AspectRatio>
        <CardContent orientation="horizontal">
          <div>
            <Typography level="body-xs">سعر الأرسال</Typography>
            <Typography
              sx={{
                fontSize: "lg",
                fontWeight: "lg",
                backgroundColor: "rgb(255, 86, 114)",
                color: "black",
              }}
            >
              مجانية
            </Typography>
          </div>
          <button
            className="main-btn"
            style={{ width: "fit-content", marginRight: "auto" }}
            onClick={() => {
              navigate(``);
            }}
          >
            التواصل
            <div className="arrow-wrapper">
              <div className="arrow"></div>
            </div>
          </button>
        </CardContent>
      </Card>
    </Grid>
  );
}

export default MessagesCard;

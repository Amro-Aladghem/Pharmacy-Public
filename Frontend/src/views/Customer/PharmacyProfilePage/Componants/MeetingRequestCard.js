import Card from "@mui/joy/Card";
import Grid from "@mui/material/Grid2";
import Typography from "@mui/joy/Typography";
import AspectRatio from "@mui/joy/AspectRatio";
import CardContent from "@mui/joy/CardContent";
import { useNavigate } from "react-router-dom";

export function MeetingRequestCard({ info }) {
  const navigate = useNavigate();

  return (
    <Grid
      size={{ xs: 12, md: 4 }}
      sx={{ display: "flex", justifyContent: "center" }}
    >
      <Card sx={{ width: 320 }}>
        <div>
          <Typography level="title-lg">
            احجز مكاملة استشارية مع الصيدلية
          </Typography>
          <Typography level="body-sm">
            {" "}
            سواء أتريدها مكالمة فيديو أو صوتية
          </Typography>
        </div>
        <AspectRatio minHeight="120px" maxHeight="200px">
          <img
            src="/PhVedioCall.png"
            loading="lazy"
            alt=""
            width={"320px"}
            height={"120px"}
          />
        </AspectRatio>
        <CardContent orientation="horizontal">
          <div>
            <Typography level="body-xs">سعر الأستشارة</Typography>
            <Typography
              sx={{
                fontSize: "lg",
                fontWeight: "lg",
                backgroundColor: "rgb(255, 86, 114)",
                color: "black",
              }}
            >
              {info.callPrice || "الخدمة غير متوفرة في هذه الصيدلية"}
            </Typography>
          </div>

          <button
            className="main-btn"
            style={{
              width: "fit-content",
              marginRight: "auto",
              flexShrink: "0",
            }}
            onClick={() => {
              navigate(`/pharmacies/${info.pharmacyId}/request/meeting/info`);
            }}
            disabled={info.isHasMeetingService}
          >
            الحجز
            <div className="arrow-wrapper">
              <div className="arrow"></div>
            </div>
          </button>
        </CardContent>
      </Card>
    </Grid>
  );
}

export default MeetingRequestCard;

import "./HistoryCard.css";
import Typography from "@mui/joy/Typography";
import { Link } from "react-router-dom";
import { useNavigate } from "react-router-dom";

export function OrderHistoryCard({
  OrderId,
  PharamcyName,
  PharmacyImage,
  PharmacyId,
  Date,
  StatusName,
  StatusId,
}) {
  const navigate = useNavigate();

  return (
    <div id="His-card-container">
      <Typography level="h2" variant="plain">
        #{OrderId}
      </Typography>
      <Typography
        level="h3"
        variant="plain"
        sx={{ backgroundColor: "rgb(255, 86, 114)", width: "fit-content" }}
      >
        صيدلية {PharamcyName}
      </Typography>
      <Typography
        level="h3"
        variant="plain"
        sx={{ backgroundColor: "rgb(2, 205, 205)", width: "fit-content" }}
      >
        حالة الأوردر : {StatusName}
      </Typography>
      <Typography level="h4" variant="plain">
        {Date}
      </Typography>

      <hr style={{ border: "2px solid black", width: "100%" }} />

      <div id="his-links-container">
        <Link to={`/pharmacies/${PharmacyId}`}>
          <img id="his-ph-img" src={PharmacyImage} alt="" />
        </Link>
        <button
          className="main-btn"
          style={{ width: "fit-content", marginRight: "auto" }}
          onClick={() => {
            navigate(`/order/${OrderId}/details`);
          }}
          disabled={false}
        >
          اظهار التفاصيل
          <div className="arrow-wrapper">
            <div className="arrow"></div>
          </div>
        </button>
      </div>

      {StatusId == 5 || StatusId == 6 ? (
        <Typography level="title-sm">
          يمكنك استرجاع النقود لهذا الطلب, اضغط الزر
        </Typography>
      ) : null}
    </div>
  );
}

export default OrderHistoryCard;

import "./HorizontalCard.css";
import "../../../assets/Styles/Mainbtn.css";
import Typography from "@mui/joy/Typography";
import { useEffect, useState } from "react";
import { GetMostActiveOrderForCustomer } from "../../../services/orderServices";
import MainLoader from "../../MainLoader";
import { GetMostActiveMeetingRequest } from "../../../services/requestServices";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../../../contexts/AuthContext";

export function HorizontalCard({ title, Type }) {
  const [info, setInfo] = useState({
    Id: null,
    PharmacyName: "",
    isNotAccepted: false,
    IsTheSameDay: false,
  });
  const [Loading, setLoading] = useState(false);
  const { authInfo } = useAuth();

  const navigate = useNavigate();

  useEffect(() => {
    if (authInfo.loggedIn) {
      fetchData(authInfo.customerId);
    }
  }, [authInfo]);

  const fetchData = async (CustomerId) => {
    setLoading(true);

    const responseResult = await (Type == "Order"
      ? GetMostActiveOrderForCustomer(CustomerId)
      : GetMostActiveMeetingRequest(CustomerId));

    setLoading(false);

    if (!responseResult.IsSuccess) {
      setInfo({ ...info, Id: null });
      return;
    }

    if (!responseResult.result) {
      setInfo({ ...info, Id: null });
      return;
    }

    setInfo({
      ...info,
      Id: responseResult.result.id,
      PharmacyName: responseResult.result.pharmacyName,
      isNotAccepted: responseResult.result.isNotAccepted,
      IsTheSameDay: responseResult.result.isTheSameDay,
    });

    setLoading(false);
  };

  function handelbtnClick() {
    if (!authInfo.loggedIn) {
      navigate("/signin");
      return;
    }

    if (info.Id == null) {
      navigate(Type == "Order" ? "/products" : "/meetingcalls");
      return;
    }

    navigate(
      Type == "Order"
        ? `/order/${info.Id}/details`
        : `/meeting-request/${info.Id}/active/request/details`
    );
    return;
  }

  return (
    <>
      <div style={{ direction: "rtl" }}>
        <h2>{title} :</h2>

        <div id="horiz-card">
          {Loading ? (
            <MainLoader />
          ) : (
            <>
              <div className="horize-card-body">
                {!authInfo.loggedIn ? (
                  <Typography flexGrow={1} display={"inline"} level="body-lg">
                    سجل دخولك{" "}
                  </Typography>
                ) : (
                  <>
                    <Typography display={"inline"} level="body-lg">
                      رقم الطلب:
                    </Typography>
                    <Typography flexGrow={1} display={"inline"} level="body-lg">
                      {info.Id == null ||
                      (!info.IsTheSameDay && info.isNotAccepted)
                        ? "لا يوجد"
                        : `${info.Id}`}
                    </Typography>
                  </>
                )}

                <button
                  className="main-btn"
                  style={{ width: "fit-content" }}
                  onClick={() => handelbtnClick()}
                >
                  {!authInfo.loggedIn
                    ? "تسجيل الدخول"
                    : !info.Id || (!info.IsTheSameDay && info.isNotAccepted)
                    ? "اطلب الأن"
                    : "عرض التفاصيل"}
                  <div className="arrow-wrapper">
                    <div className="arrow"></div>
                  </div>
                </button>
              </div>

              {info.isNotAccepted && info.IsTheSameDay ? (
                <Typography
                  level="body-lg"
                  sx={{ backgroundColor: "aqua", width: "fit-content" }}
                >
                  لديك طلب مرفوض
                </Typography>
              ) : null}

              {!authInfo.loggedIn ||
              info.Id == null ||
              info.isNotAccepted ? null : (
                <div className="horize-card-body">
                  <Typography level="body-lg">الصيدلية:</Typography>
                  <Typography level="body-lg">{info.PharmacyName} </Typography>
                </div>
              )}
            </>
          )}
        </div>
      </div>
    </>
  );
}

//

export default HorizontalCard;

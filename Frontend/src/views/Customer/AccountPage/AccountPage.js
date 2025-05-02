import OptionCard from "../../../components/Card/Horizontal/OptionCard/OptionCard";
import Grid from "@mui/material/Grid2";
import Avatar from "@mui/material/Avatar";
import "./AccountPage.css";
import { useState, useEffect } from "react";
import { CircularProgress } from "@mui/material";
import { IsCustomerLoggedIn } from "../../../services/customerServices";
import { useNavigate, Link } from "react-router-dom";
import { useAuth } from "../../../contexts/AuthContext";
import LoginAlarm from "../../../components/LoginAlarm/LoginAlarm";

const options = [
  {
    id: 1,
    optionName: "عرض الملف الشخص",
    type: "accoutnInfo",
    page: "/customer/profile/info",
  },
  { id: 7, optionName: "طلبك النشط", type: "active", page: "/" },
  { id: 8, optionName: "موعدك النشط", type: "active", page: "/" },
  {
    id: 2,
    optionName: "عرض طلباتك السابقة",
    type: "orderHistory",
    page: "/orders/history",
  },
  {
    id: 3,
    optionName: "عرض مواعيدك السابقة",
    type: "appointment",
    page: "/customer/meeting/history",
  },
  {
    id: 4,
    optionName: " تغيير تفاصيل موقعك",
    type: "loaction",
    page: "/customer/location/edit",
  },
  {
    id: 5,
    optionName: "طلبات عمليات الأسترجاع",
    type: "return",
    page: "/refund-request/history",
  },
  { id: 6, optionName: "التواصل مع الدعم الفني", type: "support", page: "/" },
];

const optionsCards = options.map((option) => {
  return (
    <Grid
      key={option.id}
      size={{ xs: 12, md: 6, sm: 6 }}
      sx={{ display: "flex", justifyContent: "center" }}
    >
      <OptionCard
        key={option.id}
        optionName={option.optionName}
        type={option.type}
        page={option.page}
      />
    </Grid>
  );
});

export function AccountPage() {
  const navigate = useNavigate();

  const [info, setInfo] = useState({ name: "", image: "" });
  const [loading, setLoading] = useState(true);
  const [result, setResult] = useState(true);

  const { authInfo } = useAuth();

  useEffect(() => {
    if (authInfo.loggedIn) {
      fetchData(
        authInfo.customerId,
        authInfo.userName,
        authInfo.profileImageLink
      );
      return;
    }

    setLoading(false);
  }, [authInfo]);

  async function fetchData(CustomerId, name, image) {
    const response = await IsCustomerLoggedIn(CustomerId);

    setLoading(false);

    if (!response.IsSuccess || !response.isLogged) {
      setResult(false);
      return;
    }

    setResult(true);
    setInfo({ ...info, name: name, image: image });
  }

  return (
    <>
      {loading ? (
        <CircularProgress size={40} />
      ) : !result || !authInfo.loggedIn ? (
        <>
          <div
            style={{
              display: "flex",
              flexDirection: "column",
              alignItems: "center",
              justifyContent: "center",
            }}
          >
            <LoginAlarm />
          </div>
        </>
      ) : (
        <>
          <div id="customer-container">
            <Avatar
              alt="Remy Sharp"
              sx={{ width: "100px", height: "100px" }}
              src={info.image}
            />
            <h2>{info.name}</h2>
          </div>

          <Grid
            container
            spacing={2}
            sx={{
              direction: "rtl",
              display: "flex",
              alignItems: "center",
              justifyContent: "center",
            }}
          >
            {optionsCards}

            <p>الرجوع للصفحة الرئيسية؟</p>
            <Link style={{ color: "black" }} to={"/"}>
              اضغط هنا
            </Link>
          </Grid>
        </>
      )}
    </>
  );
}

export default AccountPage;

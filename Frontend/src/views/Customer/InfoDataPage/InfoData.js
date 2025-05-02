import "./InfoData.css";
import InfoTextBars from "./InfoTextBars/InfoTextBars";
import "../../../assets/Styles/Mainbtn.css";
import { ShowInfo } from "../../../services/customerServices";
import { useEffect, useState } from "react";
import { UpdateInfo } from "../../../services/customerServices";
import { CircularProgress } from "@mui/material";
import OptionCard from "../../../components/Card/Horizontal/OptionCard/OptionCard";
import Grid from "@mui/material/Grid2";
import LoginAlarm from "../../../components/LoginAlarm/LoginAlarm";
import { useAuth } from "../../../contexts/AuthContext";
import { useNavigate } from "react-router-dom";
import { useAlert } from "../../../contexts/AlertContext";

// يوجد مشكلة في هذا المكون لا يعمل التحديث يعطي خطأ

const options = [
  { id: 1, optionName: "تغيير كلمة السر", type: "accoutnInfo", page: "" },
  { id: 2, optionName: "تغيير الأيميل", type: "accoutnInfo", page: "" },
  { id: 3, optionName: "تغيير الموقع", type: "loaction", page: "" },
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

export function InfoData() {
  const [loading, setLoading] = useState(true);
  const [info, setInfo] = useState({
    customerId: -1,
    firstName: "",
    lastName: "",
    phone: "",
    email: "",
  });
  const [responseResult, setResult] = useState({ result: true });

  const navigate = useNavigate();
  const { authInfo } = useAuth();
  const { setAlert } = useAlert();

  const [isEdit, setEdit] = useState(false);
  const [infoChanged, setInfoChanged] = useState(false);

  useEffect(() => {
    if (!authInfo.loggedIn) {
      setLoading(false);
      return;
    }

    fetchData(authInfo.customerId);
  }, [authInfo]);

  async function fetchData(CustomerId) {
    const response = await ShowInfo(CustomerId);

    setLoading(false);

    if (!response.IsSuccess) {
      setResult({
        ...responseResult,
        result: false,
        messsage: "فشل تحميل البيانات , الرجاء اعادة التحميل",
      });
      return;
    }

    setResult({ ...responseResult, result: true });

    setInfo({
      ...info,
      customerId: CustomerId,
      firstName: response.result.firstName,
      lastName: response.result.lastName,
      phone: response.result.phone,
      email: response.result.email,
    });
  }

  function handelbtnClicked() {
    if (!isEdit) {
      setEdit(true);
      return;
    }

    if (!infoChanged) {
      return;
    }

    if (!info.firstName || !info.lastName || !info.phone) {
      setAlert({
        type: "error",
        message: "يجب تعبئة جميع الحقول!",
        open: true,
      });
      return;
    }

    if (info.phone.startsWith("0", 0) || info.phone.length != 9) {
      setAlert({
        type: "error",
        message: "رقم الهاتف غير صالح!",
        open: true,
      });

      return;
    }

    setLoading(true);
    handelSaveClick(info);
  }

  async function handelSaveClick(updateObj) {
    const response = await UpdateInfo(updateObj);

    if (!response.Success) {
      setAlert({
        type: "error",
        message: "فشل تحديث المعلومات!",
        open: true,
      });

      return;
    }

    setAlert({
      type: "success",
      message: "تم تحديث المعلومات بنجاح !",
      open: true,
    });
    navigate("/");
  }

  return (
    <>
      {loading && !isEdit ? (
        <CircularProgress sizie={40} />
      ) : !authInfo.loggedIn ? (
        <div className="info-data-container">
          <LoginAlarm />
        </div>
      ) : !responseResult.result ? (
        <p>فشل تحمل المعلومات الرجاء اعادة تحميل الصفحة</p>
      ) : (
        <>
          <div className="info-data-container">
            <h1>معلومات حسابك</h1>
            <InfoTextBars
              isEdit={isEdit}
              info={info}
              updateInfo={setInfo}
              updateInfoChanged={setInfoChanged}
            />

            <button
              className="main-btn"
              style={{ width: "fit-content" }}
              disabled={loading}
              onClick={() => {
                handelbtnClicked();
              }}
            >
              {isEdit ? "حفظ التعديلات" : "تعديل المعلومات"}
              <div className="arrow-wrapper">
                <div className="arrow"></div>
              </div>
            </button>
          </div>

          <Grid container spacing={2} my={3} sx={{ direction: "rtl" }}>
            {optionsCards}
          </Grid>
        </>
      )}
    </>
  );
}

export default InfoData;

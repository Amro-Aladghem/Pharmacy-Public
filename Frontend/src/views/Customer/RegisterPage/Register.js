import { useState, useEffect } from "react";
import MapComponent from "../../../components/MapComponent";
import RegisterFormInfo from "./RegisterFormInfo";
import MainLoader from "../../../components/MainLoader";
import Grid from "@mui/material/Grid2";
import "./Register.css";
import { Button } from "@mui/material";
import { Registerfunc } from "../../../services/customerServices";
import { useAuth } from "../../../contexts/AuthContext";
import { useNavigate } from "react-router-dom";
import { useAlert } from "../../../contexts/AlertContext";

export function Register() {
  const [Info, setInfo] = useState({
    FirstName: "",
    LastName: "",
    Phone: "",
    latitude: "",
    longitude: "",
    Email: "",
    PersonId: -1,
  });
  const [hasAccess, setHasAccess] = useState(false);
  const [loading, setLoading] = useState(true);
  const [clicked, setClicked] = useState(false);
  const { setAlert } = useAlert();

  const { handleAuthChange } = useAuth();
  const navigate = useNavigate();

  async function handelRegisterClicked() {
    if (!Info.latitude || !Info.longitude) {
      setAlert({
        type: "error",
        message: "يجب تعبئة جميع الحقول!",
        open: true,
      });
      return;
    }

    if (!Info.FirstName || !Info.LastName || !Info.Phone) {
      setAlert({
        type: "error",
        message: "يجب تعبئة جميع الحقول!",
        open: true,
      });

      return;
    }

    setClicked(true);

    const response = await Registerfunc(Info);

    if (!response.Success) {
      alert(response.error.message);
      setClicked(false);
      return;
    }

    sessionStorage.setItem(
      "customer",
      JSON.stringify(response.data.data.customer)
    );
    sessionStorage.removeItem("Person");
    sessionStorage.setItem("expiredTokenTime", response.data.expiredTokenTime);

    handleAuthChange(
      true,
      response.data.customer.customerId,
      response.data.customer.person.userName,
      response.data.customer.person.profileImageLink,
      response.data.expiredTokenTime
    );

    navigate("/");
  }

  useEffect(() => {
    const storedUser = sessionStorage.getItem("Person");
    const parsedUser = storedUser ? JSON.parse(storedUser) : null;

    if (parsedUser) {
      setHasAccess(true);
      setInfo({
        ...Info,
        Email: parsedUser.email,
        PersonId: parsedUser.personId,
      });
    }

    setLoading(false);
  }, []);

  if (loading) {
    return (
      <Grid container alignItems={"center"}>
        <Grid>
          <MainLoader top="50%" left="50%" />
        </Grid>
      </Grid>
    );
  }

  return (
    <>
      {!hasAccess ? (
        <h1>You can't use this page!</h1>
      ) : (
        <>
          <Grid className="Register-container" container spacing={6}>
            <Grid item size={{ xs: 12, md: 6 }}>
              <MapComponent Info={Info} handler={{ setInfo }} />
            </Grid>
            <Grid id="RegisterForm-container" size={{ xs: 12, md: 6 }}>
              <RegisterFormInfo Info={Info} handlers={{ setInfo }} />
            </Grid>
          </Grid>

          <div id="btn-container">
            <Button
              id="sign-in-btn"
              type="submit"
              variant="contained"
              color="success"
              sx={{ mt: 6 }}
              disabled={clicked}
              onClick={() => {
                handelRegisterClicked();
              }}
            >
              تسجيل الدخول
              <div className="arrow-wrapper">
                <div className="arrow"></div>
              </div>
            </Button>
          </div>
        </>
      )}
    </>
  );
}

export default Register;

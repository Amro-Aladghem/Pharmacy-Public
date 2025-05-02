import Container from "@mui/material/Container";
import MeetingRoomIcon from "@mui/icons-material/MeetingRoom";
import TextField from "@mui/material/TextField";
import { Button, ListItemAvatar } from "@mui/material";
import "./SignInPage.css";
import { useState, useEffect, useContext } from "react";
import InputAdornment from "@mui/material/InputAdornment";
import Visibility from "@mui/icons-material/Visibility";
import VisibilityOff from "@mui/icons-material/VisibilityOff";
import IconButton from "@mui/material/IconButton";
import { Login } from "../../../services/customerServices";
import MainLoader from "../../../components/MainLoader";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../../../contexts/AuthContext";
import { GetCustomerCartIfExistsAtLoggedIn } from "../../../services/orderServices";
import { useAlert } from "../../../contexts/AlertContext";

export function SignInPage() {
  const [showPassword, setShowPassword] = useState(false);
  const [password, setPassword] = useState("");
  const [email, setEmail] = useState("");
  const [IsLoading, setLoadingStatus] = useState(false);

  const { authInfo, handleAuthChange } = useAuth();
  const { setAlert } = useAlert();

  const navigate = useNavigate();

  const handleTogglePassword = () => {
    setShowPassword((prev) => !prev);
  };

  function handelEmailInputChange(value) {
    setEmail(value);
  }

  function handelPasswordInputChange(value) {
    setPassword(value);
  }

  async function handleGetCustomerCartIfExists(customerId) {
    const cart = localStorage.getItem("cart");
    if (cart) return;

    const response = await GetCustomerCartIfExistsAtLoggedIn(customerId);

    if (!response.IsSuccess || !response.result) {
      localStorage.removeItem("cart");
      return;
    }

    localStorage.setItem("cart", JSON.stringify(response.result));
  }

  async function handelSignInCliked() {
    if (!email || !password) {
      setAlert({ type: "error", message: "يجب ملئ جميع الحقول", open: true });
      return;
    }

    setLoadingStatus(true);
    const response = await Login(email, password);

    setLoadingStatus(false);

    if (!response.Success) {
      setAlert({
        type: "error",
        message: `${response.error || "حدث خطأ !"}`,
        open: true,
      });
      return;
    }

    sessionStorage.setItem("customer", JSON.stringify(response.data.customer));
    sessionStorage.setItem(
      "expiredTokenTime",
      JSON.stringify(response.data.expiredTokenTime)
    );

    handleAuthChange(
      true,
      response.data.customer.customerId,
      response.data.customer.person.userName,
      response.data.customer.person.profileImageLink,
      response.data.expiredTokenTime
    );

    handleGetCustomerCartIfExists(response.data.customer.customerId);

    window.location.href = "/";
  }

  useEffect(() => {
    if (authInfo.loggedIn) {
      navigate("/");
    }
  }, []);

  return (
    <div id="sign-in-container">
      <div id="sign-in-form">
        <h1>تسجيل الدخول</h1>
        <MeetingRoomIcon sx={{ color: "rgb(5, 197, 197)" }} fontSize="large" />
        <form
          onSubmit={(event) => {
            event.preventDefault();
          }}
        >
          <TextField
            value={email}
            id="email-input"
            label="Eamil"
            type="email"
            variant="outlined"
            fullWidth
            size="small"
            margin="dense"
            onChange={(event) => {
              handelEmailInputChange(event.target.value);
            }}
          />
          <TextField
            value={password}
            label="Password"
            type={showPassword ? "text" : "password"}
            variant="outlined"
            fullWidth
            size="small"
            margin="dense"
            slotProps={{
              input: {
                endAdornment: (
                  <InputAdornment position="end">
                    <IconButton
                      sx={{ width: "fit-content" }}
                      onClick={handleTogglePassword}
                      edge="end"
                    >
                      {showPassword ? <Visibility /> : <VisibilityOff />}
                    </IconButton>
                  </InputAdornment>
                ),
              },
            }}
            onChange={(event) => {
              handelPasswordInputChange(event.target.value);
            }}
          />
          <Button
            id="sign-in-btn"
            type="submit"
            variant="contained"
            color="success"
            sx={{ mt: 2 }}
            disabled={IsLoading}
            onClick={() => {
              handelSignInCliked();
            }}
          >
            تسجيل الدخول
            <div className="arrow-wrapper">
              <div className="arrow"></div>
            </div>
          </Button>
        </form>
        <div id="more-info">
          <h6 style={{ display: "inline" }}>هل نسيت كلمة السر؟</h6>
          <a href="/">الدعم الفني</a>
          <h6 style={{ marginTop: "6px", marginBottom: "1px" }}>
            مستخدم جديد أول مرة؟
          </h6>
          <a href="/">تسجيل الدخول أول مرة</a>
        </div>

        {IsLoading ? <MainLoader top="50%" left="40%" /> : null}
      </div>
    </div>
  );
}

export default SignInPage;

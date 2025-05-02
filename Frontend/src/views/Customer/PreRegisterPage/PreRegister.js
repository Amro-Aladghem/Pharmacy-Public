import MeetingRoomIcon from "@mui/icons-material/MeetingRoom";
import TextField from "@mui/material/TextField";
import { Button, ListItemAvatar } from "@mui/material";
import "./PreRegister.css";
import { useState } from "react";
import InputAdornment from "@mui/material/InputAdornment";
import Visibility from "@mui/icons-material/Visibility";
import VisibilityOff from "@mui/icons-material/VisibilityOff";
import IconButton from "@mui/material/IconButton";
import { PreRegisterfunc } from "../../../services/customerServices";
import MainLoader from "../../../components/MainLoader";
import { GoogleLogin } from "@react-oauth/google";
import { jwtDecode } from "jwt-decode";
import { useLocation, useNavigate } from "react-router-dom";
import { useAlert } from "../../../contexts/AlertContext";

export function PreRegister() {
  const location = useLocation();
  const navigate = useNavigate();

  const queryParams = new URLSearchParams(location.search);
  const preFilledEmail = queryParams.get("email") || "";

  const [showPassword, setShowPassword] = useState(false);
  const [password, setPassword] = useState("");
  const [email, setEmail] = useState(preFilledEmail);
  const [confirmPassword, setConfirmPassword] = useState("");
  const [IsLoading, setLoadingStatus] = useState(false);
  const { setAlert } = useAlert();

  const handleTogglePassword = () => {
    setShowPassword((prev) => !prev);
  };

  function handelEmailInputChange(value) {
    setEmail(value);
  }

  function handelPasswordInputChange(value) {
    setPassword(value);
  }

  function handelConfirmPasswrodChange(value) {
    setConfirmPassword(value);
  }

  async function handelPreRegisterCliked() {
    if (!email || !password || !confirmPassword) {
      setAlert({
        type: "error",
        message: "يجب تعبئة جميع الحقول!",
        open: true,
      });
      return;
    }

    if (password !== confirmPassword) {
      setAlert({
        type: "error",
        message: "كلمة السر غير متطابقة!",
        open: true,
      });
      return;
    }

    setLoadingStatus(true);

    const response = await PreRegisterfunc(email, password);

    if (!response.Success) {
      alert(response.error.message);
      setLoadingStatus(false);
      return;
    }

    sessionStorage.setItem(
      "Person",
      JSON.stringify(response.data.registerdPersonDTO)
    );

    window.location.href = "/register";
  }

  const handleGoogleSuccess = (credentialResponse) => {
    const token = credentialResponse.credential;

    const decoded = jwtDecode(token);
    const googleEmail = decoded.email;
    setEmail(googleEmail);

    navigate(`/Preregister?email=${encodeURIComponent(googleEmail)}`);
  };

  const handleGoogleFailure = (error) => {
    setAlert({ type: "error", message: "فشل التسجيل مع Google!", open: true });
  };

  return (
    <div id="sign-up-container">
      <div id="sign-up-form">
        <h1>تسجيل أول مرة</h1>
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
            label="كلمة مرور جديدة"
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
          <TextField
            value={confirmPassword}
            label="تأكيد كلمة المرور"
            type="password"
            variant="outlined"
            fullWidth
            size="small"
            margin="dense"
            onChange={(event) => {
              handelConfirmPasswrodChange(event.target.value);
            }}
          />
          <h6 style={{ margin: "6px 0px" }}>أو سجل عبر google</h6>
          <GoogleLogin
            onSuccess={handleGoogleSuccess}
            onError={handleGoogleFailure}
          />
          <Button
            id="sign-up-btn"
            type="submit"
            variant="contained"
            color="success"
            sx={{ mt: 2, fontWeight: "bold" }}
            disabled={IsLoading}
            onClick={() => {
              handelPreRegisterCliked();
            }}
          >
            استكمال المعلومات
            <div className="arrow-wrapper">
              <div className="arrow"></div>
            </div>
          </Button>
        </form>

        {IsLoading ? <MainLoader top="50%" left="50%" /> : null}
      </div>
    </div>
  );
}

export default PreRegister;

import { createContext, useContext, useState } from "react";
import Snackbar from "@mui/material/Snackbar";
import IconButton from "@mui/material/IconButton";
import CloseIcon from "@mui/icons-material/Close";
import Alert from "@mui/material/Alert";

const AlertContext = createContext();

export function AlertProvider({ children }) {
  const [alert, setAlert] = useState({
    type: "success",
    message: "",
    open: false,
  });

  const handleClose = (event, reason) => {
    if (reason === "clickaway") {
      return;
    }

    setAlert({ ...alert, open: false });
  };

  const action = (
    <IconButton
      size="small"
      aria-label="close"
      color="inherit"
      onClick={handleClose}
    >
      <CloseIcon fontSize="small" />
    </IconButton>
  );

  return (
    <AlertContext.Provider value={{ setAlert }}>
      {children}
      <Snackbar
        open={alert.open}
        autoHideDuration={2500}
        onClose={handleClose}
        message="Note archived"
        action={action}
        anchorOrigin={{ vertical: "bottom", horizontal: "right" }}
      >
        <Alert
          onClose={handleClose}
          severity={alert.type}
          variant="filled"
          sx={{ width: "100%" }}
        >
          {alert.message}
        </Alert>
      </Snackbar>
    </AlertContext.Provider>
  );
}

export default AlertProvider;

export const useAlert = () => {
  const alert = useContext(AlertContext);
  return alert;
};

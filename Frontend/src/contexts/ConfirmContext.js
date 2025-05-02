import {
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  Button,
} from "@mui/material";
import { useState, createContext, useContext } from "react";

const ConfirmContext = createContext();

export function ConfirmProvider({ children }) {
  const [confirmState, setConfirmState] = useState({
    open: false,
    message: "",
    resolve: null,
  });

  const confirm = (message) => {
    return new Promise((resolve) => {
      setConfirmState({
        ...confirmState,
        open: true,
        message: message,
        resolve: resolve,
      });
    });
  };

  function handleClose(result) {
    if (confirmState.resolve) {
      confirmState.resolve(result);
    }

    setConfirmState({
      ...confirmState,
      open: false,
      message: "",
      resolve: null,
    });
  }

  return (
    <ConfirmContext.Provider value={{ confirm }}>
      {children}
      <Dialog
        open={confirmState.open}
        onClose={() => handleClose(false)}
        sx={{ direction: "rtl" }}
      >
        <DialogTitle>تأكيد</DialogTitle>
        <DialogContent>{confirmState.message}</DialogContent>
        <DialogActions>
          <Button onClick={() => handleClose(false)} color="error">
            لا
          </Button>
          <Button onClick={() => handleClose(true)} color="primary">
            نعم
          </Button>
        </DialogActions>
      </Dialog>
    </ConfirmContext.Provider>
  );
}

export function useConfirm() {
  const context = useContext(ConfirmContext);
  if (!context) {
    throw new Error("useConfirm يجب أن يُستخدم ضمن ConfirmProvider");
  }
  return context.confirm;
}

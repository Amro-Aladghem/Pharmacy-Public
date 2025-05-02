import { Box } from "@mui/material";
import Typography from "@mui/joy/Typography";
import AccountBalanceWalletIcon from "@mui/icons-material/AccountBalanceWallet";
import DeliveryDiningIcon from "@mui/icons-material/DeliveryDining";
import "./CheckOut.css";

const Methodes = [
  {
    id: 5,
    about: "خدمة كليك أو محفظة الكترونية",
    icon: <AccountBalanceWalletIcon />,
    name: "wallet-click",
  },
  {
    id: 4,
    about: "الدفع عند الأستلام",
    icon: <DeliveryDiningIcon />,
    name: "on-delivery",
  },
];

export function PaymentMethodes({ paymentMethode, setpaymentMethode }) {
  const handleChange = (event) => {
    setpaymentMethode(event.target.value);
  };

  const PaymentMethodesList = Methodes.map((methode, index) => {
    return (
      <Box
        key={index}
        sx={{
          width: "100%",
          display: "flex",
          alignItems: "center",
        }}
      >
        {methode.icon}
        <Typography level="title-lg" sx={{ display: "inline" }}>
          {methode.about}
        </Typography>
        <input
          className="radio-check-pay"
          value={methode.id}
          type="radio"
          checked={paymentMethode == methode.id}
          onChange={(event) => handleChange(event)}
        />
      </Box>
    );
  });

  return <>{PaymentMethodesList}</>;
}

export default PaymentMethodes;

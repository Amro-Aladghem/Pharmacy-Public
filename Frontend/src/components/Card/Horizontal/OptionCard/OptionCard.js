import "./OptionCard.css";
import ContactsIcon from "@mui/icons-material/Contacts";
import LocalMallIcon from "@mui/icons-material/LocalMall";
import DateRangeIcon from "@mui/icons-material/DateRange";
import FmdGoodIcon from "@mui/icons-material/FmdGood";
import KeyboardReturnIcon from "@mui/icons-material/KeyboardReturn";
import SupportAgentIcon from "@mui/icons-material/SupportAgent";
import "../../../../assets/Styles/Mainbtn.css";
import AutoModeIcon from "@mui/icons-material/AutoMode";
import RememberMeIcon from "@mui/icons-material/RememberMe";
import { useNavigate } from "react-router-dom";

const icons = {
  accoutnInfo: <ContactsIcon />,
  orderHistory: <LocalMallIcon />,
  appointment: <DateRangeIcon />,
  loaction: <FmdGoodIcon />,
  return: <KeyboardReturnIcon />,
  support: <SupportAgentIcon />,
  active: <AutoModeIcon />,
  vediocall: <RememberMeIcon />,
};

export function OptionCard({ optionName, page, type, buttonName }) {
  const navigate = useNavigate();

  return (
    <>
      <div id="option-card-container">
        <div id="option-name">
          {icons[type]}
          <h4>{optionName}</h4>
        </div>

        <button
          className="main-btn"
          style={{ width: "fit-content" }}
          onClick={() => {
            navigate(`${page}`);
          }}
        >
          {!buttonName ? "الذهاب" : buttonName}
          <div className="arrow-wrapper">
            <div className="arrow"></div>
          </div>
        </button>
      </div>
    </>
  );
}

export default OptionCard;

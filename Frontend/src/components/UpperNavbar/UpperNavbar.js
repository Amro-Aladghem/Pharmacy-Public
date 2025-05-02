import { styled, alpha } from "@mui/material/styles";
import AppBar from "@mui/material/AppBar";
import Box from "@mui/material/Box";
import Toolbar from "@mui/material/Toolbar";
import IconButton from "@mui/material/IconButton";
import Typography from "@mui/material/Typography";
import InputBase from "@mui/material/InputBase";
import MenuIcon from "@mui/icons-material/Menu";
import SearchIcon from "@mui/icons-material/Search";
import { Button } from "@mui/material";
import "./UpperNavbar.css";
import Badge from "@mui/material/Badge";
import ShoppingBagIcon from "@mui/icons-material/ShoppingBag";
import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import ArrowBackIcon from "@mui/icons-material/ArrowBack";
import { useContext } from "react";
import { ItemsCountContext } from "../../contexts/ItemsCountContext";
import { useAuth } from "../../contexts/AuthContext";

const Search = styled("div")(({ theme }) => ({
  position: "relative",
  borderRadius: theme.shape.borderRadius,
  backgroundColor: alpha(theme.palette.common.white, 0.15),
  "&:hover": {
    backgroundColor: alpha(theme.palette.common.white, 0.25),
  },
  marginLeft: 0,
  width: "70%",
  [theme.breakpoints.up("sm")]: {
    marginLeft: theme.spacing(1),
    width: "auto",
  },
  direction: "ltr",
}));

const SearchIconWrapper = styled("div")(({ theme }) => ({
  padding: theme.spacing(0, 2),
  height: "100%",
  position: "absolute",
  pointerEvents: "none",
  display: "flex",
  alignItems: "center",
  justifyContent: "center",
  cursor: "pointer",
}));

const StyledInputBase = styled(InputBase)(({ theme }) => ({
  direction: "rtl",
  color: "black",
  width: "100%",
  "& .MuiInputBase-input": {
    padding: theme.spacing(1, 1, 1, 0),

    paddingLeft: `calc(1em + ${theme.spacing(4)})`,
    transition: theme.transitions.create("width"),
    [theme.breakpoints.up("sm")]: {
      width: "12ch",
      "&:focus": {
        width: "20ch",
      },
    },
  },
}));

const menustyle = {
  my: "2",
  display: "block",
  fontSize: "large",
  marginRight: "16px",
  color: "black",
};

const pages = [
  { section: "الرئيسية", pagePath: "/" },
  { section: "الصيدليات", pagePath: "/pharmacies?Governorate=1" },
  { section: "استشارة", pagePath: "/pharmacies/have-meeting" },
];

export function UpperNavbar() {
  const { authInfo } = useAuth();
  const [searchtext, setSearchText] = useState("");

  const navigate = useNavigate();
  const { itemsCount } = useContext(ItemsCountContext);

  const pagesList = pages.map((page, index) => {
    return (
      <Button
        key={index}
        sx={menustyle}
        onClick={() => navigate(page.pagePath)}
      >
        {" "}
        {page.section}{" "}
      </Button>
    );
  });

  useEffect(() => {}, []);

  return (
    <Box id="Upper-nav-contnier">
      <AppBar
        position="static"
        sx={{ backgroundColor: "rgb(255, 86, 114)", width: "100%" }}
      >
        <Toolbar>
          <Typography variant="h6" noWrap component="div" sx={{ flexGrow: 1 }}>
            MUI
          </Typography>
          <Box sx={{ flexGrow: 15, display: { xs: "none", md: "flex" } }}>
            {pagesList}

            <IconButton
              onClick={() => navigate("/cart")}
              sx={{ marginRight: "16px" }}
            >
              <Badge badgeContent={itemsCount} color="secondary">
                <ShoppingBagIcon sx={{ color: "rgb(2, 205, 205)" }} />
              </Badge>
            </IconButton>

            <Button
              onClick={() =>
                navigate(authInfo.loggedIn ? "/customer/profile" : "/signin")
              }
            >
              <img
                id="Upper-nav-img"
                src="https://res.cloudinary.com/dlu3aolnh/image/upload/v1735059932/udjkg3nflplvydkrngf0.png"
                alt=""
              />
              <span style={{ color: "black" }}>
                {authInfo.loggedIn ? authInfo.userName : "تسجيل الدخول"}
              </span>
            </Button>
          </Box>

          <Search>
            <SearchIconWrapper>
              <SearchIcon />
            </SearchIconWrapper>
            <StyledInputBase
              placeholder="ابحث باسم دواء"
              inputProps={{ "aria-label": "search" }}
              value={searchtext}
              onChange={(e) => setSearchText(e.target.value)}
            />
          </Search>
          <IconButton
            disabled={searchtext == "" || searchtext == null}
            onClick={() => {
              navigate(`/search?searchtext=${searchtext}`);
            }}
            sx={{ backgroundColor: "rgb(2, 205, 205)", mx: 1, color: "white" }}
          >
            <ArrowBackIcon size="small" />
          </IconButton>
        </Toolbar>
      </AppBar>
    </Box>
  );
}

export default UpperNavbar;

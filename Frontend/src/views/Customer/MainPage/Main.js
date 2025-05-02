import Navbar from "../../../components/Navbar/Navbar";
import Carousel from "../../../components/Carousel";
import CustomeCard from "../../../components/Card/CustomeCard";
import Typography from "@mui/joy/Typography";
import { Box } from "@mui/material";
import Grid from "@mui/material/Grid2";
import HorizontalCard from "../../../components/Card/Horizontal/HorizontalCard";
import PersonPinIcon from "@mui/icons-material/PersonPin";
import "./Main.css";
import PopularProduct from "./PopularProduct/PopularProduct";
import RandomPharmacies from "./RandomPharmacies/RandomPharmacies";
import { useNavigate } from "react-router-dom";

const cards = [
  {
    Image: "pharmacyImage.jpg",
    body: "تعرف على الصيدليات ومنتجاتها",
    page: "/pharmacies",
  },
  {
    Image: "medicalsImage.jpg",
    body: "ابحث عن الأدوية او منتجات",
    page: "products",
  },
  {
    Image: "vediocallImage.jpg",
    body: "احجز جلسة أونلاين ",
    page: "/pharmacies/have-meeting",
  },
];

const cardsList = cards.map((card, index) => {
  return (
    <CustomeCard
      key={index}
      page={card.page}
      image={card.Image}
      body={card.body}
    />
  );
});

export function Main() {
  const navigate = useNavigate();

  return (
    <div style={{ direction: "rtl" }}>
      <Carousel />

      <Box sx={{ textAlign: "center" }}>
        <Typography level="h1">عالم الصيدليات بين يديك</Typography>
      </Box>

      <Grid sx={{ my: 2, marginBottom: "50px" }} container spacing={2}>
        {cardsList}
      </Grid>

      <HorizontalCard Type="Order" title="طلباتك النشطة" />
      <HorizontalCard Type="Request" title="موعد مكالمة نشط" />

      <Box sx={{ textAlign: "center", mt: "20px" }}>
        <Typography level="h2" alignSelf={"center"}>
          ابحث عن أقرب صيدلية لموقعك
        </Typography>
        <PersonPinIcon />
      </Box>

      <div id="Nearer-Ph-section-container">
        <div id="Nearer-Ph-section">
          <img id="Nearer-section-img" src="pharmacylocation.png" alt="" />

          <div id="Nearer-section-body">
            <Typography level="h4" sx={{ mb: "30px" }}>
              صيدليات قريبةوتوصيل سريع
            </Typography>

            <button
              className="main-btn"
              onClick={() => {
                navigate("/pharmacies/nearest");
              }}
            >
              الذهاب
              <div className="arrow-wrapper">
                <div className="arrow"></div>
              </div>
            </button>
          </div>
        </div>
      </div>

      <Typography level="h3">الأدوية والمنتجات الأكثر شيوعا : </Typography>

      <PopularProduct />

      <Typography level="h3"> اكتشف الصيديلات ومنتجاتها: </Typography>

      <RandomPharmacies />
    </div>
  );
}

export default Main;

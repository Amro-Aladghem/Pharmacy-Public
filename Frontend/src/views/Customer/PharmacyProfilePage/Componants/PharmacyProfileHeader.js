import { Box } from "@mui/material";
import { CircularProgress } from "@mui/material";
import StoreIcon from "@mui/icons-material/Store";
import PlaceIcon from "@mui/icons-material/Place";
import Typography from "@mui/joy/Typography";

export function PharmacyProfileHeader({ info, loading, result }) {
  function goToProductsSection() {
    window.scrollTo({
      top: document.body.scrollHeight,
      behavior: "smooth",
    });
  }
  return (
    <>
      <div id="bg-container">
        <Box sx={{ width: { lg: "70%", md: "100%", xs: "100%" } }}>
          <img src="/Background.png" alt="background" width="100%" />
        </Box>
      </div>

      {loading ? (
        <CircularProgress
          size={40}
          sx={{ position: "absolute", top: "150px", right: "50%" }}
        />
      ) : !result ? (
        <Typography sx={{ position: "absolute", top: "150px", right: "50%" }}>
          فشل تحميل البيانات
        </Typography>
      ) : (
        <Box
          id="Ph-header-container"
          sx={{
            width: "100%",
            display: "flex",
            justifyContent: "center",
            alignItems: "center",
            position: "absolute",
            top: "150px",
          }}
        >
          <Box
            sx={{
              borderRadius: "10px",
              border: "1px solid black",
              width: { md: "50%", sm: "90%", xs: "90%" },
              height: { md: "150px", sm: "100px", xs: "100px" },
              background: "white",
              display: "flex",
              padding: "5px 5px",
              alignItems: "center",
              direction: "ltr",
            }}
          >
            <img id="pharmacy-img" src={info.imageURL} alt="" />
            <div id="pharmacy-info">
              <div style={{ marginBottom: "30px" }}>
                <h2 style={{ display: "inline", backgroundColor: "aqua" }}>
                  صيدلية {info.pharmacyName}{" "}
                </h2>
                <StoreIcon
                  sx={{ color: "rgb(255, 86, 114)", display: "inline" }}
                />
              </div>

              <div>
                <h2 style={{ display: "inline" }}>{info.governorate} </h2>
                <PlaceIcon
                  sx={{ color: "rgb(255, 86, 114)", display: "inline" }}
                />
              </div>
            </div>
          </Box>
        </Box>
      )}

      <div
        style={{
          display: "flex",
          justifyContent: "center",
          marginBottom: "10px",
        }}
      >
        <button
          className="main-btn"
          style={{ width: "fit-content" }}
          onClick={() => {
            goToProductsSection();
          }}
        >
          عرض المنتجات
          <div className="arrow-wrapper">
            <div className="arrow"></div>
          </div>
        </button>
      </div>
    </>
  );
}

export default PharmacyProfileHeader;

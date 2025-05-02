import ProductCard from "../../../../components/Card/ProductCard/ProductCard";
import ArrowBackIcon from "@mui/icons-material/ArrowBack";
import { IconButton } from "@mui/material";
import { useNavigate } from "react-router-dom";
import MainLoader from "../../../../components/MainLoader";
import { useState, useEffect } from "react";
import { GetProducts } from "../../../../services/phProductsServices";

export function PopularProduct() {
  const [Loading, setLoading] = useState(true);
  const [cards, setCards] = useState([]);
  const [result, setResult] = useState(true);
  const navigate = useNavigate();

  useEffect(() => {
    fetchData();
  }, []);

  const fetchData = async () => {
    const Paginated = {
      Limit: 5,
      LastPhProductId: 0,
      IsRowsCountCalculated: false,
      CategoryId: 1,
    };

    const responseResult = await GetProducts(Paginated);

    if (!responseResult.IsSuccess) {
      setLoading(false);
      setResult(false);
      return;
    }

    setCards(responseResult.result.products);

    setLoading(false);
  };

  const ProductCards = cards.map((Product) => {
    return (
      <ProductCard
        key={Product.phProductId}
        Id={Product.phProductId}
        Image={Product.productImageURL}
        Name={Product.name}
        PharmacyName={Product.phName}
        Price={Product.price}
        PharmacyId={Product.pharmacyId}
      />
    );
  });

  return (
    <div id="main-popular-container" sx={{ gap: { xs: 4, md: 16 } }}>
      {Loading ? (
        <MainLoader />
      ) : result ? (
        <>
          {ProductCards}
          <IconButton
            sx={{
              alignSelf: "center",
              flexShrink: "0",
            }}
          >
            <ArrowBackIcon />
          </IconButton>
        </>
      ) : (
        <p>فشل التحميل</p>
      )}
    </div>
  );
}

export default PopularProduct;

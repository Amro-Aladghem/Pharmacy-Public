import { GetProductFromSearchingByName } from "../../../services/phProductsServices";
import { useEffect, useState } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import InfiniteScroll from "react-infinite-scroll-component";
import Grid from "@mui/material/Grid2";
import CircularProgress from "@mui/material/CircularProgress";
import ProductCard from "../../../components/Card/ProductCard/ProductCard";
import "./SearchProduct.css";

export function SearchProduct() {
  const location = useLocation();
  const navigate = useNavigate();

  const [loading, setLoading] = useState(true);
  const [result, setResult] = useState(true);

  const [products, setProducts] = useState([]);
  const [paginated, setPaginated] = useState({
    searchtext: "",
    LastPhProductId: 0,
  });

  const fetchData = async (isFirstTime = false, searchText = "") => {
    setLoading(true);

    const Paginated = {
      LastPhProductId: isFirstTime ? 0 : paginated.LastPhProductId,
      searchtext: isFirstTime ? searchText : paginated.searchtext,
    };

    if (isFirstTime) {
      setProducts([]);

      setPaginated((prev) => {
        return {
          ...prev,
          searchtext: searchText,
          LastPhProductId: 0,
        };
      });
    }

    const responseResult = await GetProductFromSearchingByName(Paginated);

    setLoading(false);

    if (!responseResult.IsSuccess || !responseResult.result) {
      setResult(false);
      return;
    }

    setProducts((prev) => [...prev, ...responseResult.result.products]);

    setResult(true);

    setPaginated((prev) => {
      return {
        ...prev,
        LastPhProductId: responseResult.result.lastPhProductId,
      };
    });
  };

  useEffect(() => {
    const params = new URLSearchParams(location.search);
    let searchtext = params.get("searchtext");

    if (!searchtext) {
      return;
    }

    fetchData(true, searchtext);
  }, [location.search]);

  const ProductCards = products.map((Product) => {
    return (
      <Grid
        size={{ xs: 6, md: 3, sm: 4 }}
        sx={{ display: "flex", justifyContent: "center" }}
      >
        <ProductCard
          key={Product.phProductId}
          Id={Product.phProductId}
          Image={Product.productImageURL}
          Name={Product.name}
          PharmacyName={Product.phName}
          Price={Product.price}
        />
      </Grid>
    );
  });

  return (
    <>
      <InfiniteScroll
        dataLength={products.length}
        next={fetchData}
        hasMore={result}
      >
        <Grid
          container
          spacing={1}
          sx={{
            my: 4,
            direction: "rtl",
            display: "flex",
            justifyContent: "center",
          }}
        >
          {ProductCards}

          {!result && paginated.LastPhProductId == 0 ? (
            <p>لم يتم العثور على اي أدوية</p>
          ) : null}

          {loading ? (
            <CircularProgress size={40} color="rgb(255, 86, 114)" />
          ) : null}

          {!result && paginated.LastPhProductId != 0 ? (
            <div id="note-category-page">
              <h4 style={{ flexGrow: "1" }}>
                لم تجد ما تبحث عنه؟ جرب البحث في الأقسام
              </h4>
              <button
                className="main-btn"
                style={{ width: "fit-content" }}
                onClick={() => {
                  navigate("/products");
                }}
              >
                الذهاب
                <div className="arrow-wrapper">
                  <div className="arrow"></div>
                </div>
              </button>
            </div>
          ) : null}
        </Grid>
      </InfiniteScroll>
    </>
  );
}

export default SearchProduct;

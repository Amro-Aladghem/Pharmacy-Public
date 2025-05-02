import InfiniteScroll from "react-infinite-scroll-component";
import CircularProgress from "@mui/material/CircularProgress";
import { useState, useEffect } from "react";
import { GetPharmacyProducts } from "../../../../services/phProductsServices";
import ProductCard from "../../../../components/Card/ProductCard/ProductCard";
import Grid from "@mui/material/Grid2";
import { useParams } from "react-router-dom";

export function PharmacyProducts() {
  const { pharmacyId } = useParams();
  const [products, setProducts] = useState([]);
  const [paginated, setPaginated] = useState({
    lastPhProductId: null,
    limit: 8,
    IsRowCountCalculated: false,
  });
  const [loading, setLoading] = useState(false);
  const [result, setResult] = useState(true);

  async function fetchData(isFirstTime = false) {
    const Paginated = {
      LastPhProductId: isFirstTime ? 0 : paginated.lastPhProductId,
      Limit: paginated.limit,
      IsRowsCountCalculated: paginated.IsRowCountCalculated,
      PharmacyId: pharmacyId,
    };

    setLoading(true);

    const response = await GetPharmacyProducts(Paginated);

    setLoading(false);

    if (!response.IsSuccess || !response.result) {
      setResult(false);
      return;
    }

    setProducts((prev) => [...prev, ...response.result.products]);

    setPaginated((prev) => {
      return {
        ...prev,
        lastPhProductId: response.result.lastPhProductId,
        ...(isFirstTime && {
          IsRowsCountCalculated: true,
        }),
      };
    });
  }

  const ProductCards = products.map((Product) => {
    return (
      <Grid
        size={{ xs: 6, md: 2.4, sm: 4 }}
        sx={{ display: "flex", justifyContent: "center" }}
      >
        <ProductCard
          key={Product.phProductId}
          Id={Product.phProductId}
          Image={Product.productImageURL}
          Name={Product.name}
          PharmacyName={Product.phName}
          Price={Product.price}
          PharmacyId={Product.pharmacyId}
        />
      </Grid>
    );
  });

  useEffect(() => {
    fetchData(true);
  }, []);
  return (
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
        {!result ? (
          <p>لا يوجد المزيد من الأدوية والمنتجات بهذه الصيدلية </p>
        ) : null}

        {loading ? (
          <CircularProgress size={40} color="rgb(255, 86, 114)" />
        ) : null}
      </Grid>
    </InfiniteScroll>
  );
}

export default PharmacyProducts;

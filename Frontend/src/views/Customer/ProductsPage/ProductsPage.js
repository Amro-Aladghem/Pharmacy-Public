import "./ProductsPage.css";
import { useEffect, useState } from "react";
import { GetProducts } from "../../../services/phProductsServices";
import ProductCard from "../../../components/Card/ProductCard/ProductCard";
import { useLocation } from "react-router-dom";
import Grid from "@mui/material/Grid2";
import CircularProgress from "@mui/material/CircularProgress";
import InfiniteScroll from "react-infinite-scroll-component";
import RegionsMenue from "../../../components/RegionsMenu/RegionsMenu";
import GovernoratesMenu from "../../../components/GovernoratesMenu/GovernoratesMenu";
import { Box } from "@mui/material";

export function ProductsPage() {
  const location = useLocation();

  const [paginated, setPaginated] = useState({
    LastPhProductId: 0,
    Limit: 10,
    IsRowsCountCalculated: false,
    CategoryId: 0,
    GovernorateId: null,
    RegionId: null,
  });
  const [products, setProducts] = useState([]);
  const [loading, setLoading] = useState(true);
  const [result, setResult] = useState(true);

  const fetchData = async (
    isFirstTime = false,
    categoryId = 0,
    GovernorateId,
    RegionId
  ) => {
    const Paginated = {
      LastPhProductId: isFirstTime ? 0 : paginated.LastPhProductId,
      Limit: paginated.Limit,
      IsRowsCountCalculated: paginated.IsRowsCountCalculated,
      CategoryId: isFirstTime ? categoryId : paginated.CategoryId,
      GovernorateId: isFirstTime ? GovernorateId : paginated.GovernorateId,
      RegionId: isFirstTime ? RegionId : paginated.RegionId,
    };

    if (isFirstTime) {
      setProducts([]);
    }

    setLoading(true);
    const responseResult = await GetProducts(Paginated);

    setLoading(false);

    if (!responseResult.IsSuccess) {
      setResult(false);
      return;
    }

    setProducts((prev) => [...prev, ...responseResult.result.products]);

    setResult(true);

    setPaginated((prev) => {
      return {
        ...prev,
        LastPhProductId: responseResult.result.lastPhProductId,
        ...(isFirstTime && {
          CategoryId: categoryId,
          IsRowsCountCalculated: true,
          GovernorateId: GovernorateId,
          RegionId: RegionId,
        }),
      };
    });
  };

  useEffect(() => {
    const params = new URLSearchParams(location.search);
    let categoryId = Number(params.get("category"));
    let GovernorateId = Number(params.get("Governorate"));
    let RegionId = Number(params.get("Region"));

    categoryId = categoryId || 1;

    fetchData(true, categoryId, GovernorateId, RegionId);
  }, [location.search]);

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

  return (
    <>
      <Box
        sx={{
          width: "100%",
          display: "flex",
          justifyContent: "center",
          gap: { xs: "12px", md: "20px" },
        }}
      >
        <RegionsMenue />
        <GovernoratesMenu />
      </Box>
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
          {!result ? <p>لا يوجد المزيد من الأدوية في هذا القسم</p> : null}

          {loading ? (
            <CircularProgress size={40} color="rgb(255, 86, 114)" />
          ) : null}
        </Grid>
      </InfiniteScroll>
    </>
  );
}

export default ProductsPage;

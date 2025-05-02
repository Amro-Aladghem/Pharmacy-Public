import AspectRatio from "@mui/joy/AspectRatio";
import Link from "@mui/joy/Link";
import Card from "@mui/joy/Card";
import CardContent from "@mui/joy/CardContent";
import Typography from "@mui/joy/Typography";
import QuantityButton from "../QuantityButton/QuantityButton";
import { Box } from "@mui/material";
import CartItemActions from "./CartItemActions";

export function CartItem({
  itemId,
  image,
  name,
  priceForOne,
  quantity,
  price,
  handelItemChange,
  stoke,
}) {
  return (
    <Card
      id="pharmacy-card"
      variant="outlined"
      orientation="horizontal"
      sx={{
        width: { md: "85%", xs: "90%" },
        margin: "14px 0px",
        "&:hover": {
          boxShadow: "md",
          borderColor: "neutral.outlinedHoverBorder",
        },
        direction: "rtl",
      }}
    >
      <AspectRatio ratio="1" sx={{ width: 90 }}>
        <img src={image} alt="product-Image" />
      </AspectRatio>

      <CardContent>
        <Box style={{ display: "flex", alignItems: "center" }}>
          <Box>
            <Typography level="title-lg" id="card-description">
              {name}
            </Typography>
            <Typography
              level="body-sm"
              aria-describedby="card-description"
              sx={{ mb: 0.1 }}
            >
              سعر المنتج للواحد :{priceForOne}jd
            </Typography>

            <Typography
              level="title-lg"
              sx={{
                backgroundColor: "aqua",
                borderRadius: "8px",
                width: "fit-content",
                padding: "2px 2px",
              }}
            >
              السعر:{price} jd
            </Typography>
          </Box>

          <CartItemActions
            itemId={itemId}
            quantity={quantity}
            handelItemChange={handelItemChange}
            stoke={stoke}
          />
        </Box>
      </CardContent>
    </Card>
  );
}

export default CartItem;

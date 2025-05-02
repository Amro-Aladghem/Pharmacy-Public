import { Box, stepLabelClasses } from "@mui/material";
import QuantityButton from "../QuantityButton/QuantityButton";
import DeleteIcon from "@mui/icons-material/Delete";
import { IconButton } from "@mui/material";
import { useConfirm } from "../../contexts/ConfirmContext";
import { DeleteItemFromCart } from "../../services/orderServices";
import { useAlert } from "../../contexts/AlertContext";

export function CartItemActions({ itemId, quantity, handelItemChange, stoke }) {
  const confirm = useConfirm();
  const { setAlert } = useAlert();

  async function handelDeleteItem() {
    const result = await confirm("هل أنت متأكد أنك تريد حذف هذا المنتج ؟");

    if (result) {
      const customerId = JSON.parse(
        sessionStorage.getItem("customer")
      ).customerId;
      const response = await DeleteItemFromCart(
        { ItemId: itemId, Quantity: quantity },
        customerId
      );

      if (!response.IsSuccess) {
        setAlert({
          type: "error",
          open: true,
          message: "فشل حذف المنج",
        });
        return;
      }

      handelItemChange(
        { itemId: itemId, subPriceDiff: response.result },
        "delete"
      );
    }
  }

  return (
    <Box
      sx={{
        display: "flex",
        flexDirection: "column",
        justifyContent: "center",
        marginRight: "auto",
        alignItems: "center",
      }}
    >
      <QuantityButton
        itemId={itemId}
        quantity={quantity}
        handelItemChange={handelItemChange}
        stoke={stoke}
      />
      <IconButton onClick={() => handelDeleteItem()}>
        <DeleteIcon />
      </IconButton>
    </Box>
  );
}

export default CartItemActions;

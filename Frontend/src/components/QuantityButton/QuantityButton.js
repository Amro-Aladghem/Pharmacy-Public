import "./QuantityButton.css";
import { UpdateCartItem } from "../../services/orderServices";
import { useEffect, useState } from "react";
import { useAlert } from "../../contexts/AlertContext";

export function QuantityButton({ itemId, quantity, handelItemChange, stoke }) {
  const [customerId, setCustomerId] = useState(0);
  const { setAlert } = useAlert();

  async function handelQuantitybtnClick(number) {
    const newValue = quantity + number;
    const isValied = newValue != 0 || newValue > stoke;

    if (!isValied) {
      setAlert({
        type: "error",
        message: "لا يمكنك الأنقاص اكثر من ذلك",
        open: true,
      });
      return;
    }

    const updateObj = {
      ItemId: itemId,
      Quantity: newValue,
    };

    const response = await UpdateCartItem(updateObj, customerId);

    if (!response.IsSuccess) {
      setAlert({
        type: "error",
        message: "فشل تحديث الكمية للمنتج",
        open: true,
      });
      return;
    }

    handelItemChange(response.result);
  }

  useEffect(() => {
    const customer = JSON.parse(sessionStorage.getItem("customer"));

    if (customer) {
      setCustomerId(customer.customerId);
      return;
    }
  }, []);

  return (
    <div className="quantity-btn-countainer">
      <button
        className="quantity__btn"
        onClick={() => handelQuantitybtnClick(-1)}
      >
        -
      </button>
      <div className="quantity-btn-score">{quantity}</div>
      <button
        className="quantity__btn quantity-btn-plus"
        onClick={() => handelQuantitybtnClick(1)}
      >
        +
      </button>
    </div>
  );
}

export default QuantityButton;

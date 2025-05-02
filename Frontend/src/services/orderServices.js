import axios from "axios";
import Cart from "../views/Customer/CartPage/Cart";

const baseURL = "https://localhost:7204/api/v1";

export const GetMostActiveOrderForCustomer = async (CustomerId) => {
  const URL = baseURL + "/Order/Get/Active/" + CustomerId;

  try {
    const response = await axios.get(URL, {
      withCredentials: true,
      credentials: "include",
    });

    if (!response.data.order) {
      return {
        IsSuccess: true,
        result: null,
      };
    }

    const { id, pharmacyName, isNotAccepted, isTheSameDay } =
      response.data.order;

    const result = { id, pharmacyName, isNotAccepted, isTheSameDay };

    return {
      IsSuccess: true,
      result,
    };
  } catch (error) {
    return {
      IsSuccess: false,
      error: error.response,
    };
  }
};

export const GetOrdersHistoryForTheCustomer = async (CustomerId) => {
  const URL = `${baseURL}/Order/history/${CustomerId}`;

  try {
    const response = await axios.get(URL, {
      withCredentials: true,
      credentials: "include",
    });

    const result = response.data.ordersList;

    return {
      IsSuccess: true,
      result,
    };
  } catch (error) {
    return {
      IsSuccess: false,
      result: error.response,
    };
  }
};

export const CreateCart = async (CustomerId, cartItemDTO) => {
  const URL = `${baseURL}/cart/add/${CustomerId}`;

  const form = new FormData();

  form.append("PhProductId", cartItemDTO.PhProductId);
  form.append("PharmacyId", cartItemDTO.PharmacyId);
  form.append("Quantity", cartItemDTO.Quantity);

  try {
    const response = await axios.post(URL, form, {
      withCredentials: true,
      credentials: "include",
    });

    const result = response.data.cart;

    return {
      IsSuccess: true,
      result,
    };
  } catch (error) {
    return {
      IsSuccess: false,
      error: error.response,
    };
  }
};

export const AddItemToCart = async (NewCartItemObj, CustomerId, CartId) => {
  const URL = `${baseURL}/cart/cart-items/add`;

  const form = new FormData();

  form.append("CartId", CartId);
  form.append("CustomerId", CustomerId);
  form.append("CartItem.PhProductId", NewCartItemObj.PhProductId);
  form.append("CartItem.PharmacyId", NewCartItemObj.PharmacyId);
  form.append("CartItem.Quantity", NewCartItemObj.Quantity);

  try {
    const response = await axios.post(URL, form, {
      withCredentials: true,
      credentials: "include",
    });

    const result = response.data.cart;

    return {
      IsSuccess: true,
      result,
    };
  } catch (error) {
    return {
      IsSuccess: false,
      error: error.response,
    };
  }
};

export const ShowCustomerCart = async (CartId, CustomerId) => {
  const URL = `${baseURL}/cart/show/${CartId}?CustomerId=${CustomerId}`;

  try {
    const response = await axios.get(URL, {
      withCredentials: true,
      credentials: "include",
    });

    if (!response.data.cartList) {
      return {
        IsSucess: true,
        result: null,
      };
    }

    const result = response.data.cartList;

    return {
      IsSuccess: true,
      result,
    };
  } catch (error) {
    return {
      IsSuccess: false,
      result: error.response,
    };
  }
};

export const UpdateCartItem = async (updateCartItemObj, CustomerId) => {
  const URL = `${baseURL}/cart/item/update/${CustomerId}`;

  const form = new FormData();

  form.append("ItemId", updateCartItemObj.ItemId);
  form.append("Quantity", updateCartItemObj.Quantity);

  try {
    const response = await axios.put(URL, form, {
      withCredentials: true,
      credentials: "include",
    });

    const result = response.data.cartList;

    return {
      IsSuccess: true,
      result,
    };
  } catch (error) {
    return {
      IsSuccess: false,
      error: error.response,
    };
  }
};

export const DeleteItemFromCart = async (updateCartItemObj, CustomerId) => {
  const URL = `${baseURL}/cart/item/delete/${CustomerId}`;

  const form = new FormData();

  form.append("ItemId", updateCartItemObj.ItemId);
  form.append("Quantity", updateCartItemObj.Quantity);

  try {
    const response = await axios.delete(URL, {
      data: form,
      withCredentials: true,
      credentials: "include",
    });

    return {
      IsSuccess: response.data.result.isDone,
      result: response.data.result.diff,
    };
  } catch (error) {
    return {
      IsSuccess: false,
      error: error.response,
    };
  }
};

export const VerifyCustomerLocationAtCheckOut = async (
  CustomerId,
  Longitude,
  Latitude
) => {
  const URL = `${baseURL}/order/checkout/verify-location`;

  const form = new FormData();

  form.append("CustomerId", CustomerId);
  form.append("Longitude", Longitude);
  form.append("Latitude", Latitude);

  try {
    const response = await axios.post(URL, form);

    const isChanged = response.data.result;

    return {
      IsSuccess: true,
      isChanged,
    };
  } catch (error) {
    return {
      IsSuccess: false,
      error: error.response,
    };
  }
};

export const GetCartInfoAtCheckOut = async (CustomerId, CartId) => {
  const URL = `${baseURL}/cart/${CartId}/check-out/info/${CustomerId}`;

  try {
    const response = await axios.get(URL, {
      withCredentials: true,
      credentials: "include",
    });

    const cartInfo = response.data.cartInfo;

    return {
      IsSuccess: true,
      cartInfo,
    };
  } catch (error) {
    return {
      IsSuccess: false,
      error: error.response,
    };
  }
};

export const OrderCheckOut = async (CustomerId, CartId, PaymentMethodeId) => {
  const URL = `${baseURL}/order/checkout/on-delviery`;

  const form = new FormData();

  form.append("CartId", CartId);
  form.append("CustomerId", CustomerId);
  form.append("PaymentMethodId", PaymentMethodeId);

  try {
    const response = await axios.post(URL, form, {
      withCredentials: true,
      credentials: "include",
    });

    const result = response.data.result;

    if (!result.isDone) {
      return {
        IsSuccess: false,
        result: "Failed to add this order!",
      };
    }

    return {
      IsSuccess: true,
      result,
    };
  } catch (error) {
    return {
      IsSuccess: false,
      result: error.response,
    };
  }
};

export const handleOnDeliveryCheckOut = async (
  PaymentMethodeId,
  handelError
) => {
  const CustomerId = JSON.parse(sessionStorage.getItem("customer")).customerId;
  const CartId = JSON.parse(localStorage.getItem("cart")).cartId;

  const response = await OrderCheckOut(CustomerId, CartId, PaymentMethodeId);

  if (!response.IsSuccess) {
    handelError();
    return;
  }

  localStorage.setItem(
    "cart",
    JSON.stringify({ cartId: CartId, pharmacyId: null, numberOfItems: 0 })
  );

  window.location.href = `/?orderId=${response.result.orderID}`;
};

export const OrderCheckOutViaWhatsApp = async (
  CustomerId,
  CartId,
  PaymentMethodeId
) => {
  const URL = `${baseURL}/order/checkout/via-whatsapp/link`;

  const form = new FormData();

  form.append("CartId", CartId);
  form.append("CustomerId", CustomerId);
  form.append("PaymentMethodId", PaymentMethodeId);

  try {
    const response = await axios.post(URL, form, {
      withCredentials: true,
    });

    const result = response.data.result;

    if (!result.isDone) {
      return {
        IsSuccess: false,
        result: result.errorMessage,
      };
    }

    return {
      IsSuccess: true,
      result,
    };
  } catch (error) {
    return {
      IsSusccess: false,
      result: error.response,
    };
  }
};

export const handleCheckOutViaWhatsApp = async (
  CustomerId,
  PaymentMethodeId,
  handleError
) => {
  const CartId = JSON.parse(localStorage.getItem("cart")).cartId;

  const response = await OrderCheckOutViaWhatsApp(
    CustomerId,
    CartId,
    PaymentMethodeId
  );

  if (!response.IsSuccess) {
    handleError();
    return;
  }

  localStorage.setItem(
    "cart",
    JSON.stringify({ cartId: CartId, pharmacyId: null, numberOfItems: 0 })
  );

  window.open(response.result.url, "_blank");
  window.location.href = "/";
};

export const GetOrderStatusAndDetails = async (OrderId, CustomerId) => {
  const URL = `${baseURL}/order/${OrderId}/details?CustomerId=${CustomerId}`;

  try {
    const response = await axios.get(URL, {
      withCredentials: true,
    });

    const result = response.data.orderDetails;

    return {
      IsSuccess: true,
      result,
    };
  } catch (error) {
    return {
      IsSucess: false,
      error: error.result,
    };
  }
};

export const GetOrderRealTimeStatus = async (OrderId) => {
  const URL = `${baseURL}/order/status/${OrderId}`;

  try {
    const response = await axios.get(URL);

    const statusResult = response.data.orderStatus;

    console.log(statusResult);

    return {
      IsSuccess: true,
      result: statusResult,
    };
  } catch (error) {
    return {
      IsSuccess: false,
      error: error.response,
    };
  }
};

export const CancelOrderByCustomer = async (OrderId, CustomerId) => {
  const URL = `${baseURL}/order/cancel/${OrderId}?CustomerId=${CustomerId}`;

  try {
    const response = await axios.put(URL, null, {
      withCredentials: true,
    });

    const isDone = response.data.result;

    return {
      IsSuccess: isDone,
    };
  } catch (error) {
    return {
      IsSuccess: false,
      error: error.response,
    };
  }
};

export const HandleAddItemToCart = async (
  itemObj,
  customerId,
  cart,
  IsPharmacyChanged
) => {
  const response = await (!cart
    ? CreateCart(customerId, itemObj)
    : AddItemToCart(itemObj, customerId, cart.cartId));

  if (!response.IsSuccess) {
    alert("فشل اضافة المنج اعد المحاولة! ");
    return;
  }

  if (!cart || IsPharmacyChanged) {
    localStorage.setItem("cart", JSON.stringify(response.result));
  }

  return response.result.numberOfItems;
};

export const GetCustomerCartIfExistsAtLoggedIn = async (CustomerId) => {
  const URL = `${baseURL}/cart/loggedIn/${CustomerId}`;

  try {
    const response = await axios.get(URL, {
      withCredentials: true,
    });

    const result = response.data.cart;

    return {
      IsSuccess: true,
      result,
    };
  } catch (error) {
    return {
      IsSuccess: false,
      result: error.response,
    };
  }
};

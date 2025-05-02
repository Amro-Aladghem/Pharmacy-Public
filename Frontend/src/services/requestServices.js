import axios from "axios";

const baseURL = "https://localhost:7204/api/v1/request";

export const GetMostActiveMeetingRequest = async (CustomerId) => {
  const URL = baseURL + "/meet/active/" + CustomerId;

  try {
    const response = await axios.get(URL, {
      withCredentials: true,
      credentials: "include",
    });

    if (!response.data.request) {
      return {
        IsSuccess: true,
        result: null,
      };
    }

    const { id, pharmacyName, isNotAccepted, isTheSameDay } =
      response.data.request;

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

export const GetWhatsAppLink = async (CustomerId, PharmacyId) => {
  const URL = `${baseURL}/system/customer/${CustomerId}/pharmacy/${PharmacyId}/whatsapp/link`;

  try {
    const response = await axios.get(URL, {
      withCredentials: true,
      credentials: "include",
    });

    const result = response.data.result;

    return {
      IsSuccess: true,
      result,
    };
  } catch (error) {
    return {
      IsSuccess: false,
      ErrorCode: error.response.status,
    };
  }
};

export const GetMeetingReqHistoryForCustomer = async (CustomerId) => {
  const URL = `${baseURL}/meet/customer/${CustomerId}/history`;

  try {
    const response = await axios.get(URL, {
      withCredentials: true,
    });

    const result = response.data.requests;

    return {
      IsSuccess: true,
      result,
    };
  } catch (error) {
    return {
      IsSuccess: false,
      error: error,
    };
  }
};

export const GetMeetingRequestInfo = async (RequestId, CustomerId) => {
  const URL = `${baseURL}/meet/${RequestId}/info/${CustomerId}`;

  try {
    const response = await axios.get(URL, {
      withCredentials: true,
    });

    const result = response.data.requestInfo;

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

export const GetMostActiveRequestStatusForCustomer = async (CustomerId) => {
  const URL = `${baseURL}/meet/recent/status/${CustomerId}`;

  try {
    const response = await axios.get(URL, {
      withCredentials: true,
    });

    const result = response.data.status;

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

export const CancelRequestMeetingByCustomer = async (CustomerId, RequestId) => {
  const URL = `${baseURL}/meet/cancel`;

  const form = new FormData();

  form.append("Id", RequestId);
  form.append("CustomerId", CustomerId);

  try {
    const response = await axios.put(URL, form, {
      withCredentials: true,
      headers: {
        "Content-Type": "multipart/form-data",
      },
    });

    const result = response.data.result;

    return {
      IsSuccess: true,
    };
  } catch (error) {
    return {
      IsSuccess: false,
      error: error.response,
    };
  }
};

export const AddRefundRequestForTheOrder = async (Request) => {
  const URL = `${baseURL}/refund/order/add`;

  const form = new FormData();

  form.append("CustomerId", Request.CustomerId);
  form.append("TypeName", Request.TypeName);
  form.append("RefferenceId", Request.RefferenceId);
  form.append("AdditionalInformation", Request.AdditionalInformation);

  try {
    const response = await axios.post(URL, form, {
      withCredentials: true,
    });

    if (!response.data.result.isAccepted) {
      return {
        IsSuccess: false,
        error:
          "لا يمكنك استرداد اموال لهذا الطلب! لأن حالته منتهية ولم يتم الغاءها من اي من الطرفين",
      };
    }

    return {
      IsSuccess: true,
    };
  } catch (error) {
    return {
      IsSuccess: false,
      error: "حدث خطأ ما ",
    };
  }
};

export const AddRefundRequestForMeetingRequest = async (Request) => {
  const URL = `${baseURL}/refund/meeting/add`;

  const form = new FormData();

  form.append("CustomerId", Request.CustomerId);
  form.append("TypeName", Request.TypeName);
  form.append("RefferenceId", Request.RefferenceId);
  form.append("AdditionalInformation", Request.AdditionalInformation);

  try {
    const response = await axios.post(URL, form, {
      withCredentials: true,
    });

    if (!response.result.isAccepted) {
      return {
        IsSuccess: false,
        error:
          "لا يمكنك استرداد اموال لهذا الحجز! لأن حالته منتهية ولم يتم الغاءها من اي من الطرفين",
      };
    }

    return {
      IsSuccess: true,
    };
  } catch (error) {
    return {
      IsSuccess: false,
      error: "حدث خطأ ما ",
    };
  }
};

export const GetAllRegionsForTheGov = async (GovId) => {
  const URL = `${baseURL}/system/values/regions?GovernorateId=${GovId}`;

  try {
    const response = await axios.get(URL);

    const result = response.data.regions;

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

export const GetRefundRequestHistory = async (CustomerId) => {
  const URL = `${baseURL}/refund/history/${CustomerId}`;

  try {
    const response = await axios.get(URL, {
      withCredentials: true,
    });

    if (!response.data.requests) {
      return {
        IsSuccess: true,
        result: null,
      };
    }

    const result = response.data.requests;

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

import axios from "axios";

const baseURL = "https://localhost:7204/api/v1/phproducts";

export const GetProducts = async (Paginated) => {
  let URL =
    `${baseURL}?LastPhProductId=${Paginated.LastPhProductId}` +
    `&Limit=${Paginated.Limit}` +
    `&IsRowsCountCalculated=${Paginated.IsRowsCountCalculated}` +
    `&CategoryId=${Paginated.CategoryId}`;

  if (Paginated.GovernorateId) {
    URL += `&GovernorateId=${Paginated.GovernorateId}`;
  }

  if (Paginated.RegionId) {
    URL += `&RegionId=${Paginated.RegionId}`;
  }

  try {
    const response = await axios.get(URL);

    const { products, nextPage, total, lastPhProductId } =
      response.data.productsResult;

    const result = { products, nextPage, total, lastPhProductId };

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

export const GetProductFromSearchingByName = async (Paginated) => {
  let encodedSearchText = encodeURIComponent(Paginated.searchtext);

  const URL = `${baseURL}/search?searchtext=${encodedSearchText}&LastPhProductId=${Paginated.LastPhProductId}`;

  try {
    const response = await axios.get(URL);

    if (!response.data.productsResult) {
      return {
        IsSuccess: true,
        result: null,
      };
    }

    const { products, nextPage, total, lastPhProductId } =
      response.data.productsResult;

    const result = { products, nextPage, total, lastPhProductId };

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

export const GetPharmacyProducts = async (Paginated) => {
  let URL = `${baseURL}/list?pharmacyid=${Paginated.PharmacyId}&LastPhProductId=${Paginated.LastPhProductId}&Limit=${Paginated.Limit}&IsRowsCountCalculated=${Paginated.IsRowsCountCalculated}`;

  try {
    const response = await axios.get(URL);

    if (!response.data.products) {
      return {
        IsSuccess: true,
        result: null,
      };
    }

    const { products, nextPage, total, lastPhProductId } =
      response.data.products;

    const result = { products, nextPage, total, lastPhProductId };

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

export const GetProductFullInfo = async (ProductId, PharmacyId) => {
  const URL = `${baseURL}/info/${ProductId}?PharmacyId=${PharmacyId}`;

  try {
    const response = await axios.get(URL);

    const result = response.data;

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

export const GetProductDescription = async (ProductId) => {
  const URL = `${baseURL}/description/${ProductId}`;

  try {
    const response = await axios.get(URL);

    const result = response.data;

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

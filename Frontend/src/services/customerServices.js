import axios from "axios";

const baseURL = "https://localhost:7204/api/v1/Customer";

const registerObj = {
  PersonId: null,
  FirstName: null,
  LastName: null,
  Phone: null,
  latitude: null,
  longitude: null,
};

export const PreRegisterfunc = async (Email, Password) => {
  const formData = new FormData();

  formData.append("Email", Email);
  formData.append("Password", Password);

  try {
    const response = await axios.post(baseURL + "/PreRegister", formData, {
      headers: {
        "Content-Type": "multipart/form-data",
      },
      withCredentials: true,
    });

    return {
      Success: true,
      data: response.data,
    };
  } catch (error) {
    if (error) {
      return { Success: false, error: error.response.data };
    } else {
      return { Success: false, error: "خطأ في الخادم,الرجاء اعادة المحاولة" };
    }
  }
};

export const Login = async (Email, Password) => {
  const formData = new FormData();
  formData.append("Email", Email);
  formData.append("Password", Password);

  try {
    const response = await fetch(baseURL + "/Login", {
      method: "POST",
      body: formData,
      headers: {
        // Note: Content-Type is not needed for FormData as it is automatically set
      },
      credentials: "include",
    });

    if (!response.ok) {
      const errorData = await response.json();
      return { Success: false, error: errorData.message };
    }

    const data = await response.json();
    return { Success: true, data: data };
  } catch (error) {
    return { Success: false, error: "البيانات المدخلة خاطئة" };
  }
};

export const Registerfunc = async (registerObj) => {
  const formData = new FormData();

  formData.append("PersonRegister.PersonId", registerObj.PersonId);
  formData.append("PersonRegister.LastName", registerObj.LastName);
  formData.append("PersonRegister.Phone", registerObj.Phone);
  formData.append("PersonRegister.FirstName", registerObj.FirstName);
  formData.append("Latitude", registerObj.latitude);
  formData.append("Longitude", registerObj.longitude);
  formData.append("Image", null);

  try {
    const response = await axios.post(baseURL + "/Register", formData, {
      headers: {
        "Content-Type": "multipart/form-data",
      },
      withCredentials: true,
    });

    return { Success: true, data: response.data };
  } catch (error) {
    if (error) {
      return { Success: false, error: error.response.data };
    }

    return {
      Success: false,
      error: "فشل تسجيل الدخول ,الرجاء المحاولة مرة أخرى",
    };
  }
};

export const ChangeEmail = async (UserId, NewEmail) => {
  const formData = new FormData();

  formData.append("UserId", UserId);
  formData.append("NewEmail", NewEmail);

  try {
    const response = await axios.put(baseURL + "/ChangeEmail", formData, {
      headers: {
        "Content-Type": "multipart/form-data",
      },
      withCredentials: true,
    });

    return { Success: true, data: response.data };
  } catch (error) {
    if (error) {
      return { Success: false, error: error.response.data };
    }

    return {
      Success: false,
      error: "فشل تحديث الأيميل الرجاء المحاولة مرة أخرى",
    };
  }
};

export const UpdateInfo = async (UpdateInfoObj) => {
  const formData = new FormData();

  formData.append("Person.FirstName", UpdateInfoObj.firstName);
  formData.append("Person.LastName", UpdateInfoObj.lastName);
  formData.append("Person.Phone", UpdateInfoObj.phone);
  formData.append("CustomerId", UpdateInfoObj.customerId);

  try {
    const response = await axios.put(baseURL + "/Update", formData, {
      headers: {
        "Content-Type": "multipart/form-data",
      },
      withCredentials: true,
    });

    return { Success: true, data: response.data };
  } catch (error) {
    if (error) {
      return { Success: false, error: error.response.data };
    }

    return {
      Success: false,
      error: "فشل تحديث الأيميل الرجاء المحاولة مرة أخرى",
    };
  }
};

export const ShowInfo = async (CustomerId) => {
  try {
    const response = await axios.get(baseURL + "/Show/" + CustomerId, {
      withCredentials: true,
    });

    const { firstName, lastName, phone, email } = response.data.customer.person;

    const result = { firstName, lastName, phone, email };

    return {
      IsSuccess: true,
      result,
    };
  } catch (error) {
    if (error) {
      return { IsSuccess: false, error: error.response };
    }

    return {
      IsSuccess: false,
      error: "فشل تحميل البيانات ,الرجاء اعادة تحميل الصفحة",
    };
  }
};

export const IsCustomerLoggedIn = async (CustomerId) => {
  const URL = `${baseURL}/logged/status/${CustomerId}`;

  try {
    const response = await axios.get(URL, {
      withCredentials: true,
    });

    const isLogged = response.data.isLogged;

    return {
      IsSuccess: true,
      isLogged,
    };
  } catch (error) {
    return {
      IsSuccess: false,
      isLogged: false,
    };
  }
};

export const EditLocation = async (CustomerId, LocationDTO) => {
  const URL = `${baseURL}/${CustomerId}/location/edit`;

  try {
    const response = await axios.put(
      URL,
      {
        Latitude: LocationDTO.Latitude,
        Longitude: LocationDTO.Longitude,
      },
      {
        headers: {
          "Content-Type": "application/json",
        },
        withCredentials: true,
      }
    );

    const isDone = response.data.isDone;

    return {
      IsSuccess: true,
      isDone,
    };
  } catch (error) {
    return {
      IsSuccess: false,
      error: error.response,
    };
  }
};

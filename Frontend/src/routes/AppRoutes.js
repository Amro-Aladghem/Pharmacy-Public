import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import SignInPage from "../views/Customer/SignInPage/SignInPage";
import { GoogleOAuthProvider } from "@react-oauth/google";
import PreRegister from "../views/Customer/PreRegisterPage/PreRegister";
import Register from "../views/Customer/RegisterPage/Register";
import SubNavbar from "../components/SubNavbar/SubNavbar";
import Main from "../views/Customer/MainPage/Main";
import ProductsPage from "../views/Customer/ProductsPage/ProductsPage";
import Navbar from "../components/Navbar/Navbar";
import TapsBar from "../components/TapsBar/TapsBar";
import SearchProduct from "../views/Customer/SearchProductPage/SearchProduct";
import AccountPage from "../views/Customer/AccountPage/AccountPage";
import InfoData from "../views/Customer/InfoDataPage/InfoData";
import GovernorateTaps from "../components/TapsBar/GovernorateTaps";
import Pharmacies from "../views/Customer/PharmaciesPage/Pharmacies";
import NearestPh from "../views/Customer/NearestPhPage/NearestPh";
import PharmacyProfile from "../views/Customer/PharmacyProfilePage/PharmacyProfile";
import OrderHistory from "../views/Customer/OrdersHistoryPage/OrderHistory";
import ProductPage from "../views/Customer/ProductPage/ProductPage";
import Cart from "../views/Customer/CartPage/Cart";
import RequestMeeting from "../views/Customer/RequestMeetingPage/RequestMeeting";
import CheckOut from "../views/Customer/CheckOutPage/CheckOut";
import MeetingHistory from "../views/Customer/MeetingHistoryPage/MeetingHistory";
import PharmaciesMeetingPage from "../views/Customer/PharmaciesMeetingPage/PharmaciesMeetingPage";
import OrderDetails from "../views/Customer/OrderDetailsPage/OrderDetails";
import AcitveMeetingDetails from "../views/Customer/ActiveMeetingDetailsPage/ActiveMeetingDetails";
import CheckDelivery from "../views/Customer/CheckDeliveryPage/CheckDelivery";
import RefundRequest from "../views/Customer/RefundRequestPage/RefundRequest";
import RefundRequestsHistory from "../views/Customer/RefundRequestsHistoryPage/RefundRequestsHistory";
import ChangeLocation from "../views/Customer/ChangeLocationPage/ChangeLocation";

export function AppRoutes() {
  return (
    <Routes>
      <Route element={<SubNavbar />}>
        <Route path="/signIn" element={<SignInPage />} />
        <Route
          path="/Preregister"
          element={
            <GoogleOAuthProvider clientId="1032788514382-79b6badui5m7vd7c44dbij0njctpkjfv.apps.googleusercontent.com">
              <PreRegister />
            </GoogleOAuthProvider>
          }
        ></Route>
        <Route path="/register" element={<Register />} />
      </Route>

      <Route element={<Navbar />}>
        <Route path="/" element={<Main />} />
        <Route path="/customer/profile" element={<AccountPage />} />
        <Route path="/search" element={<SearchProduct />} />
        <Route path="/customer/profile/info" element={<InfoData />} />
        <Route path="/pharmacies/nearest" element={<NearestPh />} />
        <Route path="/pharmacies/:pharmacyId" element={<PharmacyProfile />} />
        <Route path="/orders/history" element={<OrderHistory />} />
        <Route path="/products/:productId" element={<ProductPage />} />
        <Route path="/cart" element={<Cart />} />
        <Route
          path="/pharmacies/:pharmacyId/request/meeting/info"
          element={<RequestMeeting />}
        />
        <Route path="/cart/checkout" element={<CheckOut />} />
        <Route path="/customer/meeting/history" element={<MeetingHistory />} />
        <Route
          path="/pharmacies/have-meeting"
          element={<PharmaciesMeetingPage />}
        />
        <Route path="/order/:orderId/details" element={<OrderDetails />} />
        <Route
          path="/meeting-request/:requestId/active/request/details"
          element={<AcitveMeetingDetails />}
        />
        <Route path="/cart/check-delivery" element={<CheckDelivery />} />
        <Route path="/refund-request/:type" element={<RefundRequest />} />
        <Route
          path="/refund-request/history"
          element={<RefundRequestsHistory />}
        />
        <Route path="/customer/location/edit" element={<ChangeLocation />} />

        <Route element={<GovernorateTaps />}>
          <Route path="/pharmacies" element={<Pharmacies />} />
        </Route>

        <Route element={<TapsBar />}>
          <Route path="/products" element={<ProductsPage />} />
        </Route>
      </Route>
    </Routes>
  );
}

export default AppRoutes;

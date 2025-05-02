import PaymentSummery from "../CartPage/PaymentSummery";
import { useState,useEffect } from "react";
import { CircularProgress, colors } from "@mui/material";
import { GetCartInfoAtCheckOut } from "../../../services/orderServices";
import { useAuth } from "../../../contexts/AuthContext";

export function PaymentSummerySection()
{
    const [result,setResult] = useState(true);
    const [loading,setLoading] = useState(true);

    const {authInfo} = useAuth();

    const [cartInfo,setCartInfo] = useState({
        cartId:-1,
        totalPrice :0,
        deliveryFees :0,
        subPrice :0,
        serviceFees:0 
    });

    async function fetchData(CustomerId,CartId)
    {
        
        const response = await GetCartInfoAtCheckOut(CustomerId,CartId);

        setLoading(false);

        if(!response.IsSuccess)
        {
            setResult(false);
            return;
        }

        setCartInfo(response.cartInfo);
    }

    useEffect(()=>{
        
        const cartId = JSON.parse(localStorage.getItem('cart')).cartId;

        fetchData(authInfo.customerId,cartId);
    },[]);

    return (
        <>
            {
                loading?(
                    <CircularProgress size={40} sx={{color:'rgb(255, 86, 114)'}} />
                )
                :!result?(
                    <p>فشل تحميل معلومات الدفع , أعد تحميل الصفحة</p>
                )
                :(
                    <PaymentSummery
                        subFees={cartInfo.subPrice} 
                        totalFees={cartInfo.totalPrice} 
                        serviceFees={cartInfo.serviceFees}
                        deliveryFees={cartInfo.deliveryFees}  
                    />
                )
            }
        </>
        
    );
}

export default PaymentSummerySection;
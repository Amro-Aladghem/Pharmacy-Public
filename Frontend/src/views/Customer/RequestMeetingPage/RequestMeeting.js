import { Box } from '@mui/material';
import './RequestMeeting.css';
import { useState,useEffect } from 'react';
import Typography from '@mui/joy/Typography';
import VideoCameraFrontIcon from '@mui/icons-material/VideoCameraFront';
import { GetMeetingServiceInfo } from '../../../services/pharmacyServices';
import { useParams,useNavigate } from 'react-router-dom';
import {CircularProgress} from '@mui/material';
import { GetWhatsAppLink } from '../../../services/requestServices';
import { useAuth } from '../../../contexts/AuthContext';


export function RequestMeeting()
{
    const {pharmacyId} = useParams();
    const navigate = useNavigate();
    const {authInfo} = useAuth();

    const [result,setResult] = useState(true);
    const [loading,setLoading] = useState(false);
    const [info,setInfo] = useState({pharmacyId:0,name:"",image:"",price:"",isHasVedioCall:false});
    const [payLoading,setPayLoading] = useState(false);
    

    async function fetchData()
    {
       setLoading(true);

       const response = await GetMeetingServiceInfo(pharmacyId);

       setLoading(false);
 
       if(!response.IsSuccess)
       {
        setResult(false);
        return;
       }

       setInfo(response.result);
    }

    async function handelButtonClick()
    {
        if(!authInfo.loggedIn)
        {
            navigate("/signin");
            return;
        }

        setPayLoading(true);

        const customerId = JSON.parse(sessionStorage.getItem('customer')).customerId;
        const response = await GetWhatsAppLink(customerId,pharmacyId);

        setPayLoading(false);

        if(!response.IsSuccess){
             
           const message= response.ErrorCode==409?"لديك حجز فعال وغير منتهي , لا يمكن حجز أخر قبل انتهائه!"
                                                 :"حدث خطأ ما الرجاء المحاولة";
            
            alert(message);
            return;
        }

        window.open(response.result.url,'_blank');
    }

    useEffect(()=>{

        fetchData();

    },[authInfo]);
 

    return(
        <>
          <Box sx={{
            width:"100%",
            display:'flex',
            justifyContent:'center',
            alignItems:'center',
            direction:'rtl',
            flexDirection:'column'
          }}>
            
            {
              loading?(
                <CircularProgress size={40} />
              )
              :!result?(
                <p>فشل تحميل البيانات الرجاء اعادة  تحميل الصفحة</p>
              )
              :!info.isHasVedioCall?(
                <>
                    <p>لا توفر هذه الصيدلية  الأستشارة الهاتفية, جرب صيدلية اخرى</p>
                    <img src="/NoMeetingService.png" width={"250px"} height={"auto"} alt="" />
                </>
              )
              :(
                <>
                <Box sx={{
                    backgroundColor:"rgba(156, 153, 153, 0.353)",
                    borderRadius:'8px',
                    display:'flex',
                    flexDirection:'column',
                    justifyContent:'center',
                    alignItems:'center',
                    padding:'10px 10px',
                    gap:'20px',
                    width:{md:'370px',xs:'350px'},
                    marginTop:'10px'
                }}>
                
                <VideoCameraFrontIcon sx={{fontSize:'50px',color:'rgb(255, 86, 114)'}} />
                <Typography level='h1' sx={{
                    backgroundColor:"rgb(255, 86, 114)",
                    borderRadius:"5px",
                    padding:'5px 5px'
                }}
                >
                    خدمة الأستشارة الهاتفية
                </Typography>
                
                <Box sx={{
                    display:'flex',
                    gap:'20px',
                    justifyContent:'start',
                    alignItems:'center'
                }}>

                    <img src={info.image} width={"90px"} height={"auto"} style={{borderRadius:'8px'}}  alt='pharmacy-img'/>
                    <Typography level='h3'>صيدلية {info.name}</Typography>
                </Box>

                <Box sx={{
                    display:'flex',
                    justifyContent:'start',
                    width:'100%'
                }}>
                    
                    <Typography level="h3">
                        سعر الحجز:
                    </Typography>
                    <Typography level='h3' sx={{
                            backgroundColor:'rgb(255, 86, 114)',
                            marginRight:'auto',
                            padding:'3px 3px'
                        }}>
                            jd {info.price}
                    </Typography>

                </Box>

                <Typography level='h3' sx={{
                    backgroundColor:'aqua',
                    borderRadius:'5px',
                    padding:'3px 3px'
                }}
                >
                    طرق الدفع المتاحة
                </Typography>

                <Box sx={{
                    display:'flex',
                    flexDirection:'column',
                    justifyContent:'start',
                }}>
                    <Box sx={{
                        display:'flex',
                        justifyContent:'start',
                        gap:'100px',
                        width:'100%'
                    }}>
                        <label htmlFor='click-pay'  style={{
                            fontWeight:'bold',
                        }}
                        >خدمة كليك أو محفظ الكترونية
                        </label>
                        <input type="radio" readOnly id='click-pay' checked={true} />
                    </Box>
                </Box> 

                <Typography level='body-sm'
                >
                سوف يتم نقلك الى واتس اب لأستكمال عملية الحجز مع موظف خدمة العملاء للمزيد من الأمان وتجنب الأخطاء في ارسال المال
                </Typography>
                
                <button 
                    className='main-btn' 
                    disabled={payLoading}
                    style={{width:"fit-content",marginTop:'10px',flexShrink:'0'}}
                    onClick={()=>{handelButtonClick()}}
                    
                >
                    {!authInfo.loggedIn?"تسجيل الدخول للحجز":!payLoading?"استكمال عملية الدفع":"...الرجاء الأنتظار"}             
                    <div className="arrow-wrapper" >
                        <div className="arrow"></div>
        
                        </div>
                </button>


                </Box>
                <Typography level='title-sm' sx={{
                    marginTop:'10px'
                }}>ملاحظة*:
                     سوف تتمكن من استرداد المبلغ المدفوع  قبل ان تقبل الصيدلية طلب الحجز لديك 
                    , بمجرد قبولها لن تتمكن من الأسترجاع
                </Typography>

                </>
              )

            }
          </Box>
        
        </>
    );
}

export default RequestMeeting;
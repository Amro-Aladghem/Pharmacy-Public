import './Cart.css';
import { Box } from '@mui/material';
import Typography from '@mui/joy/Typography';

export function PaymentSummery({serviceFees,subFees,deliveryFees,totalFees})
{
    return(
      
        <>
            <Box sx={{
                backgroundColor:'rgba(119, 114, 114, 0.125)',
                width:'300px',
                borderRadius:'8px',
                padding:'5px 5px',
                height:'fit-content',
                marginTop:'10px'
            }}>

                <Box sx={{display:'flex',justifyContent:'center'}}>
                    <h2>تفاصيل رسوم الطلب</h2>   
                </Box> 

                <Box sx={{
                    display:'flex',
                    flexDirection:'column',
                    gap:'10px',
                    justifyContent:'start'
                }}>
                
                <Box sx={{display:'flex'}}>
                    <Typography level="title-lg">
                        سعر المنتجات:
                    </Typography>
                    <Typography level='body-lg' sx={{
                            backgroundColor:'aqua',
                            marginRight:"auto",
                            borderRadius:'10px',
                            padding:'6px 6px'
                        }}>
                        {subFees} jd
                    </Typography>
                </Box>

                <Box sx={{display:'flex',}}>
                    <Typography level="title-lg">
                        سعر التوصيل:
                    </Typography>
                    <Typography level='body-lg' sx={{
                            backgroundColor:'aqua',
                            marginRight:"auto",
                            borderRadius:'10px',
                            padding:'6px 6px'
                        }}>
                        {deliveryFees} jd
                    </Typography>
                </Box>

                <Box sx={{display:'flex'}}>
                    <Typography level="title-lg">
                        سعر الخدمة:
                    </Typography>
                    <Typography level='body-lg' sx={{
                            backgroundColor:'aqua',
                            marginRight:"auto",
                            borderRadius:'10px',
                            padding:'6px 6px'
                        }}>
                        {serviceFees} jd
                    </Typography>
                </Box>

                <Box sx={{display:'flex'}}>
                    <Typography level="title-lg">
                        سعر الكلي:
                    </Typography>
                    <Typography level='body-lg' sx={{
                            backgroundColor:'rgb(255, 86, 114)',
                            marginRight:"auto",
                            borderRadius:'10px',
                            padding:'6px 6px'
                        }}>
                        {totalFees} jd
                    </Typography>
                </Box>
                
                
                </Box>

            </Box>

        </>

    );
}



export default PaymentSummery;


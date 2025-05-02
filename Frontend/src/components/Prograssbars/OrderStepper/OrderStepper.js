import './OrderStepper.css';
import Box from '@mui/material/Box';
import Typography from '@mui/joy/Typography';
import { Stepper, Step, StepLabel } from "@mui/material";

const steps = ['تم الأرسال بنجاح', 'انتظار', 'يتم تجهيز الطلب','توصيل','تم الأستلام'];

const StatusIdToStepsIndex = {
    1:0,
    2:1,
    3:2,
    4:3,
    7:4
}

export function OrderStepper({statusId})
{

    return (
        <>
          <Box sx={{
            width:'100%',
            display:'flex',
            justifyContent:'center',
            alignItems:'center',
            marginTop:'25px'
          }}>
            <Box sx={{
                width:'90%',
                padding:'15px 15px',
                boxShadow:'2px 3px 0.5px black',
                borderRadius:'8px',
                border:'1px solid black'
            }}>
                    <Typography level='h4' sx={{direction:'rtl'}}>
                        حالة الطلب
                    </Typography>

                    <Box sx={{
                        display:'flex',
                        justifyContent:'center',
                        width:'100%'
                    }}>
                        <Box sx={{ width: '100%',display:{xs:'none',md:'block'} }}>
                            <Stepper nonLinear activeStep={StatusIdToStepsIndex[statusId]} alternativeLabel >
                                {steps.map((label, index) => (
                                <Step key={label} completed={index <= StatusIdToStepsIndex[statusId]} >
                                    <StepLabel
                                        sx={{
                                            '& .MuiStepIcon-root': {
                                                color: 'silver',
                                                border:'2px solid black', 
                                                borderRadius:'50%',
                                            },
                                            '& .Mui-completed .MuiStepIcon-root': {
                                                color: 'aqua',
                                                border:'2px solid rgb(1, 167, 167)', 
                                                borderRadius:'50%',
                                                fontWeight: 'bold'
                                            },
                                            '& .Mui-active .MuiStepIcon-root': {
                                                color: 'rgb(255, 86, 114)',
                                                border:'2px solid black', 
                                            },
                                            '& .MuiStepLabel-label': {
                                                fontWeight: 'bold',
                                                fontSize: '20px',
                                            },
                                        }}
                                        >
                                        {label}
                                    </StepLabel>
                                </Step>
                                ))}
                            </Stepper>
                        </Box>

                        <Box sx={{ maxWidth: 400 , display:{xs:'block',md:'none'},direction:'rtl' }}>
                            <Stepper activeStep={StatusIdToStepsIndex[statusId]} orientation="vertical">
                                {steps.map((label, index) => (
                                <Step key={label} completed={index <= StatusIdToStepsIndex[statusId]}>
                                    <StepLabel
                                        sx={{
                                            '& .MuiStepIcon-root': {
                                                color: 'silver',
                                                fontWeight: 'bold',
                                                border:'2px solid black', 
                                                borderRadius:'50%',
                                            },
                                            '& .Mui-completed .MuiStepIcon-root': {
                                                color: 'aqua',
                                                border:'2px solid rgb(1, 167, 167)', 
                                                borderRadius:'50%',
                                                fontWeight: 'bold'
                                            },
                                            '& .Mui-active .MuiStepIcon-root': {
                                                color: 'rgb(255, 86, 114)',
                                                border:'2px solid black', 
                                            },
                                            '& .MuiStepLabel-label': {
                                                fontWeight: 'bolder',
                                                fontSize: '20px',
                                                marginRight:'100px'
                                            },
                                        }}
                                        >
                                        {label}
                                    </StepLabel>
                                </Step>
                                ))}
                            </Stepper>
                        </Box>
                    </Box>
            </Box> 
          </Box> 

        </>    

    );
}


export default OrderStepper;
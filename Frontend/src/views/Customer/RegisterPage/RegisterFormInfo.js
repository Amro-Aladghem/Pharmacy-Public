import { TextField } from "@mui/material";
import './RegisterFormInfo.css';
import ContactEmergencyIcon from '@mui/icons-material/ContactEmergency';
import {InputAdornment} from "@mui/material";
import Button from "@mui/material";

export function RegisterFormInfo({Info,handlers})
{

    function handelFirstNameChange(value)
    {
       handlers.setInfo({...Info,FirstName:value});
    }   

    function handelLastNameChange(value)
    {
        handlers.setInfo({...Info,LastName:value});
    }

    function handelPhoneChange(value)
    {
        handlers.setInfo({...Info,Phone:value})
    }
              


   return (

    <div id="sign-up-form-register">
        <h1> أخر خطوة معلومات عنك</h1>
        <ContactEmergencyIcon sx={{color:"rgb(5, 197, 197)"}} fontSize="large" />
        <form onSubmit={(event)=>{
            event.preventDefault();
        }}>
            <TextField 
                value={Info.FirstName}
                id="FirstName-input"
                label="اسمك الأول" 
                type="text" 
                variant="outlined" 
                fullWidth 
                size="small"
                margin='dense'
                required
                onChange={(event)=>{
                    handelFirstNameChange(event.target.value);
                }}
            />
            <TextField 
                value={Info.LastName}
                id="LastName-input"
                label="اسم العائلة" 
                type="text" 
                variant="outlined" 
                fullWidth 
                size="small"
                margin='dense'
                required
                onChange={(event)=>{
                    handelLastNameChange(event.target.value);
                }}
            />
            <TextField 
                value={Info.Phone}
                
                id="Phone-input"
                label="رقم هاتفك للتواصل" 
                type="text" 
                variant="outlined" 
                fullWidth 
                size="small"
                margin='dense'
                required
                slotProps={{
                    input: {
                      startAdornment: <InputAdornment position="start">0</InputAdornment>,
                      pattern: "[1-9]*",  
                      inputMode: "numeric",
                    }}
                }
                onChange={(event)=>{
                    handelPhoneChange(event.target.value);
                }}
            />
            <TextField 
                value={Info.Email}
                id="Eamil-Input"
                label="Email" 
                type="Email" 
                variant="outlined" 
                fullWidth 
                size="small"
                margin='dense'
                required
                disabled={true}
            />
        </form>
    </div>
   



   )



}

export default RegisterFormInfo;
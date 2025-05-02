import './InfoTextBars.css';
import TextField from '@mui/material/TextField';
import Grid from '@mui/material/Grid2';
import { InputAdornment } from '@mui/material';


export function InfoTextBars({info,updateInfo,isEdit,updateInfoChanged})
{
  
    function handelFirstNameChange(value)
    {
        updateInfo((prev)=>{
          return{...info,firstName:value}
        });

        updateInfoChanged(true);
    }

    function handelLastNameChange(value)
    {
        updateInfo((prev)=>{
            return{...info,lastName:value}
          });
        
        updateInfoChanged(true);
    }

    function handelPhoneChange(value)
    {
        updateInfo((prev)=>{
            return{...info,phone:value}
          });

        updateInfoChanged(true);  
    }
 

   return (
      
        <Grid container sx={{direction:'rtl'}} >
            
            <Grid size={{xs:12,md:6}} sx={{display:'flex',justifyContent:"center"}}>
                <TextField
                    value={info.firstName}
                    type='text'
                    disabled={!isEdit}
                    className='input-field'
                    label='اسمك الأول'   
                    variant="filled" 
                    size="small"
                    margin='dense'
                    onChange={(event)=>{
                        handelFirstNameChange(event.target.value);
                    }}

                />
            </Grid>

            <Grid size={{xs:12,md:6}} sx={{display:'flex',justifyContent:"center"}}>
                <TextField
                    value={info.lastName}
                    type='text'
                    disabled={!isEdit}
                    className='input-field'
                    label='العائلة'   
                    variant="filled" 
                    size="small"
                    margin='dense'
                    onChange={(event)=>{
                        handelLastNameChange(event.target.value);
                    }}

                />
            </Grid>
            
            <Grid size={{xs:12,md:6}} sx={{display:'flex',justifyContent:"center",alignItems:'center'}}>
                <TextField 
                    value={info.phone}
                    disabled={!isEdit}
                    className='input-field'
                    label="رقم هاتفك " 
                    type="phone" 
                    variant="filled" 
                    size="small"
                    margin='dense'
                    dir='ltr'
                    slotProps={{
                        input: {
                        startAdornment: <InputAdornment position="start">0</InputAdornment>,
                        }}
                    }
                    onChange={(event)=>{
                        handelPhoneChange(event.target.value);
                    }}
                    onBeforeInput={(e) => {
                        if (!/^\d$/.test(e.data)) {
                          e.preventDefault();
                        }
                      }}
                />
            </Grid>

            <Grid size={{xs:12,md:6}} sx={{display:'flex',justifyContent:"center"}}>
                <TextField
                    value={info.email}
                    type='email'
                    className='input-field'
                    label='email'   
                    variant="filled" 
                    slotProps={{
                        input: {
                        readOnly: true,
                        },
                    }}
                    size="small"
                    margin='dense'
                />
            </Grid>
            
            <Grid size={{xs:12,md:6}} sx={{display:'flex',justifyContent:"center"}}>
                <TextField
                    type='password'
                    className='input-field'
                    label='password'   
                    variant="filled" 
                    slotProps={{
                        input: {
                        readOnly: true,
                        },
                    }}
                    value={'fakepassword'}
                    size="small"
                    margin='dense'
                />
            </Grid>


        </Grid>


   );

}



export default InfoTextBars;
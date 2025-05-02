import { Box } from "@mui/material";
import Typography from "@mui/joy/Typography";
import Chip from '@mui/material/Chip';

export function MeetingRequestInfo({requestId,date,status})
{
    return (
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
                border:'1px solid black',
                direction:'rtl'
            }}
            >
                <Typography level='h4'>
                  معلومات الحجز
                </Typography>

                <Box sx={{
                    display:'flex',
                    justifyContent:'space-between',
                    alignItems:{xs:'normal',md:'center'},
                    marginTop:'5px',
                    flexDirection:{xs:'column',md:'row'}
                }}>
                    <Box sx={{display:{xs:'flex',md:'block'},marginTop:'4px'}}>
                        <Typography level='body-md' >
                           رقم الحجز:
                        </Typography>
                        <Typography level='body-lg'>
                          #{requestId}
                        </Typography>
                    </Box>
                    <Box sx={{display:{xs:'flex',md:'block'},marginTop:'4px'}}>
                        <Typography level='body-md'>
                           تاريخ الحجز:
                        </Typography>
                        <Typography level='body-lg'>
                          {date}
                        </Typography>
                    </Box>
                    <Box sx={{display:{xs:'flex',md:'block'},marginTop:'4px'}}>
                        <Typography level='body-md'>
                           حالة الحجز:
                        </Typography>
                        <Chip color="primary" label={status}/>
                    </Box>
                </Box>
            </Box>
        </Box>
    );
}

export default MeetingRequestInfo;
import { Box } from "@mui/material";
import Typography from "@mui/joy/Typography";



export function MeetingDetailsPrice({price})
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
                    <Box sx={{
                        display:'flex',
                        marginTop:'8px'
                    }}>
                        <Typography level="title-lg">
                            سعر الحجز:
                        </Typography>
                        <Typography level="title-lg" sx={{marginRight:'auto',backgroundColor:'aqua'}}>
                            {price} jd
                        </Typography>
                    </Box>
                </Box>
            </Box>        
    );
}


export default MeetingDetailsPrice;
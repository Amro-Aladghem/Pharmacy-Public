import { Box } from "@mui/material";
import Typography from "@mui/joy/Typography";



export function PharmacyInfo({pharmacyId,pharmacyName})
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
                <Typography level="h4">
                   معلومات الصيدلية
                </Typography>

                <Box sx={{
                    display:'flex',
                    justifyContent:'space-between',
                    alignItems:{xs:'normal',md:'center'},
                    marginTop:'5px',
                    flexDirection:{xs:'column',md:'row'}
                }}>
                    <Box sx={{display:{xs:'flex',md:'block'},marginTop:'4px',gap:'3px'}}>
                        <Typography level='body-md' >
                             اسم الصيدلية:
                        </Typography>
                        <Typography level='body-lg'>
                            {pharmacyName}
                        </Typography>
                    </Box>
                    <Box sx={{display:{xs:'flex',md:'block'},marginTop:'4px',gap:'3px'}}>
                        <Typography level='body-md'>
                            رمز الصيدلية:
                        </Typography>
                        <Typography level='body-lg'>
                            {pharmacyId}
                        </Typography>
                    </Box>
                </Box>
            </Box>  

        </Box>
    );
}

export default PharmacyInfo;
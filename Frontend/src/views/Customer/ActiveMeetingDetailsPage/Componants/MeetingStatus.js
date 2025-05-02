import { Box } from "@mui/material";
import Typography from "@mui/joy/Typography";
import Button from "@mui/joy/Button";


export function MeetingStatus({statusId,meetingURL})
{
    function handleJoinMeetingClick()
    {
       if(statusId!==3)
        return;

       window.open(meetingURL,'_blanck');
    }

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

                    <Typography level="h3">
                        حالة الحجز
                    </Typography>

                    <Box sx={{
                        display:'flex',
                        justifyContent:'center',
                        width:'100%',
                        alignItems:'center'
                    }}>
                        <Button sx={{
                            backgroundColor:"rgb(255, 86, 114)",
                            padding:'12px 8px',
                            fontSize:'16px',
                            color:'white',
                            borderRadius:'13px',
                            ":hover":{
                                backgroundColor:'rgb(255, 86, 114)'
                            },
                            ":disabled":{
                              backgroundColor:'gray',
                              cursor:'not-allowed'
                            }
                            }}
                            disabled={statusId!==3}
                            onClick={()=>handleJoinMeetingClick()}
                            >

                            دخول المكالمة
                        </Button>
                    </Box>

                    <Typography level="title-sm" sx={{marginTop:'5px'}}  >
                        {statusId!==3?"*أنت الأن في قائمة الأنتظار , سيتم تفعيل زر الدخول بمجرد قبول الصيدلية الطلب":
                        "يمكنك الدخول الأن تفضل"
                        }
                    </Typography>
                    
                </Box>
            </Box>
    );
}


export default MeetingStatus;
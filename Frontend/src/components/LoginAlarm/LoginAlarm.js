import { useNavigate } from "react-router-dom";


export function LoginAlarm()
{
    const navigate = useNavigate();

    return(
        <>
            <p>يجب عليك تسجيل الدخول أولا</p>

            <button   className='main-btn' style={{width:"fit-content"}}
            onClick={()=>{navigate('/signin')}}
            >
                تسجيل الدخول             
                <div className="arrow-wrapper" >
                <div className="arrow"></div>
                
                </div>
            </button>
        </>       
    );
}


export default LoginAlarm;
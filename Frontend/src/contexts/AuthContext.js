import { createContext, useState , useEffect ,useContext } from "react";

const AuthContext = createContext(null);


export const AuthProvider = ({children})=>{

    const [authInfo,setAuthInfo] = useState({loggedIn:false,customerId:null,userName:null,profileImageLink:''});

    const AddTimeOutForTokenValidateTime = (expiredTokenTime)=>
    {
        expiredTokenTime = new Date(expiredTokenTime);      
        const now = Date.now(); 

        const remainingTime = expiredTokenTime-now;

        setTimeout(()=>{

          setAuthInfo({...authInfo,loggedIn:false,customerId:null,userName:null,profileImageLink:""});
          sessionStorage.removeItem('customer');
          sessionStorage.removeItem('expiredTokenTime');
        },remainingTime);
    }

    const handleAuthChange = (loggedIn,customerId,userName,profileImageLink,expiredTokenTime)=>{

        setAuthInfo({...authInfo,
                        loggedIn:loggedIn,
                        customerId:customerId,
                        userName:userName,
                        profileImageLink:profileImageLink
                    });

        AddTimeOutForTokenValidateTime(expiredTokenTime);            
    }

    useEffect(()=>{
       
      const customer = sessionStorage.getItem('customer');

      if(!customer)
      {
        setAuthInfo({...authInfo,loggedIn:false,customerId:null,userName:null,profileImageLink:""});
        return;
      }

      const customerAfterPars = JSON.parse(customer);

      setAuthInfo({...authInfo,loggedIn:true,
                            customerId:customerAfterPars.customerId,
                            userName:customerAfterPars.person.userName,
                            profileImageLink:customerAfterPars.person.profileImageLink
                  });
      
      const expiredTokenTime = JSON.parse(sessionStorage.getItem("expiredTokenTime"));
      
      AddTimeOutForTokenValidateTime(expiredTokenTime);

    },[]);

    return (
      <AuthContext.Provider value={{authInfo,handleAuthChange}}>
        {children}
      </AuthContext.Provider>
    );
}

export const useAuth = ()=>{

    const Auth = useContext(AuthContext);
    return Auth;
}
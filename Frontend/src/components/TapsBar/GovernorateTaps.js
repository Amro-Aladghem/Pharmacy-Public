import Tabs from '@mui/material/Tabs';
import Tab,{ tabsClasses } from '@mui/material/Tab';
import Box from '@mui/material/Box';
import { useState , useEffect } from 'react';
import { Outlet,useLocation } from 'react-router-dom';
import { useNavigate } from 'react-router-dom';


const tabsData = [
    { id: 1, name: "عمان" },
    { id: 2, name: "الزرقاء" },
    { id: 3, name: "إربد" },
    { id: 4, name: "عجلون" },
    { id: 5, name: "جرش" },
    { id: 6, name: "المفرق" },
    { id: 7, name: "البلقاء" },
    { id: 8, name: "مادبا" },
    { id: 9, name: "الكرك" },
    { id: 10, name: "الطفيلة" },
    { id: 11, name: "معان" },
    { id: 12, name: "العقبة" }
]



export function GovernorateTaps()
{
    const location = useLocation();
    const navigate = useNavigate();
    
    const [value, setValue] = useState(1);
    const [isFirstTime,setIsFirstTime]= useState(true);
    
    const handleChange = (event,newValue) => {
           
        navigate('/pharmacies?Governorate='+newValue);
        setValue(newValue);
    };

    useEffect(()=>{
    
            const params = new URLSearchParams(location.search);
    
            let GovernorateId =Number(params.get("Governorate")); 

            GovernorateId = GovernorateId || 1;
    
            setValue(GovernorateId);
            setIsFirstTime(false);
    },[]);
        
     
 
    return (
        <>
          <Box sx={{ bgcolor: 'background.paper',direction:'rtl' }}> 
                     <Tabs
                     value={value}
                     onChange={handleChange}
                     variant="scrollable"
                     scrollButtons="on"
                    
                     aria-label=""
                     sx={{
                       
                       "& .MuiTab-root": { color: "black" }, 
                       "& .MuiTab-root.Mui-selected": { color: "rgb(255, 86, 114)" },
                     }}
                     TabIndicatorProps={{
                       style: { backgroundColor: "rgb(255, 86, 114)", height: "2px" } 
                     }}
                     >
                        {tabsData.map((tap,index)=>{
         
                          return <Tab key={index} value={tap.id} label={tap.name}/>
                        })
                        }
                     </Tabs>
                    </Box>
         
            <Outlet />
        </>

    );
}


export default GovernorateTaps;
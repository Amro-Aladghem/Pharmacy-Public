import PharmacyMeetingCard from "../../../components/Card/PharmacyMeetingCard/PharmacyMeetingCard";
import Grid from '@mui/material/Grid2';
import { useState,useEffect, } from "react";
import { GetPharmaciesThatHaveMeetingService } from "../../../services/pharmacyServices";
import InfiniteScroll from "react-infinite-scroll-component";
import { CircularProgress } from "@mui/material";


export function PharmaciesMeetingPage()
{
   
   const [loading,setLoading] = useState(true);
   const [result,setResult] =useState(true);
   const [pharmacies,setPharmacies] = useState([]);
   const [paginated,setPaginated] =useState({LastPharmacyId:0,Limit:9,IsRowsCountCalculated:false});

   async function fetchData() {
      
      setLoading(true);

      const response = await GetPharmaciesThatHaveMeetingService(paginated);

      setLoading(false);

      if(!response.IsSuccess)
      {
         setResult(false);
         return;
      }

      setPaginated((paginated)=>{

         return {...paginated,LastPharmacyId:response.result.lastPharmacyId,
                              IsRowsCountCalculated:true
      }});

      setPharmacies((prev)=>{
        return  [...prev,...response.result.pharmacies]
      });
   }

   useEffect(()=>{
     
      fetchData();
   },[]);

   const PharmaciesCards = pharmacies.map((pharmacy,index)=>{
      
      return <Grid size={{xs:12,md:6}} sx={{display:'flex',justifyContent:'center',alignItems:'center'}}>
                <PharmacyMeetingCard
                  key={index}
                  pharmacyId={pharmacy.pharmacyId}
                  pharmacyName={pharmacy.pharmacyName}
                  image={pharmacy.imageURL}
                  price={pharmacy.price}
                  governateName={pharmacy.governateName}
                />
            </Grid>
   });

   

   return(
      
          
            <InfiniteScroll
               dataLength={pharmacies.length}
               next={fetchData}
               hasMore={result}
            >
               <Grid container  sx={{display:'flex',justifyContent:'center',alignItems:'center'}}>
                  {PharmaciesCards}

                     {!result?<p>لا يوجد المزيد من الصيدليات</p>
                     :null
                     }

                     {loading?<CircularProgress size={40} color='rgb(255, 86, 114)' />
                     :null}
               </Grid>   

            </InfiniteScroll>
   );
}

export default PharmaciesMeetingPage;
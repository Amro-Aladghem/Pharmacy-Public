import Accordion from '@mui/material/Accordion';
import AccordionSummary from '@mui/material/AccordionSummary';
import AccordionDetails from '@mui/material/AccordionDetails';
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import Typography from '@mui/joy/Typography';
import {CircularProgress} from '@mui/material';
import { useState } from 'react';
import { GetProductDescription } from '../../../services/phProductsServices';
import { useParams } from 'react-router-dom';



async function fetchData(productId) {
    
    const response = await GetProductDescription(productId);

    if(!response.IsSuccess)
        return;

    return response.result;
}


export function ProductDescription()
{
    const {productId} = useParams();

    const [expanded, setExpanded] = useState(false);
    const [description,setDescription] = useState("");
    const [loading,setLoading] = useState(false);


    async function handelChange(params) {
        
        setExpanded(!expanded);

        if(!description)
        {
           setLoading(true);

           const description =await fetchData(productId);

           setLoading(false);

           setDescription(description);
        }
    }

    return(
         <Accordion expanded={expanded} onChange={()=>handelChange()} sx={{width:'100%'}}>
                         <AccordionSummary
                             expandIcon={<ExpandMoreIcon />}
                             aria-controls="panel1-content"
                             id="panel1-header"
                         >
                             <Typography level="title-lg"> عرض معلومات عن المنتج </Typography>
                         </AccordionSummary>
                         <AccordionDetails>
                             {loading?(
                                <CircularProgress size={40}/>
                             ):(
                                description
                             )
                             }
                         </AccordionDetails>
                     </Accordion>

    );
}


export default ProductDescription;
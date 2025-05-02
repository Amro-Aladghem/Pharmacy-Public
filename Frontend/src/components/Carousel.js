import React, { useState, useEffect } from 'react';
import { Box } from '@mui/material';


const images = [
    '2.png',
    '1.png',
    '3.png',
  ];


export function Carousel()
{

    const [current, setCurrent] = useState(0);

    useEffect(() => {
      const timer = setInterval(() => {
        setCurrent(prev => (prev + 1) === images.length ? 0: (prev + 1));
      }, 8000); 
      return () => clearInterval(timer);
    }, []);

    return (
        <>
       
              <Box sx={{ width: '100%', height: {xs:'120px',sm:'250px',md:'320px'},my:1,direction:'ltr'}}>
                {images.map((src, index) => (
                <Box
                    key={index}
                    component="img"
                    src={src}
                    alt={`Slide ${index}`}
                    sx={{
                    width: '65%',
                    height: 'auto',
                    position: 'absolute',
                    marginLeft: '50%',
                    transform: 'translateX(-50%)',
                    opacity: index == current ? 1 : 0,
                    transition: 'opacity 1s ease-in-out',
                    borderRadius:"8px"
                    }}
                    display='block'
                />
                ))}
            </Box>
        

        </>

    )
}



export default Carousel;
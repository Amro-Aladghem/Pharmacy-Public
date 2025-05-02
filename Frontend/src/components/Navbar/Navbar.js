import UpperNavbar from "../UpperNavbar/UpperNavbar";
import BottemNavbar from "../BottemNavbar/BottemNavbar";
import { Outlet } from "react-router-dom";
import { ItemsCountProvider } from "../../contexts/ItemsCountContext";


export function Navbar()
{

    return (
    <>
      <ItemsCountProvider>
          <UpperNavbar />
            <Outlet />
          <BottemNavbar/>
       </ItemsCountProvider>
    </>

    )

}



export default Navbar;

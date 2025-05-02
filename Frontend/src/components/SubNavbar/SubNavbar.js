import './SubNavbar.css';
import { Outlet } from 'react-router-dom';

export function SubNavbar()
{
    return (
        <>
            <div id="SubNav-container">
            <a href="/">
                <img id="Sub-img"
                src='https://res.cloudinary.com/dlu3aolnh/image/upload/v1735059932/udjkg3nflplvydkrngf0.png' alt="logo"/>
                </a>
            </div>
            <Outlet />
        </>
    )
}


export default SubNavbar;
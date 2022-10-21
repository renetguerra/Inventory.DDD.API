import React from 'react'
import { NavLink } from 'react-router-dom';

export const Nav = () => {
    return (
        <nav className="nav">
            <ul>
                <li><NavLink to="/home" className={({ isActive }) => isActive ? "active" : ""} >Inicio</NavLink></li>
                <li><NavLink to="/articles" className={({ isActive }) => isActive ? "active" : ""}>Artículos</NavLink></li>
                
                <li><a href='#'>Contacto</a></li>
            </ul>
        </nav>
    )
}

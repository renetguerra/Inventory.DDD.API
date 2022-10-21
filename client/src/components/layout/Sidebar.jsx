import React from 'react'
import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import "bootstrap/dist/css/bootstrap.min.css";

export const Sidebar = () => {

const [filter, setFilter] = useState("");

const redirect = useNavigate();

const searchArticle = (e) => {
    e.preventDefault();
    let textFiltered = e.target.search_field.value;
    redirect("/filter-article/" + textFiltered, { replace:true });
}

  return (
    <>
    <aside className="side">        
        

        <div className="search">
            <h3 className="title">Buscador</h3>
            <form onSubmit={searchArticle}>
                <input type="text" name="search_field" />
                <input type="submit" id="search" value="Buscar"/>
            </form>
        </div>        
    </aside>
    

    
    </>
    


  )
}


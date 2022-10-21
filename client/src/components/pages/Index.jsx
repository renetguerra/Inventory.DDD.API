import React from 'react'
import { Link } from 'react-router-dom';

export const Index = () => {
  return (
    <div className='jumbo'>
      <h1>Inventory</h1>      
      <Link to="/articles" className='button'>Ver los artículos</Link>
    </div>
  )
}

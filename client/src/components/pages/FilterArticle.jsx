import React from 'react';
import { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import { Enviroments } from "../../enviroments/Enviroments";
import { RequestAsyncAjax } from '../../helper/RequestAsyncAjax';
import { ListArticle } from './ListArticle';

export const FilterArticle = () => {
  const [articles, setArticles] = useState([]);
  const [loading, setLoading] = useState(true);
  const params = useParams();

  useEffect(() => {    
    getArticles();    
  }, [])

  useEffect(() => {    
    getArticles();    
  }, [params])

  const getArticles = async() => {    
    const url = Enviroments.urlAPI + "article/filter-articles-by-text/" + params.textFiltered;    

    const {data, loading} = await RequestAsyncAjax(url, "GET");
   
    if (data.length > 0) {
      setArticles(data);
    } else {
      setArticles([]);
    }

    setLoading(false);
   
  }

  return (
    <>
    {loading ? "Cargando..." :
      articles.length > 0 ? <ListArticle articles={articles} setArticles={setArticles} /> : <h1>No existen artículos</h1>
    }
  </>
  )
}

import React, { useState, useEffect } from 'react';
import { useForm } from '../../hooks/useForm';
import { useParams } from 'react-router-dom';
import { Enviroments } from '../../enviroments/Enviroments';
import { RequestAsyncAjax } from '../../helper/RequestAsyncAjax';
import { Toaster, toast } from 'react-hot-toast';

export const EditArticle = ({modalEdit, setModalEdit, article, setArticle, setArticles}) => {
  const { form, sent, changed } = useForm({});
  const [hasResult, setHasResult] = useState('not_sent'); 
  const params = useParams();

  useEffect(() => {
    console.log("articleToUpdate");
    console.log(article);

  }, [])

  const updateArticle = async (e) => {
    e.preventDefault();

    // Obtenemos los datos del formulario
    let articleToUpdate = form;
    
    articleToUpdate.id = article.id;
    console.log("ArticleToUpdate");
    console.log(articleToUpdate);

    // Guardar articulo en bbdd
    const { data, loading } = await RequestAsyncAjax(
      Enviroments.urlAPI + 'article/edit-article/',
      'PUT',
      articleToUpdate
    );
    
    if (data.statusCode == 200) {
      setHasResult('saved');

      setArticle(data.value.article);

      setModalEdit(false);   

      notifyCommon().success("¡El artículo ha sido acualizado correctamente!");

      if (data.value?.domainEvents?.length > 0){      
        let msg = data.value.domainEvents[0].msgNotification.toString();     
        notifyCommon().error(msg);
      }  

      getArticles();
    }
    else {
      setHasResult('error');
    }     
  };

  const getArticles = async() => {
    const url = Enviroments.urlAPI + "article/list";   

    const {data, loading} = await RequestAsyncAjax(url, "GET");
    
    if (data.length > 0) {
      setArticles(data);                        
    }    
  }

  const notifyCustom = (props, params) => {
    return toast(props, params);
  }  

  const notifyCommon = () => {
    return toast;
  }  

  return (
    <div className="jumbo">
      <h1>Editar artículo {article?.name}</h1>

      <strong>
        {hasResult == 'saved' ? 'Artículo guardado correctamente!' : ''}
      </strong>
      <strong>
        {hasResult == 'error' ? 'Los datos introducidos no son válidos!' : ''}
      </strong>

    <form onSubmit={updateArticle} className="form" id="form">
      <div className="form-group" >        
        <input type="text" name="id" defaultValue={article?.id}  readOnly disabled="true" />
      </div>
      <div className="form-group">
        <label htmlFor="name">Nombre</label>
        <input type="text" name="name" onChange={changed} defaultValue={article?.name}/>
      </div>
      <div className="form-group">
        <label htmlFor="description">Descripción</label>
        <textarea type="text" name="description" onChange={changed} defaultValue={article?.description}/>
      </div>
      <div className="form-group">
        <label htmlFor="type">Tipo</label>
        <input type="text" name="type" onChange={changed} defaultValue={article?.type}/>
      </div>
      <div className="form-group">
        <label htmlFor="brand">Marca</label>
        <input type="text" name="brand" onChange={changed} defaultValue={article?.brand}/>
      </div>
      <div className="form-group">
        <label htmlFor="price">Precio</label>
        <input type="text" name="price" onChange={changed} defaultValue={article?.price}/>
      </div>
      <div className="form-group">
        <label htmlFor="stock">Existencia</label>
        <input type="text" name="stock" onChange={changed} defaultValue={article?.stock}/>
      </div>          
        <input type="submit" value="Guardar" className="btn btn-success" />
      </form>

      <Toaster position="top-right" reverseOrder={false} />

    </div>
  );
}

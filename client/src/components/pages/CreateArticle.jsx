import React, { useState } from 'react';
import { useForm } from '../../hooks/useForm';
import { Enviroments } from '../../enviroments/Enviroments';
import { RequestAsyncAjax } from '../../helper/RequestAsyncAjax';
import toast, { Toaster } from 'react-hot-toast';

export const CreateArticle = ({modalInsert, setModalInsert, articles, setArticles}) => {
  const { form, sent, changed } = useForm({});
  const [hasResult, setHasResult] = useState('not_sent');

  const saveArticle = async (e) => {
    e.preventDefault();

    // Obtenemos los datos del formulario
    let newArticle = form;    

    // Guardar articulo en bbdd
    const { data, loading } = await RequestAsyncAjax(
      Enviroments.urlAPI + "article/add-article",
      "POST",
      newArticle
    );    
     
    if (data.statusCode == 200) {      
      setHasResult('saved');
      document.querySelector('#form').reset();
     
      let article = data.value.article;
      setArticles(articles => [...articles, article]);      

      setModalInsert(false);
      
      notifyCommon().success("¡El artículo ha sido creado correctamente!");

      if (data.value?.domainEvents?.length > 0){      
        let msg = data.value.domainEvents[0].msgNotification.toString();     
        notifyCommon().error(msg);
      }                      
    } else {
      setHasResult('error');
    } 
  };

  const notifyCommon = () => {
    return toast;
  }  

  return (
    <div className="jumbo">
      <h1>Crear artículo</h1>

      <strong className='success'>
        {hasResult == 'saved' ? 'Artículo guardado correctamente!' : ''}
      </strong>
      <strong>
        {hasResult == 'error' ? 'Los datos introducidos no son válidos!' : ''}
      </strong>

      <form onSubmit={saveArticle} className="form" id="form">      
        <div className="form-group">
          <label htmlFor="name">Nombre</label>
          <input type="text" name="name" onChange={changed} />
        </div>
        <div className="form-group">
          <label htmlFor="description">Descripción</label>
          <textarea type="text" name="description" onChange={changed} />
        </div>
        <div className="form-group">
          <label htmlFor="type">Tipo</label>
          <input type="text" name="type" onChange={changed} />
        </div>
        <div className="form-group">
          <label htmlFor="brand">Marca</label>
          <input type="text" name="brand" onChange={changed} />
        </div>
        <div className="form-group">
          <label htmlFor="price">Precio</label>
          <input type="text" name="price" onChange={changed} />
        </div>
        <div className="form-group">
          <label htmlFor="stock">Existencia</label>
          <input type="text" name="stock" onChange={changed} />
        </div>
        <div className="form-group">
          <label htmlFor="expirationDate">Fecha de expiración</label>
          <input type="text" name="expirationDate" onChange={changed} />
        </div>        

        <input type="submit" value="Guardar" className="btn btn-success" />
      </form>

      <Toaster position="top-right" reverseOrder={false} />

    </div>
  );
};

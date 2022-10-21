import React from 'react';
import { useState, useEffect } from 'react';
import { Enviroments } from "../../enviroments/Enviroments";
import { RequestAsyncAjax } from '../../helper/RequestAsyncAjax';
import { ListArticle } from './ListArticle';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faCirclePlus } from '@fortawesome/free-solid-svg-icons';
import { Modal, ModalBody, ModalFooter, ModalHeader } from 'reactstrap';
import { CreateArticle } from './CreateArticle';
import { Toaster, toast } from "react-hot-toast";

export const Articles = () => {

  const [articles, setArticles] = useState([]);
  const [loading, setLoading] = useState(true);

  const [modalInsert, setModalInsert] = useState(false);
  const [tipoModal, setTipoModal] = useState('insert');
  

  useEffect(() => {
    getArticles();    
  }, [])

  const getArticles = async() => {
    const url = Enviroments.urlAPI + "article/list";   

    const {data, loading} = await RequestAsyncAjax(url, "GET");
    
    if (data.length > 0) {
      setArticles(data);            
      
      data.map(article => {        
        if (article.domainEvents.length > 0){      
          let msg = article.domainEvents[0].msgNotification.toString();     
          notifyCommon().error(msg);
        }
      });      
    }
    setLoading(false);   
  }

  const toogleModalInsert = () =>{           
    setModalInsert(!modalInsert);
    getArticles();
  }

  const notifyCustom = (props, params) => {
    return toast(props, params);
  };
  const notifyCommon = () => {
    return toast;
  };
  
  return (
    <>

      <div className="create">                    
        <button className="btn create" onClick={()=>toogleModalInsert() }><FontAwesomeIcon icon={faCirclePlus} size="5x"/></button>
      </div>
      <Toaster position="top-right" reverseOrder={false} />

    {loading ? "Cargando..." :
      articles.length > 0 ? <div className='list'><ListArticle articles={articles} setArticles={setArticles} /></div> : <h1>No existen artículos</h1>
    }       

    <Modal isOpen={modalInsert}>
      <ModalHeader style={{display: 'block'}}>
        <span style={{float: 'right'}} onClick={()=>toogleModalInsert()}>x</span>
      </ModalHeader>
      <ModalBody>           
        <CreateArticle modalInsert={modalInsert} setModalInsert={setModalInsert} articles={articles} setArticles={setArticles} />
      </ModalBody>          
    </Modal>
  </>
  )
};

import React, { useState } from 'react'
import { Link } from 'react-router-dom';
import { Enviroments } from '../../enviroments/Enviroments';
import { RequestAsyncAjax } from '../../helper/RequestAsyncAjax';
import "bootstrap/dist/css/bootstrap.min.css";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faPen, faTrashCan } from '@fortawesome/free-solid-svg-icons';
import { Toaster, toast } from 'react-hot-toast';
import { EditArticle } from './EditArticle';
import { Modal, ModalBody, ModalFooter, ModalHeader } from 'reactstrap';


export const ListArticle = ({articles, setArticles}) => {

  const [modalEdit, setModalEdit] = useState(false);
  const [article, setArticle] = useState({}); 

const deleteArticle = async(name) => {
  let {data} = await RequestAsyncAjax(Enviroments.urlAPI + "article/remove-article-name/" + name, "DELETE");  

  console.log(data);  
  if (data!=null && data.value.name == name){    
    let listArticles = articles.filter(article => article.id != data.value.id);
    setArticles(listArticles);
    
    if (data.value.domainEvents.length > 0){      
      let msg = data.value.domainEvents[0].msgNotification.toString();     
      notifyCommon().error(msg);
    }
  }
}

const toogleModalEdit = (article) =>{           
  setModalEdit(!modalEdit);
  setArticle(article);
}

const notifyCommon = () => {
  return toast;
}

  return (
    <>
    {
      articles.map(article => {      
          return (
            <>
              <article key={article._id} className={(article.rowActive ? 'article-item' : 'article-item-disable')}>         
                <div className='data'>
                  <h1 className="title">{article.name}</h1>
                  <p className="description">{article.description}</p>
                  <p className="brand">{article.brand}</p>            
                  <p className="price">{article.price} €</p>
                  <p className="type"><strong>Tpo:</strong> {article.type}</p>
                  <p className="stock"><strong>Existencia:</strong> {article.stock}</p>            
                  <p className="expirationDate"><strong>Fecha de expiración:</strong> {article.expirationDate}</p>
        
                  <button className="btn btn-primary" onClick={()=> toogleModalEdit(article)}><FontAwesomeIcon icon={faPen} /></button>
                  <button className="btn btn-danger" onClick={() => {deleteArticle(article.name)}} disabled={!article.rowActive}>
                    <FontAwesomeIcon icon={faTrashCan} />
                  </button>
                </div>       
              </article>   

                        
            </>

          );
        })
      }

            <Modal isOpen={modalEdit}>
              <ModalHeader style={{display: 'block'}}>
                <span style={{float: 'right'}} onClick={()=>toogleModalEdit()}>x</span>
              </ModalHeader>
              <ModalBody>           
                <EditArticle modalEdit={modalEdit} setModalEdit={setModalEdit} article={article} setArticle={setArticle} setArticles={setArticles} />
              </ModalBody>          
            </Modal>
          
            <Toaster position="top-right" reverseOrder={false} />  
    </>
   

     
  )
}

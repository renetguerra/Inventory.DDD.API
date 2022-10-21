import { useState } from 'react';

export const useForm = (initObjet = {}) => {
  const [form, setForm] = useState(initObjet);

  const serializationForm = (form) => {
    const formData = new FormData(form);
    const completeObjet = {};

    for (let [name, value] of formData) {
      completeObjet[name] = value;
    }

    return completeObjet;

  }

  const sent = (e) => {
    e.preventDefault();
    let objet = serializationForm(e.target);
    setForm(objet)
  }

  const changed = ({target}) => {
    const {name, value} = target;

    setForm({
        ...form,
        [name]: value
    });
  }

  return {
    form,
    sent,
    changed
  }

};

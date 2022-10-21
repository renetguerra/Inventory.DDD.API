export const RequestAsyncAjax = async(url, method, dataToSaved = "", files = false) => {
    
    let loading = true;

    let options = {
        method: "GET"
    };

    if (method == "GET" || method == "DELETE") {
        options = {
            method: method,
        };
    }

    
    if (method == "POST" || method == "PUT"){        
        if (files) {
            options = {
                method: method,
                body: dataToSaved
            };
        }
        else {
            options = {
                method: method,
                body: JSON.stringify(dataToSaved),            
                headers: {
                    "Content-Type": "application/json; charset=utf-8"
                }
            };
        }
       
    }
       
    const request = await fetch(url, options);    
    let data = await request.json();  
        

    loading = false;

    return {        
        data,
        loading
    }
}


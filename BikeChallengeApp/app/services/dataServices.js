// Get Data Services () 
// ********************************************************* //


app.factory('dataFactory', function ($rootScope,  $http, session) {

    var dataFactory = {};


// ############# GET ############################## // 
// ################################################ // 

    //Get values - general factory, parFlag used to pass a param for the api (api?"par")
    
        dataFactory.getValues = function (urlPath, parFlag, par) {
            if (!parFlag) {
                return $http({
                    method: 'GET',
                    url: '/api/' + urlPath,
                    headers: { 'Authorization': 'Bearer ' + session.id },
                });
            }
            else {
                console.log("In Flag");
                return $http({
                    method: 'GET',
                    url: '/api/' + urlPath+ '?' + par,
                    headers: { 'Authorization': 'Bearer ' + session.id },
                });

            }
        }
   
// ############# POST ############################## // 
// ################################################ // 

    dataFactory.postValues = function (dataObj, urlPath) {
        // obj = angular.toJson(obj, true);
        return $http({
            method: 'POST',
            url: '/api/' + urlPath,
            headers: { 'Authorization': 'Bearer ' + session.id },
            data: dataObj
        });
    }


// ############# PUT ############################## // 
// ################################################ // 

    dataFactory.updateValues = function (cust) {
        return $http({
            method: 'PUT',
            url: '/api/' + urlPath,
            headers: { 'Authorization': 'Bearer ' + session.id },
            data: dataObj
        });
    };

// ############# Delete ############################## // 
// ################################################ // 

    dataFactory.deleteValues = function (id) {
        return $http({
            method: 'DELETE',
            url: '/api/' + urlPath,
            headers: { 'Authorization': 'Bearer ' + session.id },
            data: dataObj
        });
    };


    return dataFactory;

    
});




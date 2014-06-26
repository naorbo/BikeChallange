// Get Data Services () 
// ********************************************************* //


app.factory('dataFactory', function ($rootScope, $http, session, serverBaseUrl) {

    var dataFactory = {};
    //var serverBaseUrl = 'http://proj.ruppin.ac.il/igroup1/prod/BikeChallenge/'

// ############# GET ############################## // 
// ################################################ // 

    //Get values - general factory, parFlag used to pass a param for the api (api?"par")
    
        dataFactory.getValues = function (urlPath, parFlag, par) {
            if (!parFlag) {
                return $http({
                    method: 'GET',
                    url: serverBaseUrl + '/api/' + urlPath,
                    headers: { 'Authorization': 'Bearer ' + session.id },
                });
            }
            else {
                console.log("In Flag");
                return $http({
                    method: 'GET',
                    url: serverBaseUrl + '/api/' + urlPath + '?' + par,
                    headers: { 'Authorization': 'Bearer ' + session.id },
                });

            }
        }
   
// ############# POST ############################## // 
// ################################################ // 

    dataFactory.postValues = function (urlPath, dataObj, parFlag, par) {
        // obj = angular.toJson(obj, true);
        if (!parFlag){
        return $http({
            method: 'POST',
            url: serverBaseUrl + '/api/' + urlPath,
            headers: { 'Authorization': 'Bearer ' + session.id },
            data: dataObj
        });
        }
        else {
            return $http({
                method: 'POST',
                url: serverBaseUrl + '/api/' + urlPath + "?" + par,
                headers: { 'Authorization': 'Bearer ' + session.id },
            });
        }


    }


// ############# PUT ############################## // 
// ################################################ // 

    dataFactory.updateValues = function (urlPath, dataObj, parFlag, par) {
        //return $http({
        //    method: 'PUT',
        //    url: '/api/' + urlPath + '?' +par,
        //    headers: { 'Authorization': 'Bearer ' + session.id },
        //    data: dataObj
        //});


        if (!parFlag) {
            return $http({
                method: 'PUT',
                url: serverBaseUrl + '/api/' + urlPath,
                headers: { 'Authorization': 'Bearer ' + session.id },
                data: dataObj
            });
        }
        else {
            return $http({
                method: 'PUT',
                url: serverBaseUrl + '/api/' + urlPath + "?" + par,
                headers: { 'Authorization': 'Bearer ' + session.id },
                data: dataObj
            });
        }
    };

// ############# Delete ############################## // 
// ################################################ // 

    dataFactory.deleteValues = function (urlPath, par) {
        return $http({
            method: 'DELETE',
            url: serverBaseUrl + '/api/' + urlPath + '?' + par,
            headers: { 'Authorization': 'Bearer ' + session.id },
            
        });
    };


    return dataFactory;

    
});

app.factory(
            "confirm",
            function ($window, $q) {

                // Define promise-based confirm() method.
                function confirm(message) {

                    var defer = $q.defer();

                    // The native confirm will return a boolean.
                    if ($window.confirm(message)) {

                        defer.resolve(true);

                    } else {

                        defer.reject(false);

                    }

                    return (defer.promise);

                }

                return (confirm);

            }
        );


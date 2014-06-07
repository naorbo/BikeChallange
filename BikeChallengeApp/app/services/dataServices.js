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

    dataFactory.postValues = function (urlPath, dataObj, parFlag, par) {
        // obj = angular.toJson(obj, true);
        if (!parFlag){
        return $http({
            method: 'POST',
            url: '/api/' + urlPath,
            headers: { 'Authorization': 'Bearer ' + session.id },
            data: dataObj
        });
        }
        else {
            return $http({
                method: 'POST',
                url: '/api/' + urlPath + "?" + par,
                headers: { 'Authorization': 'Bearer ' + session.id },
            });
        }


    }


// ############# PUT ############################## // 
// ################################################ // 

    dataFactory.updateValues = function (urlPath, dataObj, parFlag, par) {
        return $http({
            method: 'PUT',
            url: '/api/' + urlPath + '?' +par,
            headers: { 'Authorization': 'Bearer ' + session.id },
            data: dataObj
        });

    };

// ############# Delete ############################## // 
// ################################################ // 

    dataFactory.deleteValues = function (urlPath, par) {
        return $http({
            method: 'DELETE',
            url: '/api/' + urlPath + '?' + par,
            headers: { 'Authorization': 'Bearer ' + session.id },
            
        });
    };


    return dataFactory;

    
});



//app.factory('userProfileFactory', function ($rootScope, $scope, session, dataFactory) {

//    var userProfileFactory = {};


//    // ############# GET ############################## // 
//    // ################################################ // 

//    userProfileFactory.storeUserProfile = function (){

//    dataFactory.getUserProfile('Rider', true, "username=" + $scope.currentUser)
//            .success(function (values) {
//                sessionProfile.create(values);
//                $rootScope.personalProfile = values[0];
//                console.log("personalProfile " + $scope.currentUser);
//                console.log($scope.personalInfoHolder);
//            })

//            .error(function (value) {
//                console.log("error");

//            });
//    }

//    return userProfileFactory;


//});


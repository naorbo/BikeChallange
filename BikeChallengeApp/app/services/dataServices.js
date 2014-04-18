// Get Data Services () 
// ********************************************************* //

//app.factory('valuesFactory', function ($rootScope, $http, $q, session) {

//    var dataService = {};

//    dataService.AccessToken = session.id;
    

//    dataService.getValues = function () {
//        var deferred = $q.defer();
        
//        $http({
//            method: 'GET',
//            url: '/api/Values',
//            headers: { 'Authorization': 'Bearer ' + dataService.AccessToken },

//        })
//            .success(function (response) {

//                console.log(response)
//                deferred.resolve(response);
//            })
//            .error(function (response) {
//                deferred.reject(response);
//            });

//    }

//    return dataService;
//});

app.factory('valuesFactory', function ($rootScope, $http, $q, session) {

    var dataService = {};

    
    dataService.getValues = function (urlPath) {
        var deferred = $q.defer();

        $http({
            method: 'GET',
            url: '/api/'+urlPath,
            headers: { 'Authorization': 'Bearer ' + session.id },

        })
            .success(function (response) {

                console.log(response)
                dataService = response;
                deferred.resolve(response);
            })
            .error(function (response) {
                deferred.reject(response);
            });

    }

    return dataService;
});

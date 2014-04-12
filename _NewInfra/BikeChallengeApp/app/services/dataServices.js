// Get Data Services () 
// ********************************************************* //

app.factory('valuesFactory', function ($rootScope, $http, $q, session) {

    console.log("inside the factory");

    

    

    var dataService = {};

    dataService.AccessToken = session.id;
    

    dataService.getValues = function () {
        var deferred = $q.defer();
        console.log(dataService.AccessToken);
        $http({
            method: 'GET',
            url: '/api/Values',
            headers: { 'Authorization': 'Bearer ' + dataService.AccessToken },

        })
            .success(function (response) {

                console.log(response)
                deferred.resolve(response);
            })
            .error(function (response) {
                deferred.reject(response);
            });

    }

    return dataService;
});


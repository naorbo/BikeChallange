// Get Data Services () 
// ********************************************************* //


app.factory('dataFactory', function ($rootScope,  $http, session) {

    var dataFactory = {};

    dataFactory.getValues = function (urlPath) {
            
        return $http({
            method: 'GET',
            url: '/api/' + urlPath,
            headers: { 'Authorization': 'Bearer ' + session.id },
        });}


    return dataFactory;

    
});




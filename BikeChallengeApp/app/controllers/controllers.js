/// <reference path="../Scripts/angular.js" />


// ####################################################################################################################################################### // 
// #########################################                aboutController               ################################################################ // 
// ####################################################################################################################################################### // 



app.controller('aboutController', function ($scope) {

});


// ####################################################################################################################################################### // 
// #########################################                homeController               ################################################################ // 
// ####################################################################################################################################################### // 


app.controller('homeController', function ($scope) {

});


// ####################################################################################################################################################### // 
// #########################################                loginController               ################################################################ // 
// ####################################################################################################################################################### // 



app.controller('loginController', function ($rootScope, $scope, authFactory, AUTH_EVENTS) {
    
    $scope.failMSG = "שם משתמש או סיסמא שגויה, נסה שנית"
    $scope.failFlag = false;
    $scope.signIn = function () {
        authFactory.login($scope.loginDetails.userName, $scope.loginDetails.password).then(function () {
            $rootScope.$broadcast(AUTH_EVENTS.loginSuccess);
            console.log("Event BC - user logged ");
        }, function () {
            $rootScope.$broadcast(AUTH_EVENTS.loginFailed);
            console.log("Event BC - bad login ");
            $scope.failFlag = true;
        });
    }
});

// ####################################################################################################################################################### // 
// #########################################                signUpController               ############################################################### // 
// ####################################################################################################################################################### // 



app.controller('signUpController', function ($rootScope, $scope, $http, $timeout, $upload, dataFactory, authFactory, AUTH_EVENTS) {

    //SignUp function - register the user with the ASP.NET EF
    $scope.signUp = function () {
        console.log("Trying to EF");
        authFactory.register($scope.regDetails.userName.$viewValue, $scope.password, $scope.regDetails.confirmPassword.$viewValue).then(function () {
            console.log("Signup successfull (EF), BCing success");
            $rootScope.$broadcast(AUTH_EVENTS.registrationSuccessEF);
            $scope.loadCities();
        }, function () {
            $rootScope.$broadcast(AUTH_EVENTS.registrationFailed);
            console.log("Registration Failure ");
        } 
        )};

    
    //userRegistration - register the new user with BC DB 
    $scope.userRegistration = function (userName) {
        //authFactory.registerDetails($scope.personalDetails).then(function () {
        //})
        console.log("this is the user"+$scope.regDetails.userName.$viewValue);
        // Parse all info and adjust to server vars
        var userDetails = {};
        userDetails.RiderEmail = $scope.$$childHead.personalDetails.email;
        userDetails.RiderFname = $scope.$$childHead.personalDetails.firstName;
        userDetails.RiderLname = $scope.$$childHead.personalDetails.lastName;
        userDetails.Gender = $scope.$$childHead.personalDetails.gender;
        userDetails.RiderAddress = $scope.$$childHead.personalDetails.address;
        userDetails.City = $scope.$$childHead.personalDetails.city.CityName;
        userDetails.RiderPhone = $scope.$$childHead.personalDetails.phone;
        userDetails.BicycleType = $scope.$$childHead.personalDetails.bikeType;
        userDetails.ImagePath = $scope.$$childHead.personalDetails.imagePath;
        userDetails.BirthDate = $scope.$$childHead.personalDetails.bDay;
        userDetails.UserName = $scope.regDetails.userName.$viewValue;
        userDetails.Captain = "0"; 
        // Should be added - default KM per Day
            // Captain Flag 
            if ($scope.newOrgFlag || $scope.newTeamFlag) { userDetails.Captain = "1";}

            userDetails.Organization = $scope.$$childHead.personalDetails.org;
            userDetails.Group = $scope.$$childHead.personalDetails.team.GroupName;

        userDetails = angular.toJson(userDetails, true);
        
        // Post to Server
        
        dataFactory.postValues(userDetails,'Rider')
                 .success(function (values) {
                     alert("ההרשמה הסתיימה בהצלחה!");
                     $rootScope.$broadcast(AUTH_EVENTS.registrationSuccess);
                     
                 })
                 .error(function (error) {
                     alert("ההרשמה נכשלה!");
                 });
        }



        
    

    ///////Organization Handling 

    
    //Save & Close - Modal Window - Passes selected Org from modal windows to attribute 
    $scope.myOrg = function (chosenOrg) {
        console.log("org is " + chosenOrg);
        $scope.$$childHead.personalDetails.org = chosenOrg;
        $('#myModal').modal('hide');
        $scope.getTeamByOrg(chosenOrg);
        return;
    }

    
    //Refresh orgs list from DB
    $scope.refreshOrgs = function () {
        console.log("Inside refresher orgs");
        
        $scope.orgHolder = angular.fromJson(dataFactory.getValues('Organization', false, 0));
        console.log("This is the data :" +  $scope.orgHolder);

    }

    $scope.getOrgs = function () {
        dataFactory.getValues('Organization', false, 0)
             .success(function (values) {
                 $scope.orgs = angular.fromJson(values);
                 console.log($scope.orgs);
                 
               
             })
             .error(function (error) {
                 $scope.status = 'Unable to load Orgs data: ' + error.message;
             });
    }

    //Clears org selection
    $scope.clearSelection = function () {
        console.log("Im reseting this one ");
        $scope.orgSelection = null;
        $scope.$$childHead.personalDetails.org = null;
        $scope.$$childHead.personalDetails.team = null;
        return;
    };

    // New Organization Creator 

    $scope.regNewOrg = function (newOrgObj) {
        dataFactory.postValues(newOrgObj,'Organization')
             .success(function (response) {
                 console.log(response);
                 $scope.newOrgFlag = true;
                 alert("  הארגון  " + newOrgObj.OrganizationName + "  נוצר בהצלחה ! ");
                 // Closing Modal window 
                 $scope.$$childHead.personalDetails.org = newOrgObj.OrganizationName;
                 $('#myNewOrgModal').modal('hide');
             })
             .error(function (error) {
                 $scope.status = 'Unable to load Orgs data: ' + error.message;
                 alert("שגיאה ביצירת ארגון חדש");
             });
    }

    $scope.newOrgFlag = false;
    $scope.newTeamFlag = false;

    $scope.flagReset = function () {
        if ($scope.newOrgFlag == false) 
            $scope.newOrgFlag = true;
        else
            $scope.newOrgFlag = false;

    }

    // Team Handlers 

    // Fetch team per Org
    $scope.getTeamByOrg = function (orgName) {
        console.log('trying to get teams')
        dataFactory.getValues('Group', true, 'orgname=' + orgName)
            .success(function (values) {
                $scope.teamPerOrg = angular.fromJson(values);
            }) 
    }

    // Register a new team 

    $scope.regNewTeam = function (newTeamObj, org) {
        console.log("This is the Org PAssed" + $scope.$$childHead.personalDetails.org)
        newTeamObj.OrganizationName = $scope.$$childHead.personalDetails.org;
        dataFactory.postValues(newTeamObj, 'Group')
             .success(function (response) {
                 console.log(response);
                 newTeamObj.OrganizationName = $scope.$$childHead.personalDetails.org;
                 $scope.newTeamFlag = true;
                 alert("  הקבוצה  " + newTeamObj.GroupName + "  נוצרה בהצלחה ! ");
                 // Closing Modal window 
                 $scope.$$childHead.personalDetails.team = newTeamObj.GroupName;
                 $('#myNewTeamModal').modal('hide');
                 $scope.getTeamByOrg(newTeamObj.OrganizationName);
             })
             .error(function (error) {
                 $scope.status = 'Unable to create a new team: ' + error.message;
                 alert("שגיאה ביצירת קבוצה חדשה");
             });
    }

    // Upload image handling 


    $scope.upload = [];
    $scope.fileUploadObj = { testString1: "Test string 1", testString2: "Test string 2" };

    $scope.onFileSelect = function ($files) {
        //$files: an array of files selected, each file has name, size, and type.
        for (var i = 0; i < $files.length; i++) {
            var $file = $files[i];
            (function (index) {
                $scope.upload[index] = $upload.upload({
                    url: "/api/UserImage?UserName=" + $scope.regDetails.userName.$viewValue, // webapi url
                    method: "POST",
                    data: { fileUploadObj: $scope.fileUploadObj },
                    file: $file
                }).progress(function (evt) {
                    // get upload percentage
                    console.log('percent: ' + parseInt(100.0 * evt.loaded / evt.total));
                }).success(function (data, status, headers, config) {
                    // file is uploaded successfully
                    console.log(data);
                    $scope.$$childHead.personalDetails.imagePath = data.returnData;
                }).error(function (data, status, headers, config) {
                    // file failed to upload
                    console.log(data);
                });
            })(i);
        }
    }

    $scope.abortUpload = function (index) {
        $scope.upload[index].abort();
    }



    //Get City list

   


    $scope.loadCities = function () {
        console.log('trying to get teams');
        dataFactory.getValues('Cities', false, 0)
            .success(function (values) {
                $scope.citiesHolder = angular.fromJson(values);
            })
    }

    


    
    



   
    // Event Handlers

    $scope.$on('reg-success-ef', function () {
        $scope.showDetails = true;
        // Add here disable main form of registration 

    })

    $scope.$on('reg-success', function () {
        //login and redirection to userpage after successfull registration
        authFactory.login($scope.regDetails.userName.$viewValue, $scope.regDetails.password.$viewValue).then(function () {
            $rootScope.$broadcast(AUTH_EVENTS.loginSuccess);
            console.log("Event BC - user logged ");
        }, function () {
            $rootScope.$broadcast(AUTH_EVENTS.loginFailed);
            console.log("Event BC - bad login ");
            $scope.failFlag = true;
        })
    })


});

// ####################################################################################################################################################### // 
// #########################################                mainController               ################################################################ // 
// ####################################################################################################################################################### // 



app.controller('mainController', function ($rootScope, $location, $scope ,authFactory, session, AUTH_EVENTS) {
    
    //User login handling (display user info and redirection)

    $scope.currentUser = null;
    //$scope.currentUser = "MOFO";
    //currentUser.name = null;
    //$scope.userRoles = USER_ROLES;
    
    $scope.$on('auth-login-success', function () {
        $scope.currentUser = session.userId;
        console.log("logged in as : " + $scope.currentUser);
        $location.url("/userProfile");
    });

    $scope.$on('auth-login-failed', function () { console.log("Login Failed") });

    // User Logout handling 
    $scope.signOut = function () {

        
        authFactory.logout().then(function () {
            $rootScope.$broadcast(AUTH_EVENTS.logoutSuccess);
            console.log("Event BC - user logged out ");
                
        }, function () {
            
            console.log("Event BC - failed to log out ");
            
        });
    }

    $scope.$on('auth-logout-success', function () {
        $scope.currentUser = null;
        session.userId = null;
        session.id = null;
        $location.url("/home");
    });


});


// ####################################################################################################################################################### // 
// #########################################               userProfileController               ########################################################### // 
// ####################################################################################################################################################### // 


//User Profile - Get Data 

app.controller('userProfileController', function ($rootScope, $location, $scope, $timeout, $http, dataFactory) {
    
    console.log($scope.currentUser);
    dataFactory.getValues('Rider', true, "username=" + $scope.currentUser)
                .success(function (values) {
                   // sessionProfile.create(values);
                    $scope.personalInfoHolder = values[0];
                    $rootScope.userPersonalInfo = values[0];
                    $scope.getGroup();
                    console.log("Fetch user info for " + $scope.currentUser);
                    console.log($scope.personalInfoHolder);
                })
                .error(function (value) {
                    console.log("error");
                });
    
    $scope.getGroup = function () {
        dataFactory.getValues('Group', true, "grpname=" + $rootScope.userPersonalInfo.GroupName + "&orgname=" + $rootScope.userPersonalInfo.OrganizationName)
                .success(function (values) {
                    $scope.myGroup = values;
                })
                .error(function (value) {
                    console.log("error");
                });
    };


    


    $scope.$on('event:auth-loginRequired', function () {
        alert("You are not Authorized to view this page ... Please sign in");
        $location.url("/login")
    });

    ////////// Maps Test 
    
    var onMarkerClicked = function (marker) {
        marker.showWindow = true;
        $scope.$apply();
        //window.alert("Marker: lat: " + marker.latitude + ", lon: " + marker.longitude + " clicked!!")
    };

    var genRandomMarkers = function (numberOfMarkers, scope) {
        var markers = [];
        for (var i = 0; i < numberOfMarkers; i++) {
            markers.push(createRandomMarker(i, scope.map.bounds))
        }
        scope.map.randomMarkers = markers;
    };

    var createRandomMarker = function (i, bounds) {
        var lat_min = bounds.southwest.latitude,
                lat_range = bounds.northeast.latitude - lat_min,
                lng_min = bounds.southwest.longitude,
                lng_range = bounds.northeast.longitude - lng_min;

        var latitude = lat_min + (Math.random() * lat_range);
        var longitude = lng_min + (Math.random() * lng_range);
        return {
            latitude: latitude,
            longitude: longitude,
            title: 'm' + i
        };
    };

    angular.extend($scope, {
        map: {
            control: {},
            version: "uknown",
            heatLayerCallback: function (layer) {
                //set the heat layers backend data
                var mockHeatLayer = new MockHeatLayer(layer);
            },
            showTraffic: true,
            showBicycling: false,
            showWeather: false,
            showHeat: false,
            center: {
                latitude: 45,
                longitude: -73
            },
            options: {
                streetViewControl: false,
                panControl: false,
                maxZoom: 20,
                minZoom: 3
            },
            zoom: 3,
            dragging: false,
            bounds: {},
            markers: [
                {
                    icon: 'assets/images/blue_marker.png',
                    latitude: 45,
                    longitude: -74,
                    showWindow: false,
                    title: 'Marker 2'
                },
                {
                    icon: 'assets/images/blue_marker.png',
                    latitude: 15,
                    longitude: 30,
                    showWindow: false,
                    title: 'Marker 2'
                },
                {
                    icon: 'assets/images/blue_marker.png',
                    latitude: 37,
                    longitude: -122,
                    showWindow: false,
                    title: 'Plane'
                }
            ],
            markers2: [
                {
                    latitude: 46,
                    longitude: -77,
                    showWindow: false,
                    title: '[46,-77]'
                },
                {
                    latitude: 33,
                    longitude: -77,
                    showWindow: false,
                    title: '[33,-77]'
                },
                {
                    icon: 'assets/images/plane.png',
                    latitude: 35,
                    longitude: -125,
                    showWindow: false,
                    title: '[35,-125]'
                }
            ],
            mexiMarkers: [
                {
                    latitude: 29.302567,
                    longitude: -106.248779
                },
                {
                    latitude: 30.369913,
                    longitude: -109.434814
                },
                {
                    latitude: 26.739478,
                    longitude: -108.61084
                }
            ],
            dynamicMarkers: [],
            randomMarkers: [],
            clickMarkers: [
            {"latitude": 50.948968, "longitude": 6.944781}
            ,{"latitude": 50.94129, "longitude": 6.95817}
            ,{"latitude": 50.9175, "longitude": 6.943611}
            ],
            doClusterRandomMarkers: true,
            doUgly: true, //great name :)
            clusterOptions: {title: 'Hi I am a Cluster!', gridSize: 60, ignoreHidden: true, minimumClusterSize: 2,
                imageExtension: 'png', imagePath: 'assets/images/cluster', imageSizes: [72]},
            clickedMarker: {
                title: 'You clicked here',
                latitude: null,
                longitude: null
            },
            events: {
                tilesloaded: function (map, eventName, originalEventArgs) {
                },
                click: function (mapModel, eventName, originalEventArgs) {
                    // 'this' is the directive's scope
                    console.log("user defined event: " + eventName, mapModel, originalEventArgs);

                    var e = originalEventArgs[0];

                    if (!$scope.map.clickedMarker) {
                        $scope.map.clickedMarker = {
                            title: 'You clicked here',
                            latitude: e.latLng.lat(),
                            longitude: e.latLng.lng()
                        };
                    }
                    else {
                        $scope.map.clickedMarker.latitude = e.latLng.lat();
                        $scope.map.clickedMarker.longitude = e.latLng.lng();
                    }

                    $scope.$apply();
                },
                dragend: function () {
                    self = this;
                    $timeout(function () {
                        modified = _.map($scope.map.mexiMarkers, function (marker) {
                            return {
                                latitude: marker.latitude + rndAddToLatLon(),
                                longitude: marker.longitude + rndAddToLatLon()
                            }
                        })
                        $scope.map.mexiMarkers = modified;
                    });
                }
            },
            infoWindow: {
                coords: {
                    latitude: 36.270850,
                    longitude: -44.296875
                },
                options: {
                    disableAutoPan: true
                },
                show: false
            },
            infoWindowWithCustomClass: {
                coords: {
                    latitude: 36.270850,
                    longitude: -44.296875
                },
                options: {
                    boxClass: 'custom-info-window'
                },
                show: true
            },
            templatedInfoWindow: {
                coords: {
                    latitude: 48.654686,
                    longitude: -75.937500
                },
                options: {
                    disableAutoPan: true
                },
                show: true,
                templateUrl: 'assets/templates/info.html',
                templateParameter: {
                    message: 'passed in from the opener'
                }
            },
            polylines: [
                {
                    path: [
                        {
                            latitude: 45,
                            longitude: -74
                        },
                        {
                            latitude: 30,
                            longitude: -89
                        },
                        {
                            latitude: 37,
                            longitude: -122
                        },
                        {
                            latitude: 60,
                            longitude: -95
                        }
                    ],
                    stroke: {
                        color: '#6060FB',
                        weight: 3
                    },
                    editable: true,
                    draggable: false,
                    geodesic: false,
                    visible: true
                },
                {
                    path: [
                        {
                            latitude: 47,
                            longitude: -74
                        },
                        {
                            latitude: 32,
                            longitude: -89
                        },
                        {
                            latitude: 39,
                            longitude: -122
                        },
                        {
                            latitude: 62,
                            longitude: -95
                        }
                    ],
                    stroke: {
                        color: '#6060FB',
                        weight: 3
                    },
                    editable: true,
                    draggable: true,
                    geodesic: true,
                    visible: true
                }
            ]
        },
        toggleColor: function (color) {
            return color == 'red' ? '#6060FB' : 'red';
        }

    });

    _.each($scope.map.markers, function (marker) {
        marker.closeClick = function () {
            marker.showWindow = false;
            $scope.$apply();
        };
        marker.onClicked = function () {
            onMarkerClicked(marker);
        };
    });

    _.each($scope.map.markers2, function (marker) {
        marker.closeClick = function () {
            marker.showWindow = false;
            $scope.$apply();
        };
        marker.onClicked = function () {
            onMarkerClicked(marker);
        };
    });

    $scope.removeMarkers = function () {
        console.info("Clearing markers. They should disappear from the map now");
        $scope.map.markers.length = 0;
        $scope.map.markers2.length = 0;
        $scope.map.dynamicMarkers.length = 0;
        $scope.map.randomMarkers.length = 0;
        $scope.map.mexiMarkers.length = 0;
        $scope.map.polylines.length = 0;
        $scope.map.clickedMarker = null;
        $scope.searchLocationMarker = null;
        $scope.map.infoWindow.show = false;
        $scope.map.templatedInfoWindow.show = false;
        // $scope.map.infoWindow.coords = null;
    };
    $scope.refreshMap = function () {
        //optional param if you want to refresh you can pass null undefined or false or empty arg
        $scope.map.control.refresh({latitude: 32.779680, longitude: -79.935493});
        $scope.map.control.getGMap().setZoom(11);
        return;
    };
    $scope.getMapInstance = function () {
        alert("You have Map Instance of" + $scope.map.control.getGMap().toString());
        return;
    }
    $scope.map.clusterOptionsText = JSON.stringify($scope.map.clusterOptions);
    $scope.$watch('map.clusterOptionsText', function (newValue, oldValue) {
        if (newValue !== oldValue)
            $scope.map.clusterOptions = angular.fromJson($scope.map.clusterOptionsText);
    });

    $scope.$watch('map.doUgly', function (newValue, oldValue) {
        var json;
        if (newValue !== oldValue) {
            if (newValue)
                json = {title: 'Hi I am a Cluster!', gridSize: 60, ignoreHidden: true, minimumClusterSize: 2,
                    imageExtension: 'png', imagePath: 'http://localhost:3000/example/cluster', imageSizes: [72]};
            else
                json = {title: 'Hi I am a Cluster!', gridSize: 60, ignoreHidden: true, minimumClusterSize: 2};
            $scope.map.clusterOptions = json;
            $scope.map.clusterOptionsText = angular.toJson(json);
        }
    });

    $scope.genRandomMarkers = function (numberOfMarkers) {
        genRandomMarkers(numberOfMarkers, $scope);
    };

    $scope.searchLocationMarker = {
        coords: {
            latitude: 40.1451,
            longitude: -99.6680
        },
        options: { draggable: true },
        events: {
            dragend: function (marker, eventName, args) {
                console.log('marker dragend');
                console.log(marker.getPosition().lat());
                console.log(marker.getPosition().lng());
            }
        }
    }
    $scope.onMarkerClicked = onMarkerClicked;

    $scope.clackMarker = function($markerModel) {
        console.log("from clackMarker");
        console.log($markerModel);
    };

    $timeout(function () {
        $scope.map.polylines[0].path.push({
            latitude: 61,
            longitude: -105
        });
        $scope.map.polylines[0].path.push({
            latitude: 70,
            longitude: -105
        });
        $scope.map.infoWindow.show = true;
        var dynamicMarkers = [
            {
                latitude: 46,
                longitude: -79,
                showWindow: false
            },
            {
                latitude: 33,
                longitude: -79,
                showWindow: false
            },
            {
                icon: 'assets/images/plane.png',
                latitude: 35,
                longitude: -127,
                showWindow: false
            }
        ];
        _.each(dynamicMarkers, function (marker) {
            marker.closeClick = function () {
                marker.showWindow = false;
                $scope.$apply();
            };
            marker.onClicked = function () {
                onMarkerClicked(marker);
            };
        });
        $scope.map.dynamicMarkers = dynamicMarkers;
    }, 2000);






    ///////
  
});


// ####################################################################################################################################################### // 
// #########################################               myTeamController               ########################################################### // 
// ####################################################################################################################################################### // 

app.controller('myTeamController', function ($rootScope, $scope,dataFactory, authFactory, AUTH_EVENTS) {

    console.log("Inside myTeam View");
    //Fetch team data 
    $scope.userDetails = $rootScope.userPersonalInfo;
    $scope.myGroupName = $scope.userDetails.GroupName;
    dataFactory.getValues('Rider', true, "grpname=" + $scope.userDetails.GroupName + "&orgname=" + $scope.userDetails.OrganizationName)
                .success(function (values) {
                    $scope.teamData = values;
                })
                .error(function (value) {
                    console.log("error");
                });
});

// ####################################################################################################################################################### // 
// #########################################               dashboardController               ########################################################### // 
// ####################################################################################################################################################### // 


app.controller('dashboardController', function ($rootScope, $scope, dataFactory, AUTH_EVENTS) {
    console.log("Inside dashboard View");
    $scope.routeFlag = false;
    todayVar = new Date();
    $scope.userHistory = {};
    $scope.getHistory = function () {
        dataFactory.getValues('Rides', true, "username=" + $scope.userPersonalInfo.UserName)
                        .success(function (values) {
                            $scope.userHistory = values;
                            console.log($scope.userHistory);
                        })
                        .error(function (value) {
                            console.log("error");
                        });
    }
    $scope.calDates = [15, 10, 2, 3]; // Holds cal days a ride was reported 

    $scope.setToday = function () {
        
        todayVar = new Date();
        varToday = todayVar.getMonth();
        return varToday;

    }

    $scope.calMonth = todayVar.getMonth();
    $scope.popTest = 5;
    $scope.label = "Hello";
    $scope.name = 'World';

    $scope.alarmFromPop = function ($event) {
        console.log($event.target.id);
        $scope.routeFlag = true;
        $scope.popDate = $event.target.parentElement.attributes.name.value;

    };

    $scope.inverseFlag = function () { $scope.routeFlag = false;}


    $scope.init = function () {
        
    }
});


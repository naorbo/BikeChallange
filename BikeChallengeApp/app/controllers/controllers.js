/// <reference path="../Scripts/angular.js" />

// ####################################################################################################################################################### // 
// #########################################                adminConsoleController               ################################################################ // 
// ####################################################################################################################################################### // 

app.controller('adminConsoleController', function ($rootScope, $scope,$modal, $http, $timeout, $location, $upload, dataFactory, authFactory, AUTH_EVENTS, serverBaseUrl, confirm, session) {

    //Load data - init 
    $scope.loadData = function () {
        //Load users
        dataFactory.getValues('Rider')
            .success(function (response) {
                console.log(response);
                $scope.sourceUsers = angular.fromJson(response);
                $scope.users = angular.fromJson(response);
            })
                 .error(function (error) {
                     alert("Unable to fetch all system users...");
                 });
        //Load Events 
        dataFactory.getValues('event/GetEvents')
            .success(function (response) {
                console.log(response);
                $scope.eventsHolder = angular.fromJson(response);
            })
                 .error(function (error) {
                     alert("Unable to fetch all system users...");
                 });
        //Load Cities
        dataFactory.getValues('Cities', false, 0)
            .success(function (values) {
                $scope.citiesHolder = angular.fromJson(values);
                var tmp = [];
                angular.forEach($scope.citiesHolder, function (city) {
                    tmp.push(city.CityName);
                })
                $scope.filterOptions[0].values = tmp;
                
            })

        // Load groups & orgs

        dataFactory.getValues('Organization')
         .success(function (response) {
             console.log(response);
             $scope.organizationsHolder = angular.fromJson(response);
             dataFactory.getValues('Group')
                .success(function (response) {
                    console.log(response);
                    $scope.groupsHolder = angular.fromJson(response);
                    angular.forEach($scope.groupsHolder, function (group) {
                        angular.forEach($scope.organizationsHolder, function (organization) {
                            if (group.Organization == organization.Organization){
                                group['orgName'] = organization.OrganizationName;
                            }
                        })
                    })
                    $scope.filterOptions[1].values = $scope.groupsHolder;
                    var tmp = [];
                    angular.forEach($scope.organizationsHolder, function (org) {
                        tmp.push(org.OrganizationDes);
                    })
                    $scope.filterOptions[4].values = tmp;
                    
                })
                .error(function (error) {
                    alert("Unable to fetch all groups...");
                });
         })
                 .error(function (error) {
                     alert("Unable to fetch all orgs...");
                 });

        //Load Rides
        dataFactory.getValues('Rides', true, "username=")
            .success(function (values) {
                $scope.allRidesHolder = angular.fromJson(values);
            })

        // Load Open Challenges
            dataFactory.getValues('Competition', false)
                .success(function (values) {
                    console.log(values)
                    $scope.openChallengesHolder = angular.fromJson(values);
            });
        

    }

    $scope.loadData();
    
    

    // Accordion vars 
    $scope.oneAtATime = true;

    $scope.status = {
        isFirstOpen: true,
        isFirstDisabled: false
    };

    // Admin nav switch
    $scope.adminNav = {
        'switch' : 1 
    };
   
    // Remove User
    $scope.removeUser = function (userName) {
        confirm("האם אתה בטוח שברצונך למחוק את המשתמש ממאגר הנתונים ? (פעולה זו אינה הפיכה)").then(
                    function (response) {

                        console.log("Confirm accomplished with", response);


                        dataFactory.deleteValues('Rider', 'username='+userName)
                            .success(function (response) {
                                console.log('User deletion succeeded');
                                $scope.loadData();
                            })
                            .error(function (response) {
                                console.log("error deleting user");
                            });



                        
                    },
                    function () {

                        console.log("Confirm failed :(");

                    }
                );



    }


    
    //Remove Group
    $scope.removeGroup = function (groupName, organizationName) {
        confirm("האם אתה בטוח שברצונך למחוק את הקבוצה ממאגר הנתונים ? (פעולה זו אינה הפיכה)").then(
                    function (response) {

                        console.log("Confirm accomplished with", response);

                        // api/Group?grpname=groupname&orgname=organizationanme
                        dataFactory.deleteValues('Group', 'grpname=' + groupName + '&orgname=' + organizationName )
                            .success(function (response) {
                                if (angular.fromJson(response) == 'Error')
                                {
                                    alert('לא ניתן למחוק קבוצה המכילה משתמשים פעילים')
                                }
                                else
                                {
                                    console.log('Group deletion succeeded');
                                    $scope.loadData();
                                }
                                
                            })
                            .error(function (response) {
                                console.log("error deleting group");
                            });




                    },
                    function () {

                        console.log("Confirm failed :(");

                    }
                );
    }

    

    //Remove Organization
    $scope.removeOrganization = function (organizationName) {
        confirm("האם אתה בטוח שברצונך למחוק את הארגון ממאגר הנתונים ? (פעולה זו אינה הפיכה)").then(
                    function (response) {

                        console.log("Confirm accomplished with", response);

                        // api/Organization?orgname=organizationanme
                        dataFactory.deleteValues('Organization', 'orgname=' + organizationName)
                            .success(function (response) {
                                if (angular.fromJson(response) == 'Error') {
                                    alert('לא ניתן למחוק ארגון המכיל קבוצות רשומות')
                                }
                                else {
                                    console.log('Group deletion succeeded');
                                    $scope.loadData();
                                }

                            })
                            .error(function (response) {
                                console.log("error deleting group");
                            });




                    },
                    function () {

                        console.log("Confirm failed :(");

                    }
                );
    }




    // Replace the captain
    // api/Rider?grpname=[The name of the group]&orgname=[The name of the organization] - Not case sensative
    $scope.replaceCaptainFlag = false;
    $scope.demoteCaptain = function (groupName, orgName) {
        $scope.riderPerGroup = []; 
        dataFactory.getValues('Rider', true, 'grpname=' + groupName + '&orgname=' + orgName)
            .success(function (values) {
                $scope.riderPerGroup = angular.fromJson(values);
                $scope.replaceCaptainFlag = true;
            })
                
    }
    
    // Put api/captain?cap_usr=""&new_cap_usr=""
    // cap_usr-"The user name of old captain", new_cap_usr-"The user name of new captain"
    $scope.saveNewCaptain = function (oldCaptain, newCaptain) {
        dataFactory.updateValues('captain', newCaptain, true, 'cap_usr=' + oldCaptain + '&new_cap_usr=' + newCaptain.UserName)
            .success(function (values) {
                if (angular.fromJson(values) == "Error")
                { alert("עדכון נכשל!"); }
                else
                {
                    alert("עדכון הושלם בהצלחה!");
                    $scope.loadData();
                    $scope.replaceCaptainFlag = false;
                }
            })
                .error(function (error) {
                    alert("עדכון נכשל!");
                });
    }
            
        
    //Undo Captain demote

    $scope.undoDemote = function () {
        $scope.replaceCaptainFlag = false;
    }


    


    var genderOptions = [
        'זכר', 'נקבה'
    ];

    var bikeOptions = [
        'הרים', 'כביש', 'חשמליים'
    ];

    $scope.filterOptions = [
        {useName: 'cityFilter', displayName: 'עיר', values: $scope.citiesHolder },
        {useName:'groupFilter', displayName: 'קבוצה'},
        { useName: 'genderFilter', displayName: 'מין', values: genderOptions },
        { useName: 'bikeTypeFilter', displayName: 'סוג אופניים', values: bikeOptions },
        {useName:'orgFilter', displayName: 'ארגון'},
    ]



    // Bike Challenge Events Management 

    $scope.createNewEvent = function (size) {

        var modalInstance = $modal.open({
            templateUrl: 'newEventModalContent.html',
            controller: ModalInstanceCtrl,
            size: size,
            resolve: {
                filterOptions: function () {
                    return $scope.filterOptions;
                }
            }
        });

        modalInstance.result.then(function () {
            $scope.loadData();
        }, function () {
            console.log('Modal dismissed at: ' + new Date());
        });
    };


    var ModalInstanceCtrl = function ($rootScope, $scope, $modalInstance, filterOptions) {

        $scope.filterOptions = filterOptions;
        $scope.cancelTrigger = false;

        $scope.save = function (newEvent) {
            if (!$scope.cancelTrigger) {
                newEventJson = {
                    EventName: newEvent.name.$viewValue,
                    EventDate: newEvent.date.$viewValue,
                    EventTime: newEvent.time.$viewValue,
                    EventType: newEvent.type.$viewValue,
                    EventAddress: newEvent.address.$viewValue,
                    City: newEvent.city.$viewValue,
                    EventDetails: newEvent.description.$viewValue,
                    EventStatus: "Junk"
                }


                dataFactory.postValues('Event', newEventJson, false)
                 .success(function (response) {
                     if (angular.fromJson(response) != "Error") {
                         alert(" האירוע נוצר בהצלחה ! ");
                         $modalInstance.close();

                     }

                     else {
                         alert("שגיאה ביצירת אירוע חדש");
                     }
                 })

                 .error(function (error) {
                     alert("שגיאה ביצירת אירוע חדש");
                 });

            }
        };

        $scope.cancel = function () {
            $scope.cancelTrigger = true;
            $modalInstance.close();
        };
    };


    //Edit BC Event
    $scope.targetEvent = {};
    $scope.editEvent = function (eEvent) {
        $scope.targetEvent = eEvent;
        var modalInstance = $modal.open({
            templateUrl: 'editEventModalContent.html',
            controller: editModalInstanceCtrl,
            
            resolve: {
                filterOptions: function () {
                    return $scope.filterOptions},
                targetEvent: function () {
                    return $scope.targetEvent;
                }}
            
        });

        modalInstance.result.then(function () {
            $scope.loadData();
        }, function () {
            console.log('Modal dismissed at: ' + new Date());
        });
    };


    var editModalInstanceCtrl = function ($rootScope, $scope, $modalInstance, filterOptions, targetEvent) {

        $scope.filterOptions = filterOptions;
        $scope.targetEvent = targetEvent;
        $scope.cancelTrigger = false;

        $scope.cancel = function () {
            $scope.cancelTrigger = true;
            $modalInstance.dismiss('cancel');


        };

       

        $scope.saveChanges = function (newEvent) {
            if (!$scope.cancelTrigger) {
                newEventJson = {
                    EventName: newEvent.name.$viewValue,
                    EventDate: newEvent.date.$viewValue,
                    EventTime: newEvent.time.$viewValue,
                    EventType: newEvent.type.$viewValue,
                    EventAddress: newEvent.address.$viewValue,
                    City: newEvent.city.$viewValue,
                    EventDetails: newEvent.description.$viewValue,
                    EventStatus: "Junk"
                }


                dataFactory.updateValues('Event', newEventJson, true, 'eventname=' + targetEvent.EventName)
                 .success(function (response) {
                     if (angular.fromJson(response) != "Error") {
                         alert(" האירוע עודכן בהצלחה !");
                         $modalInstance.close();

                     }

                     else {
                         alert("שגיאה בעדכון אירוע ");
                     }
                 })

                 .error(function (error) {
                     alert("שגיאה בעדכון אירוע");
                 });


            };

        }
    };




    $scope.removeEvent = function (eventName) {
        confirm("האם אתה בטוח שברצונך למחוק את האירוע ממאגר הנתונים ? (פעולה זו אינה הפיכה)").then(
                    function (response) {

                        console.log("Confirm accomplished with", response);

                       
                        // api/Event?eventname=
                        dataFactory.deleteValues('Event', 'eventname=' + eventName)
                            .success(function (response) {
                                if (angular.fromJson(response) == 'Error') {
                                    alert('שגיאה במחיקת אירוע')
                                }
                                else {
                                    console.log('Event deletion succeeded');
                                    $scope.loadData();
                                }

                            })
                            .error(function (response) {
                                console.log("error deleting event");
                            });




                    },
                    function () {

                        console.log("Confirm failed :(");

                    }
                );
    }
    
    // Display User List per event 
    $scope.displayRegisteredUsers = function (eventName) {
        
    }

    // User list per Event Modal windows
    $scope.displayUsersPerEvent = function (eventName) {
        
        // GET api/event/GetUsers?eventname
        // get all of users from a specific event
        dataFactory.getValues('event/GetUsers', true, 'eventname=' + eventName)
            .success(function (values) {
                console.log(values)
                $scope.userPool = angular.fromJson(values);

                var modalInstance = $modal.open({
                    templateUrl: 'registeredUserModalContent.html',
                    controller: userPoolModalInstanceCtrl,
                    size: 'lg',
                    resolve: {
                        userPool: function () {
                            return $scope.userPool;
                        }
                    }
                });

                modalInstance.result.then(function () {
                    $scope.loadData();
                }, function () {
                    console.log('Modal dismissed at: ' + new Date());
                });
            })

       

        

        var userPoolModalInstanceCtrl = function ($rootScope, $scope, $modalInstance, userPool) {

            $scope.userPool = userPool;
            $scope.cancelTrigger = false;

            $scope.cancel = function () {
                $scope.cancelTrigger = true;
                $modalInstance.close();
            };
        };
    };


    // Admin Reports  
    $scope.sortVar = "UserDisplayName";
    
    // Exporting data

    $scope.export2Excel = function (filteredData) {

        var data2Export = angular.toJson(filteredData);
        

    };

    $scope.export2PDF = function (filteredData) {

        var data2Export = angular.toJson(filteredData);
        

    }

    // Broadcast a message

    $scope.publish = function (contactForm) {

        var contactInfo = {};
        contactInfo = {
            "Subject": contactForm.subject.$viewValue,
            "Body": contactForm.content.$viewValue,
        }

        dataFactory.postValues('PostMessage', contactInfo, false)
             .success(function (response) {
                 alert("ההודעה נשלחה בהצלחה");
                 $location.url("/adminConsole");
             })
             .error(function (error) {
                 alert("שגיאה");
             });

    }

    // End a Challenge

    $scope.shuffle = function (date, hash) {
        var activeChallenge = "01-"
        activeChallenge = activeChallenge.concat(date);

        // Get active month index
        var activeMonthIndx = null;
        for (var i = 0; i < $scope.openChallengesHolder.length; i++) {
            if ($scope.openChallengesHolder[i].$$hashKey == hash) {
                activeMonthIndx = i;
                break;
            }
        }

        dataFactory.getValues('shuffle', true, "date=" + activeChallenge)
                .success(function (values) {
                    console.log(values)
                    $scope.hallOfFame = angular.fromJson(values);
                    angular.forEach($scope.hallOfFame, function (category) {

                        if (category.Category == "Winning Organization") {
                            $scope.openChallengesHolder[activeMonthIndx].OrgWin = category.OrganizationDes;
                            // Set flag to indicate a shuffle was created 
                            $scope.openChallengesHolder[activeMonthIndx].ShuffleFlag = true;
                        }

                        else if (category.Category == "Winning Group") {
                            $scope.openChallengesHolder[activeMonthIndx].GrpWin = category.GroupDes;
                        }

                        else if (category.Category == "BronzeWinner") {
                            $scope.openChallengesHolder[activeMonthIndx].BronzeUser = category.UserFname.concat(" ").concat(category.UserLname);
                            $scope.openChallengesHolder[activeMonthIndx].BronzeUserName = category.UserDes;
                        }

                        else if (category.Category == "SilverWinner") {
                            $scope.openChallengesHolder[activeMonthIndx].SilverUser = category.UserFname.concat(" ").concat(category.UserLname);
                            $scope.openChallengesHolder[activeMonthIndx].SilverUserName = category.UserDes;
                        }

                        else if (category.Category == "GoldWinner") {
                            $scope.openChallengesHolder[activeMonthIndx].GoldUser = category.UserFname.concat(" ").concat(category.UserLname);
                            $scope.openChallengesHolder[activeMonthIndx].GoldUserName = category.UserDes;
                        }

                        else if (category.Category == "PlatinaWinner") {
                            $scope.openChallengesHolder[activeMonthIndx].PlatinumUser = category.UserFname.concat(" ").concat(category.UserLname);
                            $scope.openChallengesHolder[activeMonthIndx].PlatinumUserName = category.UserDes;
                        }
                    })
                });

       
    }

    $scope.saveChallenge = function (date,hash) {

        var activeChallenge = "01-"
        activeChallenge = activeChallenge.concat(date);

        // Get active month index
        var activeMonthIndx = null;
        for (var i = 0; i < $scope.openChallengesHolder.length; i++) {
            if ($scope.openChallengesHolder[i].$$hashKey == hash) {
                activeMonthIndx = i;
                break;
            }
        }

        if ($scope.openChallengesHolder[activeMonthIndx].ShuffleFlag == undefined) {
            alert("טרם הוגרלו מנצחים");
            return;
        }

        // PUT api/Competition?CompetitionDate=
        // {"OrgWin":"","GrpWin":"","PlatinumUser":"","GoldUser":"","SilverUser":"","BronzeUser":""}
        var winningEntities = {

            OrgWin: $scope.openChallengesHolder[activeMonthIndx].OrgWin,
            GrpWin: $scope.openChallengesHolder[activeMonthIndx].GrpWin,
            PlatinumUser: $scope.openChallengesHolder[activeMonthIndx].PlatinumUserName,
            GoldUser:$scope.openChallengesHolder[activeMonthIndx].GoldUserName,
            SilverUser:$scope.openChallengesHolder[activeMonthIndx].SilverUserName,
            BronzeUser:$scope.openChallengesHolder[activeMonthIndx].BronzeUserName
        }


        dataFactory.updateValues('Competition', winningEntities, true, 'CompetitionDate=' + date)
        .success(function (values) {
            if (angular.fromJson(values) == "Error")
            { alert("שגיאה בסגירת תחרות!"); }
            else
            {
                alert("עדכון הושלם בהצלחה!");
                $scope.loadData;
            }
        })

        console.log(winningEntities);
    }

    
    // Upload image handling for Org profile 

    // api/OrganizationImage?OrgName=[orgname]
    $scope.upload = [];
    $scope.fileUploadObj = { testString1: "Test string 1", testString2: "Test string 2" };

    $scope.onFileSelectOrg = function ($files,newOrg) {
        //$files: an array of files selected, each file has name, size, and type.
        for (var i = 0; i < $files.length; i++) {
            var $file = $files[i];
            (function (index) {
                $scope.upload[index] = $upload.upload({
                    url: serverBaseUrl + "/api/OrganizationImage?OrgName=" + newOrg, // webapi url
                    method: "POST",
                    headers: { 'Authorization': 'Bearer ' + session.id },
                    data: { fileUploadObj: $scope.fileUploadObj },
                    file: $file
                }).progress(function (evt) {
                    // get upload percentage
                    console.log('percent: ' + parseInt(100.0 * evt.loaded / evt.total));
                }).success(function (data, status, headers, config) {
                    // file is uploaded successfully
                    console.log(data);
                    $scope.OrgImagePath = data.returnData;
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

    // Create a new organization 
    //{ "Organizationname":"MedaTech", "OrganizationCity": "טירת הכרמל", "OrganizationImage":"[Image Location]" , "OrganizationType":"הייטק"}

    $scope.createNewOrg = function (newOrganization) {
        if ($scope.OrgImagePath == undefined) {
            $scope.OrgImagePath = "\\ProfileImages\\Organizations\\defaultOrg\\defaultOrgImage.jpg";
        }
        orgObject = {
            Organizationname: newOrganization.OrganizationName.$viewValue,
            OrganizationCity: newOrganization.city.$viewValue,
            OrganizationImage: $scope.OrgImagePath,
            OrganizationType: newOrganization.OrganizationType.$viewValue
        }
        dataFactory.postValues('Organization', orgObject, false)
                            .success(function (response) {
                                alert("  הארגון  " + orgObject.Organizationname + "  נוצר בהצלחה ! ");
                                $scope.loadData();
                                $scope.adminNav.switch = 3;
                            })
                            .error(function (error) {
                                
                                alert("שגיאה ביצירת ארגון חדש");
                            });
    }

    $scope.createNewGroup = function (newGroup) {
        
        groupObject = {
            GroupName: newGroup.GroupName.$viewValue,
            OrganizationName: newGroup.organizationName.$viewValue,
        }
        // {"GroupName":"groupName", "OrganizationName":""}
        dataFactory.postValues('Group', groupObject, false)
                            .success(function (response) {
                                alert("  הקבוצה  " + groupObject.GroupName + "  נוצרה בהצלחה ! ");
                                $scope.loadData();
                                $scope.adminNav.switch = 2;
                            })
                            .error(function (error) {
                                alert("שגיאה ביצירת ארגון חדש");
                            });
    }

    $scope.$on('event:auth-loginRequired', function () {
        alert("אינך מורשה לגשת לאיזור זה, אנא התחבר למערכת");
        $location.url("/home")
    });

});


// ####################################################################################################################################################### // 
// #########################################                pastChallengesController               ################################################################ // 
// ####################################################################################################################################################### // 


app.controller('pastChallengesController', function ($rootScope, $scope, $modal, $http, $timeout, $location, $upload, dataFactory, authFactory, AUTH_EVENTS, serverBaseUrl, confirm) {
    
    // Accordion vars 
    $scope.oneAtATime = true;

    $scope.status = {
        isFirstOpen: true,
        isFirstDisabled: false
    };

    // Load Data
    $scope.getChallenges = function () {
        dataFactory.getValues('Competition', true, 'date=')
            .success(function (values) {
                console.log(values)
                $scope.challengesHolder = angular.fromJson(values);
                }); 
    }

    $scope.getChallenges();

    $scope.$on('event:auth-loginRequired', function () {
        alert("אינך מורשה לגשת לאיזור זה, אנא התחבר למערכת");
        $location.url("/home")
    });


});
// ####################################################################################################################################################### // 
// #########################################                contactUsController               ################################################################ // 
// ####################################################################################################################################################### // 


app.controller('contactUsController', function ($rootScope, $scope, $location, $http,dataFactory, authFactory, AUTH_EVENTS, serverBaseUrl) {

    $scope.contactRequest = function (contactForm) {
        
        var contactInfo = {};
        contactInfo = {
            "Name": contactForm.name.$viewValue,
            "Email": contactForm.address.$viewValue,
            "Subject": contactForm.subject.$viewValue,
            "Body": contactForm.content.$viewValue,
        }

        dataFactory.postValues('MailMan', contactInfo, false)
             .success(function (response) {
                 console.log("Contact success");
                 alert("ההודעה נשלחה בהצלחה");
                 $location.url("/home");
             })
             .error(function (error) {
                 alert("שגיאה");
             });

    }


});
// ####################################################################################################################################################### // 
// #########################################                updateProfileController               ################################################################ // 
// ####################################################################################################################################################### // 



app.controller('updateProfileController', function ($rootScope, $scope, $http, $timeout, $upload, dataFactory, authFactory, AUTH_EVENTS, serverBaseUrl) {

    

    // Stores personal details from root scope to a scope var
    
    $scope.updateTempObject = function() {
        $scope.currentDetails = {
            BicycleType: $rootScope.userPersonalInfo.BicycleType,
            BirthDate: $rootScope.userPersonalInfo.BirthDate,
            Gender: $rootScope.userPersonalInfo.Gender,
            GroupDes: $rootScope.userPersonalInfo.GroupDes,
            GroupName: $rootScope.userPersonalInfo.GroupName,
            OrgCity: $rootScope.userPersonalInfo.OrgCity,
            OrganizationDes: $rootScope.userPersonalInfo.OrganizationDes,
            OrganizationName: $rootScope.userPersonalInfo.OrganizationName,
            City: $rootScope.userPersonalInfo.RiderCity,
            UserAddress: $rootScope.userPersonalInfo.UserAddress,
            email: $rootScope.userPersonalInfo.UserEmail,
            UserFname: $rootScope.userPersonalInfo.UserFname,
            UserLname: $rootScope.userPersonalInfo.UserLname,
            UserPhone: $rootScope.userPersonalInfo.UserPhone,
            ImagePath: $rootScope.userPersonalInfo.ImagePath,
            Captain: $rootScope.userPersonalInfo.Captain
        }
    }

    $scope.updateTempObject();

    // Get group members for captain replacement 
    $scope.getTeamMembers = function () {
        dataFactory.getValues('Rider', true, "grpname=" + $scope.currentDetails.GroupName + "&orgname=" + $scope.currentDetails.OrganizationName)
        .success(function (response) {
            console.log(response);
            $scope.groupMembers = response;
        })
             .error(function (error) {
                 alert("Unable to fetch team memebrs...");
             });
    }

    $scope.getTeamMembers();
    
    // Replace captain

    $scope.replaceCaptain = function (newCaptain) {
        dataFactory.updateValues('captain', newCaptain, true, 'cap_usr=' + $scope.currentUser + '&new_cap_usr=' + newCaptain.UserName)
        .success(function (values) {
            if (angular.fromJson(values) == "Error")
            { alert(" בדוק את הפרטים שהזנת ונסה בשנית ,ההרשמה נכשלה!"); }
            else
            {
                alert("עדכון הושלם בהצלחה!");
                dataFactory.getValues('Rider', true, "username=" + $scope.currentUser)
                .success(function (values) {
                    $scope.personalInfoHolder = values[0];
                    $rootScope.userPersonalInfo = values[0];
                    $scope.updateTempObject();
                })
                .error(function (value) {
                    console.log("error");
                });
            }
        })
                     .error(function (error) {
                         alert("עדכון נכשל!");
                     });

    }

    //Update flags for each attribute

    

    $scope.updateFlags = {
        BicycleType: false,
        BirthDate: false,
        Gender: false,
        Group: false,
        Organization: false,
        City: false,
        UserAddress: false,
        email: false,
        UserFname: false,
        UserLname: false,
        UserPhone: false,
        ImagePath: false,
    }
    //flip update flag for each attribute 
    $scope.flipFlag = function (flag) {
        $scope.updateFlags[flag] = !$scope.updateFlags[flag];
    }
    // set coorect update field string
    $scope.updateField = function (type, fieldValue, helperFieldValue) {
        var field = String();
        var helperField = String();
        
        switch (type) {

            case ("email"):
                field = "RiderEmail";
                break;
            case ("UserFname"):
                field = "RiderFname";
                break;
            case ("UserLname"):
                field = "RiderLname"
                break;
            case ("BicycleType"):
                field = "BicycleType"
                break;
            case ("Gender"):
                field = "Gender"
                break;
            case ("BirthDate"):
                field = "BirthDate"
                break;
            case ("UserAddress"):
                field = "RiderAddress"
                break;
            case ("address"):
                field = "RiderAddress"
                break;
            case ("City"):
                field = "City"
                break;
            case ("UserPhone"):
                field = "RiderPhone"
                break;
            case ("Organization"):
                field = "Organization"
                helperField ="Group"
                break;

        }
    
        //create json object
        var updateJson = {}
        if (field == "Organization")
        {
            updateJson[field] = fieldValue;
            updateJson[helperField] = helperFieldValue;
            $scope.personalDetails.org = null;
        }

        else if (field == "City") {
            updateJson[field] = fieldValue.$viewValue.CityName;
        }
        else
        {
            updateJson[field] = fieldValue.$viewValue;
           
        }
      
        dataFactory.updateValues('Rider', updateJson, true, 'username=' + $scope.currentUser )
        .success(function (values) {
            if (angular.fromJson(values) == "Error")
            { alert(" בדוק את הפרטים שהזנת ונסה בשנית ,ההרשמה נכשלה!"); }
            else
            {
                alert("עדכון הושלם בהצלחה!");
                $scope.currentDetails[type] = fieldValue.$viewValue;
                $scope.flipFlag(type);
                dataFactory.getValues('Rider', true, "username=" + $scope.currentUser)
                .success(function (values) {
                    $scope.personalInfoHolder = values[0];
                    $rootScope.userPersonalInfo = values[0];
                    $scope.updateTempObject();
                })
                .error(function (value) {
                    console.log("error");
                });   
            }
        })
                     .error(function (error) {
                         alert("עדכון נכשל!");
                     });
    }

    

    // Organization Handling 

    // Save & Close - Modal Window - Passes selected Org from modal windows to attribute 
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
        console.log("This is the data :" + $scope.orgHolder);

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
        $scope.$$childHead.personalDetails.org = null;
        $scope.$$childHead.personalDetails.team = null;
        return;
    };

    // New Organization Creator 

    $scope.regNewOrg = function (newOrgObj) {

        if (newOrgObj.imagePath == undefined) {
            newOrgObj.imagePath = "\\ProfileImages\\Organizations\\defaultOrg\\defaultOrgImage.jpg";
        }

        var newOrg = {
            OrganizationName: newOrgObj.OrganizationName,
            OrganizationCity: newOrgObj.OrganizationCity.CityName,
            OrganizationType: newOrgObj.OrganizationType,
            OrganizationImage: newOrgObj.imagePath
        };

        dataFactory.getValues("OrganizationExists", 1, "orgname=" + newOrg.OrganizationName)
                    .success(function (response) {
                        console.log(response);
                        if (response == '"NOT EXISTS\"') {
                            console.log("organization is available");

                            dataFactory.postValues('Organization', newOrg, false)
                            .success(function (response) {
                                console.log(response);
                                $scope.newOrgFlag = true;
                                alert("  הארגון  " + newOrg.OrganizationName + "  נוצר בהצלחה ! ");
                                // Closing Modal window 
                                $scope.$$childHead.personalDetails.org = newOrg.OrganizationName;
                                $('#myNewOrgModal').modal('hide');
                            })
                            .error(function (error) {
                                $scope.status = 'Unable to load Orgs data: ' + error.message;
                                alert("שגיאה ביצירת ארגון חדש");
                            });
                        }

                        else {
                            console.log("organization is NA");
                            alert("שם הארגון קיים, אנא בחר שם יחודי או הצטרף לארגון קיים")
                        }
                    })
                     .error(function (error) {
                         console.log("Unable to fetch organization existance");
                     }
                        );




       
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
        dataFactory.postValues('Group', newTeamObj, false)
             .success(function (response) {
                 console.log(response);
                 newTeamObj.OrganizationName = $scope.$$childHead.personalDetails.org;
                 $scope.newTeamFlag = true;
                 alert("  הקבוצה  " + newTeamObj.GroupName + "  נוצרה בהצלחה ! ");
                 // Closing Modal window 
                 //$scope.$$childHead.personalDetails.team = newTeamObj;
                 //$scope.$$childHead.personalDetails.team = newTeamObj.GroupName;
                 $('#myNewTeamModal').modal('hide');
                 $scope.getTeamByOrg(newTeamObj.OrganizationName);
             })
             .error(function (error) {
                 $scope.status = 'Unable to create a new team: ' + error.message;
                 alert("שגיאה ביצירת קבוצה חדשה");
             });
    }

    // Upload image handling for user profile 


    $scope.upload = [];
    $scope.fileUploadObj = { testString1: "Test string 1", testString2: "Test string 2" };

    $scope.onFileSelect = function ($files) {
        //$files: an array of files selected, each file has name, size, and type.
        for (var i = 0; i < $files.length; i++) {
            var $file = $files[i];
            (function (index) {
                $scope.upload[index] = $upload.upload({
                    url: serverBaseUrl +  "/api/UserImage?UserName=" + $scope.personalInfoHolder.UserName, // webapi url
                    method: "POST",
                    headers: { 'Authorization': 'Bearer ' + session.id },
                    data: { fileUploadObj: $scope.fileUploadObj },
                    file: $file
                }).progress(function (evt) {
                    // get upload percentage
                    console.log('percent: ' + parseInt(100.0 * evt.loaded / evt.total));
                }).success(function (data, status, headers, config) {
                    // file is uploaded successfully
                    console.log(data);
                    $scope.$$childHead.personalDetails.imagePath = data.returnData;
                    var updateJson = {
                        ImagePath: data.returnData
                    };

                    dataFactory.updateValues('Rider', updateJson, true, 'username=' + $scope.currentUser)
                            .success(function (values) {
                                if (angular.fromJson(values) == "Error")
                                { alert(" בדוק את הפרטים שהזנת ונסה בשנית ,ההרשמה נכשלה!"); }
                                else
                                {
                                    alert("עדכון הושלם בהצלחה!");
                                    $scope.currentDetails['ImagePath'] = data.returnData;
                                    $scope.flipFlag('ImagePath');
                                    dataFactory.getValues('Rider', true, "username=" + $scope.currentUser)
                                    .success(function (values) {
                                        $scope.personalInfoHolder = values[0];
                                        $rootScope.userPersonalInfo = values[0];
                                        $scope.updateTempObject();
                                    })
                                    .error(function (value) {
                                        console.log("error");
                                    });
                                }
                            })
                                         .error(function (error) {
                                             alert("עדכון נכשל!");
                                         });


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


    // Upload image handling for Org profile 

    // api/OrganizationImage?OrgName=[orgname]
    $scope.upload = [];
    $scope.fileUploadObj = { testString1: "Test string 1", testString2: "Test string 2" };

    $scope.onFileSelectOrg = function ($files) {
        //$files: an array of files selected, each file has name, size, and type.
        for (var i = 0; i < $files.length; i++) {
            var $file = $files[i];
            (function (index) {
                $scope.upload[index] = $upload.upload({
                    url: serverBaseUrl + "/api/OrganizationImage?OrgName=" + $scope.newOrg.OrganizationName, // webapi url
                    method: "POST",
                    headers: { 'Authorization': 'Bearer ' + session.id },
                    data: { fileUploadObj: $scope.fileUploadObj },
                    file: $file
                }).progress(function (evt) {
                    // get upload percentage
                    console.log('percent: ' + parseInt(100.0 * evt.loaded / evt.total));
                }).success(function (data, status, headers, config) {
                    // file is uploaded successfully
                    console.log(data);
                    $scope.newOrg.imagePath = data.returnData;


                    
                    



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
    

    $scope.loadCities();



    // Event Handlers

   

   

    $scope.$on('update-org'), function () {
        $scope.personalDetails.org = $scope.orgSelection;
        console.log($scope.personalDetails.org);
    }
    
    $scope.$on('event:auth-loginRequired', function () {
        alert("אינך מורשה לגשת לאיזור זה, אנא התחבר למערכת");
        $location.url("/home")
    });
   

});

// ####################################################################################################################################################### // 
// #########################################                bikeChallengeController               ################################################################ // 
// ####################################################################################################################################################### // 



app.controller('bikeChallengeController', function ($scope) {
    $scope.oneAtATime = true;

   
    $scope.addItem = function () {
        var newItemNo = $scope.items.length + 1;
        $scope.items.push('Item ' + newItemNo);
    };

    $scope.status = {
        isFirstOpen: true,
        isFirstDisabled: false
    };
});


// ####################################################################################################################################################### // 
// #########################################                changePasswordController               ################################################################ // 
// ####################################################################################################################################################### // 



app.controller('changePasswordController', function ($scope,$location, dataFactory) {
   

    $scope.changeUserPassword = function (passwordRequest) {

        var passwordPackage = {
            oldPassword: passwordRequest.cPassword.$viewValue,
            newPassword: passwordRequest.nPassword.$viewValue,
            confirmPassword: passwordRequest.confirmNewPassword.$viewValue
        }

        dataFactory.postValues('account/ChangePassword', passwordPackage, false)
                     .success(function (values) {
                         alert("שינוי הסיסמא הסתיים בהצלחה!");
                         $location.url("/userProfile");
                     })
                     .error(function (error) {
                         alert("סיסמא נוכחית אינה נכונה, בדוק/בדקי את הפרטים ונסה/י שוב");
                     });
        

    }

    $scope.$on('event:auth-loginRequired', function () {
        alert("אינך מורשה לגשת לאיזור זה, אנא התחבר למערכת");
        $location.url("/home")
    });
});

// ####################################################################################################################################################### // 
// #########################################                forgotPasswordController               ################################################################ // 
// ####################################################################################################################################################### // 



app.controller('forgotPasswordController', function ($scope, $location, dataFactory) {

    $scope.pleaseWait = false;
    $scope.forgotPassword = function (passwordRequest) {

        var resetPackage = {
            EmailAddress: passwordRequest.emailAddress.$viewValue
        }
        $scope.pleaseWait = true;
        dataFactory.postValues('account/ForgotPassword', resetPackage, false)
                     .success(function (values) {
                         alert("איפוס הססמא הסתיים בהצלחה , פרטי החיבור החדשים שלך נשלחו באמצעות דואר אלקטרוני");
                         $scope.pleaseWait = false;
                         $location.url("/home");
                     })
                     .error(function (error) {
                         alert("שגיאה באיפוס הסיסמא");
                     });


    }
    $scope.$on('event:auth-loginRequired', function () {
        alert("אינך מורשה לגשת לאיזור זה, אנא התחבר למערכת");
        $location.url("/home")
    });

});

// ####################################################################################################################################################### // 
// #########################################                homeController               ################################################################ // 
// ####################################################################################################################################################### // 


app.controller('homeController', function ($rootScope, $scope, authFactory, AUTH_EVENTS, serverBaseUrl) {
    
    $scope.isActiveCol = [false, false, false, false, false, false, false];
    $contentLoader = false;
   
    function numberWithCommas(n) {
        var parts = n.toString().split(".");
        return parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",") + (parts[1] ? "." + parts[1] : "");
    }
   
        var myStatsPromise = authFactory.getStats();
        myStatsPromise.then(function (result) {  // this is only run after $http completes
            // Reformatting results 
            $scope.globalStats = result[0];
            $scope.globalStats.NumOfGroups = numberWithCommas($scope.globalStats.NumOfGroups);
            $scope.globalStats.NumOfOrganizations = numberWithCommas($scope.globalStats.NumOfOrganizations);
            //$scope.globalStats.NumOfRides = numberWithCommas($scope.globalStats.NumOfRides);
            $scope.globalStats.NumOfUsers = numberWithCommas($scope.globalStats.NumOfUsers);
            $scope.globalStats.TotalCalories = numberWithCommas($scope.globalStats.TotalCalories);
            $scope.globalStats.TotalCO2 = numberWithCommas($scope.globalStats.TotalCO2);
            $scope.globalStats.fuel = numberWithCommas(Math.ceil(result[0].TotalKM / 11));
            $scope.globalStats.TotalKM = numberWithCommas($scope.globalStats.TotalKM);
            //$scope.globalStats.TotalNumOfDays = numberWithCommas($scope.globalStats.TotalNumOfDays);
            $scope.contentLoader = true;
        })
        
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
// #########################################                loginController               ################################################################ // 
// ####################################################################################################################################################### // 



app.controller('loginController', function ($rootScope, $scope, authFactory, AUTH_EVENTS, serverBaseUrl) {
    
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



app.controller('signUpController', function ($rootScope, $scope, $http, $timeout, $upload, dataFactory, authFactory, AUTH_EVENTS, serverBaseUrl, session) {

    //SignUp function - register the user with the ASP.NET EF
    $scope.signUp = function () {
        console.log("Trying to EF");
        authFactory.register($scope.regDetails.userName.$viewValue, $scope.regDetails.password.$viewValue, $scope.regDetails.confirmPassword.$viewValue).then(function () {
            console.log("Signup successfull (EF), BCing success");
            $rootScope.$broadcast(AUTH_EVENTS.registrationSuccessEF);
            $scope.loadCities();
        }, function () {
            $rootScope.$broadcast(AUTH_EVENTS.registrationFailed);
            console.log("Registration Failure ");
        } 
        )};

    
    //userRegistration - register the new user with BC DB 

  

    $scope.userRegistration = function (personalDetails) {
        
        if (personalDetails.org == undefined || personalDetails.org == null) { alert("לא נבחרה קבוצה, בחר קבוצה ונסה שנית")}
        else {

            var userDetails = {};

            userDetails.RiderEmail = personalDetails.email.$viewValue;
            userDetails.RiderFname = personalDetails.firstName.$viewValue;
            userDetails.RiderLname = personalDetails.lastName.$viewValue;
            userDetails.Gender = personalDetails.gender.$viewValue;
            userDetails.RiderAddress = personalDetails.address.$viewValue;
            userDetails.City = personalDetails.city.$viewValue.CityName;
            userDetails.RiderPhone = personalDetails.phone.$viewValue;
            userDetails.BicycleType = personalDetails.bikeType.$viewValue;
            if (personalDetails.imagePath == undefined)
            { userDetails.ImagePath = "\\ProfileImages\\Users\\defaultUser\\defaultUserImage.jpg" }
            else
            { userDetails.ImagePath = personalDetails.imagePath; }
            userDetails.BirthDate = personalDetails.bDay.$viewValue;
            userDetails.UserName = $scope.regDetails.userName.$viewValue;

            // Captain Flag 
            if ($scope.newOrgFlag || $scope.newTeamFlag) { userDetails.Captain = "1"; }
            else { userDetails.Captain = "0"; };

            userDetails.Organization = personalDetails.org;
            userDetails.Group = personalDetails.team.$viewValue.GroupName;

            userDetails = angular.toJson(userDetails, true);



            // Post to Server

            dataFactory.postValues('Rider', userDetails, false)
                     .success(function (values) {
                         if (angular.fromJson(values) == "Error")
                         { alert(" בדוק את הפרטים שהזנת ונסה בשנית ,ההרשמה נכשלה!"); }
                         else
                         {
                             alert("ההרשמה הסתיימה בהצלחה!");
                             $rootScope.$broadcast(AUTH_EVENTS.registrationSuccess);
                         }

                     })
                     .error(function (error) {
                         alert("ההרשמה נכשלה!");
                     });

        }
        }

    // Organization Handling 

    // Save & Close - Modal Window - Passes selected Org from modal windows to attribute 
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
        $scope.$$childHead.personalDetails.org = null;
        $scope.$$childHead.personalDetails.team = null;
        return;
    };

    // New Organization Creator 

    $scope.regNewOrg = function (newOrgObj) {
        
        if (newOrgObj.imagePath == undefined) {
            newOrgObj.imagePath = "\\ProfileImages\\Organizations\\defaultOrg\\defaultOrgImage.jpg";
        }
        
        var newOrg = {
            OrganizationName: newOrgObj.OrganizationName,
            OrganizationCity: newOrgObj.OrganizationCity.CityName,
            OrganizationType: newOrgObj.OrganizationType,
            OrganizationImage: newOrgObj.imagePath
            
        };
   
        dataFactory.getValues("OrganizationExists", 1, "orgname=" + newOrg.OrganizationName)
                    .success(function (response) {
                        console.log(response);
                        if (response == '"NOT EXISTS\"') {
                            console.log("organization is available");

                            dataFactory.postValues('Organization', newOrg, false)
                            .success(function (response) {
                                console.log(response);
                                $scope.newOrgFlag = true;
                                alert("  הארגון  " + newOrg.OrganizationName + "  נוצר בהצלחה ! ");
                                // Closing Modal window 
                                $scope.$$childHead.personalDetails.org = newOrg.OrganizationName;
                                $('#myNewOrgModal').modal('hide');
                            })
                            .error(function (error) {
                                $scope.status = 'Unable to load Orgs data: ' + error.message;
                                alert("שגיאה ביצירת ארגון חדש");
                            });
                        }

                        else {
                            console.log("organization is NA");
                            alert("שם הארגון קיים, אנא בחר שם יחודי או הצטרף לארגון קיים")
                        }
                    })
                     .error(function (error) {
                         console.log("Unable to fetch organization existance");
                     }
                        );
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
        dataFactory.postValues('Group',newTeamObj, false)
             .success(function (response) {
                 console.log(response);
                 newTeamObj.OrganizationName = $scope.$$childHead.personalDetails.org;
                 $scope.newTeamFlag = true;
                 alert("  הקבוצה  " + newTeamObj.GroupName + "  נוצרה בהצלחה ! ");
                 // Closing Modal window 
                 //$scope.$$childHead.personalDetails.team = newTeamObj;
                 //$scope.$$childHead.personalDetails.team = newTeamObj.GroupName;
                 $('#myNewTeamModal').modal('hide');
                 $scope.getTeamByOrg(newTeamObj.OrganizationName);
             })
             .error(function (error) {
                 $scope.status = 'Unable to create a new team: ' + error.message;
                 alert("שגיאה ביצירת קבוצה חדשה");
             });
    }

    // Upload image handling for user profile 


    $scope.upload = [];
    $scope.fileUploadObj = { testString1: "Test string 1", testString2: "Test string 2" };

    $scope.onFileSelect = function ($files) {
        //$files: an array of files selected, each file has name, size, and type.
        for (var i = 0; i < $files.length; i++) {
            var $file = $files[i];
            (function (index) {
                $scope.upload[index] = $upload.upload({
                    url: serverBaseUrl + "/api/UserImage?UserName=" + $scope.regDetails.userName.$viewValue, // webapi url
                    method: "POST",
                    headers: { 'Authorization': 'Bearer ' + session.id },
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


    // Upload image handling for Org profile 

    // api/OrganizationImage?OrgName=[orgname]
    $scope.upload = [];
    $scope.fileUploadObj = { testString1: "Test string 1", testString2: "Test string 2" };

    $scope.onFileSelectOrg = function ($files) {
        //$files: an array of files selected, each file has name, size, and type.
        for (var i = 0; i < $files.length; i++) {
            var $file = $files[i];
            (function (index) {
                $scope.upload[index] = $upload.upload({
                    url: serverBaseUrl + "/api/OrganizationImage?OrgName=" + $scope.newOrg.OrganizationName, // webapi url
                    method: "POST",
                    headers: { 'Authorization': 'Bearer ' + session.id },
                    data: { fileUploadObj: $scope.fileUploadObj },
                    file: $file
                }).progress(function (evt) {
                    // get upload percentage
                    console.log('percent: ' + parseInt(100.0 * evt.loaded / evt.total));
                }).success(function (data, status, headers, config) {
                    // file is uploaded successfully
                    console.log(data);
                    $scope.newOrg.imagePath = data.returnData;
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
        authFactory.login($scope.regDetails.userName.$viewValue, $scope.regDetails.password.$viewValue).then(function () {
            console.log("Initial Login");
        })
        // Add here disable main form of registration 

    })

    $scope.$on('reg-success', function () {
        //login and redirection to userpage after successfull registration
       // authFactory.login($scope.regDetails.userName.$viewValue, $scope.regDetails.password.$viewValue).then(function () {
            $rootScope.$broadcast(AUTH_EVENTS.loginSuccess);
         //   console.log("Event BC - user logged ");
        }, function () {
            $rootScope.$broadcast(AUTH_EVENTS.loginFailed);
            console.log("Event BC - bad login ");
            $scope.failFlag = true;
        })
    

    $scope.$on('update-org'), function () {
        $scope.personalDetails.org = $scope.orgSelection;
        console.log($scope.personalDetails.org);
    }
    
});

// ####################################################################################################################################################### // 
// #########################################                mainController               ################################################################ // 
// ####################################################################################################################################################### // 



app.controller('mainController', function ($rootScope, $location, $scope, authFactory, dataFactory, session, AUTH_EVENTS, serverBaseUrl) {
    

   

    //User login handling (display user info and redirection)

        $scope.currentUser = null;
        $scope.$on('auth-login-success', function () {
            $scope.currentUser = session.userId;
            dataFactory.getValues('Rider', true, "username=" + $scope.currentUser)
                    .success(function (values) {
                        $scope.personalInfoHolder = values[0];

                        //$scope.personalInfoHolder.ImagePath = $scope.personalInfoHolder.ImagePath.substr(1);

                        $rootScope.userPersonalInfo = values[0];

                       // $rootScope.personalInfoHolder.ImagePath = $rootScope.personalInfoHolder.ImagePath.substr(1);

                        console.log("Fetch user info for " + $scope.currentUser);
                        console.log($scope.personalInfoHolder);
                        if ($scope.personalInfoHolder.Designer) {
                            $location.url("adminConsole");
                        }
                        else
                        {
                            dataFactory.getValues('Group', true, "grpname=" + $rootScope.userPersonalInfo.GroupName + "&orgname=" + $rootScope.userPersonalInfo.OrganizationName)
                            .success(function (values) {
                                $scope.userGroup = values[0];

                                dataFactory.getValues('Organization', true, "orgname=" + $rootScope.userPersonalInfo.OrganizationName)
                                    .success(function (values) {
                                        $scope.userOrg = values[0];
                                        $location.url("/userProfile");
                                    })
                                    .error(function (value) {
                                        console.log("error");

                                        $location.url("/userProfile");
                                    })
                            .error(function (value) {
                                console.log("error");
                            });
                            })
                    .error(function (value) {
                        console.log("error");
                    });
                        }
                          
                    } )});

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

app.controller('userProfileController', function ($rootScope, $location, $scope, $timeout, $http, dataFactory, session, serverBaseUrl) {
    
    console.log($scope.currentUser);
    $scope.userGroupInfo = $rootScope.userGroup;
    
    $scope.userPersonalInfo = $rootScope.userPersonalInfo;
    $rootScope.userStats = [
        {
            personal: {},
            group: {},
            organization: {}
        }
    ];

    $rootScope.ranks = [
        {
            group: {},
            organization: {}
        }
    ];

    $rootScope.globalRanks = [
        {
            users: {},
            groups: {},
            organizations: {}
        }
    ];

    var getGroup = function () {
        dataFactory.getValues('Group', true, "grpname=" + $rootScope.userPersonalInfo.GroupName + "&orgname=" + $rootScope.userPersonalInfo.OrganizationName)
                .success(function (values) {
                    return  values;
                })
                .error(function (value) {
                    console.log("error");
                });
    };

    dataFactory.getValues('Rider', true, "username=" + $scope.currentUser)
                .success(function (values) {
                    $scope.personalInfoHolder = values[0];
                    $rootScope.userPersonalInfo = values[0];
                    $scope.myGroup = getGroup();
                    console.log("Fetch user info for " + $scope.currentUser);
                    console.log($scope.personalInfoHolder);
                })
                .error(function (value) {
                    console.log("error");
                });
    
    dataFactory.getValues('Rides', true, "username=" + $scope.currentUser)
                        .success(function (values) {
                            $rootScope.userHistory = values;
                            console.log($rootScope.userHistory);
                        })
                        .error(function (value) {
                            console.log("error");
                        });

    dataFactory.getValues('Stat', true, "grpname=" + $rootScope.userPersonalInfo.GroupName + "&orgname=" + $rootScope.userPersonalInfo.OrganizationName)
                        .success(function (values) {
                            $rootScope.userStats.group = values;
                            
                        })
                        .error(function (value) {
                            console.log("error");
                        });

    dataFactory.getValues('Stat', true, "username=" + $scope.currentUser)
                        .success(function (values) {
                            $rootScope.userStats.personal = values;

                        })
                        .error(function (value) {
                            console.log("error");
                        });
    dataFactory.getValues('Stat', true, "orgname=" + $rootScope.userPersonalInfo.OrganizationName)
                        .success(function (values) {
                            $rootScope.userStats.organization = values;
                        })
                        .error(function (value) {
                            console.log("error");
                        });
    
    dataFactory.getValues('Ranking', true, "grpname=" + $scope.userPersonalInfo.GroupDes + "&orgname=" + $scope.userPersonalInfo.OrganizationDes + "&gender=&order=Points&date=")
                 .success(function (values) {
                     $rootScope.ranks.group = angular.fromJson(values);

                 })
                 .error(function (error) {
                     console.log("error");
                 });
    
    dataFactory.getValues('Ranking', true, "orgname=" + $scope.userPersonalInfo.OrganizationDes + "&gender=&order=Points&date=")
                 .success(function (values) {
                     $rootScope.ranks.organization = angular.fromJson(values);

                 })
                 .error(function (error) {
                     console.log("error");
                 });

    // Get Global Ranking (Current Challenge)
    var challengeDate = new Date();
    var parsedDay = (challengeDate.getDate()).toString() ;
    if (parsedDay < 10) {parsedDay = ("0").concat(parsedDay)}
    var parsedMonth = (challengeDate.getMonth() + 1).toString();
    if (parsedMonth < 10) { parsedMonth = ("0").concat(parsedMonth) }
    var parsedYear = (challengeDate.getFullYear()).toString();
    concatDate = parsedYear.concat(parsedMonth).concat(parsedDay);

    dataFactory.getValues('Ranking', true, "grpname=&orgname=&gender=&order=Points&date=" + concatDate)
                 .success(function (values) {
                     $rootScope.globalRanks.users = angular.fromJson(values);

                 })
                 .error(function (error) {
                     console.log("error");
                 });
    
    dataFactory.getValues('Ranking', true, "orgname=all&gender=&order=Points&date=" + concatDate)
                 .success(function (values) {
                     $rootScope.globalRanks.groups = angular.fromJson(values);

                 })
                 .error(function (error) {
                     console.log("error");
                 });

   

    dataFactory.getValues('Ranking', true, "gender=&order=Points&date=" + concatDate)
                 .success(function (values) {
                     $rootScope.globalRanks.organizations = angular.fromJson(values);

                 })
                 .error(function (error) {
                     console.log("error");
                 });
    


    $scope.$on('event:auth-loginRequired', function () {
        alert("אינך מורשה לגשת לאיזור זה, אנא התחבר למערכת");
        $location.url("/home")
    });

    
  
});


// ####################################################################################################################################################### // 
// #########################################               myTeamController               ########################################################### // 
// ####################################################################################################################################################### // 

app.controller('myTeamController', function ($rootScope, $scope, dataFactory, authFactory, AUTH_EVENTS, serverBaseUrl) {

    console.log("Inside myTeam View");
    //Fetch team data 
    $scope.userDetails = $rootScope.userPersonalInfo;
    $scope.myGroupName = $scope.userDetails.GroupDes;
    dataFactory.getValues('Rider', true, "grpname=" + $scope.userDetails.GroupName + "&orgname=" + $scope.userDetails.OrganizationName)
                .success(function (values) {
                    $scope.teamData = values;
                })
                .error(function (value) {
                    console.log("error");
                });

    $scope.$on('event:auth-loginRequired', function () {
        alert("אינך מורשה לגשת לאיזור זה, אנא התחבר למערכת");
        $location.url("/home")
    });
});

// ####################################################################################################################################################### // 
// #########################################               dashboardController               ########################################################### // 
// ####################################################################################################################################################### // 


app.controller('dashboardController', function ($rootScope, $scope, $filter, dataFactory, AUTH_EVENTS, serverBaseUrl) {
    console.log("Inside dashboard View");
    
    $scope.refreshCal = false;

    todayVar = new Date(); // Handle active month - initial cal 
    $scope.calMonth = todayVar.getMonth(); // Handle active month - initial cal 
    $scope.calYear = todayVar.getFullYear(); // Handle active month - initial cal 

    $scope.calDates = []; // Used for refreshing the cal after changes 
    
    
    $scope.addRideFlag = false; 
    $scope.routeFlag = false;
   
    $scope.flipRideFlag = function () {
        $scope.addRideFlag = !($scope.addRideFlag);
    };

    // User Stats Holder
    $scope.userStats = [
        {
            personal: {},
            group: {},
            organization: {}
        }
    ];

    // Group / Organization ranking holder 
    $scope.ranks = [
        {
            group: {},
            organization: {}
        }
    ];

    $scope.globalRanks = [
        {
            users: {},
            groups: {},
            organizations: {}
        }
    ];


    // CAL Navigation Buttons 

    $scope.calNabButtons = function (arg) {
        if (arg == -1) {
            $scope.calMonth = $scope.calMonth - 1;
            $scope.refreshCal = !$scope.refreshCal;
           
        }
        else if (arg == 1) {
            $scope.calMonth = $scope.calMonth + 1;
            $scope.refreshCal = !$scope.refreshCal;
           
        }

        else {

            var tmp = new Date();
            $scope.calMonth = tmp.getMonth();
            $scope.refreshCal = !$scope.refreshCal;
           

        }

    }
    


    // Gets & Stores user History and Routes / Stats / Ranking 
    $scope.userStats.personal = $rootScope.userStats.personal;
    $scope.userStats.group = $rootScope.userStats.group;
    $scope.userStats.organization = $rootScope.userStats.organization;
    $scope.ranks.group = $rootScope.ranks.group;
    $scope.ranks.organization = $rootScope.ranks.organization;
    $scope.userHistory = $rootScope.userHistory;
    $scope.getHistory = function () { 
        dataFactory.getValues('Rides', true, "username=" + $scope.userPersonalInfo.UserName)
                        .success(function (values) {
                            $scope.userHistory = values;
                            console.log($scope.userHistory); 
                        })
                        .error(function (value) {
                            console.log("error");
                        });
        dataFactory.getValues('Routes', true, "username=" + $scope.userPersonalInfo.UserName)
                        .success(function (values) {
                            $scope.userRoutes = values;
                            console.log($scope.userRoutes);
                        })
                        .error(function (value) {
                            console.log("error");
                        });
        dataFactory.getValues('Stat', true, "username=" + $scope.userPersonalInfo.UserName)
                        .success(function (values) {
                            $scope.userStats.personal = values;
                            
                        })
                        .error(function (value) {
                            console.log("error");
                        });

        dataFactory.getValues('Stat', true, "grpname=" + $scope.userPersonalInfo.GroupName + "&orgname=" + $scope.userPersonalInfo.OrganizationName)
                        .success(function (values) {
                            $scope.userStats.group = values;

                        })
                        .error(function (value) {
                            console.log("error");
                        });
        dataFactory.getValues('Stat', true, "orgname=" + $scope.userPersonalInfo.OrganizationName)
                        .success(function (values) {
                            $scope.userStats.organization = values;
                        })
                        .error(function (value) {
                            console.log("error");
                        });

        
        dataFactory.getValues('Ranking', true, "grpname=" + $scope.userPersonalInfo.GroupDes + "&orgname=" + $scope.userPersonalInfo.OrganizationDes + "&gender=&order=Points&date=")
                 .success(function (values) {
                     $scope.ranks.group = angular.fromJson(values);
                     
                 })
                 .error(function (error) {
                     console.log("error");
                 });

        dataFactory.getValues('Ranking', true, "orgname=" + $scope.userPersonalInfo.OrganizationDes + "&gender=&order=Points&date=")
                 .success(function (values) {
                     $rootScope.ranks.organization = angular.fromJson(values);

                 })
                 .error(function (error) {
                     console.log("error");
                 });
        // Initialization - Get Global Ranking (Current Challenge)
        var challengeDate = new Date();
        var parsedDay = (challengeDate.getDate()).toString();
        if (parsedDay < 10) { parsedDay = ("0").concat(parsedDay) }
        var parsedMonth = (challengeDate.getMonth() + 1).toString();
        if (parsedMonth < 10) { parsedMonth = ("0").concat(parsedMonth) }
        var parsedYear = (challengeDate.getFullYear()).toString();
        concatDate = parsedYear.concat(parsedMonth).concat(parsedDay);

        dataFactory.getValues('Ranking', true, "grpname=&orgname=&gender=&order=Points&date=" + concatDate)
                     .success(function (values) {
                         $scope.globalRanks.users = angular.fromJson(values);

                     })
                     .error(function (error) {
                         console.log("error");
                     });

        dataFactory.getValues('Ranking', true, "orgname=all&gender=&order=Points&date=" + concatDate)
                     .success(function (values) {
                         $scope.globalRanks.groups = angular.fromJson(values);

                     })
                     .error(function (error) {
                         console.log("error");
                     });


        dataFactory.getValues('Ranking', true, "gender=&order=Points&date=" + concatDate)
                     .success(function (values) {
                         $scope.globalRanks.organizations = angular.fromJson(values);

                     })
                     .error(function (error) {
                         console.log("error");
                     });
        
        dataFactory.getValues('Ranking', true, 'username=' + $scope.currentUser + '&date=' + concatDate)
                .success(function (values) {
                    $scope.relativeRanks = angular.fromJson(values[0]);
                })

                .error(function (error) {
                    console.log("error");
                });

        //get Events data 

        dataFactory.getValues('event/GetEvents')
            .success(function (response) {
                console.log(response);
                $scope.systemEvents = angular.fromJson(response);
            })
                 .error(function (error) {
                     alert("Unable to fetch all system events...");
                 });
        // GET api/event?username=[The name of the organization]
        dataFactory.getValues('event', true, 'username=' + $scope.currentUser)
                .success(function (values) {
                    $scope.userEvents = angular.fromJson(values);
                })
                .error(function (error) {
                    console.log("error");
                });

    }
    
    
    // Get user History per month
    $scope.getRidesPerMonth = function () {
        $scope.getHistory();
        var tempDate = new Date($scope.calYear, $scope.calMonth, 1);

        var month = tempDate.getMonth() +1 ;
        var year = tempDate.getFullYear();

        var monthlyRidesArr = [];

        var yearParsed = new String;
        var monthParsed = new String;
        var dayParsed = new String;

        angular.forEach($scope.userHistory, function (ride) {
            yearParsed = ride.RideDate.substr(0, 4);
            monthParsed = ride.RideDate.substr(5, 2);
            dayParsed = ride.RideDate.substr(8, 2);

            if (yearParsed == year && monthParsed == month) {
                day2push = parseInt(dayParsed);
                if ($.inArray(day2push,monthlyRidesArr) == -1)
                    monthlyRidesArr.push(day2push);
            }

        
               
        });
        
        return monthlyRidesArr;
        
    };


    
    // Trigger from popover - open rides for rider
    $scope.alarmFromPop = function ($event) {
        console.log($event.target.id);
        if ($scope.routeFlag == true) { $scope.routeFlag = false }
        else { $scope.routeFlag = true; };
        $scope.popDate = $event.target.parentElement.parentElement.parentElement.attributes.name.value;
        $rootScope.activeDay = $scope.popDate; // Holds Active day @ root scope var
    };

    // Submit a ride from Existing Route


    $scope.submitRide = function (selectedRoute,$event) {
        var x = new String;
        var newRide = {
            username: $rootScope.userPersonalInfo.UserName,
            routename: selectedRoute.routeName.RouteName,
            ridedate: $event.target.parentElement.parentElement.parentElement.getAttribute('name'),
            roundtrip: selectedRoute.roundTrip
        }
        
        var tempDate = new Date(); 
        var tempStart = new Date(tempDate.getFullYear(), tempDate.getMonth(), 1)
        var myRideDate = new Date(newRide.ridedate);
        
        if (myRideDate <= tempDate && myRideDate >= tempStart) {


            var dataString = "username=" + newRide.username + '&routename=' + newRide.routename + "&ridedate=" + newRide.ridedate + "&roundtrip=" + newRide.roundtrip;

            // api/Rides?username=tester1&routename=[Existing Route Name]&ridedate=01-01-1985&roundtrip=True/False

            dataFactory.postValues('Rides', newRide, true, dataString)
                            .success(function (response) {
                                x = x.concat("#").concat(newRide.ridedate);
                                $(x).popover("hide");
                                $(x).addClass("cal-highlight");
                                $scope.flipRideFlag();
                                $scope.calDates = $scope.getRidesPerMonth()

                            })

                            .error(function (response) {
                                console.log("error");
                            });
        }
        else {
            alert('דיווח נסיעה יכול להתבצע רק בחודש התחרות הפעיל');
        }

    }

    

    $scope.inverseFlag = function () { $scope.routeFlag = false; }


    // Right Hand Dashboard

    $scope.rightDash = {};
    $scope.rightDash.switch = 1;

    // Flags 
    $scope.showMyRoutesFlag = false;
    $scope.flipShowMyRoutesFlag = function () { $scope.showMyRoutesFlag = !$scope.showMyRoutesFlag };
    $scope.addNewRouteFlag = false;
    $scope.flipAddNewRouteFlag = function () { $scope.addNewRouteFlag = !$scope.addNewRouteFlag };
    $scope.addNewSRideFlag = false;
    $scope.flipAddNewSRideFlag = function () { $scope.addNewSRideFlag = !$scope.addNewSRideFlag };

    $scope.addNewRRideFlag = false;
    $scope.flipAddNewRRideFlag = function () { $scope.addNewRRideFlag = !$scope.addNewRRideFlag };

    
    $scope.userStatsFlag = false;
    $scope.flipUserStatsFlag = function () {
        $scope.userStatsFlag = !$scope.userStatsFlag;
        if ($scope.groupStatsFlag || $scope.organizationStatsFlag) {
            $scope.groupStatsFlag = $scope.organizationStatsFlag = false;
        }
    };
    $scope.groupStatsFlag = false;
    $scope.flipGroupStatsFlag = function () {
        $scope.groupStatsFlag = !$scope.groupStatsFlag;
        if ($scope.userStatsFlag || $scope.organizationStatsFlag) {
            $scope.userStatsFlag = $scope.organizationStatsFlag = false;
        }
    };
    $scope.organizationStatsFlag = false;
    $scope.flipOrganizationStatsFlag = function () {
        $scope.organizationStatsFlag = !$scope.organizationStatsFlag;
        if ($scope.userStatsFlag || $scope.groupStatsFlag) {
            $scope.userStatsFlag = $scope.groupStatsFlag = false;
        }
    };

    $scope.showMyRanksFlag = false;
    $scope.flipShowMyRanksFlag = function () { $scope.showMyRanksFlag = !$scope.showMyRanksFlag };
    $scope.showRidersRanksFlag = false;
    $scope.flipShowRidersRanksFlag = function () { $scope.showRidersRanksFlag = !$scope.showRidersRanksFlag };
    $scope.showGroupsRanksFlag = false;
    $scope.flipShowGroupsRanksFlag = function () { $scope.showGroupsRanksFlag = !$scope.showGroupsRanksFlag };
    $scope.showOrganizationsRanksFlag = false;
    $scope.flipShowOrganizationsRanksFlag = function () { $scope.showOrganizationsRanksFlag = !$scope.showOrganizationsRanksFlag };
    $scope.showFutureEventsFlag = false;
    $scope.flipShowFutureEventsFlag = function () { $scope.showFutureEventsFlag = !$scope.showFutureEventsFlag };
    $scope.showUserEventsFlag = false;
    $scope.flipShowUserEventsFlag = function () { $scope.showUserEventsFlag = !$scope.showUserEventsFlag };
    
    // Add a new Route 
    $scope.addNewRoute = function (newRoute) {
        var route = {
            UserName: $rootScope.userPersonalInfo.UserName,
            RouteType: newRoute.type,
            RouteLength: newRoute.lenght,
            Comments: newRoute.comments,
            RouteSource: newRoute.source,
            RouteDestination: newRoute.destination
        }

        dataFactory.postValues('Routes', route, false)
                       .success(function (response) {
                           $scope.getHistory();
                           $scope.newRoute = null;
                           $scope.flipAddNewRouteFlag();
                           console.log(response);
                       })
                       .error(function (response) {
                           console.log("error");
                       });
        
    }

    // Delete a route 

    $scope.deleteRoute = function (route) {

        var routeDelete = {
            userName: $rootScope.userPersonalInfo.UserName,
            routename: route.RouteName
        }

        var par = "username=" + routeDelete.userName + "&routename=" + routeDelete.routename;
        dataFactory.deleteValues('Routes', par)
                       .success(function (response) {
                           $scope.getHistory();
                           $scope.flipShowMyRouteFlag();
                           console.log(response);
                       })
                       .error(function (response) {
                           console.log("error");
                       });

        console.log(route);

    };

    // Stats Handling 
    //  #######################

    // Gauger chart


    // Get user stats - type (calCo2/kmRides ) , period (-1 = since registrating , 0 = specific month) ,  (month, year) 
    $scope.statSelector = -1;
    
    $scope.progressStats = function () {

        var kmSummed = 0;
        var ridesSummed = 0;
        var fuelSummed = 0;
        var moneySummed = 0;
        angular.forEach(rawStats, function (monthStat) {
            if (entity == "personal") {
                kmSummed = kmSummed + monthStat.User_KM;
                ridesSummed = ridesSummed + monthStat.Num_of_Rides;
            }
        })}

    $scope.getStats = function (entity, type, period, month, year){ 

        var rawStats = {};
        if (entity == "personal")
        {  rawStats = $scope.userStats.personal; }
        else if (entity == "group")
        {  rawStats = $scope.userStats.group; }
        else
        {  rawStats = $scope.userStats.organization; }
        
        
        if (period == -1) {
            if (type == "calCo2") {

                var co2Summed = 0;
                var calSummed = 0;

                angular.forEach(rawStats, function (monthStat) {
                    if (entity == "personal")
                    {
                        co2Summed = co2Summed + monthStat.User_CO2_Kilograms_Saved;
                        calSummed = calSummed + monthStat.User_Calories;
                    }
                    else if (entity == "group")
                    {
                        co2Summed = co2Summed + monthStat.Group_CO2_Kilograms_Saved;
                        calSummed = calSummed + monthStat.Group_Calories;
                    }
                    else
                    {
                        co2Summed = co2Summed + monthStat.Organization_CO2_Kilograms_Saved;
                        calSummed = calSummed + monthStat.Organization_Calories;
                    }
                    
                });

                calSummed = Math.round(calSummed * 100) / 100;
                co2Summed = Math.round(co2Summed * 100) / 100;
                var statArray = [
                    ['Label', 'Value'],
                    ['קלוריות', calSummed],
                    ['CO2', co2Summed],
                ];

                return statArray;
            }
            if (type == "kmRides") {

                var kmSummed = 0;
                var ridesSummed = 0;
                var fuelSummed = 0;
                var moneySummed = 0;
                angular.forEach(rawStats, function (monthStat) {
                    if (entity == "personal") {
                        kmSummed = kmSummed + monthStat.User_KM;
                        ridesSummed = ridesSummed + monthStat.Num_of_Rides;
                    }
                    else if (entity == "group") {
                        kmSummed = kmSummed + monthStat.Group_KM;
                        ridesSummed = ridesSummed + monthStat.Num_of_Rides;
                    }
                    else {
                        kmSummed = kmSummed + monthStat.Organization_KM;
                        ridesSummed = ridesSummed + monthStat.Num_of_Rides;
                    }

                   
                });

                fuelSummed = Math.ceil(kmSummed / 11);
                moneySummed = Math.ceil(fuelSummed * 7.5);
                if (entity == "personal") {
                    $scope.userKmRides = {
                        km: kmSummed,
                        rides: ridesSummed,
                        fuel: fuelSummed.toString(),
                        money: moneySummed.toString()
                    };
                }
                else if (entity == "group") {
                    $scope.groupKmRides = {
                        km: kmSummed,
                        rides: ridesSummed,
                        fuel: fuelSummed,
                        money: moneySummed
                    };
                }
                else {
                    $scope.organizationKmRides = {
                        km: kmSummed,
                        rides: ridesSummed,
                        fuel: fuelSummed,
                        money: moneySummed
                    };
                }
                

            }
        }
        else {

            var statDate = new Date(year, month, 1);
            var monthParsed = statDate.getMonth() + 1;
            var yearParsed = statDate.getFullYear();
            if (type == "calCo2") {

                var co2Summed = 0;
                var calSummed = 0;
                

                angular.forEach(rawStats, function (monthStat) {

                    if (entity == "personal") {
                        if (monthParsed == monthStat.Month && yearParsed == monthStat.Year) {
                            co2Summed = co2Summed + monthStat.User_CO2_Kilograms_Saved;
                            calSummed = calSummed + monthStat.User_Calories;
                        }
                    }
                    else if (entity == "group") {
                        if (monthParsed == monthStat.Month && yearParsed == monthStat.Year) {
                            co2Summed = co2Summed + monthStat.Group_CO2_Kilograms_Saved;
                            calSummed = calSummed + monthStat.Group_Calories;
                        }
                    }
                    else {
                        if (monthParsed == monthStat.Month && yearParsed == monthStat.Year) {
                            co2Summed = co2Summed + monthStat.Organization_CO2_Kilograms_Saved;
                            calSummed = calSummed + monthStat.Organization_Calories;
                        }
                    }
               
                });

                calSummed = Math.round(calSummed * 100) / 100;
                co2Summed = Math.round(co2Summed * 100) / 100;

                var statArray = [
                    ['Label', 'Value'],
                    ['קלוריות', calSummed],
                    ['CO2', co2Summed],
                ];

                return statArray;
            }
            if (type == "kmRides") {

                var kmSummed = 0;
                var ridesSummed = 0;
                var fuelSummed = 0;
                var moneySummed = 0;
                angular.forEach(rawStats, function (monthStat) {
                    if (entity == "personal") {
                        if (monthParsed == monthStat.Month && yearParsed == monthStat.Year) {
                            kmSummed = kmSummed + monthStat.User_KM;
                            ridesSummed = ridesSummed + monthStat.Num_of_Rides;
                        }
                    }
                    else if (entity == "group") {
                        if (monthParsed == monthStat.Month && yearParsed == monthStat.Year) {
                            kmSummed = kmSummed + monthStat.Group_KM;
                            ridesSummed = ridesSummed + monthStat.Num_of_Rides;
                        }
                    }
                    else {
                        if (monthParsed == monthStat.Month && yearParsed == monthStat.Year) {
                            kmSummed = kmSummed + monthStat.Organization_KM;
                            ridesSummed = ridesSummed + monthStat.Num_of_Rides;
                        }
                    }


                       
                });
            }

                
        };
        fuelSummed = Math.ceil(kmSummed / 11);
        moneySummed = Math.ceil(fuelSummed * 7.5);
        if (entity == "personal") {
       
            $scope.userKmRides = {
                km: kmSummed,
                rides: ridesSummed,
                fuel: fuelSummed,
                money: moneySummed
            }
        }
        else if (entity == "group") {
            $scope.groupKmRides = {
                km: kmSummed,
                rides: ridesSummed,
                fuel: fuelSummed,
                money: moneySummed
            }
        }
        else {
            $scope.organizationKmRides = {
                km: kmSummed,
                rides: ridesSummed,
                fuel: fuelSummed,
                money: moneySummed
            }
        }
    
        
    };

    // Get user Ranks - period (-1 = since registrating , 0 = specific month) ,  (month, year) 
    $scope.getRanks = function (entity, period, month, year) {
        if (entity == "personal") { }
        else if (entity == "group") {
            var rawRanks = $scope.ranks.group;
            if (period == -1) {
                var summedArrPerUser = [];
                summedArrPerUser.push(['רוכב','ק"מ']);
                angular.forEach(rawRanks, function (entry) {
                    var match = 0;
                    for (var i = 0; i < summedArrPerUser.length; i++) {
                        if (entry.UserDisplayName == summedArrPerUser[i][0]) {
                            summedArrPerUser[i][1] = summedArrPerUser[i][1] + entry.User_KM;
                            match = 1;
                        } 
                    }
                    if (match == 0) { summedArrPerUser.push([entry.UserDisplayName, entry.User_KM]); }
                });
                return summedArrPerUser;
            }
            else if (period == 0) {
                var summedArrPerUser = [];
                var statDate = new Date(year, month, 1);
                monthParsed = statDate.getMonth() + 1;
                yearParsed = statDate.getFullYear();
                summedArrPerUser.push(["רוכב", "קמ"]);
                angular.forEach(rawRanks, function (entry) {
                    var match = 0;
                    for (var i = 0; i < summedArrPerUser.length; i++) {
                        if (entry.UserDisplayName == summedArrPerUser[i][0]) {
                            if (monthParsed == entry.Month && yearParsed == entry.Year) {
                                summedArrPerUser[i][1] = summedArrPerUser[i][1] + entry.User_KM;
                                match = 1;
                            }
                        }
                    }
                    if (match == 0) {
                        if (monthParsed == entry.Month && yearParsed == entry.Year) {
                            summedArrPerUser.push([entry.UserDisplayName, entry.User_KM]);
                        }
                    }
                });
                return summedArrPerUser;
            }}
        else if (entity == "organization") {
            var rawRanks = $scope.ranks.organization;
            if (period == -1) {
                var summedArrPerGroup = [];
                summedArrPerGroup.push(['קבוצה','ק"מ']);
                angular.forEach(rawRanks, function (entry) {
                    var match = 0;
                    for (var i = 0; i < summedArrPerGroup.length; i++) {
                        if (entry.GroupDes == summedArrPerGroup[i][0]) {
                            summedArrPerGroup[i][1] = summedArrPerGroup[i][1] + entry.Group_KM;
                            match = 1;
                        }
                    }
                    if (match == 0) { summedArrPerGroup.push([entry.GroupDes, entry.Group_KM]); }
                });
                return summedArrPerGroup;
            }
            else if (period == 0) {
                var summedArrPerGroup = [];
                var statDate = new Date(year, month, 1);
                monthParsed = statDate.getMonth() + 1;
                yearParsed = statDate.getFullYear();
                summedArrPerGroup.push(['קבוצה','ק"מ']);
                angular.forEach(rawRanks, function (entry) {
                    var match = 0;
                    for (var i = 0; i < summedArrPerGroup.length; i++) {
                        if (entry.GroupDes == summedArrPerGroup[i][0]) {
                            if (monthParsed == entry.Month && yearParsed == entry.Year) {
                                summedArrPerGroup[i][1] = summedArrPerGroup[i][1] + entry.Group_KM;
                                match = 1;
                            }
                        }
                    }
                    if (match == 0) {
                        if (monthParsed == entry.Month && yearParsed == entry.Year) {
                            summedArrPerGroup.push([entry.GroupDes, entry.Group_KM]);
                        }

                    }
                });
                return summedArrPerGroup;
            }
        }
    };


    // Add a spontanues ride 
    $scope.addNewSpontanicRide = function (newRide) {

        var tempDate = new Date();
        var tempStart = new Date(tempDate.getFullYear(), tempDate.getMonth(), 1)
        var myRideDate = new Date(newRide.date);

        if (myRideDate <= tempDate && myRideDate >= tempStart) {

            var newSRide = {
                UserName: $rootScope.userPersonalInfo.UserName,
                RideType: newRide.type,
                RideLength: newRide.lenght,
                RideSource: newRide.source,
                RideDate: newRide.date,
                RideDestination: newRide.destination
            }
            dataFactory.postValues('Rides', newSRide, false)
                                    .success(function (response) {
                                        $scope.addNewSRideFlag = false;
                                        $scope.getHistory();
                                        console.log(response);
                                        $("#" + newRide.date).addClass("cal-highlight");
                                    })
                                    .error(function (response) {
                                        console.log("error");
                                    });
        }
        else {
            alert('דיווח נסיעה יכול להתבצע רק בחודש התחרות הפעיל');
        }
    }



    // Add a ride from menu's route

    $scope.addNewRRideMenu = function (newRide) {
        var newRide = {
            username: $rootScope.userPersonalInfo.UserName,
            routename: newRide.routeName.RouteName,
            ridedate: newRide.date,
            roundtrip: newRide.roundTrip
        }

        var tempDate = new Date();
        var tempStart = new Date(tempDate.getFullYear(), tempDate.getMonth(), 1)
        var myRideDate = new Date(newRide.ridedate);

        if (myRideDate <= tempDate && myRideDate >= tempStart) {


            var dataString = "username=" + newRide.username + '&routename=' + newRide.routename + "&ridedate=" + newRide.ridedate + "&roundtrip=" + newRide.roundtrip;

            // api/Rides?username=tester1&routename=[Existing Route Name]&ridedate=01-01-1985&roundtrip=True/False

            dataFactory.postValues('Rides', newRide, true, dataString)
                            .success(function (response) {
                                $scope.addNewRRideFlag = false;
                                $scope.getHistory();
                                console.log(response);
                                $("#" + newRide.ridedate).addClass("cal-highlight");
                            })
                                    .error(function (response) {
                                        console.log("error");
                                    });
        }
        else {
            alert('דיווח נסיעה יכול להתבצע רק בחודש התחרות הפעיל');
        }

    }
 
    // Bike Challnege rating controller 

    // Display Month holder 
    var monthNames = ['ינואר', 'פברואר', 'מרץ', 'אפריל', 'מאי', 'יוני', 'יולי', 'אוגוסט', 'ספטמבר', 'אוקטובר', 'נובמבר', 'דצמבר'];
    $scope.displayMonth = monthNames[$scope.calMonth];




    // Register to an Event 
    // POST insert rider into an event 
    // api/Event?eventname=&username=
    $scope.registerEvent = function (userEvent) {
        
        dataFactory.postValues('Event', userEvent , true, 'eventname=' + userEvent.EventName + '&username=' + $scope.currentUser)
                     .success(function (response) {
                         if (angular.fromJson(response) != "Error") {
                             alert("ההרשמה הושלמה בהצלחה!");
                             $scope.getHistory();
                             $scope.flipShowFutureEventsFlag();
                             $scope.flipShowUserEventsFlag();
                         }
                         else {
                             alert("שגיאה במהלך ההרשמה לאירוע");
                         }
                     })
                     .error(function (error) {
                         alert("שגיאה במהלך ההרשמה לאירוע");
                     });
    }
    // api/Event?usernme=
    $scope.cancleEventRegistration = function (userEvent) {
        dataFactory.deleteValues('Event', 'username=' + $scope.currentUser + '&eventname=' + userEvent.EventName)
                           .success(function (response) {
                               alert('ביטול ההרשמה הסתיים בהצלחה!')
                               $scope.getHistory();
                           })
                           .error(function (response) {
                               alert('שגיאה בביטול ההרשמה')
                           });
    }



    // Controller init 
    $scope.init = function () {       
    }

    $scope.$on('event:auth-loginRequired', function () {
        alert("אינך מורשה לגשת לאיזור זה, אנא התחבר למערכת");
        $location.url("/home")
    });



});


/// <reference path="../Scripts/angular.js" />


// ###############################################################
// iCheck Directive - manipulating the radio/checkbox to square UI    
// ###############################################################

app.directive('icheck', function ($timeout, $parse) {
    return {
        require: 'ngModel',
        link: function($scope, element, $attrs, ngModel) {
            return $timeout(function() {
                var value;
                value = $attrs['value'];

                $scope.$watch($attrs['ngModel'], function(newValue){
                    $(element).iCheck('update');
                })

                return $(element).iCheck({
                    checkboxClass: 'icheckbox_square-blue',
                    radioClass: 'iradio_square-blue'

                }).on('ifChanged', function(event) {
                    if ($(element).attr('type') === 'checkbox' && $attrs['ngModel']) {
                        $scope.$apply(function() {
                            return ngModel.$setViewValue(event.target.checked);
                        });
                    }
                    if ($(element).attr('type') === 'radio' && $attrs['ngModel']) {
                        return $scope.$apply(function() {
                            return ngModel.$setViewValue(value);
                        });
                    }
                });
            });
        }
    }
});

// #############################
// Password match validator 
// #############################
app.directive('validPasswordC', function () {
    return {
        require: 'ngModel',
        link: function ($scope, $element, $attrs, $ctrl) {
            $ctrl.$parsers.unshift(function (viewValue) {
                console.log('inside dir');
                //console.log($scope.regDetails.password.viewValue);
                //console.log($scope.regDetails.passwordConfirm.$viewValue);
                var noMatch = viewValue != $scope.regDetails.password.$viewValue;
                $ctrl.$setValidity('noMatch', !noMatch);
               // console.log(!noMatch);
            });
        }
    }
});

// #############################
// Password match validator for ChangePassword
// #############################
app.directive('updateValidPasswordC', function () {
    return {
        require: 'ngModel',
        link: function ($scope, $element, $attrs, $ctrl) {
            $ctrl.$parsers.unshift(function (viewValue) {
                console.log('inside dir');                
                var noMatch = viewValue != $scope.changePassword.nPassword.$viewValue;
                $ctrl.$setValidity('noMatch', !noMatch);
            });
        }
    }
});



// #############################
// File upload directive 
// #############################

app.directive('fileUpload', function () {
    return {
        scope: true,        //create a new scope
        link: function (scope, el, attrs) {
            el.bind('change', function (event) {
                var files = event.target.files;
                //iterate files since 'multiple' may be specified on the element
                for (var i = 0; i < files.length; i++) {
                    //emit event upward
                    scope.$emit("fileSelected", { file: files[i] });
                }
            });
        }
    };
});


// #############################
// User name existance directive 
// #############################


app.directive('usernameValidate', function (dataFactory) {
    return {
        require: 'ngModel',
        link: function ($scope, $element, $attrs, $ctrl) {
            $ctrl.$parsers.unshift(function (viewValue){
                console.log("Got the Direc");
                var shortUserName = true;
                var Unique = true;
             
             
                if (!(/^[A-Za-z0-9]*$/.test($scope.regDetails.userName.$viewValue))) {
                    $ctrl.$setValidity('exp', false);
                }
                else
                {
                    $ctrl.$setValidity('exp', true);
                }
                if ($scope.regDetails.userName.$viewValue.length > 5) {
                        $ctrl.$setValidity('shorti', shortUserName);
                        dataFactory.getValues("UserNameExists", 1, "username=" + $scope.regDetails.userName.$viewValue)
                        .success(function (response) {
                            console.log(response);
                            if (response == '"NOT EXISTS\"')
                            {
                                console.log("Usename is available");
                                $ctrl.$setValidity('unique', true);
                            }

                            else
                            {
                                console.log("Username is NA");
                                $ctrl.$setValidity('unique', false);
                            }
                        })
                         .error(function (error) {
                             console.log("Unable to fetch username existance");
                         }
                            );
                    }

                    else {
                        $ctrl.$setValidity('shorti', !shortUserName);
                        console.log($scope.regDetails.userName.$error.shorti)
                    };
            }
        )}
    };
});

// #############################
// User email existance directive 
// #############################


app.directive('emailValidate', function (dataFactory) {
    return {
        require: 'ngModel',
        link: function ($scope, $element, $attrs, $ctrl) {
            $ctrl.$parsers.push(function (viewValue) {
                $ctrl.$setValidity('emailValidate', true);
                if($ctrl.$valid) {
                    $ctrl.$setValidity('checkingEmail', false);
                    console.log("Got the Direc");
                
               
                    dataFactory.getValues("UserNameExists", 1, "email=" + $scope.personalDetails.email.$viewValue)
                    .success(function (response) {
                        console.log(response);
                        if (response == '"NOT EXISTS\"') {
                            console.log("email is available");
                            $ctrl.$setValidity('unique', true);
                            $ctrl.$setValidity('checkingEmail', true);
                        }

                        else {
                            console.log("email is NA");
                            $ctrl.$setValidity('unique', false);
                            $ctrl.$setValidity('checkingEmail', false);
                        }
                    })
                     .error(function (error) {
                         console.log("Unable to fetch email existance");
                     }
                        );
                }
            }
        )
        }
    };
    return viewValue;
});

// #############################
// Organization existance directive 
// #############################

// GET api/OrganizationExists?orgname=
app.directive('organizationValidate', function (dataFactory) {
    return {
        require: 'ngModel',
        link: function ($scope, $element, $attrs, $ctrl) {
            $ctrl.$parsers.push(function (viewValue) {
               // $ctrl.$setValidity('organizationValidate', true);
                if ($ctrl.$valid) {
                    $ctrl.$setValidity('checkingOrganization', false);
                    console.log("Got the Direc");


                    dataFactory.getValues("OrganizationExists", 1, "orgname=" + $scope.newOrg)
                    .success(function (response) {
                        console.log(response);
                        if (response == '"NOT EXISTS\"') {
                            console.log("organization is available");
                            $ctrl.$setValidity('unique', true);
                            $ctrl.$setValidity('checkingOrganization', true);
                        }

                        else {
                            console.log("organization is NA");
                            $ctrl.$setValidity('unique', false);
                            $ctrl.$setValidity('checkingOrganization', false);
                        }
                    })
                     .error(function (error) {
                         console.log("Unable to fetch organization existance");
                     }
                        );
                }
            }
        )
        }
    };
    return viewValue;
});

// ############################
// Calendar dashboard directive 
// ############################

app.directive('calendar', ['$compile', function ($compile, $watch, $scope, attrs, dataFactory) {

    var monthNames = ['ינואר', 'פברואר', 'מרץ', 'אפריל', 'מאי', 'יוני', 'יולי', 'אוגוסט', 'ספטמבר', 'אוקטובר', 'נובמבר', 'דצמבר'];
    var days = ['ראשון', 'שני', 'שלישי', 'רביעי', 'חמישי', 'שישי', 'שבת'];

    var isLeapYear = function (year) {
        return ((year % 4 === 0) && (year % 100 !== 0)) || (year % 400 === 0);
    };

    var daysInMonth = function (date) {
        return [31, (isLeapYear(date.getYear()) ? 29 : 28), 31, 30, 31, 30, 31, 31, 30, 31, 30, 31][date.getMonth()];
    };

    var formatDateHeading = function (date) {
        var m = monthNames[date.getMonth()];
        return m.charAt(0).toUpperCase() + m.slice(1) + ' ' + date.getFullYear();
    };

    var currentDate = new Date();

    function getTemplate(month, year, dates) {

        var month = ((isNaN(month) || month == null) ? currentDate.getMonth() + 1 : month) - 1,
          year = (isNaN(year) || year == null) ? currentDate.getFullYear() : year,
          firstDay = new Date(year, month, 1),
          startDay = firstDay.getDay(),
          monthLength = daysInMonth(firstDay),
          heading = formatDateHeading(firstDay);

       // if (!dates || !dates.length) { dates = [currentDate.getDate()] };

        var tpl = [
          '<div class="cal">',
          '<table class="cal" id="dashCal">',
          '<tr><th colspan="7">' + heading + '</th></tr>',
          '<tr>'];

        days.forEach(function (day) {
            tpl.push('<td class="cal-head cellCal">' + day.toUpperCase() + '</td>');
        });
        tpl.push('</tr>');

        var day = 1,
          rows = Math.ceil((monthLength + startDay) / 7);

        for (i = 0; i < rows; i++) {
            var row = ['<tr>'];
            for (j = 0; j < 7; j++) {
                row.push('<td class="cellCal">');
                if (day <= monthLength && (i > 0 || j >= startDay)) {
                    if (day < 10) { dayZeroPrefix = "0" };
                    if (month < 10) { monthZeroPrefix = "0" };
                    if (parseInt(currentDate.getDate()) == day && parseInt(currentDate.getMonth()) == month) {
                        row.push('<div  data-close-popovers class="cal-highlight-today cal-day closepopper" data-daily-sum   id="' + year + '-' + monthZeroPrefix + '' + (month + 1) + '-' + dayZeroPrefix + '' + day + '">'); // cal-day class attrb was removed ** 
                    }
                    else if  
                    (dates.indexOf(day) != -1) { row.push('<div  data-close-popovers class="cal-day cal-highlight closepopper" data-daily-sum  id="' + year + '-' + monthZeroPrefix + '' + (month + 1) + '-' + dayZeroPrefix + '' + day + '">');}
                    else if (dates.indexOf(day) == -1) {row.push('<div  data-close-popovers  class="cal-day closepopper" data-daily-sum  id="' + year + '-' + monthZeroPrefix + '' + (month + 1) + '-' + dayZeroPrefix + '' + day + '">');
                    }
                    row.push(day + '</div>');
                    day++;
                    dayZeroPrefix = "";
                    monthZeroPrefix = "";
                }

                
                row.push('</td>');
            }
            row.push('</tr>');

            tpl.push(row.join(''));
        }
        tpl.push('</table></div>');

        return tpl.join('');
    }

    return {
        restrict: 'A',
        replace: true,
        scope: true,

        link:
            function ($scope, $element, attrs, $watch, dataFactory) {
                
                $scope.$watch(function () {
                    return ($scope.refreshCal);
                }, function () {
                    $element.html(getTemplate(parseInt($scope.calMonth) + 1, parseInt(attrs.year), $scope.getRidesPerMonth()));
                    $compile($element.contents())($scope);
                    console.log("Inside Dir");
                    $scope.getHistory();

                }, true);
   
            }

                //$scope.$watch('calMonth', function () {

                //    $element.html(getTemplate(parseInt($scope.calMonth) + 1, parseInt(attrs.year), $scope.getRidesPerMonth() /*$scope.calDates*/));
                //    $compile($element.contents())($scope);
                //    console.log("Inside Dir");
                //    $scope.getHistory();

                //});

                
            
    }
}]);

app.directive('dailySum', function ($compile, $templateCache) {
    var getTemplate = function (contentType) {
        var template = '';
        switch (contentType) {
            case 'embdHTML':
                template = $templateCache.get("popoverTemplate.html");
                break;
        }
        return template;
    }
    return {
        restrict: 'A',
        link: function (scope, element, attrs, compile) {
            var wrapDivStart = '<div data-toggle="popover" name=' + attrs.id + '>';
            var wrapDivEnd = '</div>';
            var bodyContent = getTemplate("embdHTML");
            var popOverContent = wrapDivStart.concat(bodyContent).concat(wrapDivEnd);

            popOverContent = $compile(popOverContent)(scope);
            var options = {
                title: '<button class="btn close pull-left"  id="close" onclick="angular.element(&quot;#' + attrs.id + '&quot;).popover(&quot;hide&quot;)">&times;</button><span class="pull-right">  ' + attrs.id + '  -  סיכום יומי </span> ',
                //title: '<button class="btn close pull-left"  id="close" onclick="angular.element(&quot;#' + attrs.id + '&quot;).popover(&quot;hide&quot;)">&times;</button><div class="pull-right" ng-model=' + attrs.id + '>  ' + attrs.id + '  -  סיכום יומי </div> ',
                content: popOverContent,
                placement: "right",
                html: true,
                date: scope.date
            };

            $(element).popover(options);
        }
    };
});


app.directive('closePopovers', function ($document, $rootScope, $timeout) {
    return {
        restrict: 'EA',
        link: function ($scope, scope, element, attrs) {
            $document.on('click', function (ev) {

                var targetElem = angular.element(ev.target);

                if (targetElem.data('toggle') !== 'popover'
                && targetElem.parents('[data-toggle="popover"]').length === 0
                && targetElem.parents('.popover').length === 0) {
                    if ($('.popover').length > 1) {
                        $('.popover').each(function () {
                            $scope.inverseFlag();
                            if ($scope.addRideFlag) {
                                $scope.flipRideFlag()
                            }
                            var $this = $(this);
                            var scope = $this.scope();
                            var popIdentity = $this.children(".popover-content").children(".ng-scope").attr('name').valueOf();
                            var x = new String;
                            x = x.concat("#").concat(popIdentity);
                            scope.$apply(function () {
                                $(x).popover("hide");
                            });
                        }
                        );
                    }
                }
            });
        }
    };
});




// Google Charts 
// Dashbord gauger charts - for COs / KM 

app.directive('chartPersonal', function () {
    return {
        restrict: 'A',
        scope: true,
        link: function ($scope, $elm, $attr, $watch) {
            // Create the data table.
            var entity = $elm.attr('data-entity');
            // Test
            var chartType = $elm.attr('data-chart-type');
            // State Selector var stand for period of time requested flag ( -1 = no period , 0 = spefcific month)
            $scope.$watch(function () { return $scope.statSelector }, function () {
                
                if (chartType == "bars") {
                    var rawData = [];
                    var header = ['חודש', 'ק"מ מצטבר', "רכיבות"]
                    rawData.push(header);
                    var i = 0;
                    angular.forEach($scope.userStats.personal, function (month) {
                        i++;
                        monthStringed = month.Month.toString().concat('-').concat(month.Year.toString());
                        var monthSum = [monthStringed, month.User_KM, month.Num_of_Rides];
                        rawData.push(monthSum);
                    })
                    var data = google.visualization.arrayToDataTable(rawData);
                    var options = {
                        height: 300,
                        colors: ['blue', 'orange'],
                        legend: { position: 'bottom' },
                        bar: { groupWidth: "95%" },
                        backgroundColor: 'none'
                    };
                    var chart = new google.visualization.BarChart($elm[0]);
                    
                    chart.draw(data, options);
                }


                if (chartType == "pie") {
                    if ($scope.statSelector == -1) {
                        var data = google.visualization.arrayToDataTable($scope.getRanks(entity, -1));
                        $scope.getRanks(entity, -1);
                        }
                    if ($scope.statSelector == 0) {
                        var data = google.visualization.arrayToDataTable($scope.getRanks(entity, 0, $scope.calMonth, $scope.calYear));
                        $scope.getRanks(entity, 0, $scope.calMonth, $scope.calYear);
                    }

                    var options = {
                        is3D: true,
                       // [Future] - Add a user slice : {mySlice : 0.4}
                    };

                    var chart = new google.visualization.PieChart($elm[0]);
                    chart.draw(data, options);

                }
  
            else if (chartType == "gauger") {
                if ($scope.statSelector == -1) {
                    var data = google.visualization.arrayToDataTable($scope.getStats(entity, "calCo2", -1));
                    $scope.getStats(entity, "kmRides", -1);
                    var options = {
                        max:10000,
                        width: 400, height: 240,
                        redFrom: 0, redTo: 750,
                        yellowFrom: 750, yellowTo: 6000,
                        greenFrom: 6000, greenTo: 10000,
                        minorTicks: 0
                    };
                }
                if ($scope.statSelector == 0) {
                    var data = google.visualization.arrayToDataTable($scope.getStats(entity, "calCo2", 0, $scope.calMonth, $scope.calYear));
                    $scope.getStats(entity, "kmRides", 0, $scope.calMonth, $scope.calYear);
                    var options = {
                        max: 10000,
                        width: 400, height: 240,
                        redFrom: 0, redTo: 750,
                        yellowFrom: 750, yellowTo: 6000,
                        greenFrom: 6000, greenTo: 10000,
                        minorTicks: 0
                    };
                }
                // Instantiate and draw our chart, passing in some options.
                var chart = new google.visualization.Gauge($elm[0]);
                chart.draw(data, options);
            }

                
            }, true);
        }
    }
});
//app.directive('chartPersonal', function () {
//    return {
//        restrict: 'A',
//        scope: true,
//        link: function ($scope, $elm, $attr, $watch) {
//            // Create the data table.
//            var entity = $elm.attr('data-entity');
//            var type = $elm.attr('data-my-type');
//            if (type === "pie") {
//                var data = google.visualization.arrayToDataTable([
//                  ['Task', 'Hours per Day'],
//                  ['Work', 11],
//                  ['Eat', 2],
//                  ['Commute', 2],
//                  ['Watch TV', 2],
//                  ['Sleep', 7]
//                ]);
//                var options = {
//                    title: 'My Daily Activities'
//                };
//                // Instantiate and draw our chart, passing in some options.
//                var chart = new google.visualization.PieChart($elm[0]);
//                chart.draw(data, options);
//            }
//            else {
//                $scope.$watch(function () { return $scope.statSelector }, function () {
//                    if ($scope.statSelector == -1) {
//                        var data = google.visualization.arrayToDataTable($scope.getStats(entity, "calCo2", -1));
//                        $scope.getStats(entity, "kmRides", -1);
//                        var options = {
//                            width: 400, height: 240,
//                            redFrom: 90, redTo: 100,
//                            yellowFrom: 75, yellowTo: 90,
//                            minorTicks: 5
//                        };
//                    }
//                    if ($scope.statSelector == 0) {
//                        var data = google.visualization.arrayToDataTable($scope.getStats(entity, "calCo2", 0, $scope.calMonth, $scope.calYear));
//                        $scope.getStats(entity, "kmRides", 0, $scope.calMonth, $scope.calYear);
//                        var options = {
//                            width: 400, height: 240,
//                            redFrom: 90, redTo: 100,
//                            yellowFrom: 75, yellowTo: 90,
//                            minorTicks: 5
//                        };
//                    }
//                    // Instantiate and draw our chart, passing in some options.
//                    var chart = new google.visualization.Gauge($elm[0]);
//                    chart.draw(data, options);
//                }, true);
//            }
//        }
//    }
//});

//app.directive('myPieChart', function () {
//    return {
//        restrict: 'A',
//        scope: true,
//        link: function ($scope, $elm, $attr, $watch) {
//            // Create the data table.

//            var data = google.visualization.arrayToDataTable([
//                  ['Task', 'Hours per Day'],
//                  ['Work',     11],
//                  ['Eat',      2],
//                  ['Commute',  2],
//                  ['Watch TV', 2],
//                  ['Sleep',    7]
//            ]);
//            var options = {
//                title: 'My Daily Activities'
//            };
//            // Instantiate and draw our chart, passing in some options.
//            var chart = new google.visualization.Gauge($elm[0]);
//            chart.draw(data, options);

//        }
//    }
//});
    //var data = new google.visualization.DataTable();
    //data.addColumn('string', 'Topping');
    //data.addColumn('number', 'Slices');
    //data.addRows([
    //  ['Mushrooms', 3],
    //  ['Onions', 1],
    //  ['Olives', 1],
    //  ['Zucchini', 1],
    //  ['Pepperoni', 2]
    //]);

    // Set chart options
    //var options = {
    //    'title': 'How Much Pizza I Ate Last Night',
    //    'width': 400,
    //    'height': 300
//};


//bkup



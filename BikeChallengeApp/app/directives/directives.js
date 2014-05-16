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
               // if ($scope.regDetails.userName.$viewValue == undefined) { }
                //else{
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



// ############################
// Calendar dashboard directive 
// ############################

app.directive('calendar', ['$compile', function ($compile, $watch, $scope, attrs) {

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

        if (!dates || !dates.length) { dates = [currentDate.getDate()] };

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
                    if (parseInt(currentDate.getDate()) == day && parseInt(currentDate.getMonth()) == month) {
                        row.push('<div class="cal-highlight-today" data-rel="popover" ng-click="testPop($event)" id="' + day + '">'); // cal-day class attrb was removed ** 
                        //row.push('<div class="cal-day cal-highlight-today" data-cal="' + year + '/' + month + '/' + day + '">');
                    };
                    if (dates.indexOf(day) != -1) row.push('<div class="cal-day cal-highlight" data-rel="popover" ng-click="testPop($event)" id="' + day + '">');
                    if (dates.indexOf(day) == -1) row.push('<div class="cal-day" data-rel="popover" ng-click="testPop($event)" id="' + day + '">');
                    row.push(day + '</div>');
                    day++;
                }


                //if (day <= monthLength && (i > 0 || j >= startDay)) {
                //    if (parseInt(currentDate.getDate()) == day && parseInt(currentDate.getMonth()) == month) {
                //        row.push('<div class="cal-highlight-today" data-rel="popover" ng-click="testPop($event)" id="' + year + '/' + month + '/' + day + '">'); // cal-day class attrb was removed ** 
                //        //row.push('<div class="cal-day cal-highlight-today" data-cal="' + year + '/' + month + '/' + day + '">');
                //    };
                //    if (dates.indexOf(day) != -1) row.push('<div class="cal-day cal-highlight" data-rel="popover" ng-click="testPop($event)" id="' + year + '/' + month + '/' + day + '">');
                //    if (dates.indexOf(day) == -1) row.push('<div class="cal-day" data-rel="popover" ng-click="testPop($event)" id="' + year + '/' + month + '/' + day + '">');
                //    row.push(day + '</div>');
                //    day++;
                //}



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
            function ($scope, $element, attrs, $watch) {
                $scope.$watch('calMonth', function () {

                    $element.html(getTemplate(parseInt($scope.calMonth)+1, parseInt(attrs.year), $scope.calDates));
                    $compile($element.contents())($scope);
                    console.log("Inside Dir");
                })
            }
    }
}]);



app.directive('customPopover', function () {
    return {
        restrict: 'A',
        template: '<span>{{label}}</span>',
        link: function ($scope, el, attrs) {
            $scope.label = attrs.popoverLabel;

            $(el).popover({
                trigger: 'click',
                html: true,
                content: attrs.popoverHtml,
                placement: attrs.popoverPlacement
            });
        }
    };
});
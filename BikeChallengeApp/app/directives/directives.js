/// <reference path="../Scripts/angular.js" />

// iCheck Directive - manipulating the radio/checkbox to square UI    
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


// Password match validator 

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
